using System;
using System.IO;
using SkiaSharp;
using Svg.Skia;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: SvgToPngConverter <input_svg_path> <output_png_path>");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = args[1];

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"The file {inputFilePath} does not exist.");
            return;
        }

        try
        {
            ConvertSvgToPng(inputFilePath, outputFilePath);
            Console.WriteLine($"Successfully converted {inputFilePath} to {outputFilePath}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void ConvertSvgToPng(string inputFilePath, string outputFilePath)
    {
        var svg = new SKSvg();
        svg.Load(inputFilePath);
        var bitmap = new SKBitmap((int)svg.Picture.CullRect.Width, (int)svg.Picture.CullRect.Height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);
        canvas.DrawPicture(svg.Picture);
        canvas.Flush();

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(outputFilePath);
        data.SaveTo(stream);
    }
}
