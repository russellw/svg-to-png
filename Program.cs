using System;
using System.IO;
using SkiaSharp;
using Svg.Skia;
using static System.Net.Mime.MediaTypeNames;

class Program {
	static void Main(string[] args) {
		if (args.Length < 1) {
			Console.WriteLine("Usage: svg-to-png <input_svg_path> [width] [height]");
			return;
		}

		string inputFilePath = args[0];
		string outputFilePath = Path.ChangeExtension(inputFilePath, ".png");
		int width = args.Length > 1 ? int.Parse(args[1]) : 256;	 // Default width
		int height = args.Length > 2 ? int.Parse(args[2]) : 256; // Default height

		if (!File.Exists(inputFilePath)) {
			Console.WriteLine($"The file {inputFilePath} does not exist.");
			return;
		}

		ConvertSvgToPng(inputFilePath, outputFilePath, width, height);
	}

	private static void ConvertSvgToPng(string inputFilePath, string outputFilePath, int width, int height) {
		var svg = new SKSvg();
		svg.Load(inputFilePath);
		var svgWidth = svg.Picture.CullRect.Width;
		var svgHeight = svg.Picture.CullRect.Height;

		// Calculate scale to fit the specified width and height
		float scaleX = width / svgWidth;
		float scaleY = height / svgHeight;
		float scale = Math.Min(scaleX, scaleY); // Maintain aspect ratio

		// Create a bitmap with the specified width and height
		var bitmap = new SKBitmap(width, height);
		using var canvas = new SKCanvas(bitmap);
		canvas.Clear(SKColors.White);

		// Apply scale and center the image
		canvas.Translate(width / 2.0f, height / 2.0f);
		canvas.Scale(scale);
		canvas.Translate(-svgWidth / 2.0f, -svgHeight / 2.0f);

		// Draw the SVG
		canvas.DrawPicture(svg.Picture);
		canvas.Flush();

		// Save to PNG
		using var image = SKImage.FromBitmap(bitmap);
		using var data = image.Encode(SKEncodedImageFormat.Png, 100);
		using var stream = File.OpenWrite(outputFilePath);
		data.SaveTo(stream);
	}
}
