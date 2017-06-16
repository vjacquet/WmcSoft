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

 ****************************************************************************
 * this code is adapted from the series
 * <https://blogs.msdn.microsoft.com/mattwar/2008/11/18/linq-building-an-iqueryable-provider-series/>
 * 
 ****************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Linq.Sandbox
{
    internal class ObjectReader<T> : IEnumerable<T> where T : class, new() {
        Enumerator enumerator;

        internal ObjectReader(IDataReader reader) {
            this.enumerator = new Enumerator(reader);
        }

        public IEnumerator<T> GetEnumerator() {
            Enumerator e = this.enumerator;
            if (e == null) {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }
            this.enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            IDataReader reader;
            PropertyInfo[] fields;
            int[] fieldLookup;
            T current;

            internal Enumerator(IDataReader reader) {
                this.reader = reader;
                this.fields = typeof(T).GetProperties();
            }

            public T Current {
                get { return this.current; }
            }

            object IEnumerator.Current {
                get { return this.current; }
            }

            public bool MoveNext() {
                if (this.reader.Read()) {
                    if (this.fieldLookup == null) {
                        this.InitFieldLookup();
                    }
                    T instance = new T();
                    for (int i = 0, n = this.fields.Length; i < n; i++) {
                        int index = this.fieldLookup[i];
                        if (index >= 0) {
                            var fi = this.fields[i];
                            if (this.reader.IsDBNull(index)) {
                                fi.SetValue(instance, null);
                            } else {
                                fi.SetValue(instance, this.reader.GetValue(index));
                            }
                        }
                    }
                    this.current = instance;
                    return true;
                }
                return false;
            }

            public void Reset() {
            }

            public void Dispose() {
                this.reader.Dispose();
            }

            private void InitFieldLookup() {
                Dictionary<string, int> map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
                for (int i = 0, n = this.reader.FieldCount; i < n; i++) {
                    map.Add(this.reader.GetName(i), i);
                }
                this.fieldLookup = new int[this.fields.Length];
                for (int i = 0, n = this.fields.Length; i < n; i++) {
                    int index;
                    if (map.TryGetValue(this.fields[i].Name, out index)) {
                        this.fieldLookup[i] = index;
                    } else {
                        this.fieldLookup[i] = -1;
                    }
                }
            }
        }
    }
}
