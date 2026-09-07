using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Configs
{
    public static class EntityConfig
    {
        public static readonly EntityData CoreStation = new()
        {
            type = nameof(CoreStation),
            components = new[]
            {
                ComponentConfig.MaxHp(10),
                ComponentConfig.CurrentHp(10),
                ComponentConfig.Sprite(nameof(CoreStation)),
                ComponentConfig.ColorTint(Color.white),
                ComponentConfig.Visibility(false),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.Collision(new List<string>()),
                ComponentConfig.RenderOrder(0)
            }
        };

        public static readonly EntityData EnergyStation = new()
        {
            type = nameof(EnergyStation),
            components = new[]
            {
                ComponentConfig.MaxHp(5),
                ComponentConfig.CurrentHp(5),
                ComponentConfig.Sprite(nameof(EnergyStation)),
                ComponentConfig.ColorTint(Color.white),
                ComponentConfig.Visibility(false),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.Collision(new List<string>()),
                ComponentConfig.RenderOrder(0)
            }
        };
    }
}