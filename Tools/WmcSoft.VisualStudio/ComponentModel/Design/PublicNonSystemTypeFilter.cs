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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace WmcSoft.ComponentModel.Design
{
	public class PublicNonSystemTypeFilter : PublicTypeFilter
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="T:PublicNonSystemTypeFilter"/> class.
        /// </summary>
        //We need a default constructor for the FilteredTypeBrowser to work properly
        public PublicNonSystemTypeFilter() : base()
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="T:PublicNonSystemTypeFilter"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        //We need a default constructor for the FilteredTypeBrowser to work properly
		public PublicNonSystemTypeFilter(IServiceProvider provider)
			: base(provider)
        {
        }

		/// <summary>
		/// Returns a value that indicates whether the specified type can be filtered.
		/// </summary>
		/// <param name="type">The <see cref="T:System.Type"></see> to check for filtering.</param>
		/// <param name="throwOnError">true to throw an exception when the <see cref="M:System.Workflow.ComponentModel.Design.ITypeFilterProvider.CanFilterType(System.Type,System.Boolean)"></see> is processed; otherwise, false.</param>
		/// <returns>
		/// true if the specified type is included; otherwise, false.
		/// </returns>
		// FXCOP: parameter is required.
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override bool CanFilterType(Type type, bool throwOnError)
		{
			if (base.CanFilterType(type, throwOnError) &&
				!type.FullName.StartsWith("System.") &&
				!type.FullName.StartsWith("Microsoft.") &&
				!type.IsAbstract &&
				!type.IsInterface
				)
			{
				return true;
			}

            ThrowIfOnError(throwOnError, true, WmcSoft.Properties.Resources.InvalidTypeError);

			return false;
		}

		/// <summary>
		/// Gets the description for the filter to be displayed on the class browser dialog box.
		/// </summary>
		/// <value></value>
		/// <returns>A string value that contains the description of the filter.</returns>
		public override string FilterDescription
		{
            get { return WmcSoft.Properties.Resources.PublicNonSystemTypeFilter_Description; }
		}
	}
}
