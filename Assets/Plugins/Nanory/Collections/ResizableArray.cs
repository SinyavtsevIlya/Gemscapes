using System;

namespace Nanory.Collections
{
    public class ResizableArray<T>
    {
        public T[] Values;
        public int Count;

        public ResizableArray(int capacity)
        {
            Values = new T[capacity];
            Count = 0;
        }

        public T this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        public void Add(T item)
        {
            if (Values.Length == Count)
            {
                Array.Resize(ref Values, Values.Length << 1);
            }
            Values[Count++] = item;
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}