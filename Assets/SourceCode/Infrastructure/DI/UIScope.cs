using SourceCode.UI;
using UI.Abstract;
using UI.Manager;
using UI.Presenters;
using UI.Shop;
using UI.Views;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class UIScope :LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<UIManager>();
            
            builder.Register<PopUpManager>(Lifetime.Singleton).As<IPopUpManager>();
            builder.Register<ScreenTracker>(Lifetime.Singleton).As<IScreenTracker>();
            
            //! View
            builder.Register<MainMenuView>(Lifetime.Singleton);
            builder.Register<SettingsView>(Lifetime.Singleton);
            builder.Register<PauseView>(Lifetime.Singleton);
            builder.Register<HUDView>(Lifetime.Singleton);
            builder.Register<ShopView>(Lifetime.Singleton);
            
            //! Presenter
            builder.Register<MainMenuPresenter>(Lifetime.Singleton).As<IPresenter>();   
            builder.Register<PausePresenter>(Lifetime.Singleton).As<IPresenter>();
            builder.Register<HUDPresenter>(Lifetime.Singleton).As<IPresenter>();
            
            //! Popups
            builder.Register<SettingsPresenter>(Lifetime.Singleton).As<IPopUp>();
            builder.Register<ShopPresenter>(Lifetime.Singleton).As<IPopUp>();
            
            //! Shop
            builder.Register<ShopNavigator>(Lifetime.Singleton);
            builder.Register<ShopPreviewRenderer>(Lifetime.Singleton);
        }
    }
}