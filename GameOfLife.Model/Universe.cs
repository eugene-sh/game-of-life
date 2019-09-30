using System;
using System.Buffers;

namespace GameOfLife.Model
{
    public class Universe
    {
        public const int UniverseSize = 500;
        
        public int[,] Field { get; }
        public int[,] Field2 { get; }
        
        public Universe()
        {
            Field = new int[UniverseSize, UniverseSize];
            Field2 = new int[UniverseSize, UniverseSize];

            Field[6, 3] = 1;
            Field[7, 4] = 1;
            Field[8, 2] = 1;
            Field[8, 3] = 1;
            Field[8, 4] = 1;
        }

        public void Print()
        {
            for (var i = 0; i < Field.GetLength(0); i++)
            {
                for (var j = 0; j < Field.GetLength(1); j++)
                {
                    Console.Write(Field[i, j]);
                }

                Console.WriteLine();
            }
        }

        public void SetFilled(int x, int y)
        {
            var xx = (int)Math.Floor(Convert.ToDouble(x / 10));
            var yy = (int)Math.Floor(Convert.ToDouble(y / 10));
            
            Field[yy, xx] = 1;
        }

        public int? WhatTodo(int x, int y)
        {
            var n = 0;
            
            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                        continue;

                    if (i < 0 || j < 0)
                        continue;
                    if (i >= UniverseSize || j >= UniverseSize)
                        continue;
                    
                    if (Field[i, j] == 1)
                        n++;
                }
            }

            if (n <= 1 || n >= 4)
                return 0;

            if (n == 3)
                return 1;
            
            return null;
        }

        public void GameStep()
        {
            for (var i = 0; i < Field2.GetLength(0); i++)
            {
                for (var j = 0; j < Field2.GetLength(1); j++)
                {
                    Field2[i, j] = WhatTodo(i, j) ?? Field[i, j];
                    
                   // Console.Write(Field2[i, j]);
                }

                //Console.WriteLine();
            }
            
            for (var i = 0; i < Field2.GetLength(0); i++)
            {
                for (var j = 0; j < Field2.GetLength(1); j++)
                {
                    Field[i, j] = Field2[i, j];
                }
            }
            
        }
    }
}