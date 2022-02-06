using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.WobbleBoardCalibration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Communication.Abstractions
{
    public abstract class WobbleboardFetcher : MonoBehaviour, IWobbleboardDataProvider
    {
        // Editor fields
        [SerializeField] private bool _enableLogging;
        
            
        // Private fields
        private Thread _fetchingThread;
        private IWobbleBoardConfiguration _wobbleBoardConfiguration;


        // Initialization
        private void Start()
        {
            _fetchingThread = new Thread(FetchDataLoop);
            _wobbleBoardConfiguration = this.GetInstance<IWobbleBoardConfiguration>();
            IsApplicationRunning = true;
            _fetchingThread.Start();
        }



        // Properties
        public float XAngle { get; private set; }
        public float ZAngle { get; private set; }
        private bool IsApplicationRunning { get; set; }



        // Core
        protected virtual void OnDestroy()
        {
            IsApplicationRunning = false;
        }

        private async void FetchDataLoop()
        {
            while (IsApplicationRunning)
            {
                try
                {
                    Log("Try to get info");
                    var responseSchema = new { X = 0f, Y = 0f, Z = 0f };
                    var (isTimeout, response) = await Get(responseSchema: responseSchema)
                        .TimeoutWithoutException(TimeSpan.FromSeconds(10));
                    if (!isTimeout)
                    {
                        var localCoordinates = new Vector3(response.X, response.Y, response.Z);
                        AccelerometerCoordinatesToAngles(localCoordinates, out var horizontalAngle, out var forwardAngle);
                        //(XAngle, ZAngle) = (horizontalAngle, forwardAngle);
                        SetOnMainThread(horizontalAngle, forwardAngle).Forget();                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void AccelerometerCoordinatesToAngles(Vector3 gravityLocal, out float horizontalAngle, out float forwardAngle)
        {
            gravityLocal = new Vector3(gravityLocal.z, -gravityLocal.x, -gravityLocal.y);
            gravityLocal = gravityLocal.normalized;
            var theta = _wobbleBoardConfiguration.HorizontalRotationAngle;
            // Accelertometer axis versors: Ax, Ay, Az
            // Movement aligned axis: Vx = - Az * cos(theta) + Ay * sin(theta), Vy = -sin(theta) * Az - cos(theta) * Ay, Vz = Ax
            // World aligned axis: ex = Vx * cos(alpha) + Vz * sin(alpha), ey = Vy * cos(beta) + Vz * sin(beta), ez = -Vz
            // gravity = gravityLocal.x * Ax + gravityLocal.y * Ay + gravityLocal.z* Az = -ez
            // Find alpha, beta

            var (cosTheta, sinTheta) = (Mathf.Cos(theta), Mathf.Sin(theta));

            // cVx = <lxAx + lyAy + lzAz, Vx> = <lxAx + lyAy + lzAz, -Azcos(theta) + Aysin(theta)>
            //     = <(lx,ly,lz), (0, sin(theta), cos(theta))>
            var cVx = Vector3.Dot(gravityLocal, new Vector3(0, sinTheta, cosTheta));

            // cVy = <lxAx + lyAy + lzAz, Vy> = <lxAx + lyAy + lzAz, -Azsin(theta) - Aycos(theta)>
            //     = <(lx,ly,lz), (0, -cos(theta), -sin(theta))>
            var cVy = Vector3.Dot(gravityLocal, new Vector3(0, -cosTheta, sinTheta));
            var cVz = gravityLocal.x;

            // gravity = cVx*Vx + cVy*Vy + cVz*Vz;
            // <gravity, ex> = <(cVx, CVy + cVz), (cos(alpha),0,sin(alpha))> = cVx*cos(alpha) + cVz*sin(alpha) = 0
            // <gravity, ey> = <(cVx,cVy,cVz), (0, cos(beta), sin(beta))> = cVy*cos(beta) + cVz * sin(beta) = 0
            // --> tan(alpha) = -cVx / cVz
            // --> tan(beta) = -cVy / cVz
            // We know that both alpha and beta are between [-pi/2, pi/2]
            horizontalAngle = Mathf.Atan(cVx / cVz);
            forwardAngle = Mathf.Atan(-cVy / cVz);
        }

        protected abstract UniTask<T> Get<T>(T responseSchema = default);
        private async UniTaskVoid SetOnMainThread(float xAngle, float zAngle)
        {
            await UniTask.SwitchToMainThread();
            if (float.IsNaN(xAngle))
                xAngle = 0;
            if(float.IsNaN(zAngle)) 
                zAngle = 0;
            (XAngle, ZAngle) = (xAngle, zAngle);
            Log((XAngle, ZAngle));
        }


        private void Log(object obj)
        {
            if (_enableLogging)
                Debug.Log(obj);
        }
    }
}
