//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;

namespace CrcStudio.Zip
{
    public static class ZipExtensions
    {
        public static uint ReadUInt(this Stream stream)
        {
            var buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4) throw new EndOfStreamException();
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static ushort ReadUShort(this Stream stream)
        {
            var buffer = new byte[2];
            if (stream.Read(buffer, 0, 2) != 2) throw new EndOfStreamException();
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static string ReadString(this Stream stream, int numberOfBytes, Encoding encoding)
        {
            var buffer = new byte[numberOfBytes];
            if (stream.Read(buffer, 0, numberOfBytes) != numberOfBytes) throw new EndOfStreamException();
            return encoding.GetString(buffer, 0, numberOfBytes);
        }

        public static DateTime ReadDateTime(this Stream stream)
        {
            var buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4) throw new EndOfStreamException();
            uint dosTime = BitConverter.ToUInt32(buffer, 0);
            uint sec = Math.Min(59, 2*(dosTime & 0x1f));
            uint min = Math.Min(59, (dosTime >> 5) & 0x3f);
            uint hrs = Math.Min(23, (dosTime >> 11) & 0x1f);
            uint mon = Math.Max(1, Math.Min(12, ((dosTime >> 21) & 0xf)));
            uint year = ((dosTime >> 25) & 0x7f) + 1980;
            int day = Math.Max(1, Math.Min(DateTime.DaysInMonth((int) year, (int) mon), (int) ((dosTime >> 16) & 0x1f)));
            return new DateTime((int) year, (int) mon, day, (int) hrs, (int) min, (int) sec);
        }

        public static void WriteUInt(this Stream stream, uint value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public static void WriteUShort(this Stream stream, ushort value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public static void WriteString(this Stream stream, string text, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteDateTime(this Stream stream, DateTime dateTime)
        {
            var year = (uint) dateTime.Year;
            var month = (uint) dateTime.Month;
            var day = (uint) dateTime.Day;
            var hour = (uint) dateTime.Hour;
            var minute = (uint) dateTime.Minute;
            var second = (uint) dateTime.Second;

            if (year < 1980)
            {
                year = 1980;
                month = 1;
                day = 1;
                hour = 0;
                minute = 0;
                second = 0;
            }
            else if (year > 2107)
            {
                year = 2107;
                month = 12;
                day = 31;
                hour = 23;
                minute = 59;
                second = 59;
            }

            uint dosTime = ((year - 1980) & 0x7f) << 25 |
                           (month << 21) |
                           (day << 16) |
                           (hour << 11) |
                           (minute << 5) |
                           (second >> 1);

            stream.Write(BitConverter.GetBytes(dosTime), 0, 4);
        }
    }
}