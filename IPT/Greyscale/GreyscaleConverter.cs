using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Greyscale
{
    public class GreyscaleConverter
    {
        private List<string> NamesOfAlreadyConvertedFiles { get; set; } = new List<string>();
        private string[] FilePaths { get; set; }
        private List<string> FileNames { get; set; }
        private int TotalNumbersOfFiles { get; set; }
        private int FilesConverted { get; set; }
        private Bitmap Image { get; set; }

        /// <summary>
        /// Will make greyscale versions of all images in the given folder.
        /// The new images will be named as such: "'oldname'-greyscale.ext".
        /// </summary>
        /// <param name="folderPath">Folder which images are located in.</param>
        /// <param name="fileNameAddon">This string will be appended to the old filename.</param>
        public void ConvertImagesInFolderToGreyscale(string folderPath, string fileNameAddon = "-greyscale")
        {
            #region Console prints

            Console.WriteLine($"Entered method: {nameof(ConvertImagesInFolderToGreyscale)}().");
            Console.WriteLine($"Given folderPath: {folderPath}");

            #endregion

            #region Gathering data about files

            FilePaths = Directory.GetFiles(folderPath);
            FileNames = FilePaths.Select(Path.GetFileName).ToList();
            TotalNumbersOfFiles = FilePaths.Length;
            FilesConverted = 0;

            #endregion

            #region Creating folder for greyscale files

            var folderForGreyscaleFiles = Directory.GetParent(Directory.GetParent(FilePaths.First()).ToString()).ToString() + $"\\{fileNameAddon}\\";
            Directory.CreateDirectory(folderForGreyscaleFiles);

            #endregion

            #region Find already converted files

            foreach (var name in FileNames)
            {
                if (name.Contains(fileNameAddon))
                {
                    NamesOfAlreadyConvertedFiles.Add(name);
                }
            }

            #endregion

            foreach (var fileName in FileNames)
            {
                #region Separating name and extension

                var nameAndExtension = fileName.Split('.');
                var originalFileName = nameAndExtension[0];
                var extension = nameAndExtension[1];

                #endregion

                #region Check if already converted and filetype

                // Image is greyscale
                if (NamesOfAlreadyConvertedFiles.Contains(fileName))
                {
                    TotalNumbersOfFiles--;
                    continue;
                }
                // Image has greyscale version
                if (NamesOfAlreadyConvertedFiles.Contains(originalFileName + fileNameAddon + "." + extension))
                {
                    TotalNumbersOfFiles--;
                    continue;
                }
                // Image is bad type
                if (fileName.Contains(".gif"))
                {
                    TotalNumbersOfFiles--;
                    continue;
                }

                #endregion

                #region Load image

                Console.Write($"\nConverting file ({++FilesConverted}/{TotalNumbersOfFiles}): {fileName}");

                Image = new Bitmap(folderPath + "\\" + fileName, true);

                #endregion

                for (var x = 0; x < Image.Width; x++)
                {
                    for (var y = 0; y < Image.Height; y++)
                    {
                        #region Convert pixel(x, y) to greyscale

                        var pixel = Image.GetPixel(x, y);

                        var alpha = pixel.A;
                        var red = pixel.R;
                        var green = pixel.G;
                        var blue = pixel.B;

                        var average = (red + green + blue) / 3;

                        Image.SetPixel(x, y, Color.FromArgb(alpha, average, average, average));

                        #endregion
                    }

                    #region Dots showing progress

                    for (var i = 1; i < 10; i++)
                    {
                        if (x == Image.Width / i)
                        {
                            Console.Write(".");
                        }
                    }

                    #endregion
                }

                #region Saving file

                var formattableString = $@"{folderForGreyscaleFiles}\{originalFileName}{fileNameAddon}.{extension}";
                Image.Save(formattableString);
                Console.Write(" done!");

                #endregion
            }

            Console.WriteLine("\n\nFinished converting all files!");
        }
    }
}
