using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.CSharp
{
    public static class CSharpExercises
    {
        public static void GeneralTests()
        {
            //CovarianceTest();

            IComparer<IShape> areaComparer = new AreaComparer();
            Shapes.Circles.Sort(areaComparer);


        }

        private static void CovarianceTest()
        {
            //covariance allows you to use a derived class where a base class is expected (rule: can accept big if small is expected).
            Small s1 = new Small();
            Small s2 = new Big();
            Small s3 = new Bigger();
            Big b1 = new Big();
            Big b2 = new Bigger();
            //Big b3 = new Small();
        }

        class AreaComparer : IComparer<IShape>
        {
            public int Compare(IShape x, IShape y)
            {
                return x.Area.CompareTo(y.Area);
            }
        }

        public interface IShape
        {
            double Area { get; }

            Rect BoundingBox { get; }
        }

        public static class Shapes
        {
            private static readonly List<Circle> circles = new List<Circle> {
            new Circle(new Point(0, 0), 15),
            new Circle(new Point(10, 5), 20),
        };

            private static readonly List<Square> squares = new List<Square> {
            new Square(new Point(5, 10), 5),
            new Square(new Point(-10, 0), 2)
        };

            public static List<Circle> Circles { get { return circles; } }
            public static List<Square> Squares { get { return squares; } }
        }

        class Small
        {

        }
        class Big : Small
        {

        }
        class Bigger : Big
        {

        }

        

    }
}
