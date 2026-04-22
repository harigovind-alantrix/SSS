using System;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Models;
using MessagePipe;
using VContainer.Unity;

namespace Features.Gameplay
{
    public class GameOverHandler : IInitializable, IDisposable
    {
        private readonly IGameStateService _gameStateService;
        private readonly ISubscriber<PlayerFellEvent> _fellSubscriber;

        private IDisposable _subscription;

        public GameOverHandler(
            IGameStateService gameStateService,
            ISubscriber<PlayerFellEvent> fellSubscriber 
        )
        {
            _gameStateService = gameStateService;
            _fellSubscriber = fellSubscriber;
        }

        public void Initialize()
        {
            _subscription = _fellSubscriber.Subscribe(OnPlayerFell);
        }

        private void OnPlayerFell(PlayerFellEvent _)
        {
            _gameStateService.SetState(GameState.GameOver);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}