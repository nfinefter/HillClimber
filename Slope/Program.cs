using Microsoft.Xna.Framework;

namespace Slope
{
    public class Program
    {
        const int PointVariation = 0;
        public static List<Vector2> Points = new List<Vector2>();
        public static Random rand = new Random(0);
        public static Vector2 LineGen()
        {

            float slope = rand.NextSingle() * 2 - 1 ;

            float b = rand.Next(-10, 10);

            return new Vector2(slope, b);

        }
        public static List<Vector2> PointGen(int pointCount, Vector2 line)
        {
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < pointCount; i++)
            {
                int y = (int)((line.X * i) + line.Y);

                list.Add(new Vector2(Random.Shared.Next(i - PointVariation, i + PointVariation), Random.Shared.Next(y - PointVariation, y + PointVariation)));
            }
            return list;
        }
        public static Vector2 Mutate(Vector2 curr)
        {



            if (rand.Next(0, 2) == 0)
            {
                if (rand.Next(0, 2) == 0)
                {
                    curr.X+= 0.01f;
                }
                else
                {
                    curr.X-= 0.01f;
                }
            }
            else
            {
                if (rand.Next(0, 2) == 0)
                {
                    curr.Y+= 0.02f;
                }
                else
                {
                    curr.Y-= 0.02f;
                }
            }
            return curr;
        }
        public static float ErrorCalc(Vector2 curr, List<Vector2> points)
        {
            float error = 0;

            for (int i = 0; i < points.Count; i++)
            {
                float y = curr.X * points[i].X + curr.Y;

                error += MathF.Pow(points[i].Y - y, 2);
            }

            return error;
        }

        public static void Main(string[] args)
        {
   
            Console.WriteLine("How many points would you like to generate?");
            int pointCount = int.Parse(Console.ReadLine()!);

            Vector2 line = LineGen();
            Points = PointGen(pointCount, line);

            Vector2 curr = new Vector2();

            float error = ErrorCalc(curr, Points);

            while (curr != line)
            {
                Vector2 temp = Mutate(curr);
                float newError = ErrorCalc(curr, Points);

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