using System.Text;

namespace HillClimber
{
    internal class Program
    {
        public static StringBuilder RandomizeString(int length)
        {
            StringBuilder randomString = new StringBuilder();
            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                randomString.Append((char)rand.Next(32, 127));
            }
            return randomString;
        }

        public static float ErrorCalc(StringBuilder randomString, string targetString)
        {
            float error = 0.0f;
            for (int i = 0; i < randomString.Length; i++)
            {
                error += Math.Abs(targetString[i] - randomString[i]);
            }

            return error / randomString.Length;
        }
       
        public static void Mutate(StringBuilder randomString, string targetString)
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
            {
                randomString[rand.Next(0, randomString.Length)]++;
            }
            else
            {
                randomString[rand.Next(0, randomString.Length)]--;
            }
        }

        static void Main(string[] args)
        {
            float error = 0;

            Console.WriteLine("Give a Target String");
            string targetString = Console.ReadLine();

            StringBuilder randomString = RandomizeString(targetString.Length);
            error = ErrorCalc(randomString, targetString);

            while (targetString != randomString.ToString())
            {
                string temp = randomString.ToString();
                Mutate(randomString, targetString);

                float newError = ErrorCalc(randomString, targetString);

                if (error < newError)
                {
                    randomString.Clear();
                    randomString.Append(temp);
                }
                else
                {
                    error = newError;
                }
                Console.WriteLine(randomString);
                Console.WriteLine(error);
            }
        }
    }
}