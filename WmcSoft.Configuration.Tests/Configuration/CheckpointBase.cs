using System;
using System.ComponentModel;

namespace WmcSoft.Configuration
{
    public abstract class CheckpointBase : ICheckpoint
    {
        protected CheckpointBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }
        public virtual int MinimumLevel => 0;

        public bool Verify(int level)
        {
            if (level < MinimumLevel)
                return true;

            try {
                return DoVerify(level);
            } catch (WarningException ) {
                return false;
            } catch (Exception ) {
                return false;
            }
        }

        protected abstract bool DoVerify(int level);
    }
}
