using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Pickups/Water")]
    public class WaterPickup : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            base.ModifyTruck(truck);

            truck.AddToWaterTank();
        }
    }
}
