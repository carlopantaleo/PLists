using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.True(pList.ContainsKey("prop1"));
            Assert.False(pList.ContainsKey("dummy"));
            Assert.True(pList.Contains(new KeyValuePair<string, string>("prop1", "value1")));
            Assert.False(pList.Contains(new KeyValuePair<string, string>("prop1", "dummyValue")));
            
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

            Assert.Throws<NullReferenceException>(() => pList["nullProp"] = null);

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
            Assert.Equal(6, extPlist.Count);
            
            extPlist.Remove("prop2");
            Assert.Equal(5, extPlist.Count);
            
            Assert.False(extPlist.ContainsKey("prop2"));
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

        [Fact]
        public void MoreOverrides() {
            var extPlist1 = new PList<string, string>(_pList);
            extPlist1.Remove("prop1");
            var extPlist2 = new PList<string, string>(extPlist1);
            Assert.False(extPlist2.ContainsKey("prop1"));

            extPlist1["prop6"] = "value6";
            extPlist1["prop7"] = "value7";
            extPlist2["prop8"] = "value8";
            Assert.Equal(7, extPlist2.Count);
            Assert.Equal(6, extPlist1.Count);
            
            extPlist1.Clear();
            Assert.Equal("value1", extPlist2["prop1"]);
            Assert.False(extPlist1.ContainsKey("prop6"));
            Assert.Equal(6, extPlist2.Count);
            Assert.Equal(5, extPlist1.Count);
        }

        [Fact]
        public void CopyTo() {
            var copy = new KeyValuePair<string, string>[5];
            _pList.CopyTo(copy, 0);
            Assert.All(copy.Select(c => c.Key), s => 
                Assert.Contains(s, new [] {"prop1", "prop2", "prop3", "prop4", "prop5"}));
            Assert.All(copy.Select(c => c.Value), s => 
                Assert.Contains(s, new[] {"value1", "value2", "value3", "value4", "value5"}));
        }
    }
}