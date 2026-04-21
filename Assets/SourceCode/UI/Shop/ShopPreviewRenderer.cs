using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace UI.Shop
{
    public class ShopPreviewRenderer
    {
        private Camera _previewCamera;
        private RenderTexture _renderTexture;
        private GameObject _currentPreviewInstance;

        private CancellationTokenSource _cts;

        private const float RotationSpeed = 45f;

        public void Initialize()
        {
            SetupPreviewCamera();
        }

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

        public void Show(GameObject prefab, GLoader loader)
        {
            StopRotation();
            ClearPreviewInstance();

            if (prefab == null) return;

            _currentPreviewInstance = GameObject.Instantiate(
                prefab,
                new Vector3(999f, 999f, 999f),
                Quaternion.Euler(15f, 0f, 0f));

            SetLayerRecursive(_currentPreviewInstance, LayerMask.NameToLayer("ShopPreview"));

            loader.texture = new NTexture(_renderTexture);

            _cts = new CancellationTokenSource();
            RotateLoopAsync(_cts.Token).Forget();
        }

        public void Hide()
        {
            StopRotation();
            ClearPreviewInstance();
        }


        public async UniTaskVoid RotateLoopAsync(CancellationToken ct)
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

        public void StopRotation()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void ClearPreviewInstance()
        {
            if (_currentPreviewInstance == null) return;
            GameObject.Destroy(_currentPreviewInstance);
            _currentPreviewInstance = null;
        }

        public static void SetLayerRecursive(GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform child in go.transform)
                SetLayerRecursive(child.gameObject, layer);
        }

        public void Dispose()
        {
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