using Content.UISpace;

namespace UIBinders
{
    public abstract class UIBinder
    {
        protected UIElement Element;
        
        public void Bind(UIElement element) { Element = element; }

        public virtual void OnHide() {}
        public virtual void OnShow() {}
        
        public virtual void OnAdd() {}
        public virtual void OnRemove() {}
        public virtual void OnDetached() {}
    }
}