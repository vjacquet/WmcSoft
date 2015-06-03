using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.ComponentModel
{
    [TestClass]
    public class TypeDescriptorExtensionsTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
        }

        [TestMethod]
        public void CanGetValues() {
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
            Assert.AreEqual("value1",bag["Property1"]);
            Assert.AreEqual("value2",bag["Property2"]);
            Assert.IsNull(bag["PropertyA"]);
            Assert.IsFalse(bag.ContainsKey("Property3"));
        }
    }
}
