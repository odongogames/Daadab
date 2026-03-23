using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Hazards/Sand Trap")]
    public class SandTrapHazard : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            truck.ReduceSpeed();
        }

        public override void ExitTrigger(Truck truck)
        {
            base.ExitTrigger(truck);

            truck.RestoreSpeed();
        }
    }
}
