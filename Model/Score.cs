using System;
using System.Runtime.Serialization;

namespace Model {

    /// <summary>
    /// Класс данных об игровом состоянии.
    /// </summary>
    [Serializable]
    public class Score : ISerializable {

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name {
            get {
                if (string.IsNullOrEmpty(name)) {
                    return Properties.Settings.Default.Username;
                }
                return name;
            }
            set {
                if (string.IsNullOrEmpty(value))
                    name = Properties.Settings.Default.Username;
                else {
                    name = value;
                }
            }
        }
        private string name;
        /// <summary>
        /// Количество очков.
        /// </summary>
        public int ScoreValue { get; set; }
        /// <summary>
        /// Продолжительность игры.
        /// </summary>
        public TimeSpan GameTime { get; set; }
        /// <summary>
        /// Дата начала игры.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Завершена ли игра, либо прервана.
        /// </summary>
        public bool Complete { get; set; }

        public Score() {
            Name = Properties.Settings.Default.Username;
            ScoreValue = 0;
            GameTime = TimeSpan.Zero;
            Date = DateTime.Now;
            Complete = false;
        }

        protected Score(SerializationInfo info, StreamingContext ctx) {
            Name = info.GetString("Name");
            ScoreValue = info.GetInt32("ScoreValue");
            GameTime = (TimeSpan) info.GetValue("GameTime", typeof(TimeSpan));
            Date = info.GetDateTime("Date");
            Complete = info.GetBoolean("Complete");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Name", Name);
            info.AddValue("ScoreValue", ScoreValue);
            info.AddValue("GameTime", GameTime);
            info.AddValue("Date", Date);
            info.AddValue("Complete", Complete);
        }
    }
}
