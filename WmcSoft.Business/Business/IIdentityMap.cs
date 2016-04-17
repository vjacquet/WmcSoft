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

namespace WmcSoft.Business
{
    /// <summary>
    /// Looks up instances using the map when referring to them.
    /// </summary>
    /// <typeparam name="TInstance">The type of instances.</typeparam>
    /// <typeparam name="TId">The type of the identity.</typeparam>
    /// <remarks>See http://martinfowler.com/eaaCatalog/identityMap.html </remarks>
    public interface IIdentityMap<TInstance, TId>
    {
        TId Register(TInstance instance);
        TInstance Get(TId id);
    }
}
