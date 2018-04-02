using System;
using System.IO;

namespace Converter
{
    class Program
    {
        public const string Name = "bin2vhdl";

        private const int Success = 0;
        private const int Failure = 1;

        static int Main(string[] args)
        {
            Options options;
            try
            {
                options = new Options(Name, args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{Name}: {ex.Message}");
                return Failure;
            }

            if (options.Action == Options.Actions.ShowHelp)
            {
                options.Describe(Console.Error);
                return Failure;
            }

            try
            {
                var binary = File.ReadAllBytes(options.SourceFileName);
                string text;
                switch (options.Action)
                {
                    case Options.Actions.ConvertToVhdl:
                        text = VhdlConverter.Convert(binary, options.PackageName, options.Width, options.Endian);
                        break;
                    case Options.Actions.ConvertToMif:
                        text = MifConverter.Convert(binary, options.Width, options.Endian);
                        break;
                    default:
                        throw new NotImplementedException($"{options.Action} not implemented");
                }
                File.WriteAllText(options.OutputFileName, text);
                return Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{Name}: {ex.Message}");
                return Failure;
            }
        }
    }
}
