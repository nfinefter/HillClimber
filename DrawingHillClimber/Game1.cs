using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using Slope;
using static Slope.Program;

using System;
using System.Collections.Generic;

namespace DrawingHillClimber
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        public Vector2 curr = new Vector2();
        public Vector2 Point1 = new Vector2();
        public Vector2 Point2 = new Vector2();
        public int TryCounter = 0;
        public List<Vector2> Points = new List<Vector2>();
        public Vector2 line = new Vector2();
        public int PointCount = 10;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public (Vector2, Vector2) CoordGen(Vector2 point1, Vector2 point2)
        {
            point1.X = GraphicsDevice.Viewport.X;
            point2.X = GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width;

            point1.Y = curr.Y;
            point2.Y = (point2.X * curr.X) + curr.Y;

            return (point1, point2);
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            Points = PointGen(PointCount, line);

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

            line = LineGen();

            float error = ErrorCalc(curr, Points);

            if (curr != line && TryCounter <= 1500)
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
                (Point1, Point2) = CoordGen(Point1, Point2);
                TryCounter++;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Point 1 is one point, point 2 is 2nd point line connects points
            spriteBatch.DrawLine(Point1, Point2, Color.Red);

            for (int i = 0; i < Points.Count; i++)
            {
                spriteBatch.DrawPoint(Points[i], Color.Black, 50);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}