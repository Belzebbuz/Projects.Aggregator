namespace SharedLibrary.BeetlexMessages;

public class FileReader : IDisposable
{
    public FileReader(string file, Guid projectId)
    {
        _fileInfo = new FileInfo(file);
        _projectId = projectId;
        _blocksCount = (int)(_fileInfo.Length / _blockSize);
        if (_fileInfo.Length % _blockSize > 0)
            _blocksCount++;
        FileSize = _fileInfo.Length;
        _buffer = new byte[_blockSize];
        _memoryReader = _fileInfo.OpenRead();
    }

    private readonly Guid _projectId;
    private readonly Guid _releaseId = Guid.NewGuid();

    private Stream _memoryReader;

    private byte[] _buffer;

    private FileInfo _fileInfo;

    private int _blocksCount;

    private int _blockSize = 1024 * 16;

    private int _blockIndex;

    public Guid ProjectId => _projectId;    
    public Guid ReleaseId => _releaseId;
    public string SessionId => $"{_projectId}{_releaseId}";
    public int Index => _blockIndex;

    public int Size => _blockSize;
    public double Progress => ((double)_blockIndex / (double)_blocksCount) * 100;
    public int BlocksCount => _blocksCount;

    public long FileSize { get; private set; }

    public long CompletedSize { get; private set; }

    public bool Completed => _blockIndex == _blocksCount;

    public FileContentBlock Next()
    {
        FileContentBlock result = new FileContentBlock();
        result.FileName = _fileInfo.Name;
        result.ProjectId = _projectId;
        result.ReleaseId = _releaseId;
        result.SessionId = SessionId;
        byte[] data;
        if (_blockIndex == _blocksCount - 1)
        {
            data = new byte[_fileInfo.Length - _blockIndex * _blockSize];
            result.Eof = true;
        }
        else
        {
            data = _buffer;
        }
        CompletedSize += data.Length;

        if (_memoryReader.CanRead)
        {
            _memoryReader.Read(data, 0, data.Length);
        }
        else
        {
            return null;
        }
        result.Index = _blockIndex;
        result.Data = data;
        _blockIndex++;


        return result;
    }

    public void Dispose()
    {
        _memoryReader.Dispose();
    }
}

