using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Greyscale
{
    public class GreyscaleConverter
    {
        /// <summary>
        /// Will make greyscale versions of all images in the given folder.
        /// The new images will be named as such: "'oldname'-greyscale.ext".
        /// </summary>
        /// <param name="path">Folder which images are located in.</param>
        /// <param name="fileNameExtension">This string will be appended to the old filename.</param>
        public void ConvertImagesInFolderToGreyscale(string path, string fileNameExtension = "-greyscale")
        {
            #region Console prints

            Console.WriteLine($"Entered method: {nameof(ConvertImagesInFolderToGreyscale)}().");
            Console.WriteLine($"Given path: {path}");

            #endregion

            #region Gathering data about files

            var filesPaths = Directory.GetFiles(path);
            var fileNames = filesPaths.Select(Path.GetFileName).ToList();
            var totalNumbersOfFiles = filesPaths.Length;
            var filesConverted = 0;

            #endregion

            foreach (var fileName in fileNames)
            {
                #region Check if file already has been converted or is of invalid type

                if (fileName.Contains(fileNameExtension) || fileName.Contains(".gif"))
                {
                    totalNumbersOfFiles--;
                    continue;
                }

                #endregion

                #region Load image

                Console.Write($"\nConverting file ({++filesConverted}/{totalNumbersOfFiles}): {fileName}");

                var image = new Bitmap(path + fileName, true);

                #endregion

                for (var x = 0; x < image.Width; x++)
                {
                    for (var y = 0; y < image.Height; y++)
                    {
                        var pixel = image.GetPixel(x, y);

                        var alpha = pixel.A;
                        var red = pixel.R;
                        var green = pixel.G;
                        var blue = pixel.B;

                        var average = (red + green + blue) / 3;

                        image.SetPixel(x, y, Color.FromArgb(alpha, average, average, average));
                    }

                    #region Dots showing progress

                    for (var i = 1; i < 10; i++)
                    {
                        if (x == image.Width / i)
                        {
                            Console.Write(".");
                        }
                    }

                    #endregion
                }

                #region Saving file

                var nameAndExtension = fileName.Split('.');
                var name = nameAndExtension[0];
                var extension = nameAndExtension[1];
                image.Save(path + name + fileNameExtension + "." + extension);
                Console.Write(" done!");

                #endregion
            }

            Console.WriteLine("\n\nFinished converting all files!");
        }
    }
}
