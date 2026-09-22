using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Converters;
using Data;
using Helpers;
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
        
        public void OnNew(EntityData data, Transform parent)
        {
            entityObject = new GameObject(data.type).AddComponent<EntityObject>();
            if (parent != null) entityObject.transform.SetParent(parent);
            entityObject.entity = this;
            
            id = data.id;
            type = data.type;

            Actions = new EntityActions(this);
            
            foreach (var componentEntry in data.components)
            { AddComponent(componentEntry, silent: true); }
            
            G.Resolve<Systems>().Update(id);
        }
        
        public void AddComponent(ComponentData componentData, bool silent = false)
        {
            var componentTag = componentData.tag;
            var sameComponent = components.FirstOrDefault(c => c.tag == componentTag);
            if (sameComponent != null) return;
            
            components.Add(EntityComponentDataConverter.FromData(componentData));

            if (silent) return;
            G.Resolve<Systems>().Update(id);
        }
        
        public T AddComponent<T>(ComponentData componentData, bool silent = false) where T : EntityComponent
        {
            var componentTag = componentData.tag;
            var sameComponent = components.FirstOrDefault(c => c.tag == componentTag);
            if (sameComponent != null) return null;

            var component = EntityComponentDataConverter.FromData(componentData);
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
        
        public List<ComponentData> ClearComponents(params string[] componentsToClear)
        {
            if (componentsToClear is not { Length: > 0 }) return null;
            
            var entries = new List<ComponentData>();
            for (var i = components.Count - 1; i >= 0; i--)
            {
                if (!componentsToClear.Contains(components[i].tag)) continue;
                var entry = EntityComponentDataConverter.ToData(components[i]);
                entries.Add(entry);
                RemoveComponent(components[i].tag, silent: true);
            }
            
            G.Resolve<Systems>().Update(id);
            return entries;
        }
        
        public List<ComponentData> ClearAllComponents()
        {
            var entries = new List<ComponentData>();
            for (var i = components.Count - 1; i >= 0; i--)
            {
                var entry = EntityComponentDataConverter.ToData(components[i]);
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

        public bool HasComponent(string componentTag) => components.Any(component => component.tag == componentTag);
        
        public EntityData OnDelete()
        {
            var data = new EntityData();
            data.id = id;
            data.type = type;
            data.components = ClearAllComponents();
            
            Object.Destroy(entityObject.gameObject);
            return data;
        }
    }
}