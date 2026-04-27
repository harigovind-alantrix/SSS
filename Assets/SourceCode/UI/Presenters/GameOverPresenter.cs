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
        private readonly IProgressionService _progressionService;

        public GameOverPresenter(GameOverView view,
            IGameStateService gameStateService,
            IProgressionService progressionService)
        {
            _view = view;
            _gameStateService = gameStateService;
            _progressionService = progressionService;
        }

        public void Initialize()
        {
            _view.CreateUI();
            _view.RestartButton.onClick.Add(OnRestartClicked);
            _view.MenuButton.onClick.Add(OnMenuClicked);
        }

        public void Show()
        {
            _view.Show();
            SetScore(_progressionService.SessionCoins);
        }

        public void Hide() => _view.Hide();

        private void OnRestartClicked(EventContext ctx)
            => _gameStateService.SetState(GameState.Playing);

        private void OnMenuClicked(EventContext ctx)
            => _gameStateService.SetState(GameState.Menu);

        private void SetScore(int score)
        {
            _view.CoinCountText.text = score.ToString();
        }

        public void Dispose()
        {
            if (_view != null) _view.RestartButton.onClick.Remove(OnRestartClicked);
            if (_view != null) _view.MenuButton.onClick.Remove(OnMenuClicked);
        }
    }
}