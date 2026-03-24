using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

namespace Daadab
{
    public class PooledObjectSequence : MonoBehaviour
    {
        [SerializeField] private Transform leftLaneHolder;
        public Transform LeftLaneHolder => leftLaneHolder;
        [SerializeField] private Transform midLaneHolder;
        public Transform MidLaneHolder => midLaneHolder;
        [SerializeField] private Transform rightLaneHolder;
        public Transform RightLaneHolder => rightLaneHolder;


        private void OnValidate()
        {
            Assert.IsNotNull(leftLaneHolder);
            Assert.IsNotNull(midLaneHolder);
            Assert.IsNotNull(rightLaneHolder);
        }
    }
}