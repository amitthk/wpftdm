using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpftdm.Util
{
    public static class ListHelpers
    {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items,
    Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        public static void MoveForward<T>(this List<T> list, Predicate<T> itemSelector, bool isLastToBeginning)
        {
            if (list==null)
            {
                throw new Exception("List cannot be null");
            }

            if (itemSelector==null)
            {
                throw new Exception("ItemSelector cannor be null");
            }

            var currentIndex = list.FindIndex(itemSelector);

            // Copy the current item
            var item = list[currentIndex];

            bool isLast = list.Count - 1 == currentIndex;

            if (isLastToBeginning && isLast)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item to the beginning
                list.Insert(0, item);
            }
            else if (!isLast)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item at next index
                list.Insert(currentIndex + 1, item);
            }
        }

        public static void MoveBack<T>(this List<T> list, Predicate<T> itemSelector, bool isFirstToEnd)
        {
            if (list == null)
            {
                throw new Exception("List cannot be null");
            }

            if (itemSelector == null)
            {
                throw new Exception("ItemSelector cannor be null");
            }

            var currentIndex = list.FindIndex(itemSelector);

            // Copy the current item
            var item = list[currentIndex];

            bool isFirst = 0 == currentIndex;

            if (isFirstToEnd && isFirst)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item to the end
                list.Add(item);
            }
            else if (!isFirstToEnd)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item to previous index
                list.Insert(currentIndex - 1, item);
            }
        }

        public static void MoveToPosition<T>(this List<T> list, Predicate<T> itemSelector, Predicate<T> newPositionSelector, bool isLastToBeginning)
        {
            if (list == null)
            {
                throw new Exception("List cannot be null");
            }

            if (itemSelector == null)
            {
                throw new Exception("ItemSelector cannor be null");
            }

            var currentIndex = list.FindIndex(itemSelector);

            // Copy the current item
            var item = list[currentIndex];

            var newIndex = list.FindIndex(newPositionSelector);

            bool isLast = list.Count - 1 == currentIndex;

            if (isLastToBeginning && isLast)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item to the beginning
                list.Insert(0, item);
            }
            else if (!isLast)
            {
                // Remove the item
                list.RemoveAt(currentIndex);

                // add the item at next index
                list.Insert(newIndex + 1, item);
            }
        }
    }
}
