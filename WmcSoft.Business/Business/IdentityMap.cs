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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WmcSoft.Business
{
    /// <summary>
    /// Looks up instances using the map when referring to them.
    /// </summary>
    /// <remarks>See http://martinfowler.com/eaaCatalog/identityMap.html</remarks>
    public class IdentityMap : IIdentityMap<object, long>
    {
        #region Private

        private readonly IDictionary<long, object> _map;
        private readonly ObjectIDGenerator _generator;

        #endregion

        public IdentityMap()
            : this(new Dictionary<long, object>(), new ObjectIDGenerator()) {
        }

        public IdentityMap(IDictionary<long, object> manager, ObjectIDGenerator generator) {
            _map = manager;
            _generator = generator;
        }

        public long Register(object instance) {
            if (instance == null)
                throw new ArgumentNullException("instance");

            bool firstTime;
            long id = _generator.GetId(instance, out firstTime);
            if (firstTime)
                _map.Add(id, instance);

            return id;
        }

        public object Get(long id) {
            object obj;
            if (_map.TryGetValue(id, out obj))
                return obj;
            return null;
        }
    }
}
