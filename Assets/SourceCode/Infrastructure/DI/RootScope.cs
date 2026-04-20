using VContainer;
using VContainer.Unity;
using MessagePipe;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Messages.System;
using Infrastructure.Services;
using Infrastructure.Services.Audio;

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
            builder.RegisterMessageBroker<OnGameStarted>(options);

            //? Progression
            builder.RegisterMessageBroker<OnCoinsChanged>(options);

            //? Gameplay
            builder.RegisterMessageBroker<MoveInputEvent>(options);
            builder.RegisterMessageBroker<JumpInputEvent>(options);
            builder.RegisterMessageBroker<CloneInputEvent>(options);
            builder.RegisterMessageBroker<CameraZoomEvent>(options);

            //! Services
            builder.Register<GameStateService>(Lifetime.Singleton).As<IGameStateService>();

            builder.Register<AudioService>(Lifetime.Singleton).As<IAudioService>();
        }
    }
}