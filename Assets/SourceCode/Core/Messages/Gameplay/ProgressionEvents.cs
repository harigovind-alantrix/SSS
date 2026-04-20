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
}