﻿using Assets.Scripts.Gate;
using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.BuildingStrategies
{
    public class CoinsPathStrategy : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private GateManager _gatePrefab;



        // Strategy
        public void CoinsPath(CurveBlock block)
        {
            var curve = block.Curve;
            var surface = block.CurveSurface.Surface;
            var totCoins = 4;
            var coins = new List<Coin>();

            var normalOffset = 0.2f;
            var (nMin, nMax) = (block.CurveSurface.Surface.VMin, block.CurveSurface.Surface.VMax);
            var (left, right) = (Mathf.Lerp(nMin, nMax, normalOffset), Mathf.Lerp(nMax, nMin, normalOffset));
            bool isLeft = true;
            foreach(var t in curve.QuantizedDomain(totCoins, bordersNotIncluded: true))
            {
                var (_, normalVersor) = block.Curve.GetLocalBasis(t);
                var center = block.CurveSurface.GetTopPosition(t, surface.VMiddle, topOffset: _coinPrefab.transform.localScale.y);
                var coinPosition = center + (isLeft ? left : right) * normalVersor;
                var coin = BuildCoin(block, coinPosition);
                coins.Add(coin);
                isLeft = !isLeft;
            }

            var position = block.CurveSurface.GetTopPosition(curve.MaxT, surface.VMiddle, topOffset: 2);
            var gate = BuildGate(block, position);
            gate.Initialize(coins);
        }



        // Private
        private Coin BuildCoin(CurveBlock curve, Vector3 position)
        {
            var coin = Instantiate(_coinPrefab, curve.transform);
            coin.transform.position = position;
            return coin;
        }

        private GateManager BuildGate(CurveBlock curve, Vector3 position)
        {
            var gate = Instantiate(_gatePrefab, curve.transform);
            gate.transform.position = position;
            return gate;
        }
    }
}
