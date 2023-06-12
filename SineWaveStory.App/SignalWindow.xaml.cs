/*
 * Изначально, хотел реализовать вручную на AutoResetEvent & ReaderWriterLock, но остановился на готовой BlockingCollection.
 * Изначально, использовал нативные Threads, но остановился на использовании тасок.
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SineWaveStory.App.Common;
using SineWaveStory.App.Generators;
using SineWaveStory.App.ViewModels;

namespace SineWaveStory.App
{
    /// <summary>
    /// Форма, для визуализации синусоидального сигнала.
    /// </summary>
    public partial class SignalWindow
    {
        private const float StartTime = 0f;
        private const float TimeStep = 1f / 9;
        private static float _currentTime = StartTime;
        private readonly Task _drawingTask;
        private readonly Task _generateTask;
        private readonly CancellationTokenSource _tokenSourceForDrawingTask = new CancellationTokenSource();
        private readonly CancellationTokenSource _tokenSourceForGenerateTask = new CancellationTokenSource();
        private readonly BlockingCollection<(float X, float Y)> values = new BlockingCollection<(float X, float Y)>();
        private readonly ScottPlotDrawer<float> _drawer;
        public SignalWindow()
        {
            InitializeComponent();
            Chart.Plot.SetAxisLimitsY(-1, 1);
            DataContext = ViewModel;
            _generateTask = GetGenerateTask();
            _drawingTask = GetDrawingTask();
            _drawer = ScottPlotDrawer<float>.FromWpfPlot(Chart);
        }

        public SignalViewModel ViewModel { get; } = new SignalViewModel(1f, 10f);

        private Task GetDrawingTask()
        {
            return new Task(() => GetValue(_tokenSourceForDrawingTask.Token), _tokenSourceForDrawingTask.Token);
        }

        private Task GetGenerateTask()
        {
            return new Task(() => AddValue(_tokenSourceForGenerateTask.Token), _tokenSourceForGenerateTask.Token);
        }

        public void Start(object sender, EventArgs e)
        {
            if (_generateTask.Status == TaskStatus.Created && _drawingTask.Status == TaskStatus.Created)
            {
                _generateTask.Start();
                _drawingTask.Start();
            }
        }

        public void AddValue(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var valueTuple = WaveGenerator.GetSineValue(ViewModel.Frequency, ViewModel.Amplitude, _currentTime);
                values.Add((valueTuple.X, valueTuple.Y), token);
                _currentTime += TimeStep;
                Thread.Sleep(50);
            }

            values.CompleteAdding();
        }

        public void GetValue(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (values.TryTake(out var value)) _drawer.Draw(value.X, value.Y);

                Thread.Sleep(25);
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            try
            {
                _tokenSourceForGenerateTask.Cancel();
                _tokenSourceForDrawingTask.Cancel();
            }
            catch (AggregateException exception)
            {
                exception.Handle(x => x is TaskCanceledException);
            }
        }
    }
}