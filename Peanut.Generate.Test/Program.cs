using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peanut.Generate.Test
{
    class Program
    {
    
        static void Main(string[] args)
        {
            
            string code;
            System.IO.FileStream stream = System.IO.File.Open("c:\\DBModel.cs", System.IO.FileMode.Open);
            GenerateCode gc = new GenerateCode();
            System.IO.Stream st = gc.Builder(stream);
            st.Seek(0, System.IO.SeekOrigin.Begin);
            using (System.IO.StreamReader r = new System.IO.StreamReader(st))
            {
                code = r.ReadToEnd();
                Console.Write(code);

            }


            Console.Read();
        }
    }

}

