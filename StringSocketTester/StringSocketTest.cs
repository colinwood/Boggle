using CustomNetworking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace StringSocketTester
{


    /// <summary>
    ///This is a test class for StringSocketTest and is intended
    ///to contain all StringSocketTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringSocketTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A simple test for BeginSend and BeginReceive
        ///</summary>
        [TestMethod()]
        public void Test1()
        {
            new Test1Class().run(4001);
        }

        public class Test1Class
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is a test\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }






        }

        /// <summary>
        ///A simple test for BeginSend and BeginReceive
        ///</summary>
        [TestMethod()]
        public void Test2()
        {
            new Test2Classb().run(4001);
        }

        public class Test2Classb
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "Hello world\n";

                    sendSocket.BeginSend(msg, (e, o) => { }, null);


                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");

                    Assert.AreEqual("Hello world", s1);

                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }
        }

        /// <summary>
        /// Curtis Clements and James Lundgren
        /// A modification of test1 in which we make 200 receive requests are sent, followed by 100 sends, with the 
        /// send being broken up with two different new line characters. 
        ///</summary>
        [TestMethod()]
        public void Test3()
        {
            new Test3Class().run(4001);
        }

        public class Test3Class
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests 100 times (200 total)

                    for (int i = 0; i < 100; i++)
                    {
                        receiveSocket.BeginReceive(CompletedReceive1, i);
                        receiveSocket.BeginReceive(CompletedReceive2, 100 + i);
                    }

                    // Now send the data 100 times


                    for (int i = 0; i < 100; i++)
                    {
                        String msg = "Hello world\nThis is a test\n";
                        foreach (char c in msg)
                        {
                            sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                        }
                    }

                    // Make sure the lines were received properly.

                    for (int i = 0; i < 100; i++)
                    {
                        Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                        Assert.AreEqual("Hello world", s1);
                        Assert.AreEqual(i, p1);
                        // Needed to add this in order for the mre to work correctly in the callback methods
                        mre1.Reset();

                        Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                        Assert.AreEqual("This is a test", s2);
                        Assert.AreEqual(100 + i, p2);
                        mre2.Reset();

                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }
        /// <summary>
        /// Authors: Greg Smith and Jase Bleazard
        /// Attempts sending the newline character by itself. The sockets should
        /// still send and receive a blank String, "".
        /// </summary>
        [TestMethod()]
        public void SendAndReceiveEmpty()
        {
            new SendAndReceiveEmptyClass().run(4006);
        }

        public class SendAndReceiveEmptyClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1;
            private object p1;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    // Now send the data.  Hope those receive requests didn't block!
                    sendSocket.BeginSend("\n", (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("", s1);

                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }
        }


        /// <summary>
        /// Run the original test case 100 times in a row, takes about a minute
        /// </summary>
        // [TestMethod()]
        public void Test1_Mod()
        {
            for (int i = 0; i < 100; i++)
                new Test1Class().run(4002);

        }

        /// <summary>
        /// Tests sending several long text strings and immediately closing the sending socket.
        /// According to the spec, all pending data should be sent (if possible) before closing the socket.
        /// Created by John Ballard and Maks Cegielski-Johnson for CS 3500.
        /// </summary>
        [TestMethod]
        public void SendThenClose()
        {
            new Test5Class().run(4005);
        }

        public class Test5Class
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);

                    // Now send the data and immediately close the socket.
                    String msg1 = "Bob Windle (born 1944) is a former Australian freestyle swimmer. He won the 1500 m freestyle and took bronze in the ";
                    String msg2 = "4 × 100 m freestyle relay at the 1964 Summer Olympics in Tokyo, and silver and bronze in the 4 × 200 m and 4 × 100 m ";
                    String msg3 = "freestyle relays respectively at the 1968 Summer Olympics. He is the only male swimmer to represent Australia at the ";
                    sendSocket.BeginSend(msg1 + "\n", (e, o) => { }, null);
                    sendSocket.BeginSend(msg2 + "\n", (e, o) => { }, null);
                    sendSocket.BeginSend(msg3 + "\n", (e, o) => { }, null);
                    sendSocket.Close();

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(msg1, s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(msg2, s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual(msg3, s3);
                    Assert.AreEqual(3, p3);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }
        }

        /// <author>Matthew Madden</author>
        /// <timecreated>11/11/14</timecreated>
        /// <summary>
        /// This method tests whether non-ASCII (multi-byte) characters are 
        /// passed through the String Socket intact, based on the encoding provided. 
        /// UTF-8 encoding can encode/decode any valid Unicode character.
        ///</summary>
        [TestMethod()]
        public void Test_non_ASCII()
        {
            new TestClass_non_ASCII().run(4100);
        }

        public class TestClass_non_ASCII
        {
            private ManualResetEvent mre1;
            private String msg;
            private object p1;
            StringSocket sendSocket, receiveSocket;

            // Timeout
            private static int timeout = 2000;

            public void run(int port)
            {
                TcpListener server = null;
                TcpClient client = null;


                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;
                    sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    mre1 = new ManualResetEvent(false);

                    receiveSocket.BeginReceive(CompletedReceive, 1);
                    sendSocket.BeginSend("Hêllø ?órld!\n", (e, o) => { }, null);

                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting");
                    // this will fail if the String Socket does not handle non-ASCII characters
                    Assert.AreEqual("Hêllø ?órld!", msg);
                    System.Diagnostics.Debug.WriteLine(msg);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    sendSocket.Close();
                    receiveSocket.Close();
                    server.Stop();
                    client.Close();
                }
            }

            //callback
            private void CompletedReceive(String s, Exception o, object payload)
            {
                msg = s;
                p1 = payload;
                mre1.Set();
            }
        }

        /// <author>Daniel James</author>
        /// <timecreated>11/08/14</timecreated>
        /// <summary>
        /// Tests to make sure that code in callbacks can not cause the StringSocket to get blocked.
        /// </summary>
        [TestMethod()]
        public void TestBlockingCallback()
        {
            // Declare these here so we can properally clean up.
            TcpListener server = null;
            TcpClient client = null;
            StringSocket sendSocket = null;
            StringSocket receiveSocket = null;

            // Test both receive callback and send callback separately.
            ManualResetEvent mreReceive = new ManualResetEvent(false);
            ManualResetEvent mreSend = new ManualResetEvent(false);

            // So we can unblock threads in finally.
            ManualResetEvent mreBlock = new ManualResetEvent(false);

            // Some constants used in the test case
            const int timeout = 2000;
            const int port = 8989;

            try
            {
                // Create server/client
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = new TcpClient("localhost", port);

                // Wrap the two ends of the connection into StringSockets
                sendSocket = new StringSocket(server.AcceptSocket(), new UTF8Encoding());
                receiveSocket = new StringSocket(client.Client, new UTF8Encoding());

                // Make two receive requests
                receiveSocket.BeginReceive((s, e, p) => mreBlock.WaitOne(), 1); // This one attempts to block StringSocket
                receiveSocket.BeginReceive((s, e, p) => mreReceive.Set(), 2); // This one allows assertion to pass. (Won't happen if StringSocket is blocked from the first request.)

                // Make two send requests.
                sendSocket.BeginSend("Don't let my code\n", (e, p) => mreBlock.WaitOne(), null); // This one attempts to block StringSocket
                sendSocket.BeginSend("block your code\n", (e, p) => mreSend.Set(), null); // This one allows assertion to pass. (Won't happen if StringSocket is blocked from the first request.)

                // Make sure the second requests were able to go through.
                Assert.AreEqual(true, mreSend.WaitOne(timeout), "Blocked by BeginSend callback.");
                Assert.AreEqual(true, mreReceive.WaitOne(timeout), "Blocked by BeginReceive callback.");
            }
            finally
            {
                // Cleanup
                mreBlock.Set();
                sendSocket.Close();
                receiveSocket.Close();
                server.Stop();
                client.Close();
            }
        }


        /// <summary>
        /// Tests to make sure that if Send is called before receive that the string will still be received, and not
        /// discarded. This can happen when the socket is loaded, but does not have any recipients for
        /// it's information.
        ///</summary>
        [TestMethod()]
        public void TestSendBeforeReceive()
        {
            new SendBeforeReceive().run(4001); //Run the test.
        }

        public class SendBeforeReceive
        {
            //Data used by the receiveSocket.
            private ManualResetEvent resetEvent;
            private String receivedString;
            private object receivedPayload;

            // Timeout used in test case
            private static int waitTime = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    //Initialize the connection.
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // Communicate between the threads of the test cases
                    resetEvent = new ManualResetEvent(false);

                    //Send the string of data to the socket before receive has been called
                    String msg = "This is a test, bro.\n";
                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    //Make a receive request after data has been read into the socket.
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    //Ensure that the data was received correctly.
                    Assert.AreEqual(true, resetEvent.WaitOne(waitTime), "Timed out waiting 1");
                    Assert.AreEqual("This is a test, bro.", receivedString);
                    Assert.AreEqual(1, receivedPayload);
                }
                finally
                {
                    //Stop the server, and discard the socket connection.
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                receivedString = s;
                receivedPayload = payload;
                resetEvent.Set();
            }
        }


        /// <author>Xiaobing Rawlinson and Sam Callister</author>
        /// <summary>
        /// Starts the test that will test sending and recieving 5 strings. The test is given
        /// 20 seconds to complete.
        ///</summary>
        [TestMethod()]
        public void MultiStringTest()
        {
            new StressTest().run(4000);
        }

        /// <summary>
        /// This tests sending and recieving 5 strings.
        /// </summary>
        public class StressTest
        {


            // Stores all received strings
            private HashSet<string> receiveStrings = new HashSet<string>();

            // Stores all send strings
            private HashSet<string> sendStrings = new HashSet<string>();

            // Stores all strings that where sent without the \n
            private HashSet<string> correctStrings = new HashSet<string>();

            // Size of strings being handled
            private int size = 5;

            // TIMEOUT USED IN TEST CASE, 20 SECONDS IS USED YOU MAY NEED MORE TIME
            private static int timeout = 20000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;


                try
                {
                    // Set up server and client
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    //mre1 = new ManualResetEvent(false);
                    //mre2 = new ManualResetEvent(false);

                    // Make 5 recieve requests
                    for (int i = 0; i < size; i++)
                    {

                        receiveSocket.BeginReceive(CompletedReceive1, i);
                    }
                    // Send 5 strings
                    for (int i = size; i > 0; i--)
                    {
                        sendStrings.Add(i + " bottles of beer on the wall.\n");
                        correctStrings.Add(i + " bottles of beer on the wall.");
                    }

                    // Send the strings
                    foreach (string s in sendStrings)
                    {
                        sendSocket.BeginSend(s, (e, o) => { }, null);
                    }
                    // Wait to give enough time for the call backs to return
                    Thread.Sleep(timeout);
                    // Ensure that each string was recieved
                    foreach (string s in correctStrings)
                    {
                        // Make sure the lines were received properly.   
                        Assert.IsTrue(receiveStrings.Contains(s));
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // Callback method that adds each recieved string to the recieveStrings HashSet.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                receiveStrings.Add(s);

            }
        }


        #region SendOrderTest

        /// <author> Conan Zhang and April Martin, modifying code provided by Professor de St. Germain</author>
        /// <date> 11-11-14</date>
        /// <summary>
        /// Tests whether threads are processed in the same order they are received, even if the first thread has a ludicrously long
        /// (and therefore slow) message and the second has a short one.
        /// </summary>


        [TestMethod()]
        public void SendOrderTest()
        {
            new SendOrderClass().run(4001);
        }

        /// <summary>
        /// Holds code for SendOrderTest
        /// </summary>
        public class SendOrderClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String slowMsg;
            private object p1;
            private String fastMsg;
            private object p2;

            private int count = 0;
            private int slowOrder;
            private int fastOrder;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // Set slowMsg to an absurdly long string that should take a while to process.
                    // Set fastMsg to a single character.
                    slowMsg = @"{{About|the video game character|other uses|Kirby (disambiguation){{!}}Kirby}}
                    {{Infobox VG character
                    | width = 220px
                    | name = Kirby
                    | image = [[File:Kirby Wii.png|225px]]
                    | caption = Kirby as he appears in ''[[Kirby's Return to Dream Land]]''
                    | series = [[Kirby (series)|''Kirby'' series]]
                    | firstgame = ''[[Kirby's Dream Land]]'' (1992)
                    | creator = [[Masahiro Sakurai]]
                    | artist = Masahiro Sakurai
                    | japanactor = [[Mayumi Tanaka]] (1994)<br>[[Makiko Ohmoto]] (1999-present)
                    }}
                    {{nihongo|'''Kirby'''|????|Kabi}} is a [[Character (arts)|fictional character]] and the protagonist of the 
                    ''[[Kirby (series)|Kirby series]]'' of video games owned by [[Nintendo]]. As one of Nintendo's most famous and familiar icons, 
                    Kirby's round, pink appearance and ability to copy his foe's powers to use as his own has made him a well known figure in video 
                    games, consistently ranked as one of the most iconic video game characters. He made his first appearance in 1992 in ''[[Kirby's 
                    Dream Land]]'' for the [[Game Boy]]. Originally a placeholder, created by [[Masahiro Sakurai]], for the game's early development, 
                    he has since then starred in over 20 games, ranging from [[Action game|action]] [[Platform game|platformers]] to [[Kirby's Pinball
                    Land|pinball]], [[Puzzle game|puzzle]] and [[Kirby Air Ride|racing]] games, and has been featured as a playable fighter in all 
                    ''[[Super Smash Bros.]]'' games. He has also starred in his own [[Kirby: Right Back at Ya|anime]] and manga series. His most 
                    recent appearance is in ''[[Super Smash Bros. for Nintendo 3DS and Wii U]]'', released in 2014 for the [[Nintendo 3DS]] and [[Wii 
                    U]]. Since 1999, he has been voiced by [[Makiko Ohmoto]].
                    Kirby is famous for his ability to inhale objects and creatures to obtain their attributes, as well as his ability to float with 
`                   puffed cheeks. He uses these abilities to rescue various lands, such as his home world of Dream Land, from evil forces and 
                    antagonists, such as [[Dark Matter (Kirby)|Dark Matter]] or [[Nightmare (Kirby)|Nightmare]]. On these adventures he often crosses 
                    paths with his rivals, the gluttonous [[King Dedede]] and the mysterious [[Meta Knight]]. In virtually all his appearances,
                    Kirby is depicted as cheerful, innocent, and food loving but becomes fearless, bold, and brave in the face of danger.
                    == Concept and creation ==";
                    fastMsg = "!";

                    // Send slowMsg before fastMsg
                    sendSocket.BeginSend(slowMsg, slowCallback, 1);
                    sendSocket.BeginSend(fastMsg, fastCallback, 2);

                    // Make sure that (a) neither thread timed out and
                    //(b) slowMsg was sent successfully before fastMsg
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(0, slowOrder);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(1, fastOrder);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            /// <summary>
            /// This is the callback for the first send request.  We can't make assertions anywhere
            /// but the main thread, so we write the values to member variables so they can be tested
            /// on the main thread.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="o"></param>
            /// <param name="payload"></param>
            private void slowCallback(Exception o, object payload)
            {
                slowOrder = count;
                count++;
                p1 = payload;
                mre1.Set();
            }

            /// <summary>
            /// This is the callback for the second send request.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="o"></param>
            /// <param name="payload"></param>
            private void fastCallback(Exception o, object payload)
            {
                fastOrder = count;
                count++;
                p2 = payload;
                mre2.Set();
            }
        }
        #endregion


        /// <summary>
        /// Authors: David Harrison & Peter Pan.
        /// This method tests for proper handling of null exceptions
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            Exception test = new Test2aClass().checkForNullPointException(4001);
            Assert.IsTrue(test is NullReferenceException || test is ArgumentNullException);
        }
        public class Test2aClass
        {

            private Exception e1;
            public Exception checkForNullPointException(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = new TcpClient("localhost", port);
                object payload = null;
                String s = null;

                // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                // method here, which is OK for a test case.
                Socket serverSocket = server.AcceptSocket();
                Socket clientSocket = client.Client;

                // Wrap the two ends of the connection into StringSockets
                StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                receiveSocket.BeginReceive(CompletedReceive1, 1);

                sendSocket.BeginSend(s, (e, o) => { }, payload);

                return e1;
            }
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                e1 = o;

            }

        }

        /// <summary>
        /// Created by Landen Andra, Parker Cluff
        /// Testing a large send will complete when close() is sent at the same time
        ///</summary>
        [TestMethod()]
        public void CompleteSendWithClos()
        {
            new CompleteSendWithClose().run(4001);
        }

        public class CompleteSendWithClose
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1;
            private object p1;


            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    //Create a long string for beginsend
                    string message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean leo orci, tristique vel suscipit at, " +
                    "convallis eu sem. Nam tristique fermentum augue. In mollis auctor dapibus. Donec interdum eget sem eget luctus. " +
                    "Praesent non mi vitae lacus ultricies efficitur. Vestibulum et magna pellentesque ante fermentum convallis. " +
                    "Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Praesent cursus, sem at rutrum vulputate, " +
                    "nunc augue faucibus turpis, non sollicitudin enim libero vel nibh. Vivamus venenatis at felis ut eleifend. Morbi rutrum, " +
                    "felis id vestibulum semper, nisi eros molestie felis, id auctor erat quam ut ligula. Praesent vulputate vestibulum orci, id consequat sem pretium sit amet.";
                    //Create a new thread to enusre that close gets called at the same time as BeginSend()
                    ThreadStart firstThread = new ThreadStart(() => sendSocket.BeginSend(message, (e, o) => { }, 1));
                    new Thread(firstThread).Start();
                    sendSocket.Close();

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(message, s1);
                    Assert.AreEqual(1, p1);

                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

        }



        /// <author>Kirk Partridge, Kameron Paulsen</author>
        /// <timecreated>11/12/14</timecreated>
        /// <summary>
        /// This method tests the StringSockets ability
        /// to Send Multiple strings before the BeginReceive
        /// is called.  It Sends both by single characters
        /// and full Strings.
        ///</summary>
        [TestMethod()]
        public void MultipleSendBeforeReceiveTest()
        {
            new MultipleSendBeforeReceive().run(4001);
        }
        public class MultipleSendBeforeReceive
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);



                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is a test\nStrings";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }
                    //Second Message to be sent
                    String msg2 = " sure are neat\n";
                    //Send the second message.  Should be appended to the leftovers from the foreach loop ("String").
                    sendSocket.BeginSend(msg2, (e, o) => { }, null);

                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("Strings sure are neat", s3);
                    Assert.AreEqual(3, p3);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
            // This is the callback for the third receive request.
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }
        }

        /// <author>Jake Anderson and Christopher Bowcutt</author>
        /// <summary>
        /// Test to check that StringSocket receives lines ending with \r instead of \n
        /// Note: This is modeled after the given Test1
        /// </summary>
        [TestMethod]
        public void CarriageReturnTest()
        {

            new TestCarriageReturnClass().run(4002);

        }

        /// <summary>
        /// Class used to run CarriageReturnText
        /// </summary>
        public class TestCarriageReturnClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    // NOTE: This is the line that has been changed
                    String msg = "Hello world\r\nThis is a test\r\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        /// <summary>
        /// <author>Albert Tom, Matthew Lemon</author>
        /// This test stress tests the socket on sending long strings all at once
        /// Quotes provided by Jedi Master and super spy Liam Neeson
        /// </summary>

        [TestMethod()]
        public void LongStringTest()
        {
            new Test1Classa().run(4001);
        }

        public class Test1Classa
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data. Stress test long strings
                    String msg = "I don't have money. But what I do have are a very particular set of skills acquired over a very long career in the shadows, skills that make me a nightmare for people like you. If you let my daughter go now, that will be the end of it. I will not look for you, I will not pursue you. But if you don't, I will look for you, I will find you. And I will kill you\nI don't have anything else. [waves hand] But credits will do fine.\n";

                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("I don't have money. But what I do have are a very particular set of skills acquired over a very long career in the shadows, skills that make me a nightmare for people like you. If you let my daughter go now, that will be the end of it. I will not look for you, I will not pursue you. But if you don't, I will look for you, I will find you. And I will kill you", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("I don't have anything else. [waves hand] But credits will do fine.", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        /// <summary>
        /// <author>Blake McGillis</author>
        /// <datecreated>November 12, 2014</datecreated>
        /// A simple test to ensure that the StringSocket's BeginSend method correctly triggers the user's
        /// sendCallback.
        /// </summary>
        [TestMethod()]
        public void test4()
        {
            new TestClass().run(4001);
        }

        public class TestClass
        {
            //Set up timer and payload variables
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private object p1;
            private object p2;

            // Set up the timeout
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Connect the server and client 
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the connection in StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    //Only BeginSend is called. BeginReceive should not need to be called for BeginSend to work
                    sendSocket.BeginSend("BeginSendTest1", CompletedSend1, 1);
                    sendSocket.BeginSend("BeginSendTest2", CompletedSend2, 2);

                    //Make sure the callbacks were called and the correct payload variables were assigned.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // Callback for the first BeginSend
            private void CompletedSend1(Exception o, object payload)
            {
                p1 = payload;
                mre1.Set();
            }

            // Callback for the second BeginSend
            private void CompletedSend2(Exception o, object payload)
            {
                p2 = payload;
                mre2.Set();
            }

        }


        ///<authors>Basil Vetas, Lance Petersen</authors> 
        ///<date>11/11/14</date>
        ///<summary>
        /// This test is only for the BeginSend() method and tests whether or not
        /// the callback is send after the message is comletely sent. This is done by keeping
        /// a counter variable that is incremented when the callback is called. So if your
        /// BeginSend() did not send a complete message and the callback does not get called,
        /// then the counter will not be incremented and the test will fail.
        /// 
        /// Currently the method only calls BeginSend() once, but if you change the messagesSent
        /// variable, it will increase the number of times we loop through and call BeginSend(), 
        /// and this should be equal to the number of times callback is called, and therefore
        /// equal to the number of times counter is incremented. In short, if you want to change
        /// messagesSend to numbers greater than 1, the test should still pass. 
        /// 
        /// </summary>
        public class Test2Classbc
        {
            // Data that is shared across threads
            private ManualResetEvent mre;

            // Used in Assert
            int counter = 0;

            // Used in Asser and in loop - if you change this the test should still pass
            int messagesSent = 1;

            public void run(int port)
            {

                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "Hello World\nThis is a Test\n";

                    // BeginSend() as many times as messagesSent
                    for (int i = 0; i < messagesSent; i++)
                        sendSocket.BeginSend(msg, Test2Callback, 1);

                    System.Threading.Thread.Sleep(500);
                    Assert.AreEqual(messagesSent, counter);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }

            }

            /// <summary>
            /// Callback increments the counter
            /// </summary>
            /// <param name="s"></param>
            /// <param name="o"></param>
            /// <param name="payload"></param>
            private void Test2Callback(Exception o, object payload)
            {
                counter++;
            }
        }




        //Jake Pitkin, Kevin Glanville

        /// <summary>
        ///Test sending and receiving a long string
        ///</summary>
        [TestMethod()]
        public void TestLongString()
        {
            new TestLongStringClass().run(4001);
        }

        public class TestLongStringClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1;
            private object p1;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);


                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    //Build a large string
                    StringBuilder msg = new StringBuilder();

                    for (int i = 0; i <= 10000; i++)
                    {
                        msg.Append(i);
                    }
                    msg.Append("\n");

                    sendSocket.BeginSend(msg.ToString(), (e, o) => { }, null);

                    // Make sure the long string made it through
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(msg.ToString(), s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }
        }




        /// <summary>
        /// Author: Ryan Farr
        /// A simple test to make sure Close() works
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void TestCloseBasic()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 4006);
            server.Start();
            TcpClient client = new TcpClient("localhost", 4006);

            Socket serverSocket = server.AcceptSocket();
            Socket clientSocket = client.Client;

            StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
            StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

            sendSocket.Close();
            receiveSocket.Close();

            bool test1 = serverSocket.Available == 0; //Should fail here because socket should be shutdown and closed
        }

        /// <summary>
        /// Authors: James Yeates, Tyler Down
        /// 
        /// This test send the letters "a" through "h" one at a time, appending the 
        /// characters with their associated payload into a resulting string. 
        /// A timeout is used to ensure the messages are processed in order.
        /// (This timeout may need to be increased.)
        ///</summary>
        ///
        [TestMethod()]
        public void AlphabetPayloadTest()
        {
            new AlphabetPayloadTestClass().run(4015);
        }

        public class AlphabetPayloadTestClass
        {
            // String to hold the result.
            private String result;

            // Timeout used in test case (THIS MAY NEED TO BE INCREASED)
            private static int timeout = 200;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());


                    // Make the receive requests.
                    int payload = 0;
                    for (int i = 0; i < 9; i++)
                        receiveSocket.BeginReceive(CompletedReceive1, payload++);

                    // The message to be sent.
                    String msg = "a\nb\nc\nd\ne\nf\ng\nh\n\n";

                    // Send the message character by character.
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);

                        // Sleep between send requests to allow each message to be sent in order.
                        Thread.Sleep(timeout);
                    }

                    // Check the result.
                    Assert.AreEqual("a0b1c2d3e4f5g6h78", result);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the receive requests.
            // The received message and its payload are appended to the end of the resulting string.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                result += s + payload;
            }
        }


        /// <summary>
        /// @Author Eric Albee
        /// @Author Douglas Canada
        /// Testing foreign and odd Chars 
        /// in large quanity to see if the 
        /// socket is recieving the correct sequence.
        /// November 12, 2014
        /// </summary>
        [TestMethod()]
        public void beeCanForeignTest()
        {
            new ForeignTest().run(4001);
        }

        public class ForeignTest
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1 = "";
            private object p1;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Make a receive request
                    receiveSocket.BeginReceive(CompletedReceive, 1);

                    String msg = "";
                    int SIZE = 1000;
                    string[] letters = new string[SIZE];
                    for (int i = 0; i < SIZE; i++)
                    {
                        letters[i] = (msg + (char)('a' + i));
                    }

                    string sentMessage = "";
                    for (int i = 0; i < SIZE; i++)
                    {
                        sendSocket.BeginSend(letters[i], (e, o) => { }, null);

                        // building the message to be asserted
                        sentMessage += letters[i];
                    }

                    sendSocket.BeginSend("\n", (e, o) => { }, null);

                    // Make sure the line was received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(sentMessage, s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive(String s, Exception o, object payload)
            {
                s1 += s;
                p1 = payload;
                mre1.Set();
            }
        }


        /// <summary>
        /// Authors: Ryan Welling and Jared Jensen
        /// 
        /// This method opens sockets, begins sending and then closes them
        /// and tries sending again after it has been closed.  Expects Exception
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Ryan()
        {
            // Create and start a server and client.
            TcpListener server = null;
            TcpClient client = null;

            server = new TcpListener(IPAddress.Any, 4042);
            server.Start();
            client = new TcpClient("localhost", 4042);

            // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
            // method here, which is OK for a test case.
            Socket serverSocket = server.AcceptSocket();
            Socket clientSocket = client.Client;

            // Wrap the two ends of the connection into StringSockets
            StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
            StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

            // This will coordinate communication between the threads of the test cases
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);

            // Make two receive requests
            receiveSocket.BeginReceive((s, e, o) => { }, 1);
            receiveSocket.BeginReceive((s, e, o) => { }, 2);

            // Now send the data.  
            string msg1 = "Hello world\n";
            string msg2 = "This is a test\n";

            sendSocket.BeginSend(msg1, (e, o) => { }, null);
            sendSocket.Close();

            // should throw exception, socket was closed
            sendSocket.BeginSend(msg2, (e, o) => { }, null);
        }


        /// <summary>
        /// Authors: Clint Wilkinson & Daniel Kenner
        /// 
        /// Class for Stress Test, based off of Test1Class given as part of PS7.
        /// 
        ///This is a test class for StringSocketTest and is intended
        ///to contain all StringSocketTest Unit Tests
        ///</summary>
        [TestClass()]
        public class StringSocketStressTest
        {
            /// <summary>
            /// A stress test for BeginSend and BeginReceive
            /// </summary>
            [TestMethod()]
            public void StressTest()
            {
                new StressTestClass().run(4001);
            }

            /// <summary>
            /// Class for Stress Test, based off of Test1Class given as part of PS7.
            /// </summary>
            public class StressTestClass
            {
                // Data that is shared across threads
                private ManualResetEvent mre1;
                private ManualResetEvent mre2;
                private String s1;
                private object p1;
                private String s2;
                private object p2;

                // Timeout used in test case
                private static int timeout = 2000;

                public void run(int port)
                {
                    // Create and start a server and client.
                    TcpListener server = null;
                    TcpClient client = null;

                    try
                    {
                        //setup the server
                        server = new TcpListener(IPAddress.Any, port);
                        server.Start();
                        client = new TcpClient("localhost", port);

                        // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                        // method here, which is OK for a test case.
                        Socket serverSocket = server.AcceptSocket();
                        Socket clientSocket = client.Client;

                        // Wrap the two ends of the connection into StringSockets
                        StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                        StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                        // This will coordinate communication between the threads of the test cases
                        mre1 = new ManualResetEvent(false);
                        mre2 = new ManualResetEvent(false);

                        //test a bunch of little strings
                        for (int i = 0; i <= 25000; i++)
                        {
                            //setup the receive socket
                            receiveSocket.BeginReceive(CompletedReceive1, 1);
                            //generate the string
                            sendSocket.BeginSend("A" + i + "\n", (e, o) => { }, null);
                            //wait a bit
                            Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                            //reset the timer
                            mre1.Reset();
                            //check that we are getting what we are supposed to
                            Assert.AreEqual("A" + i, s1);
                            //write out for debugging.
                            System.Diagnostics.Debug.WriteLine(s1);

                        }

                        //generate a big string to test with
                        String stress = "";
                        Random rand = new Random();
                        //put in character by character
                        for (int i = 0; i <= 25000; i++)
                        {
                            stress += ((char)(65 + rand.Next(26))).ToString();
                        }

                        //setup the receiver socket
                        receiveSocket.BeginReceive(CompletedReceive2, 2);
                        //send the big string
                        sendSocket.BeginSend(stress + "\n", (e, o) => { }, null);

                        System.Diagnostics.Debug.WriteLine(stress);

                        // Now send the data.  Hope those receive requests didn't block!
                        Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                        Assert.AreEqual(stress, s2);
                        Assert.AreEqual(2, p2);
                        //separate from last test
                        System.Diagnostics.Debug.WriteLine("");
                        //write for debugging
                        System.Diagnostics.Debug.WriteLine(s2);

                    }
                    finally
                    {
                        server.Stop();
                        client.Close();
                    }
                }
                // This is the callback for the first receive request.  We can't make assertions anywhere
                // but the main thread, so we write the values to member variables so they can be tested
                // on the main thread.
                private void CompletedReceive1(String s, Exception o, object payload)
                {
                    s1 = s;
                    p1 = payload;
                    mre1.Set();
                }
                // This is the callback for the second receive request.
                private void CompletedReceive2(String s, Exception o, object payload)
                {
                    s2 = s;
                    p2 = payload;
                    mre2.Set();
                }
            }
        }


        /// <summary>
        /// Written by: Kyle Hiroyasu and Drake Bennion
        /// This test is designed to ensure that string sockets will properly wait for strings to be sent and received
        /// The last send also ensures that a message is broken up by newline character but maintains same payload
        /// </summary>
        [TestMethod()]
        public void MessageOrderStressTest()
        {
            int Port = 4000;
            int timeout = 30000;
            TcpListener server = null;
            TcpClient client = null;


            try
            {
                server = new TcpListener(IPAddress.Any, Port);
                server.Start();
                client = new TcpClient("localhost", Port);
                Socket serverSocket = server.AcceptSocket();
                Socket clientSocket = client.Client;

                StringSocket send = new StringSocket(serverSocket, Encoding.UTF8);
                StringSocket receive = new StringSocket(clientSocket, Encoding.UTF8);

                //Messages
                string message1 = "The sky is blue\n";
                string message2 = "The grass is green\n";
                string message3 = "Drakes hat is blue\n";
                string message4 = (new String('h', 1000)) + message1 + message2 + message3;
                string message4s = (new String('h', 1000)) + message1;

                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message1, message);
                    Assert.AreEqual(1, o);
                }, 1);

                send.BeginSend(message1, (e, o) => { }, 1);

                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message2, message);
                    Assert.AreEqual(2, o);
                }, 1);

                send.BeginSend(message2, (e, o) => { }, 2);
                send.BeginSend(message3, (e, o) => { }, 3);
                send.BeginSend(message4, (e, o) => { }, 4);

                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message3, message);
                    Assert.AreEqual(3, o);
                }, 1);

                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message4s, message);
                    Assert.AreEqual(4, o);
                }, 1);
                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message2, message);
                    Assert.AreEqual(4, o);
                }, 1);
                receive.BeginReceive((message, e, o) =>
                {
                    Assert.AreEqual(message3, message);
                    Assert.AreEqual(4, o);
                }, 1);

            }
            finally
            {
                server.Stop();
                client.Close();
            }
        }


        /// <summary>
        /// Written by Ella Ortega and Jack Stafford for CS 3500, Fall 2014
        /// Ensures data was transmitted in the correct order.
        /// Sends a sentence with newlines instead of spaces.
        /// This enables seven receives.
        /// However, only three receives are called.
        /// These receives are checked for accuracy.
        /// </summary>
        [TestMethod]
        public void TestTransmissionOrderTest()
        {
            new TestLongStringSmallReturn().run(4001);
        }

        /// <summary>
        /// Called by TestMethod TestLongStringSmallReturn()
        /// </summary>
        public class TestLongStringSmallReturn
        {
            /// <summary>
            /// This method instantiates necessary object and calls BeginSend and BeginReceive
            /// </summary>
            /// <param name="port"></param>
            public void run(int port)
            {
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will be received as seven separate messages, even though it's sent all together
                    sendSocket.BeginSend("Plateaus\nare\nthe\nhighest\nform\nof\nflattery.\n", Callback1, 1);

                    receiveSocket.BeginReceive(Callback2, 2);
                    receiveSocket.BeginReceive(Callback3, 3);
                    receiveSocket.BeginReceive(Callback4, 4);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            /// <summary>
            /// Ensures no exceptions occured during sending and payload was returned correctly.
            /// </summary>
            /// <param name="e">Returned exception</param>
            /// <param name="payload"></param>
            private void Callback1(Exception e, object payload)
            {
                Assert.AreEqual(null, e);
                Assert.AreEqual(1, (int)payload);
            }

            /// <summary>
            /// Ensures no exceptions occured during receiving, payload was returned correctly, 
            /// and the correct message was received.
            /// </summary>
            /// <param name="message"></param>
            /// <param name="e"></param>
            /// <param name="payload"></param>
            private void Callback2(String message, Exception e, object payload)
            {
                Assert.AreEqual("Plateaus", message);
                Assert.AreEqual(null, e);
                Assert.AreEqual(2, (int)payload);
            }

            /// <summary>
            /// Ensures no exceptions occured during receiving, payload was returned correctly, 
            /// and the correct message was received.
            /// </summary>
            /// <param name="message"></param>
            /// <param name="e"></param>
            /// <param name="payload"></param>
            private void Callback3(String message, Exception e, object payload)
            {
                Assert.AreEqual("are", message);
                Assert.AreEqual(null, e);
                Assert.AreEqual(3, (int)payload);
            }

            /// <summary>
            /// Ensures no exceptions occured during receiving, payload was returned correctly, 
            /// and the correct message was received.
            /// </summary>
            /// <param name="message"></param>
            /// <param name="e"></param>
            /// <param name="payload"></param>
            private void Callback4(String message, Exception e, object payload)
            {
                Assert.AreEqual("the", message);
                Assert.AreEqual(null, e);
                Assert.AreEqual(4, (int)payload);
            }
        }

        /// <summary>

        /// Namgi Yoon u0759547

        /// A simple test for BeginSend and BeginReceive

        ///</summary>

        [TestMethod()]

        public void Namgi()
        {

            new StringSocketTester1().run(4001);

        }

        /// <summary>

        /// Class used for test1

        /// </summary>

        public class StringSocketTester1
        {

            // Data that is shared across threads

            private ManualResetEvent mre1, mre2, mre3;

            private String string1, string2, string3;

            private object payload1, payload2, payload3;



            // Timeout used in test case

            private static int timeout = 2000;



            public void run(int port)
            {

                // Create and start a server and client.

                TcpListener server = null;

                TcpClient client = null;

                try
                {

                    server = new TcpListener(IPAddress.Any, port);

                    server.Start();

                    client = new TcpClient("localhost", port);



                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()

                    // method here, which is OK for a test case.

                    Socket serverSocket = server.AcceptSocket();

                    Socket clientSocket = client.Client;



                    // Wrap the two ends of the connection into StringSockets

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());



                    // This will coordinate communication between the threads of the test cases

                    mre1 = new ManualResetEvent(false);

                    mre2 = new ManualResetEvent(false);

                    mre3 = new ManualResetEvent(false);



                    // Make two receive requests

                    receiveSocket.BeginReceive(CompletedReceive1, "payload for message 1");

                    receiveSocket.BeginReceive(CompletedReceive2, "payload for message 2");

                    receiveSocket.BeginReceive(CompletedReceive3, "payload for message 3");



                    // Now send the data.  Hope those receive requests didn't block!

                    String msg = "1\n2\n3\n";

                    foreach (char c in msg)
                    {

                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);

                    }



                    //Whole message at once.

                    //sendSocket.BeginSend(msg, (e, o) => { }, null);



                    //Checking message number 1

                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");

                    Assert.AreEqual("1", string1);

                    Assert.AreEqual("payload for message 1", payload1);



                    //Checking message number 2

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");

                    Assert.AreEqual("2", string2);

                    Assert.AreEqual("payload for message 2", payload2);



                    //Checking message number 3

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");

                    Assert.AreEqual("3", string3);

                    Assert.AreEqual("payload for message 3", payload3);





                }

                finally
                {

                    server.Stop();

                    client.Close();

                }

            }

            // This is the callbacks for requests.

            private void CompletedReceive1(String s, Exception o, object payload) { string1 = s; payload1 = payload; mre1.Set(); }

            private void CompletedReceive2(String s, Exception o, object payload) { string2 = s; payload2 = payload; mre2.Set(); }

            private void CompletedReceive3(String s, Exception o, object payload) { string3 = s; payload3 = payload; mre3.Set(); }

        }


        // Created by: Sam Trout and Sam England
        /// <summary>
        /// Test case checks whether or not the callback method is sent on its own threadpool. Fails if it times out because 
        /// the thread is blocked.
        /// </summary>
        [TestMethod()]
        public void BeginSendSeperateThread()
        {
            new BeginSendSeperateClass().run(4001);
        }

        public class BeginSendSeperateClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre = new ManualResetEvent(false);

            // Timeout used in test case
            private static int timeout = 20000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                    // Now send the data. Will block after newline if callback doesnt come back in its own threadpool
                    String msg = "Hopefully this works\n";
                    String msg2 = "Second message\n";

                    //calls beginsend 2 times for the different messages
                    sendSocket.BeginSend(msg, callback1, 1);
                    sendSocket.BeginSend(msg2, callback2, 2);

                    Assert.AreEqual(true, mre.WaitOne(timeout), "Timed out, callback1 blocked second BeginSend");
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            /// <summary>
            /// This callback creates an infinite while loop and if not handled properly in StringSocket will cause the program
            /// to timeout and fail
            /// </summary>
            /// <param name="e"> Default </param>
            /// <param name="payload"> Default </param>
            private void callback1(Exception e, object payload)
            {
                while (true) ;
            }

            /// <summary>
            /// This callback is for the 2nd string, this callback wont be called unless handled properly in the StringSocket
            /// mre will never be set and the program will timeout
            /// </summary>
            /// <param name="e"> Default </param>
            /// <param name="payload"> Default</param>
            private void callback2(Exception e, object payload)
            {
                mre.Set();
            }
        }


        /// <summary>
        ///James Watts & Stuart Johnsen
        ///
        ///Tests sending a single long String that contains 4 lines, seperated by "\n". The string is the lyrics
        ///to the chorus of Haddaway's "What is Love?" Lines are placed in the correct order using a sequential 
        ///integer from the callback's payload.
        ///</summary>
        [TestMethod()]
        public void What_Is_Love_Test()
        {
            new What_Is_Love_TestClass().run(4005);
        }

        public class What_Is_Love_TestClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre0;
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;

            //The String to be sent
            String whatIsLove = "What is love?\nBaby don't hurt me,\nDon't hurt me\nNo more!\n";

            //A String[] for the received lines of text. Should contain 4 elements when completed.
            String[] receivedLines = new String[4];

            // Timeout used in test case
            //private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre0 = new ManualResetEvent(false);
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);

                    //Setup 4 BeginReceives to receive the 4 lines.
                    for (int i = 0; i < 4; i++)
                    {
                        receiveSocket.BeginReceive(WhatIsLove_Callback, i);
                    }

                    sendSocket.BeginSend(whatIsLove, (e, o) => { }, null);

                    Thread.Sleep(5000);

                    Assert.AreEqual("What is love?", receivedLines[0]);
                    Assert.AreEqual("Baby don't hurt me,", receivedLines[1]);
                    Assert.AreEqual("Don't hurt me", receivedLines[2]);
                    Assert.AreEqual("No more!", receivedLines[3]);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            } //End run


            /// <summary>
            /// The callback for receive requests, uses the callback's payload to place lines in the correct order
            ///in a String array.
            ///The appropriate ManualResetEvent is chosen based on the callback's payload.
            /// </summary>
            private void WhatIsLove_Callback(String s, Exception o, object payload)
            {
                int index = (int)payload;
                receivedLines[index] = s;

                switch (index)
                {
                    case 0:
                        mre0.Set();
                        break;
                    case 1:
                        mre1.Set();
                        break;
                    case 2:
                        mre2.Set();
                        break;
                    case 3:
                        mre3.Set();
                        break;
                }
            }
        }

        /// <summary>
        /// Test that a long string, then a short string, and then a long one are correctly received in in the right sent order.
        /// Written by Zane Zakraisek and Alex Ferro
        /// </summary>
        [TestMethod()]
        public void TestLongShortLongStringRX()
        {
            new TestLongShortLongStringRXClass().run(4002);
        }
        /// <summary>
        /// This is the test class for TestLongShortLongStringRX
        /// Test that a long string, then a short string, and then a long one are correctly received in in the right sent order.
        /// Written by Zane Zakraisek and Alex Ferro
        /// </summary>
        public class TestLongShortLongStringRXClass
        {
            // Data that is shared across threads
            // Used to ensure the correct testing assertion on the main thread
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;
            // Test strings
            private String shortString = "This is a journey through time!\n";
            private String longString = "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single" +
            "domestic sufficed to serve him. He breakfasted and dined at the club" +
            "or near friends, which is certainly more unusual. He lived alone" +
            "in his house in Saville Row, whither none penetrated. A single\n";

            // Timeout used in test case
            private static int timeout = 2000;
            /// <summary>
            /// Run the test on the specified port
            /// </summary>
            /// <param name="port"></param>
            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);

                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);

                    sendSocket.BeginSend(longString, (e, o) => { }, null);
                    sendSocket.BeginSend(shortString, (e, o) => { }, null);
                    sendSocket.BeginSend(longString, (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(longString.Replace("\n", ""), s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(shortString.Replace("\n", ""), s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual(longString.Replace("\n", ""), s3);
                    Assert.AreEqual(3, p3);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            /// <summary>
            /// This is the callback for the first receive request. We can't make assertions anywhere
            /// but the main thread, so we write the values to member variables so they can be tested
            /// on the main thread.
            /// </summary>
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }
            /// <summary>
            /// This is the callback for the second receive request.
            /// </summary>
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
            /// <summary>
            /// This is the callback for the third receive request.
            /// </summary>
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }
        }

        // File: PS7ForumTestCase.cs
        // Authors: Eric Stubbs, CJ Dimaano
        // CS 3500 - Fall 2014
        /// <summary>
        /// Authors: CJ Dimaano and Eric Stubbs
        /// Date: November 12, 2014
        /// </summary>
        [TestClass]
        public class PS7ForumTestCase
        {
            /// <summary>
            /// This tests that a small string with no \n characters and a large 
            /// string with many newline characters will be received in the 
            /// correct order and with their correct payload. 
            /// </summary>
            [TestMethod]
            public void TestMethod1()
            {
                new PS7TestCase().run(4000);
            }

            public class PS7TestCase
            {
                // One short string with no '\n' characters and one long string with many.
                private string reallyLongMessagePart1 = "Lorem ipsum dolor sit amet,";
                private string reallyLongMessagePart2 = " consectetur adipiscing \nelit. Aliquam lacinia eros quis odio convallis, sit amet suscipit quam\n dapibus. Praesent at arcu lacus. Donec eget iaculis felis. Curabitur\n vestibulum molestie volutpat. Donec imperdiet odio a lectus imperdiet, non pulvinar nulla aliquet. Quisque luctus dui elit\n, non accumsan ante interdum et. Phasellus turpis magna, iaculis nec fermentum eget, imperdiet et metus.\n Integer id fermentum elit. Nullam lacinia nisl et purus eleifend, eget imperdiet magna blandit. Sed dignissim pellentesque tortor. Pellentesque vitae consectetur quam. Etiam consectetur ornare laoreet. Pellentesque auctor ac eros et pulvinar. \nNunc dapibus, libero nec \nscelerisque lacinia, magna sapien malesuada ligula, eu tristique ju\nsto risus nec neque. Quisque tincidunt arcu non purus posuere luctus. Suspendisse id lectus in est luctus pellentesque a non mauris. Phasellus ornare mauris ut justo elementum facilisis. Proin sagittis egestas est ac luctus. Donec aliquet gr\navida velit, sollicitudin sagittis augue aliquet et. Pellentesque venenatis accumsan mi quis dictum. Aliquam eu mauris pharetra, volutpat urna at, accumsan enim. Interdum et malesuada fames ac ante ipsum p\nrimis in faucibus. Sed elit turpis, hendrerit dapibus sollicitudin porttitor, convallis et eros. Mauris lacus mi, mollis eget lacinia quis, condimentum eu ex. Suspendisse mollis sit amet neque a congue. Vestibulum scelerisque hendrerit felis sit amet fringilla. \nVestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis tincidunt maximus eros scelerisque volutpat. Quisque scelerisque neque eget dictum fermentum\n. Curabitur eget bibendum arcu. Fusce tempus, nisl vel malesuada commodo, enim lorem sollicitudin libero, ac condimentum tortor nisl sed elit. Nunc est purus, cursus sed justo ut,\n scelerisque posuere nibh. Nulla condimentum tincidunt mauris, sodales dictum ante cursus nec. \nAenean purus nisl, ultricies quis scelerisque nec, \niaculis nec nisl. Fusce eu magna augue. In arcu sem, accumsan vitae ligula ut, porta commodo sapien. Phasellus vel pellentesque risus. Proin in arcu leo. Donec sed dolor et arcu bibendum vehicula sed vel felis. Sed in lacinia diam. Etiam ac elit id ipsum mattis bibendum. \nSuspendisse tempor, elit quis efficitur suscipit, erat erat pellentesque sem, eget elementum nibh tortor vel \nnibh. Curabitur semper lacus non nibh aliquet tincidunt. Praesent porttitor pretium ullamcorper. Fusce consequat ex vitae elit euismod mollis. Cras at ipsum in nisl aliquet euismod ac non\n elit. Integer vitae diam auctor, pellentesque nulla et, vulputate quam. In dictum feugiat blandit. Quisque et tristique sem. Sed egestas ultricies nibh sed auctor. Cras ullamcorper quam sit\n amet lectus fermentum, non feugiat leo molestie. Curabitur imperdiet turpis nec eros pulvinar, eget laoreet tell\nus blandit. Proin sagittis quam sed massa \nluctus sollicitudin. Nulla et eros an\nte.\n";

                // Array to hold the messages sent.
                private string[] messageParts;

                // Array to hold the messages received.
                private string[] messagePartsReceived;

                // An int that keeps track of how many receive callbacks have returned.
                private int receiveCount = 0;

                public void run(int port)
                {
                    TcpListener server = null;
                    TcpClient client = null;

                    // Store each message in a different array bucket.
                    string wholeMessage = reallyLongMessagePart1 + reallyLongMessagePart2;
                    this.messageParts = wholeMessage.Split('\n');
                    messagePartsReceived = new string[messageParts.Length];

                    try
                    {
                        // Run the server and the client.
                        server = new TcpListener(IPAddress.Any, port);
                        server.Start();
                        client = new TcpClient("localhost", port);

                        // Create the sockets for the client and server.
                        Socket serverSocket = server.AcceptSocket();
                        Socket clientSocket = client.Client;

                        // Create StringSockets for the client and server. 
                        StringSocket serverStringSocket = new StringSocket(serverSocket, new UTF8Encoding());
                        StringSocket clientStringSocket = new StringSocket(clientSocket, new UTF8Encoding());

                        // Call a beginReceive for each message (each time there is a \n).
                        for (int i = 0; i < this.messageParts.Length - 1; i++)
                            clientStringSocket.BeginReceive(MessageReceived, i);

                        // Send the two parts of the message.
                        serverStringSocket.BeginSend(reallyLongMessagePart1, (e, o) => { }, null);
                        serverStringSocket.BeginSend(reallyLongMessagePart2, (e, o) => { }, null);

                        // Wait until all the messages have been received back (all the callbacks have been called).
                        while (this.receiveCount != this.messageParts.Length - 1) ;

                        lock (this.messagePartsReceived)
                        {
                            // The array index relates to the payload, so the messages that were sent 
                            // should be the same as the messages received back.
                            for (int i = 0; i < this.messageParts.Length - 1; i++)
                                Assert.AreEqual(this.messageParts[i], this.messagePartsReceived[i]);
                        }
                    }
                    finally
                    {
                        server.Stop();
                        client.Close();
                    }
                }

                /// <summary>
                /// The callback for beginReceive.
                /// </summary>
                private void MessageReceived(String s, Exception e, object payload)
                {
                    lock (this.messagePartsReceived)
                    {
                        // Keep track of each message received
                        this.receiveCount++;

                        // Save the message that was received to the array slot corresponding to its payload number.
                        int i = (int)payload;
                        messagePartsReceived[i] = s;
                    }
                }
            }
        }


        /// <summary>
        /// Jonathan Kraiss and Arash Tadjiki
        /// Tests the functionality of beginReceive when it receieves a message that is not terminated by a newline.
        /// The expected functionality is for the method to preserve the unfinished string, collect the rest of the message,
        /// and return the sum of the partial messages as a single string.
        /// 
        /// Note: The message is known to have ended when the request receives a newline character
        ///</summary>

        [TestMethod()]

        public void partialSendsTest()
        {
            new PartialSendsClass().run(4002);
        }

        public class PartialSendsClass
        {
            // Storage for the result of a two sends combining into a single message
            private string receivedMessage = "";

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                // Make a connection that can ultimately be wrapped into string sockets
                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Retrieve the sockets used in the connection.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the pre-connected sockets into string sockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    /* ---------------------- Perform the test -----------------------
                    * This test will send part of a message to a socket requesting a send.
                    * It will then wait and send the next half terminated by a newline character.
                    * When the send is entirely complete, the message received should be the combination of the two sends. 
                    */

                    // Prepare the socket to recieve partial sends.
                    receiveSocket.BeginReceive(formReceivedMessage, 1);

                    // Send the first of a message (without a terminating newline symbol).
                    sendSocket.BeginSend("First half of the message followed by ", (e, o) => { }, 1);

                    // Wait to preserve the order of the sends and to simulate breaks in the sending of a message
                    Thread.Sleep(1000);

                    // Using the same callback as the first send, send the second half of the message (with the terminating newline symbol).
                    sendSocket.BeginSend("the second half of the message.\n", (e, o) => { }, 1);

                    // Make sure the string is the sum of the partial sends
                    Assert.AreEqual("First half of the message followed by the second half of the message.\n", receivedMessage);

                }

                finally
                {
                    server.Stop();
                    client.Close();
                }

            }

            /// <summary>
            /// A simple receiveCallback method for tests to use.
            /// This callback appends the string received to a member variable to be accessed later.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="e"></param>
            /// <param name="payload"></param>
            private void formReceivedMessage(string s, Exception e, object payload)
            {
                receivedMessage = s;
            }
        }
        /// <summary>
        ///This is a test class for StringSocketTest and it tests a long string
        /// (the complete lyrics to Let it Go) followed by a short string (#frozen4lyfe)
        /// As you can see, our title relates to Frozen, too. We're still not over it.
        /// Also, we did this in UTF7 encoding because we should test for that.
        ///</summary>
        ///<authors>
        ///Courtney Burness & Pierce Darragh
        ///</authors>
        //[TestClass()]

        private TestContext testContextInstance2;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextCourtney
        {
            get
            {
                return testContextInstance2;
            }
            set
            {
                testContextInstance2 = value;
            }
        }

        /// <summary>
        ///A simple test for BeginSend and BeginReceive
        ///</summary>
        [TestMethod()]
        public void Test2Courtney()
        {
            new Test2Class().run(4001);
        }

        public class Test2Class
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF7Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF7Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "The snow glows white on the mountain tonight Not a footprint to be seen A kingdom of isolation, And it looks like I'm the queen. The wind is howling like this swirling storm inside Couldn't keep it in, heaven knows I tried! Don't let them in, don't let them see Be the good girl you always have to be Conceal, don't feel, don't let them know Well, now they know! Let it go, let it go Can't hold it back anymore Let it go, let it go Turn away and slam the door! I don't care What they're going to say Let the storm rage on, The cold never bothered me anyway! It's funny how some distance Makes everything seem small And the fears that once controlled me Can't get to me at all! It's time to see what I can do To test the limits and break through No right, no wrong, no rules for me I'm free! Let it go, let it go I am one with the wind and sky Let it go, let it go You'll never see me cry! Here I stand And here I'll stay  Let the storm rage on! My power flurries through the air into the ground  My soul is spiraling in frozen fractals all around And one thought crystallizes like an icy blast I'm never going back, The past is in the past! Let it go, let it go And I'll rise like the break of dawn Let it go, let it go That perfect girl is gone! Here I stand In the light of day Let the storm rage on, The cold never bothered me anyway! \n#frozen4lyfe\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(msg, (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("The snow glows white on the mountain tonight Not a footprint to be seen A kingdom of isolation, And it looks like I'm the queen. The wind is howling like this swirling storm inside Couldn't keep it in, heaven knows I tried! Don't let them in, don't let them see Be the good girl you always have to be Conceal, don't feel, don't let them know Well, now they know! Let it go, let it go Can't hold it back anymore Let it go, let it go Turn away and slam the door! I don't care What they're going to say Let the storm rage on, The cold never bothered me anyway! It's funny how some distance Makes everything seem small And the fears that once controlled me Can't get to me at all! It's time to see what I can do To test the limits and break through No right, no wrong, no rules for me I'm free! Let it go, let it go I am one with the wind and sky Let it go, let it go You'll never see me cry! Here I stand And here I'll stay  Let the storm rage on! My power flurries through the air into the ground  My soul is spiraling in frozen fractals all around And one thought crystallizes like an icy blast I'm never going back, The past is in the past! Let it go, let it go And I'll rise like the break of dawn Let it go, let it go That perfect girl is gone! Here I stand In the light of day Let the storm rage on, The cold never bothered me anyway! ", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("#frozen4lyfe", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }





        /// <summary>
        /// A test that makes sure data is being stored internally to the
        /// StringSocket when there are no pending receive requests yet. 
        /// Also, that once two receive requests are made, that those requests
        /// grab only two strings from the beginning of the stored data and
        /// that the remaining stored data does not interfere and overwrite it.
        ///</summary>
        [TestMethod()]
        public void Test2unknown()
        {
            new Test2Classa().run(4001);
        }

        public class Test2Classa
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    // Build and start the server.
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Construct the StringSockets with the already connected underlying sockets.
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Send the data in the specified string. There are not yet any receive requests on the 
                    // receiving socket. The data should be stored internally to the receiving string socket
                    // and processed after a receive request is made.
                    String msg = "Space\n is disease and \ndanger wrap\nped in darkn\ness and silence\n.\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make two receive requests. They should only receive the first two increments of data
                    // containing newline characters ("Space" and " is disease and ")
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Make sure that the extra jibberish did not overwrite the expected
                    // strings.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Space", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(" is disease and ", s2);
                    Assert.AreEqual(2, p2);
                }
                // Make sure to clean up sockets and close gracefully.
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        /// <summary>
        /// Written by Camille Rasmussen and Jessie Delacenserie
        /// 
        /// This Test Method tests the behavior of the Close() method upon no calls to the BeginSend or
        /// BeginReceive methods. This test makes sure that the client socket is not disconnected when
        /// you invoke Close() on the sendSocket, but that the server socket is successfully disconnected
        /// when you do so.
        /// 
        /// We also test to make sure you can't access the Socket after it's StringSocket's Close() has 
        /// been invoked, by calling a Socket's Available property, which throws an exception if the
        /// Socket has been closed properly.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            new CloseWithoutReceieveOrSend().run(4001);
        }

        public class CloseWithoutReceieveOrSend
        {
            public void run(int port)
            {
                TcpListener server = null;
                TcpClient client = null;

                // set up random encoder to use
                Encoding encoder = new ASCIIEncoding();

                // to check if exception was thrown and handled
                bool caught = false;

                try
                {
                    // create and start the server
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    // create the client
                    client = new TcpClient("localhost", port);

                    // set up the server and client sockets
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // set up string sockets with sockets
                    StringSocket sendSocket = new StringSocket(serverSocket, encoder);
                    StringSocket receiveSocket = new StringSocket(clientSocket, encoder);

                    // make sure the sockets are connected initially
                    Assert.IsTrue(serverSocket.Connected);
                    Assert.IsTrue(clientSocket.Connected);

                    // close the sendSocket String Socket
                    sendSocket.Close();
                    // make sure wrapped Socket specified is disconnected accordingly
                    Assert.IsFalse(serverSocket.Connected);
                    // and the other wrapped Socket isn't affected
                    Assert.IsTrue(clientSocket.Connected);

                    // close the receiveSocket String Socket
                    receiveSocket.Close();
                    // make sure wrapped socket specified is disconnected accordingly
                    Assert.IsFalse(clientSocket.Connected);

                    // this should throw an exception if the Socket was closed properly
                    int amount = serverSocket.Available;
                }
                // exception caught here
                catch (ObjectDisposedException e)
                {
                    caught = true;
                }
                finally
                {
                    // close up your resources and stop the server
                    server.Stop();
                    client.Close();

                    // make sure proper exception was thrown and caught
                    Assert.IsTrue(caught);
                }
            }
        }



        /// <summary>
        /// Written By: Sam Godfrey and Fahad Alothaimeen
        /// Using a modified test case provded by Dr. St. Germain
        /// This test method starts two clients and makes sure that the order that messages are sent are received by the correct clients.
        /// </summary>
        [TestMethod()]
        public void MultipuleCLientTest()
        {
            new MultipuleCLientTestClass().run(4001);
        }

        public class MultipuleCLientTestClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private static StringSocket sendSocket;
            private static StringSocket receiveSocket;
            private static StringSocket receiveSocket1;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, 23);
                    server.Start();
                    client = new TcpClient("localhost", 23);
                    // Create three sockets, one that is the socket for the server connection and two are going to represent the clients
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;
                    Socket clientSocket1 = client.Client;

                    // Wrap the sockets in a string socket
                    sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());
                    receiveSocket1 = new StringSocket(clientSocket1, new UTF8Encoding());

                    // These items will coordinate the threads
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Start two BeginReceive methods, one for each client socket
                    receiveSocket.BeginReceive(CompletedReceive, 1);
                    receiveSocket1.BeginReceive(CompletedReceive1, 2);

                    // Send 'two' messages from the server. You should parse this message into two messages
                    String msg = "Hello world\nThis is a test\n";
                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    // The first message should be received by the first client
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);
                    // The second message should be received by the second client
                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                    // Create a third message to send, this message should be recieved by the first client
                    string msg1 = "Hello There\n";
                    sendSocket.BeginSend(msg1, (e, o) => { }, null);
                    Thread.Sleep(2);
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("Hello There", s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread. Inside this assertion we call BeginReceive again to keep receiving messages.
            // This callback method starts a new BeginReceive method after we have received a full message. This allows
            // for continuous listening after a message has been recieved an parsed.
            private void CompletedReceive(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                Console.Write("BeginRecieve1: {0}\n", s1);
                mre1.Set();
                receiveSocket.BeginReceive(CompletedReceive, 1);
            }
            // The call back for the second receive request is the exact same as the first
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                Console.Write("BeginRecieve2: {0}\n", s2);
                mre2.Set();
                receiveSocket1.BeginReceive(CompletedReceive1, 2);
            }
        }



        #region Send1Receive2
        /// <summary>
        /// Test method to check to see if the BeginReceive can handle
        /// two "\n" passed in from a single BeginSend.
        /// </summary>
        [TestMethod]
        public void Send1Receive2()
        {
            new Test10Class().run(4005);
        }

        public class Test10Class
        {
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // String messages that are sent from the server to the client
                    String message1 = "Hello World!";
                    String message2 = " Goodbye World!";

                    // Combines both messages together separated by the "\n" character
                    sendSocket.BeginSend(message1 + "\n" + message2 + "\n", (e, o) => { }, null);

                    // Each BeginReceive should each receive one message independently
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    sendSocket.Close();

                    // Checks that the received data corresponds with the messages sent
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(message1, s1);
                    Assert.AreEqual(1, p1);
                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(message2, s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }
        #endregion
        /// 
        /// Makes sure that callbacks are called in a new
        /// thread, by passing a callback that sleeps forever.
        /// Sheyne Anderson and Nathan Donaldson 
        /// 
        [TestMethod()]
        public void AnnoyinglyLongCallback()
        {
            new AnnoyinglyLongCallbackTestClass().run(4001);
        }

        public class AnnoyinglyLongCallbackTestClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 20000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is a test\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => Thread.Sleep(Timeout.Infinite), null);
                    }
                    //sendSocket.BeginSend(msg, (e, o) => { },null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        /// <author>John Robe and Dietrich Geisler</author>
        /// <summary>
        /// Tests that the string socket can handle an extremely long single string with interspersed new line characters
        /// </summary>
        [TestMethod()]
        public void TestMassiveSingleMessage()
        {
            //Declare everything before the try-catch block so that the finally can activate correctly
            TcpListener server = null;
            TcpClient client = null;
            Socket serverSocket = null;
            Socket clientSocket = null;

            StringSocket sendSocket = null;
            StringSocket receiveSocket = null;

            int port = 4002;
            int timeout = 10000;

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = new TcpClient("localhost", port);

                //Sets up the sockets from both ends
                serverSocket = server.AcceptSocket();
                clientSocket = client.Client;

                //Wraps the sockets into string sockets
                sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                //Creates 10 unique mres for our 10 substrings
                ManualResetEvent[] mreList = new ManualResetEvent[10];
                for (int i = 0; i < 10; i++)
                    mreList[i] = new ManualResetEvent(false);

                //Creates 10 unique strings for the results from our sockets
                String[] results = new String[10];
                for (int i = 0; i < 10; i++)
                    results[i] = "";

                //Sets up a large string of random characters with 10 dispersed newline symbols
                String toSend = "";
                String[] toTest = new String[10];
                Random rng = new Random();

                for (int i = 0; i < 10; i++)
                {
                    double segmentSize = rng.Next(2000, 5000);

                    for (int j = 0; j < segmentSize; j++) //Creates a series of segments of random length
                        toTest[i] += (char)(rng.Next(26) + 65); //Appends a random upper-case letter to the string

                    toSend += toTest[i] + "\n"; //Adds a newline character to the end of the current string segment
                }

                //Starts 10 unique receives
                for (int i = 0; i < 10; i++)
                {
                    receiveSocket.BeginReceive((s, e, p) => { results[(int)p] = s; mreList[(int)p].Set(); }, i); //Note that i is used as the payload to get the correct array index
                }

                sendSocket.BeginSend(toSend, (e, p) => { }, null);

                //Test that all the strings came back correctly
                for (int i = 0; i < 10; i++)
                {
                    Assert.AreEqual(true, mreList[i].WaitOne(timeout));
                    Assert.AreEqual(toTest[i], results[i]);
                }
            }

            finally
            {
                //Clean everything up
                sendSocket.Close();
                receiveSocket.Close();
                server.Stop();
                client.Close();

            }
        }

        /// <summary>
        ///A simple test for ensuring the BeginReceive method is nonblocking
        ///</summary>
        [TestMethod()]
        public void Test1Taylor()
        {
            new Test1ClassTaylor().run(4001);
        }

        /// <summary>
        /// Author: John McKay, Taylor Jenkins
        /// Citation: modified teacher's test to check to see if BeginReceive is non-blocking
        /// This code is not guarenteed to work. Please comment if you have issues with this test.
        /// </summary>
        public class Test1ClassTaylor
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private object p1;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;
            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Now send the data. We are testing to see if the callback will block
                    // or if it will run on a different thread, and allow us to beginReceive again below
                    String msg = "h\re\rl\rl\ro\r";
                    sendSocket.BeginReceive((s, e, p) => { while (true) { } }, p1);

                    // start a new beginReceive before the old callback is finished (in 2 seconds)
                    // and see if our callback has actually been called.
                    sendSocket.BeginReceive((s, e, p) => { mre1.Set(); }, p2);

                    // wait 2 seconds
                    // This test should pass, if your callback in your beginReceive in non-blocking
                    Assert.IsTrue(mre1.WaitOne(timeout));
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

        }


        ///// <summary>
        ///// Extension to the test class.
        ///// </summary>
        ///// <param name="port"></param>
        //public void runTest5(int port)
        //{
        //    // Create and start a server and client.
        //    TcpListener server = null;
        //    TcpClient client = null;
        //    try
        //    {
        //        server = new TcpListener(IPAddress.Any, port);
        //        server.Start();
        //        client = new TcpClient("localhost", port);

        //        // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
        //        // method here, which is OK for a test case.
        //        Socket serverSocket = server.AcceptSocket();
        //        Socket clientSocket = client.Client;


        //        StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
        //        StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

        //        mre1 = new ManualResetEvent(false);
        //        mre2 = new ManualResetEvent(false);

        //        // Make two receive requests
        //        for (int i = 0; i < 26; i++)
        //        {

        //            Thread.Sleep(10);
        //            char c = (char)(i + 65);
        //            string message = "";
        //            if (i % 2 == 0)
        //            {
        //                receiveSocket.BeginReceive(CompletedReceive1, 1);
        //                if (!ReferenceEquals(s2, null))
        //                {
        //                    message += s2;
        //                }
        //                message += (c + "\n");
        //                sendSocket.BeginSend(message, (e, o) => { }, null);
        //            }
        //            else
        //            {
        //                sendSocket.BeginReceive(CompletedReceive2, 2);
        //                if (!ReferenceEquals(s1, null))
        //                {
        //                    message += s1;
        //                }
        //                message += (c + "\n");
        //                receiveSocket.BeginSend(message, (e, o) => { }, null);
        //            }
        //        }
        //        Thread.Sleep(100);
        //        Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", s2);
        //        string large = "";
        //        for (int i = 0; i < 10; i++)
        //        {
        //            ThreadPool.QueueUserWorkItem(x => sendSocket.BeginSend(s2, (e, o) => { }, null));
        //            large += s2;
        //        }
        //        ThreadPool.QueueUserWorkItem(x => receiveSocket.BeginReceive(CompletedReceive1, 1));
        //        for (int i = 0; i < 10; i++)
        //        {
        //            ThreadPool.QueueUserWorkItem(x => sendSocket.BeginSend(s2, (e, o) => { }, null));
        //            large += s2;
        //        }
        //        Thread.Sleep(100);
        //        sendSocket.BeginSend("\n", (e, o) => { }, null);
        //        Thread.Sleep(10);
        //        Assert.AreEqual(large, s1);

        //        //Close both Send and Receive sockets so that Asynchronous callbacks don't throw an exception once
        //        //the server and client are stopped.
        //        sendSocket.Close();
        //        receiveSocket.Close();
        //    }

        //    finally
        //    {
        //        server.Stop();
        //        client.Close();
        //    }
        //}


        /// <summary>
        /// <Author>Brandon Hilton and Meher Samineni</Author>
        /// </summary>
        [TestMethod]
        public void Test2Brandon()
        {
            new Test2ClassBrandon().run(4001);
        }
        public class Test2ClassBrandon
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private ManualResetEvent mre4;
            private ManualResetEvent mre5;
            private ManualResetEvent mre6;
            private ManualResetEvent mre7;
            private String s1 = "";
            private String s2 = "";
            private String s3 = "";
            private String s4 = "";
            private String s5 = "";
            private String s6 = "";
            private String s7 = "";
            private object p1 = null;
            private object p2 = null;
            private object p3 = null;
            private object p4 = null;
            private object p5 = null;
            private object p6 = null;
            private object p7 = null;

            // Timeout used in test case
            int timeout = 20000;

            public void run(int port)
            {

                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);
                    mre4 = new ManualResetEvent(false);
                    mre5 = new ManualResetEvent(false);
                    mre6 = new ManualResetEvent(false);
                    mre7 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s1 = s; p1 = p; mre1.Set(); }, 1);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s2 = s; p2 = p; mre2.Set(); }, 2);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s3 = s; p3 = p; mre3.Set(); }, 3);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s4 = s; p4 = p; mre4.Set(); }, 4);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s5 = s; p5 = p; mre5.Set(); }, 5);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s6 = s; p6 = p; mre6.Set(); }, 6);
                    receiveSocket.BeginReceive((string s, Exception e, object p) => { s7 = s; p7 = p; mre7.Set(); }, 7);
                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is Ruby\nThis is Weiss\nThis is Blake\nThis is Yang\nTeam RWBY\nI don't Know what else to say\n";

                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is Ruby", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("This is Weiss", s3);
                    Assert.AreEqual(3, p3);

                    Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
                    Assert.AreEqual("This is Blake", s4);
                    Assert.AreEqual(4, p4);

                    Assert.AreEqual(true, mre5.WaitOne(timeout), "Timed out waiting 5");
                    Assert.AreEqual("This is Yang", s5);
                    Assert.AreEqual(5, p5);

                    Assert.AreEqual(true, mre6.WaitOne(timeout), "Timed out waiting 6");
                    Assert.AreEqual("Team RWBY", s6);
                    Assert.AreEqual(6, p6);

                    Assert.AreEqual(true, mre7.WaitOne(timeout), "Timed out waiting 6");
                    Assert.AreEqual("I don't Know what else to say", s7);
                    Assert.AreEqual(7, p7);

                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

        }


        /// <summary>
        ///A simple test for BeginSend and BeginReceive
        ///This is a test to ensure that the user handles a blank message that is not a zero-byte message (empty string)
        /// This test is a modification of the test that Dr. de Saint Germain. 
        /// Modifiers: Daniel Clyde & Alex Whitelock
        ///</summary>
        [TestMethod()]
        public void multipleNewLines()
        {
            new Test1ClassDan().run(4001);
        }

        public class Test1ClassDan
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String emptyMessage;
            private object p1;
            private String finalMessage;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make 14 receive requests which will get the data from "Hello world" and 14

                    for (int i = 0; i < 14; i++)
                        receiveSocket.BeginReceive(receiveEmpty, 1);
                    receiveSocket.BeginReceive(recieveFinal, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    //This string contains 13 newLine symbols
                    String lines = "\n\n\n\n\n\n\n\n\n\n\n\n\n";
                    //This string contains "Hello world" followed by a newLine, 13 newLine characters, and then "This is a test" followed by a final newLine character.
                    String msg = "Hello world\n" + lines + "This is a test\n";

                    sendSocket.BeginSend(msg.ToString(), (e, o) => { }, null);

                    //Expect the StringSocket to continue receiving data until the final message is received ("This is a test").
                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", finalMessage);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for all the \n requests
            private void receiveEmpty(String s, Exception o, object payload)
            {
                emptyMessage = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the final receive request and should capture the this is a test.
            private void recieveFinal(String s, Exception o, object payload)
            {
                finalMessage = s;
                p2 = payload;
                mre2.Set();
            }
        }


        /// <summary>
        ///This is a test class for StringSocketTest and is intended
        ///to contain all StringSocketTest Unit Tests
        ///<author>Brady Mathews and Kevin Faustino</author>
        ///</summary>
        [TestClass()]
        public class StringSocketTest1
        {


            private TestContext testContextInstance;

            /// <summary>
            ///Gets or sets the test context which provides
            ///information about and functionality for the current test run.
            ///</summary>
            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Additional test attributes
            // 
            //You can use the following additional attributes as you write your tests:
            //
            //Use ClassInitialize to run code before running the first test in the class
            //[ClassInitialize()]
            //public static void MyClassInitialize(TestContext testContext)
            //{
            //}
            //
            //Use ClassCleanup to run code after all tests in a class have run
            //[ClassCleanup()]
            //public static void MyClassCleanup()
            //{
            //}
            //
            //Use TestInitialize to run code before running each test
            //[TestInitialize()]
            //public void MyTestInitialize()
            //{
            //}
            //
            //Use TestCleanup to run code after each test has run
            //[TestCleanup()]
            //public void MyTestCleanup()
            //{
            //}
            //
            #endregion


            /// <summary>
            ///A simple test for BeginSend and BeginReceive
            ///</summary>
            [TestMethod()]
            public void Test2()
            {
                new Test1Class1().run(4001);
            }

            public class Test1Class1
            {
                // Data that is shared across threads
                private ManualResetEvent mre1;
                private ManualResetEvent mre2;
                private String s1;
                private object p1;
                private String s2;
                private object p2;
                private String s3;
                private object p3;

                // Timeout used in test case
                private static int timeout = 16000;

                public void run(int port)
                {
                    // Create and start a server and client.
                    TcpListener server = null;
                    TcpClient client = null;

                    try
                    {
                        server = new TcpListener(IPAddress.Any, port);
                        server.Start();
                        client = new TcpClient("localhost", port);

                        // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                        // method here, which is OK for a test case.
                        Socket serverSocket = server.AcceptSocket();
                        Socket clientSocket = client.Client;

                        // Wrap the two ends of the connection into StringSockets
                        StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                        StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                        // This will coordinate communication between the threads of the test cases
                        mre1 = new ManualResetEvent(false);
                        mre2 = new ManualResetEvent(false);

                        for (int i = 0; i < 200; i++)
                        {
                            receiveSocket.BeginReceive(CompletedReceive3, i);
                            sendSocket.BeginSend("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz\n", (e, o) => { }, null);
                            Thread.Sleep(10);
                            //Run this assert method 200 times - and make sure no letters flipped
                            //Sometimes letters will flip due to a thread racing instance
                            Assert.AreEqual("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz", s3);
                            Assert.AreEqual(i, p3);
                        }

                        // Make two receive requests
                        receiveSocket.BeginReceive(CompletedReceive1, 1);
                        receiveSocket.BeginReceive(CompletedReceive2, 2);
                        // Now send the data.  Hope those receive requests didn't block!
                        String msg = "Hello World!\nThis is a test\n";
                        foreach (char c in msg)
                        {
                            sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                        }

                        // Make sure the lines were received properly.
                        Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                        Assert.AreEqual("Hello World!", s1);
                        Assert.AreEqual(1, p1);

                        //Callbacks threads are blocking if top assert fails -- This computation should
                        //Not take over 30 seconds if the threads are non blocking
                        Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                        Assert.AreEqual("This is a test", s2);
                        Assert.AreEqual(2, p2);
                    }
                    finally
                    {
                        server.Stop();
                        client.Close();
                    }
                }

                // This is the callback for the first receive request.  We can't make assertions anywhere
                // but the main thread, so we write the values to member variables so they can be tested
                // on the main thread.
                private void CompletedReceive1(String s, Exception o, object payload)
                {
                    //A very expensive computation taking 15 seconds
                    Thread.Sleep(15000);
                    s1 = s;
                    p1 = payload;
                    mre1.Set();
                }

                // This is the callback for the second receive request.
                private void CompletedReceive2(String s, Exception o, object payload)
                {
                    //A very expensive computation taking 15 seconds
                    Thread.Sleep(15000);
                    s2 = s;
                    p2 = payload;
                    mre2.Set();
                }

                private void CompletedReceive3(String s, Exception o, object payload)
                {
                    s3 = s;
                    p3 = payload;
                }
            }


        }

        /// <summary>
        /// modified version of the above test from the ps7 skeleton
        /// </summary>
        [TestMethod]
        public void TestMethod1modified()
        {
            new StringTesters().run(4002);
        }

        public class StringTesters
        {
            private ManualResetEvent mre1;
            private string s1;
            private Object p1;

            private static int timeout = 2000;

            public void run(int port)
            {

                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);


                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    mre1 = new ManualResetEvent(false);

                    receiveSocket.BeginReceive(CompletedRecieve1, 1);

                    string msg = "Hey Guy\n";

                    sendSocket.BeginSend(msg, (e, o) => { }, "cat");

                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hey Guy", s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            private void CompletedRecieve1(string s, Exception e, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

        }

        [TestMethod]
        public void testBrokenMessage()
        {
            Thread test = new Thread(o =>
            {
                new BrokenMessages().run(4001);
            });
            test.Start();
            test.Abort();
        }

        /// <summary>
        /// Test class written by Michael Zhao and Aaron Hsu. Tests whether
        /// a) a large message broken across different BeginSends sends properly
        /// b) if a dangling message (one not terminated with a "\n") is handled properly
        /// </summary>
        public class BrokenMessages
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;


            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);

                    //Create a long string for beginsend
                    string message1 = "hnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnggggggggggggggggggggggggggggggggggggggggggggggggggg ";
                    string message2 = "soooooooooooooooooooooccccckkeeeeetttssssssss\nhnnnnnnnnnnnnnnnnnnnnnnnnggggggggggggggggggggggggggggggggg ";
                    string message3 = "hnnnnnnnnnnnnnnnnnnnnngggggggggggggggggggggggggggggg\nhnnnnnnnnnnnnnnnnnnnnnnnnnnggggggggggggggggggggg";
                    string[] messages = Regex.Split(message1 + message2 + message3, "\n");

                    //Create a new thread to enusre that close gets called at the same time as BeginSend()
                    sendSocket.BeginSend(message1, (e, o) => { }, null);
                    sendSocket.BeginSend(message2, (e, o) => { }, null);
                    sendSocket.BeginSend(message3, (e, o) => { }, null);
                    sendSocket.Close();

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(messages[0], s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(messages[1], s2);
                    Assert.AreEqual(2, p2);

                    //Check that we timeout waiting for the third receive request, and that s3 remains null.
                    Assert.AreEqual(false, mre3.WaitOne(timeout), "Did not time out waiting for 3");
                    Assert.AreEqual(null, s3);


                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre1.Set();
            }

            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre1.Set();
            }
        }

        /// <summary>
        ///A test for empty strings and carriage returns
        ///Tests that \n works for empty strings, \r works for full strings, \r works for empty strings
        ///Also tests to make sure that strings are received in the proper order while doing so
        ///Modifies test case already provided, done by Logan Gore
        ///</summary>
        [TestMethod()]
        public void TestMultipleEmptyStrings()
        {
            new EmptyAndCarriageStrings().run(4001);
        }

        public class EmptyAndCarriageStrings
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make three receive requests
                    receiveSocket.BeginReceive(ReceiveEmpty, 1);
                    receiveSocket.BeginReceive(ReceiveString, 2);
                    receiveSocket.BeginReceive(ReceiveEmpty, 1);

                    // Send multiple new lines mixing both carriage and newline characters
                    String msg = "\nHelloWorld\r\r";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure empty string with \n
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("", s1);
                    Assert.AreEqual(1, p1);

                    //Make sure full string with \r
                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("HelloWorld", s2);
                    Assert.AreEqual(2, p2);

                    //Make sure empty string with \r
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("", s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            //call back that receives the empty string
            private void ReceiveEmpty(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // call back that receives helloworld, ensuring that carriage returns work with strings
            private void ReceiveString(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        /// <summary>
        /// Val Nicholas Hallstrom
        /// This test makes sure that your program handles sparatic newlines properly.
        /// I used the setupServerClient method from another useful test I found on the forum since the way I was trying to setup the sockets didn't seem to work :/
        /// </summary>
        [TestMethod()]
        public void MultipleNewlines()
        {
            int timeout = 1000;
            StringSocket sendSocket;
            StringSocket receiveSocket;
            string s1 = "";
            int p1 = 0;
            string s2 = "";
            int p2 = 0;
            string s3 = "";
            int p3 = 0;
            string s4 = "";
            int p4 = 0;
            string s5 = "";
            int p5 = 0;

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            ManualResetEvent mre3 = new ManualResetEvent(false);
            ManualResetEvent mre4 = new ManualResetEvent(false);
            ManualResetEvent mre5 = new ManualResetEvent(false);

            setupServerClient(63333, out sendSocket, out receiveSocket);

            receiveSocket.BeginReceive((s, e, p) => { s1 = s; p1 = (int)p; mre1.Set(); }, 1);
            receiveSocket.BeginReceive((s, e, p) => { s2 = s; p2 = (int)p; mre2.Set(); }, 2);
            receiveSocket.BeginReceive((s, e, p) => { s3 = s; p3 = (int)p; mre3.Set(); }, 3);
            receiveSocket.BeginReceive((s, e, p) => { s4 = s; p4 = (int)p; mre4.Set(); }, 4);
            receiveSocket.BeginReceive((s, e, p) => { s5 = s; p5 = (int)p; mre5.Set(); }, 5);
            sendSocket.BeginSend("\nWhat's\n\nup?\n\n", (e, p) => { }, null);

            Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
            Assert.AreEqual("", s1);
            Assert.AreEqual(1, p1);

            Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
            Assert.AreEqual("What's", s2);
            Assert.AreEqual(2, p2);

            Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
            Assert.AreEqual("", s3);
            Assert.AreEqual(3, p3);

            Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
            Assert.AreEqual("up?", s4);
            Assert.AreEqual(4, p4);

            Assert.AreEqual(true, mre5.WaitOne(timeout), "Timed out waiting 5");
            Assert.AreEqual("", s5);
            Assert.AreEqual(5, p5);
        }

        public void setupServerClient(int port, out StringSocket sendSocket, out StringSocket receiveSocket)
        {
            // Create and start a server and client.
            TcpListener server = null;
            TcpClient client = null;
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            client = new TcpClient("localhost", port);

            // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
            // method here, which is OK for a test case.
            Socket serverSocket = server.AcceptSocket();
            Socket clientSocket = client.Client;

            // Wrap the two ends of the connection into StringSockets
            sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
            receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

        }

        /// <summary>
        ///This is a test class for StringSocketTest and is intended
        ///to contain all StringSocketTest Unit Tests
        ///</summary>
        [TestClass()]
        public class StringSocketTest7
        {


            private TestContext testContextInstance;

            /// <summary>
            ///Gets or sets the test context which provides
            ///information about and functionality for the current test run.
            ///</summary>
            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Additional test attributes
            // 
            //You can use the following additional attributes as you write your tests:
            //
            //Use ClassInitialize to run code before running the first test in the class
            //[ClassInitialize()]
            //public static void MyClassInitialize(TestContext testContext)
            //{
            //}
            //
            //Use ClassCleanup to run code after all tests in a class have run
            //[ClassCleanup()]
            //public static void MyClassCleanup()
            //{
            //}
            //
            //Use TestInitialize to run code before running each test
            //[TestInitialize()]
            //public void MyTestInitialize()
            //{
            //}
            //
            //Use TestCleanup to run code after each test has run
            //[TestCleanup()]
            //public void MyTestCleanup()
            //{
            //}
            //
            #endregion


            /// <summary>
            /// Written by Hannah Eyre and Michael Nelson by modifying the test in PS7Skeleton.
            /// This test should make sure the StringSocket is correctly handling different types of unusual strings, such as characters out of
            /// the UTF-8 encoding, empty strings, and null strings.
            /// 
            /// The test for UTF-8 involves a UTF-16 capital sigma. Because this is outside the range of acceptable UTF-8 values, the replacement
            /// fallback handler should replace this with a "?" character unless the EncoderFallback has been set to exception or best-fit.
            ///</summary>
            [TestMethod()]
            public void BadStringTest()
            {
                new BadStringsTest().run(4001);
            }

            public class BadStringsTest
            {
                // Data that is shared across threads
                private ManualResetEvent mre1;
                private ManualResetEvent mre2;
                private ManualResetEvent mre3;
                private ManualResetEvent mre4;
                private String s1;
                private object p1;
                private String s2;
                private object p2;
                private String s3;
                private object p3;
                private object p4;

                // Timeout used in test case
                private static int timeout = 2000;

                public void run(int port)
                {
                    // Create and start a server and client.
                    TcpListener server = null;
                    TcpClient client = null;

                    try
                    {
                        server = new TcpListener(IPAddress.Any, port);
                        server.Start();
                        client = new TcpClient("localhost", port);

                        // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                        // method here, which is OK for a test case.
                        Socket serverSocket = server.AcceptSocket();
                        Socket clientSocket = client.Client;

                        // Wrap the two ends of the connection into StringSockets
                        StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                        StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                        // This will coordinate communication between the threads of the test cases
                        mre1 = new ManualResetEvent(false);
                        mre2 = new ManualResetEvent(false);
                        mre3 = new ManualResetEvent(false);

                        // Make two receive requests
                        receiveSocket.BeginReceive(CompletedReceive1, 1);
                        receiveSocket.BeginReceive(CompletedReceive2, 2);
                        receiveSocket.BeginReceive(CompletedReceive3, 3);

                        // Now send the data.  Hope those receive requests didn't block!
                        String msg = "\u01A9\nHello\n\n"; //sigma, Hello on new line
                        foreach (char ch in msg)
                            sendSocket.BeginSend(ch.ToString(), (e, o) => { }, null);

                        // Make sure the lines were received properly.
                        Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                        Assert.AreEqual("?", s1);
                        Assert.AreEqual(1, p1);

                        Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                        Assert.AreEqual("Hello", s2);
                        Assert.AreEqual(2, p2);

                        Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                        Assert.AreEqual("", s3);
                        Assert.AreEqual(3, p3);

                        sendSocket.BeginSend(null, (e, o) => { }, null);

                        Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
                        Assert.AreEqual(4, p4);
                    }
                    finally
                    {
                        server.Stop();
                        client.Close();
                    }
                }

                // This is the callback for the first receive request.  We can't make assertions anywhere
                // but the main thread, so we write the values to member variables so they can be tested
                // on the main thread.
                private void CompletedReceive1(String s, Exception o, object payload)
                {
                    Assert.IsNull(o);
                    s1 = s;
                    p1 = payload;
                    mre1.Set();
                }

                // This is the callback for the second receive request.
                private void CompletedReceive2(String s, Exception o, object payload)
                {
                    Assert.IsNull(o);
                    s2 = s;
                    p2 = payload;
                    mre2.Set();
                }

                private void CompletedReceive3(String s, Exception o, object payload)
                {
                    Assert.IsNull(o);
                    s3 = s;
                    p3 = payload;
                    mre3.Set();
                }

                private void CompletedReceive4(String s, Exception o, object payload)
                {
                    Assert.IsNull(s);
                    Assert.IsTrue(o is ArgumentNullException);
                    p4 = payload;
                    mre4.Set();
                }
            }
        }

        /// <summary>
        /// <author>Derek Heldt-Werle</author>
        /// Test to ensure that the characters following a new line is properly
        /// disposed of, and only the data preceeding the new line character is returned.
        /// </summary>
        [TestMethod()]
        public void LongStringFollowedByShortStringTest()
        {
            new Test1ClassDerek().run(4001);
        }

        public class Test1ClassDerek
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "Is this the real life? Is this just fantasy?\n Insert more lyrics here";

                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Is this the real life? Is this just fantasy?", s1);
                    Assert.AreEqual(1, p1);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }




        /// <summary>
        ///Authors: Blake Burton, Cameron Minkel
        ///Date: 11/12/14
        ///
        ///This is a simple test which makes sure the supplied callback
        ///to StringSocket's BeginSend method is called upon method completion.
        ///</summary>
        [TestMethod()]
        public void TestBeginSendCallback()
        {
            new Test2ClassBlake().run(4001);
        }

        public class Test2ClassBlake
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private bool callbackInvoked = false;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the socket from the connection
                    Socket serverSocket = server.AcceptSocket();

                    // Wrap the socket into a StringSocket
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Send a test message
                    sendSocket.BeginSend("Please pass.", CompletedSend, new Object());
                    mre1.WaitOne(timeout);

                    // Make sure the callback was called 
                    Assert.IsTrue(callbackInvoked);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // The bool will tell us if the callback was invoked.
            private void CompletedSend(Exception o, object payload)
            {
                callbackInvoked = true;
                mre1.Set();
            }
        } // end TestBeginSendCallback



        /// <summary>
        /// Test designed by Ren Miao & Chonghuan Xia
        /// By sending many random strings, to check whether the receiver get the right string.
        ///</summary>
        [TestMethod()]
        public void TestRandomString()
        {
            new NewTestClass().run(4111);
        }

        public class NewTestClass
        {

            List<ReceivedInfo> received;
            private int numberOfTest = 500;

            // Timeout used in test case
            private static int timeout = 5000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                List<String> strList = new List<string>();
                List<String> expected = new List<string>();
                received = new List<ReceivedInfo>();

                for (int i = 0; i < numberOfTest; i++)
                {
                    StringGenerator generator = new StringGenerator();
                    string temp = generator.Generate(20);

                    strList.Add(temp + "\n");
                    expected.Add(temp);

                    // required here since Random object depends on the system time to generate 
                    // the random number. If generating too quickly we will get same number everytime!
                    Thread.Sleep(10);
                }

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    Random random = new Random();

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // Make 20 receive requests
                    for (int i = 0; i < numberOfTest; i++)
                    {
                        receiveSocket.BeginReceive(CompletedReceive1, i);
                    }

                    foreach (string s in strList)
                    {
                        // genearte a random number, if even, send the string character by character, if odd, send the whole once.
                        int ranNum = random.Next();

                        if (ranNum % 2 == 0)
                        {
                            foreach (char c in s)
                            {
                                sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                            }
                        }
                        else
                            sendSocket.BeginSend(s, (e, o) => { }, null);
                    }

                    // let the communication finish. you may need to change the timeout;
                    Thread.Sleep(timeout);

                    for (int i = 0; i < numberOfTest; i++)
                    {
                        // make sure each received message is the same as expected.
                        Assert.AreEqual(expected[i], received[i].Message);
                        // make sure the payload for each receiveCallback is as expected.
                        Assert.AreEqual(i, received[i].Payload);
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                // add the string and payload info into the received list.
                received.Add(new ReceivedInfo(s, payload));
            }

            /// <summary>
            /// a class to store the string and payload information from each receiveCallback.
            /// </summary>
            private class ReceivedInfo
            {
                public Object Payload { get; set; }
                public String Message { get; set; }

                public ReceivedInfo(string s, Object p)
                {
                    Payload = p;
                    Message = s;
                }
            }

            /// <summary>
            /// a class to generate random string, adapted from online resources
            /// </summary>
            public class StringGenerator
            {
                List<char> _characters;
                public StringGenerator()
                {
                    _characters = new List<char>();
                    // Fill character list with A-Z.
                    for (int i = 65; i <= 90; i++)
                    {
                        _characters.Add((char)i);
                    }
                    // Fill character list with a-z.
                    for (int i = 97; i <= 122; i++)
                    {
                        _characters.Add((char)i);
                    }
                    // Fill character list with 0-9.
                    for (int i = 48; i <= 57; i++)
                    {
                        _characters.Add((char)i);
                    }
                }
                public string Generate(int lenght)
                {
                    Random random = new Random();
                    StringBuilder buffer = new StringBuilder(lenght);
                    for (int i = 0; i < lenght; i++)
                    {
                        int randomNumber = random.Next(0, _characters.Count);
                        char randomChar = _characters[randomNumber];
                        buffer.Append(randomChar);
                    }
                    return buffer.ToString();
                }
            }
        }





        /// <summary>
        /// A simple test for BeginSend and BeginReceive making sure whitespace isn't removed and quotation marks are preserved.
        /// Also tests whether requests can be made before and after the requested data is sent.
        /// Provided test case modified by Drew McClelland.
        ///</summary>
        [TestMethod()]
        public void Test1Drew()
        {
            new Test1ClassDrew().run(4001);
        }

        public class Test1ClassDrew
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private ManualResetEvent mre4;
            private ManualResetEvent mre5;
            private ManualResetEvent mre6;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;
            private String s4;
            private object p4;
            private String s5;
            private object p5;
            private String s6;
            private object p6;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);
                    mre4 = new ManualResetEvent(false);
                    mre5 = new ManualResetEvent(false);
                    mre6 = new ManualResetEvent(false);

                    // Make three receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);


                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "\t\n \n\"\"\n\"\n";
                    String msg2 = "\'\'\n\'\n";
                    // Sends first message as individual characters.
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Sends second message as one continuous line.
                    sendSocket.BeginSend(msg2, (e, o) => { }, null);

                    // Receive last three messages.
                    receiveSocket.BeginReceive(CompletedReceive4, 4);
                    receiveSocket.BeginReceive(CompletedReceive5, 5);
                    receiveSocket.BeginReceive(CompletedReceive6, 6);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("\t", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual(" ", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("\"\"", s3);
                    Assert.AreEqual(3, p3);

                    Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
                    Assert.AreEqual("\"", s4);
                    Assert.AreEqual(4, p4);

                    Assert.AreEqual(true, mre5.WaitOne(timeout), "Timed out waiting 5");
                    Assert.AreEqual("\'\'", s5);
                    Assert.AreEqual(5, p5);

                    Assert.AreEqual(true, mre6.WaitOne(timeout), "Timed out waiting 6");
                    Assert.AreEqual("\'", s6);
                    Assert.AreEqual(6, p6);


                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            // Should receive tab (\t) character.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // Should receive three consecutive spaces ( ).
            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            // Should receive two quotation marks ("")
            // This is the callback for the third receive request.
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }

            // Should receive one quotation mark (").
            // This is the callback for the fourth receive request.
            private void CompletedReceive4(String s, Exception o, object payload)
            {
                s4 = s;
                p4 = payload;
                mre4.Set();
            }

            // Should receive two single quotation marks ('').
            // This is the callback for the fifth receive request.
            private void CompletedReceive5(String s, Exception o, object payload)
            {
                s5 = s;
                p5 = payload;
                mre5.Set();
            }

            // Should receive one single quotation mark (').
            // This is the callback for the sixth receive request.
            private void CompletedReceive6(String s, Exception o, object payload)
            {
                s6 = s;
                p6 = payload;
                mre6.Set();
            }
        }

        /// <summary>
        /// Test Author: Douglas Thompson
        /// 11/12/2014
        /// A simple test for UTF-Characters: ?, é, ™ (trademark), and © (copyright).
        /// The test should complete with the UTF-Characters intact while receiving the string.
        ///</summary>
        [TestMethod()]
        public void SimpleTestForUTFChars()
        {
            new Test1ClassDouglas().run(4001);
        }

        public class Test1ClassDouglas
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data with an integral character, an accent, trademark symbol and copyright character. Hope those receive requests didn't block!
                    String msg = "? 3x dx\nPokémon™ © Nintendo\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("? 3x dx", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("Pokémon™ © Nintendo", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }



        /// <summary>
        ///This tests that the code accounts 
        ///for a user giving a null callback method 
        ///Since there are no instructions on what type of 
        ///Exception should be thrown, we test only that one
        ///was thrown so this test should be used after other 
        ///tests have been proven to pass with code being tested
        ///If one makes a decision about the type of exception one 
        ///could modify the expected type of exception and then 
        ///rerun the test. 
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NullCallbackTest()
        {
            new NullCallbackTestClass().run(4001);
        }

        public class NullCallbackTestClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1;
            private object p1;

            // Timeout used in test case
            private static int timeout = 200;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {

                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This would not really be needed but will be left in for similarity to the original test case 
                    mre1 = new ManualResetEvent(false);


                    // Makes a receive request
                    receiveSocket.BeginReceive(CompletedReceive1, 1);


                    // Now sends data. but should throw an error because the call back given to send is null 
                    String msg = "Hello world\nThis is a test\nMore test";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), null, null);
                    }

                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }


        }


        /// <summary>
        /// Authors: Blake Beckett and Victor Johnson
        /// 
        /// A simple test metod that ensures that messages are received in the correct order, despite when they were sent
        /// </summary>
        [TestMethod]
        public void StringSocketCrowdsourceTest()
        {
            new StringSocketCrowdsourceTestClass().runTest(4001);
        }

        private class StringSocketCrowdsourceTestClass
        {
            // Data that is shared across threads
            private List<Tuple<string, Exception, object>> results;

            public StringSocketCrowdsourceTestClass()
            {
                //use a list of tuples to store the received information from each beginreceive
                results = new List<Tuple<string, Exception, object>>();
            }

            public void runTest(int port)
            {
                //create server and clien members
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    //Initialize server and client
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Connect teh two
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap them in string sockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // First add two receive calls
                    receiveSocket.BeginReceive(MessageReceived1, 1);
                    receiveSocket.BeginReceive(MessageReceived1, 2);

                    //the first two recieve calls should get teh data from this send call
                    sendSocket.BeginSend("Is it working?\nProbably not...\n", (e, o) => { }, 0);

                    //This time, start sending the data first
                    sendSocket.BeginSend("I have no idea what I am doing\nIt's quite sad\n", (e, o) => { }, 0);

                    //If all went well, these should get the strings sent in thesecond beginSend call
                    receiveSocket.BeginReceive(MessageReceived1, 3);
                    receiveSocket.BeginReceive(MessageReceived1, 4);

                    //Make the thread sleep to avoid makin asertions before messages ave been sent(ok for a test case)
                    Thread.Sleep(2000);

                    //Use this method to make assertions
                    MakeAssertions();

                }

                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            //This is th callback used for all beginReceive calls
            private void MessageReceived1(string s, Exception e, object o)
            {
                results.Add(Tuple.Create(s, e, o));
            }

            //Used to make all assertions about the test.
            private void MakeAssertions()
            {
                Assert.AreEqual("Is it working?", results[0].Item1);
                Assert.IsNull(results[0].Item2);
                Assert.AreEqual(1, Convert.ToInt16(results[0].Item3));

                Assert.AreEqual("Probably not...", results[1].Item1);
                Assert.IsNull(results[1].Item2);
                Assert.AreEqual(2, Convert.ToInt16(results[1].Item3));

                Assert.AreEqual("I have no idea what I am doing", results[2].Item1);
                Assert.IsNull(results[2].Item2);
                Assert.AreEqual(3, Convert.ToInt16(results[2].Item3));

                Assert.AreEqual("It's quite sad", results[3].Item1);
                Assert.IsNull(results[3].Item2);
                Assert.AreEqual(4, Convert.ToInt16(results[3].Item3));

            }
        }



        /// <summary>
        /// Braden Scothern Test Case
        /// This test heavily relies on the original test we were given
        /// But it has been modified to run 5 times meaning that the ports need to be properly closed
        /// It also sends each char as it's own message now by putting a "\n" after each.
        /// This means that more messages are sent possibly resulting in more blocking if not strucutured correctly.
        /// </summary>
        [TestMethod()]
        public void Test_Port_ASCII_JIM2000() //ASCII code for JIM is 747377
        {
            for (int counter = 0; counter < 5; counter++)
            {
                String[] sA = new String[26];
                object[] pA = new object[26];
                String tester = "Hello world This is a test";

                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, ('J' + 'I' + 'M' + 2000));
                    server.Start();
                    client = new TcpClient("localhost", ('J' + 'I' + 'M' + 2000));

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    ManualResetEvent mre1 = new ManualResetEvent(false);
                    ManualResetEvent mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    for (int i = 0; i < tester.Length; i++)
                    {
                        receiveSocket.BeginReceive((s, o, p) => { sA[i] = s; pA[i] = p; }, i);
                    }

                    // Now send the data.  Hope those receive requests didn't block!
                    String msg = "H\ne\nl\nl\no\n \nw\no\nr\nl\nd\n \nT\nh\ni\ns\n \ni\ns\n \na\n \nt\ne\ns\nt\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    for (int i = 0; i < tester.Length; i++)
                    {
                        Assert.AreEqual(true, mre1.WaitOne(150), "Timed out waiting for char" + sA[i] + " (" + (i + 1) + ")");
                        Assert.AreEqual(tester[i], sA[i]);
                        Assert.AreEqual(i, pA[i]);
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }
        }



        /// <summary>
        /// Author: Dharani Adhikari
        /// This test case is based on what Prof. Jim provided and just checks
        /// if the stringSocket holds message properly when there are many receive requests but only
        /// a few messages were sent.
        /// Note: Since my StringSocket class is not working yet, I am not sure this test case works properly.
        ///</summary>
        [TestMethod()]
        public void TestMessageLeak()
        {
            new MessageLeak().run(4001);
        }

        public class MessageLeak
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private ManualResetEvent mre4;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;
            private String s4;
            private object p4;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);
                    mre4 = new ManualResetEvent(false);

                    // Make four receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);
                    receiveSocket.BeginReceive(CompletedReceive4, 4);

                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is a test\nThis is the end of message\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("This is the end of message", s3);
                    Assert.AreEqual(3, p3);

                    Assert.AreEqual(false, mre4.WaitOne(timeout), "Timed out waiting 4");
                    Assert.AreEqual(null, s4);
                    Assert.AreNotEqual(4, p4);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            // This is the callback for the third receive request.
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }

            // This is the callback for the fourth receive request.
            private void CompletedReceive4(String s, Exception o, object payload)
            {
                s4 = s;
                p4 = payload;
                mre4.Set();
            }
        }


        /// <summary>
        ///Tests a very long string in a different language
        ///</summary>
        [TestMethod()]
        public void Test1Langu()
        {
            new Test1ClassLangu().run(4001);
        }


        public class Test1ClassLangu
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private ManualResetEvent mre4;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;
            private String s4;
            private object p4;

            // Timeout used in test case
            private static int timeout = 20000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);
                    mre4 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);
                    receiveSocket.BeginReceive(CompletedReceive3, 3);
                    receiveSocket.BeginReceive(CompletedReceive4, 4);

                    // Now send the data.
                    String msg = "a\nb\nDer er et yndigt land, det står med brede bøge nær salten østerstrand :|Det bugter sig i bakke, dal, det hedder gamle Danmark "
                    + "og det er Frejas sal :| Der sad i fordums tid de harniskklædte kæmper, udhvilede fra strid :| Så drog de frem til fjenders mén, nu hvile deres "
                    + "bene bag højens bautasten :| Det land endnu er skønt, thi blå sig søen bælter, og løvet står så grønt :| Og ædle kvinder, skønne møer og mænd "
                    + "og raske svende bebo de danskes øer :| Hil drot og fædreland! Hil hver en danneborger, som virker, hvad han kan! :| Vort gamle Danmark skal bestå, "
                    + "så længe bøgen spejler sin top i bølgen blå\nc\n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("a", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("b", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("Der er et yndigt land, det står med brede bøge nær salten østerstrand :|Det bugter sig i bakke, dal, det hedder gamle Danmark "
                    + "og det er Frejas sal :| Der sad i fordums tid de harniskklædte kæmper, udhvilede fra strid :| Så drog de frem til fjenders mén, nu hvile deres "
                    + "bene bag højens bautasten :| Det land endnu er skønt, thi blå sig søen bælter, og løvet står så grønt :| Og ædle kvinder, skønne møer og mænd "
                    + "og raske svende bebo de danskes øer :| Hil drot og fædreland! Hil hver en danneborger, som virker, hvad han kan! :| Vort gamle Danmark skal bestå, "
                    + "så længe bøgen spejler sin top i bølgen blå", s3);
                    Assert.AreEqual(3, p3);

                    Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
                    Assert.AreEqual("c", s4);
                    Assert.AreEqual(4, p4);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            // This is the callback for the third receive request
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }

            // This is the callback for the fourth receive request.
            private void CompletedReceive4(String s, Exception o, object payload)
            {
                s4 = s;
                p4 = payload;
                mre4.Set();
            }
        }

        /// <summary>
        /// Author: Chaofeng Zhou and PinEn Chen
        /// Testing two client and thread sending
        /// this test is based the test provided by Jim
        ///</summary>
        [TestMethod()]
        public void TwoCleintRunningDifferentThreads()
        {
            new TwoCleintRunningDifferentThreadsClass().run(4010);
        }

        public class TwoCleintRunningDifferentThreadsClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            // recieved string from client 1
            private String s1;
            // payload from client 1
            private object p1;
            // recieved string from client 2
            private String s2;
            // payload from client 2
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and tow clients.
                TcpListener server = null;
                TcpClient client1 = null;
                TcpClient client2 = null;

                try
                {
                    // create a sever
                    server = new TcpListener(IPAddress.Any, port);
                    // start sever
                    server.Start();
                    // create client1
                    client1 = new TcpClient("localhost", port);
                    // create client2
                    client2 = new TcpClient("localhost", port);

                    // get sockets for the two clients and server
                    Socket serverSocket1 = server.AcceptSocket();
                    Socket clientSocket1 = client1.Client;
                    Socket serverSocket2 = server.AcceptSocket();
                    Socket clientSocket2 = client2.Client;

                    // Wrap the four ends of the connection into StringSockets
                    StringSocket sendSocket1 = new StringSocket(serverSocket1, new UTF8Encoding());
                    StringSocket sendSocket2 = new StringSocket(serverSocket2, new UTF8Encoding());
                    StringSocket receiveSocket1 = new StringSocket(clientSocket1, new UTF8Encoding());
                    StringSocket receiveSocket2 = new StringSocket(clientSocket2, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests for client 1 and client 2
                    receiveSocket1.BeginReceive(CompletedReceive1, 1);
                    receiveSocket2.BeginReceive(CompletedReceive2, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    String msg1 = "Hello, PinEn Chen.\nHow is your midterm.\n";
                    String msg2 = "Hello\nIt is not your business.\n";

                    // client 1 sends message 1 and client 2 sends messae 2 
                    // using thread
                    ThreadStart threadFunc1 = new ThreadStart(() => SocketSending(msg1, sendSocket1));
                    ThreadStart threadFunc2 = new ThreadStart(() => SocketSending(msg2, sendSocket2));
                    Thread worker1 = new Thread(threadFunc1);
                    Thread worker2 = new Thread(threadFunc2);

                    worker1.Start();
                    worker2.Start();

                    worker1.Join();
                    worker2.Join();

                    // Make sure the first time for the two clients work
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello, PinEn Chen.", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("Hello", s2);
                    Assert.AreEqual(2, p2);

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    receiveSocket1.BeginReceive(CompletedReceive1, 1);
                    receiveSocket2.BeginReceive(CompletedReceive2, 2);

                    // Make sure the second time for the two clients work 
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("How is your midterm.", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("It is not your business.", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client1.Close();
                    client2.Close();
                }
            }

            // call back for client 1
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // call back for client 2
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            // a function used for sending message through string socket
            // use this method because I want to do this on different thread
            private void SocketSending(String s, StringSocket ss)
            {
                foreach (char c in s)
                {
                    ss.BeginSend(c.ToString(), (e, o) => { }, null);
                }
            }
        }

        /// <author>Shuang Li & Xueyang Han</author>
        /// <summary>
        /// Send two strings simultaneously and check the order of receive is correct.
        /// Check payload in send callback have the same value as calling beginSend
        /// in the beginning. 
        /// </summary>
        [TestMethod]
        public void SendCallbackTest()
        {
            new TestSendCallback().run(4002);
        }

        /// <summary>
        /// Class used to run CarriageReturnText
        /// </summary>
        public class TestSendCallback
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private object p3;
            private object p4;

            // Timeout used in test case
            private static int timeout = 2000;

            /// <summary>
            /// run
            /// </summary>
            /// <param name="port"></param>
            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive((s, e, o) => { s1 = s; p1 = o; mre1.Set(); }, 1);
                    receiveSocket.BeginReceive((s, e, o) => { s2 = s; p2 = o; mre2.Set(); }, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    // NOTE: This is the line that has been changed
                    
                    sendSocket.BeginSend("Hello world\n", (e, o) => { p3 = o; mre1.Set(); }, 3);
                    sendSocket.BeginSend("This is a test\n", (e, o) => { p4 = o; mre2.Set(); }, 4);
                    Assert.AreEqual(3, p3);
                    Assert.AreEqual(4, p4);
                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }
        }

        /// <summary>
        /// Written by Christpher McAfee
        ///A simple test for BeginSend and BeginReceive, which calls BeginReceive 1000 times followed by BeginReceive 1000 times,
        ///making sure both match at the end.
        ///</summary>
        [TestMethod()]
        public void TestManyReceiveAndSend()
        {
            new Test1ClassCrist().run(4001);
        }

        public class Test1ClassCrist
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    //*** Test for many receive requests followed by many send requests, asserting that they both end at the same value
                    // Make two receive requests
                    for (int i = 0; i < 1000; i++)
                    {
                        receiveSocket.BeginReceive(CompletedReceive, i);
                    }

                    // Now send the data. Hope those receive requests didn't block!
                    for (int i = 0; i < 1000; i++)
                    {
                        String msg = i.ToString() + "/n";
                        foreach (char c in msg)
                        {
                            sendSocket.BeginSend(c.ToString(), (e, o) => { }, i);
                        }
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting");
                    Assert.AreEqual(999, s1);
                    Assert.AreEqual(999, p1);


                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }


        }



        /// <summary>
        /// Authors: Brant Nielsen and Janelle Michaelis
        /// 
        /// Tests multithreading functionality. Sets up BeginReceive callbacks on a client from multiple different
        /// concurrent threads, and sends multiple random messages to the client on different concurrent threads.
        /// Makes sure that all messages are received properly (though not in any particular order).
        /// </summary>
        [TestMethod]
        public void MultithreadingTest()
        {
            new MultithreadingClass().run(4000);
        }

        public class MultithreadingClass
        {

            private Random rng;
            private List<string> sentMessages;
            private StringSocket receiveSocket;
            private StringSocket sendSocket;

            private int messagesToSendPerThread = 100;
            private int randomMessageMinLength = 4;
            private int randomMessageMaxLength = 30;
            private int messagesSentCount;
            private object messagesSentCountLock;

            public void run(int port)
            {
                rng = new Random();
                sentMessages = new List<string>();
                messagesSentCount = 0;
                messagesSentCountLock = new object();

                TcpListener server = new TcpListener(IPAddress.Any, port);
                server.Start();
                TcpClient client = new TcpClient("localhost", port);

                // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                // method here, which is OK for a test case.
                Socket serverSocket = server.AcceptSocket();
                Socket clientSocket = client.Client;

                // Wrap the two ends of the connection into StringSockets
                sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                //Set up BeginReceive callbacks on 3 different threads
                Task waitForMessages1 = new Task(waitForExpectedMessages);
                Task waitForMessages2 = new Task(waitForExpectedMessages);
                Task waitForMessages3 = new Task(waitForExpectedMessages);

                waitForMessages1.Start();
                waitForMessages2.Start();
                waitForMessages3.Start();

                //Wait for all BeginReceive statements to be executed
                while (!(waitForMessages1.IsCompleted && waitForMessages2.IsCompleted && waitForMessages3.IsCompleted))
                {
                    Thread.Sleep(10);
                }

                //Send random messages on 3 different threads
                Task sendRandomMessages1 = new Task(sendRandomMessages);
                Task sendRandomMessages2 = new Task(sendRandomMessages);
                Task sendRandomMessages3 = new Task(sendRandomMessages);

                sendRandomMessages1.Start();
                sendRandomMessages2.Start();
                sendRandomMessages3.Start();

                //Wait for all BeginSend statements to be executed
                while (!(sendRandomMessages1.IsCompleted && sendRandomMessages2.IsCompleted && sendRandomMessages3.IsCompleted))
                {
                    Thread.Sleep(10);
                }

                //Ensure that the messages are given sufficient time to be sent (200ms to be safe)
                Thread.Sleep(200);

                //Ensure that all BeginSend callbacks were executed
                Assert.AreEqual(messagesToSendPerThread * 3, messagesSentCount);

                //Ensure that all messages that were sent were actually received (when a message is received, it's removed from the sentMessages list)
                Assert.AreEqual(0, sentMessages.Count);

            }

            /// <summary>
            /// Thread that will run BeginReceive calls on the receiving socket
            /// </summary>
            private void waitForExpectedMessages()
            {
                for (int i = 0; i < messagesToSendPerThread; i++)
                {
                    receiveSocket.BeginReceive(beginReceiveCallback, null);

                    //One in ten times, sleep for 5 milliseconds. This ensures that the threads will complete at varying speeds.
                    if (rng.Next(10) == 0)
                    {
                        Thread.Sleep(5);
                    }
                }
            }

            /// <summary>
            /// Callback for the BeginReceive method on the receiving socket. Will ensure that all received messages
            /// were actually sent, and will remove properly sent messages from the sentMessages list to help ensure
            /// that all sent messages were received. 
            /// </summary>
            private void beginReceiveCallback(String receivedString, Exception exception, object payload)
            {
                lock (sentMessages)
                {
                    //Was this message acutally sent? The method below will return -1 if it wasn't
                    int receivedStringIndex = sentMessages.IndexOf(receivedString);

                    Assert.AreNotEqual(-1, receivedStringIndex);

                    //Remove the message from the sentMessages list (will help us ensure that all sent messages were received)
                    sentMessages.RemoveAt(receivedStringIndex);
                }
            }

            /// <summary>
            /// Thread that will run BeginSend calls on the sending socket. Will send random messages to the client, and
            /// record that these messsages were sent using the sentMessages List.
            /// </summary>
            private void sendRandomMessages()
            {
                for (int i = 0; i < messagesToSendPerThread; i++)
                {
                    //Generate a random message
                    string message = generateRandomString(rng, randomMessageMinLength, randomMessageMaxLength);

                    //Add this message to our sentMessages list
                    lock (sentMessages)
                    {
                        sentMessages.Add(message);
                    }

                    //Sent the message using the sending string socket
                    sendSocket.BeginSend(message + "\n", (e, p) =>
                    {
                        //Make sure that the callback was actually executed. If there's a discrepency between the number of messages
                        //sent and the number of callbacks executed, we'll know some were not executed correctly.
                        lock (messagesSentCountLock)
                        {
                            messagesSentCount++;
                        }
                    }, null);

                    //One in ten times, sleep for 5 milliseconds. This ensures that the threads will complete at varying speeds.
                    if (rng.Next(10) == 0)
                    {
                        Thread.Sleep(5);
                    }
                }
            }

            /// <summary>
            /// Added as a private member so this doesn't have to be re-created on every call to generateRandomMessage
            /// </summary>
            private readonly string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            /// <summary>
            /// Generates a random alphanumeric string of minLength and maxLength (both inclusive) using the specified
            /// random number generator.
            /// </summary>
            private string generateRandomString(Random rng, int minLength, int maxLength)
            {
                int totalLength = rng.Next(minLength, maxLength + 1);

                StringBuilder finalString = new StringBuilder();

                for (int i = 1; i <= totalLength; i++)
                {
                    finalString.Append(rng.Next(alphanumericCharacters.Length));
                }

                return finalString.ToString();
            }

        }


        /// Andrew Van Tassell & Derek Johnson
        /// <summary>
        ///A simple test for BeginSend and BeginReceive
        ///</summary>
        [TestMethod()]
        public void Test2Derek()
        {
            new Test2ClassDerek().run(4001);
        }

        public class Test2ClassDerek
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String s1;
            private object p1;

            // Timeout used in test case
            private static int timeout = 10000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    // setup the necessary components to help build a random string
                    Random random = new Random();
                    StringBuilder sb = new StringBuilder();


                    // send 100 random strings with varying \n characters
                    for (int i = 0; i < 1000; i++)
                    {

                        int n1Distributor = random.Next(0, 10);

                        // create a very long random string with multiple randomly placed '\n' chars throughout
                        for (int j = 0; j < 1000; j++)
                        {
                            if (j / n1Distributor == 0)
                            {
                                sb.Append("\n");
                            }

                            sb.Append(Convert.ToChar(random.Next(0, 100)));
                        }

                        String msg = sb.ToString();

                        // Now send the data.  Hope the string socket can handle a large weird string

                        sendSocket.BeginSend(msg, (e, o) => { }, null);

                        // Make sure the messages were received properly.
                        Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting");
                        Assert.AreEqual(msg, s1);
                        Assert.AreEqual(1, p1);
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the SS_callback for the first receive request.  We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }
        }


        /// <summary>
        /// test case to check if an out of order send works and that the socket properly handles adjacent newlines
        /// based on original StringSocketTesterCode
        /// changes by marsh poulson
        /// </summary>
        [TestMethod()]
        public void Test2Again()
        {
            // Create and start a server and client.
            TcpListener server = null;
            TcpClient client = null;
            int port = 2000;
            int timeout = 2000;
            object p1 = null, p2 = null;
            string s1 = null, s2 = null;
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = new TcpClient("localhost", port);

                // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                // method here, which is OK for a test case.
                Socket serverSocket = server.AcceptSocket();
                Socket clientSocket = client.Client;

                // Wrap the two ends of the connection into StringSockets
                StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                // This will coordinate communication between the threads of the test cases
                var mre1 = new ManualResetEvent(false);
                var mre2 = new ManualResetEvent(false);

                // Make two receive requests
                receiveSocket.BeginReceive((s, e, payload) =>
                {
                    s1 = s;
                    p1 = payload;
                    mre1.Set();
                }, 1);
                receiveSocket.BeginReceive((s, e, payload) =>
                {
                    s2 = s;
                    p2 = payload;
                    mre2.Set();
                }, 2);

                // Now send the data. Hope those receive requests didn't block!
                String msg = "Hello\n\n\n\nThis is a test\n\n\n\n\n\n\n\n";


                sendSocket.BeginSend(msg, (e, p) => { }, null);

                // Make sure the lines were received properly.
                Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                Assert.AreEqual("Hello", s1);
                Assert.AreEqual(1, p1);

                Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                Assert.AreEqual("This is a test", s2);
                Assert.AreEqual(2, p2);
            }
            finally
            {
                server.Stop();
                client.Close();
            }
        }

        /// <summary>
        /// Authors: Braden Caywood & Matthew Adamson
        /// Based heavily off of skeleton test.
        ///A simple test for checking correct backslashes and carriage returns.
        ///</summary>
        [TestMethod()]
        public void TestMultipleSlashes()
        {
            new TestMultipleSlashesClass().run(5102);
        }

        public class TestMultipleSlashesClass
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "/d/e/a/c/e/y/h/1/4/2/5/@/$/r/(/+/=///d/n";
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("/d/e/a/c/e/y/h/1/4/2/5/@/$", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("/(/+/=///d", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }

        [TestMethod()]
        public void RegexFun()
        {
            new TestViaRegex().run(4001);
        }

        public class TestViaRegex
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private String pattern;
            private int[] payloads;
            private int payloadCounter;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);

                    // Make five receive requests, payloaded [0..4]
                    for (int i = 0; i < 5; i++)
                    {
                        receiveSocket.BeginReceive(CompilePattern, i);
                    }

                    // Lazy-ish init.
                    pattern = "";
                    payloads = new int[5];
                    payloadCounter = 0;

                    // Now send the data.
                    string[] message = new string[] { "((b)\n", "(1)\n", "(ary)\n", "(S)\n", "(0)(1)(0))\n" };
                    for (int i = 0; i < 5; i++)
                    {
                        sendSocket.BeginSend(message[i], (e, o) => { }, null);
                    }

                    string resultTest = "b1naryS010";

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual(true, Regex.IsMatch(resultTest, pattern, RegexOptions.IgnorePatternWhitespace));

                    // For redundancy, ensure the payloads arrived in order, for redundancy.
                    for (int i = 0; i < 5; i++)
                    {
                        Assert.AreEqual(i, payloads[i]);
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the receive request. It will add each received string
            // to a pattern used by regex to test a final string. If the pattern is received in
            // the appropriate order, it will match the test string.
            private void CompilePattern(String s, Exception o, object payload)
            {
                pattern += pattern;
                payloads[payloadCounter] = (int)payload;
                payloadCounter++;

                if (payloadCounter == 5)
                    mre1.Set();
            }
        }


        /// <summary>
        /// <author>Chengfei Shi, Zhi Wang</author>
        /// We place \n after each char we send in to test to the code, make sure the callback has return the right string without \n
        ///</summary>
        [TestMethod()]
        public void Test21()
        {
            new Test21case().run(4001);
        }

        /// <summary>

        /// 
        /// </summary>
        public class Test21case
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private int count = 0;
            private String[] output;
            private String callbackString;
            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    String msg = "This is a test data. It should return same result.";
                    count = msg.Length;
                    output = new string[count];
                    // Make many receive requests
                    for (int j = 0; j < count; j++)
                    {
                        receiveSocket.BeginReceive(CompletedReceive1, j);
                    }


                    //place \n after each char and send to stringSocket
                    int i = 0;
                    foreach (char c in msg)
                    {
                        sendSocket.BeginSend(c.ToString() + "\n", (e, o) => { }, null);
                        i++;
                    }

                    foreach (String str in output)
                    {
                        callbackString += str;
                    }

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("This is a test data. It should return same result.", callbackString);
                    //Assert.AreEqual(1, p1);


                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                int i;
                //this should be able to parse the object to i since all of the objects send in are int.
                int.TryParse(payload.ToString(), out i);
                output[i] = s;
                p1 = payload;
                mre1.Set();
            }


        }



        /// <summary>
        /// Authors: Jarom Norris and Sarah Cotner
        /// November 2014
        /// University of Utah CS 3500 with Dr. de St. Germain
        /// This is a simple test to make sure that string socket is written to be non-blocking,
        /// regardless of inappropriate callbacks. Uses the functions BlockingTestCallback1 and
        /// BlockingTestCallback2.
        /// </summary>
        [TestMethod()]
        public void JaromAndSarahNonBlockingTest()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 4002);
            server.Start();
            TcpClient client = new TcpClient("localhost", 4002);

            Socket serverSocket = server.AcceptSocket();
            Socket clientSocket = client.Client;

            StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
            StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

            sendSocket.BeginSend("This test wa", (e, p) => { }, 1);
            sendSocket.BeginSend("s made by\n", (e, p) => { }, 2);
            sendSocket.BeginSend("Jarom and Sarah!\n", (e, p) => { }, 3);

            receiveSocket.BeginReceive(BlockingTestCallback1, 4);
            receiveSocket.BeginReceive(BlockingTestCallback2, 5);
        }

        public void BlockingTestCallback1(string s, Exception e, object payload)
        {
            while (true)
                Thread.Sleep(500);
        }

        public void BlockingTestCallback2(string s, Exception e, object payload)
        {
            Assert.AreEqual(s, "Jarom and Sarah!");
            Assert.AreEqual(payload, 5);
        }





        ///<summary>

        /// Tests how the string sockect handles different sorts of Strings

        /// composed in different manners. Mashing Unix and Windows always

        /// ends up okay, right?

        /// 

        /// Michael Pregman and Alec Adair

        ///</summary>

        [TestMethod()]

        public void TestStringHandling()
        {

            new TestStringHandlingClass().run(4001);

        }



        public class TestStringHandlingClass
        {

            // Data that is shared across threads

            private ManualResetEvent mre1;

            private ManualResetEvent mre2;

            private ManualResetEvent mre3;

            private ManualResetEvent mre4;

            private String s1;

            private object p1;

            private String s2;

            private object p2;

            private String s3;

            private object p3;

            private String s4;

            private object p4;



            // Timeout used in test case

            private static int timeout = 2000;



            public void run(int port)
            {

                // Create and start a server and client.

                TcpListener server = null;

                TcpClient client = null;



                try
                {

                    server = new TcpListener(IPAddress.Any, port);

                    server.Start();

                    client = new TcpClient("localhost", port);



                    // Obtain the sockets from the two ends of the connection.  We are using the blocking AcceptSocket()

                    // method here, which is OK for a test case.

                    Socket serverSocket = server.AcceptSocket();

                    Socket clientSocket = client.Client;



                    // Wrap the two ends of the connection into StringSockets

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());



                    // This will coordinate communication between the threads of the test cases

                    mre1 = new ManualResetEvent(false);

                    mre2 = new ManualResetEvent(false);



                    // Make two receive requests

                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    receiveSocket.BeginReceive(CompletedReceive3, 3);



                    // Now send the data.  Hope those receive requests didn't block!

                    // May or may not include carriage return

                    String msg = "I scared for the\r\n3810 test tomorrow.\n\n";

                    foreach (char c in msg)
                    {

                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);

                    }



                    // Sent as the entire string and should still pick up on the new line

                    String msg2 = "This is an entire line!\n";

                    sendSocket.BeginSend(msg2, (e, o) => { }, null);



                    // Make sure the lines were received properly.

                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");

                    Assert.AreEqual("I scared for the", s1);

                    Assert.AreEqual(1, p1);



                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");

                    Assert.AreEqual("3810 test tomorrow.", s2);

                    Assert.AreEqual(2, p2);



                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");

                    Assert.AreEqual("", s3);

                    Assert.AreEqual(3, p3);



                    // Recieve next message.

                    receiveSocket.BeginReceive(CompletedReceive4, 4);



                    Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");

                    Assert.AreEqual("This is an entire line!", s4);

                    Assert.AreEqual(4, p4);

                }

                finally
                {

                    server.Stop();

                    client.Close();

                }

            }



            // This is the callback for the first receive request.  We can't make assertions anywhere

            // but the main thread, so we write the values to member variables so they can be tested

            // on the main thread.

            private void CompletedReceive1(String s, Exception o, object payload)
            {

                s1 = s;

                p1 = payload;

                mre1.Set();

            }



            // This is the callback for the second receive request.

            private void CompletedReceive2(String s, Exception o, object payload)
            {

                s2 = s;

                p2 = payload;

                mre2.Set();

            }



            // This is the callback for the third receive request.

            private void CompletedReceive3(String s, Exception o, object payload)
            {

                s3 = s;

                p3 = payload;

                mre3.Set();

            }



            // This is the callback for the third receive request.

            private void CompletedReceive4(String s, Exception o, object payload)
            {

                s4 = s;

                p4 = payload;

                mre4.Set();

            }

        }

        /// <summary>
        /// Authors: Caius Worthen and Candace Wilson
        /// 
        /// This test will be checking if the expected empty string is returned from a message that contains nothing but "\n\n\n\n"
        ///</summary>
        [TestMethod()]
        public void TestCaius()
        {
            new TestClassCaius().run(4001);
        }



        public class TestClassCaius
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private ManualResetEvent mre3;
            private ManualResetEvent mre4;
            private String s1;
            private object p1;
            private String s2;
            private object p2;
            private String s3;
            private object p3;
            private String s4;
            private object p4;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);
                    mre3 = new ManualResetEvent(false);
                    mre4 = new ManualResetEvent(false);

                    // Make four receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 1);
                    receiveSocket.BeginReceive(CompletedReceive3, 1);
                    receiveSocket.BeginReceive(CompletedReceive4, 1);

                    // Now send the data. Hope those receive requests didn't block!
                    sendSocket.BeginSend("\n\n\n\n", (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("", s2);
                    Assert.AreEqual(2, p2);

                    Assert.AreEqual(true, mre3.WaitOne(timeout), "Timed out waiting 3");
                    Assert.AreEqual("", s3);
                    Assert.AreEqual(3, p3);

                    Assert.AreEqual(true, mre4.WaitOne(timeout), "Timed out waiting 4");
                    Assert.AreEqual("", s4);
                    Assert.AreEqual(4, p4);

                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }

            // This is the callback for the third receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive3(String s, Exception o, object payload)
            {
                s3 = s;
                p3 = payload;
                mre3.Set();
            }

            // This is the callback for the fourth receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive4(String s, Exception o, object payload)
            {
                s4 = s;
                p4 = payload;
                mre4.Set();
            }
        }

        /// <author> Chris Weeter </author>

        /// <summary>

        /// Load tests the Socket by sending 100 messages

        /// </summary>

        [TestMethod()]

        public void LoadTest()
        {

            new LoadTest1().run(4001);

        }

        class LoadTest1
        {



            public void run(int port)
            {

                // Create and start a server and client.

                TcpListener server = null;

                TcpClient client = null;

                // Number of runs is set

                int runs = 100;

                string lastMessage = "100\n";





                server = new TcpListener(IPAddress.Any, port);

                server.Start();

                client = new TcpClient("localhost", port);





                // Obtain the sockets from the two ends of the connection.                  
                Socket serverSocket = server.AcceptSocket();

                Socket clientSocket = client.Client;





                // Wrap the two ends of the connection into StringSockets

                StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());



                // Puts in a receive request to the socket

                receiveSocket.BeginReceive(GetCallBack, null);



                // For loop to run the BeginSend method 100 times, incrementing the message sent each time

                for (int i = 0; i < runs; i++)
                {

                    sendSocket.BeginSend(i.ToString(), SendCallBack, null);

                }

                // Sends the final message with the newline character added on

                sendSocket.BeginSend(lastMessage, SendCallBack, null);

            }



            /// <summary>

            /// Gets the string that was sent over the socket

            /// </summary>

            /// <param name="s"> String message that should contain the message that was passed through the socket</param>

            /// <param name="e"> Exception that should be null</param>

            /// <param name="payload"> Payload that was passed </param>

            void GetCallBack(String s, Exception e, Object payload)
            {

                Assert.IsNotNull(s);

                Assert.AreEqual("0123456789101112131415161718192021222324252627282930313233343536373839404142434445464748495051525354555657585960616263646566676869707172737475767778798081828384858687888990919293949596979899100", s);

                Assert.IsNull(e);

                Assert.IsNull(payload);

            }



            /// <summary>

            /// Send Callback Method to ensure that an exception wasn't thrown after each BeginSend, and that the payload was passed correctly

            /// </summary>

            /// <param name="e">Exception should be null</param>

            /// <param name="payload">Payload should be null </param>

            void SendCallBack(Exception e, Object payload)
            {

                // Checks to make sure that there was no exception thrown

                Assert.IsNull(e);



                Assert.IsNull(payload);



            }



        }


        /// <summary>
        /// This is the test class for StringSocket.
        /// 
        /// By Feng Chen and Wei Zhao
        /// </summary>
        [TestClass()]
        public class Test
        {

            /// <summary>
            /// The recieving string 1. 
            /// </summary>
            private String rs1;

            /// <summary>
            /// The recieving string 2. 
            /// </summary>
            private String rs2;

            /// <summary>
            /// The recieving object 1. 
            /// </summary>
            private Object ro1;

            /// <summary>
            /// The recieving object 2. 
            /// </summary>
            private Object ro2;

            private ManualResetEvent mre1;
            private ManualResetEvent mre2;

            /// <summary>
            /// Call back method 1.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="o"></param>
            /// <param name="payload"></param>
            private void Callback1(String s, Exception o, object payload)
            {
                rs1 = s;
                ro1 = payload;
                mre1.Set();
            }

            /// <summary>
            /// Call back method 2k.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="o"></param>
            /// <param name="payload"></param>
            private void Callback2(String s, Exception o, object payload)
            {
                rs2 = s;
                ro2 = payload;
                mre2.Set();
            }

            /// <summary>
            /// The test method for string socket, sending random strings and repeating 1000 times.
            /// </summary>
            [TestMethod()]
            public void Test1000()
            {

                Random r = new Random();

                // repeating loop
                for (int i = 0; i < 1000; i++)
                {
                    TcpListener server = null;
                    TcpClient client = null;
                    try
                    {
                        server = new TcpListener(IPAddress.Any, 4001);
                        server.Start();
                        client = new TcpClient("localhost", 4001);

                        StringSocket sendSocket = new StringSocket(server.AcceptSocket(), new UTF8Encoding());
                        StringSocket receiveSocket = new StringSocket(client.Client, new UTF8Encoding());

                        mre1 = new ManualResetEvent(false);
                        mre2 = new ManualResetEvent(false);

                        // use random string to send
                        int min1 = r.Next(5);
                        int max1 = min1 + r.Next();
                        String ss1 = generateString(min1, max1);

                        // object to send
                        int so1 = r.Next();

                        // use second random string to send
                        int min2 = r.Next(5);
                        int max2 = min2 + r.Next();
                        String ss2 = generateString(min1, max1);

                        // another object to send
                        int so2 = r.Next();

                        receiveSocket.BeginReceive(Callback1, so1);
                        receiveSocket.BeginReceive(Callback2, so2);

                        String msg = ss1 + "\n" + ss2 + "\n";
                        foreach (char c in msg)
                            sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);

                        Assert.IsTrue(mre1.WaitOne(2000));
                        Assert.AreEqual(ss1, rs1);
                        Assert.AreEqual(so1, ro1);

                        Assert.IsTrue(mre2.WaitOne(2000));
                        Assert.AreEqual(ss2, rs2);
                        Assert.AreEqual(so2, ro2);
                    }
                    finally
                    {
                        server.Stop();
                        client.Close();
                    }
                }
            }

            /// <summary>
            /// This method generate a string to send.
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            private static string generateString(int min, int max)
            {
                string s = "";

                Random r = new Random();
                int length = min + r.Next(max - min);
                for (int i = 0; i < length; i++)
                    s += (char)r.Next(128);

                return s;
            }
        }



        /// <summary>
        /// Damir Verkic and Brent Bagley
        /// November 12, 2014.
        /// 
        /// NOTE: Our implementation of the StringSocket class is not complete, so this test case may be faulty.
        /// 
        /// A test which sends multiple strings of the alphabet. The order of strings sent are:
        /// "A\nAB\nABC\nABCD\nABCDE\n.......ABCDEFGHIJKLMNOPQRSTUVWXYZ\n"
        /// </summary>
        [TestMethod()]
        public void TestCaseFor3500()
        {
            new TestCase().run(4111);
        }

        /// <summary>
        /// The testCase class containing the run method.
        /// </summary>
        public class TestCase
        {
            private ManualResetEvent mre1;

            /// Instance variables to build the string to send.
            private String stringToSend;
            private StringBuilder stringToBuild;

            /// String array to hold the received strings.
            private String[] stringsToReceive;


            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                // Instantiate the variables needed to build the strings.
                stringToSend = "";
                stringToBuild = new StringBuilder("");
                stringsToReceive = new String[26];

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This for loop starts at 65 (ascii value for A) and stops at 90 (ascii value for Z).
                    for (int i = 65; i <= 90; i++)
                    {
                        // Invoke the Callback method. First time calling this method will
                        // set the payload to zero and increment up to 26.
                        receiveSocket.BeginReceive(Callback, i - 65);

                        // Create the string.
                        stringToBuild.Append((char)i);
                        stringToSend += stringToBuild.ToString() + "\n";

                        // Begin sending the string.
                        sendSocket.BeginSend(stringToSend, (e, o) => { }, null);

                        mre1.Reset();

                        Assert.AreEqual(stringToSend, stringsToReceive[i]);
                    }
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            /// <summary>
            /// The callback for recieve requests, uses the payload to place the lines in a string array.
            /// </summary>
            private void Callback(string s, Exception e, object payload)
            {
                stringsToReceive[(int)payload] = s;
                mre1.Set();
            }
        }

        /// <summary>
        ///A simple test for BeginSend and BeginReceive based heavily on the test case provided by Dr. Jim

        ///Tests for proper handling of /n and /r as well as testing for incoming bytes when there is no callback

        ///</summary>
        [TestMethod()]
        public void Test1Jim()
        {
            new Test1ClassJim().run(4001);
        }

        public class Test1ClassJim
        {
            // Data that is shared across threads
            private ManualResetEvent mre1;
            private ManualResetEvent mre2;
            private String s1;
            private object p1;
            private String s2;
            private object p2;

            // Timeout used in test case
            private static int timeout = 2000;

            public void run(int port)
            {
                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    client = new TcpClient("localhost", port);

                    // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                    // method here, which is OK for a test case.
                    Socket serverSocket = server.AcceptSocket();
                    Socket clientSocket = client.Client;

                    // Wrap the two ends of the connection into StringSockets
                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

                    // This will coordinate communication between the threads of the test cases
                    mre1 = new ManualResetEvent(false);
                    mre2 = new ManualResetEvent(false);

                    // Make two receive requests
                    receiveSocket.BeginReceive(CompletedReceive1, 1);
                    receiveSocket.BeginReceive(CompletedReceive2, 2);

                    // Now send the data. Hope those receive requests didn't block!
                    String msg = "Hello world\nThis is a test\rThis should be ignored because no callback associated with it\n";


                    sendSocket.BeginSend(msg, (e, o) => { }, null);

                    // Make sure the lines were received properly.
                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
                    Assert.AreEqual("Hello world", s1);
                    Assert.AreEqual(1, p1);

                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
                    Assert.AreEqual("This is a test", s2);
                    Assert.AreEqual(2, p2);
                }
                finally
                {
                    server.Stop();
                    client.Close();
                }
            }

            // This is the callback for the first receive request. We can't make assertions anywhere
            // but the main thread, so we write the values to member variables so they can be tested
            // on the main thread.
            private void CompletedReceive1(String s, Exception o, object payload)
            {
                s1 = s;
                p1 = payload;
                mre1.Set();
            }

            // This is the callback for the second receive request.
            private void CompletedReceive2(String s, Exception o, object payload)
            {
                s2 = s;
                p2 = payload;
                mre2.Set();
            }
        }



        /// <summary>

        ///Created by Camille Humphries

        ///

        ///This tests sending a string with a few newlines in it

        ///</summary>

        [TestMethod()]

        public void Test1Humphries()
        {

            new Test1ClassHumphries().run(4001);

        }



        public class Test1ClassHumphries
        {

            // Data that is shared across threads

            private ManualResetEvent mre1;

            private ManualResetEvent mre2;

            private String s1;

            private object p1;

            private String s2;

            private object p2;



            // Timeout used in test case

            private static int timeout = 2000;



            public void run(int port)
            {

                // Create and start a server and client.

                TcpListener server = null;

                TcpClient client = null;



                try
                {

                    server = new TcpListener(IPAddress.Any, port);

                    server.Start();

                    client = new TcpClient("localhost", port);



                    // Obtain the sockets from the two ends of the connection.

                    Socket serverSocket = server.AcceptSocket();

                    Socket clientSocket = client.Client;



                    // Wrap the two ends of the connection into StringSockets

                    StringSocket sendSocket = new StringSocket(serverSocket, new UTF8Encoding());

                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());



                    // This will coordinate communication between the threads of the test cases

                    mre1 = new ManualResetEvent(false);

                    mre2 = new ManualResetEvent(false);



                    // Make two receive requests

                    receiveSocket.BeginReceive(CompletedReceive1, 1);

                    receiveSocket.BeginReceive(CompletedReceive2, 2);



                    // Now send the data.  Hope those receive requests didn't block!

                    String msg = "Hi there\nTesting Testing\n";

                    foreach (char c in msg)
                    {

                        sendSocket.BeginSend(c.ToString(), (e, o) => { }, null);

                    }



                    // Make sure the lines were received properly.

                    Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");

                    Assert.AreEqual("Hi there", s1);

                    Assert.AreEqual(1, p1);



                    Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");

                    Assert.AreEqual("Testing Testing", s2);

                    Assert.AreEqual(2, p2);

                }

                finally
                {

                    server.Stop();

                    client.Close();

                }

            }



            // Callback for the first receive request.

            private void CompletedReceive1(String s, Exception o, object payload)
            {

                s1 = s;

                p1 = payload;

                mre1.Set();

            }



            // Callback for the second receive request.

            private void CompletedReceive2(String s, Exception o, object payload)
            {

                s2 = s;

                p2 = payload;

                mre2.Set();

            }

        }



        /// <summary>
        /// Tests that the receive callback doesn't block
        /// </summary>
        [TestMethod()]
        public void Test71()
        {
            int timeout = 2000;
            StringSocket sendSocket;
            StringSocket receiveSocket;
            string s1 = "";
            int p1 = 0;
            string s2 = "";
            int p2 = 0;

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);

            setupServerClient(4001, out sendSocket, out receiveSocket);

            receiveSocket.BeginReceive((s, e, p) => { s1 = s; p1 = (int)p; mre1.Set(); Thread.Sleep(10000); }, 1);
            receiveSocket.BeginReceive((s, e, p) => { s2 = s; p2 = (int)p; mre2.Set(); }, 2);
            sendSocket.BeginSend("Hello\nWorld\n", (e, p) => { }, null);

            Assert.AreEqual(true, mre1.WaitOne(timeout), "Timed out waiting 1");
            Assert.AreEqual("Hello", s1);
            Assert.AreEqual(1, p1);

            //if the StringSocket doesn't run the callback on its own thread, this assert will fail
            Assert.AreEqual(true, mre2.WaitOne(timeout), "Timed out waiting 2");
            Assert.AreEqual("World", s2);
            Assert.AreEqual(2, p2);

        }

        public void setupServerClientAgain(int port, out StringSocket sendSocket, out StringSocket receiveSocket)
        {
            // Create and start a server and client.
            TcpListener server = null;
            TcpClient client = null;
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            client = new TcpClient("localhost", port);

            // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
            // method here, which is OK for a test case.
            Socket serverSocket = server.AcceptSocket();
            Socket clientSocket = client.Client;

            // Wrap the two ends of the connection into StringSockets
            sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
            receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());

        }

        /// <summary>

        /// Authors: Samuel Davidson and Elliot Hatch
        /// Tests that you can handle sending and receive 1000 messages.
        /// </summary>
        [TestMethod()]
        public void TestGnarly()
        {
            new TestGnarlyClass().run(4001);
        }
        public class TestGnarlyClass
        {
            int timeout = 2000;
            StringSocket sendSocket;
            StringSocket receiveSocket;
            // Messages Recieved
            int messagesReceived = 0;
            int messagesSent = 0;

            public void run(int port)
            {


                // Create and start a server and client.
                TcpListener server = null;
                TcpClient client = null;
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = new TcpClient("localhost", port);

                // Obtain the sockets from the two ends of the connection. We are using the blocking AcceptSocket()
                // method here, which is OK for a test case.
                Socket serverSocket = server.AcceptSocket();
                Socket clientSocket = client.Client;

                // Wrap the two ends of the connection into StringSockets
                sendSocket = new StringSocket(serverSocket, new UTF8Encoding());
                receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());


                for (int x = 0; x < 1000; x++)
                {
                    receiveSocket.BeginReceive(MessageReceivedCount, x);
                }
                for (int x = 0; x < 1000; x++)
                {
                    sendSocket.BeginSend("Message: " + x + "\n", MessageSentCount, x);
                }

            }

            private void MessageReceivedCount(String s, Exception o, object payload)
            {
                lock (this)
                {
                    messagesReceived = messagesReceived + 1;
                    if (messagesReceived == 1000)
                    {
                        runAssertions();
                    }
                }

            }
            private void MessageSentCount(Exception o, object payload)
            {
                lock (this)
                {
                    messagesSent = messagesSent + 1;
                }
            }

            private void runAssertions()
            {
                Assert.AreEqual(1000, messagesSent, "Assert that the 1000 messages sent were sent");
                Assert.AreEqual(1000, messagesReceived, "Assert that the 1000 messages sent were recieved");
            }
        }


    }

}
