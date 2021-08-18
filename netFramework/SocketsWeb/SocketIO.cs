using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoomBang.Forms;
using BoomBang.game.instances;
using BoomBang.game.manager;

namespace BoomBang.SocketsWeb
{
    class SocketIO
    {
        public static Socket WebSocket = null;
        private static bool ThreadReceivData = true;
        public static void Initialize()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3300));
            socket.Listen(0);
            Emulator.Form.WriteLine("WebSocket connection open (Port: 3300)", "success");

            var client = socket.Accept();
            WebSocket = client;

            while (ThreadReceivData)
            {
                reciveData(client);
            }
            /*client.Close();
            socket.Close();*/
        }

        public static void sendData(Socket client, string idType, string parameters)
        {
            if (Emulator.WebSockets)
            {
                var buffer = Encoding.UTF8.GetBytes(idType + "|" + parameters);
                client.Send(buffer, 0, buffer.Length, 0);
            }
        }

        private static void reciveData(Socket client)
        {
            try
            {
                var buffer = new byte[255];
                int rec = client.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);

                string idType = getIdType(Encoding.UTF8.GetString(buffer));
                string[] parameters = getParameters(Encoding.UTF8.GetString(buffer));
                try
                {
                    callPackage(session(parameters[0]), idType, parameters);
                }
                catch
                {
                    callPackage(null, idType, parameters);
                }
            }
            catch
            {
                Emulator.Form.WriteLine("Node is closed.", "error");
                ThreadReceivData = false; 
                return;
            }
         
        }

        private static SessionInstance session (string token_uid)
        {
            foreach(SessionInstance session in UserManager.UsuariosOnline.Values.ToList())
            {
                if (session.User.token_uid == token_uid)
                {
                    return session;
                }
            }
            return null;
        }

        private static string getIdType(string buffer)
        {
            string[] data = buffer.Split('|');
            return data[0];
        }

        private static string [] getParameters(string buffer)
        {
            string[] data = buffer.Split('|');
            string[] parameters = data[1].Split(',');
            return parameters;
        }

        private static void callPackage(SessionInstance session, string idType, string[] parameters)
        {
            if (session != null)
            {
                switch (idType)
                {
                    case "yellow-alert":
                        CallPackage.alert(session);
                        break;
                    case "change-ninja":
                        CallPackage.changeNinja(session, parameters);
                        break;
                    case "change-ficha-user":
                        CallPackage.changeFichaUser(session, parameters);
                        break;
                }
            }
        }
    }
}
