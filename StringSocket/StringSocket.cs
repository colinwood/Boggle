using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace CustomNetworking
{
   /// <summary>
   /// A StringSocket is a wrapper around a Socket.  It provides methods that
   /// asynchronously read lines of text (strings terminated by newlines) and
   /// write strings. (As opposed to Sockets, which read and write raw bytes.)
   ///
   /// StringSockets are thread safe.  This means that two or more threads may
   /// invoke methods on a shared StringSocket without restriction.  The
   /// StringSocket takes care of the synchonization.
   ///
   /// Each StringSocket contains a Socket object that is provided by the client.
   /// A StringSocket will work properly only if the client refrains from calling
   /// the contained Socket's read and write methods.
   ///
   /// If we have an open Socket s, we can create a StringSocket by doing
   ///
   ///    StringSocket ss = new StringSocket(s, new UTF8Encoding());
   ///
   /// We can write a string to the StringSocket by doing
   ///
   ///    ss.BeginSend("Hello world", callback, payload);
   ///
   /// where callback is a SendCallback (see below) and payload is an arbitrary object.
   /// This is a non-blocking, asynchronous operation.  When the StringSocket has
   /// successfully written the string to the underlying Socket, or failed in the
   /// attempt, it invokes the callback.  The parameters to the callback are a
   /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
   /// the Exception that caused the send attempt to fail.
   ///
   /// We can read a string from the StringSocket by doing
   ///
   ///     ss.BeginReceive(callback, payload)
   ///
   /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
   /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
   /// string of text terminated by a newline character from the underlying Socket, or
   /// failed in the attempt, it invokes the callback.  The parameters to the callback are
   /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
   /// string or the Exception will be non-null, but nor both.  If the string is non-null,
   /// it is the requested string (with the newline removed).  If the Exception is non-null,
   /// it is the Exception that caused the send attempt to fail.
   /// </summary>

   public class StringSocket
   {
       // These delegates describe the callbacks that are used for sending and receiving strings.
       public delegate void SendCallback(Exception e, object payload);
       public delegate void ReceiveCallback(String s, Exception e, object payload);

       /// <summary>
       /// Queue that holds the received strings
       /// </summary>
       private Queue<string> messages_queue;

       /// <summary>
       /// Outgoing and incoming queues that2
       /// </summary>
       private Queue<Send_Request> send_queue;
       private Queue<Receive_Request> receive_queue;


       /// <summary>
       /// UNderlying socket that the strigns will actually be sent over.
       /// </summary>
       private Socket socket { get; set; }

       /// <summary>
       /// The encoding being use such as ascii or utf8
       /// </summary>
       private Encoding encoding;

       /// <summary>
       /// A string that holds the message being received until we see a new line character at which point 
       /// this message is placed in the messages queue. 
       /// </summary>
       private string message_received;


       /// <summary>
       /// Creates a StringSocket from a regular Socket, which should already be connected.
       /// The read and write methods of the regular Socket must not be called after the
       /// LineSocket is created.  Otherwise, the StringSocket will not behave properly.
       /// The encoding to use to convert between raw bytes and strings is also provided.
       /// </summary>
       public StringSocket(Socket s, Encoding e)
       {
           this.socket = s;
           this.encoding = e;
           this.send_queue = new Queue<Send_Request>();
           this.receive_queue = new Queue<Receive_Request>();
           this.messages_queue = new Queue<string>();
       }

       public bool SocketConnected()
       {
           Socket s = this.socket;
           try { 
           bool part1 = s.Poll(1000, SelectMode.SelectRead);
           bool part2 = (s.Available == 0);
           if (part1 && part2)
               return false;
           }
           catch (Exception e)
           {
                
           }
           return true;
              
       }
       /// <summary>
       /// Nested class that holds an individual requests outgoing text, callback, and 
       /// payload. 
       /// </summary>
       public class Send_Request
       {
           public string s { get; set; }
           public SendCallback callback { get; set; }
           public object payload { get; set; }

           public Send_Request(string s, SendCallback callback, object payload)
           {
               this.s = s;
               this.callback = callback;
               this.payload = payload;

           }
       }

       /// <summary>
       /// Nested class that holds a receieve requests callback and payload. 
       /// </summary>
       public class Receive_Request
       {
           public ReceiveCallback callback { get; set; }
           public object payload { get; set; }

           public Receive_Request(ReceiveCallback callback, object payload)
           {
               this.callback = callback;
               this.payload = payload;
           }
       }

       /// <summary>
       /// We can write a string to a StringSocket ss by doing
       ///
       ///    ss.BeginSend("Hello world", callback, payload);
       ///
       /// where callback is a SendCallback (see below) and payload is an arbitrary object.
       /// This is a non-blocking, asynchronous operation.  When the StringSocket has
       /// successfully written the string to the underlying Socket, or failed in the
       /// attempt, it invokes the callback.  The parameters to the callback are a
       /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
       /// the Exception that caused the send attempt to fail.
       ///
       /// This method is non-blocking.  This means that it does not wait until the string
       /// has been sent before returning.  Instead, it arranges for the string to be sent
       /// and then returns.  When the send is completed (at some time in the future), the
       /// callback is called on another thread.
       ///
       /// This method is thread safe.  This means that multiple threads can call BeginSend
       /// on a shared socket without worrying around synchronization.  The implementation of
       /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
       /// string arriving via a BeginSend method call must be sent (in its entirety) before
       /// a later arriving string can be sent.
       /// </summary>
       public void BeginSend(String s, SendCallback callback, object payload)
       {
           // Ensure the method is non blcoking by only allowing one thread past here at a time. 
           lock (send_queue)
           {
               //Place the next message in the queue to sent out. 
               send_queue.Enqueue(new Send_Request(s, callback, payload));
               if (send_queue.Count == 1)
               {
                   //call the helper that goes through the requests in the queue
                   SendBytes();
               }
           }
       }
       /// <summary>
       /// Utilizes the queue data structure to send the oldest request out first. 
       /// 
       /// Attempt to send the bytes over the underlying socket. If for some reason 
       /// the socket is closed or there are no bytes to be sent throws an exception
       /// but guarantees that the call back is queued up and called by the OS
       /// </summary>
       private void SendBytes()
       {

           // added a while loop that checks if there is something in the queue
           while (send_queue.Count != 0)
           {
               Send_Request next_request = send_queue.Peek(); //Get the next request to be sent but leave it on the queue
               byte[] outgoing_bytes = encoding.GetBytes(next_request.s); //Convert the send requests string to bytes

               //attempt to send out the oldest request if successful break out of the loop else dequeue the request and 
               //fire up a thread for its call back. 
               try
               {
                   socket.BeginSend(outgoing_bytes, 0, outgoing_bytes.Length, SocketFlags.None, SCallback, outgoing_bytes);
                   break;
               }
               catch (Exception e)
               {
                   Send_Request request = send_queue.Dequeue();
                   ThreadPool.QueueUserWorkItem(x => request.callback(e, request.payload));
               }
           }
       }
       /// <summary>
       /// Called once a message is succefully sent. ensures that all desired bytes have been sent otherwise
       /// uses the underlying socket to send out the message again. 
       /// </summary>
       /// <param name="result">Standard callback parameter</param>
       private void SCallback(IAsyncResult result)
       {
           //Initalize it to 0 everytime the callback is called to ensure 
           //it must be set in order to reflect some number of bytes being recieved. 
           int sent_bytes = 0;
           byte[] total_bytes = (byte[])result.AsyncState;

           try
           {
               //Get the number of bytes actually sent out
               sent_bytes = socket.EndSend(result);
           }
           catch (Exception e)
           {
            
               Send_Request request = send_queue.Dequeue();
               ThreadPool.QueueUserWorkItem(x => request.callback( e,request.payload));
           }

           //Fail! We need to find out the remaining bytes and send them
           if (total_bytes.Length != sent_bytes)
           {
               try
               {
                   socket.BeginSend(total_bytes, sent_bytes, total_bytes.Length - sent_bytes, SocketFlags.None, SCallback, total_bytes);
               }
               catch (Exception e)
               {
                   SendHelper();
                   return;
               }
           }
           //Success! Entire message was sent so now we only need make the callback
           else
           {
               lock (send_queue)
               {
                   SendHelper();
               }  
           }
       }

       /// <summary>
       /// Private helper method that dequeues the request the request that was sent and makes a thread for the callback
       /// </summary>
       private void SendHelper()
       {
           Send_Request next_request = send_queue.Dequeue();
           ThreadPool.QueueUserWorkItem(x => next_request.callback(null, next_request.payload));
           SendBytes();
       }




       /// <summary>
       ///
       /// <para>
       /// We can read a string from the StringSocket by doing
       /// </para>
       ///
       /// <para>
       ///     ss.BeginReceive(callback, payload)
       /// </para>
       ///
       /// <para>
       /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
       /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
       /// string of text terminated by a newline character from the underlying Socket, or
       /// failed in the attempt, it invokes the callback.  The parameters to the callback are
       /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
       /// string or the Exception will be non-null, but nor both.  If the string is non-null,
       /// it is the requested string (with the newline removed).  If the Exception is non-null,
       /// it is the Exception that caused the send attempt to fail.
       /// </para>
       ///
       /// <para>
       /// This method is non-blocking.  This means that it does not wait until a line of text
       /// has been received before returning.  Instead, it arranges for a line to be received
       /// and then returns.  When the line is actually received (at some time in the future), the
       /// callback is called on another thread.
       /// </para>
       ///
       /// <para>
       /// This method is thread safe.  This means that multiple threads can call BeginReceive
       /// on a shared socket without worrying around synchronization.  The implementation of
       /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
       /// arriving line of text must be passed to callbacks in the order in which the corresponding
       /// BeginReceive call arrived.
       /// </para>
       ///
       /// <para>
       /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
       /// even when there are no pending callbacks.  StringSocket implementations should refrain
       /// from buffering an unbounded number of incoming bytes beyond what is required to service
       /// the pending callbacks.
       /// </para>
       ///
       /// <param name="callback"> The function to call upon receiving the data</param>
       /// <param name="payload">
       /// The payload is "remembered" so that when the callback is invoked, it can be associated
       /// with a specific Begin Receiver....
       /// </param>
       ///
       /// <example>
       ///   Here is how you might use this code:
       ///   <code>
       ///                    client = new TcpClient("localhost", port);
       ///                    Socket       clientSocket = client.Client;
       ///                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());
       ///                    receiveSocket.BeginReceive(CompletedReceive1, 1);
       ///
       ///   </code>
       /// </example>
       /// </summary>
       ///
       ///

       public void BeginReceive(ReceiveCallback callback, object payload)
       {
           // ensure it is non blocking by only allowing one thread past here
           lock (receive_queue)
           {
               //place the new request in the queue to be processed
               receive_queue.Enqueue(new Receive_Request(callback, payload));
               if (receive_queue.Count == 1)
               {
                   GetBytes();
               }
           }
       }


       /// <summary>
       /// Helper method used to manage the receive_queue. Locks the queue and then checks to 
       /// see if there are any receive requests or messages needing to be dealt with. 
       /// </summary>
       private void GetBytes()
       {
           //ensure the method is non blocking by locking the data and only allowing one thread in at a time.
           lock (receive_queue)
           {
               //Loop that executes when there are messages in the message queue
               while (receive_queue.Count > 0 && messages_queue.Count > 0)
               {
                   //Utilze the queue data structure and prepare the next request to have its callback called.
                   String next_message = messages_queue.Dequeue();
                   Receive_Request next_request = receive_queue.Dequeue();

                   //Call the callback for each of the new line delimited messages
                   ThreadPool.QueueUserWorkItem(x => next_request.callback(next_message, null, next_request.payload));
               }

               while (receive_queue.Count > 0 )
               {
                   //create an empty buffer to store the received bytes into. 
                   byte[] receive_buffer = new byte[1024];
                   try
                   { 
                       socket.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None, RCallback, receive_buffer);
                       break;
                   }
                   catch (Exception e)
                   {
                       
                       //call that requests callback and dequeue it
                       Receive_Request next_request = receive_queue.Dequeue();                  
                       ThreadPool.QueueUserWorkItem(x => next_request.callback("", e, next_request.payload));               
                   }
               }
             }

       }

       private void RCallback(IAsyncResult result)
       {
           //Create a place to hold a value for the number of bytes received 
           //Initalize it to 0 everytime the callback is called to ensure we dont have data overlap
           int bytes_received = 0;

           //cast the result and get the recieved bytes
           byte[] incoming_buffer = (byte[])result.AsyncState;

           //Get the number of bytes acutally received. 
           try
           {
               bytes_received = socket.EndReceive(result);

           }
           catch (Exception e)
           {
               Receive_Request request = receive_queue.Dequeue();
               ThreadPool.QueueUserWorkItem(x => request.callback("", e,request.payload));
           }
           message_received = message_received + encoding.GetString(incoming_buffer, 0, bytes_received);

           //gets the index of the new line character and if it is not within the message set it to -1
           int newline_index = message_received.IndexOf('\n');

           //loop that checks if a new line character is in the message each time
           //iterates over the message and for each message that contains a new line character 
           //at the end places it into the messages queue
           while ( newline_index != -1)
           {     
               string next_message = message_received.Substring(0, newline_index);
               messages_queue.Enqueue(next_message);
               message_received = message_received.Substring(newline_index + 1);

               //update the new line index because that is when a new message starts per the specss. 
               newline_index = message_received.IndexOf('\n');
           }

           //Go try and process the receive queue
           GetBytes();

       }

       /// <summary>
       /// Calling the close method will close the String Socket (and the underlying
       /// standard socket).  The close method  should make sure all
       ///
       /// Note: ideally the close method should make sure all pending data is sent
       ///
       /// Note: closing the socket should discard any remaining messages and
       ///       disable receiving new messages
       ///
       /// Note: Make sure to shutdown the socket before closing it.
       ///
       /// Note: the socket should not be used after closing.
       /// </summary>
       public void Close()
       {
           while (true)
           {
               if (send_queue.Count == 0)
               {
                   try
                   {
                       //check to see if the socket is connected and if it is then shit it down and disconnect. 
                       if (socket.Connected)
                       {
                           socket.Shutdown(SocketShutdown.Both);
                           socket.Close();
                       }
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine(e.Message);
                   }
                   break;
               }
           }
       }
   }
}