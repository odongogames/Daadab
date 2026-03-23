using System;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    [Serializable]
    public class LaneSequence
    {
        public Lane Lane;
        public List<UnitData> Units;
    }

    [CreateAssetMenu(menuName ="Units/Unit Sequence")]
    public class UnitSequence : MyScriptableObject
    {
        [SerializeField] private List<LaneSequence> lanes = new();
        public List<LaneSequence> Lanes => lanes;

        public bool GetLane(Lane lane, out LaneSequence laneSequence)
        {
            foreach (var sequence in lanes)
            {
                if (sequence.Lane == lane)
                {
                    laneSequence = sequence;
                    return true;
                }
            }

            laneSequence = new LaneSequence();
            return false;
        }

        public void Clear()
        {
            lanes = new()
            {
                new LaneSequence
                {
                    Lane = Lane.Left,
                    Units = new()
                },
                new LaneSequence
                {
                    Lane = Lane.Mid,
                    Units = new()
                },
                new LaneSequence
                {
                    Lane = Lane.Right,
                    Units = new()
                }
            };
        }
    }
}
