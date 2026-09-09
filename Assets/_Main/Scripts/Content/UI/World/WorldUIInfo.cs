using System.Collections.Generic;
using Content.UISpace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Helpers
{
    public class WorldUIInfo
    {
        public readonly string Name;
        public readonly string ID;
        public readonly Transform Parent;
        public readonly VisualTreeAsset Asset;
        public readonly Reactive<bool> IsReady = new();

        public WorldUIInfo(string name, string id, VisualTreeAsset asset, Transform parent)
        {
            Name = name;
            ID = id;
            Asset = asset;
            Parent = parent;
        }

        public UIHolder Holder;
        public VisualElement Root;
        public PanelRenderer Renderer;

        public readonly Dictionary<string, string> AttachedElementsNamesIdsMap = new();
    }
}