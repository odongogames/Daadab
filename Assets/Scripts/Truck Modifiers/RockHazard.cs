using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Hazards/Rocks")]
    public class RockHazard : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            if (truck.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }

            truck.ReduceSpeed();
        }

        public override void FinishModifyingTruck(Truck truck)
        {
            base.FinishModifyingTruck(truck);

            truck.RestoreSpeed();
        }
    }
}
