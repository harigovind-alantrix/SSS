using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class MainMenuView : ViewBase
    {
        protected override string PackageName => "MainMenu";
        protected override string ComponentName => "MainMenuPanel";
        
        private GButton _playButton;
        private GButton _shopButton;
        private GButton _settingsButton;
        private GButton _quitButton;

        protected override void OnCreateUI()
        {
            _playButton = Panel.GetChild("PlayBtn").asButton;
            _shopButton = Panel.GetChild("ShopBtn").asButton;
            _settingsButton = Panel.GetChild("SettingsBtn").asButton;
            _quitButton = Panel.GetChild("QuitBtn").asButton;
        }
        
        protected override void OnShow() { }

        protected override void OnHide() { }

        public GButton PlayButton     => _playButton;
        public GButton ShopButton     => _shopButton;
        public GButton SettingsButton => _settingsButton;
        public GButton QuitButton     => _quitButton;
    }
}