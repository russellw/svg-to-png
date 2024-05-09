import cairosvg
import sys
import os

def convert_svg_to_png(svg_file, output_file):
    try:
        cairosvg.svg2png(url=svg_file, write_to=output_file)
        print(f"Conversion successful: '{output_file}' created.")
    except Exception as e:
        print(f"Error converting file {svg_file}: {e}")

def main():
    if len(sys.argv) != 3:
        print("Usage: python svg_to_png.py <input_svg_file> <output_png_file>")
        sys.exit(1)

    input_file, output_file = sys.argv[1], sys.argv[2]

    # Check if the input file exists
    if not os.path.exists(input_file):
        print(f"No such file: '{input_file}'")
        sys.exit(1)

    # Check if the input file is an SVG
    if not input_file.lower().endswith('.svg'):
        print("The input file must be an SVG file.")
        sys.exit(1)

    convert_svg_to_png(input_file, output_file)

if __name__ == "__main__":
    main()
