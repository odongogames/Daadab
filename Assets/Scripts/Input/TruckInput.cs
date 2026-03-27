using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TruckInput : MonoBehaviour, IUnitComponent
    {
        private Truck truck;
        private InputReader inputReader;

        private void Awake()
        {
            truck = GetComponent<Truck>();
            Assert.IsNotNull(truck);
        }
        
        private void Start()
        {
            inputReader = InputReader.Instance;
            Assert.IsNotNull(inputReader);
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

            // if (inputReader.StartBoost())
            // {
            //     truck.StartBoost();
            // }
        }
        
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
    }
}
