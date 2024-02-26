using Microsoft.Xna.Framework;

namespace Slope
{
    public class Program
    {
        public static Vector2 LineGen()
        {
            Random rand = Random.Shared;

            float slope = rand.NextSingle() * 2 - 1;

            float b = rand.Next(-101, 101);

            return new Vector2(slope, b);

        }
        public static List<Vector2> PointGen(int pointCount, Vector2 line)
        {
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < pointCount; i++)
            {
                int y = (int)((line.X * i) + line.Y);

                list.Add(new Vector2(Random.Shared.Next(i - 5, i + 5), Random.Shared.Next(y - 5, y + 5)));
            }
            return list;
        }
        public static Vector2 Mutate(Vector2 curr)
        {

            if (Random.Shared.Next(0, 2) == 0)
            {
                if (Random.Shared.Next(0, 2) == 0)
                {
                    curr.X++;
                }
                else
                {
                    curr.X--;
                }
            }
            else
            {
                if (Random.Shared.Next(0, 2) == 0)
                {
                    curr.Y++;
                }
                else
                {
                    curr.Y--;
                }
            }
            return curr;
        }
        public static float ErrorCalc(Vector2 curr, List<Vector2> points)
        {
            float error = 0;

            List<Vector2> Points = PointGen(points.Count, curr);

            for (int i = 0; i < Points.Count; i++)
            {
                float y = curr.X * Points[i].X + curr.Y;

                error += Math.Abs(Points[i].Y - y);
            }

            return error;
        }

        public static void Main(string[] args)
        {
   
            Console.WriteLine("How many points would you like to generate?");
            int pointCount = int.Parse(Console.ReadLine()!);

            Vector2 line = LineGen();
            List<Vector2> points = PointGen(pointCount, line);

            Vector2 curr = new Vector2();

            float error = ErrorCalc(curr, points);

            while (curr != line)
            {
                Vector2 temp = Mutate(curr);
                float newError = ErrorCalc(curr, points);

                if (error < newError)
                {
                    curr = Mutate(curr);
                }
                else
                {
                    curr = temp;
                    error = newError;
                }
                Console.WriteLine($"{curr.X}, {curr.Y}");
                Console.WriteLine(error);
            }

        }
    }
}