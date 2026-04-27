namespace Core.Interfaces
{
    public interface IProgressionService
    {
        int Coins { get; }
        int SessionCoins { get; }
        void AddCoins(int amount);
        bool SpendCoins(int amount);
        void ResetSessionCoins();
        
        bool IsOwned(string itemId);
        void SetOwned(string itemId);
    }
}