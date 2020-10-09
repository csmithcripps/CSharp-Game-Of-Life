using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    public abstract class CommandOption<T>
    {
        public T value;
        public string name;
        public CommandOption(string name, T defaultValue)
        {
            this.name = name;
            value = defaultValue;
        }
        public T Get() { return value; }
        public virtual void Set(T value) { this.value = value; }

        public override string ToString()
        {
            return $"{value}";
        }


        public void Write(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0, 20}:\t", label);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{value}");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}