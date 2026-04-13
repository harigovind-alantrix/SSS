namespace Core.Interfaces
{
    public interface IAudioService
    {
        float MusicVolume { get; }
        float SFXVolume   { get; }
        bool  IsMuted     { get; }

        void SetMusicVolume(float value);
        void SetSFXVolume(float value);
        void ToggleMute();
    }
}