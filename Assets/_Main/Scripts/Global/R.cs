using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class R
{
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

    public static class SpritesLoader
    {
        private static Dictionary<string, Sprite> _spritesCache;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CacheSprites()
        {
            _spritesCache = new Dictionary<string, Sprite>();
            var sprites = Resources.LoadAll<Sprite>("Sprites");
            foreach (var sprite in sprites)
            { _spritesCache.Add(sprite.name, sprite); }
        }

        public static Sprite LoadSprite(string name) => _spritesCache.GetValueOrDefault(name);
    }
}