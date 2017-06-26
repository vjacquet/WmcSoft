using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Canvas
{
    public class CanvasImageDataTests
    {
        class FakeImageData : IImageSize<int>
        {
            public FakeImageData(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public int Height {
                get; set;
            }

            public int Width {
                get; set;
            }
        }

        struct PutImageDataParameters
        {
            public int dx;
            public int dy;
            public int dirtyX;
            public int dirtyY;
            public int dirtyWidth;
            public int dirtyHeight;
        }

        class MockCanvasImageData : ICanvasImageData<FakeImageData, int>
        {
            public MockCanvasImageData()
            {
                Calls = new Queue<PutImageDataParameters>();
            }

            public FakeImageData CreateImageData(int sw, int sh)
            {
                return new FakeImageData(sw, sh);
            }

            public FakeImageData GetImageData(int sx, int sy, int sw, int sh)
            {
                return new FakeImageData(sw, sh);
            }

            public void PutImageData(FakeImageData imagedata, int dx, int dy, int dirtyX, int dirtyY, int dirtyWidth, int dirtyHeight)
            {
                Calls.Enqueue(new PutImageDataParameters { dx = dx, dy = dy, dirtyX = dirtyX, dirtyY = dirtyY, dirtyWidth = dirtyWidth, dirtyHeight = dirtyHeight });
            }

            public Queue<PutImageDataParameters> Calls { get; }
        }

        [Fact]
        public void CanPutImageData()
        {
            var canvas = new MockCanvasImageData();
            var imagedata = new FakeImageData(320, 200);

            canvas.PutImageData(imagedata, 5, 10, dirtyY: 50);
            Assert.Equal(new PutImageDataParameters { dx = 5, dy = 10, dirtyX = 0, dirtyY = 50, dirtyWidth = 320, dirtyHeight = 200 }, canvas.Calls.Dequeue());

            canvas.PutImageData(imagedata, 5, 10);
            Assert.Equal(new PutImageDataParameters { dx = 5, dy = 10, dirtyX = 0, dirtyY = 0, dirtyWidth = 320, dirtyHeight = 200 }, canvas.Calls.Dequeue());
        }
    }
}
