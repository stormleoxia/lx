using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Files.Grep
{
    class DirectoryExplorer : IDirectoryExplorer
    {
        private readonly IConsole _console;
        private const int SizeInBytes = 16*1024*1024; // 16M

        public DirectoryExplorer(IConsole console)
        {
            _console = console;
        }

        public void Explore(string directory, string pattern)
        {
            if (Directory.Exists(directory))
            {
                var enumerable = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories);
                var tasks = new Queue<Task>();
                foreach (var file in enumerable)
                {
                    tasks.Enqueue(ExploreFile(pattern, file));
                    if (tasks.Count == 5)
                    {
                        while (tasks.Count != 0)
                        {
                            var task = tasks.Dequeue();
                            task.Wait(20000);
                        }
                    }
                }
            }
        }

        private Task ExploreFile(string pattern, string file)
        {
            return Task.Run(delegate
            {
                StringBuilder builder = new StringBuilder();
                try
                {
                    using (var fileStream = new FileStream(file, FileMode.Open))
                    {
                        using (BufferedStream stream = new BufferedStream(fileStream, SizeInBytes))
                        {
                            using (TextReader reader = new StreamReader(stream))
                            {
                                var lineNumber = 0;
                                var firstOccurence = true;
                                var line = " ";
                                do
                                {
                                    if (line.Contains(pattern))
                                    {
                                        if (firstOccurence)
                                        {
                                            firstOccurence = false;
                                            builder.AppendLine(file + ":");
                                        }
                                        builder.AppendFormat("Line {0}: {1}", lineNumber, line);
                                        builder.AppendLine();
                                    }
                                    line = reader.ReadLine();
                                    lineNumber++;
                                } while (!string.IsNullOrEmpty(line));
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Swallow it (otherwise it will mess the output)
                }
                catch (Exception e)
                {
                    _console.Error.WriteLine("On " + file + ": " + e.Message);
                }
                _console.Write(builder.ToString());
            });
        }
    }
}