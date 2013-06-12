using System;
using System.Runtime.Serialization;

namespace Model {

    [Serializable]
    public class Score : ISerializable {

        private string name;
        public string Name {
            get {
                if (name == null || name == "") {
                    return Properties.Settings.Default.Username;
                }
                return name;
            }
            set {
                if (value == null || value == "")
                    name = Properties.Settings.Default.Username;
                else {
                    name = value;
                    Properties.Settings.Default.Username = name;
                    Properties.Settings.Default.Save();
                }
            }
        }
        public int ScoreValue { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime Date { get; set; }
        public bool Complete { get; set; }

        public Score() {
            Name = "Неизвестно";
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
