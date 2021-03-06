﻿using System;
using System.ComponentModel;
using Xunit;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace WmcSoft.Time
{
    public class TimeUnitTests
    {
        [Fact]
        public void AreTimeUnitsSorted()
        {
            var units = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Second,
                TimeUnit.Minute,
                TimeUnit.Hour,
                TimeUnit.Day,
                TimeUnit.Week,

                TimeUnit.Month,
                TimeUnit.Quarter,
                TimeUnit.Semester,
                TimeUnit.Year,
            };

            var length = units.Length;
            for (int i = 1; i < length; i++) {
                Assert.True(units[i - 1] < units[i]);
            }
        }

        [Fact]
        public void CanConvertToString()
        {
            var date = Convert.ChangeType("2017-01-17", typeof(DateTime));

            var unit = TimeUnit.Second;
            var converter = TypeDescriptor.GetConverter(unit);

            Assert.True(converter.CanConvertTo(typeof(string)));

            var s = converter.ConvertToInvariantString(unit);
            Assert.Equal("Second", s);

            var descriptor = (InstanceDescriptor)converter.ConvertTo(unit, typeof(InstanceDescriptor));
            var instance = descriptor.Invoke();
            Assert.Equal(instance, unit);
        }

        [Fact]
        public void CanSerializeAndDeserialize()
        {
            var unit = TimeUnit.Second;
            using (var ms = new MemoryStream()) {
                var f = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                f.Serialize(ms, unit);
                ms.Seek(0, SeekOrigin.Begin);
                var instance = (TimeUnit)f.Deserialize(ms);
                Assert.Equal(unit, instance);
            }
        }

        [Fact]
        public void CanFindNextFinerUnit()
        {
            Assert.Equal(TimeUnit.Day, TimeUnit.Week.NextFinerUnit());
            Assert.Equal(TimeUnit.Hour, TimeUnit.Day.NextFinerUnit());
            Assert.Equal(TimeUnit.Minute, TimeUnit.Hour.NextFinerUnit());
            Assert.Equal(TimeUnit.Second, TimeUnit.Minute.NextFinerUnit());
            Assert.Equal(TimeUnit.Millisecond, TimeUnit.Second.NextFinerUnit());
            Assert.Equal(TimeUnit.Millisecond, TimeUnit.Millisecond.NextFinerUnit());

            Assert.Equal(TimeUnit.Semester, TimeUnit.Year.NextFinerUnit());
            Assert.Equal(TimeUnit.Quarter, TimeUnit.Semester.NextFinerUnit());
            Assert.Equal(TimeUnit.Month, TimeUnit.Quarter.NextFinerUnit());
            Assert.Equal(TimeUnit.Month, TimeUnit.Month.NextFinerUnit());
        }

        [Fact]
        public void CheckBaseUnits()
        {
            var units = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Second,
                TimeUnit.Minute,
                TimeUnit.Hour,
                TimeUnit.Day,
                TimeUnit.Week,

                TimeUnit.Month,
                TimeUnit.Quarter,
                TimeUnit.Semester,
                TimeUnit.Year,
            };
            var expected = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,

                TimeUnit.Month,
                TimeUnit.Month,
                TimeUnit.Month,
                TimeUnit.Month,
            };

            var actual = Array.ConvertAll(units, _ => _.BaseUnit);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EnsureIsConvertibleDetectSimpleUnits()
        {
            var hour = new Duration(1, TimeUnit.Hour);
            var day = new Duration(1, TimeUnit.Day);
            var month = new Duration(1, TimeUnit.Month);

            Assert.True(hour.Unit.IsConvertibleToMilliseconds());
            Assert.False(hour.Unit.IsConvertibleToDays());
            Assert.False(hour.Unit.IsConvertibleToMonths());

            Assert.True(day.Unit.IsConvertibleToMilliseconds());
            Assert.True(day.Unit.IsConvertibleToDays());
            Assert.False(day.Unit.IsConvertibleToMonths());

            Assert.False(month.Unit.IsConvertibleToMilliseconds());
            Assert.False(month.Unit.IsConvertibleToDays());
            Assert.True(month.Unit.IsConvertibleToMonths());
        }

        [Fact]
        public void EnsureIsConvertibleDetectMixedUnits()
        {
            var hour = new Duration(1, TimeUnit.Hour);
            var day = new Duration(1, TimeUnit.Day);
            var mix = hour + day;

            Assert.True(mix.Unit.IsConvertibleToMilliseconds());
            Assert.False(mix.Unit.IsConvertibleToDays());
        }
    }
}
