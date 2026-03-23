using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Registry registry;
        
        [SerializeField] private Transform leftLaneHolder;
        [SerializeField] private Transform midLaneHolder;
        [SerializeField] private Transform rightLaneHolder;

        [Header("Runtime Only")]
        [SerializeField] private UnitSequence unitSequence;

        private void Awake()
        {
            Assert.IsNotNull(registry);
            Assert.IsNotNull(leftLaneHolder);
            Assert.IsNotNull(midLaneHolder);
            Assert.IsNotNull(rightLaneHolder);
        }

        public void SpawnObjects(UnitSequence sequence)
        {
            unitSequence = sequence;

            SpawnLane(Lane.Left, leftLaneHolder);
            SpawnLane(Lane.Mid, midLaneHolder);
            SpawnLane(Lane.Right, rightLaneHolder);
        }

        private void SpawnLane(Lane lane, Transform laneHolder)
        {
            if (unitSequence.GetLane(lane, out LaneSequence laneSequence))
            {
                foreach (var data in laneSequence.Units)
                {
                    var unit = Instantiate(data.UnitVariable.GetValue(), laneHolder);
                    unit.SetUnitData(data);
                    unit.ApplyUnitData();
                }
            }
        }
    }
}
