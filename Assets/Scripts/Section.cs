using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class Section : MonoBehaviour
    {
        [SerializeField] private Transform sectionStart;
        public Transform SectionStart => sectionStart;
        [SerializeField] private Transform sectionEnd;
        public Transform SectionEnd => sectionEnd;
        [SerializeField] private ObjectSpawner objectSpawner;

        [SerializeField] private Transform playerTransform;

        private Transform myTransform;

        // TODO: Find player transform in a better way
        public void SetPlayerTransform(Transform trans) => playerTransform = trans;

        private void Awake()
        {
            Assert.IsNotNull(sectionStart);
            Assert.IsNotNull(sectionEnd);
            Assert.IsNotNull(objectSpawner);

            Assert.IsTrue(sectionEnd.position.z > sectionStart.position.z);

            myTransform = transform;
        }

        private void Update()
        {
            if (null != playerTransform)
            {
                if (playerTransform.position.z > myTransform.position.z + 90)
                {
                    objectSpawner.Disable();
                    gameObject.SetActive(false);
                }
            }
        }

        public void SetObjectSequence(PooledObjectSequence sequence)
        {
            if (null == sequence)
            {
                Debug.LogError($"{name} cannot spawn objects. Object sequence is null!");
                return;
            }

            objectSpawner.SpawnObjects(sequence);
        }

        public float GetLength()
        {
            return sectionEnd.position.z - sectionStart.position.z;
        }
    }
}
