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

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Runner.ViewModel;
using Xamarin.Forms;

namespace NUnit.Runner.View
{
    /// <summary>
    /// Xamarin.Forms view of a list of test results
    /// </summary>
    public partial class TestListView : ContentPage
	{
		internal TestListView(TestListViewModel model)
        {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
		}

        internal async void ViewTest(object sender, SelectedItemChangedEventArgs e)
        {
            var test = e.SelectedItem as TestListItemViewModel;
            if (test != null)
            {
                var runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

                runner.Load(test.TestAssembly, new Dictionary<string, object>());
                
                await Navigation.PushAsync(new TestView(new TestViewModel(runner, new SelectedTestFilter(test.Test.FullName))));
            }
        }

        

        /// <summary>
        /// Nested class provides an empty filter - one that always
        /// returns true when called. It never matches explicitly.
        /// </summary>
        private class SelectedTestFilter : TestFilter
        {
            public string FullName { get; private set; }

            public SelectedTestFilter(string fullName)
            {
                FullName = fullName;
            }

            public override bool Match(ITest test)
            {
                return FullName.StartsWith(test.FullName);
            }

            public override bool Pass(ITest test)
            {
                return FullName.StartsWith(test.FullName);
            }

            public override bool IsExplicitMatch(ITest test)
            {
                return false;
            }

            public override TNode AddToXml(TNode parentNode, bool recursive)
            {
                return parentNode.AddElement("filter");
            }
        }
    }
}
