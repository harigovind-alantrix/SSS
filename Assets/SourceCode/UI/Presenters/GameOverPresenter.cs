using System;
using Core.Interfaces;
using Core.Models;
using FairyGUI;
using UI.Abstract;
using UI.Views;

namespace UI.Presenters
{
    public class GameOverPresenter : IPresenter, IDisposable
    {
        public GameState TargetState => GameState.GameOver;

        private readonly GameOverView _view;
        private readonly IGameStateService _gameStateService;

        public GameOverPresenter(GameOverView view, IGameStateService gameStateService)
        {
            _view = view;
            _gameStateService = gameStateService;
        }

        public void Initialize()
        {
            _view.CreateUI();
            _view.RestartButton.onClick.Add(OnRestartClicked);
            _view.MenuButton.onClick.Add(OnMenuClicked);
        }

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();

        private void OnRestartClicked(EventContext ctx)
            => _gameStateService.SetState(GameState.Playing);

        private void OnMenuClicked(EventContext ctx)
            => _gameStateService.SetState(GameState.Menu);

        public void Dispose()
        {
            if(_view != null) _view.RestartButton.onClick.Remove(OnRestartClicked);
            if(_view != null) _view.MenuButton.onClick.Remove(OnMenuClicked);
        }
    }
}