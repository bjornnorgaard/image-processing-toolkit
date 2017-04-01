using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Greyscale
{
    public class GreyScaleConverterThreading
    {
        private List<string> FileNames { get; set; }
        private int TotalNumberOfFiles { get; set; }
        private string GreyscaleFolder { get; set; }
        private string[] FilePaths { get; set; }
        private int ConvertedFiles { get; set; }
        private string Path { get; }
        private string Addon { get; }

        public GreyScaleConverterThreading(string path, string addon)
        {
            Path = path;
            Addon = addon;

            LoadFileData();
            CreateFolderForGreyscaleImages();
            StartThreads();
        }

        private void StartThreads()
        {
            var tasks = new List<Task>();

            for (var index = 0; index < FilePaths.Length; index++)
            {
                var localIndex = index;
                tasks.Add(Task.Factory.StartNew(() => ConvertImagesToGreyscale(localIndex)));
            }
            foreach (var task in tasks)
            {
                task.Wait();
            }
        }

        private void ConvertImagesToGreyscale(int fileNameIndex)
        {
            var nameAndExtension = FileNames[fileNameIndex].Split('.');
            var originalName = nameAndExtension.First();
            var extension = nameAndExtension.Last();

            var image = new Bitmap(FilePaths[fileNameIndex], true);

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
            }

            var saveLocation = $@"{GreyscaleFolder}\{originalName}{Addon}.{extension}";
            image.Save(saveLocation);
        }

        private void CreateFolderForGreyscaleImages()
        {
            GreyscaleFolder = Directory.GetParent(Directory.GetParent(FilePaths.First()).ToString()) + $"\\{Addon}\\";
            Directory.CreateDirectory(GreyscaleFolder);
        }

        private void LoadFileData()
        {
            FilePaths = Directory.GetFiles(Path);

            if (FilePaths.Length == 0)
            {
                throw new FileNotFoundException(Path);
            }

            FileNames = new List<string>();

            foreach (var filePath in FilePaths)
            {
                FileNames.Add(System.IO.Path.GetFileName(filePath));
            }

            TotalNumberOfFiles = FileNames.Count;
            ConvertedFiles = 0;
        }
    }
}
