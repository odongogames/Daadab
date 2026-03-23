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

        private void Awake()
        {
            Assert.IsNotNull(sectionStart);
            Assert.IsNotNull(sectionEnd);

            Assert.IsTrue(sectionEnd.position.z > sectionStart.position.z);
        }

        public float GetLength()
        {
            return sectionEnd.position.z - sectionStart.position.z;
        }
    }
}
