using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class RegistryEnabler : MonoBehaviour
    {
        [SerializeField] private Registry registry;

        private void Awake()
        {
            Assert.IsNotNull(registry);
            registry.Enable();
        }
    }
}
