using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft.Threading
{
    public static class JobInstrumentationProvider
    {
        #region NullInstrumentationProvider class

        /// <summary>
        /// Represents an implementation of <see cref="IJobInstrumentationProvider"/> that does nothing.
        /// </summary>
        class NullInstrumentationProvider : IJobInstrumentationProvider
        {
            #region IJobDisptacherInstrumentationProvider Membres

            public void FireJobDispatched()
            {
            }

            public void FireJobExecuted()
            {
            }

            public void FireJobCompleted()
            {
            }

            public void FireJobFailed(Exception exception)
            {
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Returns an implementation of <see cref="IJobInstrumentationProvider"/> that does nothing.
        /// </summary>
        public static readonly IJobInstrumentationProvider Null = new NullInstrumentationProvider();
    }
}
