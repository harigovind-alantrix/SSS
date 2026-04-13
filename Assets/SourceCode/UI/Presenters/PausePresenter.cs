using System;
using Core.Interfaces;
using Core.Models;
using FairyGUI;
using UI.Abstract;
using UI.Views;

namespace UI.Presenters
{
    public class PausePresenter : IPresenter,IDisposable
    {
        public GameState TargetState => GameState.Paused;

        private readonly PauseView _view;
        private readonly IGameStateService _gameStateService;
        private readonly IPopUpManager _popUpManager;

        public PausePresenter(
            PauseView view,
            IGameStateService gameStateService,
            IPopUpManager popUpManager
        )
        {
            _view = view;
            _gameStateService = gameStateService;
            _popUpManager = popUpManager;
        }

        public void Initialize()
        {
            _view.CreateUI();
            _view.ResumeButton.onClick.Add(OnResumeClicked);
            _view.SettingsButton.onClick.Add(OnSettingsClicked);
            _view.MainMenuButton.onClick.Add(OnMainMenuClicked);
        }

        public void Show() => _view.Show();

        public void Hide() => _view.Hide();

        private void OnResumeClicked(EventContext ctx) =>
            _gameStateService.SetState(GameState.Playing);
        private void OnSettingsClicked(EventContext ctx) =>
            _popUpManager.Open<ISettingsPopUp>();
        
        private void OnMainMenuClicked(EventContext ctx) =>
            _gameStateService.SetState(GameState.Menu);
        

        public void Dispose()
        {
            if(_view.ResumeButton != null) _view.ResumeButton.onClick.Remove(OnResumeClicked);
            if(_view.SettingsButton != null) _view.SettingsButton.onClick.Remove(OnSettingsClicked);
            if(_view.MainMenuButton != null) _view.MainMenuButton.onClick.Remove(OnMainMenuClicked);
        }
    }
}