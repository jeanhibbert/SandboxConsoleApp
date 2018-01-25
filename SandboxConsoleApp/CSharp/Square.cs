using System.Windows;
using static SandboxConsoleApp.CSharp.CSharpExercises;

namespace SandboxConsoleApp.CSharp
{
    public sealed class Square : IShape
    {
        private readonly Point topLeft;
        private readonly double sideLength;

        public Square(Point topLeft, double sideLength)
        {
            this.topLeft = topLeft;
            this.sideLength = sideLength;
        }

        public double Area { get { return sideLength * sideLength; } }

        public Rect BoundingBox
        {
            get { return new Rect(topLeft, new Size(sideLength, sideLength)); }
        }
    }
}