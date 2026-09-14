using System;
using Constants;
using UIBinders;
using UnityEngine.InputSystem;
using Utils;

namespace GameStates
{
    public class BuildMenuState : GameState
    {
        private const string BUILD_MENU_STATE_HINTS = "RETURN - ESC; CHOOSE - ENTER";
        
        public event Action<BuildSelectionInfo> OnSelectionChanged;
        
        private string _buildMenuId;
        private string _hintsPanelId;
        private BuildSelectionInfo _selectionInfo;
        
        public override void OnEnter()
        {
            _selectionInfo = new BuildSelectionInfo();
            
            G.Resolve<Inputs>().ChangeActionsMap(ActionsMaps.UI);
            G.Resolve<Inputs>().UISelectUp.performed += OnSelectUp;
            G.Resolve<Inputs>().UISelectDown.performed += OnSelectDown;
            
            _buildMenuId = G.Resolve<UI>().Add(nameof(GameUIBuildMenu), info: null);
            _hintsPanelId = G.Resolve<UI>().Add(nameof(GameUIHintsPanel), new UIInfo(BUILD_MENU_STATE_HINTS));
        }

        private void OnSelectUp(InputAction.CallbackContext ctx)
        {
            _selectionInfo.IndexChange = -1;
            OnSelectionChanged?.Invoke(_selectionInfo);
        }

        private void OnSelectDown(InputAction.CallbackContext ctx)
        {
            _selectionInfo.IndexChange = 1;
            OnSelectionChanged?.Invoke(_selectionInfo);
        }

        public override void OnKeyPressed(KeyboardKeys key)
        {
            switch (key)
            {
                case KeyboardKeys.Esc: EnterExplorationState(); break;
                case KeyboardKeys.Enter: EnterBuildConfirmationState(); break;
            }
        }

        private void EnterExplorationState() => G.Resolve<States>().ChangeState<ExplorationState>();
        private void EnterBuildConfirmationState()
        {
            var param = _selectionInfo.SelectedIndex;
            var parameters = new StateParameters(param);
            G.Resolve<States>().ChangeState<BuildConfirmationState>(parameters);
        }

        public override void OnExit()
        {
            G.Resolve<Inputs>().UISelectUp.performed -= OnSelectUp;
            G.Resolve<Inputs>().UISelectDown.performed -= OnSelectDown;
            
            G.Resolve<UI>().Remove(_hintsPanelId);
            G.Resolve<UI>().Remove(_buildMenuId);
        }
    }

    public class BuildSelectionInfo { public int IndexChange; public int SelectedIndex; }
}