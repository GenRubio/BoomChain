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

namespace BoomBang.game.handler
{
    class CatalogoHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(189133, new ProcessHandler(CargarCatalogo));
            HandlerManager.RegisterHandler(189180, new ProcessHandler(CargarMochila));
            HandlerManager.RegisterHandler(189134, new ProcessHandler(Comprar_Oro));
            HandlerManager.RegisterHandler(189137, new ProcessHandler(Comprar_Plata));
            HandlerManager.RegisterHandler(189181, new ProcessHandler(CargarObjeto));
            HandlerManager.RegisterHandler(189136, new ProcessHandler(PonerObjeto));
            HandlerManager.RegisterHandler(189142, new ProcessHandler(CambiarColores));
            HandlerManager.RegisterHandler(189145, new ProcessHandler(MoverObjeto));
            HandlerManager.RegisterHandler(189140, new ProcessHandler(Quitar_Objeto));
            HandlerManager.RegisterHandler(189143, new ProcessHandler(VoltearObjeto));
            HandlerManager.RegisterHandler(189158, new ProcessHandler(Control));
            HandlerManager.RegisterHandler(189165, new ProcessHandler(Dar_Bebida));
            HandlerManager.RegisterHandler(189159, new ProcessHandler(ActivarObjeto));
            HandlerManager.RegisterHandler(189144, new ProcessHandler(CambiarTamañoObjeto));
        }
        private static void CargarCatalogo(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                Packet_189_133(Session);
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
        private static void Comprar_Oro(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    mysql client = new mysql();
                    Comprar_Oro(Session, client, Parameters);
                }
            }
        }
        private static void Comprar_Plata(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    mysql client = new mysql();
                    Comprar_Plata(Session, client, Parameters);
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
        private static void PonerObjeto(SessionInstance Session, string[,] Parameters)
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
            BuyObjectInstance Compra = CatalogoManager.ObtenerCompra(compra_id);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Compra.sala_id != 0) return;
                if (Compra.id != compra_id) return;
                if (Compra.objeto_id != Objeto_id) return;
                if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("PonerObjeto"); return; }
                
                if (CatalogoManager.ColocarObjeto(Session.User.Sala, Compra, compra_id, x, y, tam, rotation, Espacio_Ocupado))
                {
                   
                    Packet_189_136(Session, Compra);
                    Packet_189_169(Session, compra_id, Objeto_id);
                    if (listas.Plantas.Contains(Compra.objeto_id))
                    {
                        Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Dias, 1);
                        Compra.Planta_sol = Time.GetCurrentAndAdd(AddType.Dias, 7);
                        new Thread(() => PlantasManager.planta_sql(Compra)).Start();
                        new Thread(() => planta_regada(Session, Compra)).Start();
                    }
                }
            }
        }
     
        private static void CambiarColores(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            string hex = Parameters[1, 0];
            string rgb = Parameters[2, 0];
            BuyObjectInstance Compra = CatalogoManager.ObtenerCompra(Compra_ID);
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
            BuyObjectInstance Compra = CatalogoManager.ObtenerCompra(Compra_ID);
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
        static void Quitar_Objeto(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            mysql client = new mysql();
            BuyObjectInstance Compra = CatalogoManager.ObtenerCompra(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("Quitar_Objeto"); return; }
                if (CatalogoManager.QuitarObjeto(Session.User.Sala, Compra))
                {
                    DataRow row = client.ExecuteQueryRow("SELECT * FROM objetos WHERE id = '" + Compra.objeto_id + "'");
                    CatalogObjectInstance item = new CatalogObjectInstance(row);
                  
                    if (listas.Objetos_Area.Contains(Compra.objeto_id))
                    {
                        borar_sala_creada_objeto(Compra);
                    }
                  
                    Packet_189_140(Session, Compra);
                    Packet_189_139(Session, item, Compra.id, 1, Compra.tam);
                    if (listas.Plantas.Contains(Compra.objeto_id))
                    {
                        Compra.Planta_agua = 0;
                        Compra.Planta_sol = 0;
                        PlantasManager.planta_sql(Compra);
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
                    BuyObjectInstance Item = CatalogoManager.ObtenerCompra(ID);
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
            BuyObjectInstance Item = CatalogoManager.ObtenerCompra(ID);
            if (Item != null)
            {
                if (Item.usuario_id != Session.User.id) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Item.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id) { Session.FinalizarConexion("Control"); return; }
                Packet_189_158(Session, ID, Estado);
            }
        }
        static void Dar_Bebida(SessionInstance Session, string[,] Parameters)
        {
            mysql client = new mysql();
            int compra_id = int.Parse(Parameters[0, 0]);
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(165);
            server.AppendParameter(compra_id);///Objeto id
            Session.User.Sala.SendData(server, Session);

            BuyObjectInstance Compra = CatalogoManager.ObtenerCompra(compra_id);

            if (Time.GetDifference(Compra.Planta_agua) > 0)
            {
                Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Dias, 1);
                client.ExecuteNonQuery("UPDATE objetos_comprados SET planta_agua = '" + Compra.Planta_agua + "' WHERE id = '" + Compra.id + "'");
            }
            else
            {
                Compra.Planta_sol = Time.GetCurrentAndAdd(AddType.Dias, 7);
                Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Dias, 1);
                client.ExecuteNonQuery("UPDATE objetos_comprados SET planta_agua = '" + Compra.Planta_agua + "' WHERE id = '" + Compra.id + "'");
                client.ExecuteNonQuery("UPDATE objetos_comprados SET planta_sol = '" + Compra.Planta_sol + "' WHERE id = '" + Compra.id + "'");
            }
            new Thread(() => planta_regada(Session, Compra)).Start();
        }
        static void planta_regada(SessionInstance Session, BuyObjectInstance Compra)
        {
            Thread.Sleep(new TimeSpan(0, 0, 1));
            ServerMessage planta = new ServerMessage();
            planta.AddHead(189);
            planta.AddHead(173);
            planta.AppendParameter(Compra.id);
            planta.AppendParameter((86400 - Time.GetDifference(Compra.Planta_agua)) / 12);
            planta.AppendParameter(Time.GetDifference(Compra.Planta_agua));
            planta.AppendParameter((604800 - Time.GetDifference(Compra.Planta_sol)) / 4);
            planta.AppendParameter(Time.GetDifference(Compra.Planta_sol));
            planta.AppendParameter(1);
            Session.User.Sala.SendData(planta);
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

                    BuyObjectInstance Item = CatalogoManager.ObtenerCompra(ID);
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
        static void Packet_Aceptar_Compra(SessionInstance Session, int item, bool Objeto_Normal, int Items_Nesesarios, int Item_id, int loteria, bool Oro, string tam, int cantidad)
        {
            mysql client = new mysql();
            if (Oro == true) { Packet_189_134(Session); }
            if (Oro == false) { Packet_189_137(Session); }
            client.SetParameter("objeto_id", item);
            DataRow objetos = client.ExecuteQueryRow("SELECT * FROM objetos WHERE id = @objeto_id");
            if (objetos != null)
            {
                CatalogObjectInstance objeto = new CatalogObjectInstance(objetos);
                client.SetParameter("id", Session.User.id);
                DataRow usuarios = client.ExecuteQueryRow("SELECT * FROM usuarios WHERE id = @id");
                if (usuarios != null)
                {
                    if ((int)objetos["precio_oro"] > 0)
                    {
                        if ((int)usuarios["oro"] >= (int)objetos["precio_oro"])
                        {
                            
                            for (int a = 0; a < cantidad; a++)
                            {
                                if (a == 0)
                                {
                                    client.SetParameter("item_id", objeto.id);
                                    client.SetParameter("userid", Session.User.id);
                                    client.SetParameter("hex", objeto.colores_hex);
                                    client.SetParameter("rgb", objeto.colores_rgb);
                                    client.SetParameter("tam", tam);
                                    client.SetParameter("default_data", objeto.default_data);
                                    client.SetParameter("loteria_numero", loteria);
                                    if (client.ExecuteNonQuery("INSERT INTO objetos_comprados (`objeto_id`, `colores_hex`, `colores_rgb`, `usuario_id`, `tam`, `data`, `loteria_numero`) VALUES (@item_id, @hex, @rgb, @userid, @tam, @default_data, @loteria_numero)") == 1)
                                    {
                                        client.SetParameter("id", objeto.id);
                                        client.SetParameter("UserID", Session.User.id);
                                        int compra_id = int.Parse(Convert.ToString(client.ExecuteScalar("SELECT MAX(id) FROM objetos_comprados WHERE objeto_id = @id AND usuario_id = @UserID")));
                                        Packet_189_139(Session, objeto, compra_id, 1, tam);
                                    }
                                }
                                else if (a > 0 && objeto.swf == "Egg_Teleport")
                                {
                                    client.SetParameter("id_objeto", objeto.id);
                                    client.SetParameter("user_id", Session.User.id);
                                    DataRow primer_item_muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE objeto_id = @id_objeto AND usuario_id = @user_id AND id_objeto_2 = -1");
                                    if (primer_item_muchila != null)
                                    {
                                        client.SetParameter("item_id", objeto.id);
                                        client.SetParameter("userid", Session.User.id);
                                        client.SetParameter("hex", objeto.colores_hex);
                                        client.SetParameter("rgb", objeto.colores_rgb);
                                        client.SetParameter("tam", tam);
                                        client.SetParameter("default_data", objeto.default_data);
                                        client.SetParameter("loteria_numero", loteria);
                                        client.SetParameter("id_objeto_2", (int)primer_item_muchila["id"]);
                                        if (client.ExecuteNonQuery("INSERT INTO objetos_comprados (`objeto_id`, `colores_hex`, `colores_rgb`, `usuario_id`, `tam`, `data`, `loteria_numero`, `id_objeto_2`) VALUES (@item_id, @hex, @rgb, @userid, @tam, @default_data, @loteria_numero, @id_objeto_2)") == 1)
                                        {
                                            client.SetParameter("id", objeto.id);
                                            client.SetParameter("UserID", Session.User.id);
                                            int compra_id = int.Parse(Convert.ToString(client.ExecuteScalar("SELECT MAX(id) FROM objetos_comprados WHERE objeto_id = @id AND usuario_id = @UserID")));
                                            Packet_189_139(Session, objeto, compra_id, 1, tam);
                                        }
                                    }
                                    client.SetParameter("id_objeto", objeto.id);
                                    client.SetParameter("user_id", Session.User.id);
                                    DataRow actualizar_primer_objeto = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE objeto_id = @id_objeto AND id_objeto_2 = -1 AND usuario_id = @user_id");
                                    if (actualizar_primer_objeto != null)
                                    {
                                        client.SetParameter("id_objeto", objeto.id);
                                        client.SetParameter("user_id", Session.User.id);
                                        DataRow detectar_ob_2 = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @user_id AND objeto_id = @id_objeto AND id_objeto_2 != -1 ORDER BY id DESC");
                                        if (detectar_ob_2 != null)
                                        {
                                            client.SetParameter("user_id", Session.User.id);
                                            client.SetParameter("id", (int)actualizar_primer_objeto["id"]);
                                            client.SetParameter("id_objeto_2", (int)detectar_ob_2["id"]);
                                            client.ExecuteNonQuery("UPDATE objetos_comprados SET id_objeto_2 = @id_objeto_2 WHERE usuario_id = @user_id AND id = @id");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((int)objetos["precio_plata"] > 0)
                    {
                        if ((int)usuarios["plata"] >= (int)objetos["precio_plata"])
                        {
                      
                            for (int a = 0; a < cantidad; a++)
                            {
                                if (a == 0)
                                {
                                    client.SetParameter("item_id", objeto.id);
                                    client.SetParameter("userid", Session.User.id);
                                    client.SetParameter("hex", objeto.colores_hex);
                                    client.SetParameter("rgb", objeto.colores_rgb);
                                    client.SetParameter("tam", tam);
                                    client.SetParameter("default_data", objeto.default_data);
                                    client.SetParameter("loteria_numero", 0);
                                    if (client.ExecuteNonQuery("INSERT INTO objetos_comprados (`objeto_id`, `colores_hex`, `colores_rgb`, `usuario_id`, `tam`, `data`, `loteria_numero`) VALUES (@item_id, @hex, @rgb, @userid, @tam, @default_data, @loteria_numero)") == 1)
                                    {
                                        client.SetParameter("id", objeto.id);
                                        client.SetParameter("UserID", Session.User.id);
                                        int compra_id = int.Parse(Convert.ToString(client.ExecuteScalar("SELECT MAX(id) FROM objetos_comprados WHERE objeto_id = @id AND usuario_id = @UserID")));
                                        Packet_189_139(Session, objeto, compra_id, 1, tam);
                                    }
                                }
                                if (a > 0 && objeto.swf == "Teleport_Vale")
                                {
                                    client.SetParameter("id_objeto", objeto.id);
                                    client.SetParameter("user_id", Session.User.id);
                                    DataRow primer_item_muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE objeto_id = @id_objeto AND usuario_id = @user_id AND id_objeto_2 = -1");
                                    if (primer_item_muchila != null)
                                    {
                                        client.SetParameter("item_id", objeto.id);
                                        client.SetParameter("userid", Session.User.id);
                                        client.SetParameter("hex", objeto.colores_hex);
                                        client.SetParameter("rgb", objeto.colores_rgb);
                                        client.SetParameter("tam", tam);
                                        client.SetParameter("default_data", objeto.default_data);
                                        client.SetParameter("loteria_numero", 0);
                                        client.SetParameter("id_objeto_2", (int)primer_item_muchila["id"]);
                                        if (client.ExecuteNonQuery("INSERT INTO objetos_comprados (`objeto_id`, `colores_hex`, `colores_rgb`, `usuario_id`, `tam`, `data`, `loteria_numero`, `id_objeto_2`) VALUES (@item_id, @hex, @rgb, @userid, @tam, @default_data, @loteria_numero, @id_objeto_2)") == 1)
                                        {
                                            client.SetParameter("id", objeto.id);
                                            client.SetParameter("UserID", Session.User.id);
                                            int compra_id = int.Parse(Convert.ToString(client.ExecuteScalar("SELECT MAX(id) FROM objetos_comprados WHERE objeto_id = @id AND usuario_id = @UserID")));
                                            Packet_189_139(Session, objeto, compra_id, 1, tam);
                                        }
                                    }
                                    client.SetParameter("id_objeto", objeto.id);
                                    client.SetParameter("user_id", Session.User.id);
                                    DataRow actualizar_primer_objeto = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE objeto_id = @id_objeto AND id_objeto_2 = -1 AND usuario_id = @user_id");
                                    if (actualizar_primer_objeto != null)
                                    {
                                        client.SetParameter("id_objeto", objeto.id);
                                        client.SetParameter("user_id", Session.User.id);
                                        DataRow detectar_ob_2 = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @user_id AND objeto_id = @id_objeto AND id_objeto_2 != -1 ORDER BY id DESC");
                                        if (detectar_ob_2 != null)
                                        {
                                            client.SetParameter("user_id", Session.User.id);
                                            client.SetParameter("id", (int)actualizar_primer_objeto["id"]);
                                            client.SetParameter("id_objeto_2", (int)detectar_ob_2["id"]);
                                            client.ExecuteNonQuery("UPDATE objetos_comprados SET id_objeto_2 = @id_objeto_2 WHERE usuario_id = @user_id AND id = @id");
                                        }
                                    }
                                }
                            }
                            if (objeto.id == 3043)
                            {
                                client.SetParameter("id", Session.User.id);
                                client.ExecuteNonQuery("UPDATE objetos_comprados SET data = 'Conejo: Compro objetos de oro! A buen precio :v' WHERE usuario_id = @id AND objeto_id = 3043");
                                objeto.default_data = "Conejo: Compro objetos de oro! A buen precio :v";
                            }
                        }
                    }
                }
            }
        }
        static void Comprar_Plata(SessionInstance Session, mysql client, string[,] Parameters)
        {
            int objeto_id = int.Parse(Parameters[0, 0]);
            client.SetParameter("objeto_id", objeto_id);
            DataRow objetos = client.ExecuteQueryRow("SELECT * FROM objetos WHERE id = @objeto_id");
            if (objetos != null)
            {
                client.SetParameter("user_id", Session.User.id);
                DataRow usuario = client.ExecuteQueryRow("SELECT * FROM usuarios WHERE id = @user_id");
                if (usuario != null)
                {
                    if ((int)usuario["plata"] >= (int)objetos["precio_plata"])
                    {
                        if (listas.Llaves_Casas.Contains(objeto_id))
                        {
                            client.SetParameter("id_user", Session.User.id);
                            client.SetParameter("id_objeto", objeto_id);
                            DataRow muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @id_user AND objeto_id = @id_objeto");
                            if (muchila != null)
                            {
                                NotificacionesManager.NotifiChat(Session, "Catálogo: Ya tienes este objeto comprado. Algunos objetos solo pueden ser comprados 1 vez.");
                                ServerMessage server = new ServerMessage();
                                server.AddHead(189);
                                server.AddHead(134);
                                server.AppendParameter(1);
                                Session.SendData(server);
                                return;
                            }
                        }
                        if ((int)objetos["vip"] == 1) { return; }
                        if (listas.Objetos_Catalogo_Plata.Contains(objeto_id))
                        {
                            client.SetParameter("id_user", Session.User.id);
                            client.SetParameter("objeto_id", objeto_id);
                            DataRow muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @id_user AND objeto_id = @objeto_id");
                            if (muchila != null)
                            {
                                NotificacionesManager.NotifiChat(Session, "Catálogo: Ya tienes este objeto comprado. Algunos objetos solo pueden ser comprados 1 vez.");
                                ServerMessage server = new ServerMessage();
                                server.AddHead(189);
                                server.AddHead(134);
                                server.AppendParameter(1);
                                Session.SendData(server);
                                return;
                            }
                        }
                        int cantidad = 1;
                        if ((string)objetos["swf"] == "Teleport_Vale") { cantidad = 2; }
                        Packet_Aceptar_Compra(Session, objeto_id, false, 0, 0, 0, false, Parameters[1, 0], cantidad);
                    }
                    else
                    {
                        ServerMessage server = new ServerMessage();
                        server.AddHead(189);
                        server.AddHead(134);
                        server.AppendParameter(1);
                        Session.SendData(server);
                    }
                }
            }
        }
        static void Comprar_Oro(SessionInstance Session, mysql client, string[,] Parameters)
        {
            int objeto_id = int.Parse(Parameters[0, 0]);
            client.SetParameter("objeto_id", objeto_id);
            DataRow objetos = client.ExecuteQueryRow("SELECT * FROM  objetos WHERE id = @objeto_id");
            if (objetos != null)
            {
                client.SetParameter("user_id", Session.User.id);
                DataRow usuario = client.ExecuteQueryRow("SELECT * FROM usuarios WHERE id = @user_id");
                if (usuario != null)
                {
                    if ((int)usuario["oro"] >= (int)objetos["precio_oro"])
                    {
                        if (listas.Llaves_Casas.Contains(objeto_id))
                        {
                            client.SetParameter("id_user", Session.User.id);
                            client.SetParameter("id_objeto", objeto_id);
                            DataRow muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @id_user AND objeto_id = @id_objeto");
                            if (muchila != null)
                            {
                                NotificacionesManager.NotifiChat(Session, "Catálogo: Ya tienes este objeto comprado. Algunos objetos solo pueden ser comprados 1 vez.");
                                ServerMessage server = new ServerMessage();
                                server.AddHead(189);
                                server.AddHead(134);
                                server.AppendParameter(1);
                                Session.SendData(server);
                                return;
                            }
                        }
                        if ((int)objetos["vip"] == 1) { return; }
                        if (listas.Objetos_Catalogo_Oro.Contains(objeto_id))
                        {
                            client.SetParameter("id_user", Session.User.id);
                            client.SetParameter("objeto_id", objeto_id);
                            DataRow muchila = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE usuario_id = @id_user AND objeto_id = @objeto_id");
                            if (muchila != null)
                            {
                                NotificacionesManager.NotifiChat(Session, "Catálogo: Ya tienes este objeto comprado. Algunos objetos solo pueden ser comprados 1 vez.");
                                ServerMessage server = new ServerMessage();
                                server.AddHead(189);
                                server.AddHead(134);
                                server.AppendParameter(1);
                                Session.SendData(server);
                                return;
                            }
                        }
                        int loteria_numero = 0;
                        if (objeto_id == 871)//Loteria Semanal
                        {
                            DataRow loteria = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE NOT (loteria_numero < (SELECT MAX(loteria_numero) FROM objetos_comprados))");
                            loteria_numero = (int)loteria["loteria_numero"];
                            loteria_numero++;
                        }
                      
                        int cantidad = 1;
                        if ((string)objetos["swf"] == "Egg_Teleport") { cantidad = 2; }
                        Packet_Aceptar_Compra(Session, objeto_id, false, 0, 0, loteria_numero, true, Parameters[1, 0], cantidad);

                    }
                    else
                    {
                        ServerMessage server = new ServerMessage();
                        server.AddHead(189);
                        server.AddHead(134);
                        server.AppendParameter(1);
                        Session.SendData(server);
                    }
                }
            }
        }
        private static void Packet_189_133(SessionInstance Session)
        {
            mysql client = new mysql();
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(133);
            foreach (CatalogObjectInstance Item in CatalogoManager.ObtenerCatalogo())
            {
                server.AppendParameter(Item.id);
                server.AppendParameter(-1);
                server.AppendParameter(Item.titulo);
                server.AppendParameter(Item.swf);
                server.AppendParameter(Item.descripcion);
                if (Item.limitado == 1) { server.AppendParameter(Item.oro_descuento); }
                else { server.AppendParameter(Item.precio_oro); }
                server.AppendParameter(Item.precio_plata);
                if (Item.tipo_arrastre == 28 || Item.tipo_arrastre == 30) { server.AppendParameter(Item.categoria + "³18"); }//Pociones
                else if (Item.limitado == 1) { server.AppendParameter(Item.categoria + "³17"); }
                else { server.AppendParameter(Item.categoria); }
                server.AppendParameter(Item.colores_hex);
                server.AppendParameter(Item.colores_rgb);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(Item.parte_1);
                server.AppendParameter(Item.parte_2);
                server.AppendParameter(Item.parte_3);
                server.AppendParameter(Item.parte_4);
                server.AppendParameter(Item.tam_n);
                server.AppendParameter(Item.tam_g);
                server.AppendParameter(Item.tam_p);
                server.AppendParameter(Item.espacio_ocupado_n);// something_1
                server.AppendParameter(0);//Espacio ocupado 2
                server.AppendParameter(0);//Espacio ocupado 3
                server.AppendParameter(Item.something_4);// something_4
                server.AppendParameter(Item.something_5);// something_5
                server.AppendParameter(Item.something_6);// something_6
                server.AppendParameter(Item.tipo_arrastre); //something 10
                server.AppendParameter(Item.vip);
                server.AppendParameter(Item.espacio_mapabytes);
                server.AppendParameter(Item.id == 1112 && Session.User.admin > 0 ? 1 : Item.visible);
                server.AppendParameter(Item.tipo_rare);// something_12
                server.AppendParameter(Item.arrastrable); //something_13
                server.AppendParameter(Item.intercambiable); // something_14
                server.AppendParameter(Item.salas_usables); //something_15
                server.AppendParameter(Item.rotacion);
            }
            Session.SendDataProtected(server);
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
            if (listas.Objetos_Area.Contains(Compra.objeto_id))
            {
                if (Compra.objeto_id == 602)//ID de Igloo en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Igloo", 16);// 16 es el modelo de escenario16.swf = Igloo
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 142)//ID de tienda camuflaje en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Tienda Camuflaje", 8);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 128)//ID de calabazaaargh! en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Calabazaaargh!", 15);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 613)//ID de tienda voodoo en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Tienda Voodoo", 7);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 612)//ID de tienda tribal en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Tienda Tribal", 7);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 611)//ID de tienda fuego en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Tienda Fuego", 7);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 603)//ID de tienda india en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Tienda India", 7);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 976)//ID de barco pirata en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Barco Pirata", 18);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 892)//ID de casa china en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Okiya", 20);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 888)//ID de casa china dojo en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Dojo", 19);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 654)//ID de torre mágica en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Torre Mágica", 22);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                else if (Compra.objeto_id == 444)//ID de madriguera en catalago
                {
                    int id_sala = crear_sala_objeto(Session, Compra, "Madriguera", 17);// 
                    server.AppendParameter(new object[] { Compra.id, 0, id_sala, Compra.open == 0 ? "" : "1", id_sala, 12, 0 });
                    Compra.data = Convert.ToString(id_sala);
                }
                if (!SalasManager.Salas_Privadas.ContainsKey(Convert.ToInt32(Compra.data)))
                {
                    mysql client = new mysql();
                    DataRow row = client.ExecuteQueryRow("SELECT * FROM escenarios_privados WHERE id = '" + Convert.ToInt32(Compra.data) + "'");
                    if (row != null)
                    {
                        EscenarioInstance Escenario = new EscenarioInstance(row);
                        SalasManager.Salas_Privadas.Add(Convert.ToInt32(Compra.data), new SalaInstance(Convert.ToInt32(Compra.data), Escenario));
                    }
                }
            }
            else
            {
                server.AppendParameter(Compra.data);
            }
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
        private static void Packet_189_140(SessionInstance Session, BuyObjectInstance Compra)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(140);
            server.AppendParameter(Compra.id);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_189_139(SessionInstance Session, CatalogObjectInstance item, int compra_id, int Cantidad, string tam)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(139);
            server.AppendParameter(compra_id);
            server.AppendParameter(item.id);
            server.AppendParameter(item.colores_hex);
            server.AppendParameter(item.colores_rgb);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(tam);
            server.AppendParameter(item.espacio_ocupado_n);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(Cantidad);
            Session.SendData(server);
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
