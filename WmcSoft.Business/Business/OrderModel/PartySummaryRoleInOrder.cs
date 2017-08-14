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

using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.OrderModel
{
    /// <summary>
    /// Represents a role within the <see cref="Order"/> process that can be played by a <see cref="PartyModel.PartySummary"/>
    /// </summary>
    public enum PartySummaryRoleInOrder
    {
        // buying role
        // ---------------------------------

        /// <summary>
        /// Details of the <see cref="Party"/> that raises the <see cref="Order"/>.
        /// </summary>
        OrderInitiator,
        /// <summary>
        /// Details of the <see cref="Party"/> that pays the <see cref="Order"/>.
        /// </summary>
        Purchaser,
        /// <summary>
        /// Details of the <see cref="Party"/> that receives one or more items from the <see cref="Order"/>.
        /// </summary>
        OrderReceiver,
        /// <summary>
        /// Details of the <see cref="Party"/> that receives one item from the <see cref="Order"/>.
        /// </summary>
        OrderLineReceiver,

        // selling role
        // ---------------------------------

        /// <summary>
        /// Details of the <see cref="Party"/> that supplies the goods or services. 
        /// </summary>
        Vendor,
        /// <summary>
        /// Details of a third <see cref="Party"/> that accepted the <see cref="Order"/> on behalf of the <see cref="Vendor"/>.
        /// </summary>
        SalesAgent,
        /// <summary>
        /// Details of the <see cref="Party"/> that raises invoices and receives payment against the <see cref="Order"/>.
        /// </summary>
        PaymentReceiver
    }
}
