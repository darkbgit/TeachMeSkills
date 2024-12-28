namespace TextAnalyzer.IO.Files;

public class FileAssist : IDisposable
{
    public FileAssist(string filePath, FileMode fileMode, FileAccess fileAccess)
    {
        try
        {
            FileStream = new FileStream(filePath, fileMode, fileAccess);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new ArgumentException(e.Message, e);
        }
        catch (DirectoryNotFoundException e)
        {
            throw new ArgumentException(e.Message, e);
        }
    }

    public FileStream FileStream { get; }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            FileStream?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FileAssist()
    {
        Dispose(false);
    }
}

