namespace UI.Abstract
{
    public interface IPopUp
    {
        void Initialize();
        void Open();
        void Close();
    }
    
    public interface IShopPopUp    : IPopUp { }
    public interface ISettingsPopUp : IPopUp { }
}