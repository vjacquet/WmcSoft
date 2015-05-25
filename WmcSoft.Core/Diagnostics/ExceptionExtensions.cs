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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace WmcSoft.Diagnostics
{
    public static class ExceptionExtensions
    {
        static IEnumerable<DictionaryEntry> DoCrawlData<K>(this Exception exception, K converter)
            where K : IDataKeyConverter {
            while (exception != null) {
                foreach (DictionaryEntry entry in exception.Data) {
                    if (converter.IsSupported(entry))
                        yield return entry;
                }
                exception = exception.InnerException;
            }
        }

        static IEnumerable<DictionaryEntry> DoCrawlUniqueData<K>(this Exception exception, K converter)
           where K : IDataKeyConverter {
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
           where K : IDataKeyConverter {
            if (keepDuplicates)
                return exception.DoCrawlData(converter);
            return exception.DoCrawlUniqueData(converter);
        }
        public static IEnumerable<DictionaryEntry> CrawlData<K>(this Exception exception, bool keepDuplicates = false)
            where K : IDataKeyConverter {
            if (keepDuplicates)
                return exception.DoCrawlData(DataKeyConverter.Default);
            return exception.DoCrawlUniqueData(DataKeyConverter.Default);
        }

        public static Exception CaptureCaller<K>(this Exception exception, K converter)
            where K : IDataKeyConverter {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var key = converter.ToKey("caller");
            exception.Data[key] = method.Name + " of " + method.DeclaringType.FullName;
            return exception;
        }
        public static Exception CaptureCaller(this Exception exception) {
            return exception.CaptureCaller(DataKeyConverter.Default);
        }

        public static Exception CaptureContext<K>(this Exception exception, K converter, object context)
            where K : IDataKeyConverter {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(context)) {
                var value = descriptor.GetValue(context);
                var stringify = value != null && !descriptor.PropertyType.IsSerializable;
                var key = converter.ToKey(descriptor.Name);
                exception.Data[key] = stringify
                    ? value.ToString()
                    : value;
            }
            return exception;
        }
        public static Exception CaptureContext(this Exception exception, object context) {
            return exception.CaptureContext(DataKeyConverter.Default, context);
        }

        public static Exception RemoveCapturedEntry<K>(this Exception exception, K converter, string name, bool crawlInnerExceptions = false)
            where K : IDataKeyConverter {
            var key = converter.ToKey(name);
            do {
                exception.Data.Remove(key);
            } while (crawlInnerExceptions && (exception = exception.InnerException) != null);
            return exception;
        }
        public static Exception RemoveCapturedEntry(this Exception exception, string name, bool crawlInnerExceptions = false) {
            return exception.RemoveCapturedEntry(DataKeyConverter.Default, name, crawlInnerExceptions);
        }

        public static object GetCapturedEntry<K>(this Exception exception, K converter, string name, bool crawlInnerExceptions = false)
            where K : IDataKeyConverter {
            var key = converter.ToKey(name);
            do {
                var value = exception.Data[key];
                if (value != null)
                    return value;
            } while (crawlInnerExceptions && (exception = exception.InnerException) != null);
            return null;
        }
        public static object GetCapturedEntry(this Exception exception, string name, bool crawlInnerExceptions = false) {
            return exception.GetCapturedEntry(DataKeyConverter.Default, name, crawlInnerExceptions);
        }
    }
}
