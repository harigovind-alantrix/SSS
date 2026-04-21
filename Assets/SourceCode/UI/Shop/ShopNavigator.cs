using System;
using Core.Interfaces;
using Core.Models;
using UnityEngine;

namespace UI.Shop
{
    public class ShopNavigator
    {
        private readonly IShopService _shopService;
        private int _index;

        public ShopItem Current => _shopService.Items[_index];
        public bool HasPrev    => _index > 0;
        public bool HasNext    => _index < _shopService.Items.Count - 1;

        public ShopNavigator(IShopService shopService)
        {
            _shopService = shopService;
        }

        public void SyncToSelected()
        {
            var selected = _shopService.GetSelected();
            _index = 0;
            for (int i = 0; i < _shopService.Items.Count; i++)
            {
                if (_shopService.Items[i].id == selected.id)
                {
                    _index = i;
                    break;
                }
            }
        }

        public void Prev() => _index = Mathf.Max(0, _index - 1);
        public void Next() => _index = Mathf.Min(_shopService.Items.Count - 1, _index + 1);
    }
}