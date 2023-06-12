using System;

namespace SineWaveStory.App.Generators
{
    /// <summary>
    /// Генератор точек сигналов.
    /// </summary>
    public static class WaveGenerator
    {
        /// <summary>
        /// Генерирует точку синусоидального сигнала.
        /// </summary>
        /// <param name="frequency">Частота.</param>
        /// <param name="amplitude">Амплитуда.</param>
        /// <param name="time">Точка времени.</param>
        /// <returns></returns>
        public static (float X, float Y) GetSineValue(float frequency, float amplitude, float time)
        {
            var value = (float) Math.Sin(2f * Math.PI * (frequency * time));
            return (time, amplitude * value);
        }
    }
}