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
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ContextMenuStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripMonthCalendar : ToolStripControlHost
    {
        #region Lifecycle

        public ToolStripMonthCalendar()
            : base(new MonthCalendar()) {
        }

        #endregion

        #region Properties

        public MonthCalendar MonthCalendar {
            get { return Control as MonthCalendar; }
        }

        public Day FirstDayOfWeek {
            get { return MonthCalendar.FirstDayOfWeek; }
            set { value = MonthCalendar.FirstDayOfWeek; }
        }

        public void AddBoldedDate(DateTime dateToBold) {
            MonthCalendar.AddBoldedDate(dateToBold);
        }

        #endregion

        #region Events

        // Subscribe and unsubscribe the control events you wish to expose.
        protected override void OnSubscribeControlEvents(Control c) {
            base.OnSubscribeControlEvents(c);

            var monthCalendarControl = (MonthCalendar)c;
            monthCalendarControl.DateChanged += new DateRangeEventHandler(OnDateChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c) {
            base.OnUnsubscribeControlEvents(c);

            var monthCalendarControl = (MonthCalendar)c;
            monthCalendarControl.DateChanged -= new DateRangeEventHandler(OnDateChanged);
        }

        /// <summary>
        /// The DateChanged event.
        /// </summary>
        public event DateRangeEventHandler DateChanged {
            add { this.Events.AddHandler(DateChangedEvent, value); }
            remove { this.Events.RemoveHandler(DateChangedEvent, value); }
        }
        static object DateChangedEvent = new Object();

        /// <summary>
        /// Raise the DateChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDateChanged(object sender, DateRangeEventArgs e) {
            DateRangeEventHandler handler = this.Events[DateChangedEvent] as DateRangeEventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}