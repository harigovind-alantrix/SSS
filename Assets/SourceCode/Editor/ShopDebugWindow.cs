using UnityEditor;
using UnityEngine;

namespace SourceCode.Editor
{
    public class ShopDebugWindow : EditorWindow
    {
        private int _coinsToAdd = 100;

        [MenuItem("Debug/Shop Debug")]
        public static void Open() => GetWindow<ShopDebugWindow>("Shop Debug");

        private void OnGUI()
        {
            GUILayout.Label("Coins", EditorStyles.boldLabel);
            
            int current = PlayerPrefs.GetInt("Coins", 0);
            EditorGUILayout.LabelField("Current Coins", current.ToString());
            
            _coinsToAdd = EditorGUILayout.IntField("Amount to Add", _coinsToAdd);

            if (GUILayout.Button("Add Coins"))
            {
                PlayerPrefs.SetInt("Coins", current + _coinsToAdd);
                PlayerPrefs.Save();
                Debug.Log($"[ShopDebug] Coins set to {current + _coinsToAdd}");
            }

            if (GUILayout.Button("Reset Coins"))
            {
                PlayerPrefs.SetInt("Coins", 0);
                PlayerPrefs.Save();
                Debug.Log("[ShopDebug] Coins reset to 0");
            }

            GUILayout.Space(10);
            GUILayout.Label("Ownership", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset All Owned Items"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("[ShopDebug] All PlayerPrefs cleared");
            }
        }
    }
}