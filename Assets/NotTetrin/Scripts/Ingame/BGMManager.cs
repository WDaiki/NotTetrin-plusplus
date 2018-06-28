﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame {
    [RequireComponent(typeof(AudioSource))]
    public class BGMManager : MonoBehaviour {
        public AudioClip[] Clips;

        private AudioSource audioSource;
        private List<AudioClip> shuffleClips;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
            shuffleClips = new List<AudioClip>(Clips.Length);
        }

        public void Add(AudioClip clip) {
            shuffleClips.Add(clip);
        }

        public void Remove(AudioClip clip) {
            shuffleClips.Remove(clip);
        }

        public void RandomPlay() {
            // クリップがない場合、無音
            if (shuffleClips.Count == 0) { return; }

            var clip = shuffleClips.Shuffle().ElementAt(0);
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void Stop() {
            audioSource.Stop();
        }
    }
}
