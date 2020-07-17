using System;
using System.Collections.Generic;
using System.IO;


namespace Task2
{
    public delegate double Func(double x);

    class MinimumF
    {

        public static double F(double x)
        {
            return x * x - 50 * x + 10;
        }

        public static double F2(double x)
        {
            return Math.Sin(x);
        }

        public static double F3(double x)
        {
            return Math.Cos(x);
        }

        private static readonly List<Func> Functions = new List<Func>{F,F2,F3};

        public static void SaveFunc(string fileName, double a, double b, double h, Func f)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                Console.WriteLine(f(x));
                bw.Write(f(x));
                x += h;// x=x+h;
            }
            bw.Close();
            fs.Close();
        }

        public static double[] Load(string fileName, out double min)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            min = double.MaxValue;
            double d;
            List<double> valueList = new List<double>();
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d = bw.ReadDouble();
                if (d < min) min = d;
                valueList.Add(d);
            }
            bw.Close();
            fs.Close();

            return valueList.ToArray();
        }

        public static void Menu()
        {
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"
                               Программа нахождения минимума функций.");
                Console.WriteLine(@"
                                1. Формула функции y = x * x-50*x+10
                                2. Формула функции y = sin(x) 
                                3. Формула функции y = cos (x)");
                Console.WriteLine("\t\tВыбери номер функции минимум которой хочешь найти:");

                bool isCorrect;
                int v;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\tВведи цифру от одного до трех.                        Для выхода введи ноль.");

                    if (int.TryParse(Console.ReadLine(), out v))
                        isCorrect = !(v > 0 && v < 4);
                    else isCorrect = true;
                    if(v == 0) return;
                }
                while (isCorrect);

                switch (v)
                {
                    case 1:
                        Console.WriteLine(@"
                              Выбрана функция №1 : Формула функции:  y = x * x-50*x+10");
                        Demonstration(Functions[0]);
                        break;

                    case 2:
                        Console.WriteLine(@"
                              Выбрана функция №2 : Формула функции   y = sin(x)");
                        Demonstration(Functions[1]);
                        break;
                    case 3:
                        Console.WriteLine(@"
                              Выбрана функция №3 : Формула функции:  y = cos (x)");
                        Demonstration(Functions[2]);
                        break;
                }

                Console.ReadKey();
            } while (true);
        }

        private static void Demonstration(Func f)
        {
            Console.WriteLine("\tНа каком отрезке будем искать минимум функции?");
            Console.WriteLine("\tВведи минимальное значение отрезка ");
            double a = CorrectInput();
            Console.WriteLine("\tВведи максимальное значение отрезка ");
            double b = CorrectInput();
            SaveFunc("data.bin", a, b, 0.5, f);
            Console.ForegroundColor = ConsoleColor.Red;
            Load("data.bin", out double min);
            Console.WriteLine($" \nМинимальное значение функции найдено! Вот оно: {min}");
        }

        private static double CorrectInput()
        {
            bool isCorrect;
            double res;
            do
            {
                Console.WriteLine("\tНеобходимо ввести вещественное число: ");
                isCorrect = !double.TryParse(Console.ReadLine(), out res);
                if (isCorrect)
                {
                    Console.Beep();
                    Console.WriteLine("NO!");
                }
            } while (isCorrect);
            Console.WriteLine("OK");
            return res;
        }
    }
}
