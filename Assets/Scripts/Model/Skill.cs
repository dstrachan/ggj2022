using System;
using Newtonsoft.Json;

namespace Model
{
    public class Skill
    {
        public long Xp { get; set; } = 1;
        [JsonIgnore]
        public double Value => Math.Pow(Math.Log10(Xp), 3) + 10;
    }
}