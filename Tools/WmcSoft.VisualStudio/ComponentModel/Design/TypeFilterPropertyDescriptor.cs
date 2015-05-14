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
using System.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
	/// <summary>
	/// Custom proxy of the property descriptor, that provides an implementation of the <see cref="ITypeFilterProvider"/> 
	/// interface and delegates onto the received provider.
	/// </summary>
	public class TypeFilterPropertyDescriptor : PropertyDescriptor, ITypeFilterProvider
	{
		PropertyDescriptor descriptor;
		ITypeFilterProvider filterProvider;

		public TypeFilterPropertyDescriptor(PropertyDescriptor descriptor, ITypeFilterProvider filterProvider)
			: base(descriptor)
		{
			this.descriptor = descriptor;
			this.filterProvider = filterProvider;
		}

		#region PropertyDescriptor Members

		public override bool CanResetValue(object component)
		{
			return descriptor.CanResetValue(component);
		}

		public override Type ComponentType
		{
			get { return descriptor.ComponentType; }
		}

		public override object GetValue(object component)
		{
			return descriptor.GetValue(component);
		}

		public override bool IsReadOnly
		{
			get { return descriptor.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return descriptor.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			descriptor.ResetValue(component);
		}

		public override void SetValue(object component, object value)
		{
			descriptor.SetValue(component, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return descriptor.ShouldSerializeValue(component);
		}

		#endregion

		#region ITypeFilterProvider Members

		public bool CanFilterType(Type type, bool throwOnError)
		{
			return filterProvider.CanFilterType(type, throwOnError);
		}

		public string FilterDescription
		{
			get { return filterProvider.FilterDescription; }
		}

		#endregion
	}
}
