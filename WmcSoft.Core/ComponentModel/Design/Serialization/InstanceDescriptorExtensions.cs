#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.ComponentModel.Design.Serialization;

namespace WmcSoft.ComponentModel.Design.Serialization
{
    /// <summary>
    /// Defines the extension methods to the <see cref="InstanceDescriptor"/> class.
    /// This is a static class.
    /// </summary>
    public static class InstanceDescriptorExtensions
    {
        /// <summary>
        /// Returns the <see cref="InstanceDescriptor"/> describing how to construct
        /// a type with the given argument.
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="type">The type to be constructed.</param>
        /// <param name="arg">The argument.</param>
        /// <returns>The <see cref="InstanceDescriptor"/> describing how to construct the type.</returns>
        public static InstanceDescriptor DescribeConstructor<T>(this Type type, T arg)
        {
            var method = type.GetConstructor(new Type[] { typeof(T) });
            var arguments = new object[] { arg };
            return new InstanceDescriptor(method, arguments, true);
        }

        /// <summary>
        /// Returns the <see cref="InstanceDescriptor"/> describing how to construct
        /// a type with the given arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="type">The type to be constructed.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <returns>The <see cref="InstanceDescriptor"/> describing how to construct the type.</returns>
        public static InstanceDescriptor DescribeConstructor<T1, T2>(this Type type, T1 arg1, T2 arg2)
        {
            var method = type.GetConstructor(new Type[] { typeof(T1), typeof(T2) });
            var arguments = new object[] { arg1, arg2 };
            return new InstanceDescriptor(method, arguments, true);
        }

        /// <summary>
        /// Returns the <see cref="InstanceDescriptor"/> describing how to construct
        /// a type with the given arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="type">The type to be constructed.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <returns>The <see cref="InstanceDescriptor"/> describing how to construct the type.</returns>
        public static InstanceDescriptor DescribeConstructor<T1, T2, T3>(this Type type, T1 arg1, T2 arg2, T3 arg3)
        {
            var method = type.GetConstructor(new Type[] { typeof(T1), typeof(T2), typeof(T3) });
            var arguments = new object[] { arg1, arg2, arg3 };
            return new InstanceDescriptor(method, arguments, true);
        }

        /// <summary>
        /// Returns the <see cref="InstanceDescriptor"/> describing how to construct
        /// a type by calling a method with the given argument.
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="type">The type to be constructed.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="arg">The argument.</param>
        /// <returns>The <see cref="InstanceDescriptor"/> describing how to construct the type.</returns>
        public static InstanceDescriptor DescribeMethod<T>(this Type type, string name, T arg)
        {
            var method = type.GetMethod(name, new Type[] { typeof(T) });
            var arguments = new object[] { arg };
            return new InstanceDescriptor(method, arguments, true);
        }
    }
}
