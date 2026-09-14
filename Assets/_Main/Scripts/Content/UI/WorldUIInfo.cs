using System.Collections.Generic;
using Content.UISpace;
using Content.WorldSpace;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldUIInfo : UIInfo
{
    public readonly string Name;
    public readonly string ID;
    public readonly Transform Parent;
    public readonly VisualTreeAsset RootAsset;
    public readonly Reactive<bool> IsReady = new();
    public readonly Entity EntityOwner;
    
    public WorldUIInfo(string name, string id, VisualTreeAsset rootAsset,
    Transform parent, Entity entityOwner, params object[] parameters) : base(parameters)
    { Name = name; ID = id; RootAsset = rootAsset; Parent = parent; EntityOwner = entityOwner; }

    public UIHolder Holder;
    public VisualElement Root;
    public PanelRenderer Renderer;

    public readonly Dictionary<string, string> AttachedElementsNamesIdsMap = new();
}