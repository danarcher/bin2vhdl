using System;
using System.Collections.Generic;
using System.IO;
using Mono.Options;

namespace Converter
{
    class Options
    {
        private readonly string programName;

        public Options(string programName, string[] args)
        {
            this.programName = programName;
            List<string> extra;

            try
            {
                extra = OptionSet.Parse(args);
            }
            catch (OptionException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }

            if (Action != Actions.ShowHelp)
            {
                if (extra.Count != 1)
                {
                    throw new ArgumentException("Expected a single binary file to read");
                }

                SourceFileName = extra[0];
                if (OutputFileName == null)
                {
                    OutputFileName = Path.ChangeExtension(SourceFileName, Action == Actions.ConvertToVhdl ? ".vhdl" : ".mif");
                }
                if (PackageName == null)
                {
                    PackageName = Path.GetFileNameWithoutExtension(OutputFileName);
                }
            }
        }

        public enum Actions
        {
            ShowHelp,
            ConvertToVhdl,
            ConvertToMif,
        }

        public Actions Action { get; private set; } = Actions.ConvertToVhdl;
        public string SourceFileName { get; private set; }
        public string OutputFileName { get; private set; }
        public string PackageName { get; private set; }
        public int Width { get; private set; } = 8;
        public Endian Endian { get; private set; } = Endian.Big;

        private OptionSet OptionSet => new OptionSet
        {
            { "h|?|help", "Print this message and exit.", x => Action = Actions.ShowHelp },
            { "o=|output=", "Write the output to a file.", x => OutputFileName = x ?? OutputFileName },
            { "p=|package=", "Name the generated package.", x => PackageName = x ?? PackageName },
            { "w8", "Write 8 bit values (the default).", x => Width = 8 },
            { "w16", "Write 16 bit values.", x => Width = 16 },
            { "w32", "Write 32 bit values.", x => Width = 32 },
            { "b|big-endian", "Big endian data (the default).", x => Endian = Endian.Big },
            { "l|little-endian", "Little endian data.", x => Endian = Endian.Little },
            { "mif", "Generate a MIF file instead of VHDL.", x => Action = Actions.ConvertToMif }
        };

        public void Describe(TextWriter writer)
        {
            writer.WriteLine($"{programName} v1.0 - Convert binary files to VHDL source files");
            writer.WriteLine($"Copyright (c) 2018 by Dan J Archer");
            writer.WriteLine();
            writer.WriteLine($"Usage: {programName} [options] source.bin");
            writer.WriteLine("Options:");
            OptionSet.WriteOptionDescriptions(writer);
        }
    }
}
