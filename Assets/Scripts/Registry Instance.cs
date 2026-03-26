using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class RegistryInstance : MonoBehaviour
    {
        public static RegistryInstance Instance;

        [SerializeField] private Registry registry;

        public Registry Registry => registry;

        private void Awake()
        {
            
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            Assert.IsNotNull(registry);
        }
    }
}
