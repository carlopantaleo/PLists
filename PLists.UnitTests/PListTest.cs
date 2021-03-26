using System.Collections.Generic;
using PLists.Exceptions;
using Xunit;

namespace PLists.UnitTests {
    public class PListTest {
        private readonly IPList<string, string> _pList;
        
        public PListTest() {
            _pList = new PList<string, string> {
                ["prop1"] = "value1",
                ["prop2"] = "value2",
                ["prop3"] = "value3",
                ["prop4"] = "value4",
                ["prop5"] = "value5",
            };
        }

        [Fact]
        public void BasePListOperations() {
            CommonPListOperations(_pList);    
        }
        
        private static void CommonPListOperations(IPList<string, string> pList) {
            Assert.Equal("value1", pList["prop1"]);
            var ex = Assert.Throws<PropertyNotFoundException<string>>(() => pList["propx"]);
            Assert.Equal("propx", ex.PropertyKey);
            Assert.True(pList.TryGetValue("prop1", out _));
            Assert.False(pList.TryGetValue("propx", out _));

            
            pList.Add(new KeyValuePair<string, string>("propy", "valuey"));
            Assert.Equal("valuey", pList["propy"]);

            pList.Remove("propy");
            ex = Assert.Throws<PropertyNotFoundException<string>>(() => pList["propy"]);
            Assert.Equal("propy", ex.PropertyKey);

            pList["propz"] = "valuez";
            Assert.Equal("valuez", pList["propz"]);
            pList["propz"] = "valuezz";
            Assert.Equal("valuezz", pList["propz"]);

            pList.Remove(new KeyValuePair<string, string>("propz", "foo"));
            ex = Assert.Throws<PropertyNotFoundException<string>>(() => pList["propz"]);
            Assert.Equal("propz", ex.PropertyKey);

            Assert.Equal(5, pList.Count);
            Assert.All(pList.Keys, s => Assert.Contains(s, new [] {"prop1", "prop2", "prop3", "prop4", "prop5"}));
            Assert.All(pList.Values, s => Assert.Contains(s, new[] {"value1", "value2", "value3", "value4", "value5"}));
        }

        [Fact]
        public void RawInheritedPList() {
            var extPlist = new PList<string, string>(_pList);
            CommonPListOperations(extPlist);
        }

        [Fact]
        public void Overrides() {
            var extPlist = new PList<string, string>(_pList) {
                ["prop1"] = "overriden", 
                ["propx"] = "valuex"
            };
            extPlist.Remove("prop2");
            
            Assert.Equal("overriden", extPlist["prop1"]);
            Assert.Equal("value1", extPlist.Prototype?["prop1"]);
            Assert.False(extPlist.TryGetValue("prop2", out _));
            Assert.True(extPlist.Prototype?.TryGetValue("prop2", out _));
            Assert.Equal("value2", extPlist.Prototype?["prop2"]);
            Assert.Equal("valuex", extPlist["propx"]);
            Assert.False(extPlist.Prototype?.TryGetValue("propx", out _));
            
            Assert.Equal(5, extPlist.Count);
            Assert.All(extPlist.Keys, s => Assert.Contains(s, new [] {"prop1", "prop3", "prop4", "prop5", "propx"}));
            Assert.All(extPlist.Values, s => Assert.Contains(s, new[] {"overriden", "value3", "value4", "value5", "valuex"}));
        }
    }
}