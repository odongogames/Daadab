
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Registry")]
    public class Registry : MyScriptableObject
    {
        public static Registry Instance;

        [SerializeField] private float totalGameTime = 60;
        public float TotalGameTime => totalGameTime; 

        private float shortTime = .3f;
        public float ShortTime => shortTime;

        private float mediumTime = .5f;
        public float MediumTime => mediumTime;

        private float longTime = .7f;
        public float LongTime => longTime;


        /// <summary>
        /// What is the number of water droplets that will be spawned for this mission?
        /// </summary>
        [SerializeField] private uint totalWaterCount;
        public uint TotalWaterCount => totalWaterCount;

        public void SetTotalWaterCount(uint count)
        {
            totalWaterCount = count;

            Debug.Log($"Set total water count: {totalWaterCount}");
        }

        /// <summary>
        /// Distance between each lane on the x-axis
        /// </summary>
        [SerializeField][Range(0, 6)] private float laneDistance = 3;
        public float LaneDistance => laneDistance;

        [SerializeField] private uint objectSequenceLength = 60;
        public float ObjectSequenceLength => objectSequenceLength;

        [SerializeField] private bool useFog;
        public bool UseFog => useFog;

        [SerializeField] private bool curveWorld;
        public bool CurveWorld => curveWorld;

        [SerializeField] private float sidewaysCurveStrength;
        public float SidewaysCurveStrength => sidewaysCurveStrength;

        [SerializeField] private float backwardsCurveStrength;
        public float BackwardsCurveStrength => backwardsCurveStrength;

        public override void OnValidate()
        {
            base.OnValidate();

            Initialise();
        }

        public void Enable()
        {
            Initialise();
        }

        private void Initialise()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
