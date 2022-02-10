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


const char* ssid     = "MAGIKA";
const char* password = "!magika!";

// Set web server port number to 80
WiFiServer server(80);

// Auxiliar variables to store the current output state
String output2State = "off";
String output1State = "off";
String outputDATAState = "off";
String object = "balance board";
String status = "not working";
String IP;
String UdpIp;
int UdpPort;
bool _udpInitialized = false;

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


// declarations
void getAccelerometerData(float * x, float * y, float * z);


// Utils
void UpdateUdpIpAndPort(String message){
  StaticJsonDocument<1000> doc;
  deserializeJson(doc, message);
  String ip = doc["ipTarget"];
  int port = doc["portTarget"];
  UdpIp = ip;
  UdpPort = port;
  Serial.println(ip);
  Serial.println(port);
}

void UpdateUdpIpAndPort(DynamicJsonDocument doc){
  String ip = doc["ipTarget"];
  int port = doc["portTarget"];
  UdpIp = ip;
  UdpPort = port;
  Serial.println(ip);
  Serial.println(port);
}


// REST
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
     JsonArray availableCredentials = doc.createNestedArray("availableCredentials");
     availableCredentials.add("I3Lab");     
     return doc;
}


DynamicJsonDocument GetDeviceStateJson(){
   DynamicJsonDocument doc(1024);
   doc["battery"] = 0;
   doc["freeHeap"] = 15184;
   doc["uptime"] = 423;
   doc["stateChannel"] = GetStateChannelJson();
   doc["eventChannel"] = GetEventChannel();
   doc["network"] = GetNetwork();

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
  doc["id"] = "1736107";
  doc["productId"] = "1736107_wobbleboard_product_id";
  doc["deviceModel"] = "Wobbleboard";
  doc["mdnsService"] = "smartobject";
  doc["mdnsAddress"] = "wobbleboard_5007";
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
  // JsonArray rfids = doc.createNestedArray("rfids");
  // rfids.add(0);
  return doc;
}

DynamicJsonDocument BuildRfidsJson()
{
  DynamicJsonDocument rfids(50);
  // rfids["rifds"] = "0";
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


// Rest accelertometer data
DynamicJsonDocument GetAccelerometerDataAsJson(){
  float x, y, z;
  getAccelerometerData(&x, &y, &z);

  DynamicJsonDocument doc(100);
  doc["x"] = x;
  doc["y"] = y;
  doc["z"] = z;
  return doc;
}


// UDP
DynamicJsonDocument GetUdpGyroscopeJson(){
  DynamicJsonDocument doc(100);
  doc["x"] = 0;
  doc["y"] = 0;
  doc["z"] = 0;
  return doc;
}


DynamicJsonDocument GetUdpPositionJson(){
  DynamicJsonDocument doc(100);
  doc["x"] = 0;
  doc["y"] = 0;
  doc["z"] = 0;
  return doc;
}


DynamicJsonDocument GetUdpKinematicsJson(){
  DynamicJsonDocument doc(300);
  doc["id"] = "id";
  doc["accelerometer"] = GetAccelerometerDataAsJson();
  doc["gyroscope"] =GetUdpGyroscopeJson();
  doc["position"] =GetUdpGyroscopeJson();
  return doc;
}

DynamicJsonDocument GetUdpSensorsJson(){
  DynamicJsonDocument doc(300);
  doc["Kinematics"] = GetUdpKinematicsJson();
  return doc;
}

DynamicJsonDocument GetAccelerometerDataAsJsonInMagicRoom(){
  float x, y, z;
  getAccelerometerData(&x, &y, &z);

  DynamicJsonDocument doc(1000);
  doc["id"] = "id",
  doc["Sensors"] = GetUdpSensorsJson();
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


DynamicJsonDocument getResponse(String requestType){
  if(requestType.indexOf("getState") >= 0)
    return GetStateJson();
  
  if(requestType.indexOf("getInformation") >= 0)
    return GetSystemInfo();    

  if(requestType.indexOf("getCapabilities") >= 0)
    return GetCapabilitiesJson();   
  
  return GetAccelerometerDataAsJsonInMagicRoom();
}




String readHeader(WiFiClient client){
  String header;
  while(client.available())
    header += (char) client.read();
  return header;
}



// REST Routing
void restServerRouting(){
    httpRestServer.on("/", HTTP_POST, []() {
        String header = httpRestServer.arg("plain");
        Serial.println(header);
        String requestType = getRequestType(header);
        DynamicJsonDocument json = getResponse(requestType);
        String response;
        serializeJson(json, response);
        httpRestServer.send(200, "text/json", response.c_str());
      });

       httpRestServer.on("/setStream", HTTP_POST, []() {
        String header = httpRestServer.arg("plain");
        // String requestType = getRequestType(header);
        // DynamicJsonDocument doc = getResponse(requestType);
        UpdateUdpIpAndPort(header);
        _udpInitialized = true;
        httpRestServer.send(200, "text/json", "ok");
      });
  }


  // Accelerometer
  void printAccelerometerData(){
    
  /* Get new sensor events with the readings */
  sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);

  /* Print out the values */
  Serial.print("Acceleration X: ");
  Serial.print(a.acceleration.x);
  Serial.print(", Y: ");
  Serial.print(a.acceleration.y);
  Serial.print(", Z: ");
  Serial.print(a.acceleration.z);
  Serial.println(" m/s^2");

  Serial.print("Rotation X: ");
  Serial.print(g.gyro.x);
  Serial.print(", Y: ");
  Serial.print(g.gyro.y);
  Serial.print(", Z: ");
  Serial.print(g.gyro.z);
  Serial.println(" rad/s");

  Serial.print("Temperature: ");
  Serial.print(temp.temperature);
  Serial.println(" degC");

  Serial.println("");

  Serial.println("Horizontal Angle");
  Serial.println(atan(- a.acceleration.x / a.acceleration.z) * 180 / 3.14);


  Serial.println("");

  Serial.println("Vertical Angle");
  Serial.println(atan(- a.acceleration.y / a.acceleration.z) * 180 / 3.14);

  delay(500);
  }

  // void getAccelerometerAngles(float * xAngle, float * zAngle){
     
  //    *xAngle = atan(- a.acceleration.x / a.acceleration.z);
  //    *zAngle = atan(a.acceleration.y / a.acceleration.z);
  // }

  void getAccelerometerData(float * x, float * y, float * z){
     sensors_event_t a, g, temp;
     mpu.getEvent(&a, &g, &temp);
     *x = a.acceleration.x;
     *y = a.acceleration.y;
     *z = a.acceleration.z;
  }



//UDP
void udpSendPacketTo(IPAddress ip, uint16_t port, const char * data)
{
  // send a reply, to the IP address and port that sent us the packet we received
    Udp.beginPacket(ip, port);
    Udp.write(data);
    Udp.endPacket();
}

void udpSendUdpPacket(char* data){
  udpSendPacketTo(Udp.remoteIP(), Udp.remotePort(), data);
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

    udpSendUdpPacket(result); 
  }
}

