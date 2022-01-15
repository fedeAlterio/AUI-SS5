// Load Wi-Fi library
#include <ESP8266WiFi.h>
#include <FastLED.h>
#include <ESP8266mDNS.h>
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
#include <WiFiUdp.h>
#include "ESP8266WebServer.h"
#include "ArduinoJson.h"
#define NUM_LEDS 2
#define DATA_PIN 12
#define HTTP_REST_PORT 80

CRGB leds[NUM_LEDS];
Adafruit_MPU6050 mpu;

// Replace with your network credentials
const char* ssid     = "ssid";
const char* password = "pass";

// Set web server port number to 80
WiFiServer server(80);

// Auxiliar variables to store the current output state
String output2State = "off";
String output1State = "off";
String outputDATAState = "off";
String object = "balance board";
String status = "not working";
String IP;

char result[20];

// Assign output variables to GPIO pins
const int output2 = 5;
const int output1 = 4;

// Current time
unsigned long currentTime = millis();
// Previous time
unsigned long previousTime = 0; 
// Define timeout time in milliseconds (example: 2000ms = 2s)
const long timeoutTime = 2000;
unsigned int localPort = 8888;

char packetBuffer[UDP_TX_PACKET_MAX_SIZE + 1]; //buffer to hold incoming packet,
char  ReplyBuffer[] = "come stai?\r\n";       // a string to send back

WiFiUDP Udp;
ESP8266WebServer httpRestServer(HTTP_REST_PORT);

void udpSendUdpPacket(WiFiUDP* Udp,char* data){
  // send a reply, to the IP address and port that sent us the packet we received
    Udp->beginPacket(Udp->remoteIP(), Udp->remotePort());
    Udp->write(data);
    Udp->endPacket();
}




DynamicJsonDocument GetStateChannelJson(){
   DynamicJsonDocument doc(124);
   doc["url"] = "192.168.0.24:57005";
   doc["frequency"] = 0;
   return doc;
}

DynamicJsonDocument GetEventChannel(){
   DynamicJsonDocument doc(50);
   doc["url"] = "0.0.0.0:0";
   return doc;
}


DynamicJsonDocument GetNetwork(){
     DynamicJsonDocument doc(1024);
     doc["ip"] = "192.168.0.38";
     doc["ssid"] = "NeRV";     
     return doc;
}

DynamicJsonDocument GetDeviceStateJson(){
   DynamicJsonDocument doc(1024);
   doc["battery"] = 0;
   doc["freeHeap"] = 15184;
   doc["uptime"] = 423;
   doc["stateChannel"] = GetStateChannelJson();
   doc["eventChannel"] = GetEventChannel();
   JsonArray availableCredentials = doc.createNestedArray("availableCredentials");
   availableCredentials.add("I3Lab");
   return doc;
}

DynamicJsonDocument GetSoftwareVersion()
{
  DynamicJsonDocument softwareVersion(299);
  softwareVersion["smartifier"] = "0.3.0-74-g6393940-dirty";
  softwareVersion["compiledAt"] = "Jun 11 2020 12:56:06";
  softwareVersion["espSdk"] = "2.2.2-dev(38a443e)";
  softwareVersion["arduinoCore"] = "2_6_3";
  return softwareVersion;
}


DynamicJsonDocument GetInformationJson(){
  DynamicJsonDocument doc(1024);
  doc["id"] = "1935007";
  doc["productId"] = "123456_smartobject_ABCDEF";
  doc["deviceModel"] = "Smile O Meter";
  doc["mdnsService"] = "smartobject";
  doc["mdnsAddress"] = "smileometer_5007";
  doc["softwareVersion"] = GetSoftwareVersion();
  return doc;
}




DynamicJsonDocument GetSystemInfo(){
  DynamicJsonDocument doc(1024);
  doc["deviceState"] = GetDeviceStateJson();
  doc["information"] = GetInformationJson();
  return doc;  
}





// GetState

DynamicJsonDocument BuildLightJson()
{
  DynamicJsonDocument light(299);
  light["id"] = "l1";  
  light["color"] = "aaaaaa";
  return light;
}


DynamicJsonDocument GetStateJson(){
  DynamicJsonDocument doc(1024);
  doc["responseType"] = "state";
  JsonArray lights = doc.createNestedArray("lights");
  DynamicJsonDocument light1 = BuildLightJson();
  DynamicJsonDocument light2 = BuildLightJson();
  lights.add(light1);
  lights.add(light2);
  return doc;
}


// Get Capabilities
DynamicJsonDocument GetSensorsJson(){
  DynamicJsonDocument doc(124);
  JsonArray rfids = doc.createNestedArray("rfids");
  rfids.add(0);
  return doc;
}

