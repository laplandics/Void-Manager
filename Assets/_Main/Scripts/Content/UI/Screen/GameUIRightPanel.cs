using Managers.UIManagers;
using UnityEngine.UIElements;
using Utils;

namespace Content.UISpace
{
    public class GameUIRightPanel : UIElement
    {
        protected override void OnAdd()
        {
            ElementInstance = GetInstance(R.GameUIRightPanelAsset);
            G.Resolve<UI>().GetRoot().Add(ElementInstance);
            ElementInstance.Q<Button>(Constants.Names.UI_GAME_RIGHT_PANEL_CLOSE_BUTTON_NAME).clicked +=
                G.Resolve<GameUIManager>().RemoveRightPanel;

            G.Resolve<EntityUIManager>().AddEntityBuildTemplates();
        }
        
        protected override void OnRemove()
        {
            ElementInstance.Q<Button>(Constants.Names.UI_GAME_RIGHT_PANEL_CLOSE_BUTTON_NAME).clicked -=
                G.Resolve<GameUIManager>().RemoveRightPanel;
            G.Resolve<UI>().GetRoot().Remove(ElementInstance);
        }
    }
}