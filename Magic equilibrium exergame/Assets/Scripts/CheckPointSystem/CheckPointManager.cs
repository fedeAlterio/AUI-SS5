using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    public CheckPoint[] checkPoints;

    public List<CheckPoint> orderedCheckPoints = new List<CheckPoint>();

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
        checkPoints = FindObjectsOfType<CheckPoint>();

        orderedCheckPoints = checkPoints.OrderBy(x=> x.iD).ToList();

        lastCheckpoint = 0;
    }

    // When a checkpoint is reached, check whether that is the furthest the player has reached so far
    // If it is, update the index
    public void NewCheckpoint(CheckPoint checkPoint)
    {
        int tempIndex = orderedCheckPoints.IndexOf(checkPoint);

        if(lastCheckpoint > tempIndex)
        {
            lastCheckpoint = tempIndex;
        }
    }   

    // Tells where to respawn the player
    public Vector3 GetRespawn()
    {
        return checkPoints[lastCheckpoint].checkPointPosition;
    } 
}
