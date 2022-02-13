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
        private readonly Dictionary<SkillEnum, Skill> _skills;
        
        [JsonIgnore] public Skill Strength => _skills[SkillEnum.Strength];
        [JsonIgnore] public Skill Intelligence => _skills[SkillEnum.Intelligence];
        [JsonIgnore] public Skill Charisma => _skills[SkillEnum.Charisma];
        
        public static readonly DateTime GameStartTime = new(2022, 1, 30, 8, 0, 0);

        public DateTime Time { get; set; }
        
        // Rate of time compared to real time (higher is faster). 
        public float TimeSpeed { get; set; }
        
        public long Money { get; set; }
        
        public int Family { get; set; }
        public int Days => (Time - GameStartTime).Days;

        private static readonly string DataFile = $"{Application.persistentDataPath}/data.json";

        // Global instance of the game state
        public static GameState Instance = new GameState();

        public GameState()
        {
            _skills = new()
            {
                [SkillEnum.Strength] = new Skill(),
                [SkillEnum.Intelligence] = new Skill(),
                [SkillEnum.Charisma] = new Skill(),
            };
            Time = GameStartTime;
            TimeSpeed = 1;
            Money = 500;
            Family = 50;
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

        // Load and set GameState.Instance. If no save file exists, a new
        // Game state is used.
        private static GameState Load()
        {
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                try
                {
                    Instance = JsonConvert.DeserializeObject<GameState>(json);
                }
                catch
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }

            return Instance;
        }

        public static void Reset()
        {
            Instance = new GameState();
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