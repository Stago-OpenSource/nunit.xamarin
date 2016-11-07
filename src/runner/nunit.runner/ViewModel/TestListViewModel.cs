// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    class TestListViewModel : BaseViewModel
    {
        readonly IList<Assembly> _testAssemblies;

        /// <summary>
        /// A list of tests that did not pass
        /// </summary>
        public ObservableCollection<TestListItemViewModel> Tests { get; private set; }

        public TestListViewModel(IList<Assembly> assemblies)
        {
            _testAssemblies = assemblies;

            Tests = new ObservableCollection<TestListItemViewModel>();
            AddTests();
        }

        /// <summary>
        /// Add all tests that did not pass to the Results collection
        /// </summary>
        /// <param name="result"></param>
        /// <param name="viewAll"></param>
        private void AddTests()
        {
            var builder = new DefaultTestAssemblyBuilder();
            foreach (var testAssembly in _testAssemblies)
            {
                var test = builder.Build(testAssembly, new Dictionary<string, object>());

                AddTestItem(testAssembly, test);
            }
        }

        private void AddTestItem(Assembly testAssembly, ITest test)
        {
            if (test.HasChildren)
            {
                foreach (var testItem in test.Tests)
                {
                    AddTestItem(testAssembly, testItem);
                }
            }
            else
            {
                Tests.Add(new TestListItemViewModel(testAssembly, test));
            }
        }
    }
}
