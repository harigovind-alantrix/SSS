using System;
using FairyGUI;
using Core.Interfaces;
using Core.Models;
using UI.Abstract;
using UI.Views;

namespace UI.Presenters
{
    public class MainMenuPresenter : IPresenter, IPopUpAware, IDisposable
    {
        public GameState TargetState => GameState.Menu;

        private readonly MainMenuView _view;
        private readonly IGameStateService _gameStateService;
        private readonly IPopUpManager _popUpManager;

        public MainMenuPresenter(
            MainMenuView view,
            IGameStateService gameStateService,
            IPopUpManager popUpManager)
        {
            _view = view;
            _gameStateService = gameStateService;
            _popUpManager = popUpManager;
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
        
        public void OnPopUpOpen() => _view.SetPopUpState(true);

        public void OnPopUpClose()=> _view.SetPopUpState(false);

        private void OnPlayClicked(EventContext ctx)
            => _gameStateService.SetState(GameState.Playing);

        private void OnShopClicked(EventContext ctx)
            => _popUpManager.Open<IShopPopUp>();

        private void OnSettingsClicked(EventContext ctx)
            => _popUpManager.Open<ISettingsPopUp>();

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