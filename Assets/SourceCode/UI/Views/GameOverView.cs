using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class GameOverView : ViewBase
    {
        protected override string PackageName => "GameOver";
        protected override string ComponentName => "GameOverPanel";

        private GButton _restartButton;
        private GButton _menuButton;
        private GTextField _coinCountText;

        protected override void OnCreateUI()
        {
            _restartButton = Panel.GetChild("RestartBtn").asButton;
            _menuButton = Panel.GetChild("MenuBtn").asButton;
            _coinCountText = Panel.GetChild("CoinCountText").asTextField;
        }

        public GButton RestartButton => _restartButton;
        public GButton MenuButton => _menuButton;
        public GTextField CoinCountText => _coinCountText;
    }
}