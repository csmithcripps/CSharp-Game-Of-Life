using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// A collection that is circular in nature. The first and last elements
    /// of this collection are considered neighbours. This collection only stores
    /// value types.
    /// </summary>
    class UniverseMemory
    {
        private List<CellStatus[,]> dataStore;
        private int count;
        private int height;
        private int width;
        private int memoryLimit;

        /// <summary>
        ///     Creates a new CircularStorage instance.
        /// </summary>
        /// <param name="limit">
        ///     The maximum number of elements that can be stored in the CircularStorage.
        /// </param>
        public UniverseMemory(int limit, ref Settings settings)
        {
            memoryLimit = limit;
            count = 0;
            height = settings.height;
            width = settings.width;
            dataStore = new List<CellStatus[,]>();
        }

        /// <summary>
        ///     Accesses the capacity of the CircularStorage (get only).
        /// </summary>
        public int MemoryLimit
        {
            get
            {
                return memoryLimit;
            }
        }

        /// <summary>
        ///     Accesses the number of elements in the CircularStorage (get only).
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        ///     Accesses an element of the CircularStorage by index. If the index exceeds
        ///     the limits of the CircularStorage, it will "wrap around".
        /// </summary>
        /// <param name="index">
        ///     The index of the element to be accessed.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        ///     If CircularStorage instance is empty (has 0 elements):
        ///     *   Exception message will be "CircularStorage instance is empty.".
        /// </exception>
        /// <returns>
        ///     An access reference to the element of the CircularStorage.  
        /// </returns>
        public CellStatus[,] this[int index]
        {
            get
            {
                if (count == 0)
                {
                    throw new IndexOutOfRangeException("CircularStorage instance is empty.");
                }
                return dataStore[index % count];
            }
            set
            {
                if (index == 0)
                {
                    throw new IndexOutOfRangeException("CircularStorage instance is empty.");
                }
                dataStore[index % count] = value;
            }
        }

        /// <summary>
        ///     Adds an item to the start of the CircularStorage (all other elements 
        ///     are shift one element forward).
        /// </summary>
        /// <param name="item">
        ///     The item to be added.
        /// </param>
        /// <returns>
        ///     True if the item could be added (if there is still space in the 
        ///     CircularStorage), false otherwise.
        /// </returns>
        public bool Add(CellStatus[,] item)
        {
            if (count < memoryLimit)
            {
                dataStore.Insert(0, item);
                count++;
                return true;
            }
            else
            {
                dataStore.RemoveAt(count - 1);
                dataStore.Insert(0, item);
                return true;
            }
        }

        /// <summary>
        ///     Searches the CircularStorage for the first element that 
        ///     matches the predicate.
        /// </summary>
        /// <param name="predicate">
        ///     Predicate method used to search.
        /// </param>
        /// <returns>
        ///     The index of the first element that matches the predicate, if 
        ///     there are no matches it will return -1.
        /// </returns>
        public int Search(CellStatus[,] item)
        {
            for (int i = 0; i < count; i++)
            {
                bool itemEquals = true;
                for (int row = 0; row < height; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        if (dataStore[i][row, column] != item[row, column])
                        {
                            itemEquals = false;
                        }
                    }
                }
                if (itemEquals)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Returns a string representation of the CircularStorage using brace notation.
        /// </summary>
        /// <returns>
        ///     A string representation of the CircularStorage
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");
            for (int i = 0; i < count; i++)
            {
                builder.Append(dataStore[i]);
                if (i < count - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(" }");
            return builder.ToString();
        }
    }
}