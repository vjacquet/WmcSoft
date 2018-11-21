using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WmcSoft.IO.Sources
{
    public class BatchTest
    {
        [Fact]
        public async void CanCreateBatch()
        {
            using (var batch = new StubBatch()) {
                batch.Add("A", new StubStreamSource("A"));
                batch.Add("B", new StubStreamSource("B"));

                await batch.CommitAsync(CancellationToken.None);
            }
        }
    }

    public class StubBatch : Batch<IDisposable>
    {
        protected override Task<IDisposable> CreateCommitScopeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Disposable.Empty);
        }

        protected override Task ProcessAsync(IDisposable scope, string name, IStreamSource source, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class StubStreamSource : IStreamSource
    {
        public StubStreamSource(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Stream OpenSource()
        {
            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms, Encoding.UTF8)) {
                writer.WriteLine(Name);
            }
            ms.Seek(0L, SeekOrigin.Begin);

            return ms;
        }
    }
}
