using System;
using System.IO;
using SkiaSharp;
using Svg.Skia;
using static System.Net.Mime.MediaTypeNames;

class Program {
	static void Main(string[] args) {
		if (args.Length < 1) {
			Console.WriteLine("Usage: svg-to-png <input_svg_path>");
			return;
		}

		string inputFilePath = args[0];
		string outputFilePath = Path.ChangeExtension(inputFilePath, ".png");

		if (!File.Exists(inputFilePath)) {
			Console.WriteLine($"The file {inputFilePath} does not exist.");
			return;
		}

		ConvertSvgToPng(inputFilePath, outputFilePath);
	}

	private static void ConvertSvgToPng(string inputFilePath, string outputFilePath) {
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
