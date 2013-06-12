using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Model {

    /// <summary>
    /// Учёт результатов игры.
    /// </summary>
    public class ScoreManager {

        private const string FILENAME = "scores.bin";
        private const int INCREMENT = 100;

        public static Score Current {
            get;
            set;
        }

        private static DateTime StartTime;

        private static List<Score> hiScores;
        public static List<Score> HiScores {
            get {
                return hiScores;
            }
        }

        public static TimeSpan GetGameTime() {
            if (StartTime == null || Current == null) return TimeSpan.Zero;
            Update();
            return Current.GameTime;
        }

        /// <summary>
        /// Увеличить результат.
        /// </summary>
        /// <param name="count">во сколько раз увеличить</param>
        public static void IncreaseScore(int count) {
            Current.ScoreValue += count * INCREMENT;
        }

        /// <summary>
        /// Уменьшить результат.
        /// </summary>
        /// <param name="count">во сколько раз уменьшить</param>
        public static void DecreaseScore(int count) {
            Current.ScoreValue -= count * INCREMENT;
            if (Current.ScoreValue < 0) Current.ScoreValue = 0;
        }

        public static void InitNewGame() {
            Load();
            Current = new Score {
                Date = DateTime.Now
            };

            StartTime = DateTime.Now;
        }

        public static void Update() {
            Current.GameTime = DateTime.Now - StartTime;
        }

        public static void EndGame(bool isComplete) {
            Current.GameTime = DateTime.Now - StartTime;
            Current.Complete = isComplete;

            hiScores.Add(Current);
            Save();
        }


        /// <summary>
        /// Сохранить результаты.
        /// </summary>
        public static void Save() {
            try {
                var stream = File.Open(FILENAME, FileMode.Create);
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, hiScores);
                stream.Flush();
                stream.Close();
            } catch (IOException) { }
        }

        /// <summary>
        /// Загрузить результаты.
        /// </summary>
        public static void Load() {
            try {
                var stream = File.Open(FILENAME, FileMode.Open);
                if (stream == null) {
                    hiScores = new List<Score>();
                    return;
                }
                var bformatter = new BinaryFormatter();
                hiScores = bformatter.Deserialize(stream) as List<Score>;
                stream.Close();
            } catch (IOException) {
                hiScores = new List<Score>();
            }
            
        }
    }
}
