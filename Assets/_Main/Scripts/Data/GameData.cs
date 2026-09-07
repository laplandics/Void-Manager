using System;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public int vSync;
        public int fps;
        
        public EntityData[] entities;
    }
}