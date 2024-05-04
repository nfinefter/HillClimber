using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using Slope;
using static Slope.Program;

using HillClimber;
using System;
using System.Collections.Generic;

namespace DrawingHillClimber
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        public Vector2 Curr = new Vector2();
        public Vector2 Point1 = new Vector2();
        public Vector2 Point2 = new Vector2();
        public int TryCounter = 0;
        public Vector2 Line = new Vector2();
        public int PointCount = 10;
        public int Multiple;
        public Vector2 Offset;
        public float Error;

        ErrorFunction errorFunc;
        ActivationFunction activationFunc;
        Perceptron perceptron;
        double[][] inputs;
        double[] desiredOutputs;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        public (int, Vector2) MapPoints(List<Vector2> points)
        {
            Vector2 left = points[0];
            Vector2 right = points[0];
            Vector2 top = points[0];
            Vector2 bottom = points[0];


            for (int i = 1; i < points.Count; i++)
            {
                if (left.X > points[i].X)
                {
                    left = points[i];
                }
                if (right.X < points[i].X)
                {
                    right = points[i];
                }
                if (top.Y > points[i].Y)
                {
                    top = points[i];
                }
                if (bottom.Y < points[i].Y)
                {
                    bottom = points[i];
                }
            }
            Vector2 offset = new Vector2(left.X, top.Y);

            int multiple1 = (int)(GraphicsDevice.Viewport.Width / (right.X - offset.X));
            int multiple2 = (int)(GraphicsDevice.Viewport.Height / (bottom.Y - offset.Y));

            return (Math.Min(multiple2, multiple1), offset);
        }
        public (Vector2, Vector2) CoordGen(Vector2 point1, Vector2 point2)
        {
            point1.X = GraphicsDevice.Viewport.X;
            point2.X = GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width;

            point1.Y = Curr.Y;
            point2.Y = (point2.X * Curr.X) + Curr.Y;

            return (point1, point2);
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            Line = LineGen();

            Points = PointGen(PointCount, Line);
            (Multiple, Offset) = MapPoints(Points);


            //CoordGen();

            errorFunc = new ErrorFunction(Perceptron.SquaredError, Perceptron.DerivativeSquaredError);
            activationFunc = new ActivationFunction(Perceptron.Sigmoid, Perceptron.DerivativeSigmoid);

            //Line of Best Fit Perceptron
            perceptron = new Perceptron(3, 0.001, Random.Shared, errorFunc);
            
            //Gradient Descent Perceptron
            //perceptron = new Perceptron(3, 0.001, activationFunc, errorFunc);

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
            desiredOutputs = new double[]{ 0, 0, 0, 1, 0, 1, 1, 1 };


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Console.WriteLine("How many points would you like to generate?");
            //int pointCount = int.Parse(Console.ReadLine()!);             

            //HillClimber:
            //Line = LineGen();

            //Error = ErrorCalc(Curr, Points);

            //if (Curr != Line && TryCounter <= 5000)
            //{
            //    Vector2 temp = Mutate(Curr);
            //    float newError = ErrorCalc(temp, Points);

            //    if (Error > newError)
            //    {
            //        Curr = temp;
            //        Error = newError;
            //    }
            //    Console.WriteLine($"{Curr.X}, {Curr.Y}");
            //    Console.WriteLine(Error);
            //    (Point1, Point2) = CoordGen(Point1, Point2);
            //    TryCounter++;
            //}

            //Perceptron Line of Best Fit:

            double error = perceptron.TrainWithHillClimbing(inputs, desiredOutputs, int.MaxValue);

            for (int i = 0; i < 5000; i++)
            {
                error = perceptron.TrainWithHillClimbing(inputs, desiredOutputs, error);
                error = activationFunc.Function(error);


                for (int j = 0; j < inputs.Length; j++)
                {
                    for (int k = 0; k < inputs[j].Length; k++)
                    {
                        Console.Write(inputs[j][k]);
                    }
                    Console.WriteLine($" {error}");
                }
            }


            //Perceptron AND/OR Gate:

            //double error = perceptron.Train(inputs, desiredOutputs);

            //for (int i = 0; i < 5000; i++)
            //{
            //    error = perceptron.Train(inputs, desiredOutputs);

            //    double[] output = perceptron.Compute(inputs);

            //    for (int j = 0; j < inputs.Length; j++)
            //    {
            //        for (int k = 0; k < inputs[j].Length; k++)
            //        {
            //            Console.Write(inputs[j][k]);
            //        }
            //        Console.WriteLine($" {perceptron.activationFunc.Function(output[j])}");

            //    }
            //}

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            var dataBreadth = GraphicsDevice.Viewport.Width / Multiple;
            //Point 1 is one point, point 2 is 2nd point Line connects points
           
            var yIntercept = new Vector2(0, Line.Y - Offset.Y);
            spriteBatch.DrawLine(yIntercept * Multiple, new Vector2(GraphicsDevice.Viewport.Width, (yIntercept.Y + Line.X * dataBreadth) * Multiple), Color.Red, 10);

            for (int i = 0; i < Points.Count; i++)
            {
                spriteBatch.DrawPoint((Points[i] - Offset) * Multiple, Color.Black, 10);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}