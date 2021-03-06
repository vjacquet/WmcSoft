﻿using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class FieldwiseEqualityComparerTests
    {
        static readonly IEqualityComparer<Address> Comparer = FieldwiseEqualityComparer<Address>.FromFields();

        private class Address
        {
            private readonly string _address1;
            private readonly string _city;
            private readonly string _state;

            public Address(string address1, string city, string state)
            {
                _address1 = address1;
                _city = city;
                _state = state;
            }

            public string Address1 {
                get { return _address1; }
            }

            public string City {
                get { return _city; }
            }

            public string State {
                get { return _state; }
            }
        }

        private class ExpandedAddress : Address
        {
            private readonly string _address2;

            public ExpandedAddress(string address1, string address2, string city, string state)
            : base(address1, city, state)
            {
                _address2 = address2;
            }

            public string Address2 {
                get { return _address2; }
            }
        }

        [Fact]
        public void AddressEqualsWorksWithIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            Assert.Equal(address, address2, Comparer);
        }

        [Fact]
        public void AddressEqualsWorksWithNonIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            Assert.NotEqual(address, address2, Comparer);
        }

        [Fact]
        public void AddressEqualsWorksWithNonIdenticalDerivedAddresses()
        {
            var comparer = FieldwiseEqualityComparer<ExpandedAddress>.FromFields();
            var address = new ExpandedAddress("Address1", "Address2", "Austin", "TX");
            var address2 = new ExpandedAddress("Address2", "Address2", "Austin", "TX");

            Assert.NotEqual(address, address2, comparer);
        }

        [Fact]
        public void AddressEqualsWorksWithNulls()
        {
            Address address = new Address(null, "Austin", "TX");
            Address address2 = new Address("Address2", "Austin", "TX");

            Assert.NotEqual(address, address2, Comparer);
        }

        [Fact]
        public void AddressEqualsWorksWithNullsOnOtherObject()
        {
            var address = new Address("Address2", "Austin", "TX");
            var address2 = new Address("Address2", null, "TX");

            Assert.NotEqual(address, address2, Comparer);
        }

        [Fact]
        public void AddressEqualsIsReflexive()
        {
            var address = new Address("Address1", "Austin", "TX");

            Assert.Equal(address, address, Comparer);
        }

        [Fact]
        public void AddressEqualsIsSymmetric()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            Assert.NotEqual(address, address2, Comparer);
            Assert.NotEqual(address2, address, Comparer);
        }

        [Fact]
        public void AddressEqualsIsTransitive()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");
            var address3 = new Address("Address1", "Austin", "TX");

            Assert.Equal(address, address2, Comparer);
            Assert.Equal(address2, address3, Comparer);
            Assert.Equal(address3, address, Comparer);
        }

        [Fact]
        public void DerivedTypesBehaveCorrectly()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            Assert.NotEqual(address, address2, Comparer);
        }

        [Fact]
        public void EqualValueObjectsHaveSameHashCode()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            Assert.Equal(Comparer.GetHashCode(address), Comparer.GetHashCode(address2));
        }

        [Fact]
        public void TransposedValuesGiveDifferentHashCodes()
        {
            var address = new Address(null, "Austin", "TX");
            var address2 = new Address("TX", "Austin", null);

            Assert.NotEqual(Comparer.GetHashCode(address), Comparer.GetHashCode(address2));
        }

        [Fact]
        public void UnequalValueObjectsHaveDifferentHashCodes()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            Assert.NotEqual(Comparer.GetHashCode(address), Comparer.GetHashCode(address2));
        }

        [Fact]
        public void TransposedValuesOfFieldNamesGivesDifferentHashCodes()
        {
            var address = new Address("_city", null, null);
            var address2 = new Address(null, "_address1", null);

            Assert.NotEqual(Comparer.GetHashCode(address), Comparer.GetHashCode(address2));
        }

        [Fact]
        public void DerivedTypesHashCodesBehaveCorrectly()
        {
            var address = new ExpandedAddress("Address99999", "Apt 123", "New Orleans", "LA");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            Assert.NotEqual(Comparer.GetHashCode(address), Comparer.GetHashCode(address2));
        }
    }
}
