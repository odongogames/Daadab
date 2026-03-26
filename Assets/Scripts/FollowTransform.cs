using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class FollowTransform : GameStateSubscriber
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField][Range(0, 1)] private float xPositionMultiplier = 0.5f;

        private Transform myTransform;

        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(target);

            myTransform = transform;

            offset = myTransform.position - target.position;
        }

        private void LateUpdate()
        {
            var newPosition = target.position + offset;
            newPosition.x *= xPositionMultiplier;

            myTransform.position = newPosition;
        }

        public override void Initialise()
        {
            base.Initialise();

            myTransform.position = target.position + offset;
        }
    }
}
