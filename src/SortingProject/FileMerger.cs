using System.Text;

namespace SortingProject
{
    public class FileMerger : IFileMerger
    {
        public void MergeFiles(IList<string> filesToMerge, string outputFile)
        {
            using var outputStream = new FileStream(outputFile, FileMode.Create);
            using var binaryWriter = new BinaryWriter(outputStream);
            var fileStreams = new FileStream[filesToMerge.Count];
            var streamReaders = new StreamReader[filesToMerge.Count];
            var chunkFinished = new bool[filesToMerge.Count];
            var priorityQueue = new PriorityQueue<byte[], byte[]>(new CustomComparer());
            var records = new byte[filesToMerge.Count][];
            try
            {
                for (int i = 0; i < filesToMerge.Count; i++)
                {
                    fileStreams[i] = new FileStream(filesToMerge[i], FileMode.Open, FileAccess.Read, FileShare.None, 1024 * 1024);
                    streamReaders[i] = new StreamReader(fileStreams[i]);
                    var record = ReadSingleLineBytes(streamReaders[i]);
                    records[i] = record;
                    // Add the initial records to the priority queue
                    priorityQueue.Enqueue(record, record);
                }
                while (priorityQueue.Count > 0)
                {
                    var minRecord = priorityQueue.Dequeue();
                    // Find the index of minRecord in the records list
                    int minIndex = Array.IndexOf(records, minRecord);
                    binaryWriter.Write(minRecord);

                    records[minIndex] = ReadSingleLineBytes(streamReaders[minIndex]);
                    if (records[minIndex].Length == 0)
                    {
                        chunkFinished[minIndex] = true;
                    }
                    if (!chunkFinished[minIndex])
                    {
                        // Add the next record from the same file to the priority queue
                        priorityQueue.Enqueue(records[minIndex], records[minIndex]);
                    }
                }
            }
            finally
            {
                // Close and dispose of the input file streams and readers
                for (int i = 0; i < filesToMerge.Count; i++)
                {
                    fileStreams[i].Close();
                    fileStreams[i].Dispose();
                    streamReaders[i].Close();
                    streamReaders[i].Dispose();
                }
                for (int i = 0; i < filesToMerge.Count; i++)
                {
                    string? file = filesToMerge[i];
                    if (File.Exists(file)) File.Delete(file);
                }
            }
        }

        private byte[] ReadSingleLineBytes(StreamReader streamReader)
        {
            var line = streamReader.ReadLine();
            if (line == null)
            {
                return new byte[0];  // End of file reached
            }
            return Encoding.UTF8.GetBytes(line + "\n");
        }
    }


}
