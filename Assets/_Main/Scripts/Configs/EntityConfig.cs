using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Configs
{
    public static class EntityConfig
    {
        public static readonly EntityData Cursor = new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(Cursor),
            components = new List<ComponentData>
            {
                ComponentConfig.Sprite(nameof(Cursor)),
                ComponentConfig.CameraFollow(),
                ComponentConfig.ColorTint(Color.white),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.InputMovement(),
                ComponentConfig.RenderOrder(100)
            }
        };
        
        public static EntityData Star(Vector3 position, Color color) => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(Star),
            components = new List<ComponentData>
            {
                ComponentConfig.Sprite("Pixel"),
                ComponentConfig.ColorTint(color),
                ComponentConfig.Position(position),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.RenderOrder(0)
            }
        };

        public static EntityData Asteroid(string type, string spriteName, Vector3 position) => new()
        {
            id = Guid.NewGuid().ToString(),
            type = type,
            components = new List<ComponentData>
            {
                ComponentConfig.Sprite(spriteName),
                ComponentConfig.ColorTint(Color.gray3),
                ComponentConfig.Position(position),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.RenderOrder(5)
            }
        };
    }
}
