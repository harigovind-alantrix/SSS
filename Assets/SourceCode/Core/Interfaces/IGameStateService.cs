using Core.Models;

namespace Core.Interfaces
{
    public interface IGameStateService
    {
        GameState CurrentState { get; }
        void SetState(GameState newState);
    }
}