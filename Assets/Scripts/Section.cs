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

        private void Awake()
        {
            Assert.IsNotNull(sectionStart);
            Assert.IsNotNull(sectionEnd);
            Assert.IsNotNull(objectSpawner);

            Assert.IsTrue(sectionEnd.position.z > sectionStart.position.z);
        }

        public void SetUnitSequence(UnitSequence unitSequence)
        {
            if (null == unitSequence)
            {
                Debug.LogError($"{name} cannot spawn objects. Unit sequence is null!");
                return;
            }

            objectSpawner.SpawnObjects(unitSequence);
        }

        public float GetLength()
        {
            return sectionEnd.position.z - sectionStart.position.z;
        }
    }
}
