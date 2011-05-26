//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.IO;

namespace CrcStudio.Zip
{
    public class Crc32Hash
    {
        private const int BufferSize = 4096;

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
        public static uint CalculateHash(string fileSystemPath)
        {
            uint crc32 = Crc32Hash.DefaultSeed;
            using (var stream = File.OpenRead(fileSystemPath))
            {
                int bytesRead;
                var buffer = new byte[BufferSize];
                do
                {
                    bytesRead = stream.Read(buffer, 0, BufferSize);
                    if (bytesRead <= 0) break;
                    crc32 = Crc32Hash.CalculateHash(crc32, buffer, bytesRead);
                } while (bytesRead == BufferSize);
                crc32 ^= Crc32Hash.DefaultSeed;
            }
            return crc32;
        }
    }
}