
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Registry")]
    public class Registry : MyScriptableObject
    {
        /// <summary>
        /// Distance between each lane on the x-axis
        /// </summary>
        [SerializeField][Range(0, 6)] private float laneDistance = 3;
        public float LaneDistance => laneDistance;
        
        [SerializeField] private uint objectSequenceLength = 60;
        public float ObjectSequenceLength => objectSequenceLength;

        [SerializeField] private bool curveWorld;
        public bool CurveWorld => curveWorld;

        [SerializeField] private float sidewaysCurveStrength;
        public float SidewaysCurveStrength => sidewaysCurveStrength;

        [SerializeField] private float backwardsCurveStrength;
        public float BackwardsCurveStrength => backwardsCurveStrength;
    }
}
