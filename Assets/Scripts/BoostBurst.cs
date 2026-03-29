using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class BoostBurst : MonoBehaviour
    {
        private new ParticleSystem particleSystem;

        private Transform truckTransform;
        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;

            particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(particleSystem);

            SpeedBoostPickup.OnPickupBoost += PickupBoostResponse;
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            truckTransform = truck.transform; 
        }

        private void OnDestroy()
        {
            SpeedBoostPickup.OnPickupBoost -= PickupBoostResponse;
        }

        private void PickupBoostResponse()
        {
            myTransform.position = truckTransform.position + new Vector3(0, 0, 6);

            particleSystem.Emit(45);
        }
    }
}