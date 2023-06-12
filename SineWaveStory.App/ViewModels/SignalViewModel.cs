using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SineWaveStory.App.ViewModels
{
    /// <summary>
    /// Модель, необходимая для работы формы <see cref="SignalWindow"/>
    /// </summary>
    public sealed class SignalViewModel : INotifyPropertyChanged
    {
        private float _amplitude;

        private float _frequency;

        /// <summary>
        /// Инстанцирует <see cref="SignalViewModel"/>.
        /// </summary>
        /// <param name="amplitude">Амплитуда.</param>
        /// <param name="frequency">Частота.</param>
        public SignalViewModel(float amplitude, float frequency)
        {
            Amplitude = amplitude;
            Frequency = frequency;
        }

        [Range(1, 100, ErrorMessage = "Частота должна быть от {1} до {2}")]
        public float Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                if (PropertyChanged != null) OnPropertyChanged();
            }
        }

        [Range(1, 100, ErrorMessage = "Амплитуда должна быть в диапазоне от {1} до {2}")]
        public float Amplitude
        {
            get => _amplitude;
            set
            {
                _amplitude = value;
                if (PropertyChanged != null) OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}