using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace syqwq
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public void Introduce()
        {
            WriteLine($"Hello, my name is {Name} and I am {Age} years old.");
        }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
    namespace hello
    {
        internal class Program
        {
            static void Main(string[] args)
            {
                syqwq.Person person = new syqwq.Person("wbw", 18);
                person.Introduce();
                ReadKey();
            }
        }
    }
}
