using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Runner.Services
{
    /// <summary>
    /// Test filter based on the test Category attribute
    /// </summary>
    public class CategoryFilter : ITestFilter
    {
        /// <summary>
        /// Category property name
        /// </summary>
        public readonly string CATEGORY = "Category";

        /// <summary>
        /// Setup the filter
        /// </summary>
        /// <param name="categoryName">The category name to lookup</param>
        public CategoryFilter(string categoryName)
        {
            CategoryName = categoryName;
        }

        /// <summary>
        /// The Category name to filter on
        /// </summary>
        public string CategoryName { get; }

        /// <inheritdoc/>
        public bool Pass(ITest test)
        {
            if (test.IsSuite) // let the suite pass the filter in order to check children
            {
                return true;
            }
            return HasCategory(test) || ParentHasCategory(test);
        }

        /// <inheritdoc/>
        public bool IsExplicitMatch(ITest test)
        {
            return HasCategory(test);
        }

        /// <summary>
        /// See if the test parent match the category filter
        /// </summary>
        /// <param name="test">The test to check</param>
        /// <returns>true if the parent test match the category the filter is looking for</returns>
        private bool ParentHasCategory(ITest test)
        {
            ITest parent = test.Parent;
            while (parent != null)
            {
                if (HasCategory(parent))
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        /// <summary>
        /// Check is the given test has got the category property and if it match the filter
        /// </summary>
        /// <param name="test">The test to check</param>
        /// <returns>true if the test match the category the filter is looking for</returns>
        private bool HasCategory(ITest test)
        {
            if (test.Properties.ContainsKey(CATEGORY))
            {
                return test.Properties[CATEGORY].Contains(CategoryName);
            }
            return false;
        }

        /// <inheritdoc/>
        public TNode ToXml(bool recursive)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public TNode AddToXml(TNode parentNode, bool recursive)
        {
            throw new System.NotImplementedException();
        }
    }
}
