using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HillClimber
{
    public class Perceptron
    {
        public double LearningRate { get; set; }
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
        public Random random;
        public ActivationFunction activationFunc;
        public ErrorFunction errorFunc;

        public Perceptron(int amountOfInputs, double learningRate, ActivationFunction activationFunction, ErrorFunction errorFunction)
        { /*Initializes the Perceptron*/

            weights = new double[amountOfInputs];
            LearningRate = learningRate;
            activationFunc = activationFunction;
            errorFunc = errorFunction;
        }
        public Perceptron(double[] initialWeightValues, double initialBiasValue, double mutationAmount, Random random, ErrorFunction errorFunc)
        { /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            this.mutationAmount = mutationAmount;
            this.random = random;
            this.errorFunc = errorFunc;
        }
        public Perceptron(int amountOfInputs, double mutationAmount, Random random, ErrorFunction errorFunc)
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
        public double ActivationCompute(double[] inputs)
        {
            //Adds activation function to the base compute function

            double activationInput = Compute(inputs);
            return activationFunc.Function(activationInput);
        }
        public double GetError(double[][] inputs, double[] desiredOutputs)
        { /*computes the output using the inputs returns the average error between each output row and each desired output row using errorFunc*/

            double[] outputs = Compute(inputs);

            double error = 0;

            for (int i = 0; i < outputs.Length; i++)
            {
                error += errorFunc.Function(outputs[i], desiredOutputs[i]);
            }

            return error /= outputs.Length;
        }
        public double PartialDerivative(double[] inputs, double desiredOutput, double input) => errorFunc.Derivative(activationFunc.Function(input), desiredOutput) * activationFunc.Derivative(activationFunc.Function(Compute(inputs)));
        public double Train(double[] inputs, double desiredOutput)
        { /*trains the perceptron using gradient descent for one iteration and returns the error */

            double activationInput = ActivationCompute(inputs);
            double output = activationFunc.Function(activationInput);
            double error = errorFunc.Function(output, desiredOutput);

      
            double weightChangeScalar = errorFunc.Derivative(output, desiredOutput) * activationFunc.Derivative(activationInput) * -LearningRate;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += weightChangeScalar * inputs[i];
            }
            bias += weightChangeScalar;

            return error;
        }

        public double Train(double[][] inputs, double[] desiredOutputs)
        { /*batch trains the perceptron using gradient descent for one iteration and returns the error */
            double[] activationInputs = new double[inputs.Length];
            double[] outputs = new double[inputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                double activationInput = Compute(inputs[i]);
                activationInputs[i] = activationInput;
                outputs[i] = activationFunc.Function(activationInput);
            }

            double error = GetError(inputs, desiredOutputs);


            for (int i = 0; i < desiredOutputs.Length; i++)
            {
                double weightChangeScalar = errorFunc.Derivative(outputs[i], desiredOutputs[i]) * activationFunc.Derivative(activationInputs[i]) * -LearningRate;

                for (int j = 0; j < weights.Length; j++)
                {
                    weights[j] += weightChangeScalar * inputs[i][j];
                }
                bias += weightChangeScalar;
            }

            return error;
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
                if (mutationItem == weights.Length)
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

        public static double DerivativeSquaredError(double currOutput, double actualOutput) => 2 * (actualOutput - currOutput);

        public static double Sigmoid(double input) => 1 / (1 + (Math.Exp(-Math.Abs(input))));

        public static double DerivativeSigmoid(double input) => Sigmoid(input) * (1 - Sigmoid(input));

        public static double Binary(double input) => input < 0.5f ? 0 : 1;
    }
}
