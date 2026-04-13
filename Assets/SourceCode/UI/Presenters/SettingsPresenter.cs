using System;
using Core.Interfaces;
using Core.Models;
using FairyGUI;
using UI.Abstract;
using UI.Views;

namespace UI.Presenters
{
    public class SettingsPresenter : ISettingsPopUp ,IDisposable
    {
        private readonly SettingsView _view;
        private readonly IAudioService _audioService;
        private readonly IPopUpManager _popUpManager;

        public SettingsPresenter(
            SettingsView view,
            IAudioService audioService,
            IPopUpManager popUpManager)
        {
            _view = view;
            _audioService = audioService;
            _popUpManager = popUpManager;
        }

        public void Initialize()
        {
            _popUpManager.Register<ISettingsPopUp>(this);
            
            _view.CreateUI();
            _view.MusicSlider.onChanged.Add(OnMusicSliderChanged);
            _view.SfxSlider.onChanged.Add(OnSfxSliderChanged);
            _view.MuteBtn.onClick.Add(OnMuteClicked);
            _view.CloseBtn.onClick.Add(OnCloseClicked);
        }
        public void Open() => _view.Show();
        public void Close() => _view.Hide();
        
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
            _popUpManager.Close<ISettingsPopUp>();
        }

        public void Dispose()
        {
            if(_view.MusicSlider != null)  _view.MusicSlider.onChanged.Remove(OnMusicSliderChanged);
            if(_view.SfxSlider != null) _view.SfxSlider.onChanged.Remove(OnSfxSliderChanged);
            if(_view.MuteBtn != null) _view.MuteBtn.onClick.Remove(OnMuteClicked);
            if(_view.CloseBtn != null) _view.CloseBtn.onClick.Remove(OnCloseClicked);
        }
    }
}