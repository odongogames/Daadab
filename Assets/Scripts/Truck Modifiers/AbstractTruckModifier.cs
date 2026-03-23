using UnityEngine;

namespace Daadab
{
    public abstract class AbstractTruckModifier : MyScriptableObject
    {
        /// <summary>
        /// How long does this modifier last in seconds?
        /// </summary>
        [SerializeField] protected float lifeTime = 0;
        public float GetLifeTime() => lifeTime;

        public virtual void ModifyTruck(Truck truck)
        {
            Debug.Log($"{name} modify truck");
        }

        public virtual void FinishModifyingTruck(Truck truck)
        {
            Debug.Log($"{name} finish modifying truck");
        }
    }
}
