using System;
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Pickups/Health")]
    public class HealthPickup : AbstractTruckModifier
    {
        public static Action OnPickupHealth;

        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            if (truck.TryGetComponent(out IDamageable damageable))
            {
                damageable.AddHealth();
                OnPickupHealth?.Invoke();
            }
        }
    }
}
