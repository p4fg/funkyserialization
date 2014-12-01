using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ProtoBuf;


namespace FunkySerialization
{

 

    [ProtoContract]
    public class Item
    {
        [ProtoMember(1)] 
        public List<Aggregate> Items;
    }

    [ProtoContract]
    public class Aggregate
    {
        [ProtoMember(1)] 
        public List<SubItem> Items;
    }

    [ProtoContract]
    public class SubItem
    {
        [ProtoMember(1)]
        public long Long1;

        [ProtoMember(2)]
        public long Long2;

        [ProtoMember(3)]
        public long Long3;

        [ProtoMember(4)]
        public long Long4;

        [ProtoMember(5)]
        public long Long5;

        [ProtoMember(6)]
        public long Long6;
    }

    class FunkySerialization
    {
        [Test, Explicit]
        public void ShouldSaveInPrefixStyle()
        {
            Item item = null;
            var stopwatch = new Stopwatch();
            using (
                var fs = File.Open(@"C:\temp\huge.pbuf", FileMode.Open)
                )
            {
                Console.Out.WriteLine("Loading item...");
                Console.Out.Flush();
                item = Serializer.Deserialize<Item>(fs);
                Console.Out.WriteLine("Item deserialized!");
                Console.Out.Flush();
            }
            
            using (
                var fs =
                    File.Create(@"C:\temp\withoutprefix.pbuf"))
            {
                Console.Out.WriteLine("Starting serializing without prefix.");
                Console.Out.Flush();
                stopwatch.Start();
                Serializer.Serialize(fs,item);
                stopwatch.Stop();
                Console.Out.WriteLine("Done serializing without prefix. Time=" + stopwatch.ElapsedMilliseconds  + " ms");
                Console.Out.Flush();
            }

            var fileSizeWithout = new FileInfo(@"C:\temp\withoutprefix.pbuf").Length;

            using (
                var fs =
                    File.Create(@"C:\temp\withprefix.pbuf"))
            {
                Console.Out.WriteLine("Starting serializing WITH prefix.");
                Console.Out.Flush();
                stopwatch.Reset();
                stopwatch.Start();
                Serializer.SerializeWithLengthPrefix(fs,item, PrefixStyle.Base128);
                stopwatch.Stop();
                Console.Out.WriteLine("Done serializing WITH prefix. Time=" + stopwatch.ElapsedMilliseconds + " ms");
                Console.Out.Flush();
            }

            var fileSizeWith = new FileInfo(@"C:\temp\withprefix.pbuf").Length;
            Assert.That(fileSizeWith, Is.EqualTo(fileSizeWithout).Within(80).Percent);


        }
    }
}
