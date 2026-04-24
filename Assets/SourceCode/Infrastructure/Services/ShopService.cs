using System.Collections.Generic;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Models;
using Data.Configs;
using MessagePipe;
using UnityEngine;

namespace Infrastructure.Services
{
    public class ShopService : IShopService
    {
        private readonly ShopConfig _config;
        private readonly IProgressionService _progression;
        private readonly ISaveService _save;

        private readonly HashSet<string> _ownedIds = new();

        public int Coins => _progression.Coins;
        public IReadOnlyList<ShopItem> Items => _config.items;


        public ShopService(
            ShopConfig config,
            IProgressionService progression,
            ISaveService save)
        {
            _config = config;
            _progression = progression;
            _save = save;

            foreach (var item in _config.items)
            {
                if (_progression.IsOwned(item.id))
                {
                    _ownedIds.Add(item.id);
                }
            }
        }


        public bool TryPurchase(string itemId)
        {
            var item = _config.items.Find(x => x.id == itemId);
            if (item == null || item.isOwned) return false;
            if (!_progression.SpendCoins(item.price)) return false;

            _ownedIds.Add(itemId);
            _progression.SetOwned(itemId);
            Select(itemId);
            return true;
        }

        public ShopItem GetSelected()
        {
            var id = _save.GetString(SaveKeys.SelectedSkin);

            var item = _config.items.Find(x => x.id == id && _ownedIds.Contains(x.id));

            return item ?? _config.items[0];
        }

        public void Select(string itemId)
        {
            if (!_ownedIds.Contains(itemId)) return;
            _save.SetString(SaveKeys.SelectedSkin, itemId);
            _save.Save();
        }
        
        public bool IsOwned(string itemId) => _ownedIds.Contains(itemId);
    }
}