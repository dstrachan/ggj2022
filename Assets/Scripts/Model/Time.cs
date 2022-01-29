using System;
using Newtonsoft.Json;

namespace Model
{
    public class Time
    {
        private DateTime _prevTime;
        private DateTime _gameTime;

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

        public Time()
        {
            Reset();
        }

        public void Reset()
        {
            _prevTime = DateTime.UtcNow;
            _gameTime = new DateTime(2000, 1, 1);
        }
    }
}