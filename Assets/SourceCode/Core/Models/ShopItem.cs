using System;
using UnityEngine;

namespace Core.Models
{
    [Serializable]
    public class ShopItem
    {
            public string id;
            public string displayName;
            public int price;
            public GameObject prefab;
            public bool isOwned;
    }
}