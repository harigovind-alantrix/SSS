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
        
        [Header("Save Keys")]
        public string masterMutedKey  = "MasterMuted";
        public string musicVolumeKey  = "MusicVol";
        public string musicMutedKey   = "MusicMuted";
        public string sfxVolumeKey    = "SfxVol";
        public string sfxMutedKey     = "SfxMuted";
        
        [Header("Music")]
        public MusicKey menuMusic;
        public MusicKey gameplayMusic;
        public MusicKey gameOverMusic;

        [Header("SFX")]
        public SfxKey jump;
        public SfxKey land;
        public SfxKey clone;
        public SfxKey death;
        public SfxKey purchase;
        public SfxKey buttonClick;
    }
}