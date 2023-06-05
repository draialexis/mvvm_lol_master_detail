﻿using Model;
using System.Collections.ObjectModel;
using VMToolkit;

namespace ViewModel
{
    public class ChampionVM : PropertyChangeNotifier
    {

        public Champion Model
        {
            get => model;
            set
            {
                if (model is not null && model.Equals(value)) return;
                model = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Icon));
                OnPropertyChanged(nameof(Bio));
                OnPropertyChanged(nameof(Class));
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(Characteristics));
            }
        }
        private Champion model;

        public ChampionVM(Champion model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            UpdateCharacteristicsVM();
        }

        /// <summary>
        /// Clone constructor
        /// </summary>
        /// <param name="championVM"></param>
        public ChampionVM(ChampionVM championVM)
        {
            if (championVM is null || championVM.Model is null)
            {
                throw new ArgumentNullException(nameof(championVM));
            }

            Champion modelClone = new(championVM.Name)
            {
                Class = championVM.Model.Class,
                Icon = championVM.Model.Icon,
                Image = championVM.Model.Image,
                Bio = championVM.Model.Bio,
            };
            Model = modelClone;
            foreach (var characteristic in championVM.Model.Characteristics)
            {
                Model.AddCharacteristics(new Tuple<string, int>(characteristic.Key, characteristic.Value));
            }

            UpdateCharacteristicsVM();
        }
        public string Name
        {
            get => Model.Name;
        }
        public string Icon
        {
            get => Model.Icon;
            set
            {
                if (Model == null || Model.Icon.Equals(value)) return;
                Model.Icon = value;
                OnPropertyChanged();
            }
        }

        public string Bio
        {
            get => Model.Bio;
            set
            {
                if (Model == null || Model.Bio.Equals(value)) return;
                Model.Bio = value;
                OnPropertyChanged();
            }
        }

        public string Class
        {
            get => Model.Class.ToString();
            set
            {
                if (Model == null || Model.Class.ToString().Equals(value)) return;
                if (Enum.TryParse(value, out ChampionClass classAsEnum))
                {
                    Model.Class = classAsEnum;
                    OnPropertyChanged();
                }
            }
        }

        public string Image
        {
            get => Model.Image.Base64;
            set
            {
                if (Model == null || Model.Image.Base64.Equals(value)) return;
                Model.Image.Base64 = value;
                OnPropertyChanged();
            }
        }

        public ReadOnlyObservableCollection<CharacteristicVM> Characteristics
        {
            get => new(characteristics);
        }
        private ObservableCollection<CharacteristicVM> characteristics = new();

        private void UpdateCharacteristicsVM()
        {
            characteristics = new();
            foreach (var characteristic in Model.Characteristics)
            {
                characteristics.Add(new CharacteristicVM { Key = characteristic.Key, Value = characteristic.Value });
            }
            OnPropertyChanged(nameof(Characteristics));
        }

        public void AddCharacteristics(Tuple<string, int>[] characteristicsTuples)
        {
            if (characteristicsTuples is null || !characteristicsTuples.Any())
            {
                throw new ArgumentException(null, nameof(characteristicsTuples));
            }
            if (characteristicsTuples.Any(llanfairpwllgwyngyllgogerychwyrndrobwllllantisiliogogogoch => string.IsNullOrWhiteSpace(llanfairpwllgwyngyllgogerychwyrndrobwllllantisiliogogogoch.Item1)))
            {
                throw new ArgumentException(null, nameof(characteristicsTuples));
            }

            Model.AddCharacteristics(characteristicsTuples);
            UpdateCharacteristicsVM();
        }

        public void AddCharacteristic(Tuple<string, int> characteristic)
        {
            AddCharacteristics(new Tuple<string, int>[] { characteristic });
        }

        public void RemoveCharacteristic(CharacteristicVM characteristic)
        {
            if (characteristic is null)
            {
                throw new ArgumentNullException(nameof(characteristic));
            }
            RemoveCharacteristic(characteristic.Key);
        }

        public void RemoveCharacteristic(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(null, nameof(key));
            }
            if (Model.RemoveCharacteristics(key))
            {
                UpdateCharacteristicsVM();
            }
        }

        public void RemoveCharacteristic(int index)
        {
            if (index < 0 || index >= characteristics.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            characteristics.RemoveAt(index);
            UpdateCharacteristicsVM();
        }

        public void UpdateCharacteristic(CharacteristicVM characteristic)
        {
            RemoveCharacteristic(characteristic);
            AddCharacteristic(new Tuple<string, int>(characteristic.Key, characteristic.Value));
            UpdateCharacteristicsVM();
        }
    }
}
