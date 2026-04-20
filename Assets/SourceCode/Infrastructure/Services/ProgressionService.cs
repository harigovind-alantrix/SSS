using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Models;
using MessagePipe;

namespace Infrastructure.Services
{
    public class ProgressionService : IProgressionService
    {
        private readonly ISaveService _save;
        private readonly IPublisher<OnCoinsChanged> _coinsPublisher;
        
        public int Coins { get; private set; }

        public ProgressionService(ISaveService save,
            IPublisher<OnCoinsChanged> coinsPublisher)
        {
            _save = save;
            _coinsPublisher = coinsPublisher;
            Load();
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            int prevCoins = Coins;
            Coins += amount;
            _save.SetInt(SaveKeys.Coins, Coins);
            _save.Save();
            _coinsPublisher.Publish(new OnCoinsChanged(prevCoins, Coins));
        }

        public bool SpendCoins(int amount)
        {
           if(amount <= 0 || Coins < amount) return false;
           int prevCoins = Coins;
           Coins -= amount;
           _save.SetInt(SaveKeys.Coins, Coins);
           _save.Save();
           _coinsPublisher.Publish(new OnCoinsChanged(prevCoins, Coins));
           return true;
        }

        public bool IsOwned(string itemId)
        {
            return _save.GetBool(SaveKeys.OwnedPrefix + itemId);
        }

        public void SetOwned(string itemId)
        {
            _save.SetBool(SaveKeys.OwnedPrefix + itemId, true);
            _save.Save();
        }
        
        private void Load()
        {
            Coins = _save.GetInt(SaveKeys.Coins);
        }
    }
}