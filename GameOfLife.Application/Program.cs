using System;
using System.Collections.Generic;
using GameOfLife.Model;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var uni = new Universe();
            var window = new RenderWindow(new VideoMode(500, 500), "SFML running in .NET Core");
            window.Closed += (_, __) => window.Close();
            window.SetFramerateLimit(30);
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.White);
                
                foreach (var f in DrawField(uni))
                {
                    window.MouseButtonPressed += (sender, eventArgs) =>
                    {
                        if (eventArgs.Button == Mouse.Button.Left)
                        {
                            uni.SetFilled(eventArgs.X, eventArgs.Y);
                        }
                    };
                    
                    window.Draw(f);
                }

                window.Display();
                uni.GameStep();
            }
        }
                
        
        public static IEnumerable<RectangleShape> DrawField(Universe u)
        {

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (u.Field[i, j] == 0)
                        yield return new RectangleShape(new Vector2f(10, 10))
                        {
                            FillColor = Color.White,
                            OutlineColor = Color.Black,
                            OutlineThickness = 1,
                            Position = new Vector2f(j * 10, i * 10)
                        };
                    else
                        yield return new RectangleShape(new Vector2f(10, 10))
                        {
                            FillColor = Color.Green,
                            OutlineColor = Color.Black,
                            OutlineThickness = 1,
                            Position = new Vector2f(j * 10, i * 10)
                        };
                }
            }

        }
    }
}