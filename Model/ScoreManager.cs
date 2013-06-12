using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Model {

    /// <summary>
    /// Учёт результатов игры.
    /// </summary>
    public static class ScoreManager {

        private const string FILENAME = "scores.bin";
        private const int INCREMENT = 100;

        /// <summary>
        /// Текущие результаты игры.
        /// </summary>
        public static Score Current {
            get;
            private set;
        }

        /// <summary>
        /// Имя игрока по умолчанию.
        /// </summary>
        public static string DefaultName {
            get {
                return Properties.Settings.Default.Username;
            }
            set {
                Properties.Settings.Default.Username = value;
            }
        }

        private static DateTime StartTime;

        /// <summary>
        /// Список лучших результатов.
        /// </summary>
        public static List<Score> HiScores {
            get {
                return hiScores;
            }
        }
        private static List<Score> hiScores;

        /// <summary>
        /// Получить продолжительность игры.
        /// </summary>
        /// <returns></returns>
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

        private static void Update() {
            Current.GameTime = DateTime.Now - StartTime;
        }

        /// <summary>
        /// Сохранение результатов завершенной игры.
        /// </summary>
        /// <param name="isComplete"></param>
        public static void EndGame(bool isComplete) {
            Current.GameTime = DateTime.Now - StartTime;
            Current.Complete = isComplete;

            hiScores.Add(Current);
            Save();
        }


        /// <summary>
        /// Сохранить результаты в файл.
        /// </summary>
        private static void Save() {
            Properties.Settings.Default.Save();
            // Выбираем лучшие 15 записей.
            var top15 = (from score in hiScores
                         orderby score.ScoreValue descending, score.GameTime
                         select score).Take(15).ToList();
            try {
                var stream = File.Open(FILENAME, FileMode.Create);
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, top15);
                stream.Flush();
                stream.Close();
            } catch (IOException) { }
        }

        /// <summary>
        /// Загрузить результаты из файла.
        /// </summary>
        private static void Load() {
            try {
                var stream = File.Open(FILENAME, FileMode.Open);
                if (stream.Length > 0) {
                    var bformatter = new BinaryFormatter();
                    hiScores = bformatter.Deserialize(stream) as List<Score>;
                    stream.Close();
                } else hiScores = new List<Score>();
            } catch (Exception) {
                hiScores = new List<Score>();
            }
            
        }
    }
}
