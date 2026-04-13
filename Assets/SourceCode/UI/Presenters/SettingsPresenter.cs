using Core.Interfaces;
using Core.Models;
using FairyGUI;
using UI.Abstract;
using UI.Views;

namespace UI.Presenters
{
    public class SettingsPresenter : IPresenter
    {
        public GameState TargetState => GameState.Settings;

        private readonly SettingsView _view;
        private readonly IGameStateService _gameStateService;
        private readonly IAudioService _audioService;

        public SettingsPresenter(
            SettingsView view,
            IGameStateService gameStateService,
            IAudioService audioService)
        {
            _view = view;
            _gameStateService = gameStateService;
            _audioService = audioService;
        }

        public void Initialize()
        {
            _view.CreateUI();
            _view.MusicSlider.onChanged.Add(OnMusicSliderChanged);
            _view.SfxSlider.onChanged.Add(OnSfxSliderChanged);
            _view.MuteBtn.onClick.Add(OnMuteClicked);
            _view.CloseBtn.onClick.Add(OnCloseClicked);
        }

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();
        
        private void OnMusicSliderChanged(EventContext ctx)
        {
            var normalized = (float)_view.MusicSlider.value / 100f;
            _audioService.SetMusicVolume(normalized);
        }

        private void OnSfxSliderChanged(EventContext ctx)
        {
            var normalized = (float)_view.SfxSlider.value / 100f;
            _audioService.SetSFXVolume(normalized);
        }

        private void OnMuteClicked(EventContext ctx)
        {
            _audioService.ToggleMute();
            _view.MuteBtn.selected = _audioService.IsMuted;
        }

        private void OnCloseClicked(EventContext ctx)
        {
            _gameStateService.SetState(_gameStateService.PreviousState);
        }
    }
}