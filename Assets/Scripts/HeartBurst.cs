using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class HeartBurst : MonoBehaviour
    {
        private new ParticleSystem particleSystem;

        private Transform truckTransform;
        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;

            particleSystem = GetComponent<ParticleSystem>();
            Assert.IsNotNull(particleSystem);

            HealthPickup.OnPickupHealth += PickupHealthResponse;
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            truckTransform = truck.transform; 
        }

        private void OnDestroy()
        {
            HealthPickup.OnPickupHealth -= PickupHealthResponse;
        }

        private void PickupHealthResponse()
        {
            myTransform.position = truckTransform.position + new Vector3(0, 0, 6);

            particleSystem.Emit(45);
        }
    }
}