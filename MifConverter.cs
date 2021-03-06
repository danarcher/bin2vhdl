﻿using System;
using System.Text;

namespace Converter
{
    internal class MifConverter
    {
        public static string Convert(byte[] binary, int width, Endian endian)
        {
            if (width != 8 && width != 16 && width != 32)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            var builder = new StringBuilder();
            var byteCount = width / 8;
            var length = binary.Length / byteCount;
            if (length * byteCount < binary.Length)
            {
                ++length;
            }

            Func<int, byte> read = i => (i < binary.Length ? binary[i] : byte.MinValue);

            builder.AppendLine($"-- This file was autogenerated by {Program.Name} - do not edit");
            builder.AppendLine();
            builder.AppendLine($"WIDTH={width};");
            builder.AppendLine($"DEPTH={length};");
            builder.AppendLine();
            builder.AppendLine("ADDRESS_RADIX=HEX;");
            builder.AppendLine("DATA_RADIX=HEX;");
            builder.AppendLine();
            builder.AppendLine("CONTENT BEGIN");
            switch (width)
            {
                case 8:
                    for (var i = 0; i < length; ++i)
                    {
                        var value = read(i);
                        builder.AppendLine($"{i:X8}: {value:X2};");
                    }
                    break;
                case 16:
                    for (var i = 0; i < length; ++i)
                    {
                        var hi = read(i * 2);
                        var lo = read(i * 2 + 1);
                        var value = (endian == Endian.Big) ? ((hi << 8) | lo) : ((lo << 8) | hi);
                        builder.AppendLine($"{i:X8}: {value:X4};");
                    }
                    break;
                case 32:
                    for (var i = 0; i < length; ++i)
                    {
                        var a = read(i * 4);
                        var b = read(i * 4 + 1);
                        var c = read(i * 4 + 2);
                        var d = read(i * 4 + 3);
                        var value = (endian == Endian.Big) ? ((a << 24) | (b << 16) | (c << 8) | d) : ((d << 24) | (c << 16) | (b << 8) | a);
                        builder.AppendLine($"{i:X8}: {value:X8};");
                    }
                    break;
                default:
                    throw new NotImplementedException($"Conversion for width {width} is not implemented");
            }
            builder.AppendLine("END;");
            return builder.ToString();
        }
    }
}
