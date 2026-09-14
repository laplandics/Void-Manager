using Constants;
using Data;
using UnityEngine;

namespace Tools
{
    public static class InitialDataCreator
    {
        public static GameData CreateGameData()
        {
            var data = new GameData();
            data.vSync = Configs.GameConfig.VSYNC;
            data.fps = Configs.GameConfig.FPS;
            
            //DEBUG ONLY
            Debug.LogWarning("DEBUG STARFIELD SEED ASSIGMENT");
            data.seed = $"0000{Separators.SEED_PART_SEPARATOR}1111{Separators.SEED_PART_SEPARATOR}2222" +
                        $"{Separators.SEED_MAP_SEPARATOR}" +
                        $"3333{Separators.SEED_PART_SEPARATOR}4444{Separators.SEED_PART_SEPARATOR}5555" +
                        $"{Separators.SEED_MAP_SEPARATOR}" +
                        $"6666{Separators.SEED_PART_SEPARATOR}7777{Separators.SEED_PART_SEPARATOR}8888";
            //DEBUG ONLY
            
            data.entities = new[]
            { Configs.EntityConfig.Cursor };

            return data;
        }
    }
}