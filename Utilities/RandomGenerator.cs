using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AlgoTrading.Utilities
{
    public static class RandomGenerator
    {
        static RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

        public static float Generate(int low, int high)
        {
            if (low > high)
                throw new ArgumentOutOfRangeException("minValue");
            if (low == high) return low;
            long diff = high - low;
            while (true)
            {
                byte[] buffer = new byte[32];

                RNG.GetBytes(buffer);
                uint rand = BitConverter.ToUInt32(buffer, 0);

                long max = (1 + (long)uint.MaxValue);
                long remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(low + (rand % diff));
                }
            }
        }
    }
}
