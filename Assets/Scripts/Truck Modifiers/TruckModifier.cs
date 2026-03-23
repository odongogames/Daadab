using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TruckModifier : MonoBehaviour
    {
        [SerializeField] private AbstractTruckModifier modifier;
        public AbstractTruckModifier GetModifier() => modifier;

        private void Awake()
        {
            Assert.IsNotNull(modifier);
        }
    }
}
