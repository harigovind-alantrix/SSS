using System;
using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class MainMenuView : ViewBase
    {
        private GButton _playButton;
        private GButton _shopButton;
        private GButton _settingsButton;
        private GButton _exitButton;
        
        public override void CreateUI()
        {
          Panel = CreatePanel("MainMenu", "MainMenuPanel");
          _playButton = Panel.GetChild("PlayBtn").asButton;
          _shopButton = Panel.GetChild("ShopBtn").asButton;
          _settingsButton = Panel.GetChild("SettingsBtn").asButton;
          _exitButton = Panel.GetChild("ExitBtn").asButton;
          
        }
        
        protected override void OnShow()
        {
            
        }

        protected override void OnHide()
        {
         
        }

        public void OnPlayBtnClick(Action action)
        {
            _playButton.onClick.Add(() => action());
        }

        public void OnShopBtnClick(Action action)
        {
            _shopButton.onClick.Add(() => action());
        }

        public void OnSettingsBtnClick(Action action)
        {
            _settingsButton.onClick.Add(() => action());
        }

        public void OnExitBtnClick(Action action)
        {
            _exitButton.onClick.Add(() => action());
        }
    }
}