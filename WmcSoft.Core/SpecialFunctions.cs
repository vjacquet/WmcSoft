namespace WmcSoft
{
    internal class SpecialFunctions<TElement>
    {
        public static TElement Identity(TElement x)
        {
            return x;
        }

        public static bool Always(TElement x)
        {
            return true;
        }

        public static bool Never(TElement x)
        {
            return false;
        }
    }
}
