using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft.Threading
{
    public interface IJobQueue
    {
        void Enqueue(IJob item);
        IJob Dequeue();
        bool TryDequeue(out IJob item, TimeSpan timeout);
        void Clear(Action<IJob> action);
    }
}
