using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel
{
    internal class TestListItemViewModel
    {
        public TestListItemViewModel(Assembly testAssembly, ITest test)
        {
            Test = test;
            TestAssembly = testAssembly;
            Name = test.Name;
            Parent = test.FullName;
        }

        public ITest Test { get; private set; }
        public Assembly TestAssembly { get; private set; }

        public string Name { get; private set; }
        public string Parent { get; private set; }
    }
}
