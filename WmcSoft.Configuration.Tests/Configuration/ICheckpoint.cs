namespace WmcSoft.Configuration
{
    public interface ICheckpoint
    {
        string Name { get; }
        int MinimumLevel { get; }
        bool Verify(int level);
    }
}
