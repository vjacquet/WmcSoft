using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using WmcSoft.IO;
using Xunit;

namespace WmcSoft.Runtime.Serialization
{
    public class XmlSerializerTests
    {
        [DataContract(Name ="person", Namespace = "urn:wmcsoft:tests")]
        public class Person
        {
            [DataMember(Name = "dateOfBirth")]
            readonly DateTime _dob;

            [DataMember(Name = "fullName")]
            readonly string _name;

            public Person(string name, DateTime dateOfBirth)
            {
                _name = name;
                _dob = dateOfBirth;
            }

            public string Name => _name;

            public DateTime DateOfBirth => _dob.Date;
        }


        [Fact]
        public void CanSerializeComplexObject()
        {
            var einstein = new Person("Einstein", new DateTime(1879, 3, 14));
            var ms = new MemoryStream();

            var serializer = new XmlSerializer<Person>();
            serializer.Serialize(ms, einstein);

            ms.Rewind();
            var document = new XmlDocument();
            document.Load(ms);

            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("w", @"urn:wmcsoft:tests");
            Assert.Equal("Einstein", document.SelectSingleNode("w:person/w:fullName", nsmgr).InnerText);
        }

        [Fact]
        public void CanDeserializeComplexObject()
        {
            const string data= @"<person xmlns='urn:wmcsoft:tests'><dateOfBirth>1879-03-14</dateOfBirth><fullName>Einstein</fullName></person>";
            var serializer = new XmlSerializer<Person>();
            var person = serializer.Deserialize(new StringReader(data));
            Assert.Equal("Einstein", person.Name);
            Assert.Equal(new DateTime(1879, 3, 14), person.DateOfBirth);
        }
    }
}
