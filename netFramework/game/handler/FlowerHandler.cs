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
            HandlerManager.RegisterHandler(120132, Actualizar_avatar);
            HandlerManager.RegisterHandler(120147120, loadFlowerInterfaz);//Regalo Grande
            HandlerManager.RegisterHandler(132120, new ProcessHandler(Method_132_120));
            HandlerManager.RegisterHandler(132121, new ProcessHandler(Method_132_121));
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
      
        private static void Actualizar_avatar(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                if (Session.User.PreLock_Cambio_Colores == true) return;
                if (int.Parse(Parameters[1, 0]) >= 1 && int.Parse(Parameters[1, 0]) <= 11)
                {
                    if (int.Parse(Parameters[1, 0]) >= 1 && int.Parse(Parameters[1, 0]) <= 11 && Parameters[0, 0].Substring(0,6) == "000000") { return; }
                    if (int.Parse(Parameters[1, 0]) >= 8 && int.Parse(Parameters[1, 0]) <= 9 && Parameters[0, 0].Substring(6, 6) == "000000") { return; }
                    UserManager.ActualizarAvatar(Session.User, Parameters[0, 0], int.Parse(Parameters[1, 0]));
                    Session.User.Time_Cambio_Colores = Time.GetCurrentAndAdd(AddType.Segundos, 13);
                }
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
        private static void Noticia_handler(SessionInstance Session)
        {
            mysql client = new mysql();
            client.ExecuteNonQuery("UPDATE usuarios SET noticia_registro = 0 WHERE id = '" + Session.User.id + "'");
            Session.User.noticia_registro = 0;
        }
        private static void handler_120147121(SessionInstance Session)
        {
            if (Time.GetDifference(Session.User.timespam_regalo_grande) >= 1)
            {
                TimeSpan tiempo = TimeSpan.FromSeconds(Time.GetDifference(Session.User.timespam_regalo_grande));
                 Packet_183(Session, "¡Mega Regalo!\rPróximo regalo en: " + tiempo + "\rContenido: Objetos: Rare, Muy Rare, Casi Unicos, Unicos        vv Baja vv\rCreditos: 1k, 1.5k, 2.0k, 2.5k, 3.0k - 10k, 15k, 20k, 25k , 30k (Plata - Oro)\rPuntos: Shurikens: 10 - 25 - 35 - 50 || Coco: 10 - 25 - 35 - 50\rVIP: 1Mes || 3Meses || 6Meses");
            }
            if (Time.GetDifference(Session.User.timespam_regalo_grande) <= 0)
            {
                Session.User.timespam_regalo_grande = 0;
                Session.User.timespam_regalo_grande = Time.GetCurrentAndAdd(AddType.Horas, 8);
                UserManager.ActualizarEstadisticas(Session.User);
                 Packet_120_147_121(Session);
             
            }
        }
        private static void handler_120137(SessionInstance Session)
        {
            Session.User.timespam_regalo_peque = 0;
            Session.User.timespam_regalo_peque = Time.GetCurrentAndAdd(AddType.Dias, 1);
       
        }
        private static void handler_120146(SessionInstance Session, string nombre)
        {
            mysql client = new mysql();
            DataRow objeto = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE objeto_id = '3065' AND usuario_id = '" + Session.User.id + "'");
            if (objeto != null)
            {
                DataRow comprobar_nombre = client.ExecuteQueryRow("SELECT * FROM usuarios WHERE nombre = '" + nombre + "'");
                if (comprobar_nombre != null)
                {
                    Packet_120_146(Session, 0);
                    return;
                }
                client.ExecuteNonQuery("UPDATE usuarios SET nombre_antiguo = '" + Session.User.nombre + "' WHERE id = '" + Session.User.id + "'");
                client.ExecuteNonQuery("UPDATE usuarios SET nombre = '" + nombre + "' WHERE id = '" + Session.User.id + "'");
                client.ExecuteNonQuery("UPDATE usuarios SET cambio_nombre = '0' WHERE id = '" + Session.User.id + "'");
                client.ExecuteNonQuery("DELETE FROM objetos_comprados WHERE objeto_id = '3065' AND usuario_id = '" + Session.User.id + "' LIMIT 1");
                Session.User.nombre = nombre;
                Packet_120_146(Session, 1);
                Packet_189_169(Session, -1, 3065);
            }
        }
        private static void Packet_132_127(SessionInstance Session, string Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(132);
            server.AddHead(127);
            server.AppendParameter(0);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(Convert.ToString(DateTime.Now).Substring(0, 16));
            server.AppendParameter(Parameters);
            server.AppendParameter(2);
            Session.SendData(server);
        }
        private static void Packet_120_147_120(SessionInstance Session)
        {
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 500));
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(147);
            server.AddHead(120);
            server.AppendParameter(1);
            server.AppendParameter(Time.GetDifference(Session.User.timespam_regalo_grande));
            server.AppendParameter(1);
            Session.SendDataProtected(server);
        }
        private static void Packet_209_128(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(209);
            server.AddHead(128);
            server.AppendParameter(0);
            Session.SendData(server);
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
        private static void Packet_183(SessionInstance Session, string mensaje)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(183);
            server.AppendParameter(mensaje);
            Session.SendData(server);
        }
        private static void Packet_120_147_121(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(147);
            server.AddHead(121);
            server.AppendParameter(1);
            Session.SendData(server);
        }
        private static void Packet_120_146(SessionInstance Session, int parametro)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(146);
            server.AppendParameter(parametro);
            Session.SendData(server);
        }
        private static void Packet_189_169(SessionInstance Session, int compra_id, int Item)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(169);
            server.AppendParameter(compra_id);
            server.AppendParameter(Item);
            server.AppendParameter(1);
            Session.SendData(server);
        }
    }
}
