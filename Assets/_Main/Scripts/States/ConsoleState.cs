using UnityEngine.InputSystem;
using Utils;

namespace GameStates
{
    public class ConsoleState : GameState
    {
        public override void OnEnter()
        {
            G.Resolve<Inputs>().ChangeActionsMap(ActionsMaps.UI);
            G.Resolve<Inputs>().UITab.performed += EnterExplorationState;
        }

        public override void OnKeyPressed(KeyboardKeys key)
        {
            switch (key)
            {
                case KeyboardKeys.Enter: G.Resolve<Console>().EnterCommand(); break;
                case KeyboardKeys.Backspace: G.Resolve<Console>().RemoveLastChar(); break;
                case KeyboardKeys.Esc: break;
                
                default: G.Resolve<Console>().AddChar(key.ToString().ToUpper()); break;
            }
        }

        private void EnterExplorationState(InputAction.CallbackContext ctx)
        { G.Resolve<Console>().ForgetCommand(); G.Resolve<States>().ChangeState<ExplorationState>(); }

        public override void OnExit() { G.Resolve<Inputs>().UITab.performed -= EnterExplorationState; }
    }
}