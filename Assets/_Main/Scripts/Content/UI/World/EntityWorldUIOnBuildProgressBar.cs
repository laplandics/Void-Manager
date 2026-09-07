using System;
using Constants;
using Content.WorldSpace;
using Helpers;
using UnityEngine.UIElements;
using static Configs.ComponentConfig;

namespace Content.UISpace
{
    [EntityWorldUI]
    public class EntityWorldUIOnBuildProgressBar : UIElement
    {
        private readonly WorldUIInfo _info;
        private readonly EntityComponentFloat _buildComponent;
        private IDisposable _subscription;
        
        public EntityWorldUIOnBuildProgressBar(WorldUIInfo info)
        {
            _info = info;
            _buildComponent = info.Parent.GetComponent<EntityObject>().entity
                .GetComponent<EntityComponentFloat>(nameof(OnBuild));
        }
        
        protected override void OnAdd()
        {
            ElementInstance = GetInstance(R.EntityWorldUIOnBuildProgressBarAsset);
            var progressBar = ElementInstance.Q<ProgressBar>("EntityProgressBar");
            progressBar.highValue = Values.ENTITY_BUILD_STAGE_1_PROGRESS_MAX;
            _subscription = _buildComponent.stream.SubscribeSilently((newValue, _) => { progressBar.value = newValue; });
            
            _info.Root.Add(ElementInstance);
        }

        protected override void OnRemove()
        {
            _info.Root.Remove(ElementInstance);
            
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}