namespace Core.Interfaces
{
    public interface IProgressionService
    {
        int Coins { get; }
        void AddCoins(int amount);
        bool SpendCoins(int amount);
        
        bool IsOwned(string itemId);
        void SetOwned(string itemId);
    }
}