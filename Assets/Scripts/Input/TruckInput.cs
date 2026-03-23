using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TruckInput : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;

        private Truck laneSwitcher;

        private void Awake()
        {
            Assert.IsNotNull(inputReader);

            laneSwitcher = GetComponent<Truck>();
            Assert.IsNotNull(laneSwitcher);
        }

        private void Update()
        {
            if (inputReader.StartMovingLeft())
            {
                laneSwitcher.SetXDirection(-1);
            }

            if (inputReader.StartMovingRight())
            {
                laneSwitcher.SetXDirection(1);
            }
        }
    }
}
