using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    public class DistanceMeter : GameStateSubscriber
    {
        private Slider slider;

        private DistanceCalculator distanceCalculator;

        public override void Awake()
        {
            base.Awake();

            slider = GetComponent<Slider>();
            Assert.IsNotNull(slider);

            distanceCalculator = DistanceCalculator.Instance;
            Assert.IsNotNull(distanceCalculator);
        }

        private void Update()
        {
            slider.value = distanceCalculator.PlayerDistToAbsoluteWorldEndNormalised;
        }
    }
}