void SendAccelerometerDataThroughUdp(){
  IPAddress ip;
  ip.fromString("192.168.1.12");
  DynamicJsonDocument data = GetAccelerometerDataAsJson();
  String json;
  serializeJson(data, json);
  udpSendPacketTo(ip, 8000, json.c_str());
}


void SendAccelerometerDataThroughUdpInMagicRoom(){
  IPAddress ip;
  ip.fromString(UdpIp.c_str());
  DynamicJsonDocument data = GetAccelerometerDataAsJsonInMagicRoom();
  String json;
  serializeJson(data, json);
  udpSendPacketTo(ip, UdpPort, json.c_str());
}





// Initialization
void connectToWifi(){
  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to A ");
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
  Serial.println("\nMPU6050 Found!");
  leds[0] = CRGB::Blue;
  leds[1] = CRGB::Green;
}


void initializeUdp() {
  Udp.begin(localPort);  
  Serial.printf("UDP server on port %d\n", localPort);
}


void initializeMpu(){
  mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
  mpu.setGyroRange(MPU6050_RANGE_500_DEG);
  mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);
}


void TestUdpIpAndPortUpdater(){
  DynamicJsonDocument doc(200);
  doc["ipTarget"] = "192.168.1.22";
  doc["portTarget"] = 9999;
  String text;
  serializeJson(doc, text);
  UpdateUdpIpAndPort(text);
}
void PrintAccelerometerData() {
 String response;
  serializeJson(GetAccelerometerDataAsJsonInMagicRoom(), response);
  Serial.println(response);
}

// Arduino Setup
void setup() {
  Serial.begin(115200);

  PrintAccelerometerData();
  TestUdpIpAndPortUpdater();

  initializeMPU();
  connectToWifi();
  initializeUdp();
  initializeMpu();

  FastLED.addLeds<WS2812B, DATA_PIN, RGB>(leds, NUM_LEDS);
  FastLED.setBrightness(50);
  
  if(MDNS.begin("myesp")){
    status = "ok";
  };

  restServerRouting();
  httpRestServer.begin();
}


void loop(){

 
  MDNS.update();
  httpRestServer.handleClient();
  // printAccelerometerData();  
  SendAccelerometerDataThroughUdp();
  if(_udpInitialized)
    SendAccelerometerDataThroughUdpInMagicRoom();
}

