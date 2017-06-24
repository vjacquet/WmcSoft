using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace WmcSoft.ComponentModel
{
    public class TypeDescriptorExtensionsTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
        }

        [Fact]
        public void CanGetValues()
        {
            var model = new Model {
                Property1 = "value1",
                Property2 = "value2",
                Property3 = "value3",
            };
            var bag = new Dictionary<string, object>();
            bag.Add("Property1", null);
            bag.Add("Property2", null);
            bag.Add("PropertyA", null);
            TypeDescriptor.GetProperties(typeof(Model)).GetValues(model, bag);
            Assert.Equal("value1", bag["Property1"]);
            Assert.Equal("value2", bag["Property2"]);
            Assert.Null(bag["PropertyA"]);
            Assert.False(bag.ContainsKey("Property3"));
        }
    }
}
