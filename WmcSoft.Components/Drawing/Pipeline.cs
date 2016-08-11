#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Drawing;
using System.Linq;

namespace WmcSoft.Drawing
{
    public class Pipeline : ImageTransformation, ICollection<ImageTransformation>
    {
        private readonly List<ImageTransformation> _transformations;

        Pipeline() {
            _transformations = new List<ImageTransformation>();
        }

        public int Count {
            get {
                return _transformations.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return ((ICollection<ImageTransformation>)_transformations).IsReadOnly;
            }
        }

        public bool PreserveProperties { get; set; }

        public void Add(ImageTransformation item) {
            _transformations.Add(item);
        }

        public override Image Apply(Image image) {
            Image result = image;
            if (_transformations.Any()) {
                var properties = image.PropertyItems;
                foreach (var transform in _transformations) {
                    var temp = transform.Apply(result);
                    if (temp != result) {
                        result.Dispose();
                        result = temp;
                    }
                }
                if (PreserveProperties && result != image) {
                    for (int i = 0; i < properties.Length; i++) {
                        try {
                            result.SetPropertyItem(properties[i]);
                        }
                        catch (ArgumentException) {
                        }
                    }
                }
            }
            return result;
        }

        public void Clear() {
            _transformations.Clear();
        }

        public bool Contains(ImageTransformation item) {
            return _transformations.Contains(item);
        }

        public void CopyTo(ImageTransformation[] array, int arrayIndex) {
            _transformations.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ImageTransformation> GetEnumerator() {
            return ((ICollection<ImageTransformation>)_transformations).GetEnumerator();
        }

        public bool Remove(ImageTransformation item) {
            return _transformations.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((ICollection<ImageTransformation>)_transformations).GetEnumerator();
        }
    }
}