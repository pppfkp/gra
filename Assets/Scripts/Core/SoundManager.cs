    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        // ustawianie właściwości muzyki
        public static SoundManager instance { get; private set; }
        private AudioSource source;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                source = GetComponent<AudioSource>();
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip _sound)
        {
            source.PlayOneShot(_sound);
        }
    }
