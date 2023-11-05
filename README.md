# ExternalSorting

## Sorting Project
The Sorting Project is a C# [External Sorting](https://en.wikipedia.org/wiki/External_sorting) project designed to take a large input file, split it into smaller chunks, sort the chunks, and then merge them into a single sorted output file.
The project sorts text-based files line by line where each line consists of a random number, followed by a period(.), then a white space, and then a random text, and apply the sorting based on the text part, then the number part of the line.

### Algorithm
The sorting process in this project involves the following steps:
- Splitting the Input File: The input file is initially divided into smaller, manageable chunks. This is accomplished by the FileSplitter class.
- Sorting Chunks: Within each chunk, the data is split into individual lines and sorted in memory using the CustomComparer. This step is executed in the FileSorter class.
- Merging Sorted Chunks: The sorted lines from each chunk are merged into a single byte array or file while preserving the sorted order. The FileMerger class handles this process. A [priority queue (min-heap)](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2) efficiently selects the next line to be merged based on the comparison logic provided by the CustomComparer.

### Usage
To use the Sorting Project, you can follow these steps:
1. Clone or download the project to your local machine.
2. Open the project in your preferred C# development environment (e.g., Visual Studio).
3. Build and run the project. You can execute the program with the following command:

   ```shell
   dotnet run --project SortingProject -- -i input.txt -o output.txt -d temp -c 100
   
This command will read the input file from the same execution directory, split the input file into chunks, store them in temp folder, each chunk is 100MB, sort the chunks and merge them into a single output.txt sorted file.

The following attributes could be configured:
- ```-i``` The input file path to be sorted (including the file name).
- ```-o``` The output file path to be sorted (including the file name).
- ```-d``` The temporary output directory to store chunks.
- ```-c``` The chunk size in MB.

### Time Complexity
The overall time complexity is typically O(N * log(N)), where N is the total number of records in the input data.

### Parallel Processing
In some parts of the code, parallel processing is used to speed up the sorting process. Multiple chunks can be sorted and merged simultaneously using multiple threads, making the sorting process more efficient (but more memory consuming :smile:).


## File Generator Project
The File Generator project is a C# application that generates a file filled with random data following these constraints:
- Each line in the file consists of a random number, followed by a period(.), then a white space, and then a random text, e.g. ```123. ABCD```
- The file must contain some lines sharing the same text part.

### Usage
To use the File Generator, you can follow these steps:
1. Clone or download the project to your local machine.
2. Open the project in your preferred C# development environment (e.g., Visual Studio).
3. Build and run the project. You can execute the program with the following command:

   ```shell
   dotnet run --project FileGenerator -- -f output.txt -s 10 -t 5 -T 15 -n 1 -N 1000
   
This command will generate a 10MB text file located in the same execution directory, named ```output.txt```, the random numbers of each line between 1 and 1000 and the string length of each line between 5 and 15 characters.

The following attributes could be configured:
- ```-f``` The output file path to be generated (including the file name).
- ```-s``` The file size in MB.
- ```-t``` and ```-T``` The minimum and the maximum value of the random number in each line.
- ```-n``` and ```-N``` The minimum and the maximum length of the text part in each line.
