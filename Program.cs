using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace PracticeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager manager = new GameManager();
            manager.Play();
        }
    }
}
