using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Collections.Generic
{
	public interface PriorityQueueElement : IComparable
	{
		int PQIndex
		{
			get;
			set;
		}
	}

	public class PriorityQueue //:  ICollection
	{
		public List<PriorityQueueElement> data = null;

		public PriorityQueue()
		{
			data = new List<PriorityQueueElement>();
		}
		public PriorityQueue(int initSize)
		{
			data = new List<PriorityQueueElement>(initSize);
		}

		public bool IsEmpty()
		{
			return (data.Count==0);
		}
		public void Clear()
		{
			data.Clear();
		}
		public void Insert(PriorityQueueElement obj)
		{
			int hole = data.Count;
			data.Add(obj);
			while (hole>0 && obj.CompareTo(data[(hole-1)/2])<0)
			{
				data[hole] = data[(hole-1)/2];
				data[hole].PQIndex = hole;
				hole = (hole-1) / 2;
			}
			data[hole] = obj;
			obj.PQIndex = hole;
		}
		public PriorityQueueElement DeleteMin()
		{
			if (IsEmpty()) throw new Exception();
			PriorityQueueElement obj = data[0];

			data[0] = data[data.Count-1];
			data.RemoveAt(data.Count-1);
			if (data.Count > 0)
			{
				data[0].PQIndex = 0;
				PercolateDown(0);
			}

			return obj;
		}
		public PriorityQueueElement GetMin()
		{
			if (IsEmpty()) throw new Exception();
			return data[0];
		}

		public void Update(PriorityQueueElement obj)
		{
			PercolateUp(obj.PQIndex);
			PercolateDown(obj.PQIndex);
		}

		private void PercolateUp(int hole)
		{
			PriorityQueueElement obj = data[hole];
			while (hole>0 && obj.CompareTo(data[(hole-1)/2])<0)
			{
				int parent = (hole-1) / 2;
				PriorityQueueElement parentObj = data[parent];
				data[hole] = data[parent];
				parentObj.PQIndex = hole;
				hole = parent;
			}
			data[hole] = obj;
			obj.PQIndex = hole;
		}
		private void PercolateDown(int hole)
		{
			PriorityQueueElement obj = data[hole];
			while ((hole*2+1) < data.Count)
			{
				int child = hole*2 + 1;
				if (child != data.Count-1)
				{
					if (data[child+1].CompareTo(data[child])<0)
						child++;
				}
				PriorityQueueElement childObj = data[child];
				if (childObj.CompareTo(obj) < 0)
				{
					data[hole] = childObj;
					childObj.PQIndex = hole;
					hole = child;
				}
				else break;
			}
			data[hole] = obj;
			obj.PQIndex = hole;
		}
	}
}
