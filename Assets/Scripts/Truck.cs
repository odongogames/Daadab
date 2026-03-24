using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class Truck : MonoBehaviour, IUnitComponent
    {
        public static Truck Instance;
        [SerializeField] private Registry registry;

        [Header("Movement")]
        [SerializeField] private float xSpeed;
        [SerializeField] private float zSpeed;

        [SerializeField] private float moveTimeout = .2f;
        [SerializeField] private Lane lane;

        [Header("Runtime only")]
        [SerializeField] private uint waterTank;
        [SerializeField][Range(-1, 1)] private int direction;
        [SerializeField] private bool disableLaneSwitching;

        private float targetX;
        private float xPosition;
        private float zPosition;
        private float xVelocity;
        private float zSpeedReduced;
        private float zSpeedOriginal;
        private float zSpeedBoosted;
        private float lastMoveTime = 1;
        private Lane previousLane;
        private Transform myTransform;

        public Action<uint> OnAddToWaterTank;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            myTransform = transform;

            Assert.IsNotNull(registry);

            zSpeedOriginal = zSpeed;
            zSpeedReduced = zSpeed / 2;
            zSpeedBoosted = zSpeed * 1.5f;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            targetX = (int)lane * registry.LaneDistance;

            xPosition = Mathf.SmoothDamp(
                current: myTransform.position.x,
                target: targetX,
                currentVelocity: ref xVelocity,
                smoothTime: xSpeed * Time.fixedDeltaTime
            );
            zPosition = Mathf.MoveTowards(
                current: myTransform.position.z,
                target: myTransform.position.z + 1,
                maxDelta: zSpeed * Time.fixedDeltaTime
            );

            myTransform.position = new Vector3(xPosition, 0, zPosition);
        }

        private void GoToPreviousLane()
        {
            direction = (int)previousLane - (int)lane;
            SwitchLane(direction);
        }

        private void SwitchLane(int newDirection)
        {
            if (newDirection != 0)
            {
                direction = newDirection;
                lastMoveTime = Time.time;
                previousLane = lane;
            }

            // cast Lane to int and apply direction
            var newLane = Mathf.Clamp((int)lane + direction, -1, 1);

            lane = (Lane) newLane;
        }

        public void SetXDirection(int newDirection)
        {
            if (disableLaneSwitching)
            {
                Debug.Log($"Lane switching disabled");
                return;
            }
            
            if (Time.time > lastMoveTime + moveTimeout)
            {
                SwitchLane(newDirection);
            }
        }

        public void AddToWaterTank()
        {
            waterTank++;
            Debug.Log($"Add to watertank: {waterTank}");
            OnAddToWaterTank?.Invoke(waterTank);
        }

        public void EnterActiveState()
        {
            enabled = true;
        }

        public void ExitActiveState()
        {
            enabled = false;
        }

        public void ResetMe()
        {
        }

        public void ReduceSpeed()
        {
            zSpeed = zSpeedReduced;

            disableLaneSwitching = true;
        }

        public void RestoreSpeed()
        {
            zSpeed = zSpeedOriginal;

            disableLaneSwitching = false;
        }

        public void BoostSpeed()
        {
            disableLaneSwitching = false;

            zSpeed = zSpeedBoosted;
        }
    }
}
