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
            HandlerManager.RegisterHandler(189134, new ProcessHandler(buyObject));
            HandlerManager.RegisterHandler(189181, new ProcessHandler(CargarObjeto));
            HandlerManager.RegisterHandler(189136, new ProcessHandler(putObjectSala));
            HandlerManager.RegisterHandler(189142, new ProcessHandler(CambiarColores));
            HandlerManager.RegisterHandler(189145, new ProcessHandler(MoverObjeto));
            HandlerManager.RegisterHandler(189140, new ProcessHandler(removeObjectSala));
            HandlerManager.RegisterHandler(189143, new ProcessHandler(VoltearObjeto));
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
        private static void buyObject(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala != null)
            {
                int objeto_id = int.Parse(Parameters[0, 0]);

                CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(objeto_id);
                if (Item != null)
                {
                    BuyObjectInstance buyObject = BuyObjectDAO.addAndGetBuyObject(Session.User, Item);
                    if (buyObject != null)
                    {
                        CallPackage.addItemUserBack(Session, Item, buyObject, 1);
                    }
                }
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
        private static void putObjectSala(SessionInstance Session, string[,] Parameters)
        {
            int compra_id = int.Parse(Parameters[0, 0]);
            int Zona_ID = int.Parse(Parameters[1, 0]);
            int Objeto_id = int.Parse(Parameters[2, 0]);
            int x = int.Parse(Parameters[3, 0]);
            int y = int.Parse(Parameters[4, 0]);
            string tam = Parameters[5, 0];
            int rotation = int.Parse(Parameters[6, 0]);
            string Espacio_Ocupado = Parameters[7, 0];
            string Colores_Hex = Parameters[8, 0];
            string Colores_Rgb = Parameters[9, 0];

            if (Session.User.Sala.Escenario.Creador.id != Session.User.id)
            {
                Session.FinalizarConexion("PonerObjeto");
                return;
            }

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(compra_id);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Compra.sala_id != 0) return;
                if (Compra.id != compra_id) return;
                if (Compra.objeto_id != Objeto_id) return;
                if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;

                BuyObjectInstance newObject = new BuyObjectInstance(compra_id,
                    Objeto_id,
                    Zona_ID,
                    Session.User.id,
                    x,
                    y,
                    Colores_Hex,
                    Colores_Rgb,
                    rotation,
                    tam,
                    Espacio_Ocupado,
                    0,
                    0);
 
                if (BuyObjectDAO.putObjectInArea(Session.User.Sala, Compra, newObject))
                {
                    Compra.posX = newObject.posX;
                    Compra.posY = newObject.posY;
                    Compra.sala_id = Session.User.Sala.id;
                    Compra.espacio_ocupado = newObject.espacio_ocupado;
                    Compra.tam = newObject.tam;
                    Compra.putObjectSala(Session);
                    Compra.removeObjectBack(Session);

                    CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(Compra.objeto_id);
                    if (Item != null && Item.Planta != null)
                    {
                        Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.limit_riegue_time);
                        Compra.Planta_sol = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.creation_time);
                        new Thread(() => Compra.regarPlanta(Session)).Start();

                        BuyObjectDAO.updatePlantaAguaAndSol(Compra);
                    }

                    Session.User.Sala.ObjetosEnSala.Add(Compra.id, Compra);
                    if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id))
                    {
                        Session.User.Sala.FijarChutas(Compra);
                        /*
                        0,0 > Posicion central
                        0,1 > Trajectoria derecha - abajo
                        1,0 > Trajectoria derecha - arriba
                        -1,0 > Trajectoria esquerda - abajo
                        0, -1 > Trajectoria esquerda - arriba
                        1,1 > Derecha
                        */
                    }
                }
            }
        }
     
        private static void CambiarColores(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            string hex = Parameters[1, 0];
            string rgb = Parameters[2, 0];
            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("CambiarColores"); return; }


                new Thread(() => cambiarColoresObjectSQL(Compra, hex, rgb)).Start();
                Session.User.Sala.ObjetosEnSala[Compra.id].colores_hex = hex;
                Session.User.Sala.ObjetosEnSala[Compra.id].colores_rgb = rgb;
                Packet_189_142(Session, Compra, Parameters);
            }
        }
        private static void cambiarColoresObjectSQL(BuyObjectInstance Compra, string hex, string rgb)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            client.SetParameter("hex", hex);
            client.SetParameter("dec", rgb);
            client.ExecuteNonQuery("UPDATE objetos_comprados SET colores_hex = @hex, colores_rgb = @dec WHERE id = @id");
        }
        static void MoverObjeto(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            int Objeto_ID = int.Parse(Parameters[1, 0]);
            int x = int.Parse(Parameters[2, 0]);
            int y = int.Parse(Parameters[3, 0]);
            string ocupe = Parameters[4, 0];
            string tam = Parameters[5, 0];
            int rotation = int.Parse(Parameters[6, 0]);
            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Compra.objeto_id != Objeto_ID) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("MoverObjeto"); return; }
              
                using (mysql client = new mysql())
                {
                    client.SetParameter("id", Compra.id);
                    client.SetParameter("posX", x);
                    client.SetParameter("posY", y);
                    client.SetParameter("espacio_ocupado", ocupe);
                    if (client.ExecuteNonQuery("UPDATE objetos_comprados SET posX = @posX, posY = @posY, espacio_ocupado = @espacio_ocupado WHERE id = @id") == 1)
                    {
                        Session.User.Sala.EliminarChutas(Session.User.Sala.ObjetosEnSala[Compra.id]);
                        Session.User.Sala.ObjetosEnSala[Compra.id].posX = x;
                        Session.User.Sala.ObjetosEnSala[Compra.id].posY = y;
                        Session.User.Sala.ObjetosEnSala[Compra.id].espacio_ocupado = ocupe;
                        Session.User.Sala.FijarChutas(Session.User.Sala.ObjetosEnSala[Compra.id]);
                      
                        Packet_189_145(Session, Compra);
                    }
                }
            }
        }
        private static void removeObjectSala(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) {
                    Session.FinalizarConexion("Quitar_Objeto");
                    return;
                }
                if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id))
                {
                    if (BuyObjectDAO.removeObjectSala(Compra))
                    {
                        Session.User.Sala.EliminarChutas(Compra);
                        Session.User.Sala.ObjetosEnSala.Remove(Compra.id);

                        CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(Compra.objeto_id);
                        if (Item != null)
                        {
                            Compra.addObjectBack(Session);
                            Compra.removeObjectSala(Session);

                            if (Item.Planta != null)
                            {
                                Compra.Planta_agua = 0;
                                Compra.Planta_sol = 0;
                                BuyObjectDAO.updatePlantaAguaAndSol(Compra);
                            }
                        }
                    }
                }
            }
        }
        static void VoltearObjeto(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User.Sala != null)
            {
                using (mysql client = new mysql())
                {
                    int NewRotation = 0;
                    int ID = Convert.ToInt32(Parameters[0, 0]);
                    int ItemID = Convert.ToInt32(Parameters[1, 0]);
                    int x = Convert.ToInt32(Parameters[2, 0]);
                    int y = Convert.ToInt32(Parameters[3, 0]);
                    string coor = Parameters[4, 0];
                    string size_rotation = Parameters[5, 0];
                    string rotation = Parameters[6, 0];
                    if (rotation == string.Empty)
                    {
                        NewRotation = int.Parse(size_rotation);
                    }
                    else
                    {
                        NewRotation = int.Parse(rotation);
                    }
                    BuyObjectInstance Item = BuyObjectDAO.getBuyObject(ID);
                    if (Item != null)
                    {
                        if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Item.id)) return;
                        if (Item.usuario_id == Session.User.id)
                        {
                            client.SetParameter("id", Item.id);
                            client.SetParameter("r", NewRotation);
                            if (client.ExecuteNonQuery("UPDATE objetos_comprados SET rotation = @r WHERE id = @id") == 1)
                            {
                                Session.User.Sala.ObjetosEnSala[Item.id].rotation = NewRotation;
                                Item.rotation = NewRotation;
                                Packet_189_143(Session, ID, NewRotation, coor);
                            }
                        }
                    }
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
        static void Packet_189_136(SessionInstance Session, BuyObjectInstance Compra)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(136);
            server.AppendParameter(Compra.id);
            server.AppendParameter(Compra.objeto_id);
            server.AppendParameter(Session.User.Sala.Escenario.id);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(Compra.posX);
            server.AppendParameter(Compra.posY);
            server.AppendParameter(Compra.rotation);
            server.AppendParameter(Compra.tam);
            server.AppendParameter("");
            server.AppendParameter(Compra.espacio_ocupado);
            server.AppendParameter(Compra.colores_hex);
            server.AppendParameter(Compra.colores_rgb);
            server.AppendParameter("0");
            server.AppendParameter("0");
            server.AppendParameter("");
            Session.User.Sala.SendData(server, Session);
        }
        private static void borar_sala_creada_objeto(BuyObjectInstance Compra)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            DataRow muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE id = @id");
            if (muchila != null)
            {
                client.SetParameter("area_id", Convert.ToInt32((string)muchila["data"]));
                client.ExecuteNonQuery("DELETE FROM escenarios_privados WHERE id = @area_id");
                client.SetParameter("id_compra", Compra.id);
                client.SetParameter("data", "");
                client.ExecuteNonQuery("UPDATE objetos_comprados SET data = @data WHERE id = @id_compra");

                if (!SalasManager.Salas_Privadas.ContainsKey(Convert.ToInt32((string)muchila["data"])))
                {
                    DataRow row = client.ExecuteQueryRow("SELECT * FROM escenarios_privados WHERE id = '" + Convert.ToInt32((string)muchila["data"]) + "'");
                    if (row != null)
                    {
                        SalasManager.Salas_Privadas.Remove(Convert.ToInt32((string)muchila["data"]));
                    }
                }
            }
        }
        private static int crear_sala_objeto(SessionInstance Session, BuyObjectInstance Compra, string nombre_sala, int modelo)
        {
            mysql client = new mysql();
            int id_area = 0;
            client.SetParameter("id_sala", Session.User.Sala.Escenario.id);
            DataRow islas = client.ExecuteQueryRow("SELECT * FROM escenarios_privados WHERE id = @id_sala");
            if (islas != null)
            {
                client.SetParameter("nombre", nombre_sala);
                client.SetParameter("modelo", modelo);
                client.SetParameter("CreadorID", Session.User.Sala.Escenario.Creador.id);
                client.SetParameter("objeto_id", Compra.id);
                client.SetParameter("color_1", "15CCFE35D3FEFEF8D98B59371E5B11");
                client.SetParameter("color_2", "6,86,82,15,84,100,78,63,100,111,53,55,51,16,39");
                client.SetParameter("Ultima_Sala", (int)islas["IslaID"]);
                client.ExecuteNonQuery("INSERT INTO escenarios_privados (`nombre`, `modelo`, `CreadorID`, `objeto_id`, `color_1`, `color_2`, `Ultima_Sala` ) VALUES (@nombre, @modelo, @CreadorID, @objeto_id, @color_1, @color_2, @Ultima_Sala)");
            }
            client.SetParameter("objeto_id", Compra.id);
            client.SetParameter("modelo", modelo);
            client.SetParameter("CreadorID", Session.User.Sala.Escenario.Creador.id);
            DataRow buscar_id_sala = client.ExecuteQueryRow("SELECT * FROM escenarios_privados WHERE CreadorID  = @CreadorID AND modelo = @modelo AND objeto_id = @objeto_id");
            if (buscar_id_sala != null)
            {
                client.SetParameter("id", Compra.id);
                client.SetParameter("data", (int)buscar_id_sala["id"]);
                client.ExecuteNonQuery("UPDATE objetos_comprados SET data = @data WHERE id = @id");
                id_area = (int)buscar_id_sala["id"];
            }
            return id_area;
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
        private static void Packet_189_142(SessionInstance Session, BuyObjectInstance Compra, string[,] Parameters)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(142);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].id);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].colores_hex);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].colores_rgb);
            server.AppendParameter(Parameters[3, 0]);
            server.AppendParameter(Parameters[4, 0]);
            server.AppendParameter(Parameters[5, 0]);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_189_145(SessionInstance Session, BuyObjectInstance Compra)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(145);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].id);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].posX);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].posY);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].espacio_ocupado);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].tam);
            server.AppendParameter(Session.User.Sala.ObjetosEnSala[Compra.id].rotation);
            Session.User.Sala.SendData(server, Session);
        }
      
        private static void Packet_189_143(SessionInstance Session, int ID, int NewRotation, string coor)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(143);
            server.AppendParameter(ID);
            server.AppendParameter(NewRotation);
            server.AppendParameter(coor);
            Session.User.Sala.SendData(server, Session);
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
        private static void Packet_189_134(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(134);
            server.AppendParameter(1);
            Session.SendData(server);
        }
        private static void Packet_189_137(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(137);
            server.AppendParameter(1);
            Session.SendData(server);
        }
    }
}
