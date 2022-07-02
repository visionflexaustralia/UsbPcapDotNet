using System.Runtime.InteropServices;

namespace UsbPcapDotNet.TestHarness;

public static class Utils
{
    /// <summary>
    ///     Convert byte array to string as a sequence of hex values.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHexString(this IEnumerable<byte> bytes, string deliminator = "")
    {
        return bytes.Aggregate(string.Empty, (current, b) => $"{current}{b:x2}{deliminator}")[..^deliminator.Length];
    }

    /// <summary>
    ///     Compare two bytes array
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <param name="length"></param>
    /// <returns>bool</returns>
    public static bool CompareBytes(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2, int length)
    {
        var isEqual = true;
        for (var i = 0; i < length && i < a1.Length && i < a2.Length; i++)
        {
            isEqual = isEqual && a1[i] == a2[i];
        }

        return isEqual;
    }


        /// <summary>
        /// Seek bytes from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="target"></param>
        /// <returns>stream position</returns>
        /// <exception cref="ArgumentException"></exception>
        public static long SeekBytes(Stream stream, byte[] target, bool negativeIfNone = false)
        {
            var matches = 0;
            var readByte = stream.ReadByte();

            while (stream.Position <= stream.Length)
            {
                //var streamByte = Encoding.UTF8.GetString(new [] {Convert.ToByte(readByte)});
                //var targetByte = Encoding.UTF8.GetString(new [] { target[matches]});
                //Console.WriteLine($@"Current: {streamByte} Looking For: {targetByte}");
                if (readByte == target[matches])
                {
                    matches++;
                    if (matches == target.Length)
                    {
                        return stream.Position;
                    }
                }
                else
                {
                    matches = 0;
                }

                if (stream.Position == stream.Length)
                {
                    break;
                }

                readByte = stream.ReadByte();
            }

            if (negativeIfNone)
            {
                return -1;
            }

            throw new ArgumentException(
                "Stream must contain target.");
        }

        /// <inheritdoc cref="ContainsTargetBytes(Stream,byte[])"/>
        public static bool ContainsTargetBytes(byte[] source, byte[] target)
        {
            using var memory = new MemoryStream(source);
            return ContainsTargetBytes(memory, target);
        }

        /// <summary>
        /// Check if a bytes array exist in a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="target"></param>
        /// <returns>bool</returns>
        public static bool ContainsTargetBytes(Stream stream, byte[] target)
        {
            var matches = 0;
            var readByte = stream.ReadByte();

            while (stream.Position <= stream.Length)
            {
                // var streamByte = Encoding.UTF8.GetString(new [] {Convert.ToByte(readByte)});
                // var targetByte = Encoding.UTF8.GetString(new [] { target[matches]});
                // Console.WriteLine($@"Current: {streamByte} Looking For: {targetByte}");
                if (readByte == target[matches])
                {
                    matches++;
                    if (matches == target.Length)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        return true;
                    }
                }
                else
                {
                    matches = 0;
                }

                if (stream.Position == stream.Length)
                {
                    return false;
                }

                readByte = stream.ReadByte();
            }

            return false;
        }

        static T BytesToStructure<T>(this byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
            handle.Free();
            return result;
        }
}
