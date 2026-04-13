using SourceCode.UI;
using UI.Abstract;
using UI.Manager;
using UI.Presenters;
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
            
            //! Presenter
            builder.Register<MainMenuPresenter>(Lifetime.Singleton).As<IPresenter>();
            builder.Register<SettingsPresenter>(Lifetime.Singleton).As<IPopUp>();
        }
    }
}