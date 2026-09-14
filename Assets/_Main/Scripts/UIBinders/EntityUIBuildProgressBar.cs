using System;
using Content.WorldSpace;
using EntitySystems;
using UnityEngine.UIElements;
using static Configs.ComponentConfig;

namespace UIBinders
{
    [UIBinder] public class EntityUIBuildProgressBar : UIBinder
    {
        private const string BUILD_PROGRESS_BAR_NAME = "BuildProgressBar";
        private ProgressBar _progressBar;
        private IDisposable _subscription;
        private VisualElement _root;
        
        public override void OnAdd()
        {
            if (Element.Info is not WorldUIInfo worldInfo) return;
            var entity = worldInfo.EntityOwner;
            if (entity == null) return;
            
            _progressBar = Element.ElementInstance.Q<ProgressBar>(BUILD_PROGRESS_BAR_NAME);
            _progressBar.highValue = OnBuildSystem.ENTITY_BUILD_STAGE_0_PROGRESS_MAX;
            _progressBar.value = _progressBar.highValue;
            
            var onBuildComponent = entity.GetComponent<EntityComponentFloat>(nameof(OnBuild));
            _subscription = onBuildComponent.stream.Subscribe((value, _) => UpdateProgressBar(value));
            
            _root = worldInfo.Root;
            _root.Add(Element.ElementInstance);
        }

        private void UpdateProgressBar(float progress)
        { _progressBar.value = OnBuildSystem.ENTITY_BUILD_STAGE_0_PROGRESS_MAX - progress; }
        
        public override void OnRemove()
        {
            _root.Remove(Element.ElementInstance);
        }

        public override void OnDetached()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}