using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// This code from https://github.com/jacking75/conf_kgc2016_SuperSocket/blob/master/BinaryPacketClient/ClientSocket.cs
namespace DemoClient
{
    public class ClientSocket
    {
        public Socket socket = null;

        public bool Connect(string ip, int port)
        {
            IPAddress serverIP = IPAddress.Parse(ip);
            int serverPort = port;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(serverIP, serverPort));

            if (socket == null || socket.Connected == false)
            {
                return false;
            }

            return true;
        }

        //스트림에서 읽어오기(소켓 연결확인은 버튼을 누르면...! form 에서...)
        public Tuple<int, byte[]> Read()
        {
            byte[] getbyte = new byte[4096];
            var nRecv = socket.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
            return new Tuple<int, byte[]>(nRecv, getbyte);
        }

        //스트림에 쓰기
        public void Write(byte[] senddata)
        {
            socket.Send(senddata, 0, senddata.Length, SocketFlags.None);
        }

        //소켓과 스트림 닫기
        public void Close()
        {
            socket.Close();
        }
    }
}
