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
using System.ComponentModel;
using System.Linq;

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// Cretes a scope in which the specified target wil not be notified of the givent events.
    /// </summary>
    /// <remarks>The calling sequence of the delegate is changed.</remarks>
    public class EventBarrier : IDisposable
    {
        EventHandlerList _sourceList;
        EventHandlerList _restorationList = new EventHandlerList();

        public EventBarrier(EventHandlerList eventHandlerList, object target, params object[] events) {
            _sourceList = eventHandlerList;

            foreach (object @event in events) {
                var sentinel = _sourceList[@event];
                if (sentinel == null)
                    continue;

                foreach (var handler in sentinel.GetInvocationList(d => d.Target == target)) {
                    _sourceList.RemoveHandler(@event, handler);
                    _restorationList.AddHandler(@event, handler);
                }
            }
        }

        #region IDisposable Membres

        public void Dispose() {
            _sourceList.AddHandlers(_restorationList);
        }

        #endregion
    }
}
