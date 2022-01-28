using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Model
{
    public class GameState
    {
        public Skill Strength { get; } = new();
        public Skill Intelligence { get; } = new();
        public Skill Charisma { get; } = new();

        private static readonly string DataFile = $"{Application.persistentDataPath}/data.json";

        private GameState()
        {
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(DataFile, json);
        }

        public static GameState Load()
        {
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                try
                {
                    return JsonConvert.DeserializeObject<GameState>(json);
                }
                catch
                {
                    return new GameState();
                }
            }

            return new GameState();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}