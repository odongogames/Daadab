using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class LaneSwitcher : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float xSpeed;
        [SerializeField] private float zSpeed;

        [SerializeField] private Vector3 velocity;
        [SerializeField] private float moveTimeout = .2f;
        [SerializeField] private Lane lane;
        /// <summary>
        /// Distance between each lane on the x-axis
        /// </summary>
        [SerializeField] [Range(0, 4)] private int xMoveAmount = 2;
        
        [Header("Runtime only")]
        [SerializeField][Range(-1, 1)] private int direction;

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
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            // if (!isMoving) return;

            targetX = (int)lane * xMoveAmount;
            // xPosition = Mathf.MoveTowards(
            //     current:  myTransform.position.x,
            //     target: targetX,
            //     maxDelta: xSpeed * Time.fixedDeltaTime
            // );
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
            // xPosition = Mathf.Lerp(
            //     a:  myTransform.position.x,
            //     b: targetX,
            //     t: xSpeed * Time.fixedDeltaTime
            // );
            // xPosition = Mathf.Lerp(a: xPosition, b: targetX, t: xSpeed * Time.fixedDeltaTime);

            isMovingToSide = Mathf.Abs(targetX - myTransform.position.x) > 0.1f;

            myTransform.position = new Vector3(xPosition, 0, zPosition);
        }

        private void GoToPreviousLane()
        {
            direction = (int)previousLane - (int)lane;
            // Debug.Log(direction.x);
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
                    // Debug.Log("Move from MID to LEFT");
                }
                else if (lane == Lane.Right)
                {
                    lane = Lane.Mid;
                    // Debug.Log("Move from RIGHT to MIX");
                }
            }
            else if (direction > 0)
            {
                if (lane == Lane.Mid)
                {
                    lane = Lane.Right;
                    // Debug.Log("Move from MID to RIGHT");
                }
                else if (lane == Lane.Left)
                {
                    lane = Lane.Mid;
                    // Debug.Log("Move from LEFT to MIX");
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
    }
}
