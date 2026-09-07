using System;
using UnityEngine.UIElements;

namespace Content.UISpace
{
    public abstract class UIElement
    {
        public string ID;
        protected VisualElement ElementInstance;

        public void Add(out string id)
        {
            OnAdd();
            ID = Guid.NewGuid().ToString();
            G.Resolve<Utils.UI>().RegisterUIElement(this);
            ElementInstance.RegisterCallback<DetachFromPanelEvent>(DetachedCallback);
            id = ID;
        }

        private void RemoveActions()
        {
            ElementInstance.UnregisterCallback<DetachFromPanelEvent>(DetachedCallback);
            G.Resolve<Utils.UI>().UnregisterUIElement(this);
        }

        public void Remove() { RemoveActions(); OnRemove(); }
        public void Hide() { ElementInstance.style.display = DisplayStyle.None; OnHide(); }
        public void Show() { ElementInstance.style.display = DisplayStyle.Flex; OnShow(); }
        private void DetachedCallback(DetachFromPanelEvent evt) { RemoveActions(); OnDetach(); }

        protected static VisualElement GetInstance(VisualTreeAsset asset) => asset.Instantiate()
            .Q<VisualElement>(Constants.Names.UI_ELEMENT_NAME);
        
        protected abstract void OnAdd();
        protected abstract void OnRemove();
        protected virtual void OnDetach() {}
        protected virtual void OnHide() {}
        protected virtual void OnShow() {}
    }
}