using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public abstract class AbstractTruckModifier : MyScriptableObject
    {
        [SerializeField] private List<AbstractTruckModifier> cancellers = new();

        /// <summary>
        /// How long does this modifier last in seconds?
        /// </summary>
        [SerializeField] protected float lifeTime = 0;
        public float GetLifeTime() => lifeTime;

        /// <summary>
        /// Should the modifier be disable once it touches player
        /// </summary>
        [SerializeField] protected bool disableOnUse;
        public bool DisableOnUse() => disableOnUse;

        [SerializeField] protected AudioClips audio;
        public AudioClips GetAudio() => audio;

        public virtual void ModifyTruck(Truck truck)
        {
            // Debug.Log($"{name} modify truck");
        }

        public virtual void FinishModifyingTruck(Truck truck)
        {
            Debug.Log($"{name} finish modifying truck");
        }

        public virtual void ExitTrigger(Truck truck)
        {
            // Debug.Log($"{name} on trigger exit");
        }

        public bool IsCancelledBy(AbstractTruckModifier modifier)
        {
            return cancellers.Contains(modifier);
        }
    }
}
