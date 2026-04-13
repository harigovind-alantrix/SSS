namespace UI.Abstract
{
    public interface IPopUpManager
    {
        public void Register<T>(IPopUp popUp) where T : IPopUp;
        public void Open<T>() where T : IPopUp;
        public void Close<T>() where T : IPopUp;
        public void CloseAll();
    }
}