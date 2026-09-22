using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class R
{
    public static GameObject Camera => Resources.Load<GameObject>("Prefabs/Camera");
    
    public static PanelSettings UISettingsAsset => Resources.Load<PanelSettings>("UI/Settings/UISettings");
    public static PanelSettings WorldUISettingsAsset => Resources.Load<PanelSettings>("UI/Settings/WorldUISettings");
    
    public static VisualTreeAsset GameUIRootAsset => Resources.Load<VisualTreeAsset>("UI/Root/GameUIRoot");
    public static VisualTreeAsset EntityWorldUIRootAsset => Resources.Load<VisualTreeAsset>("UI/Root/EntityWorldUIRoot");
    
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

    public static class UIAssetsLoader
    {
        private static Dictionary<string, VisualTreeAsset> _uiAssetsCache;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CacheUIAssets()
        {
            _uiAssetsCache = new Dictionary<string, VisualTreeAsset>();
            var assets = Resources.LoadAll<VisualTreeAsset>("UI/Assets");
            foreach (var asset in assets)
            { _uiAssetsCache.Add(asset.name, asset); }
        }
        
        public static VisualTreeAsset LoadUIAsset(string name) => _uiAssetsCache.GetValueOrDefault(name);
    }
}