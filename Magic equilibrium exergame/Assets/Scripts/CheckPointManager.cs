using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    private List<CheckPoint> checkPoints;

    private int lastCheckpoint;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        lastCheckpoint = 0;
    }

    // When a checkpoint is reached, check whether that is the furthest the player has reached so far
    // If it is, update the index
    public void NewCheckpoint(CheckPoint checkPoint)
    {
        int tempIndex = checkPoints.IndexOf(checkPoint);

        if(lastCheckpoint > tempIndex)
        {
            lastCheckpoint = tempIndex;
        }
    }    
}
