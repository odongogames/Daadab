using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class SFXPlayer : MonoBehaviour
    {
        public static SFXPlayer Instance;

        [SerializeField] private AudioClips clickSound;
        [SerializeField] private AudioClips collectSound;
        [SerializeField] private AudioClips gravelSound;
        [SerializeField] private AudioClips pingSound;
        [SerializeField] private AudioClips powerupSound;

        [SerializeField] private AudioSource audioSource;

        private float volume = 1;
        private Camera mainCamera;
        private Transform mainCameraTransform;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            Assert.IsNotNull(clickSound);
            Assert.IsNotNull(collectSound);
            Assert.IsNotNull(gravelSound);
            Assert.IsNotNull(pingSound);
            Assert.IsNotNull(powerupSound);

            Assert.IsNotNull(audioSource);

            mainCamera = Camera.main;
            // mainCameraTransform = Camera.main.transform;
        }

        public void PlayClickSound() => PlayClip(clickSound);
        public void PlayCollectSound() => PlayClip(collectSound);
        public void PlayPingSound() => PlayClip(pingSound);
        public void PlayPowerupSound() => PlayClip(powerupSound);

        public void PlayClip(AudioClips clips)
        {
            PlaySound(clips, Vector3.zero);
        }

        private void PlaySound(AudioClip clip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume * volumeMultiplier);
        }

        private void PlaySound(AudioClips clipArray, Vector3 position, float volume = 1f)
        {
            if (clipArray == null)
            {
                Debug.LogWarning("cannot play. null audio clip!");
                return;
            }

            volume = clipArray.volume;
            var clip = clipArray.clips[UnityEngine.Random.Range(0, clipArray.clips.Length)];
            // Debug.Log("Play: " + clip.name);

            PlaySound(clip, position, volume);
        }

        public void StartPlayingGravelSound()
        {
            StartPlayingSound(gravelSound);
        }

        public void StartPlayingSound(AudioClips clips)
        {
            var clip = clips.clips[UnityEngine.Random.Range(0, clips.clips.Length)];

            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopPlayingSound()
        {
            audioSource.Pause();
        }
    }
}
