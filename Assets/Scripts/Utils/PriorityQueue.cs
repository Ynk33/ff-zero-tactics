using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public List<Tuple<T, double>> Elements
    {
        get { return elements; }
    }

    public void Enqueue(T item, double priorityValue)
    {
        elements.Add(Tuple.Create(item, priorityValue));
    }

    public T Dequeue()
    {
        int bestPriorityIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
            {
                bestPriorityIndex = i;
            }
        }

        T bestItem = elements[bestPriorityIndex].Item1;
        elements.RemoveAt(bestPriorityIndex);
        return bestItem;
    }

    public T Peek()
    {
        int bestPriorityIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestPriorityIndex].Item2)
            {
                bestPriorityIndex = i;
            }
        }

        T bestItem = elements[bestPriorityIndex].Item1;
        return bestItem;
    }

    public void Remove(T item, double priorityValue)
    {
        int tupleIndex = -1;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item1.Equals(item) && elements[i].Item2 == priorityValue)
            {
                tupleIndex = i;
            }
        }

        if (tupleIndex >= 0)
        {
            elements.RemoveAt(tupleIndex);
        }
    }
}
