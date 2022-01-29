using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Model
{
    public class GameState
    {
        public Time Time { get; } = new();
        public Skill Strength { get; } = new();
        public Skill Intelligence { get; } = new();
        public Skill Charisma { get; } = new();

        private static readonly string DataFile = $"{Application.persistentDataPath}/data.json";

        private GameState()
        {
        }

        public static GameState Load()
        {
            GameState gameState = null;
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                try
                {
                    gameState = JsonConvert.DeserializeObject<GameState>(json);
                }
                catch
                {
                    // ignored
                }
            }

            gameState ??= new GameState();
            gameState.InitEvents();
            return gameState;
        }

        public static GameState Reset()
        {
            if (File.Exists(DataFile))
            {
                File.Delete(DataFile);
            }

            return Load();
        }

        private void InitEvents()
        {
            Strength.PropertyChanged += (_, _) => Save();
            Intelligence.PropertyChanged += (_, _) => Save();
            Charisma.PropertyChanged += (_, _) => Save();
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(DataFile, json);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}