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


namespace WmcSoft.Business.OrderModel
{
    /// <summary>
    /// Represents an event sent to an <see cref="Order"/> that changes its <see cref="OrderStatus"/>.
    /// </summary>
    public abstract class LifecycleEvent : OrderEvent
    {
        protected LifecycleEvent(OrderIdentifier identifier)
            : base(identifier) {
        }
    }

    /// <summary>
    /// Represents an event sent to an <see cref="Order"/> that changes its <see cref="OrderStatus"/> to <see cref="OrderStatus.Open"/>.
    /// </summary>
    public class OpenEvent : LifecycleEvent
    {
        public OpenEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }
    }

    /// <summary>
    /// Represents an event sent to an <see cref="Order"/> that changes its <see cref="OrderStatus"/> to <see cref="OrderStatus.Closed"/>.
    /// </summary>
    public class CloseEvent : LifecycleEvent
    {
        public CloseEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }
    }

    /// <summary>
    /// Represents an event sent to an <see cref="Order"/> that changes its <see cref="OrderStatus"/> to <see cref="OrderStatus.Cancelled"/>.
    /// </summary>
    public class CancelEvent : LifecycleEvent
    {
        public CancelEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }

        public string Reason { get; set; }
    }
}