using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.Services
{
    public class PlayerPrefsSaveService : ISaveService
    {
        public int    GetInt(string key, int defaultValue = 0)          => PlayerPrefs.GetInt(key, defaultValue);
        public float  GetFloat(string key, float defaultValue = 0f)     => PlayerPrefs.GetFloat(key, defaultValue);
        public string GetString(string key, string defaultValue = "")   => PlayerPrefs.GetString(key, defaultValue);
        public bool   GetBool(string key, bool defaultValue = false)    => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        public void SetInt(string key, int value)       => PlayerPrefs.SetInt(key, value);
        public void SetFloat(string key, float value)   => PlayerPrefs.SetFloat(key, value);
        public void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
        public void SetBool(string key, bool value)     => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public void Save() => PlayerPrefs.Save();
    }
}