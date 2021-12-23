using Assets.Scripts.Gate;
using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public class CoinsBlockStrategy : BlockStrategy
    {
        // Editor fields
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private GateManager _gatePrefab;




        // Strategy
        protected override CurveBlock ApplyStrategy(CurveBlock curveBlock)
        {
            var coins = CreateCoins(curveBlock, 4);
            var gate = CreateGate(curveBlock);
            gate.Initialize(coins);
            return curveBlock;
        }



        // Block building
        private List<Coin> CreateCoins(CurveBlock block, int totCoins)
        {
            var curve = block.Curve;
            var surface = block.CurveSurface.Surface;
            var coins = new List<Coin>();

            var normalOffset = 0.2f;
            var (nMin, nMax) = (block.CurveSurface.Surface.VMin, block.CurveSurface.Surface.VMax);
            var (left, right) = (Mathf.Lerp(nMin, nMax, normalOffset), Mathf.Lerp(nMax, nMin, normalOffset));
            bool isLeft = true;
            foreach (var t in curve.QuantizedDomain(totCoins, bordersNotIncluded: true))
            {
                // Get local system of axis (i.e. forward = tangent to curve)
                var (tangentVersor, rightVersor, upVersor) = block.Curve.GetLocalBasis(t);                
                var center = block.CurveSurface.GetTopPosition(t, surface.VMiddle);
                var coinPosition = center + (isLeft ? left : right) * rightVersor;

                // Building an oriented gameObject to correctly orientate the coin 
                var orientedWrapper = NewOrientedGameObject(coinPosition, tangentVersor, upVersor);
                orientedWrapper.transform.parent = block.transform;

                // Instantiate the coin inside the wrapper
                var coin = BuildCoin(block, parent: orientedWrapper.transform);
                coins.Add(coin);
                isLeft = !isLeft;
            }

            return coins;
        }


        private GateManager CreateGate(CurveBlock block)
        {
            var curve = block.Curve;

            // Get local system of coordinates (i.e. forward = tangent to curve)
            var (f, n, u) = block.Curve.GetLocalBasis(curve.MaxT);
            var position = block.CurveSurface.GetTopPosition(curve.MaxT, block.CurveSurface.Surface.VMiddle);
            var orientedWrapper = NewOrientedGameObject(position, f, u);
            orientedWrapper.transform.parent = block.transform;
            var gate = BuildGate(block, orientedWrapper.transform);
            return gate;
        }

        private GameObject NewOrientedGameObject(Vector3 position, Vector3 zAxis, Vector3 upAxis)
        {
            var gameObject = new GameObject("Oriented Wrapper");
            var rotation = Quaternion.LookRotation(zAxis, upAxis);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            return gameObject;
        }
        
        private Coin BuildCoin(CurveBlock curve, Transform parent) => Instantiate(_coinPrefab, parent);
        private GateManager BuildGate(CurveBlock curve, Transform parent) => Instantiate(_gatePrefab, parent);
    }
}
