using System;
using System.Threading;
using Core.Interfaces;
using Core.Models;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UI.Abstract;
using UI.Shop;
using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class ShopPresenter : IShopPopUp, IDisposable
    {
        private readonly ShopView _view;
        private readonly IPopUpManager _popUpManager;
        private readonly IShopService _shopService;
        private readonly IProgressionService _progression;
        private readonly ShopNavigator _navigator;
        private readonly ShopPreviewRenderer _previewRenderer;

        public ShopPresenter(
            ShopView view,
            IPopUpManager popUpManager,
            IShopService shopService,
            IProgressionService progression,
            ShopNavigator navigator,
            ShopPreviewRenderer previewRenderer)
        {
            _view = view;
            _popUpManager = popUpManager;
            _shopService = shopService;
            _progression = progression;
            _navigator = navigator;
            _previewRenderer = previewRenderer;
        }

        public void Initialize()
        {
            _popUpManager.Register<IShopPopUp>(this);

            _view.CreateUI();
            _view.PreviousButton.onClick.Add(OnPrev);
            _view.NextButton.onClick.Add(OnNext);
            _view.BuyButton.onClick.Add(OnBuy);
            _view.SelectButton.onClick.Add(OnSelect);
            _view.CloseButton.onClick.Add(OnClose);

            _previewRenderer.Initialize();
        }

        public void Open()
        {
            Debug.Log($"[Shop] Coins: {_progression.Coins}");
            _navigator.SyncToSelected();
            RefreshDisplay();
            _view.Show();
        }

        public void Close()
        {
            _previewRenderer.Hide();
            _view.Hide();
        }
        private void OnPrev(EventContext ctx)
        {
            _navigator.Prev();
            RefreshDisplay();
        }

        private void OnNext(EventContext ctx)
        {
            _navigator.Next();
            RefreshDisplay();
        }

        private void OnBuy(EventContext ctx)
        {
            Debug.Log(
                $"[Shop] Attempting purchase of {_navigator.Current.id}, price {_navigator.Current.price}, coins {_progression.Coins}");
            if (_shopService.TryPurchase(_navigator.Current.id))
                RefreshDisplay();
            else
                Debug.Log("[Shop] Purchase failed");
        }

        private void OnSelect(EventContext ctx)
        {
            _shopService.Select(_navigator.Current.id);
            RefreshDisplay();
        }

        private void OnClose(EventContext ctx)
        {
            _popUpManager.Close<IShopPopUp>();
        }
        
        private void RefreshDisplay()
        {
            var item = _navigator.Current;
            var selected = _shopService.GetSelected();

            _view.SetPrice(item.price);
            _view.SetCoins(_progression.Coins);
            _view.RefreshActionButton(item.isOwned, item.id == selected.id);

            _view.PreviousButton.enabled = _navigator.HasPrev;
            _view.NextButton.enabled = _navigator.HasNext;

            _previewRenderer.Show(item.prefab,_view.PreviewLoader);
        }

        public void Dispose()
        {
            if (_view.PreviousButton != null) _view.PreviousButton.onClick.Remove(OnPrev);
            if (_view.NextButton != null) _view.NextButton.onClick.Remove(OnNext);
            if (_view.BuyButton != null) _view.BuyButton.onClick.Remove(OnBuy);
            if (_view.SelectButton != null) _view.SelectButton.onClick.Remove(OnSelect);
            if (_view.CloseButton != null) _view.CloseButton.onClick.Remove(OnClose);

            _previewRenderer.Dispose();
        }
    }
}