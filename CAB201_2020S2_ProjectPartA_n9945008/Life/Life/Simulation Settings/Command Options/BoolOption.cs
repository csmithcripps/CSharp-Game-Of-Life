namespace Life
{
    public class BoolOption : CommandOption<bool>
    {
        public BoolOption(string name, bool defaultValue) : base(name, defaultValue) { }
        public static implicit operator bool(BoolOption I) => I.value;
        public static explicit operator BoolOption(bool I) =>
                                        new BoolOption(nameof(I).ToString(), I);
    }
}