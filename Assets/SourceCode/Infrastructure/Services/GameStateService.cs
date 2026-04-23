using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MessagePipe;
using Core.Models;
using Core.Interfaces;
using Core.Messages.System;

namespace Infrastructure.Services
{
    public class GameStateService : IGameStateService
    {
        private readonly IPublisher<OnGameStateChanged> _onGameStateChangedPublisher;
        private readonly IPublisher<OnGameRestarted>    _onGameRestartedPublisher;
        
        private static readonly Dictionary<GameState, GameState[]> _allowed = new() {
            { GameState.Menu,          new[] { GameState.Playing,} },
            { GameState.Playing,       new[] { GameState.Paused, GameState.GameOver, } },
            { GameState.Paused,        new[] { GameState.Playing, GameState.Menu } },
            { GameState.GameOver,      new[] { GameState.Playing,GameState.Menu } },
        };
        
        public GameState CurrentState { get; private  set; } = GameState.Menu;

        public GameStateService(
            IPublisher<OnGameStateChanged> onGameStateChangedPublisher,
            IPublisher<OnGameRestarted>    onGameRestartedPublisher)
            
        {
            _onGameStateChangedPublisher = onGameStateChangedPublisher;
            _onGameRestartedPublisher = onGameRestartedPublisher;
        }
        
        public void SetState(GameState newState)
        {
            if(CurrentState == newState) return;

            if (!_allowed[CurrentState].Contains(newState))
            {
                Debug.LogWarning($"[GameState] Invalid transition: {CurrentState} → {newState}");
                return;
            }
            
            var previousState = CurrentState;
            CurrentState = newState;
            
            _onGameStateChangedPublisher.Publish(new OnGameStateChanged(previousState, newState));
            
            if(newState == GameState.Playing&&
                previousState== GameState.GameOver)
                {
                    _onGameRestartedPublisher.Publish(new OnGameRestarted());
                }
        }
    }
}