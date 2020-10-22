using System;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// A collection of CellStatus arrays and associated methods
    /// Inspired by Circular Storage examples provided by teaching staff
    /// at QUT.
    /// </summary>
    public class UniverseMemory
    {
        private readonly List<CellStatus[,]> dataStore;
        private readonly int height;
        private readonly int width;
        private readonly int memoryLimit;


        /// <summary>
        ///     Creates a new UniverseMemory instance.
        /// </summary>
        /// <param name="limit">
        ///     The maximum number of elements that can be stored.
        /// <param name="settings">
        ///     System settings for information on the size of the CellStatus arrays
        /// </param>
        public UniverseMemory(int limit, Settings settings)
        {
            memoryLimit = limit;
            Count = 0;
            height = settings.height;
            width = settings.width;
            dataStore = new List<CellStatus[,]>();
        }
        /// <summary>
        ///     Accesses the number of elements in memory.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Accesses an element of the UniverseMemory by index. If the index exceeds
        ///     the limits of the UniverseMemory, it will "wrap around".
        /// </summary>
        /// <param name="index">
        ///     The index of the element to be accessed.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        ///     If this instance is empty (has 0 elements):
        ///     *   Exception message will be "Storage instance is empty.".
        /// </exception>
        /// <returns>
        ///     An access reference to the element of the UniverseMemory.  
        /// </returns>
        public CellStatus[,] this[int index]
        {
            get
            {
                if (Count == 0)
                {
                    throw new IndexOutOfRangeException("Storage instance is empty.");
                }
                return dataStore[index % Count];
            }
            set
            {
                if (index == 0)
                {
                    throw new IndexOutOfRangeException("Storage instance is empty.");
                }
                dataStore[index % Count] = value;
            }
        }

        /// <summary>
        ///     Adds an item to the start of the UniverseMemory (all other elements 
        ///     are shift one element forward).
        /// </summary>
        /// <param name="item">
        ///     The item to be added.
        /// </param>
        /// </returns>
        public void Add(CellStatus[,] item)
        {
            if (Count < memoryLimit)
            {
                dataStore.Insert(0, item);
                Count++;
            }
            else
            {
                dataStore.RemoveAt(Count - 1);
                dataStore.Insert(0, item);
            }
        }

        /// <summary>
        ///     Searches the Memory for the first element that 
        ///     matches the item.
        /// </summary>
        /// <param name="item">
        ///     A 2D CellStatus array to search for
        /// </param>
        /// <returns>
        ///     The index of the first element that matches the item, if 
        ///     there are no matches it will return -1.
        /// </returns>
        public int Search(CellStatus[,] item)
        {
            for (int i = 0; i < Count; i++)
            {
                bool itemEquals = true;
                for (int row = 0; row < height; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        if (dataStore[i][row, column] != item[row, column])
                        {
                            itemEquals = false;
                            break;
                        }
                    }
                    if (!itemEquals) break;
                }
                if (itemEquals)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}