// Inspired from Mvp.Xml

using System;
using System.Collections.Generic;
using System.Text;

namespace WmcSoft.CustomTools.Design
{
	/// <summary>
	/// Determines which versions of VS.NET are supported by the custom tool.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SupportedVersionAttribute : Attribute
	{
		Version version;

		/// <summary>
		/// Initializes the attribute.
		/// </summary>
		/// <param name="version">Version supported by the tool.</param>
		public SupportedVersionAttribute(string version)
		{
			this.version = new Version(version);
		}

		/// <summary>
		/// Version supported by the tool.
		/// </summary>
		public Version Version
		{
			get
			{
				return version;
			}
		}
	}
}
