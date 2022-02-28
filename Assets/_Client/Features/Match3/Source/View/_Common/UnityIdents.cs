namespace Client.Match3
{
    public static class SimConstants
    {
        public const int GridSubdivison = 31;
        public const int LineMatchLength = 3;
        public const int GravityAmount = 1;
        public static int MaxVelocity => GridSubdivison / 2;
    }

    public static class UnityIdents
    {
        public static class Time
        {
            public const float FixedDelta = 0.02f; 
        }

        public static class Layers
        {

        }

        public static class Tags
        {
            // Tags are unnecessary for now...
        }
    }
}
