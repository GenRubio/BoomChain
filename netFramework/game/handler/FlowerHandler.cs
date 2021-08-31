using BoomBang.game.instances;
using BoomBang.game.instances.manager;
using BoomBang.game.manager;
using BoomBang.server;
using BoomBang.SocketsWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    class FlowerHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(120133, handler_120133);
            HandlerManager.RegisterHandler(120141, handler_120141);
            HandlerManager.RegisterHandler(120134, handler_120134);//Concursos
            HandlerManager.RegisterHandler(210120, handler_210120);
            HandlerManager.RegisterHandler(120147120, loadFlowerInterfaz);//Regalo Grande
            HandlerManager.RegisterHandler(132120, new ProcessHandler(Method_132_120));
            HandlerManager.RegisterHandler(132121, new ProcessHandler(Method_132_121));
            HandlerManager.RegisterHandler(120143, new ProcessHandler(Method_120_143));
        }
        private static void Method_120_143(SessionInstance Session, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(143);
            server.AppendParameter(0);
            Session.SendData(server);
        }
        private static void Method_132_121(SessionInstance Session, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(132);
            server.AddHead(121);
            server.AppendParameter(0);
            Session.SendData(server);
        }
        private static void Method_132_120(SessionInstance Session, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(132);
            server.AddHead(120);
            server.AppendParameter(0);
            Session.SendData(server);
        }

        private static void loadFlowerInterfaz(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null
                && Session.User.Sala == null)
            {
                SocketIO.sendData(SocketIO.WebSocket, "entrarFlowerPower", Session.User.token_uid);
            }
        }
      
        
        private static void handler_210120(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                 Packet_210_120(Session);
            }
        }
        private static void handler_120134(SessionInstance Session, string[,] Parameters)///Concurso?
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                 Packet_120_134(Session);
            }
        }
        private static void handler_120141(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                 Packet_120_141(Session);
            }
        }
        private static void Packet_210_120(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(210);
            server.AddHead(120);
            server.AppendParameter(new object[] { 1, 20, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 2, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 });
            server.AppendParameter(new object[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 });
            Session.SendDataProtected(server);
        }
        private static void Packet_120_134(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(134);
            server.AppendParameter(new object[] { 2, 1 });
            server.AppendParameter(new object[] { 0, 1 });
            Session.SendDataProtected(server);         
        }
        private static void handler_120133(SessionInstance Session, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(133);
            server.AppendParameter(1);
            server.AppendParameter(1);
            Session.SendDataProtected(server);
        }
        private static void Packet_120_141(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(141);
            server.AppendParameter(0);
            Session.SendDataProtected(server);
        }
    }
}
