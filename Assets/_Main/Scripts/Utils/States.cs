using System;
using System.Collections.Generic;
using GameStates;
using UnityEngine;

namespace Utils
{
    public class States
    {
        private GameState _currentState;
        private readonly Dictionary<Type, GameState> _cachedStates = new();

        public void Activate()
        {
            G.Resolve<Inputs>().PressedKey.SubscribeSilently((value, _) =>
                _currentState?.OnKeyPressed(value), notifyAlways: true);
        }
        
        public void ChangeState<T>(StateParameters parameters = null) where T : GameState, new()
        {
            _currentState?.OnExit();

            if (_cachedStates.TryGetValue(typeof(T), out var cachedState))
            { _currentState = cachedState; }
            else { _currentState = new T(); _cachedStates.Add(typeof(T), _currentState); }
            
            if (parameters != null) _currentState.SetParameters(parameters);
            _currentState.OnEnter();
        }
    }
}