using System;
using System.Threading;
using Core.Interfaces;
using Core.Models;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UI.Abstract;
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

        private Camera _previewCamera;
        private RenderTexture _renderTexture;
        private GameObject _currentPreviewInstance;

        private CancellationTokenSource _cts;

        private const float RotationSpeed = 45f;
        private int _index;

        public ShopPresenter(
            ShopView view,
            IPopUpManager popUpManager,
            IShopService shopService,
            IProgressionService progression)
        {
            _view = view;
            _popUpManager = popUpManager;
            _shopService = shopService;
            _progression = progression;
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

            SetupPreviewCamera();
        }

        public void Open()
        {
            Debug.Log($"[Shop] Coins: {_progression.Coins}");
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

            RefreshDisplay();
            _view.Show();
        }

        public void Close()
        {
            _view.Hide();
        }

        //! ---
        private void OnPrev(EventContext ctx)
        {
            _index = Mathf.Max(0, _index - 1);
            RefreshDisplay();
        }

        private void OnNext(EventContext ctx)
        {
            _index = Mathf.Min(_shopService.Items.Count - 1, _index + 1);
            RefreshDisplay();
        }

        private void OnBuy(EventContext ctx)
        {
            Debug.Log($"[Shop] Attempting purchase of {CurrentItem().id}, price {CurrentItem().price}, coins {_progression.Coins}");
            if (_shopService.TryPurchase(CurrentItem().id))
                RefreshDisplay();
            else
                Debug.Log("[Shop] Purchase failed");
        }

        private void OnSelect(EventContext ctx)
        {
            _shopService.Select(CurrentItem().id);
            RefreshDisplay();
        }

        private void OnClose(EventContext ctx)
        {
            _popUpManager.Close<IShopPopUp>();
        }

        //! ---
        private void RefreshDisplay()
        {
            var item = CurrentItem();
            var selected = _shopService.GetSelected();

            _view.SetPrice(item.price);
            _view.SetCoins(_progression.Coins);
            _view.RefreshActionButton(item.isOwned, item.id == selected.id);
            
            _view.PreviousButton.enabled = _index > 0;
            _view.NextButton.enabled = _index < _shopService.Items.Count - 1;

            SpawnPreview(item.prefab);
        }

        private ShopItem CurrentItem() => _shopService.Items[_index];

        private void SetupPreviewCamera()
        {
            var go = new GameObject("[ShopPreviewCam]");
            _previewCamera = go.AddComponent<Camera>();
            _previewCamera.clearFlags = CameraClearFlags.SolidColor;
            _previewCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
            _previewCamera.orthographic = true;
            _previewCamera.orthographicSize = 1.2f;
            _previewCamera.cullingMask = LayerMask.GetMask("ShopPreview");
            _previewCamera.enabled = false;
            _previewCamera.nearClipPlane = 0.1f;
            _previewCamera.farClipPlane = 100f;

            go.transform.position = new Vector3(999f, 999f, -4f);
            go.transform.LookAt(new Vector3(999f, 999f, 999f));

            _renderTexture = new RenderTexture(725, 550, 16);
            _previewCamera.targetTexture = _renderTexture;
        }

        private void SpawnPreview(GameObject prefab)
        {
            StopRotation();
            ClearPreviewInstance();

            if (prefab == null) return;

            _currentPreviewInstance = GameObject.Instantiate(
                prefab,
                new Vector3(999f, 999f, 999f),
                Quaternion.Euler(15f, 0f, 0f));

            SetLayerRecursive(_currentPreviewInstance, LayerMask.NameToLayer("ShopPreview"));

            _view.PreviewLoader.texture = new NTexture(_renderTexture);

            _cts = new CancellationTokenSource();
            RotateLoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid RotateLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (_currentPreviewInstance == null) return;

                _currentPreviewInstance.transform.Rotate(
                    Vector3.up, RotationSpeed * Time.deltaTime, Space.World);

                _previewCamera.enabled = true;
                _previewCamera.Render();
                _previewCamera.enabled = false;

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
        }

        private void StopRotation()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void ClearPreviewInstance()
        {
            if (_currentPreviewInstance == null) return;
            GameObject.Destroy(_currentPreviewInstance);
            _currentPreviewInstance = null;
        }

        private static void SetLayerRecursive(GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform child in go.transform)
                SetLayerRecursive(child.gameObject, layer);
        }

        public void Dispose()
        {
            if (_view.PreviousButton != null) _view.PreviousButton.onClick.Remove(OnPrev);
            if (_view.NextButton != null) _view.NextButton.onClick.Remove(OnNext);
            if (_view.BuyButton != null) _view.BuyButton.onClick.Remove(OnBuy);
            if (_view.SelectButton != null) _view.SelectButton.onClick.Remove(OnSelect);
            if (_view.CloseButton != null) _view.CloseButton.onClick.Remove(OnClose);

            StopRotation();
            ClearPreviewInstance();

            if (_previewCamera != null)
                GameObject.Destroy(_previewCamera.gameObject);

            if (_renderTexture != null)
            {
                _renderTexture.Release();
                _renderTexture = null;
            }
        }
    }
}