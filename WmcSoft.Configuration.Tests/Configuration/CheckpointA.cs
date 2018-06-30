namespace WmcSoft.Configuration
{
    public class CheckpointA : CheckpointBase
    {
        public CheckpointA(string name) : base(name)
        {
        }

        protected override bool DoVerify(int level)
        {
            return true;
        }
    }
}
