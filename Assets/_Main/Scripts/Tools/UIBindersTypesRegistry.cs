using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UIBinders;
using UnityEngine;

namespace Tools
{
    public static class UIBindersTypesRegistry
    {
        private static readonly Dictionary<string, Type> TypesByName = new();

        private static bool IsInitialized { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (IsInitialized) return;
            TypesByName.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] typesInAssembly;

                try { typesInAssembly = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException ex)
                { typesInAssembly = ex.Types.Where(t => t != null).ToArray(); }

                foreach (var type in typesInAssembly)
                {
                    if (type == null || type.IsAbstract || type.IsInterface) continue;

                    var attribute = type.GetCustomAttribute<UIBinderAttribute>(inherit: false);
                    if (attribute == null) continue;

                    var key = type.Name;
                    if (TypesByName.TryGetValue(key, out var existingType))
                    {
                        Debug.LogError(
                            $"[EntityWorldUITypesRegistry] Дублирующийся ключ '{key}': " +
                            $"тип '{existingType.FullName}' уже зарегистрирован, " +
                            $"попытка перезаписи типом '{type.FullName}' пропущена.");
                        continue;
                    }

                    TypesByName.Add(key, type);
                }
            }

            IsInitialized = true;
        }

        private static Type GetType(string key) => TypesByName.TryGetValue(key, out var type)
        ? type : throw new KeyNotFoundException($"[UIBindersTypesRegistry] Тип с ключом '{key}' не найден в реестре.");

        public static UIBinder CreateInstance(string key, params object[] args)
        { var type = GetType(key); return (UIBinder)Activator.CreateInstance(type, args); }
    }
}