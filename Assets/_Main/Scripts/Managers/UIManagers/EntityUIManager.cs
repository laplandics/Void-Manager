using System.Collections;
using System.Collections.Generic;
using Content.UISpace;
using Utils;

namespace Managers.UIManagers
{
    public class EntityUIManager
    {
        private List<string> _buildTemplateIds = new();
        
        public void Activate() {}

        public void AddEntityBuildTemplates() { G.Resolve<Coroutines>().Start(AddBuildTemplatesRoutine()); }

        public void RemoveEntityBuildTemplate(string templateId) { _buildTemplateIds.Remove(templateId); }
        
        private IEnumerator AddBuildTemplatesRoutine()
        {
            yield return null;
            new EntityUIBuildTemplate(Configs.EntityConfig.CoreStation).Add(out var template1);
            new EntityUIBuildTemplate(Configs.EntityConfig.EnergyStation).Add(out var template2);
            
            _buildTemplateIds.Add(template1);
            _buildTemplateIds.Add(template2);
        }
    }
}