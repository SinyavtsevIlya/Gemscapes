namespace Client
{
    public struct Health
    {
        public FInt Value;

        public static implicit operator FInt(Health src)
        {
            return src.Value;
        }
    }
}