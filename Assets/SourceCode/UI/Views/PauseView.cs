using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class PauseView : ViewBase
    {
        protected override string PackageName => "Pause";
        protected override string ComponentName => "PausePanel";

        private GButton _resumeButton;
        private GButton _settingsButton;
        private GButton _mainMenuButton;


        protected override void OnCreateUI()
        {
            _resumeButton = Panel.GetChild("ResumeBtn").asButton;
            _settingsButton = Panel.GetChild("SettingsBtn").asButton;
            _mainMenuButton = Panel.GetChild("MainMenuBtn").asButton;
        }

        public GButton ResumeButton => _resumeButton;
        public GButton SettingsButton => _settingsButton;
        public GButton MainMenuButton => _mainMenuButton;
    }
}