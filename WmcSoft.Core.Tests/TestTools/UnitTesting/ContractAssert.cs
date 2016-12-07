using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.TestTools.UnitTesting
{
    public static class ContractAssert
    {
        public static void Disposable(IDisposable disposable) {
            disposable.Dispose();
            disposable.Dispose();
        }

        public static void Collection<TCollection>(TCollection collection)
            where TCollection : ICollection<int>, new() {
            collection.Clear();
            Assert.AreEqual(0, collection.Count);

            collection.Add(1);
            Assert.AreEqual(1, collection.Count);
            Assert.IsTrue(collection.Contains(1));
            Assert.IsFalse(collection.Contains(2));

            collection.Add(2);
            Assert.AreEqual(2, collection.Count);
            Assert.IsTrue(collection.Contains(1));
            Assert.IsTrue(collection.Contains(2));

            var buffer = new int[collection.Count + 2];
            collection.CopyTo(buffer, 2);
            CollectionAssert.AreEquivalent(buffer.Skip(2).ToArray(), collection.ToArray());

            Assert.IsTrue(collection.Remove(2));
            Assert.AreEqual(1, collection.Count);

            Assert.IsFalse(collection.Remove(3));
            Assert.AreEqual(1, collection.Count);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }
    }
}
