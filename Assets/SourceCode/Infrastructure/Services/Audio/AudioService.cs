using Core.Interfaces;

namespace Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        public float MusicVolume { get; }
        public float SFXVolume { get; }
        public bool IsMuted { get; }
        public void SetMusicVolume(float value)
        {
            throw new System.NotImplementedException();
        }
        public void SetSFXVolume(float value)
        {
            throw new System.NotImplementedException();
        }
        public void ToggleMute()
        {
            throw new System.NotImplementedException();
        }
    }
}