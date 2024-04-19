using System.Text;

namespace HillClimber
{
    internal class Program
    {
        
        public static StringBuilder RandomizeString(int length)
        {
            StringBuilder randomString = new StringBuilder();
            Random rand = Random.Shared;

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
            Random rand = Random.Shared;
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
            //float error = 0;

            //Console.WriteLine("Give a Target String");

            //string targetString = Console.ReadLine();

            //StringBuilder randomString = RandomizeString(targetString.Length);
            //error = ErrorCalc(randomString, targetString);

            //while (targetString != randomString.ToString())
            //{
            //    string temp = randomString.ToString();
            //    Mutate(randomString, targetString);

            //    float newError = ErrorCalc(randomString, targetString);

            //    if (error < newError)
            //    {
            //        randomString = new StringBuilder(temp);
            //    }
            //    else
            //    {
            //        error = newError;
            //    }
            //    Console.WriteLine(randomString);
            //    Console.WriteLine(error);
            //}

            ErrorFunction errorFunc;
            ActivationFunction activationFunc;
            Perceptron perceptron;
            double[][] inputs;
            double[] desiredOutput;

            errorFunc = new ErrorFunction(Perceptron.SquaredError, Perceptron.DerivativeSquaredError);
            activationFunc = new ActivationFunction(Perceptron.Binary, Perceptron.DerivativeSigmoid);

            perceptron = new Perceptron(3, 0.001, Random.Shared, errorFunc);

            inputs = new double[][]
            {

                new double[] { 0, 0, 0},
                new double[] { 0, 1, 0},
                new double[] { 1, 0, 0},
                new double[] { 1, 1, 0},
                new double[] { 0, 0, 1},
                new double[] { 0, 1, 1},
                new double[] { 1, 0, 1},
                new double[] { 1, 1, 1}
            };
            desiredOutput = new double[] { 0, 0, 0, 1, 0, 1, 1, 1 };

            double error = perceptron.TrainWithHillClimbing(inputs, desiredOutput, int.MaxValue);

            for (int i = 0; i < 5000; i++)
            {
                error = perceptron.TrainWithHillClimbing(inputs, desiredOutput, error);

                double[] output = perceptron.Compute(inputs);

                for (int j = 0; j < inputs.Length; j++)
                {
                    for (int k = 0; k < inputs[j].Length; k++)
                    {
                        Console.Write(inputs[j][k]);
                    }
                    Console.WriteLine($" {activationFunc.Function(output[j])}");
                }
            }

        }
    }
}