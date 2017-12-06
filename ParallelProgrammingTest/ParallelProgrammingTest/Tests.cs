using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ParallelProgrammingTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var example = new Example();
            var fize = example.GetDirSize("/Users/romcabral/Documents/trend/GIT");
            
            // this should not be 0 of course...
            Assert.AreEqual(0, fize);
            //Console.WriteLine("Dir size {0}", fize);
        }
    }
    
    // http://reedcopsey.com/series/parallelism-in-net4/
    class Example
    {
        // https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-foreach-loop
        public long GetDirSize(string dir)
        {
            long totalSize = 0;
            
            // A simple source for demonstration purposes. Modify this path as necessary.
            String[] files = System.IO.Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            

            // Method signature: Parallel.ForEach(IEnumerable<TSource> source, Action<TSource> body)
            // Be sure to add a reference to System.Drawing.dll.
            
            Parallel.ForEach(files, (currentFile) => 
            {
                // The more computational work you do here, the greater 
                // the speedup compared to a sequential foreach loop.
                String filename = System.IO.Path.GetFileName(currentFile);

                Interlocked.Add(ref totalSize, new FileInfo(currentFile).Length);
                
                // Peek behind the scenes to see how work is parallelized.
                // But be aware: Thread contention for the Console slows down parallel loops!!!

                Console.WriteLine("Processing {0} on thread {1}", filename, Thread.CurrentThread.ManagedThreadId);
                //close lambda expression and method invocation
            });


            return totalSize;
        }
    }
}