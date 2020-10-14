using System;
using System.Collections.Generic;
using System.Globalization;

namespace Life
{
    public class RuleOption : CommandOption<List<int>>
    {
        public RuleOption(string name, List<int> defaultValue) :
                            base(name, defaultValue)
        { }

        public void Set(string[] param)
        {
            value = new List<int>();
            for (int paramCursor = 0; paramCursor < param.Length; paramCursor++)
            {
                if (param[paramCursor] == "...")
                {
                    int rangeStart = int.Parse(param[paramCursor - 1],
                                        NumberStyles.Integer |
                                        NumberStyles.AllowDecimalPoint |
                                        NumberStyles.AllowThousands,
                                        CultureInfo.InvariantCulture);
                    int rangeStop = int.Parse(param[paramCursor + 1],
                                        NumberStyles.Integer |
                                        NumberStyles.AllowDecimalPoint |
                                        NumberStyles.AllowThousands,
                                        CultureInfo.InvariantCulture);
                    for (int i = rangeStart; i <= rangeStop; i++)
                    {
                        if (!value.Contains(i))
                        {
                            value.Add(i);
                        }
                    }
                }
                else
                {
                    int paramNumber = int.Parse(param[paramCursor],
                                            NumberStyles.Integer |
                                            NumberStyles.AllowDecimalPoint |
                                            NumberStyles.AllowThousands,
                                            CultureInfo.InvariantCulture);

                    if (!value.Contains(paramNumber))
                    {
                        value.Add(paramNumber);
                    }

                }

            }
            value.Sort();

        }

        public override void Write(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0, 20}:\t", label);
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (int number in value)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public override string ToString()
        {
            string returnStr = "";

            foreach (int number in value)
            {
                returnStr += number.ToString() + " ";
            }
            return returnStr;
        }
        public static implicit operator List<int>(RuleOption I) => I.value;

        public static implicit operator int[](RuleOption I) => I.value.ToArray();

    }
}