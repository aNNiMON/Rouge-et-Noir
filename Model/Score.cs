using System;
using System.Runtime.Serialization;

namespace Model {

    [Serializable]
    public class Score : ISerializable {

        public string Name { get; set; }
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

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("Name", Name);
            info.AddValue("ScoreValue", ScoreValue);
            info.AddValue("GameTime", GameTime);
            info.AddValue("Date", Date);
            info.AddValue("Complete", Complete);
        }
    }
}
