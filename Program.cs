using System;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Vector Analysis System");
        Console.WriteLine("======================");

        // Input vectors
        Console.Write("Enter components of Vector A (comma-separated, e.g., 1,2,3): ");
        var vectorA = ReadVector();

        Console.Write("Enter components of Vector B (comma-separated, e.g., 4,5,6): ");
        var vectorB = ReadVector();

        // Operations
        Console.WriteLine("\nVector Operations:");
        var addition = vectorA + vectorB;
        Console.WriteLine($"A + B: {addition}");

        var subtraction = vectorA - vectorB;
        Console.WriteLine($"A - B: {subtraction}");

        var dotProduct = vectorA.DotProduct(vectorB);
        Console.WriteLine($"A · B: {dotProduct}");

        if (vectorA.Count == 3 && vectorB.Count == 3)
        {
            try
            {
                var crossProduct = CrossProduct(vectorA, vectorB);
                Console.WriteLine($"A × B: {crossProduct}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        var magnitudeA = vectorA.L2Norm();
        Console.WriteLine($"|A|: {magnitudeA}");

        var unitVectorA = vectorA.Normalize(2);
        Console.WriteLine($"Unit Vector of A: {unitVectorA}");

        var angle = Math.Acos(dotProduct / (vectorA.L2Norm() * vectorB.L2Norm())) * (180 / Math.PI);
        Console.WriteLine($"Angle between A and B: {angle:F2}°");

        // Projection
        var projection = vectorB * (dotProduct / vectorB.DotProduct(vectorB));
        Console.WriteLine($"Projection of A onto B: {projection}");

        // Visualization
        Console.WriteLine("\nGenerating visualization...");
        PlotVectors(vectorA, vectorB);

        Console.WriteLine("Vector plot saved as 'plot.png' in the current directory.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static Vector<double> ReadVector()
    {
        var input = Console.ReadLine()?.Split(',');
        return Vector<double>.Build.DenseOfArray(Array.ConvertAll(input, double.Parse));
    }

    static Vector<double> CrossProduct(Vector<double> vectorA, Vector<double> vectorB)
    {
        if (vectorA.Count != 3 || vectorB.Count != 3)
        {
            throw new InvalidOperationException("Cross product is only defined for 3D vectors.");
        }

        var x = vectorA[1] * vectorB[2] - vectorA[2] * vectorB[1];
        var y = vectorA[2] * vectorB[0] - vectorA[0] * vectorB[2];
        var z = vectorA[0] * vectorB[1] - vectorA[1] * vectorB[0];

        return Vector<double>.Build.DenseOfArray(new double[] { x, y, z });
    }

    static void PlotVectors(Vector<double> vectorA, Vector<double> vectorB)
    {
        var plotModel = new PlotModel { Title = "Vector Visualization" };

        var lineA = new LineSeries { Title = "Vector A", Color = OxyPlot.OxyColors.Red };
        lineA.Points.Add(new DataPoint(0, 0));
        lineA.Points.Add(new DataPoint(vectorA[0], vectorA[1]));

        var lineB = new LineSeries { Title = "Vector B", Color = OxyPlot.OxyColors.Blue };
        lineB.Points.Add(new DataPoint(0, 0));
        lineB.Points.Add(new DataPoint(vectorB[0], vectorB[1]));

        plotModel.Series.Add(lineA);
        plotModel.Series.Add(lineB);

        // Export the plot as a PNG image using FileStream
        using (var stream = File.Create("plot.png"))
        {
            var exporter = new PngExporter { Width = 800, Height = 600 };
            exporter.Export(plotModel, stream);
        }
    }
}
