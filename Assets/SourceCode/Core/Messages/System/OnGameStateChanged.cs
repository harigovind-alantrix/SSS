using Core.Models;

namespace Core.Messages.System
{
    public readonly struct OnGameStateChanged
    {
        public readonly GameState PreviousState;
        public readonly GameState CurrentState;
        
        public OnGameStateChanged(GameState previousState, GameState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }
    }
}