using System;
using ScottPlot;
using ScottPlot.Plottable;

namespace SineWaveStory.App.Common
{
    /// <summary>
    /// Рисовальщик (обертка) над <see cref="ScatterPlotList{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScottPlotDrawer<T>
    {
        private readonly WpfPlot _plot;

        private readonly ScatterPlotList<T> _scatterPlot;

        private ScottPlotDrawer(in WpfPlot plot, in ScatterPlotList<T> scatterPlot)
        {
            _plot = plot;
            _scatterPlot = scatterPlot;
        }

        /// <summary>
        /// Инстанцирует <see cref="ScottPlotDrawer{T}"/> на основе <see cref="WpfPlot"/>
        /// </summary>
        /// <param name="plot">Экземпляр <see cref="WpfPlot"/></param>
        /// <returns></returns>
        public static ScottPlotDrawer<T> FromWpfPlot(in WpfPlot plot)
        {
            if (plot == null) throw new ArgumentNullException(nameof(plot));
            var scatter = plot.Plot.AddScatterList<T>();
            scatter.Smooth = true;
            return new ScottPlotDrawer<T>(plot, scatter);
        }

        /// <summary>
        /// Выводит точку на полотно.
        /// </summary>
        /// <param name="x">Точка с координатой X.</param>
        /// <param name="y">Точка с координатой Y.</param>
        public void Draw(T x, T y)
        {
            _scatterPlot.Add(x, y);
            _plot.Dispatcher.Invoke(() => _plot.Refresh());
        }
    }
}