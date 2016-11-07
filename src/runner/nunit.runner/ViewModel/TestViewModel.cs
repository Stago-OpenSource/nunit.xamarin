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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Runner.Extensions;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    class TestViewModel : BaseViewModel, ITestListener
    {
        public TestViewModel(ITestResult result)
        {
            SetResult(result);
        }

        public TestViewModel(NUnitTestAssemblyRunner runner, ITestFilter filter)
        {
            Message = "Test is running";

            Task.Run(() => runner.RunAsync(this, filter));
        }

        private void SetResult(ITestResult result)
        {
            TestResult = result;
            Message = StringOrNone(result.Message);
            Output = StringOrNone(result.Output);
            StackTrace = StringOrNone(result.StackTrace);

            var builder = new StringBuilder();
            IPropertyBag props = result.Test.Properties;
            foreach (string key in props.Keys)
            {
                foreach (var value in props[key])
                {
                    builder.AppendFormat("{0} = {1}{2}", key, value, Environment.NewLine);
                }
            }
            Properties = StringOrNone(builder.ToString());
        }

        private static ITestResult GetLastResultNode(ITestResult result)
        {
            if (result.HasChildren)
            {
                return GetLastResultNode(result.Children.First());
            }
            return result;
        }

        public ITestResult TestResult { get; private set; }
        public string Message { get; private set; }
        public string Output { get; private set; }
        public string StackTrace { get; private set; }
        public string Properties { get; private set; }

        /// <summary>
        /// Gets the color for this result.
        /// </summary>
        public Color Color
        {
            get { return TestResult == null? Color.White : TestResult.Color(); }
        }

        private string StringOrNone(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "<none>";
            return str;
        }

        /// <summary>Called when a test has just started</summary>
        /// <param name="test">The test that is starting</param>
        public void TestStarted(ITest test)
        {
            
        }

        /// <summary>Called when a test has finished</summary>
        /// <param name="result">The result of the test</param>
        public void TestFinished(ITestResult result)
        {
            if (!result.HasChildren)
            {
                Device.BeginInvokeOnMainThread(
                    () =>
                    {
                        SetResult(GetLastResultNode(result));

                        OnPropertyChanged("Message");
                        OnPropertyChanged("Output");
                        OnPropertyChanged("StackTrace");
                        OnPropertyChanged("Properties");
                        OnPropertyChanged("Color");
                        OnPropertyChanged("TestResult");
                    });
            }
        }

        /// <summary>
        /// Called when a test produces output for immediate display
        /// </summary>
        /// <param name="output">A TestOutput object containing the text to display</param>
        public void TestOutput(TestOutput output)
        {
            
        }
    }
}
