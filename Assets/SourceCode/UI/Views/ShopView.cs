using FairyGUI;
using UI.Abstract;

namespace UI.Views
{
    public class ShopView : ViewBase
    {
        protected override string PackageName => "Shop";
        protected override string ComponentName => "ShopPanel";

        private GButton _previousButton;
        private GButton _nextButton;
        private GButton _buyButton;
        private GButton _selectButton;
        private GButton _backButton;
        private GLoader _previewLoader;
        private GTextField _priceText;
        private GTextField _coinsText;

        protected override void OnCreateUI()
        {
            _previousButton = Panel.GetChild("PreviousBtn").asButton;
            _nextButton = Panel.GetChild("NextBtn").asButton;
            _buyButton = Panel.GetChild("BuyBtn").asButton;
            _selectButton = Panel.GetChild("SelectBtn").asButton;
            _backButton = Panel.GetChild("BackBtn").asButton;
            _previewLoader = Panel.GetChild("PreviewLoader").asLoader;
            _priceText = Panel.GetChild("PriceTxt").asTextField;
            _coinsText = Panel.GetChild("CoinsTxt").asTextField;
            
            Panel.sortingOrder = 10;
        }

        public GButton PreviousButton => _previousButton;
        public GButton NextButton => _nextButton;
        public GButton BuyButton => _buyButton;
        public GButton SelectButton => _selectButton;
        public GButton CloseButton => _backButton;
        public GLoader PreviewLoader => _previewLoader;

        public void SetPreview(string texturePath)
            => _previewLoader.url = texturePath;

        public void SetPrice(int price) => _priceText.text = $"{price} 🪙";
        public void SetCoins(int coins) => _coinsText.text = $"{coins} 🪙";

        public void RefreshActionButton(bool isOwned, bool isSelected)
        {
            _buyButton.visible = !isOwned;
            _selectButton.visible = isOwned;
            _selectButton.selected = isSelected;
            _selectButton.grayed = isSelected;
        }
    }
}