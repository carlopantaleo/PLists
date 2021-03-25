using PLists.Exceptions;
using Xunit;

namespace PLists.UnitTests {
    public class PListTest {
        private readonly IPList<string, string> _pList;
        
        public PListTest() {
            _pList = new PList<string, string>();
        }

        [Fact]
        public void BasePListOperations() {
            _pList["prop1"] = "value1";
            
            Assert.Equal("value1", _pList["prop1"]);
            var ex = Assert.Throws<PropertyNotFoundException<string>>(() => _pList["prop2"]);
            Assert.Equal("prop2", ex.PropertyKey);
            Assert.True(_pList.TryGetValue("prop1", out _));
            Assert.False(_pList.TryGetValue("prop2", out _));
        }
    }
}