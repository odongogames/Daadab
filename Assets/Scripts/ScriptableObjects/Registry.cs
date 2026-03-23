
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Registry")]
    public class Registry : MyScriptableObject
    {
        /// <summary>
        /// Distance between each lane on the x-axis
        /// </summary>
        [SerializeField][Range(0, 4)] private uint laneDistance = 3;
        public uint LaneDistance => laneDistance;
    }
}
