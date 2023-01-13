using SFML.Graphics;
using SFML.Window;
using Shapes.Applications;
using System.IO;

namespace Shapes
{
    class Program
    {
        private const string DefaultInputFileName = "input.txt";
        private const string DefaultOutputFileName = "output.txt";

        static void Main(string[] args)
        {
            StreamReader inputFileStream = new(DefaultInputFileName);
            StreamWriter outputFileStream = new(DefaultOutputFileName);
            RenderWindow window = new(new VideoMode(1000, 1000), "Shapes");

            Application application = new(window, inputFileStream, outputFileStream);

            application.ReadDefaultShapes();
            application.ProcessWindow();
            application.PrintShapesInfo();

            inputFileStream.Close();
            outputFileStream.Close();
        }
    }
}
