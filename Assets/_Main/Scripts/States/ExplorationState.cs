using Constants;
using UIBinders;
using Utils;

namespace GameStates
{
    public class ExplorationState : GameState
    {
        private const string EXPLORATION_STATE_HINTS = "MOVE CURSOR - ARROWS UP/DOWN/LEFT/RIGHT; " +
                                                      "BUILD MODE - B; EXAMINE - ENTER; SHOW/HIDE HINTS - H";
        
        private bool _hintsHidden;
        private string _hintsPanelId;
        
        public override void OnEnter()
        {
            G.Resolve<Inputs>().ChangeActionsMap(ActionsMaps.Player);

            _hintsPanelId = G.Resolve<UI>().Add(nameof(GameUIHintsPanel), new UIInfo(EXPLORATION_STATE_HINTS));
            
            if (!_hintsHidden) return;
            _hintsHidden = false;
            ShowHideHints();
        }

        public override void OnKeyPressed(KeyboardKeys key)
        {
            switch (key)
            {
                case KeyboardKeys.B: EnterBuildState(); break;
                case KeyboardKeys.H: ShowHideHints(); break;
                case KeyboardKeys.Esc: EnterPauseState(); break;
            }
        }

        private void EnterBuildState() => G.Resolve<States>().ChangeState<BuildMenuState>();
        private void EnterPauseState() => G.Resolve<States>().ChangeState<PauseState>();
        
        private void ShowHideHints()
        {
            if (!G.Resolve<UI>().TryGetUIElement(_hintsPanelId, out var panel)) return;
            if (_hintsHidden) { panel.Show(); _hintsHidden = false; }
            else { panel.Hide(); _hintsHidden = true; }
        }

        public override void OnExit() { G.Resolve<UI>().Remove(_hintsPanelId); }
    }
}