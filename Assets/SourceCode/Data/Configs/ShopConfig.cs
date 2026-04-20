using System.Collections.Generic;
using Core.Models;
using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Config/ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        public List<ShopItem> items = new();
    }
}