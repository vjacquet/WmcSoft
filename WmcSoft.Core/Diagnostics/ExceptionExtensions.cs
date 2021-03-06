﻿#region Licence

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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Exception"/> class.
    /// This is a static class. 
    /// </summary>
    public static class ExceptionExtensions
    {
        static IEnumerable<DictionaryEntry> DoCrawlData<K>(Exception exception, K converter)
            where K : IDataKeyConverter
        {
            while (exception != null) {
                foreach (DictionaryEntry entry in exception.Data) {
                    if (converter.IsSupported(entry))
                        yield return entry;
                }
                exception = exception.InnerException;
            }
        }

        static IEnumerable<DictionaryEntry> DoCrawlUniqueData<K>(Exception exception, K converter)
           where K : IDataKeyConverter
        {
            var set = new HashSet<object>();
            while (exception != null) {
                foreach (DictionaryEntry entry in exception.Data) {
                    if (converter.IsSupported(entry) && set.Add(entry.Key))
                        yield return entry;
                }
                exception = exception.InnerException;
            }
        }

        public static IEnumerable<DictionaryEntry> CrawlData<K>(this Exception exception, K converter, bool keepDuplicates = false)
           where K : IDataKeyConverter
        {
            if (keepDuplicates)
                return DoCrawlData(exception, converter);
            return DoCrawlUniqueData(exception, converter);
        }

        public static IEnumerable<DictionaryEntry> CrawlData<K>(this Exception exception, bool keepDuplicates = false)
            where K : IDataKeyConverter
        {
            if (keepDuplicates)
                return DoCrawlData(exception, DataKeyConverter.Default);
            return DoCrawlUniqueData(exception, DataKeyConverter.Default);
        }

        public static Exception CaptureCaller<K>(this Exception exception, K converter)
            where K : IDataKeyConverter
        {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var key = converter.ConvertTo("caller");
            exception.Data[key] = method.Name + " of " + method.DeclaringType.FullName;
            return exception;
        }

        /// <summary>
        /// Captures the name of the caller in the "caller" Data property of the exception.
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>The given exception</returns>
        public static Exception CaptureCaller(this Exception exception)
        {
            return CaptureCaller(exception, DataKeyConverter.Default);
        }

        public static Exception CaptureContext<K>(this Exception exception, K converter, object context)
            where K : IDataKeyConverter
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(context)) {
                var value = descriptor.GetValue(context);
                var stringify = value != null && !descriptor.PropertyType.IsSerializable;
                var key = converter.ConvertTo(descriptor.Name);
                exception.Data[key] = stringify
                    ? value.ToString()
                    : value;
            }
            return exception;
        }

        /// <summary>
        /// Captures the properties of the context in the Data roperty of the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context.</param>
        /// <returns>The given exception.</returns>
        public static Exception CaptureContext(this Exception exception, object context)
        {
            return CaptureContext(exception, DataKeyConverter.Default, context);
        }

        public static Exception RemoveCapturedEntry<K>(this Exception exception, K converter, string name, bool crawlInnerExceptions = false)
            where K : IDataKeyConverter
        {
            var key = converter.ConvertTo(name);
            do {
                exception.Data.Remove(key);
            } while (crawlInnerExceptions && (exception = exception.InnerException) != null);
            return exception;
        }
        public static Exception RemoveCapturedEntry(this Exception exception, string name, bool crawlInnerExceptions = false)
        {
            return RemoveCapturedEntry(exception, DataKeyConverter.Default, name, crawlInnerExceptions);
        }

        public static object GetCapturedEntry<K>(this Exception exception, K converter, string name, bool crawlInnerExceptions = false)
            where K : IDataKeyConverter
        {
            var key = converter.ConvertTo(name);
            do {
                var value = exception.Data[key];
                if (value != null)
                    return value;
            } while (crawlInnerExceptions && (exception = exception.InnerException) != null);
            return null;
        }
        public static object GetCapturedEntry(this Exception exception, string name, bool crawlInnerExceptions = false)
        {
            return GetCapturedEntry(exception, DataKeyConverter.Default, name, crawlInnerExceptions);
        }

        public static void RethrowWithNoStackTraceLoss(this Exception exception)
        {
            // see <http://bradwilson.typepad.com/blog/2008/04/small-decisions.html>
            var remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString.SetValue(exception, exception.StackTrace);
            throw exception;
        }
    }
}
