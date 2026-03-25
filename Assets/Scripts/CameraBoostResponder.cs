using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class CameraBoostResponder : GameStateSubscriber
    {
        [SerializeField] private float normalFieldOfView = 60;
        [SerializeField] private float boostedFieldOfView = 90;
        [SerializeField] private float transitionTime = 1f;

        private bool enteringBoostView;
        private bool exitingBoostView;

        private new Camera camera;

        public override void Awake()
        {
            base.Awake();

            camera = GetComponent<Camera>();
            Assert.IsNotNull(camera);

            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
        }

        private void Update()
        {
            if (enteringBoostView)
            {
                camera.fieldOfView = Mathf.MoveTowards(
                    current: camera.fieldOfView,
                    target: boostedFieldOfView,
                    maxDelta: Time.deltaTime * transitionTime
                );

                if (camera.fieldOfView == boostedFieldOfView)
                {
                    enteringBoostView = false;
                    Debug.Log("Entered boost view complete");
                }
            }

            if (exitingBoostView)
            {
                camera.fieldOfView = Mathf.MoveTowards(
                    current: camera.fieldOfView,
                    target: normalFieldOfView,
                    maxDelta: Time.deltaTime * transitionTime
                );

                if (camera.fieldOfView == normalFieldOfView)
                {
                    exitingBoostView = false;
                    Debug.Log("Exit boost view complete");
                }
            }
        }

        private void SpeedBooster_OnStartBoost()
        {
            enteringBoostView = true;
        }

        private void SpeedBooster_OnFinishBoost()
        {
            exitingBoostView = true;
        }
    }
}
