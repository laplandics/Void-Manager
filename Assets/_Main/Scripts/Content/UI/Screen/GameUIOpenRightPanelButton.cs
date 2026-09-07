using Constants;
using Managers.UIManagers;
using UnityEngine.UIElements;
using Utils;

namespace Content.UISpace
{
    public class GameUIOpenRightPanelButton : UIElement
    {
        private Button _openRightPanelButton;
        
        protected override void OnAdd()
        {
            ElementInstance = GetInstance(R.GameUIOpenRightPanelButtonAsset);
            _openRightPanelButton = ElementInstance.Q<Button>(Names.UI_GAME_RIGHT_PANEL_OPEN_BUTTON_NAME);
            _openRightPanelButton.clicked += G.Resolve<GameUIManager>().AddRightPanel;
            
            G.Resolve<UI>().GetRoot().Add(ElementInstance);
        }
        
        protected override void OnRemove()
        {
            _openRightPanelButton.clicked -= G.Resolve<GameUIManager>().AddRightPanel;
            G.Resolve<UI>().GetRoot().Remove(ElementInstance);
        }
    }
}