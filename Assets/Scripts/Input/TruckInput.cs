using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TruckInput : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private InputReader inputReader;

        private Truck laneSwitcher;

        public void EnterActiveState()
        {
            enabled = true;
        }

        public void ExitActiveState()
        {
            enabled = false;
        }

        public void ResetMe()
        {
            enabled = false;
        }

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
