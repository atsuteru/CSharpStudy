using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;

namespace CSharpStudy
{
    [TestClass]
    public class StringBuilderTest
    {
        [TestMethod]
        public void Concat1()
        {
            int numberOfItem = 4;

            Debug.WriteLine("5��");
            //wata
            Debug.WriteLine("--- wata ---");
            Debug.WriteLine(numberOfItem+"��");
            //tachi
            Debug.WriteLine("--- tachi ---");
            var builder = new StringBuilder();
            builder.Append(numberOfItem);
            builder.Append("��");
            Debug.WriteLine(builder.ToString());
            //kami
            Debug.WriteLine("--- kami ---");
            Debug.WriteLine($"{numberOfItem}��");
        }
    }
}
