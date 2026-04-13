namespace UI.Abstract
{
    public interface IScreenTracker
    {
        IPopUpAware ActiveOverlayAware { get; }
        void SetActive(IPresenter presenter);
    }
}