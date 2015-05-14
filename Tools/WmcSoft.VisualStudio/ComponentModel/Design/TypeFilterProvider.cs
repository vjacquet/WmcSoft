//===============================================================================
// Microsoft patterns & practices
// Web Service Software Factory
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Globalization;
using System.Workflow.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

namespace WmcSoft.ComponentModel.Design
{
    /// <summary>
    /// Base class for type filter providers used by <see cref="FilteredTypeBrowser"/> editor.
    /// </summary>
    public abstract class TypeFilterProvider : ITypeFilterProvider
    {
        /// <summary>
        /// Returns a value that indicates whether the specified type can be filtered.
        /// </summary>
        /// <param name="type">The <see cref="T:System.Type"></see> to check for filtering.</param>
        /// <param name="throwOnError">true to throw an exception when the <see cref="M:System.Workflow.ComponentModel.Design.ITypeFilterProvider.CanFilterType(System.Type,System.Boolean)"></see> is processed; otherwise, false.</param>
        /// <returns>
        /// true if the specified type can be filtered; otherwise, false.
        /// </returns>
        public abstract bool CanFilterType(Type type, bool throwOnError);

        /// <summary>
        /// Gets the description for the filter to be displayed on the class browser dialog box.
        /// </summary>
        /// <value></value>
        /// <returns>A string value that contains the description of the filter.</returns>
        public abstract string FilterDescription { get; }

        /// <summary>
        /// Throws if on error.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <param name="errorCondition">if set to <c>true</c> [error condition].</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		protected void ThrowIfOnError(bool throwOnError, bool errorCondition, string message, params object[] args)
        {
            if (throwOnError && errorCondition)
            {
                throw new ArgumentException(String.Format(
                    CultureInfo.CurrentCulture, message, args));
            }
        }
    }
}
