using System;
using System.Collections;

namespace Model {

    public static class Util {

        private static readonly Random rnd = new Random();

        public static Random GetRandom() {
            return rnd;
        }

        public static void Shuffle(this IList list) {
            int size = list.Count;
            while (size > 1) {
                size--;
                int index = rnd.Next(size + 1);
                Object value = list[index];
                list[index] = list[size];
                list[size] = value;
            }
        }
    }
}
