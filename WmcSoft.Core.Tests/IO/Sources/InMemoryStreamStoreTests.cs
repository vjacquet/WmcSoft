namespace WmcSoft.IO.Sources
{
    public class InMemoryStreamStoreTests : StreamStoreTests
    {
        protected override StreamStore CreateEmptyStore(IDateTimeSource source)
        {
            return new InMemoryStreamStore(source);
        }
    }
}
