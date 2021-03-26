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
            Assert.Equal("value1", _pList["prop1"]);
            var ex = Assert.Throws<PropertyNotFoundException<string>>(() => _pList["propx"]);
            Assert.Equal("propx", ex.PropertyKey);
            Assert.True(_pList.TryGetValue("prop1", out _));
            Assert.False(_pList.TryGetValue("propx", out _));

            
            _pList.Add(new KeyValuePair<string, string>("propy", "valuey"));
            Assert.Equal("valuey", _pList["propy"]);

            _pList.Remove("propy");
            ex = Assert.Throws<PropertyNotFoundException<string>>(() => _pList["propy"]);
            Assert.Equal("propy", ex.PropertyKey);

            _pList["propz"] = "valuez";
            Assert.Equal("valuez", _pList["propz"]);
            _pList["propz"] = "valuezz";
            Assert.Equal("valuezz", _pList["propz"]);

            _pList.Remove(new KeyValuePair<string, string>("propz", "foo"));
            ex = Assert.Throws<PropertyNotFoundException<string>>(() => _pList["propz"]);
            Assert.Equal("propz", ex.PropertyKey);

            Assert.Equal(5, _pList.Count);
            Assert.All(_pList.Keys, s => Assert.Contains(s, new [] {"prop1", "prop2", "prop3", "prop4", "prop5"}));
            Assert.All(_pList.Values, s => Assert.Contains(s, new[] {"value1", "value2", "value3", "value4", "value5"}));
        }
    }
}