using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Hazards/Sand Trap")]
    public class SandTrapHazard : AbstractTruckModifier
    {
        public override void ModifyTruck(Truck truck)
        {
            if (truck.IsBoosting())
            {
                Debug.Log($"{name} cannot modify truck it's boosting");
                return;
            }

            truck.ReduceSpeed();
            
            SFXPlayer.Instance.StartPlayingGravelSound();
        }

        public override void ExitTrigger(Truck truck)
        {
            if (truck.IsBoosting())
            {
                Debug.Log($"{name} cannot modify truck it's boosting");
                return;
            }

            base.ExitTrigger(truck);

            truck.RestoreSpeed();

            SFXPlayer.Instance.StopPlayingSound();
        }
    }
}
