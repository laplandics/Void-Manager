using UnityEngine.UIElements;
using Utils;

namespace UIBinders
{
    [UIBinder] public class GameUIHintsPanel : UIBinder
    {
        private const string HINTS_LABEL_NAME = "HintsLabel";
        
        public override void OnAdd()
        {
            var info = Element.Info;
            var labelText = info?.Parameters[0].ToString();
            labelText ??= "NO HINTS FOUND";
            
            var label = Element.ElementInstance.Q<Label>(HINTS_LABEL_NAME);
            label.text = labelText;
            
            var root = G.Resolve<UI>().GetRoot();
            root.Add(Element.ElementInstance);
        }
        
        public override void OnRemove()
        {
            var root = G.Resolve<UI>().GetRoot();
            root.Remove(Element.ElementInstance);
        }
    }
}