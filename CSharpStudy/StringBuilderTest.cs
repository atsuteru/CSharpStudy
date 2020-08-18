using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CSharpStudy
{
    [TestClass]
    public class StringBuilderTest
    {
        [TestMethod]
        public void Concat1()
        {
            int numberOfItem = 4;
            int lineCounter = 0;

            var watch = new Stopwatch();

            Debug.WriteLine("5ŒÂ");
            //wata
            Debug.WriteLine("--- wata ---");
            watch.Start();
            while(watch.ElapsedMilliseconds <= 3000)
            {
                Debug.WriteLine(numberOfItem + "ŒÂ");
                lineCounter++;
            }
            watch.Stop();
            Debug.WriteLine($"---End of wata--- score:{lineCounter:#,##0}");
            Thread.Sleep(3000);

            //tachi
            Debug.WriteLine("--- tachi ---");
            lineCounter = 0;
            watch.Reset();
            watch.Start();
            var builder = new StringBuilder();
            while (watch.ElapsedMilliseconds <= 3000)
            {
                builder.Clear();
                builder.Append(numberOfItem);
                builder.Append("ŒÂ");
                Debug.WriteLine(builder.ToString());
                lineCounter++;
            }
            watch.Stop();
            Debug.WriteLine($"---End of tachi--- score:{lineCounter:#,##0}");
            Thread.Sleep(3000);
            //kami
            Debug.WriteLine("--- kami ---");
            lineCounter = 0;
            watch.Reset();
            watch.Start();
            while (watch.ElapsedMilliseconds <= 3000)
            {
                Debug.WriteLine($"{numberOfItem}ŒÂ");
                lineCounter++;
            }
            watch.Stop();
            Debug.WriteLine($"---End of kami--- score:{lineCounter:#,##0}");
            Thread.Sleep(3000);


        }
    }
}
