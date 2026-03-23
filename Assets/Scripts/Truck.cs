using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class Truck : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private Registry registry;

        [Header("Movement")]
        [SerializeField] private float xSpeed;
        [SerializeField] private float zSpeed;

        [SerializeField] private Vector3 velocity;
        [SerializeField] private float moveTimeout = .2f;
        [SerializeField] private Lane lane;
        
        [Header("Runtime only")]
        [SerializeField] [Range(-1, 1)] private int direction;

        private float targetX;
        private float xPosition;
        private float zPosition;
        private float xVelocity;
        private float lastMoveTime = 1;
        private bool isMovingToSide;
        private Lane previousLane;
        private Transform myTransform;


        public bool IsMovingToSide() => isMovingToSide;

        private void Awake()
        {
            myTransform = transform;

            Assert.IsNotNull(registry);
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
                current:  myTransform.position.z,
                target: myTransform.position.z + 1,
                maxDelta: zSpeed * Time.fixedDeltaTime
            );

            isMovingToSide = Mathf.Abs(targetX - myTransform.position.x) > 0.1f;

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

            if (direction < 0)
            {
                if (lane == Lane.Mid)
                {
                    lane = Lane.Left;
                }
                else if (lane == Lane.Right)
                {
                    lane = Lane.Mid;
                }
            }
            else if (direction > 0)
            {
                if (lane == Lane.Mid)
                {
                    lane = Lane.Right;
                }
                else if (lane == Lane.Left)
                {
                    lane = Lane.Mid;
                }
            }
        }

        public void SetXDirection(int newDirection, bool force = false)
        {
            if (!force && Time.time > lastMoveTime + moveTimeout)
            {
                SwitchLane(newDirection);
            }
        }

        private void StopMoving()
        {
            velocity = Vector2.zero;
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
    }
}
