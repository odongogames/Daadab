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

        [Header("Movement")]
        [SerializeField] private float xSpeed;
        [SerializeField] private float zSpeed;

        [SerializeField] private float moveTimeout = .2f;
        [SerializeField] private Lane lane;

        [Header("Runtime only")]
        [SerializeField][Range(-1, 1)] private int direction;
        [SerializeField] private bool disableLaneSwitching;

        private float targetX;
        private float xPosition;
        private float zPosition;
        private float xVelocity;
        private float zSpeedReduced;
        private float zSpeedOriginal;
        private float zSpeedBoosted;
        private float lastMoveTime = 0;
        private Vector3 originalPosition;
        private Lane previousLane;

        private Transform myTransform;
        private SpeedBooster booster;
        private WaterTank waterTank;
        private SFXPlayer SFXPlayer;
        private Registry registry;


        public bool IsBoosting() => booster.IsBoosting();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            myTransform = transform;
            originalPosition = myTransform.position;

            Assert.IsNotNull(registry);

            booster = GetComponent<SpeedBooster>();
            Assert.IsNotNull(booster);

            waterTank = GetComponent<WaterTank>();
            Assert.IsNotNull(waterTank);

            zSpeedOriginal = zSpeed;
            zSpeedReduced = zSpeed / 2;
            zSpeedBoosted = zSpeed * 2f;
        }

        private void Start()
        {
            SFXPlayer = SFXPlayer.Instance;
            Assert.IsNotNull(SFXPlayer);

            enabled = false;
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

        public void EnterActiveState()
        {
            enabled = true;
            Debug.Log("Enable truck");
        }

        public void ExitActiveState()
        {
            enabled = false;
            Debug.Log("Disable truck");

        }

        public void ResetMe()
        {
            disableLaneSwitching = false;
            RestoreSpeed();
            lane = Lane.Mid;
            myTransform.position = originalPosition;
        }

        public void ReduceSpeed()
        {
            ChangeZSpeed(zSpeedReduced);

            disableLaneSwitching = true;
        }

        public void RestoreSpeed()
        {
            ChangeZSpeed(zSpeedOriginal);

            disableLaneSwitching = false;
        }

        public void StartBoost()
        {
            booster.StartBoost();

            disableLaneSwitching = false;

            ChangeZSpeed(zSpeedBoosted);                
        }

        private void ChangeZSpeed(float newSpeed)
        {
            zSpeed = newSpeed;

            Debug.Log($"Set truck Z speed: {zSpeed}");
        }

        public void AddToWaterTank()
        {
            waterTank.AddToWaterTank();
        }
    }
}
