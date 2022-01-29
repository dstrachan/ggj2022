using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Model
{
    public class Skill : INotifyPropertyChanged
    {
        private long _xp;

        public long Xp
        {
            get => _xp;
            set
            {
                if (value == _xp) return;
                _xp = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public double Value => Math.Pow(Math.Log10(Xp + 1), 3) + 1;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}