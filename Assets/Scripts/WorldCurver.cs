using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Daadab
{
    public class WorldCurver : MonoBehaviour
    {
//         private static bool isOn;
        
// #if UNITY_EDITOR
//         private static float sidewaysStrengthStatic;
//         private static float backwardsStrengthStatic;
// #endif

        [SerializeField] private Registry registry;

        //         private void OnValidate()
        //         {
        // #if UNITY_EDITOR
        //             sidewaysStrengthStatic = registry.sidewaysStrength.GetValue();
        //             backwardsStrengthStatic = registry.backwardsStrength.GetValue();
        // #endif
        //         }

        private void Awake()
        {
            Assert.IsNotNull(registry);
        }

        private void Start()
        {
            // isOn = registry.CurveWorld;
            UpdateValues();
        }

        // void Update()
        // {
        //     UpdateValues();
        // }

        [ContextMenu("Update Values")]
        private void UpdateValues()
        {
            Shader.SetGlobalFloat(
                name: "Vector1_2542eafe8f3f460e8f51d8938be2233c",
                value: registry.CurveWorld ? registry.SidewaysCurveStrength : 0
            );
            Shader.SetGlobalFloat(
                name: "Vector1_4ca42106f3354cc6afd0117201cae601",
                value: registry.CurveWorld ? registry.BackwardsCurveStrength : 0
            );
        }
    }
}
