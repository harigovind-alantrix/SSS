using System;
using System.Collections.Generic;
using UI.Abstract;
using UnityEngine;

namespace UI.Manager
{
    public class PopUpManager :IPopUpManager
    {
        private readonly Dictionary<Type, IPopUp> _popups = new();

        public void Register<T>(IPopUp popUp) where T : IPopUp
        {
            _popups[typeof(T)] = popUp;
        }

        public void Open<T>() where T : IPopUp
        {
            if (_popups.TryGetValue(typeof(T), out IPopUp popUp))
            {
                popUp.Open();
            }
            else
            {
                Debug.LogWarning($"[PopUpManager] No overlay registered for {typeof(T).Name}");
            }
        }
        
        public void Close<T>() where T : IPopUp
        {
            if (_popups.TryGetValue(typeof(T), out var overlay))
                overlay.Close();
        }

        public void CloseAll()
        {
            foreach (var overlay in _popups.Values)
                overlay.Close();
        }
    }
}