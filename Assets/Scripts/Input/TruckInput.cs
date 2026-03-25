using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TruckInput : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private InputReader inputReader;

        private Truck truck;

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

            truck = GetComponent<Truck>();
            Assert.IsNotNull(truck);
        }

        private void Update()
        {
            if (inputReader.StartMovingLeft())
            {
                truck.SetXDirection(-1);
            }

            if (inputReader.StartMovingRight())
            {
                truck.SetXDirection(1);
            }

            if (inputReader.StartBoost())
            {
                truck.StartBoost();
            }
        }
    }
}
