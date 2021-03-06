# bin2vhdl

Convert binary files to VHDL source files or to Altera Memory Initialization Format (MIF) files.

    Usage: bin2vhdl [options] source.bin
    Options:
      -h, -?, --help             Print this message and exit.
      -o, --output=VALUE         Write the output to a file.
      -p, --package=VALUE        Name the generated package.
          --w8                   Write 8 bit values (the default).
          --w16                  Write 16 bit values.
          --w32                  Write 32 bit values.
      -b, --big-endian           Big endian data (the default).
      -l, --little-endian        Little endian data.
      --mif                      Generate a MIF file instead of VHDL.

For example:

    > bin2vhdl sector0.bin -p fancy_pants -w32 --little-endian

Given a binary file whose hex dump is:

    00000000  EB 00 90 20 20 20 20 20  20 20 20 00 02 40 56 02 |...        ..@V.|
    00000010  02 00 00 00 00 F8 00 00  3F 00 FF 00 00 20 00 00 |........?.... ..|
    00000020  00 AC DA 01 D5 0E 00 00  00 00 00 00 02 00 00 00 |................|
    00000030  01 00 06 00 00 00 00 00  00 00 00 00 00 00 00 00 |................|
    00000040  80 00 29 32 64 30 62 4E  4F 20 4E 41 4D 45 20 20 |..)2d0bNO NAME  |
    00000050  20 20 46 41 54 33 32 20  20 20 00 00 00 00 00 00 |  FAT32   ......|
    ...

Produces a file such as:

    -- This file was autogenerated by bin2vhdl - do not edit

    library IEEE;
    use IEEE.std_logic_1164.all;
    use IEEE.numeric_std.all;

    package fancy_pants is
      type fancy_pants_data_t is array(0 to 127) of std_logic_vector(31 downto 0);
      constant fancy_pants_data : fancy_pants_data_t :=
      (X"209000EB",
       X"20202020",
       ...);

    end fancy_pants;
