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
}
