using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class SettingsView : ViewBase
    {
        protected override string PackageName => "Settings";
        protected override string ComponentName => "SettingsPanel";

        private GSlider _musicSlider;
        private GSlider _sfxSlider;
        private GButton _muteBtn;
        private GButton _backBtn;

        protected override void OnCreateUI()
        {
            _musicSlider = Panel.GetChild("MusicSlider").asSlider;
            _sfxSlider = Panel.GetChild("SfxSlider").asSlider;
            _muteBtn = Panel.GetChild("MuteBtn").asButton;
            _backBtn = Panel.GetChild("BackBtn").asButton;
            
            Panel.sortingOrder = 10;
        }
        
        public GSlider MusicSlider => _musicSlider;
        public GSlider SfxSlider => _sfxSlider;
        public GButton MuteBtn => _muteBtn;
        public GButton CloseBtn => _backBtn;
    }
}