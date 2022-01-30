using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace Model
{
    public class GameState
    {
        private static readonly Lazy<GameState> Lazy = new(Load);

        private readonly Dictionary<SkillEnum, Skill> _skills = new()
        {
            [SkillEnum.Strength] = new Skill(),
            [SkillEnum.Intelligence] = new Skill(),
            [SkillEnum.Charisma] = new Skill(),
        };

        [JsonIgnore]
        public Time Time { get; } = new();

        public Watchable<int> Days { get; } = new();
        public Skill Strength => _skills[SkillEnum.Strength];
        public Skill Intelligence => _skills[SkillEnum.Intelligence];
        public Skill Charisma => _skills[SkillEnum.Charisma];
        public long Money { get; set; }
        public int Family { get; set; }

        private static readonly string DataFile = $"{Application.persistentDataPath}/data.json";

        public static GameState Instance => Lazy.Value;

        private GameState()
        {
            Time.Reset();
            Days.Reset();
            Strength.Reset();
            Intelligence.Reset();
            Charisma.Reset();
            Money = 50;
            Family = 50;
        }

        public void Reset()
        {
            Time.Reset();
            Days.Reset();
            Strength.Reset();
            Intelligence.Reset();
            Charisma.Reset();
            Money = 50;
            Family = 50;

            Save();
            Load();
        }

        public static void DeleteSaveFile()
        {
            if (File.Exists(DataFile))
            {
                File.Delete(DataFile);
            }
        }

        public static bool HasSaveGame()
        {
            return File.Exists(DataFile);
        }

        private static GameState Load()
        {
            GameState gameState = null;
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                try
                {
                    gameState = JsonConvert.DeserializeObject<GameState>(json);
                    if (gameState.Days.Value > 0)
                    {
                        gameState.Time.SetDays(gameState.Days.Value);
                    }
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

        private void InitEvents()
        {
            Days.PropertyChanged += (_, _) => Save();
        }

        // TODO: Save at start of new day
        public void Save()
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