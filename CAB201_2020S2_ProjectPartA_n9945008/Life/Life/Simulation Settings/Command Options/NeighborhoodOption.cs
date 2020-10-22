using System.Collections.Generic;
using System;

namespace Life
{
    public class NeighborhoodOption : CommandOption<NeighborhoodType>
    {
        private List<string> allowableValues;
        public IntOption order = new IntOption("Order", 1, 1,10);
        public BoolOption centerCount = new BoolOption("Center Count", false);

        public NeighborhoodOption(string name, NeighborhoodType defaultValue,
                            string[] allowableValues) :
                            base(name, defaultValue)
        {
            this.allowableValues = new List<string>(allowableValues);
        }

        public void Set(string[] paramArray)
        {
            string param = paramArray[0];
            if (allowableValues.Contains(param.ToLower()))
            {
                switch (param.ToLower())
                {
                    case "moore":
                        value = NeighborhoodType.moore;
                        break;

                    case "vonneumann":
                        value = NeighborhoodType.vonNeumann;
                        break;
                }
            }
            else
            {
                throw new System.ArgumentOutOfRangeException($"{param}",
                            $"Argument must be either Moore or vonNeumann");
            }

            param = paramArray[1];
            order.Set(param);

            param = paramArray[2];
            centerCount.Set(param);
        }


        public override void Write(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0, 20}:\t", label);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\t\t(Type) {value}");
            Console.WriteLine($"\t\t(Order) {order}");
            Console.WriteLine($"\t\t(Center Count) {centerCount}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static implicit operator NeighborhoodType(NeighborhoodOption I) => I.value;

    }
}