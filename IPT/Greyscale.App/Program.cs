using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Greyscale.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Users\Nørgaard\Documents\img\originals";

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var multi = new GreyScaleConverterThreading(path, "grey");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            stopwatch.Start();
            var single = new GreyscaleConverter();
            single.ConvertImagesInFolderToGreyscale(path, "scale");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}   
