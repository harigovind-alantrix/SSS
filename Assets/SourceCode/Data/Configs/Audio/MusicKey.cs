using UnityEngine;

namespace Data.Configs.Audio
{
    [CreateAssetMenu(fileName = "MusicKey", menuName = "Config/Audio/MusicKey")]
    public class MusicKey : ScriptableObject
    {
        public AudioClip clip;
    }
}