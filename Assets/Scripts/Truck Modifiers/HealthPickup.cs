using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Pickups/Health")]
    public class HealthPickup : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            if (truck.TryGetComponent(out Health health))
            {
                health.AddHealth();
            }
        }
    }
}
