using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Business
{
    public class CancellableFilterPipelineTests
    {
        class AddItemFilter<T> : IFilter<List<T>>
        {
            private readonly T _item;

            public AddItemFilter(T item)
            {
                _item = item;
            }

            public void OnExecuting(List<T> context)
            {
                context.Add(_item);
            }

            public void OnExecuted(List<T> context)
            {
            }
        }

        [Fact]
        public void CanRunCancellableFilterPipeline()
        {
            var pipeline = new CancellableFilterPipeline<List<int>>(
                c => c.Count == 3,
                new AddItemFilter<int>(1),
                new AddItemFilter<int>(2),
                new AddItemFilter<int>(3),
                new AddItemFilter<int>(4)
            );
            var context = new List<int>();
            pipeline.Run(context);

            var expected = new[] { 1, 2, 3 };
            Assert.Equal(expected, context);
        }
    }
}
