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

        private Transform myTransform;

        private void Awake()
        {
            Assert.IsNotNull(sectionStart);
            Assert.IsNotNull(sectionEnd);

            Assert.IsTrue(sectionEnd.position.z > sectionStart.position.z);

            myTransform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SectionRemover"))
            {
                gameObject.SetActive(false);   
            }
        }

        public float GetLength()
        {
            return sectionEnd.position.z - sectionStart.position.z;
        }
    }
}
