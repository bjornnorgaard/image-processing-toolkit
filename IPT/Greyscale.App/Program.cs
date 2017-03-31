using System;

namespace Greyscale.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // Will get path of 'img' folder, placed in default 'Documents' directory.
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}" + @"\img\";

            var greyscaleConverter = new GreyscaleConverter();
            greyscaleConverter.ConvertImagesInFolderToGreyscale(path, "grey");
        }
    }
}
