namespace GCLab;

// =====================================================
// 2) LOH + cache estático sem política de expiração
// =====================================================
class BigBufferHolder
{
    private static byte[] _buffer;

    public static byte[] Run()
    {
        _buffer = new byte[85000]; // Aloca no LOH
        return _buffer;
    }

    public static void Clear()
    {
        _buffer = null;
    }
}
