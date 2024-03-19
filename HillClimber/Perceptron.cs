using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HillClimber
{
    public class Perceptron
    {
        double[] weights;
        double bias;

        private double mut;
        double mutationAmount
        {
            get
            {
                return mut;
            }
            set
            {
                mut = value;
            }
        }
        Random random;
        Func<double, double, double> errorFunc;

        public Perceptron(double[] initialWeightValues, double initialBiasValue, double mutationAmount, Random random, Func<double, double, double> errorFunc)
        { /*initializes the weights array and bias*/            
            weights = initialWeightValues;
            bias = initialBiasValue;
            this.mutationAmount = mutationAmount;
            this.random = random;
            this.errorFunc = errorFunc;
        }
        public Perceptron(int amountOfInputs, double mutationAmount, Random random, Func<double, double, double> errorFunc)
        { /*Initializes the weights array given the amount of inputs*/

            weights = new double[amountOfInputs];
            this.mutationAmount = mutationAmount;
            this.random = random;
            this.errorFunc = errorFunc;
        }
        public (int, double) Mutate()
        {
            var temp = random.Next(0, weights.Length + 1);
            
            var currentMutationAmount = mutationAmount * (random.Next(2) * 2 - 1);

            if (temp == weights.Length)
            {
                bias += currentMutationAmount;
            }
            else
            {
                weights[temp] += currentMutationAmount;
            }

            return (temp, currentMutationAmount);
        }
        public void Randomize(double min, double max)
        { /*Randomly generates values for every weight including the bias*/

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.Next((int)min, (int)max);
            }

        }
        public double Compute(double[] inputs)
        { /*computes the output with given input*/

            double output = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }


            return output + bias;
        }
        public double[] Compute(double[][] inputs)
        { /*computes the output for each row of inputs*/

            double[] output = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int k = 0; k < inputs[i].Length; k++)
                {
                    output[i] += inputs[i][k] * weights[k];
                }

                output[i] += bias;
            }

            return output;

        }
        public double GetError(double[][] inputs, double[] desiredOutputs)
        { /*computes the output using the inputs returns the average error between each output row and each desired output row using errorFunc*/

            double[] outputs = Compute(inputs);

            double error = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                error += errorFunc(outputs[i], desiredOutputs[i]);
            }

            return error /= outputs.Length;
        }

        public double TrainWithHillClimbing(double[][] inputs, double[] desiredOutputs, double currentError)
        { /*attempts one hill climbing training iteration and returns the new current error*/

            int mutationItem;
            double mutationAmount;

            (mutationItem, mutationAmount) = Mutate();

            double newError = GetError(inputs, desiredOutputs);

            if (newError >= currentError)
            {
                if(mutationItem == weights.Length)
                {
                    bias -= mutationAmount;
                }
                else
                {
                    weights[mutationItem] -= mutationAmount;
                }
                return currentError;
            }

            return newError;

        }
        public static double SquaredError(double currOutput, double actualOutput) => Math.Pow(actualOutput - currOutput, 2);

    }
}
