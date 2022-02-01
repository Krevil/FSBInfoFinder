# FSBInfoFinder
Console application for finding FSB stream indexes from info files

Created in order to make it easier to find sounds in Halo MCC's FSB files from a given sound tag.

Steps to use this program:

1. Open a sound tag in Assembly
2. Use the First Pitch Range Index of your sound as the destination index in the Pitch Ranges block in the ugh (i've got a lovely bunch of coconuts) tag
3. Right click on Encoded First Permutation And Counts and use View Value As to get the value as an Int16/UInt16 and use it as the destination index for Permutations
4. Drag the appropriate fsb.info file onto this program's executable and input the FSB Info Value from the Permutations index
