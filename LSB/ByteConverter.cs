using System.Collections;

namespace LSB
{
    public static class ByteConverter
    {
        public static BitArray ByteToBits(byte source)
        {
            var  bitArray = new BitArray(8);
            
            for (int i = 0; i < 8; i++)
            {
                bitArray[i] = (source >> i & 1) == 1;
            }
            
            return bitArray;
        }

        public static byte BitsToByte(BitArray source)
        {
            byte result = 0;
            
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] == true)
                {
                    result += (byte)Math.Pow(2, i);
                }    
            }
            
            return result;
        }
    }
}
