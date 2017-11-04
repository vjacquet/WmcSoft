using System;

namespace WmcSoft
{
    internal class MockRandom : Random
    {
        private readonly Func<int, int, int> _generator;

        public MockRandom(Func<int, int, int> generator)
        {
            _generator = generator;
        }

        public int Called { get; private set; }

        public override int Next(int minValue, int maxValue)
        {
            Called++;
            return _generator(minValue, maxValue);
        }

        public override int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public override int Next()
        {
            return Next(int.MinValue, int.MaxValue);
        }

        protected override double Sample()
        {
            throw new NotSupportedException();
        }

        public override void NextBytes(byte[] buffer)
        {
            throw new NotSupportedException();
        }

        public override double NextDouble()
        {
            throw new NotSupportedException();
        }
    }
}
