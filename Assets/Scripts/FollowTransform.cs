using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private Transform myTransform;

        private void Awake()
        {
            Assert.IsNotNull(target);
            
            myTransform = transform;

            offset = myTransform.position - target.position;
        }

        private void LateUpdate()
        {
            myTransform.position = target.position + offset;
        }
    }
}
