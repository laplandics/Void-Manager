using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Data;
using Helpers;
using Tools;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Content.WorldSpace
{
    [Serializable]
    public class Entity
    {
        public EntityObject entityObject;
        
        public string id;
        public string type;
        
        [SerializeReference] public List<EntityComponent> components = new();
        
        public EntityActions Actions { get; private set; }
        
        public void OnNew(EntityData data, int index)
        {
            entityObject = new GameObject($"{data.type} {index + 1}").AddComponent<EntityObject>();
            entityObject.entity = this;
            
            id = data.id;
            type = data.type;

            Actions = new EntityActions(this);
            
            foreach (var componentEntry in data.components)
            { AddComponent(componentEntry, silent: true); }
            
            G.Resolve<Systems>().Update(id);
        }
        
        public void AddComponent(string componentEntry, bool silent = false)
        {
            var componentTag = componentEntry.Split(Separators.ENTITY_COMPONENT_DATA_SEPARATOR)[1];
            var sameComponent = components.FirstOrDefault(c => c.tag == componentTag);
            if (sameComponent != null) return;
            
            components.Add(EntityComponentDataConverter.FromEntry(componentEntry));

            if (silent) return;
            G.Resolve<Systems>().Update(id);
        }
        
        public T AddComponent<T>(string componentEntry, bool silent = false) where T : EntityComponent
        {
            var componentTag = componentEntry.Split(Separators.ENTITY_COMPONENT_DATA_SEPARATOR)[1];
            var sameComponent = components.FirstOrDefault(c => c.tag == componentTag);
            if (sameComponent != null) return null;

            var component = EntityComponentDataConverter.FromEntry(componentEntry);
            components.Add(component);

            if (silent) return (T)component;
            G.Resolve<Systems>().Update(id);
            
            return (T)component;
        }

        public void RemoveComponent(string componentTag, bool silent = false)
        {
            var componentToRemove = components.FirstOrDefault(c => c.tag == componentTag);
            if (componentToRemove == null) return;
            
            components.Remove(componentToRemove);

            if (silent) return;
            G.Resolve<Systems>().Update(id);
        }
        
        public List<string> ClearComponents(params string[] excludeTags)
        {
            excludeTags ??= Array.Empty<string>();
            var entries = new List<string>();
            for (var i = components.Count - 1; i >= 0; i--)
            {
                if (excludeTags.Contains(components[i].tag)) continue;
                var entry = EntityComponentDataConverter.ToEntry(components[i]);
                entries.Add(entry);
                RemoveComponent(components[i].tag, silent: true);
            }
            
            G.Resolve<Systems>().Update(id);
            return entries;
        }

        public T GetComponent<T>(string componentTag) where T : EntityComponent
        {
            foreach (var component in components)
            {
                if (component is not T typedComponent) continue;
                if (typedComponent.tag != componentTag) continue;
                return typedComponent;
            }
            
            return null;
        }
        
        public EntityData OnDelete()
        {
            var data = new EntityData();
            data.id = id;
            data.type = type;
            data.components = ClearComponents().ToArray();
            
            Object.Destroy(entityObject.gameObject);
            return data;
        }
    }
}