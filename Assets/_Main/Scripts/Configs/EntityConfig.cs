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
            components = new[]
            {
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Sprite(nameof(Cursor)),
                ComponentConfig.ColorTint(new Color(0.9f, 0.9f, 0.9f, 1.0f)),
                ComponentConfig.RenderOrder(100),
                ComponentConfig.InputMovement(),
                ComponentConfig.CameraFollow()
            }
        };

        public static EntityData Frame(Vector3 position) => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(Frame),
            components = new[]
            {
                ComponentConfig.Position(position),
                ComponentConfig.Sprite(nameof(Frame)),
                ComponentConfig.ColorTint(new Color(1f, 0.92f, 0.016f, 0.5f)),
                ComponentConfig.RenderOrder(100)
            }
        };
        
        public static EntityData Star() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(Star),
            components = new[]
            {
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.Sprite(nameof(Star)),
                ComponentConfig.ColorTint(Color.white),
                ComponentConfig.RenderOrder(0)
            }
        };

        public static EntityData Asteroid() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(Asteroid),
            components = new[]
            {
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.Sprite(nameof(Asteroid)),
                ComponentConfig.ColorTint(Color.slateGray),
                ComponentConfig.RenderOrder(5)
            }
        };
        
        public static EntityData CoreStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(CoreStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(CoreStation)),
                ComponentConfig.ColorTint(Color.white),
                ComponentConfig.RenderOrder(10),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>())
            }
        };

        public static EntityData EnergyStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(EnergyStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(EnergyStation)),
                ComponentConfig.ColorTint(Color.cyan),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };

        public static EntityData CitadelStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(CitadelStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(CitadelStation)),
                ComponentConfig.ColorTint(Color.green),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };

        public static EntityData GatheringStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(GatheringStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(GatheringStation)),
                ComponentConfig.ColorTint(Color.brown),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };

        public static EntityData HabitateStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(HabitateStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(HabitateStation)),
                ComponentConfig.ColorTint(Color.blueViolet),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };

        public static EntityData MiningStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(MiningStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(MiningStation)),
                ComponentConfig.ColorTint(Color.chocolate),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };
        
        public static EntityData ScienceStation() => new()
        {
            id = Guid.NewGuid().ToString(),
            type = nameof(ScienceStation),
            components = new[]
            {
                ComponentConfig.Sprite(nameof(ScienceStation)),
                ComponentConfig.ColorTint(Color.royalBlue),
                ComponentConfig.Position(Vector3.zero),
                ComponentConfig.Rotation(Vector3.zero),
                ComponentConfig.EntityUI(new List<string>()),
                ComponentConfig.RenderOrder(10)
            }
        };
        
        public static EntityData StationByIndex(int index)
        {
            return index switch
            {
                0 => CoreStation(),
                1 => EnergyStation(),
                2 => CitadelStation(),
                3 => GatheringStation(),
                4 => HabitateStation(),
                5 => MiningStation(),
                6 => ScienceStation(),
                _ => default
            };
        }

        public static string[] StationNames => new[]
        {
            nameof(CoreStation),
            nameof(EnergyStation),
            nameof(CitadelStation),
            nameof(GatheringStation),
            nameof(HabitateStation),
            nameof(MiningStation),
            nameof(ScienceStation)
        };
    }
}
