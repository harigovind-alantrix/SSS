using Core.Interfaces;
using Core.Models;
using Data.Configs.Audio;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly AudioConfig _config;
        private readonly AudioMixer _mixer;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private readonly ISaveService _save;

        private const float MinDb = -80f;

        public bool IsMasterMuted { get; private set; }
        public bool IsMusicMuted { get; private set; }
        public bool IsSfxMuted { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }

        public AudioService(AudioConfig config,
            [Key(InjectId.Music)] AudioSource musicSource,
            [Key(InjectId.Sfx)] AudioSource sfxSource,
            ISaveService save)
        {
            _config = config;
            _mixer = config.mixer;
            _musicSource = musicSource;
            _sfxSource = sfxSource;
            _save = save;

            _musicSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("Music")[0];
            _sfxSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("Sfx")[0];

            Load();
            ApplyAll();
        }

        //! Public Methods
        public void PlaySfx(SfxKey key)
        {
            if (key == null || key.clip == null) return;
            if (IsSfxMuted || IsMasterMuted) return;
            _sfxSource.PlayOneShot(key.clip);
        }

        public void PlayMusic(MusicKey key)
        {
            if (key == null || key.clip == null) return;
            if (_musicSource.clip == key.clip && _musicSource.isPlaying) return;

            _musicSource.clip = key.clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void StopMusic()
            => _musicSource.Stop();

        public void SetMasterMute(bool isMuted)
        {
            IsMasterMuted = isMuted;
            _save.SetBool(SaveKeys.MasterMuted, IsMasterMuted);
            _save.Save();
            ApplyMasterMute();
        }

        public void SetMusicMute(bool isMuted)
        {
            IsMusicMuted = isMuted;
            _save.SetBool(SaveKeys.MusicMuted, IsMusicMuted);
            _save.Save();
            ApplyMusicVolume();
        }

        public void SetSfxMute(bool isMuted)
        {
            IsSfxMuted = isMuted;
            _save.SetBool(SaveKeys.SfxMuted, IsSfxMuted);
            _save.Save();
            ApplySfxVolume();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            _save.SetFloat(SaveKeys.MusicVolume, MusicVolume);
            _save.Save();
            ApplyMusicVolume();
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Clamp01(volume);
            _save.SetFloat(SaveKeys.SfxVolume, SfxVolume);
            _save.Save();
            ApplySfxVolume();
        }

        //! Private Methods

        private void Load()
        {
            MusicVolume = _save.GetFloat(SaveKeys.MusicVolume, 1f);
            SfxVolume = _save.GetFloat(SaveKeys.SfxVolume, 1f);
            IsMusicMuted = _save.GetBool(SaveKeys.MusicMuted, false);
            IsSfxMuted = _save.GetBool(SaveKeys.SfxMuted, false);
            IsMasterMuted = _save.GetBool(SaveKeys.MasterMuted, false);
        }

        private void ApplyAll()
        {
            ApplyMasterMute();
            ApplyMusicVolume();
            ApplySfxVolume();
        }

        private void ApplyMasterMute()
        {
            float db = IsMasterMuted ? MinDb : 0f;
            _mixer.SetFloat(_config.masterVolumeParam, db);
        }

        private void ApplyMusicVolume()
        {
            float db = IsMusicMuted ? MinDb : LinearToDecibel(MusicVolume);
            _mixer.SetFloat(_config.musicVolumeParam, db);
        }

        private void ApplySfxVolume()
        {
            float db = IsSfxMuted ? MinDb : LinearToDecibel(SfxVolume);
            _mixer.SetFloat(_config.sfxVolumeParam, db);
        }

        private static float LinearToDecibel(float linear)
            => Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }
}