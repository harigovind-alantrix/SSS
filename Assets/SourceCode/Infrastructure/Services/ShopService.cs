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
        
        private int _coins;
        private string _selectedId;

        public int Coins => _coins;
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
                item.isOwned = _progression.IsOwned(item.id);
        }


        public bool TryPurchase(string itemId)
        {
            var item = _config.items.Find(x => x.id == itemId);
            if (item == null || item.isOwned) return false;
            if (!_progression.SpendCoins(item.price)) return false;

            item.isOwned = true;
            _progression.SetOwned(itemId);
            Select(itemId);
            return true;
        }

        public ShopItem GetSelected()
        {
           var id = _save.GetString(SaveKeys.SelectedSkin);
           return _config.items.Find(x => x.id == id) ?? _config.items[0];
        }

        public void Select(string itemId)
        {
            var item = _config.items.Find(x => x.id == itemId && x.isOwned);
            if (item == null) return;
            _save.SetString(SaveKeys.SelectedSkin, itemId);
            _save.Save();
        }
    }
}