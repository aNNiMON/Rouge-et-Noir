using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Model {

    /// <summary>
    /// Учёт результатов игры.
    /// </summary>
    public static class ScoreManager {

        private const string SCORE_TABLE_URL = "";
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
        /// Занимаемое игроком место в рейтинге.
        /// </summary>
        private static int Place = 0;
        
        /// <summary>
        /// Получить продолжительность игры.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetGameTime() {
            if (StartTime == null || Current == null) return TimeSpan.Zero;
            Current.GameTime = DateTime.Now - StartTime;
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
            Current = new Score {
                Date = DateTime.Now
            };

            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Сохранение результатов завершенной игры.
        /// </summary>
        /// <param name="isComplete"></param>
        public static void EndGame(bool isComplete) {
            Current.GameTime = DateTime.Now - StartTime;
            Current.Complete = isComplete;

            try {
                SendCurrentScore();
            } catch (Exception) { }
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Возвращает позицию игрока в рейтинге.
        /// </summary>
        /// <returns></returns>
        public static int GetPlace() {
            return Place;
        }

        /// <summary>
        /// Загрузить результаты.
        /// </summary>
        public static void Load() {
            hiScores = new List<Score>();
            try {
                LoadOnlineScores();
            } catch (Exception) { }
        }

        private static void LoadOnlineScores() {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Util.GetTextFromUrl(SCORE_TABLE_URL + "xml", Encoding.UTF8));

            var scores = doc.SelectNodes("hiscores/scores");
            foreach (XmlNode node in scores) {
                var score = new Score();
                score.Name = node.SelectSingleNode("name").InnerText;
                score.ScoreValue = Convert.ToInt32(node.SelectSingleNode("score").InnerText);
                int gametime = Convert.ToInt32(node.SelectSingleNode("gametime").InnerText);
                score.GameTime = TimeSpan.FromSeconds(gametime);
                score.Complete = !(node.SelectSingleNode("complete").InnerText.Equals("0"));
                int date = Convert.ToInt32(node.SelectSingleNode("date").InnerText);
                score.Date = Util.UnixTimeStampToDateTime(date);

                hiScores.Add(score);
            }
        }

        /// <summary>
        /// Отправить текущий результат. 
        /// </summary>
        private static void SendCurrentScore() {
            var values = new NameValueCollection();
            values.Add("nm", Current.Name.Trim());
            values.Add("sc", Current.ScoreValue.ToString());
            values.Add("gt", Current.GameTime.TotalSeconds.ToString());
            values.Add("cm", (Current.Complete ? "1" : "0"));

            var request = Util.CreateRequest(SCORE_TABLE_URL + "add", values);
            var response = (HttpWebResponse) request.GetResponse();
            string answer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (!answer.StartsWith("0")) {
                try {
                    Place = Convert.ToInt32(answer.Trim());
                } catch (Exception) {
                    Place = 0;
                }
            }
        }
    }
}
