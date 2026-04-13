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
            
            //! View
            builder.Register<MainMenuView>(Lifetime.Singleton);
            builder.Register<SettingsView>(Lifetime.Singleton);
            
            //! Presenter
            builder.Register<MainMenuPresenter>(Lifetime.Singleton).As<IPresenter>();
            builder.Register<SettingsPresenter>(Lifetime.Singleton).As<IPresenter>();
        }
    }
}