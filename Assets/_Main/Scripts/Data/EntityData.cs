using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public struct EntityData
    {
        public string id;
        public string type;
        public List<ComponentData> components;
    }
}