namespace Core.Interfaces
{
    public interface ISaveService
    {
        int    GetInt(string key, int defaultValue = 0);
        float  GetFloat(string key, float defaultValue = 0f);
        string GetString(string key, string defaultValue = "");
        bool   GetBool(string key, bool defaultValue = false);

        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetString(string key, string value);
        void SetBool(string key, bool value);

        void Save();
    }
}