using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepetierHost.model
{
    /// <summary>
    /// This interface represents a subset of the methods of a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface BasicList<T> : IEnumerable<T>
    {
        int Count
        {
            get;
        }
        T Last
        {
            get;
        }
        void AddLast(T o);
        T ElementAt(int index);

        BasicList<T> SkipAsList(int offset);
        IEnumerable<T> Skip(int offset);
        BasicList<T> TakeAsList(int count);
        IEnumerable<T> Take(int count);
    }

    /// <summary>
    /// This class is an implementation in memory of the basic list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryLinkedListBasicList<T> : BasicList<T>
    {
        protected LinkedList<T> list;
        public InMemoryLinkedListBasicList(LinkedList<T> list)
        {
            this.list = list;
        }
        public InMemoryLinkedListBasicList()
        {
            list = new LinkedList<T>();
        }
        public int Count
        {
            get { return list.Count; }
        }
        public T Last
        {
            get { return list.Last.Value; }
        }
        public void AddLast(T o)
        {
            list.AddLast(o);
        }

        public T ElementAt(int index)
        {
            return list.ElementAt(index);
        }

        public BasicList<T> SkipAsList(int offset)
        {
            return new InMemoryLinkedListBasicList<T>(new LinkedList<T>(list.Skip(offset)));
        }
        public IEnumerable<T> Skip(int offset)
        {
            return list.Skip(offset);
        }
        public BasicList<T> TakeAsList(int count)
        {
            return new InMemoryLinkedListBasicList<T>(new LinkedList<T>(list.Take(count)));
        }
        public IEnumerable<T> Take(int count)
        {
            return list.Take(count);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
