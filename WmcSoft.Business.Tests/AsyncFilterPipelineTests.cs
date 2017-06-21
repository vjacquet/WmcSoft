using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace WmcSoft.Business
{
    public class AsyncFilterPipelineTests
    {
        class AddItemFilter<T> : IAsycnFilter<List<T>>
        {
            private readonly T _item;

            public AddItemFilter(T item)
            {
                _item = item;
            }

            public async Task OnExecutionAsync(List<T> context, ActionExecutionDelegate next)
            {
                context.Add(_item);
                await next();
            }
        }

        [Fact]
        public async Task CanRunAsyncFilterPipeline()
        {
            var pipeline = new AsyncFilterPipeline<List<int>>(
                new AddItemFilter<int>(1),
                new AddItemFilter<int>(2),
                new AddItemFilter<int>(3)
            );
            var context = new List<int>();
            await pipeline.RunAsync(context);

            var expected = new[] { 1, 2, 3 };
            Assert.Equal(expected, context);
        }
    }
}