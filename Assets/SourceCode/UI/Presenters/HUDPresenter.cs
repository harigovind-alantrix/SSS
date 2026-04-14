using System;
using Core.Interfaces;
using Core.Models;
using FairyGUI;
using UI.Abstract;
using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class HUDPresenter : IPresenter, IDisposable
    {
        public GameState TargetState => GameState.Playing;

        private readonly HUDView _view;
        private readonly IGameStateService _gameStateService;

        public HUDPresenter(
            HUDView view,
            IGameStateService gameStateService
            )
        {
            _view = view;
            _gameStateService = gameStateService;
        }
        public void Initialize()
        {
            _view.CreateUI();
            _view.PauseButton.onClick.Add(OnPauseClicked);
            _view.LeftButton.onClick.Add(OnLeftClicked);
            _view.RightButton.onClick.Add(OnRightClicked);
            _view.CloneButton.onClick.Add(OnCloneClicked);
        }

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();

        private void OnPauseClicked(EventContext ctx) =>
            _gameStateService.SetState(GameState.Paused);

        private void OnLeftClicked(EventContext ctx) =>
            Debug.Log("Left clicked");

        private void OnRightClicked(EventContext ctx) =>
            Debug.Log("Right clicked");

        private void OnCloneClicked(EventContext ctx) =>
            Debug.Log("Clone clicked");

        public void Dispose()
        {
            if (_view.PauseButton != null) _view.PauseButton.onClick.Remove(OnPauseClicked);
            if (_view.RightButton != null) _view.RightButton.onClick.Remove(OnRightClicked);
            if (_view.LeftButton != null) _view.LeftButton.onClick.Remove(OnLeftClicked);
            if (_view.CloneButton != null) _view.CloneButton.onClick.Remove(OnCloneClicked);
        }
    }
}