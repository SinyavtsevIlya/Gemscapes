namespace Client
{
    public struct Health
    {
        public int Value;

        public static implicit operator int(Health src)
        {
            return src.Value;
        }
    }
}