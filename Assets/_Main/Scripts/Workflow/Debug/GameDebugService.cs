using System;
using System.Collections.Generic;
using UnityEngine;

namespace Workflow.Debug
{
    public static class GameDebugService
    {
        private static Debugger _debugger;
        private static List<Action> _onDrawGizmosDebug;
        
        public static void Init()
        {
            _debugger = new GameObject("Debugger").AddComponent<Debugger>();
            
            _debugger.OnDrawGizmosEvent -= PerformOnDrawGizmos;
            _debugger.OnDrawGizmosEvent += PerformOnDrawGizmos;
            
            _onDrawGizmosDebug = new List<Action>();
        }
        
        private static void PerformOnDrawGizmos() => _onDrawGizmosDebug.ForEach(a => a?.Invoke());
        public static void RegisterOnDrawGizmosDebug(Action debugAction) => _onDrawGizmosDebug.Add(debugAction);
    }
}