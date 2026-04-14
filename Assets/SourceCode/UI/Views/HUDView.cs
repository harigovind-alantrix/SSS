using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class HUDView :ViewBase
    {
        protected override string PackageName => "HUD";
        protected override string ComponentName => "HUDPanel";
        
        private GButton _pauseButton;
        private GButton _leftButton;
        private GButton _rightButton;
        private GButton _cloneButton;
        
        protected override void OnCreateUI()
        {
            _pauseButton = Panel.GetChild("PauseBtn").asButton;
            _leftButton = Panel.GetChild("LeftBtn").asButton;
            _rightButton = Panel.GetChild("RightBtn").asButton;
            _cloneButton = Panel.GetChild("CloneBtn").asButton;
        }
        
        public GButton PauseButton => _pauseButton;
        public GButton LeftButton => _leftButton;
        public GButton RightButton => _rightButton;
        public GButton CloneButton => _cloneButton;
    }
}