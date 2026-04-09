using System;
using Core.Messages.System;
using Core.Models;
using FairyGUI;
using MessagePipe;
using UI.Presenters;
using VContainer.Unity;

namespace UI.Manager
{
    public class UIManager : IStartable, IDisposable
    {
        private readonly MainMenuPresenter _mainMenuPresenter;
        private readonly ISubscriber<OnGameStateChanged> _subscriber;
        private IDisposable _subscription;

        public UIManager(MainMenuPresenter mainMenuPresenter,
            ISubscriber<OnGameStateChanged> subscriber)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _subscriber = subscriber;
        }

        public void Start()
        {
            var scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
            scaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            GRoot.inst.SetContentScaleFactor(2352, 1568, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            scaler.ApplyChange();
            GRoot.inst.ApplyContentScaleFactor();
            GRoot.inst.MakeFullScreen();
            
            _mainMenuPresenter.Initialize();

            _mainMenuPresenter.Show();
            
            _subscription = _subscriber.Subscribe(OnGameStateChanged);
        }
        
        private void OnGameStateChanged(OnGameStateChanged message)
        {
            HideAll();
            
            switch (message.CurrentState)
            {
                case GameState.Menu:
                    _mainMenuPresenter.Show();
                    break;
                case GameState.Playing:
                case GameState.Shop:
                case GameState.Settings:
                   
                    break;
            }
        }

        private void HideAll()
        {
            _mainMenuPresenter.Hide();
        }
        
        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}