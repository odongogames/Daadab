using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TransformLookAt : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;

            Assert.IsNotNull(target);

            myTransform.LookAt(target);
        }

        private void Update()
        {
            myTransform.LookAt(target);
        }
    }
}
