using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Data.Configs.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [Header("Mixer")]
        public AudioMixer mixer;

        public string masterVolumeParam = "MasterVolume";
        public string musicVolumeParam = "MusicVolume";
        public string sfxVolumeParam = "SFXVolume";
    }
}