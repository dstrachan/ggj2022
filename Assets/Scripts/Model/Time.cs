using System;
using Newtonsoft.Json;

namespace Model
{
    public class Time
    {
        public static readonly DateTime FirstDay = new(2000, 1, 1, 8, 0, 0);

        private DateTime _prevTime = DateTime.UtcNow;
        private DateTime _gameTime = FirstDay;

        public DateTime Value
        {
            get
            {
                var currTime = DateTime.UtcNow;
                _gameTime += (currTime - _prevTime) * Factor;
                _prevTime = currTime;
                return _gameTime;
            }
        }

        [JsonIgnore]
        public float Factor { get; set; } = 1;

        public void SetDays(int days)
        {
            _prevTime = DateTime.UtcNow;
            _gameTime = FirstDay.Add(TimeSpan.FromDays(days));
        }

        public void Reset()
        {
            _prevTime = DateTime.UtcNow;
            _gameTime = FirstDay;
        }
    }
}