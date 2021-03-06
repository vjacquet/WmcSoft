﻿#region Licence

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
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a bag of items for which items are enumerated in random order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    public class RandomBag<T> : Bag<T>
    {
        #region RandomAdapter class

        /// <summary>
        /// Adapter to always take the last item and thus removing RandomBag randomness...
        /// </summary>
        class RandomAdapter : Random
        {
            public override int Next(int minValue, int maxValue)
            {
                return maxValue - 1;
            }

            public override int Next(int maxValue)
            {
                return maxValue - 1;
            }

            public override int Next()
            {
                throw new NotSupportedException();
            }

            public override void NextBytes(byte[] buffer)
            {
                throw new NotSupportedException();
            }

            public override double NextDouble()
            {
                throw new NotSupportedException();
            }
        }

        static readonly Random Default = new RandomAdapter();

        #endregion

        private readonly Random _random;

        public RandomBag() : this(Default)
        {
        }

        public RandomBag(int capacity) : this(capacity, Default)
        {
        }

        public RandomBag(IEnumerable<T> collection) : this(collection, Default)
        {
        }

        public RandomBag(Random random) : base()
        {
            _random = random;
        }

        public RandomBag(int capacity, Random random) : base(capacity)
        {
            _random = random;
        }

        public RandomBag(IEnumerable<T> collection, Random random) : base(collection)
        {
            _random = random;
        }

        public T Pick()
        {
            var last = Count;
            return PopAt(_random.Next(last));
        }

        public override Enumerator GetEnumerator()
        {
            var array = new T[Count];
            CopyTo(array, 0);
            array.Shuffle(_random);
            return new Enumerator(this, array);
        }
    }
}