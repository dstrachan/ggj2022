using System;
using Newtonsoft.Json;

namespace Model
{
    public class Time
    {
        private DateTime _prevTime = DateTime.UtcNow;
        private DateTime _gameTime = new(2000, 1, 1);

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
    }
}