using Data;

namespace Tools
{
    public static class InitialDataCreator
    {
        public static GameData CreateGameData()
        {
            var data = new GameData();
            data.vSync = Configs.GameConfig.VSYNC;
            data.fps = Configs.GameConfig.FPS;
            
            data.seed = Seed.Generate();
            
            data.entities = new[]
            { Configs.EntityConfig.Cursor };

            return data;
        }
    }
}