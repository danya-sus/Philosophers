using System;
using System.Collections.Generic;
using System.Threading;

namespace Philosophers
{
    class Program
    {
        private static int PHILOSOFERS = 5;
        private static int FORKS = 5;
        private static int THOUGHTS = 10;
        private static int _maxOfThoughts = 3;
        private static Random _random = new Random();

        //------------------

        public class Fork
        {
            public bool Free;
            public Mutex mutex = new Mutex();
            public Fork()
            {
                Free = true;
            }
        }
        public class Philosoph
        {
            public int Id;
            public int _numberOfThoughts;
            public Fork left_fork = new Fork();
            public Fork right_fork = new Fork();

            public Philosoph(int _id)
            {
                Id = _id;
                _numberOfThoughts = 0;
            }
            public void Reflection()
            {
                _numberOfThoughts++;
            }
            public void Eating()
            {
                _numberOfThoughts = 0;
            }
        }

        private static List<Fork> forks = new List<Fork>();

        static void Main(string[] args)
        {
            Thread[] philosofers = new Thread[PHILOSOFERS];
            for (int i = 0; i < PHILOSOFERS; i++)
            {
                philosofers[i] = new Thread(new ParameterizedThreadStart(PhilosofherMain));
                philosofers[i].Name = $"№{i + 1}";
                philosofers[i].Start(i);
            }

            foreach (var p in philosofers)
            {
                p.Join();
            }

            Console.WriteLine("Main thread ended.");
        }

        public static void PhilosofherMain(object id)
        {
            int _id = (int)id;
            Philosoph p = new Philosoph(_id);

            for (int i = 0; i < THOUGHTS; i++)
            {
                p.Reflection();
                Thread.Sleep(300);
                ConsoleHelper.WriteToConsole("Философ " + Thread.CurrentThread.Name, " думает " + p._numberOfThoughts + " раз подряд.");

                //Рандом для интереса - философы кушают когда хотят или когда нужно
                //Вопрос по флагу - нужен ли он в классе вообще?
                if (p._numberOfThoughts == 3 || _random.Next(0, 2) == 1)
                {
                    p.left_fork.mutex.WaitOne();
                    //p.left_fork.Free = false;

                    if (p.right_fork.mutex.WaitOne())
                    {
                        //Вопрос по мьютексу - нужно ли писать следующую строку, если есть условие?
                        //p.right_fork.mutex.WaitOne();
                        //p.right_fork.Free = false;
                        ConsoleHelper.WriteToConsole("Философ " + Thread.CurrentThread.Name, " кушает.");
                        p.Eating();
                        Thread.Sleep(300);
                        //p.left_fork.Free = true;
                        //p.right_fork.Free = true;
                        p.left_fork.mutex.ReleaseMutex();
                        p.right_fork.mutex.ReleaseMutex();
                    }
                    else
                    {
                        //p.left_fork.Free = true;
                        p.left_fork.mutex.ReleaseMutex();
                    }
                }
                if (p._numberOfThoughts > 3)
                {
                    ConsoleHelper.WriteToConsole("Философ " + Thread.CurrentThread.Name, " умирает. -------------------- Смерть философа.");
                    return;
                }
            }
        }
    }
}
