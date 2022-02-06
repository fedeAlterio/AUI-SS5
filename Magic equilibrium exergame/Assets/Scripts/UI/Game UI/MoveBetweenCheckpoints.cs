using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Game_UI
{
    public class MoveBetweenCheckpoints : MonoBehaviour
    {
        // Private fields
        private RespawnManager _respawnManager;



        // Initialization
        private void Awake()
        {
            _respawnManager = FindObjectOfType<RespawnManager>();
        }



        // Core
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
                MoveNextCheckpoint();
            else if(Input.GetKeyDown(KeyCode.Alpha1))   
                MovePreviousCheckpoint();
        }


        private void MoveNextCheckpoint()
        {
            var checkpointManager = CheckPointManager.instance;
            var currentCheckpointNumber = checkpointManager.LastCheckpoint;
            if (currentCheckpointNumber == checkpointManager.CheckPoints.Count - 1)
                return;

            var nextCheckpoint = checkpointManager.CheckPoints[currentCheckpointNumber + 1];
            _respawnManager.MovePlayerToCheckpoint(nextCheckpoint);
        }

        private void MovePreviousCheckpoint()
        {
            var checkpointManager = CheckPointManager.instance;
            var currentCheckpointNumber = checkpointManager.LastCheckpoint;
            if (currentCheckpointNumber == 0)
                return;

            var nextCheckpoint = checkpointManager.CheckPoints[currentCheckpointNumber - 1];
            _respawnManager.MovePlayerToCheckpoint(nextCheckpoint);
        }
    }
}
