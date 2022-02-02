using System;
using Newtonsoft.Json;

namespace Model
{
    public class Skill
    {
        public long Xp { get; set; }

        [JsonIgnore]
        public double Value => Math.Pow(Math.Log10(Xp + 1), 3) + 100;

        public void Reset()
        {
            Xp = 0;
        }
    }
}