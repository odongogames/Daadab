using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] [Range(0,1)] private float xPositionMultiplier = 0.5f;
        [SerializeField] private float smoothTime = 1;

        private Vector3 velocity;
        private Transform myTransform;

        private void Awake()
        {
            Assert.IsNotNull(target);
            
            myTransform = transform;

            offset = myTransform.position - target.position;
        }

        private void LateUpdate()
        {
            var newPosition = target.position + offset;
            newPosition.x *= xPositionMultiplier;

            myTransform.position = Vector3.SmoothDamp(
                current: myTransform.position, 
                target: newPosition,
                currentVelocity: ref velocity,
                smoothTime: Time.deltaTime * smoothTime
            );
        }
    }
}
