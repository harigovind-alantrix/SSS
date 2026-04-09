using System;
using Core.Messages.System;
using Core.Models;
using MessagePipe;
using UI.Views;
using VContainer.Unity;

namespace UI.Presenters
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly IPublisher<OnGameStateChanged> _publisher;
        
        public MainMenuPresenter(
            MainMenuView mainMenuView,
            IPublisher<OnGameStateChanged> publisher)
        {
           _mainMenuView = mainMenuView;
           _publisher = publisher;
           
        }
        public void Initialize()
        {
            _mainMenuView.CreateUI();
            
            _mainMenuView.OnPlayBtnClick((() =>
            {
                _publisher.Publish(new OnGameStateChanged(GameState.Menu,GameState.Playing));
            }));
            
            _mainMenuView.OnShopBtnClick((() =>
            {
                
            }));
            
            _mainMenuView.OnSettingsBtnClick((() =>
            {
                
            }));
            
            _mainMenuView.OnExitBtnClick((() =>
            {
                
            }));
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}