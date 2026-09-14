using System;
using UIBinders;
using UnityEngine.UIElements;

namespace Content.UISpace
{
    public class UIElement
    {
        public string ID;
        public VisualElement ElementInstance;
        public UIBinder Binder;
        
        public UIInfo Info;
        
        private const string UI_ELEMENT_ROOT_CONTAINER = "UIElement";
        private readonly string _elementName;

        public UIElement(string elementName, UIInfo info = null)
        {
            _elementName = elementName;
            Info = info;
        }
        
        public string Add()
        {
            ID = Guid.NewGuid().ToString();

            var elementAsset = R.UIAssetsLoader.LoadUIAsset(_elementName);
            if (elementAsset == null) return null;
            var template = elementAsset.Instantiate();
            ElementInstance = template.Q<VisualElement>(UI_ELEMENT_ROOT_CONTAINER);

            Binder = Tools.UIBindersTypesRegistry.CreateInstance(_elementName);
            Binder.Bind(this);
            
            G.Resolve<Utils.UI>().RegisterUIElement(this);
            
            Binder.OnAdd();
            return ID;
        }
        
        public void Remove() { RemoveActions(); Binder.OnRemove(); }
        private void DetachedCallback(DetachFromPanelEvent evt) { RemoveActions(); Binder.OnDetached(); }

        private void RemoveActions()
        {
            ElementInstance.UnregisterCallback<DetachFromPanelEvent>(DetachedCallback);
            G.Resolve<Utils.UI>().UnregisterUIElement(this);
        }
        
        public virtual void Hide() { ElementInstance.style.display = DisplayStyle.None; Binder.OnHide(); }
        public virtual void Show() { ElementInstance.style.display = DisplayStyle.Flex; Binder.OnShow(); }
    }
}