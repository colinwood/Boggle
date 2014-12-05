// Authored By Victor Marruffo & Colin Wood
// 11/25/2014

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleServer;
using System.Collections.Generic;
using System.Net.Sockets;
using CustomNetworking;
using System.Text;
using System.Threading;

namespace BoggleServer
{
    [TestClass]
    public class UnitTest1
    {
 
        public StringSocket ss1;
        public StringSocket ss2;
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public String incoming_message = "";
        public BoggleServer b;
        public HashSet<String> incoming_message_stream1;
        public HashSet<String> incoming_message_stream2;

        /// <summary>
        /// Start the server and stop it.
        /// </summary>
        [TestMethod]
        public void ConstructorTest()
        {        
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(Stop);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
        }

        /// <summary>
        /// Check that we can connect to the server send connect message 3 seconds after it starts
        /// </summary>
        [TestMethod]
        public void SendConnectTest()
        {        
            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(5000);
            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message );
        }

        /// <summary>
        /// Check that we can connect to the server send connect message 3 seconds after it starts
        /// </summary>
        [TestMethod]
        public void TimerTest()
        {

            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(5000);
            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message);
            incoming_message_stream1 = new HashSet<string>();

            Thread t3 = new Thread(Receive1);
            t3.Start();

            Thread.Sleep(10000);
            Assert.AreEqual(incoming_message_stream1.ToString(), new HashSet<string>(){"10","9","8","7","6","5","4","3","2","1"}.ToString());
       
        }

        /// <summary>
        /// Check that we can connect to the server send connect message 3 seconds after it starts
        /// </summary>
        [TestMethod]
        public void ScoreReceiveTest()
        {

            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(5000);
            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message);

            incoming_message_stream1 = new HashSet<string>();
            incoming_message_stream2 = new HashSet<string>();

            Thread t3 = new Thread(Receive1);
            t3.Start();

            Thread.Sleep(3000);
            ss1.BeginSend("WORD SAKDJHASK\n", (e, o) => { }, ss1);
            Thread.Sleep(3000);

            Assert.AreEqual(true , String.Join(" ", incoming_message_stream1).Contains("SCORE -1 0"));

        }


        /// <summary>
        /// Check that the game summary at the end of the game
        /// </summary>
        [TestMethod]
        public void GameSummaryTest()
        {

            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(5000);
            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message);
            incoming_message_stream1 = new HashSet<string>();
            incoming_message_stream2 = new HashSet<string>();

            Thread t3 = new Thread(Receive1);
            Thread t4 = new Thread(Receive2);
            t3.Start();
            t4.Start();


            Thread.Sleep(3000);
            ss1.BeginSend("WORD BLOWfish\n", (e, o) => { }, ss1);
            Thread.Sleep(20000);

            Assert.AreEqual(true, String.Join(" ", incoming_message_stream1).Contains("STOP 1 BLOWFISH"));
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream2).Contains("STOP 0  1 BLOWFISH"));

        }

        [TestMethod]
        public void ScoringTest()
        {

            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(3000);

            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message);
            incoming_message_stream1 = new HashSet<string>();
            incoming_message_stream2 = new HashSet<string>();

            Thread t3 = new Thread(Receive1);
            Thread t4 = new Thread(Receive2);
            t3.Start();
            t4.Start();

            Thread.Sleep(1000);

            ss1.BeginSend("WORD AAAA\n", (e, o) => { }, ss1);
            Thread.Sleep(1000);
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream1).Contains("SCORE 1 0"));
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream2).Contains("SCORE 0 1"));


            ss2.BeginSend("WORD AAAA\n", (e, o) => { }, ss2);
            Thread.Sleep(1000);
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream1).Contains("SCORE 0 0"));
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream2).Contains("SCORE 0 0"));

            ss2.BeginSend("WORD BIRD\n", (e, o) => { }, ss2);
            Thread.Sleep(1000);
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream1).Contains("SCORE 0 -1"));
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream2).Contains("SCORE -1 0"));

            ss2.BeginSend("WORD AAAAA\n", (e, o) => { }, ss2);
            Thread.Sleep(1000);
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream1).Contains("SCORE 0 1"));
            Assert.AreEqual(true, String.Join(" ", incoming_message_stream2).Contains("SCORE 1 0"));
        }

        /// <summary>
        /// Check that we can connect to the server send and then if one player disconnects terminate is sent to the reaming player
        /// </summary>
        [TestMethod]
        public void TerminateTest()
        {

            allDone = new ManualResetEvent(false);
            Thread t1 = new Thread(StartServer);
            Thread t2 = new Thread(SendConnectMessage);

            t1.Start();
            Thread.Sleep(2000);
            t2.Start();
            Thread.Sleep(5000);
            ss1.BeginReceive(NewGameCallBack, new Object());
            allDone.WaitOne();
            Assert.AreEqual("START AAAAAAAAAAAAAAAA 10 John", incoming_message);
            incoming_message_stream1 = new HashSet<string>();
            try
            {
            Thread t3 = new Thread(Receive1);
            t3.Start();

            Thread.Sleep(3000);
            
                ss2.Close();
            }
            catch (Exception e)
            {

            }
            Thread.Sleep(3000);
           
           
            Assert.AreEqual(true, String.Join(" ",incoming_message_stream1).Contains("TERMINATED"));
            
        }

        public void Receive1()
        {
            ss1.BeginReceive(RCallback1, new Object());
            allDone.WaitOne();
        }

        public void Receive2()
        {
            ss2.BeginReceive(RCallback2, new Object());
            allDone.WaitOne();
        }


        public void RCallback1(String s, Exception e, object payload)
        {
            incoming_message_stream1.Add(s);
            allDone.Set();
            if(!s.Contains("STOP")){
            ss1.BeginReceive(RCallback1, new object());
            }
        }
        public void RCallback2(String s, Exception e, object payload)
        {
            incoming_message_stream2.Add(s);
            allDone.Set();
            if (!s.Contains("STOP"))
            {
                ss2.BeginReceive(RCallback2, new object());
            }
        }

        public void NewGameCallBack(String s, Exception e, object payload)
        {
            incoming_message = s;
            allDone.Set();
        }

        public void SendConnectMessage()
        {
            TcpClient client = new TcpClient("localhost", 2000);
            ss1 = new StringSocket(client.Client, new UTF8Encoding());
            ss1.BeginSend("PLAY Colin\n", (e, o) => { }, ss1);

            TcpClient client2 = new TcpClient("localhost", 2000);
            ss2 = new StringSocket(client2.Client, new UTF8Encoding());
            ss2.BeginSend("PLAY John\n", (e, o) => { }, ss2);
        }

        public void StartServer()
        {
            b = new BoggleServer(2000, 10, new HashSet<String>(){"AAAAAAAAAAAAAAA", "AAAA", "AAAAA", "BBB"}, "AAAAAAAAAAAAAAAA");
        }

        public void Stop()
        {
            b.Stop();
        }

    }
}
