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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here



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

            Console.WriteLine("How many points would you like to generate?");
            int pointCount = int.Parse(Console.ReadLine()!);

            Vector2 line = LineGen();
            List<Point> points = PointGen(pointCount, line);

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


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.DrawLine(,);

            base.Draw(gameTime);
        }
    }
}