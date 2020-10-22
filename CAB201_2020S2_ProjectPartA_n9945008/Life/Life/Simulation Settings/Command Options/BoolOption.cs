namespace Life
{
    public class BoolOption : CommandOption<bool>
    {
        public BoolOption(string name, bool defaultValue) : base(name, defaultValue) { }

        public void Set(string param)
        {
            switch (param)
            {
                case "true":
                    value = true;
                    break;

                case "false":
                    value = false;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException(param, "Argument must be either true or false");
            }
        }
        public static implicit operator bool(BoolOption I) => I.value;
        public static explicit operator BoolOption(bool I) =>
                                        new BoolOption(nameof(I).ToString(), I);
    }
}