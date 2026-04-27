using VContainer;
using VContainer.Unity;
using MessagePipe;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Messages.System;
using Core.Models;
using Data.Configs;
using Data.Configs.Audio;
using Infrastructure.Services;
using Infrastructure.Services.Audio;
using UnityEngine;

namespace Infrastructure.DI
{
    public class RootScope : LifetimeScope
    {
        [SerializeField]
        private AudioConfig audioConfig;

        [SerializeField]
        private ShopConfig shopConfig;
        
        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        protected override void Configure(IContainerBuilder builder)
        {
            //! Message Pipe
            var options = builder.RegisterMessagePipe(options => { options.EnableCaptureStackTrace = true; });
            builder.RegisterMessageBroker<OnGameStateChanged>(options);
            builder.RegisterMessageBroker<OnGameRestarted>(options);

            //! Progression
            builder.RegisterMessageBroker<OnCoinsChanged>(options);
            builder.RegisterMessageBroker<OnSessionCoinsChanged>(options); 
            
            //! Gameplay
            builder.RegisterMessageBroker<PlayerFellEvent>(options);
            builder.RegisterMessageBroker<MoveInputEvent>(options);
            builder.RegisterMessageBroker<JumpInputEvent>(options);
            builder.RegisterMessageBroker<CloneInputEvent>(options);
            builder.RegisterMessageBroker<CameraZoomEvent>(options);

            //! Services
            builder.Register<GameStateService>(Lifetime.Singleton).As<IGameStateService>();
            builder.Register<PlayerPrefsSaveService>(Lifetime.Singleton).As<ISaveService>();
            builder.Register<ProgressionService>(Lifetime.Singleton).As<IProgressionService>();

            //! Audio
            CreateAudioSource();
            builder.RegisterInstance(audioConfig);
            builder.RegisterInstance(_musicSource).Keyed(InjectId.Music);
            builder.RegisterInstance(_sfxSource).Keyed(InjectId.Sfx);
            builder.Register<AudioService>(Lifetime.Singleton).As<IAudioService>();


            //! Shop
            builder.RegisterInstance(shopConfig);
            builder.Register<ShopService>(Lifetime.Singleton).As<IShopService>();
        }

        private void CreateAudioSource()
        {
            var audioGo = new GameObject("[AudioService]");
            DontDestroyOnLoad(audioGo);

            _musicSource = audioGo.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.outputAudioMixerGroup = audioConfig.mixer.FindMatchingGroups("Music")[0];

            _sfxSource = audioGo.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;
            _sfxSource.loop = false;
            _sfxSource.outputAudioMixerGroup = audioConfig.mixer.FindMatchingGroups("Sfx")[0];
        }
    }
}