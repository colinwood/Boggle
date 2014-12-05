using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleClientModel;
using System.Threading;

namespace BoggleClientTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConnectTest()
        {
            Model client1 = new Model();
            Model client2 = new Model();
            
            Thread t1 = new Thread(() => client1.Connect("john", "localhost"));
            Thread t2 = new Thread(() => client2.Connect("Ben", "localhost"));
            t1.Start();
            t2.Start();
            Thread.Sleep(10000);
        }

        [TestMethod]
        public void PlayWord()
        {
            Model client1 = new Model();
            Model client2 = new Model();
            

            Thread t1 = new Thread(() => client1.Connect("john", "localhost"));
            Thread t2 = new Thread(() => client2.Connect("Ben", "localhost"));
            t1.Start();
            t2.Start();
            Thread.Sleep(5000);
            
            client1.PlayWord("jasdkasjh");
            client2.PlayWord("kajshdkasjh");
            Thread.Sleep(10000);
            Assert.AreEqual(-1, client1.self_score);
            Thread.Sleep(3000);           
        }

        [TestMethod]
        public void GameSummaryCLientTest()
        {
            Model client1 = new Model();
            Model client2 = new Model();


            Thread t1 = new Thread(() => client1.Connect("john", "localhost"));
            Thread t2 = new Thread(() => client2.Connect("Ben", "localhost"));
            t1.Start();
            t2.Start();
            Thread.Sleep(2000);

            client1.PlayWord("grrr");
            client2.PlayWord("grrr");
            Thread.Sleep(15000);
            Assert.AreEqual(-1, client2.messages);
            Thread.Sleep(3000);
        }
    }
}
