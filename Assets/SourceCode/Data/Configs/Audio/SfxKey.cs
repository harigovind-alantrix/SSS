using UnityEngine;

namespace Data.Configs.Audio
{
    [CreateAssetMenu(fileName = "SfxKey", menuName = "Config/Audio/SfxKey")]
    public class SfxKey : ScriptableObject
    {
        public AudioClip clip;
    }
}