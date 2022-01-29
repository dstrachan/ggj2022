using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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

        public Time Time { get; } = new();
        public Skill Strength => _skills[SkillEnum.Strength];
        public Skill Intelligence => _skills[SkillEnum.Intelligence];
        public Skill Charisma => _skills[SkillEnum.Charisma];
        public long Money { get; set; }
        public long Love { get; set; }

        private static readonly string DataFile = $"{Application.persistentDataPath}/data.json";

        public static GameState Instance => Lazy.Value;

        private GameState()
        {
        }

        public void Reset()
        {
            Time.Reset();
            Strength.Reset();
            Intelligence.Reset();
            Charisma.Reset();
            Money = 0;
            Love = 0;

            Save();
        }

        public void SkipTimeForDuration(TimeSpan duration)
        {
            var currentTime = Time.Value;
            var endTime = currentTime.Add(duration);
            
            throw new NotImplementedException();
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