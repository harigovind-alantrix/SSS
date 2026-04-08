using VContainer;
using VContainer.Unity;
using MessagePipe;
using Core.Interfaces;
using Core.Messages.System;
using Infrastructure.Services;

namespace Infrastructure.DI
{
    public class RootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //! Message Pipe
            var options = builder.RegisterMessagePipe(options =>
            {
                options.EnableCaptureStackTrace = true;
            });
            builder.RegisterMessageBroker<OnGameStateChanged>(options);

            //! Services
            builder.Register<GameStateService>(Lifetime.Singleton).As<IGameStateService>();
        }
    }
}