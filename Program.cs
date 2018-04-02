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
                var vhdl = VhdlConverter.Convert(binary, options.PackageName, options.Width, options.Endian);
                File.WriteAllText(options.OutputFileName, vhdl);
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
