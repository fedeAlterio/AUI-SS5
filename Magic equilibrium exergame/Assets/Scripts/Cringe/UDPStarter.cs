using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPStarter : MonoBehaviour
{

    public string objectName;
    public bool udpStarted;

    // Start is called before the first frame update
    void Start()
    {

        var g = MagicRoomManager.instance;

        if(g != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!udpStarted)
            TrySubscribeUdp();
    }


    private void TrySubscribeUdp()
    {        
        var toyManager = MagicRoomManager.instance.MagicRoomSmartToyManager;
        var obj = toyManager.GetSmartToyByName(objectName);
        if(obj == null)
            return;

        Debug.Log("non arriveremo mai qui");
        var smartToy = obj.GetComponent<SmartToy>();        
        toyManager.SubscribeEvent(EventType.UDP, smartToy.state.Id);
        udpStarted = true;
    }
}
