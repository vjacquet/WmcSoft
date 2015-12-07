﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class SortedSequenceTests
    {
        [TestMethod]
        public void CanAdd() {
            var collection = new SortedSequence<char>();
            collection.AddRange(new []{'b', 'a', 'c'});
            var expected = "abc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddWithCollision() {
            var collection = new SortedSequence<string>(StringComparer.InvariantCultureIgnoreCase);
            collection.AddRange(new[] { "b", "a", "c", "B" });
            var expected = "abBc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemove() {
            var collection = new SortedSequence<char>();
            collection.AddRange(new[] { 'b', 'a', 'c' });
            Assert.IsTrue(collection.Remove('a'));
            var expected = "bc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(collection.Remove('a'));
        }

    }
}