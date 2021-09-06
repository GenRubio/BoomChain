using BoomBang.game.instances;
using BoomBang.game.instances.manager;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using BoomBang.Forms;
using BoomBang.game.dao;
using BoomBang.SocketsWeb;

namespace BoomBang.game.handler
{
    class CatalogoHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(189133, new ProcessHandler(CargarCatalogo));
            HandlerManager.RegisterHandler(189180, new ProcessHandler(CargarMochila));
            HandlerManager.RegisterHandler(189181, new ProcessHandler(CargarObjeto));
            HandlerManager.RegisterHandler(189158, new ProcessHandler(Control));
            HandlerManager.RegisterHandler(189159, new ProcessHandler(ActivarObjeto));
            HandlerManager.RegisterHandler(189144, new ProcessHandler(CambiarTamañoObjeto));
        }
        private static void CargarCatalogo(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                ServerMessage server = new ServerMessage();
                server.AddHead(189);
                server.AddHead(133);
                foreach (CatalogObjectInstance Item in CatalagoObjetosDAO.getCatalagoObjetosList())
                {
                    server.AppendParameter(Item.id);
                    server.AppendParameter(-1);
                    server.AppendParameter(Item.titulo);
                    server.AppendParameter(Item.swf_name);
                    server.AppendParameter(Item.descripcion);
                    server.AppendParameter(Item.price);
                    server.AppendParameter(-1);
                    server.AppendParameter(1);
                    server.AppendParameter(Item.colores_hex);
                    server.AppendParameter(Item.colores_rgb);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter("parte_1");
                    server.AppendParameter("parte_2");
                    server.AppendParameter("parte_3");
                    server.AppendParameter("parte_4");
                    server.AppendParameter(1);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(Item.espacio_mapabytes);// something_1
                    server.AppendParameter(0);//Espacio ocupado 2
                    server.AppendParameter(0);//Espacio ocupado 3
                    server.AppendParameter(0);// something_4
                    server.AppendParameter(0);// something_5
                    server.AppendParameter(0);// something_6
                    server.AppendParameter(Item.tipo); //something 10
                    server.AppendParameter(0);
                    server.AppendParameter(Item.espacio_mapabytes);
                    server.AppendParameter(Item.visible);
                    server.AppendParameter(1);// something_12
                    server.AppendParameter(1); //something_13
                    server.AppendParameter(-1); // something_14
                    server.AppendParameter("1³0"); //Salas usables
                    server.AppendParameter(Item.rotacion);
                }
                Session.SendDataProtected(server);
            }
        }

        private static void CargarMochila(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                mysql client = new mysql();
                Packet_189_180(Session, client);
            }
        }
        private static void CargarObjeto(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    Packet_189_181(Session, Parameters);
                }
            }
        }
        static void Control(SessionInstance Session, string[,] Parameters)
        {
            int ID = Convert.ToInt32(Parameters[0, 0]);
            int Estado = Convert.ToInt32(Parameters[1, 0]);
            BuyObjectInstance Item = BuyObjectDAO.getBuyObject(ID);
            if (Item != null)
            {
                if (Item.usuario_id != Session.User.id) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Item.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("Control"); return; }
                Packet_189_158(Session, ID, Estado);
            }
        }
        
        static void ActivarObjeto(SessionInstance Session, string[,] Parameters)
        {
            Packet_189_159(Session, Parameters);
        }
        static void CambiarTamañoObjeto(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User.Sala != null)
            {
                using (mysql client = new mysql())
                {
                    int ID = Convert.ToInt32(Parameters[0, 0]);
                    int ItemID = Convert.ToInt32(Parameters[1, 0]);
                    int x = Convert.ToInt32(Parameters[2, 0]);
                    int y = Convert.ToInt32(Parameters[3, 0]);
                    string coor = Parameters[4, 0];
                    string size_rotation = Parameters[5, 0];
                    string rotation = Parameters[6, 0];

                    BuyObjectInstance Item = BuyObjectDAO.getBuyObject(ID);
                    if (Item != null)
                    {
                        if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Item.id)) return;
                        if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("CambiarTamañoObjeto"); return; }
                        if (Item.usuario_id == Session.User.id)
                        {
                            client.SetParameter("id", Item.id);
                            client.SetParameter("r", size_rotation);
                            if (client.ExecuteNonQuery("UPDATE objetos_comprados SET tam = @r WHERE id = @id") == 1)
                            {
                                Session.User.Sala.ObjetosEnSala[Item.id].tam = size_rotation;
                                Item.tam = size_rotation;
                                Packet_189_144(Session, ID, size_rotation, coor);
                            }
                        }
                    }
                }
            }
        }
        private static void Packet_189_180(SessionInstance Session, mysql client)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(180);
            Int64 total_item = 0;
            foreach (DataRow dRow in client.ExecuteQueryTable("SELECT distinct objeto_id from objetos_comprados WHERE usuario_id = '" + Session.User.id + "' AND sala_id = 0").Rows)
            {
                client.SetParameter("KekoID", Session.User.id);
                client.SetParameter("Item", Convert.ToInt32(dRow["objeto_id"]));
                total_item = (Int64)client.ExecuteScalar("SELECT COUNT(id) FROM objetos_comprados WHERE objeto_id = @Item AND sala_id = 0 AND usuario_id = '" + Session.User.id + "'");
                server.AppendParameter(Convert.ToInt32(dRow["objeto_id"]));
                server.AppendParameter(total_item);
            }
            Session.SendData(server);
        }
        private static void Packet_189_181(SessionInstance Session, string[,] Parameters)
        {
            mysql client = new mysql();
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(181);
            foreach (DataRow row in client.ExecuteQueryTable("SELECT * FROM objetos_comprados WHERE usuario_id = '" + Session.User.id + "' AND objeto_id = '" + int.Parse(Parameters[0, 0]) + "' AND sala_id = '0'").Rows)
            {
                BuyObjectInstance Item = new BuyObjectInstance(row);
                server.AppendParameter(Item.id);
                server.AppendParameter(Item.objeto_id);
                server.AppendParameter(Item.colores_hex);
                server.AppendParameter(Item.colores_rgb);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(Item.tam);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(1);
            }
            Session.SendData(server);
        }

        private static void Packet_189_158(SessionInstance Session, int ID, int Estado)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(158);
            server.AppendParameter(ID);
            server.AppendParameter(Estado);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_189_159(SessionInstance Session, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(159);
            server.AppendParameter(Parameters[0, 0]);
            server.AppendParameter(Parameters[1, 0]);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_189_144(SessionInstance Session, int ID, string size_rotation, string coor)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(144);
            server.AppendParameter(ID);
            server.AppendParameter(size_rotation);
            server.AppendParameter(coor);
            Session.User.Sala.SendData(server, Session);
        }
    }
}
