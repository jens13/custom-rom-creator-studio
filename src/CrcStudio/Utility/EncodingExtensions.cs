//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.Text;

public static class EncodingExtensions
{
    private static readonly byte[] UnicodeBom = new byte[] {255, 254};
    private static readonly byte[] BigEndianUnicodeBom = new byte[] {254, 255};
    private static readonly byte[] Utf8Bom = new byte[] {239, 187, 191};

    public static Encoding DetectEncoding(this byte[] buffer)
    {
        byte[] byteOrderMark;
        return (DetectEncoding(buffer, out byteOrderMark));
    }

    /// <exception cref="ArgumentNullException"><c>buffer</c> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c>ArgumentOutOfRangeException</c>.</exception>
    public static Encoding DetectEncoding(this byte[] buffer, out byte[] byteOrderMark)
    {
        if (buffer == null)
        {
            throw (new ArgumentNullException("buffer", "Parameter 'buffer' can not be null."));
        }

        if (buffer.Length == 0)
        {
            throw (new ArgumentOutOfRangeException("buffer",
                                                   "Parameter 'buffer' must be bigger than zero bytes for encoding to be detected."));
        }

        if (IsUnicode(buffer))
        {
            byteOrderMark = UnicodeBom;
            return (Encoding.Unicode);
        }
        if (IsBigEndianUnicode(buffer))
        {
            byteOrderMark = BigEndianUnicodeBom;
            return (Encoding.BigEndianUnicode);
        }
        if (IsUTF8(buffer))
        {
            byteOrderMark = Utf8Bom;
            return (Encoding.UTF8);
        }
        if (DetectUTF8(buffer))
        {
            byteOrderMark = new byte[0];
            return (Encoding.UTF8);
        }

        byteOrderMark = new byte[0];
        return (Encoding.GetEncoding(1252));
    }

    public static Encoding DetectEncoding(this Stream stream)
    {
        byte[] byteOrderMark;
        return (DetectEncoding(stream, out byteOrderMark));
    }

    /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
    public static Encoding DetectEncoding(this Stream stream, out byte[] byteOrderMark)
    {
        long pos = -1;
        if (stream.CanSeek)
        {
            pos = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
        }

        if (!stream.CanRead)
        {
            throw (new ArgumentException("Parameter 'stream' can not be read from.", "stream"));
        }

        int streamLength = 4096;
        if (streamLength > stream.Length)
        {
            streamLength = (int) stream.Length;
        }
        var buffer = new byte[streamLength];
        stream.Read(buffer, 0, streamLength);
        if (stream.CanSeek)
        {
            stream.Seek(pos, SeekOrigin.Begin);
        }
        return (DetectEncoding(buffer, out byteOrderMark));
    }

    private static bool IsBigEndianUnicode(byte[] buffer)
    {
        if (buffer == null)
        {
            return (false);
        }
        if (buffer.Length < 3)
        {
            return (false);
        }
        return (254 == buffer[0] && 255 == buffer[1]);
    }

    private static bool IsUnicode(byte[] buffer)
    {
        if (buffer == null)
        {
            return (false);
        }
        if (buffer.Length < 3)
        {
            return (false);
        }
        return (255 == buffer[0] && 254 == buffer[1]);
    }

    private static bool IsUTF8(byte[] buffer)
    {
        if (buffer == null)
        {
            return (false);
        }
        if (buffer.Length < 4)
        {
            return (false);
        }
        return (239 == buffer[0] && 187 == buffer[1] && 191 == buffer[2]);
    }

    private static bool DetectUTF8(byte[] buffer)
    {
        //			11..	10..	good
        //			00..	10..	bad
        //			10..	10..	don't care
        //			11..	00..	bad
        //			11..	11..	bad
        //			00..	00..	don't care
        //			10..	00..	don't care
        //			00..	11..	don't care
        //			10..	11..	don't care

        int utf8Good = 0, utf8Bad = 0;

        for (int i = 1; i < buffer.Length; i++)
        {
            byte current = buffer[i];
            byte previous = buffer[i - 1];

            if ((current & 0xC0) == 0x80)
            {
                if ((previous & 0xC0) == 0xC0)
                {
                    utf8Good++;
                }
                else if ((previous & 0x80) == 0x00)
                {
                    utf8Bad++;
                }
            }
            else if ((previous & 0xC0) == 0xC0)
            {
                utf8Bad++;
            }
        }
        return utf8Good >= utf8Bad;
    }
}