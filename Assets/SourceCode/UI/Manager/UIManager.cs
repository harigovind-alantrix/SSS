using System;
using System.Collections.Generic;
using VContainer.Unity;
using MessagePipe;
using FairyGUI;
using Core.Messages.System;
using Core.Models;
using UI.Abstract;

namespace UI.Manager
{
    public class UIManager : IStartable, IDisposable
    {
        private readonly List<IPresenter> _presenters;
        private readonly List<IPopUp> _popUps;
        private readonly ISubscriber<OnGameStateChanged> _subscriber;
        private readonly IScreenTracker _screenTracker;

        private IDisposable _subscription;

        public UIManager(
            IEnumerable<IPresenter> presenters,
            IEnumerable<IPopUp> popUps,
            ISubscriber<OnGameStateChanged> subscriber,
            IScreenTracker screenTracker)

        {
            _presenters = new List<IPresenter>(presenters);
            _popUps = new List<IPopUp>(popUps);
            _subscriber = subscriber;
            _screenTracker = screenTracker;
        }

        public void Start()
        {
            var scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
            scaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            GRoot.inst.SetContentScaleFactor(2352, 1568, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            scaler.ApplyChange();
            GRoot.inst.ApplyContentScaleFactor();
            GRoot.inst.MakeFullScreen();

            foreach (var popUp in _popUps)
            {
                popUp.Initialize();
            }

            foreach (var presenter in _presenters)
            {
                presenter.Initialize();
            }

            ShowForState(GameState.Menu);

            _subscription = _subscriber.Subscribe(OnGameStateChanged);
        }

        private void OnGameStateChanged(OnGameStateChanged message) =>
            ShowForState(message.CurrentState);


        private void ShowForState(GameState state)
        {
            foreach (var presenter in _presenters)
            {
                if (presenter.TargetState == state)
                {
                    _screenTracker.SetActive(presenter);
                    presenter.Show();
                }
                else
                {
                    presenter.Hide();
                }
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}