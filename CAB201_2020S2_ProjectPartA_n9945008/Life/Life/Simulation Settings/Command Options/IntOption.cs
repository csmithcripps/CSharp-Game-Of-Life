using System;
using System.Globalization;

namespace Life
{
    public class IntOption : CommandOption<int>
    {
        private double min;
        private double max;

        public IntOption(string name, int defaultValue,
                            double min = Double.NegativeInfinity,
                            double max = Double.PositiveInfinity) :
                            base(name, defaultValue)
        {
            this.min = min;
            this.max = max;
        }

        public void Set(string param)
        {
            int paramNumber = int.Parse(param,
                                    NumberStyles.Integer |
                                    NumberStyles.AllowDecimalPoint |
                                    NumberStyles.AllowThousands,
                                    CultureInfo.InvariantCulture);
            if ((paramNumber >= min) && (paramNumber <= max))
            {
                value = paramNumber;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException($"{paramNumber}",
                            $"Argument must be within the range {min}:{max}");
            }
        }

        public static implicit operator int(IntOption I) => I.value;

    }
}