using UnityEngine;
using UnityEngine.UIElements;

public static class R
{
    public static Sprite LoadSprite(string name) => Resources.Load<Sprite>($"Sprites/{name}");
    public static GameObject LoadColliderObject(string name) => Resources.Load<GameObject>($"Colliders/{name}");
    
    public static PanelSettings UISettingsAsset => Resources.Load<PanelSettings>("UI/UISettings");
    public static PanelSettings WorldUISettingsAsset => Resources.Load<PanelSettings>("UI/WorldUISettings");
    
    public static VisualTreeAsset GameUIRootAsset => Resources.Load<VisualTreeAsset>("UI/GameUI/GameUIRoot");
    public static VisualTreeAsset GameUITopPanelAsset => Resources.Load<VisualTreeAsset>("UI/GameUI/GameUITopPanel");
    public static VisualTreeAsset GameUIRightPanelAsset => Resources.Load<VisualTreeAsset>("UI/GameUI/GameUIRightPanel");
    public static VisualTreeAsset GameUIOpenRightPanelButtonAsset => Resources.Load<VisualTreeAsset>("UI/GameUI/GameUIOpenRightPanelButton");
    
    public static VisualTreeAsset EntityWorldUIRootAsset => Resources.Load<VisualTreeAsset>("UI/EntityUI/EntityWorldUIRoot");
    public static VisualTreeAsset EntityWorldUIOnBuildProgressBarAsset => Resources.Load<VisualTreeAsset>("UI/EntityUI/EntityWorldUIOnBuildProgressBar");
    
    public static VisualTreeAsset EntityUIBuildTemplateAsset => Resources.Load<VisualTreeAsset>("UI/EntityUI/EntityUIBuildTemplate");
    
}