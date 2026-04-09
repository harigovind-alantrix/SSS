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
            builder.Register<MainMenuView>(Lifetime.Singleton);
            builder.Register<MainMenuPresenter>(Lifetime.Singleton);
            
        }
    }
}