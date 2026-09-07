using System;
using UnityEngine;

namespace Content.WorldSpace
{
    [Serializable] public abstract class EntityComponent { public string tag; }
    
    [Serializable] public class EntityComponentEmpty : EntityComponent {}
    
    [Serializable] public class EntityComponentInt : EntityComponent
    { [SerializeReference] public Reactive<int> stream; }
    
    [Serializable] public class EntityComponentFloat : EntityComponent
    { [SerializeReference] public Reactive<float> stream; }
    
    [Serializable] public class EntityComponentBool : EntityComponent
    { [SerializeReference] public Reactive<bool> stream; }
    
    [Serializable] public class EntityComponentString : EntityComponent
    { [SerializeReference] public Reactive<string> stream; }
    
    [Serializable] public class EntityComponentVector2 : EntityComponent
    { [SerializeReference] public Reactive<Vector2> stream; }
    
    [Serializable] public class EntityComponentVector3 : EntityComponent
    { [SerializeReference] public Reactive<Vector3> stream; }
    
    [Serializable] public class EntityComponentColor : EntityComponent
    { [SerializeReference] public Reactive<Color> stream; }
    
    [Serializable] public class EntityComponentStringList : EntityComponent
    { [SerializeReference] public ReactiveList<string> stream; }
}