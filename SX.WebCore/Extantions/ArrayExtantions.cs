using System;

namespace SX.WebCore.Extantions
{
    public static class ArrayExtantions
    {
        /// <summary>
        /// Удалить елемент из массива
        /// </summary>
        /// <typeparam name="T">Тип элементов массива</typeparam>
        /// <param name="arr">Ссылка на массив</param>
        /// <param name="index">Индекс удаляемого элемента</param>
        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int i = index; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            Array.Resize(ref arr, arr.Length - 1);
        }
    }
}
