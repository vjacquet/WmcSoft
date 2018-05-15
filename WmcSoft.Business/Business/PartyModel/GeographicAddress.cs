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
using System.Globalization;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a geographic location at which a Party may be contacted.
    /// It is a postal address for the <see cref="Party"/>.
    /// </summary>
    public class GeographicAddress : AddressBase, IFormattable
    {
        // <www.upu.int>
        // <www.bitboost.com/ref/international-address-formats.html>

        #region Lifecycle

        public GeographicAddress()
        {
            Country = RegionInfo.CurrentRegion;
        }

        #endregion

        #region Properties

        public override string Address {
            get {
                // TODO: decide how to render the address considering the locale.
                return String.Empty;
            }
        }

        public string[] AddressLines { get; set; }
        public string Town { get; set; }
        public string RegionOrState { get; set; }
        public string ZipOrPostCode { get; set; }
        public RegionInfo Country { get; set; }

        #endregion

        #region IFormattable

        public sealed override string ToString()
        {
            return ToString("I", null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format) {
            case "l":
                return string.Join("\r\n", AddressLines);
            case "t":
                return (Town ?? "");
            case "T":
                return ToUpper(Town, Country);
            case "z":
            case "p":
                return (ZipOrPostCode ?? "");
            case "c":
                if (Country == null)
                    return "";
                return Country.NativeName;
            case "C":
                if (Country == null)
                    return "";
                return ToUpper(Country.NativeName, Country);
            case "r":
            case "s":
                return (RegionOrState ?? "");
            case "R":
            case "S":
                return ToUpper(RegionOrState, Country);
            case "O2":
                if (Country == null)
                    return "";
                return Country.TwoLetterISORegionName;
            case "O3":
                if (Country == null)
                    return "";
                return Country.ThreeLetterISORegionName;
            case "g":
            case "G":
            case "i":
            case "I":
                return Format(format, formatProvider?.GetFormat<GeographicAddressFormatInfo>() ?? GeographicAddressFormatInfo.Current);
            }
            return null;
        }

        string Format(string format, GeographicAddressFormatInfo gfi)
        {
            switch (format) {
            case "g":
                return string.Format(gfi.Resolve(Country), this);
            case "G":
                return string.Format(gfi.Resolve(Country).Replace(":t}", ":T}"), this);
            case "i":
                return string.Format(gfi.Resolve(Country) + "\r\n{0:C}", this);
            case "I":
                return string.Format(gfi.Resolve(Country).Replace(":t}", ":T}") + "\r\n{0:C}", this);
            }
            throw new InvalidOperationException();
        }

        static string ToUpper(string value, RegionInfo region)
        {
            if (value == null)
                return "";
            var culture = region != null ? new CultureInfo(region.ThreeLetterISORegionName) : CultureInfo.InvariantCulture;
            return culture.TextInfo.ToUpper(value);
        }

        #endregion
    }

    public class GeographicAddressFormatInfo : IFormatProvider, ICloneable
    {
        #region Registry

        static GeographicAddressFormatInfo()
        {
            var gfi = new GeographicAddressFormatInfo();
            gfi._registry.Add("", "{0:l}\r\n{0:t} {0:r} {0:z}");
            gfi._registry.Add("FR", "{0:l}\r\n{0:z} {0:t}");
            Current = Default = ReadOnly(gfi);
        }
        public static readonly GeographicAddressFormatInfo Default;

        public static GeographicAddressFormatInfo Current { get; set; }

        #endregion

        #region Address formats

        sealed class RegistryAdapter<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        {
            private readonly GeographicAddressFormatInfo _owner;

            public RegistryAdapter(GeographicAddressFormatInfo owner, IDictionary<TKey, TValue> storage)
            {
                _owner = owner;
                Readable = storage;
            }

            IDictionary<TKey, TValue> Readable { get; }

            IDictionary<TKey, TValue> Writable {
                get {
                    _owner.VerifyWritable();
                    return Readable;
                }
            }

            public ICollection<TKey> Keys {
                get { return Readable.Keys; }
            }

            public ICollection<TValue> Values {
                get { return Readable.Values; }
            }

            public int Count {
                get { return Readable.Count; }
            }

            public bool IsReadOnly {
                get { return false; }
            }

            public TValue this[TKey key] {
                get { return Readable[key]; }
                set { Writable[key] = value; }
            }

            public bool ContainsKey(TKey key)
            {
                return Readable.ContainsKey(key);
            }

            public void Add(TKey key, TValue value)
            {
                Writable.Add(key, value);
            }

            public bool Remove(TKey key)
            {
                return Writable.Remove(key);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return Readable.TryGetValue(key, out value);
            }

            public void Add(KeyValuePair<TKey, TValue> item)
            {
                Writable.Add(item);
            }

            public void Clear()
            {
                Writable.Clear();
            }

            public bool Contains(KeyValuePair<TKey, TValue> item)
            {
                return Readable.Contains(item);
            }

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                Readable.CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TKey, TValue> item)
            {
                return Writable.Remove(item);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return Readable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Readable.GetEnumerator();
            }

            IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
            IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
        }

        #endregion

        private readonly Dictionary<string, string> _registry = new Dictionary<string, string>();

        private readonly GeographicAddressFormatInfo _parent;

        private GeographicAddressFormatInfo()
        {
        }

        public GeographicAddressFormatInfo(GeographicAddressFormatInfo parent)
        {
            _parent = parent;
        }

        public bool IsReadOnly { get; private set; }

        public IDictionary<string, string> AddressFormats {
            get {
                return new RegistryAdapter<string, string>(this, _registry);
            }
        }

        internal string Resolve(RegionInfo region)
        {
            return Resolve(region.TwoLetterISORegionName) ?? Resolve("");
        }

        private string Resolve(string region)
        {
            var gfi = this;
            string format = null;
            while (gfi != null && !gfi._registry.TryGetValue(region, out format))
                gfi = gfi._parent;
            return format;
        }

        private void VerifyWritable()
        {
            if (!IsReadOnly)
                return;
            throw new InvalidOperationException();
        }

        public static GeographicAddressFormatInfo ReadOnly(GeographicAddressFormatInfo gfi)
        {
            if (gfi == null) throw new ArgumentNullException(nameof(gfi));
            if (gfi.IsReadOnly)
                return gfi;

            var clone = (GeographicAddressFormatInfo)gfi.MemberwiseClone();
            clone.IsReadOnly = true;
            return clone;
        }

        #region IFormatProvider

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(GeographicAddressFormatInfo))
                return this;
            return null;
        }

        #endregion

        #region ICloneable

        object ICloneable.Clone()
        {
            var clone = (GeographicAddressFormatInfo)MemberwiseClone();
            clone.IsReadOnly = false;
            return clone;
        }

        #endregion
    }
}
