using Core.Models;

namespace Core.Messages.Gameplay
{
    public readonly struct OnCoinsChanged
    {
        public readonly int PreviousCoins;
        public readonly int CurrentCoins;

        public OnCoinsChanged(int previousCoins, int currentCoins)
        {
            PreviousCoins = previousCoins;
            CurrentCoins = currentCoins;
        }
    }
    public readonly struct OnSessionCoinsChanged
    {
        public readonly int SessionCoins;
        public OnSessionCoinsChanged(int sessionCoins) => SessionCoins = sessionCoins;
    }
}