using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ClientSocket();
            if (client.Connect("127.0.0.1", 9696))
            {

            }
            else
                Console.WriteLine("Fail to connect.");

            byte[] body = Encoding.Default.GetBytes("ddkdkdkdkkddkdkdk");
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(body.Length)); // Body Length
            bytes.AddRange(BitConverter.GetBytes(2)); // Route
            bytes.AddRange(BitConverter.GetBytes(1)); // ResultCode
            bytes.AddRange(body); // Body

            client.Write(bytes.ToArray());

            client.Close();
        }
    }
}
