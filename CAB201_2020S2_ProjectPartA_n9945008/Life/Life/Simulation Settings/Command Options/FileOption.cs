using System.IO;

namespace Life
{
    public class FileOption : CommandOption<string>
    {

        public FileOption(string name, string defaultValue) :
                            base(name, defaultValue)
        { }

        public override void Set(string param)
        {
            // Check that the parameter is in range and assign it
            if (Path.GetExtension(param) == ".seed")
            {
                value = param;
            }
            else
            {
                throw new System.ArgumentException("Some Arguments Entered Incorrectly", param);
            }
        }

        public static implicit operator string(FileOption I) => I.value;

    }
}