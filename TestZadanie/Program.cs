using System;
using System.Drawing;
using LiveCharts;
using LiveCharts.Wpf;

using System.Drawing.Common;
using OxyPlot;

namespace Zadanie
{
    
    class MainClass
    {
        public static void Main(string [] args)
        {
            double m = 1.0;
            double L = 5.0;
            double x0 = 3.0;
            double y0 = -4.0;
            double vx0 = 1.0;
            double vy0 = 0.0;
            double T = 1001.0;
            double h = 0.01;

            int n = (int)(T / h);
            double[] x = new double[n];
            double[] y = new double[n];
            double[] vx = new double[n];
            double[] vy = new double[n];
            double[] t = new double[n];

            x[0] = x0;
            y[0] = y0;
            vx[0] = vx0;
            vy[0] = vy0;
            t[0] = 0.0;

            for (int i = 1; i < n; i++)
            {
                double kx1 = vx[i - 1];
                double ky1 = vy[i - 1];
                double kvx1 = (-x[i - 1] / L) * g(t[i - 1]);
                double kvy1 = (-y[i - 1] / L) * g(t[i - 1]) - g(t[i - 1]);

                double kx2 = vx[i - 1] + 0.5 * h * kvx1;
                double ky2 = vy[i - 1] + 0.5 * h * kvy1;
                double kvx2 = (-x[i - 1] - 0.5 * h * kx1) / L * g(t[i - 1] + 0.5 * h);
                double kvy2 = (-y[i - 1] - 0.5 * h * ky1) / L * g(t[i - 1] + 0.5 * h) - g(t[i - 1] + 0.5 * h);

                double kx3 = vx[i - 1] + 0.5 * h * kvx2;
                double ky3 = vy[i - 1] + 0.5 * h * kvy2;
                double kvx3 = (-x[i - 1] - 0.5 * h * kx2) / L * g(t[i - 1] + 0.5 * h);
                double kvy3 = (-y[i - 1] - 0.5 * h * ky2) / L * g(t[i - 1] + 0.5 * h) - g(t[i - 1] + 0.5 * h);
                double kx4 = vx[i - 1] + h * kvx3;
                double ky4 = vy[i - 1] + h * kvy3;
                double kvx4 = (-x[i - 1] - h * kx3) / L * g(t[i - 1] + h);
                double kvy4 = (-y[i - 1] - h * ky3) / L * g(t[i - 1] + h) - g(t[i - 1] + h);

                x[i] = x[i - 1] + h * (kx1 + 2 * kx2 + 2 * kx3 + kx4) / 6.0;
                y[i] = y[i - 1] + h * (ky1 + 2 * ky2 + 2 * ky3 + ky4) / 6.0;
                vx[i] = vx[i - 1] + h * (kvx1 + 2 * kvx2 + 2 * kvx3 + kvx4) / 6.0;
                vy[i] = vy[i - 1] + h * (kvy1 + 2 * kvy2 + 2 * kvy3 + kvy4) / 6.0;
                t[i] = t[i - 1] + h;
            }

            // Построение графика
            double[] r = new double[n];
            for (int i = 0; i < n; i++)
            {
                r[i] = Math.Sqrt(x[i] * x[i] + y[i] * y[i]);
            }

            double L2 = L * L;

// создаем серию данных для построения графика
            var rSeries = new LineSeries
            {
                Title = "r(t)",
                Values = new ChartValues<double>(r),
                LineSmoothness = 0, // для отключения сглаживания линии графика
            };

// создаем серию данных для отображения горизонтальной линии
            var lSeries = new LineSeries
            {
                Title = "L^2",
                Values = new ChartValues<double>(new[] { L2, L2 }),
            };

// создаем объект графика и добавляем на него серии данных
            var chart = new CartesianChart
            {
                Width = 600,
                Height = 400,
                LegendLocation = LegendLocation.Right,
                Series = { rSeries, lSeries },
                AxisX = new Axis { Title = "t" },
                AxisY = new Axis { Title = "sqrt(x(t)^2 + y(t)^2)" },
                Title = new DefaultTitle { Text = "sqrt(x(t)^2 + y(t)^2) vs t" },
            };

// сохраняем график в файл
            var exporter = new SvgExporter { Width = 600, Height = 400 };
            exporter.ExportToFile(chart, "plot.svg");
        }

        
        static double g(double t)
        {
            return 9.81 + 0.05*Math.Sin(Math.Tau*t);
        }
    }
}