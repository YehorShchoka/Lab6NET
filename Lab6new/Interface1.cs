namespace Lab6new
{
    public interface IFileLoader
    {
        Task<List<T>?> LoadAsync<T>(string filePath);
    }

    public interface IFileSaver
    {
        Task SaveAsync<T>(string filePath, List<T> data);
    }
}
