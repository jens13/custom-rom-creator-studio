//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Zip
{
    public class Crc32Hash
    {
        public const uint DefaultPolynomial = 0xedb88320;
        public const uint DefaultSeed = 0xffffffff;

        private static readonly uint[] _defaultTable;

        static Crc32Hash()
        {
            _defaultTable = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                var entry = (uint) i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ DefaultPolynomial;
                    else
                        entry = entry >> 1;
                _defaultTable[i] = entry;
            }
        }

        public static uint CalculateHash(uint seed, byte[] buffer, int length)
        {
            uint crc = seed;
            for (int i = 0; i < length; i++)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ _defaultTable[buffer[i] ^ crc & 0xff];
                }
            }
            return crc;
        }
    }
}