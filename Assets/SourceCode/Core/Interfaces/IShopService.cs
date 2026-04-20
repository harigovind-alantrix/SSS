using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces
{
    public interface IShopService
    {
        int Coins { get; }
        IReadOnlyList<ShopItem> Items { get; }
        bool TryPurchase(string itemId);
        ShopItem GetSelected();
        void Select(string itemId);
    }
}
