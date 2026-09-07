using System;
using System.Collections.Generic;
using Content.WorldSpace;
using Data;
using EntitySystems;
using Managers.UIManagers;
using UnityEngine.UIElements;
using Utils;

namespace Content.UISpace
{
    public class EntityUIBuildTemplate : UIElement
    {
        private readonly EntityData _buildingEntity;
        private readonly Dictionary<string, IDisposable> _subscriptionsMap = new();
        
        public EntityUIBuildTemplate(EntityData buildingEntity) { _buildingEntity = buildingEntity; }
        
        protected override void OnAdd()
        {
            ElementInstance = GetInstance(R.EntityUIBuildTemplateAsset);
            var root = G.Resolve<UI>().GetRoot();
            var container = root.Q<VisualElement>(Constants.Names.UI_STATIONS_BUILD_TEMPLATES_CONTAINER_NAME);
            container.Add(ElementInstance);
            
            ElementInstance.RegisterCallback<PointerDownEvent>(OnTemplateClicked);
        }

        private void OnTemplateClicked(PointerDownEvent _)
        {
            G.Resolve<GameUIManager>().HideRightPanel();
            
            var entity = G.Resolve<Entities>().New(_buildingEntity);
            var onBuildComponent = entity.AddComponent<EntityComponentFloat>(Configs.ComponentConfig.OnBuild(0));
            
            _subscriptionsMap[entity.id] = onBuildComponent.stream.Subscribe((newValue, _) =>
            { if (OnBuildSystem.CalculateStage(newValue) <= 0) return; OnEntityBuilt(entity.id); });
        }

        private void OnEntityBuilt(string id)
        {
            G.Resolve<GameUIManager>().ShowRightPanel();
            _subscriptionsMap[id].Dispose();
        }
        
        protected override void OnRemove()
        {
            var root = G.Resolve<UI>().GetRoot();
            var container = root.Q<VisualElement>(Constants.Names.UI_STATIONS_BUILD_TEMPLATES_CONTAINER_NAME);
            container.Remove(ElementInstance);
        }

        protected override void OnDetach()
        {
            ElementInstance.UnregisterCallback<PointerDownEvent>(OnTemplateClicked);
            foreach (var keyValuePair in _subscriptionsMap) { keyValuePair.Value.Dispose(); }
            _subscriptionsMap.Clear();
            
            G.Resolve<EntityUIManager>().RemoveEntityBuildTemplate(ID);
        }
    }
}