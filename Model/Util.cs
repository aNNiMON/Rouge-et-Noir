using System;
using System.Collections.Generic;
using System.Linq;

namespace Model {

    public static class Util {

        private static readonly Random rnd = new Random();

        public static Random GetRandom() {
            return rnd;
        }

        public static void Shuffle<T>(this IList<T> list) {
            int size = list.Count;
            while (size > 1) {
                size--;
                int index = rnd.Next(size + 1);
                T value = list[index];
                list[index] = list[size];
                list[size] = value;
            }
        }

        /// <summary>
        /// Переместить объекты из одного списка в другой.
        /// ОБъекты берутся из начала исходного списка
        /// и добавляются в конец результирующего.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from">список, из которого нужно переместить объекты</param>
        /// <param name="to">результирующий список</param>
        /// <param name="length">количество объектов для перемещения</param>
        public static void Move<T>(List<T> from, List<T> to, int length) {
            var selected = from.Skip(Math.Max(0, from.Count() - length)).Take(length).ToList();
            selected.ForEach(item => from.Remove(item));
            to.AddRange(selected);
        }
    }
}
