using Content.UISpace;
using Utils;

namespace Managers.UIManagers
{
    public class GameUIManager
    {
        private string _topPanelId;
        private string _openRightPanelButtonId;
        private string _rightPanelId;
        
        public void Activate()
        {
            new GameUITopPanel().Add(out _topPanelId);
            AddOpenRightPanelButton();
        }

        public void AddRightPanel()
        {
            new GameUIRightPanel().Add(out _rightPanelId);
            RemoveOpenRightPanelButton();
        }

        public void RemoveRightPanel()
        {
            if (string.IsNullOrEmpty(_rightPanelId)) return;
            G.Resolve<UI>().TryGetUIElement(_rightPanelId, out var rightPanel);
            rightPanel.Remove();
            _rightPanelId = null;

            AddOpenRightPanelButton();
        }

        public void HideRightPanel()
        {
            if (string.IsNullOrEmpty(_rightPanelId)) return;
            G.Resolve<UI>().TryGetUIElement(_rightPanelId, out var rightPanel);
            rightPanel.Hide();
        }

        public void ShowRightPanel()
        {
            if (string.IsNullOrEmpty(_rightPanelId)) return;
            G.Resolve<UI>().TryGetUIElement(_rightPanelId, out var rightPanel);
            rightPanel.Show();
        }
        
        private void AddOpenRightPanelButton()
        {
            new GameUIOpenRightPanelButton().Add(out _openRightPanelButtonId);
        }

        private void RemoveOpenRightPanelButton()
        {
            if (string.IsNullOrEmpty(_openRightPanelButtonId)) return;
            G.Resolve<UI>().TryGetUIElement(_openRightPanelButtonId, out var rightPanelButton);
            rightPanelButton.Remove();
        }
    }
}