using Constants;
using UnityEngine;
using Utils;

namespace GameStates
{
    public class PauseState : GameState
    {
        public override void OnEnter()
        {
            //DEBUG ONLY
            Debug.LogWarning("GAME IS PAUSED");
            //DEBUG ONLY
            
            G.Resolve<Inputs>().ChangeActionsMap(ActionsMaps.UI);
            GamePause.PauseGame();
        }

        public override void OnKeyPressed(KeyboardKeys key)
        {
            switch (key)
            {
                case KeyboardKeys.Esc: EnterExplorationState(); break;
            }
        }

        private void EnterExplorationState() => G.Resolve<States>().ChangeState<ExplorationState>();
        
        public override void OnExit()
        {
            //DEBUG ONLY
            Debug.LogWarning("GAME IS UNPAUSED");
            //DEBUG ONLY
            
            GamePause.ResumeGame();
        }
    }
}