namespace Content.UISpace
{
    public class GameUITopPanel : UIElement
    {
        protected override void OnAdd()
        {
            ElementInstance = GetInstance(R.GameUITopPanelAsset);
            G.Resolve<Utils.UI>().GetRoot().Add(ElementInstance);
        }

        protected override void OnRemove()
        {
            G.Resolve<Utils.UI>().GetRoot().Remove(ElementInstance);
        }
    }
}