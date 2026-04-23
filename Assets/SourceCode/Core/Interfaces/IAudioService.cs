using Data.Configs.Audio;

namespace Core.Interfaces
{
    public interface IAudioService
    {
        bool IsMasterMuted { get; }
        bool IsMusicMuted { get; }
        bool IsSfxMuted { get; }
        float MusicVolume { get; }
        float SfxVolume { get; }

        void PlaySfx(SfxKey key);
        void PlayMusic(MusicKey key);
        void StopMusic();
        void SetMasterMute(bool isMuted);
        void SetMusicMute(bool isMuted);
        void SetSfxMute(bool isMuted);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
    }
}