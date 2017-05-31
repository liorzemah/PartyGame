using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Test_Game
{
    public class Client
    {
        private TcpClient client;

        public Client(string ip, int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                new Thread(Chat).Start();
                new Thread(Recieve).Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Recieve()
        {

            while (true)
            {

                try
                {

                    NetworkStream netStream = client.GetStream();
                    byte[] bytes = new byte[1024];
                    netStream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.ASCII.GetString(bytes);
                    string message = data.Substring(0, data.IndexOf("\0"));
                    Console.WriteLine("<Server>: " + message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Chat()
        {

            while (true)
            {

                string message = Console.ReadLine();
                SendTo(client, message);
                Console.WriteLine("<Client>: " + message);
            }
        }

        public void SendTo(TcpClient client, string message)
        {

            try
            {
                NetworkStream netStream = client.GetStream();
                byte[] bytes = Encoding.ASCII.GetBytes(message);
                netStream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            MemoryStream fs = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, obj);
            byte[] rval = fs.ToArray();
            fs.Close();
            return rval;
        }

        public object ByteArrayToObject(byte[] Buffer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(Buffer);
            object rval = null;
            try
            {
                rval = formatter.Deserialize(stream);
            }
            catch
            {
                rval = null;
            }
            stream.Close();
            return rval;
        }

    }
}
