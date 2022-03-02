using Nanory.Lex;
using System.Collections.Generic;
using System;

namespace Client.Match3
{
    [Serializable]
    public struct InputRecord : IEcsAutoReset<InputRecord>
    {
        public string LevelName;
        public List<Frame> Frames;

        public void AutoReset(ref InputRecord c)
        {
            c.Frames?.Clear();
            c.Frames = null;
        }

        [Serializable]
        public struct Frame
        {
            public int Tick;
            public SwapPieceRequestSerialized Swap;
            // NOTE: more input can be stored here.

            [Serializable]
            public struct SwapPieceRequestSerialized
            {
                public int A;
                public int B;
            }
        }
    }
}