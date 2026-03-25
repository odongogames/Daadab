using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Hazards/Rocks")]
    public class RockHazard : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            if (truck.IsBoosting())
            {
                Debug.Log($"{name} cannot modify truck it's boosting");
                return;
            }

            base.ModifyTruck(truck);

            if (truck.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }

            truck.ReduceSpeed();
        }

        public override void FinishModifyingTruck(Truck truck)
        {
            if (truck.IsBoosting())
            {
                Debug.Log($"{name} cannot modify truck it's boosting");
                return;
            }

            base.FinishModifyingTruck(truck);

            truck.RestoreSpeed();
        }
    }
}
