using System;

namespace Tools
{
    public static class Seed
    {
        private static string _master;
        
        public static string Generate() => Guid.NewGuid().ToString("N");
        
        public static void SetMasterSeed(string masterSeed) => _master = masterSeed;

        public static int GetSeed(string channel)
        {
            unchecked
            {
                uint h = 2166136261;
                foreach (var c in _master + "|" + channel)
                {
                    h ^= c;
                    h *= 16777619;
                }
                
                return (int)h;
            }
        }
    }
}