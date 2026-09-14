using System;

namespace Tools
{
    public static class PositionHash
    {
        public static float Value01(int seed, float x, float y)
        {
            unchecked
            {
                int h = seed;
                h = h * 397 + BitConverter.SingleToInt32Bits(x);
                h = h * 397 + BitConverter.SingleToInt32Bits(y);
                h ^= h >> 15;
                h *= (int)0x2c1b3c6d;
                h ^= h >> 12;
                h *= (int)0x297a2d39;
                h ^= h >> 15;
                return (h & 0x7fffffff) / (float)int.MaxValue;
            }
        }
    }
}