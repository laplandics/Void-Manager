using System;
using UnityEngine;

namespace Workflow.Debug
{
    public class Debugger : MonoBehaviour
    {
        public event Action OnDrawGizmosEvent;
        
        private void OnDrawGizmos() => OnDrawGizmosEvent?.Invoke();
    }
}