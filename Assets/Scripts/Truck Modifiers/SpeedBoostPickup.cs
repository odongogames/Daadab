using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Pickups/Speed Boost")]
    public class SpeedBoostPickup : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            truck.BoostSpeed();
        }

        public override void FinishModifyingTruck(Truck truck)
        {
            base.FinishModifyingTruck(truck);

            truck.RestoreSpeed();
        }
    }
}
