using System.Collections;
using System.Collections.Generic;
using Content.UISpace;
using Helpers;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Utils
{
    public class UI
    {
        private UIHolder _uiHolder;
        private VisualElement _root;
        private bool _isUIReady;
        
        private readonly Dictionary<string, WorldUIInfo> _worldUIMap = new();
        private readonly Dictionary<string, UIElement> _uiElementsMap = new();
        
        public IEnumerator SetUI()
        {
            _isUIReady = false;
            
            if (_uiHolder != null) Object.Destroy(_uiHolder.gameObject);
            _uiHolder = new GameObject("UI").AddComponent<UIHolder>();
            
            var renderer = _uiHolder.gameObject.AddComponent<PanelRenderer>();
            renderer.panelSettings = R.UISettingsAsset;
            renderer.visualTreeAsset = R.GameUIRootAsset;
            
            renderer.UnregisterUIReloadCallback(OnUIReload);
            renderer.RegisterUIReloadCallback(OnUIReload);
            
            _uiHolder.uiRenderer = renderer;
            yield return new WaitUntil(() => _isUIReady);
        }

        public void SetWorldUI(WorldUIInfo info)
        {
            if (!_worldUIMap.TryAdd(info.ID, info)) return;
            var worldUIObject = new GameObject(info.Name);
            worldUIObject.transform.SetParent(info.Parent);
            worldUIObject.transform.localPosition = new Vector3(0f, 0f, -1f);
            
            var worldUIHolder = worldUIObject.AddComponent<UIHolder>();
            info.Holder = worldUIHolder;
            
            var renderer = worldUIObject.AddComponent<PanelRenderer>();
            info.Renderer = renderer;
            info.Holder.uiRenderer = renderer;
            
            renderer.panelSettings = R.WorldUISettingsAsset;
            renderer.visualTreeAsset = info.Asset;
            
            renderer.UnregisterUIReloadCallback(OnWorldUIReload);
            renderer.RegisterUIReloadCallback(OnWorldUIReload);

            info.IsReady.Value = false;
        }

        public void RemoveWorldUI(WorldUIInfo info) => _worldUIMap.Remove(info.ID);
        
        public VisualElement GetRoot() => _root;

        public bool TryGetUIElement(string id, out UIElement uiElement) => _uiElementsMap.TryGetValue(id, out uiElement);
        
        public void RegisterUIElement(UIElement element) => _uiElementsMap[element.ID] = element;
        public void UnregisterUIElement(UIElement element) => _uiElementsMap.Remove(element.ID);
        
        private void OnUIReload(PanelRenderer _, VisualElement root) { _root = root; _isUIReady = true; }

        private void OnWorldUIReload(PanelRenderer renderer, VisualElement root)
        {
            foreach (var (_, uiInfo) in _worldUIMap)
            {
                if (renderer != uiInfo.Renderer) continue;
                
                uiInfo.Root = root;
                uiInfo.IsReady.Value = true;
            }
        }
    }
}