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

using System.Collections.Generic;

namespace WmcSoft.Business.PartyModel
{
    public class WeightedPreferenceCollection : IDictionary<Preference, double>
    {
        #region Private Fields

        Dictionary<Preference, double> inner;

        #endregion

        #region Lifecycle

        public WeightedPreferenceCollection() {
            inner = new Dictionary<Preference, double>();
        }

        #endregion

        #region IDictionary<Preference,double> Members

        public void Add(Preference option, double weight) {
            inner.Add(option, weight);
        }

        public bool ContainsOption(Preference option) {
            return inner.ContainsKey(option);
        }

        bool IDictionary<Preference, double>.ContainsKey(Preference key) {
            return inner.ContainsKey(key);
        }

        ICollection<Preference> IDictionary<Preference, double>.Keys {
            get {
                return inner.Keys;
            }
        }

        public ICollection<Preference> Options {
            get {
                return inner.Keys;
            }
        }

        public bool Remove(Preference option) {
            return inner.Remove(option);
        }

        public bool TryGetWeight(Preference option, out double weight) {
            return inner.TryGetValue(option, out weight);
        }

        bool IDictionary<Preference, double>.TryGetValue(Preference option, out double weight) {
            return inner.TryGetValue(option, out weight);
        }

        ICollection<double> IDictionary<Preference, double>.Values {
            get { return inner.Values; }
        }

        public ICollection<double> Weights {
            get { return inner.Values; }
        }

        public double this[Preference key] {
            get { return inner[key]; }
            set { inner[key] = value; }
        }

        #endregion

        #region ICollection<KeyValuePair<Preference,double>> Members

        void ICollection<KeyValuePair<Preference, double>>.Add(KeyValuePair<Preference, double> item) {
            ((ICollection<KeyValuePair<Preference, double>>)inner).Add(item);
        }

        void ICollection<KeyValuePair<Preference, double>>.Clear() {
            ((ICollection<KeyValuePair<Preference, double>>)inner).Clear();
        }

        bool ICollection<KeyValuePair<Preference, double>>.Contains(KeyValuePair<Preference, double> item) {
            return ((ICollection<KeyValuePair<Preference, double>>)inner).Contains(item);
        }

        void ICollection<KeyValuePair<Preference, double>>.CopyTo(KeyValuePair<Preference, double>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<Preference, double>>)inner).CopyTo(array, arrayIndex);
        }

        int ICollection<KeyValuePair<Preference, double>>.Count {
            get {
                return ((ICollection<KeyValuePair<Preference, double>>)inner).Count;
            }
        }

        bool ICollection<KeyValuePair<Preference, double>>.IsReadOnly {
            get {
                return ((ICollection<KeyValuePair<Preference, double>>)inner).IsReadOnly;
            }
        }

        bool ICollection<KeyValuePair<Preference, double>>.Remove(KeyValuePair<Preference, double> item) {
            return ((ICollection<KeyValuePair<Preference, double>>)inner).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<Preference,double>> Members

        IEnumerator<KeyValuePair<Preference, double>> IEnumerable<KeyValuePair<Preference, double>>.GetEnumerator() {
            return inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)inner).GetEnumerator();
        }

        #endregion
    }
}