using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

namespace Daadab
{
    /// <summary>
    /// EDITOR ONLY:
    /// Build a sequence of unit data for all three lanes and save to a Unit Sequence object
    /// </summary>
    public class UnitSequenceBuilder : MonoBehaviour
    {
        [SerializeField] private UnitSequence sequence;
        [SerializeField] private Transform leftLaneHolder;
        [SerializeField] private Transform midLaneHolder;
        [SerializeField] private Transform rightLaneHolder;

        private void Awake()
        {
            Assert.IsNotNull(sequence);
            Assert.IsNotNull(leftLaneHolder);
            Assert.IsNotNull(midLaneHolder);
            Assert.IsNotNull(rightLaneHolder);
        }

        [ContextMenu("Build sequence")]
        private void BuildSequence()
        {
            if (!Application.isPlaying)
            {
                Awake();
            }

            sequence.Clear();

            BuildLane(leftLaneHolder, Lane.Left);
            BuildLane(midLaneHolder, Lane.Mid);
            BuildLane(rightLaneHolder, Lane.Right);
        }

        private void BuildLane(Transform holder, Lane lane)
        {
            for (int i = 0; i < holder.childCount; i++)
            {
                if (holder.GetChild(i).TryGetComponent(out Unit unit))
                {
                    unit.SaveUnitData();
                    if (sequence.GetLane(lane, out LaneSequence laneSequence))
                    {
                        laneSequence.Units.Add(unit.GetUnitData());
                    }
                }
            }
        }
    }
}