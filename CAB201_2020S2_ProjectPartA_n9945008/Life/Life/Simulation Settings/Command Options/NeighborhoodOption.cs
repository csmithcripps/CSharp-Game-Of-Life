using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using Display;

namespace Life
{
    public class NeighborhoodOption : CommandOption<Neighborhood>
    {
        private List<string> allowableValues;

        public NeighborhoodOption(string name, Neighborhood defaultValue,
                            string[] allowableValues) :
                            base(name, defaultValue)
        {
            this.allowableValues = new List<string>(allowableValues);
        }

        public void Set(string param)
        {
            if (allowableValues.Contains(param.ToLower()))
            {
                switch (param.ToLower())
                {
                    case "moore":
                        value = Neighborhood.moore;
                        break;

                    case "vonneumann":
                        value = Neighborhood.vonNeumann;
                        break;
                }
            }
            else
            {
                throw new System.ArgumentOutOfRangeException($"{param}",
                            $"Argument must be either Moore or vonNeumann");
            }
        }


        public static implicit operator Neighborhood(NeighborhoodOption I) => I.value;

    }
}