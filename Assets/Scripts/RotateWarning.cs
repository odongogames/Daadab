using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class RotateWarning : MonoBehaviour
    {
        public GameObject warningPanel; // Assign your UI Panel here

        private void Awake()
        {
            Assert.IsNotNull(warningPanel);   
        }

        void Update()
        {
            // If the height is greater than the width, they are in Portrait
            bool isPortrait = Screen.height > Screen.width;
            warningPanel.SetActive(isPortrait);
        }
    }
}
