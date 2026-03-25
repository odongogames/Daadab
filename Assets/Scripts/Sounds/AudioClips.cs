using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Audio Clips")]
    public class AudioClips : MyScriptableObject
    {
        [Header("Settings")]
        [Range(0, 1)] public float volume = 1;
        [Header("Sounds")]
        public AudioClip[] clips;
    }
}
