using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Business
{
    public class TemporalEnumerableTests
    {
        class Person : ITemporal
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public DateTime? ValidSince { get; set; }
            public DateTime? ValidUntil { get; set; }
        }

        public class PersonAddress : ITemporal
        {
            public int PersonId { get; set; }
            public int AddressId { get; set; }

            public DateTime? ValidSince { get; set; }
            public DateTime? ValidUntil { get; set; }
        }

        abstract class Address : ITemporal
        {
            public int Id { get; set; }
            public DateTime? ValidSince { get; set; }
            public DateTime? ValidUntil { get; set; }
        }

        class PostalAddress : Address
        {
            public string[] Lines { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
        }

        class EmailAddress : Address
        {
            public string Email { get; set; }
        }

        class TelecomAddress : Address
        {
            public string Number { get; set; }
        }

        private readonly List<Person> Persons = new List<Person> {
            new Person { Id=1, Name="Vincent JAQUET", ValidSince = new DateTime(2017, 01, 01), ValidUntil = new DateTime(2017, 05, 01)},
            new Person { Id=2, Name="Sandrine JACQUET", ValidSince = new DateTime(2017, 02, 01)},
            new Person { Id=1, Name="Vincent JACQUET", ValidSince = new DateTime(2017, 05, 01)},
            new Person { Id=3, Name="Alicia JACQUET", ValidSince = new DateTime(2017, 06, 01)},
        };

        [Fact]
        public void CanEnumerateTemporalAsOf()
        {
            var v1 = Persons.AsOf(new DateTime(2017, 01, 15)).Select(p => p.Name);
            var v2 = Persons.AsOf(new DateTime(2017, 02, 15)).Select(p => p.Name);
            var v3 = Persons.AsOf(new DateTime(2017, 05, 15)).Select(p => p.Name);

            Assert.Equal(new[] { "Vincent JAQUET" }, v1);
            Assert.Equal(new[] { "Vincent JAQUET", "Sandrine JACQUET" }, v2);
            Assert.Equal(new[] { "Sandrine JACQUET", "Vincent JACQUET" }, v3);
        }


        [Fact]
        public void CanEnumerateTemporalBetween()
        {
            var actual = Persons.Between(new DateTime(2017, 01, 15), new DateTime(2017, 05, 15)).Select(p => p.Name);

            Assert.Equal(new[] { "Vincent JAQUET", "Sandrine JACQUET", "Vincent JACQUET" }, actual);
        }

    }
}
