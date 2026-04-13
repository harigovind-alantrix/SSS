using UI.Abstract;

namespace SourceCode.UI
{
    public class ScreenTracker :IScreenTracker
    {
        private IPresenter _active;

        public IPopUpAware ActiveOverlayAware => _active as IPopUpAware;

        public void SetActive(IPresenter presenter)
            => _active = presenter;
    }
}