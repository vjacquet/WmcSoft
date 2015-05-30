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
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WmcSoft.AI.FuzzyLogic
{
    public partial class Fuzzifier : Component
    {
        private readonly Random _random;
        private readonly List<IFuzzyCategory> _list;

        public Fuzzifier(Random random) {
            _list = new List<IFuzzyCategory>();
            _random = random;

            InitializeComponent();
        }

        public Fuzzifier()
            : this(new Random()) {
        }


        public Fuzzifier(IContainer container)
            : this() {
            container.Add(this);
        }

        public ICollection<IFuzzyCategory> Categories {
            get { return _list; }
        }

        public IFuzzyCategory Interpret(double value) {
            // compute the cumulated probability
            double[] cumulated = new double[_list.Count];
            double total = 0.0;
            int index = 0;
            foreach (IFuzzyCategory category in _list) {
                double membership = category.EvaluateMembership(value);
                total += membership;
                cumulated[index++] = total;
            }

            // find the value
            double alea = _random.NextDouble() * total;
            int found = Array.BinarySearch<double>(cumulated, alea);
            return (found >= 0)
                ? _list[found]
                : _list[~found];
        }
    }
}
