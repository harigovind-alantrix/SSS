using UnityEngine;

namespace Data.Configs.Audio
{
    [CreateAssetMenu(fileName = "AudioKeys", menuName = "Config/Audio/AudioKeys")]
    public class AudioKeys : ScriptableObject
    {
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