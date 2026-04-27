using System;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Messages.System;
using Core.Models;
using MessagePipe;

namespace Infrastructure.Services
{
    public class ProgressionService : IProgressionService ,IDisposable
    {
        private readonly ISaveService _save;
        private readonly IPublisher<OnCoinsChanged> _coinsPublisher;
        private readonly IPublisher<OnSessionCoinsChanged> _sessionCoinsPublisher;
        private readonly IDisposable _subscription;
        
        public int Coins { get; private set; }
        public int SessionCoins { get; private set; }

        public ProgressionService(ISaveService save,
            IPublisher<OnCoinsChanged> coinsPublisher,
            IPublisher<OnSessionCoinsChanged> sessionCoinsPublisher,
            ISubscriber<OnGameStateChanged> stateChangedSubscriber
            )
        {
            _save = save;
            _coinsPublisher = coinsPublisher;
            _sessionCoinsPublisher = sessionCoinsPublisher;

            _subscription = stateChangedSubscriber.Subscribe(OnGameStateChanged);
            Load();
        }
        private void OnGameStateChanged(OnGameStateChanged evt)
        {
            if (evt.CurrentState == GameState.Playing && 
                (evt.PreviousState == GameState.Menu || evt.PreviousState == GameState.GameOver))
            {
                ResetSessionCoins();
            }
        }
        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            int prevCoins = Coins;
            
            Coins += amount;
            SessionCoins += amount;
            
            _save.SetInt(SaveKeys.Coins, Coins);
            _save.Save();
            
            _coinsPublisher.Publish(new OnCoinsChanged(prevCoins, Coins));
            _sessionCoinsPublisher.Publish(new OnSessionCoinsChanged(SessionCoins));
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
        public void ResetSessionCoins()
        {
            SessionCoins = 0;
            _sessionCoinsPublisher.Publish(new OnSessionCoinsChanged(SessionCoins));
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

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}