using UnityEngine.InputSystem;
using Utils;

namespace GameStates
{
    public class ExplorationState : GameState
    {
        public override void OnEnter()
        {
            G.Resolve<Inputs>().ChangeActionsMap(ActionsMaps.Player);
            G.Resolve<Inputs>().PlayerTab.performed += EnterConsoleState;
        }

        private void EnterConsoleState(InputAction.CallbackContext ctx)
        { G.Resolve<States>().ChangeState<ConsoleState>(); }

        public override void OnKeyPressed(KeyboardKeys key) { }
        
        public override void OnExit() { G.Resolve<Inputs>().UITab.performed -= EnterConsoleState; }
    }
}