﻿using System;
using System.Linq;
using Xunit;

namespace WmcSoft.AI
{
    public class MeasuringTests
    {
        [Fact]
        public void CanMeasureRootMeanSquareOverSquaredMeanError()
        {
            var random = new Random(1664);
            var t = new[] { 2d, 2.5d, 3d, 2.5d, 3.2d };
            var mean = t.Sum() / t.Length;

            var o = new double[t.Length];
            for (int i = 0; i < o.Length; i++) {
                o[i] = t[i] - 0.25 + random.NextDouble() / 2d;
            }

            var num = 0d;
            var denum = 0d;
            for (int i = 0; i < o.Length; i++) {
                num += (t[i] - o[i]) * (t[i] - o[i]);
                denum += t[i] * t[i];
            }
            var expected = Math.Sqrt(num / denum);

            var measuring = new Measuring(t);
            var m = measuring.Measure(o);
            var actual = m.RootMeanSquareOverSquaredMeanError;

            Assert.Equal(expected, actual, 12);
        }

        [Fact]
        public void CanMeasureRelativeRootMeanSquareOverVariance()
        {
            var random = new Random(1664);
            var t = new[] { 2d, 2.5d, 3d, 2.5d, 3.2d };
            var mean = t.Sum() / t.Length;

            var o = new double[t.Length];
            for (int i = 0; i < o.Length; i++) {
                o[i] = t[i] - 0.25 + random.NextDouble() / 2d;
            }

            var num = 0d;
            var denum = 0d;
            for (int i = 0; i < o.Length; i++) {
                num += (t[i] - o[i]) * (t[i] - o[i]);
                denum += (t[i] - mean) * (t[i] - mean);
            }
            var expected = Math.Sqrt(num / denum);

            var measuring = new Measuring(t);
            var m = measuring.Measure(o);
            var actual = m.RootMeanSquareOverVarianceMeanError;

            Assert.Equal(expected, actual, 12);
        }
    }
}
