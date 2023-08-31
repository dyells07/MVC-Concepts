using System;
using System.Collections;
using System.IO;


namespace ExamPrepation {
class Program
    {
        static void Main(string[] args)
        {
            ArrayList al = new ArrayList();
            Console.WriteLine("Adding SomeNumbers:");
            al.Add(45);
            al.Add(47);
            al.Add(48);
            al.Add(49);
            al.Add(40);
            al.Add(44);
            al.Add(42);
            Console.WriteLine("Capacity={0}", al.Capacity);
            Console.WriteLine("Count={0}", al.Count);
            Console.WriteLine("Content: ");
            foreach (int i in al) Console.WriteLine(i + "");


            FileStream F = new FileStream("test.dat", FileMode.OpenOrCreate,
          FileAccess.ReadWrite);

            for (int i = 1; i <= 20; i++)
            {
                F.WriteByte((byte)i);
            }
            F.Position = 0;
            for (int i = 0; i <= 20; i++)
            {
                Console.Write(F.ReadByte() + " ");
            }
            F.Close();
            Console.ReadKey();





        }
    }


}

