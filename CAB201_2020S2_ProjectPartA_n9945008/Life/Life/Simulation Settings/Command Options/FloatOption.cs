using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    public class FloatOption : CommandOption<float>
    {
        private float min;
        private float max;

        public FloatOption(string name, float defaultValue,
                            double min = Double.NegativeInfinity,
                            double max = Double.PositiveInfinity) :
                            base(name, defaultValue)
        {
            this.min = (float)min;
            this.max = (float)max;
        }

        public void Set(string param)
        {
            float paramNumber = float.Parse(param,
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
                throw new System.ArgumentOutOfRangeException($"{value}", 
                            $"Argument must be within the range {min}:{max}");
            }
        }

        public static implicit operator float(FloatOption I) => I.value;

    }
}