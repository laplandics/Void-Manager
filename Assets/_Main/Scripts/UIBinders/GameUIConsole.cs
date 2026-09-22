using UnityEngine.UIElements;
using Utils;

namespace UIBinders
{
    [UIBinder] public class GameUIConsole : UIBinder
    {
        private Label _commandLabel;
        
        public override void OnAdd()
        {
            var root = G.Resolve<UI>().GetRoot();
            _commandLabel = Element.ElementInstance.Q<Label>("CommandLabel");
            
            root.Add(Element.ElementInstance);
            G.Resolve<Console>().Command.Subscribe((value, _) => OnCommandChanged(value));
        }

        private void OnCommandChanged(string command) { _commandLabel.text = command; }
        
        public override void OnRemove()
        {
            var root = G.Resolve<UI>().GetRoot();
            root.Remove(Element.ElementInstance);
        }
    }
}