using System;

namespace Data
{
    [Serializable]
    public struct EntityData
    {
        public string id;
        public string type;
        public string[] components;
    }
}