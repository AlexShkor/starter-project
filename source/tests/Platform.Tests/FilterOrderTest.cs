using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DQF.Platform.ViewServices;
using NUnit.Framework;

namespace Platform.Tests
{

    [TestFixture]
    public class FilterOrderTest
    {
        private BaseFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _filter = new BaseFilter();
        }

        [TearDown]
        public void TearDown()
        {
            _filter.Ordering.Clear();
        }

        [Test]
        public void simple()
        {
            _filter.AddOrder((TestView x) => x.Name);
            Assert.AreEqual("Name", _filter.Ordering.First().Key);
        } 
        
        [Test]
        public void inner()
        {
            _filter.AddOrder((TestView x) => x.InnerView.Value);
            Assert.AreEqual("InnerView.Value", _filter.Ordering.First().Key);
        }

        [Test]
        public void desc()
        {
            _filter.AddOrder((TestView x) => x.InnerView.Value, true);
            Assert.AreEqual("InnerView.Value", _filter.Ordering.First().Key);
            Assert.AreEqual(true, _filter.Ordering.First().Desc);
        }

        public class TestView
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public TestInnerView InnerView { get; set; }

            public TestView()
            {
                InnerView = new TestInnerView();
            }

        }

        public class TestInnerView
        {
            public String Value { get; set; }
        }
    }
}