DynamicJsonDocument BuildRfidsJson()
{
  DynamicJsonDocument rfids(50);
  rfids["rifds"] = "0";
  return rfids;
}

DynamicJsonDocument GetActuators(){
  DynamicJsonDocument doc(300);
  JsonArray lights = doc.createNestedArray("lights");
  lights.add("l1");
  lights.add("l2");
  return doc;
}


DynamicJsonDocument GetProperties()
{
  DynamicJsonDocument properties(1024);
  DynamicJsonDocument id(1024);
  JsonArray type = id.createNestedArray("type");
  type.add("string");
  type.add("null");
  properties["id"] = id;
  DynamicJsonDocument color(1024);
  color["type"] = "string";
  properties["color"] = color;
  DynamicJsonDocument brightness(1024);
  brightness["type"] = "integer";
  properties["brightness"] = brightness;
  return properties;
}

DynamicJsonDocument GetSchema()
{
  DynamicJsonDocument schema(1024);
  schema["type"] = "object";
  schema["properties"] = GetProperties();
  return schema;
}

DynamicJsonDocument GetFirstCommand()
{
  DynamicJsonDocument doc(1024);
  doc["name"] = "light";
  doc["schema"] = GetSchema();
  return doc;
}


DynamicJsonDocument GetCapabilitiesJson()
{
  DynamicJsonDocument doc(1024);

  doc["id"] = "1234567";
  doc["name"] = "WobbleBoard";
  doc["sensors"] = GetSensorsJson();
  doc["actuators"] = GetActuators();
  JsonArray commands = doc.createNestedArray("commands");
  commands.add(GetFirstCommand());

  return doc;
}


String getRequestType(String header){
    String requestType = "requestType";
    unsigned int index = header.indexOf(requestType) + requestType.length() + 3;         
    int start = header.indexOf("\"", index);
    int end = header.indexOf("\"", start + 1);
    requestType = header.substring(start + 1, end); 
    return requestType;
}


void getState(){
  httpRestServer.send(200, "text/json", "{\"name\": \"Hello world\"}");
}

DynamicJsonDocument getResponse(String requestType){
  if(requestType.indexOf("getState") >= 0)
    return GetStateJson();
  
  if(requestType.indexOf("getInformation") >= 0)
    return GetSystemInfo();    

  if(requestType.indexOf("getCapabilities") >= 0)
    return GetCapabilitiesJson();   
    
  return DynamicJsonDocument(1);
}



void restServerRouting(){
    httpRestServer.on("/", HTTP_POST, []() {
        String header = httpRestServer.arg("plain");
        String requestType = getRequestType(header);
        DynamicJsonDocument json = getResponse(requestType);
        String response;
        serializeJson(json, response);
        httpRestServer.send(200, "text/json", response.c_str());
      });
  }


void connectToWifi(){
  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  IP = WiFi.localIP().toString();
}

void initializeMPU(){
  if (!mpu.begin()) {
      Serial.println("Failed to find MPU6050 chip");
      while (1) {
        delay(10);
      }
    }
  Serial.println("MPU6050 Found!");
}




String readHeader(WiFiClient client){
  String header;
  while(client.available())
    header += (char) client.read();
  return header;
}


void sendUdp(){
  int packetSize = Udp.parsePacket();
  if (packetSize) {
    Serial.printf("Received packet of size %d from %s:%d\n    (to %s:%d, free heap = %d B)\n",
                  packetSize,
                  Udp.remoteIP().toString().c_str(), Udp.remotePort(),
                  Udp.destinationIP().toString().c_str(), Udp.localPort(),
                  ESP.getFreeHeap());

    // read the packet into packetBufffer
    int n = Udp.read(packetBuffer, UDP_TX_PACKET_MAX_SIZE);
    packetBuffer[n] = 0;
    Serial.println("Contents:");
    Serial.println(packetBuffer);

    udpSendUdpPacket(&Udp,result); 
  }
}

void initializeUdp(){
  Udp.begin(localPort);  
  Serial.printf("UDP server on port %d\n", localPort);
}


void initializeAccelerometer(){
  mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
}


//
//
// Arduino main 
//
//



void setup() {
  Serial.begin(115200);

  FastLED.addLeds<NEOPIXEL, DATA_PIN>(leds, NUM_LEDS);
  
  initializeMPU();
  connectToWifi();
  initializeUdp();
  

  
  
  if(MDNS.begin("myesp")){
    status = "ok";
  };

  restServerRouting();
  httpRestServer.begin();
}


void loop(){

  MDNS.update();
  sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);

  dtostrf(g.gyro.x, 2, 5, result);
  httpRestServer.handleClient();
}

