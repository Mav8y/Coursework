using System;
using System.IO;

public class LimitMp3Stream : Stream
{
    private readonly Stream _baseStream;
    private readonly long _limit;
    private long _totalBytesRead;

    public LimitMp3Stream(Stream baseStream, double maxBytes)
    {
        _baseStream = baseStream;
        _limit = (long)maxBytes;
        _totalBytesRead = 0;
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => _limit;

    public override long Position
    {
        get => _totalBytesRead;
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_totalBytesRead >= _limit)
        {
            return 0; // Достигнут лимит, прекращаем чтение
        }

        long remainingBytes = _limit - _totalBytesRead;
        int bytesToRead = (int)Math.Min(count, remainingBytes);

        int bytesRead = _baseStream.Read(buffer, offset, bytesToRead);
        _totalBytesRead += bytesRead;

        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}
