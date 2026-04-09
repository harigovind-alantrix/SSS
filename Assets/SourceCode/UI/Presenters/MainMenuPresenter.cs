using System;
using Core.Messages.System;
using Core.Models;
using FairyGUI;
using MessagePipe;
using UI.Views;
using VContainer.Unity;

namespace UI.Presenters
{
    public class MainMenuPresenter : IDisposable
    {
        private readonly MainMenuView _view;
        private readonly IPublisher<OnGameStateChanged> _publisher;

        public MainMenuPresenter(
            MainMenuView view,
            IPublisher<OnGameStateChanged> publisher)
        {
            _view = view;
            _publisher = publisher;
        }

        public void Initialize()
        {
            _view.CreateUI();
            _view.PlayButton.onClick.Add(OnPlayClicked);
            _view.ShopButton.onClick.Add(OnShopClicked);
            _view.SettingsButton.onClick.Add(OnSettingsClicked);
            _view.QuitButton.onClick.Add(OnExitClicked);
        }

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();

        private void OnPlayClicked(EventContext ctx)
            => _publisher.Publish(new OnGameStateChanged(GameState.Menu, GameState.Playing));

        private void OnShopClicked(EventContext ctx)
            => _publisher.Publish(new OnGameStateChanged(GameState.Menu, GameState.Shop));

        private void OnSettingsClicked(EventContext ctx)
            => _publisher.Publish(new OnGameStateChanged(GameState.Menu, GameState.Settings));

        private void OnExitClicked(EventContext ctx)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public void Dispose()
        {
            if (_view.PlayButton != null) _view.PlayButton.onClick.Remove(OnPlayClicked);
            if (_view.ShopButton != null) _view.ShopButton.onClick.Remove(OnShopClicked);
            if (_view.SettingsButton != null) _view.SettingsButton.onClick.Remove(OnSettingsClicked);
            if (_view.QuitButton != null) _view.QuitButton.onClick.Remove(OnExitClicked);
        }
    }
}