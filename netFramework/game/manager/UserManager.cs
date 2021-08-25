using BoomBang.game.handler;
using BoomBang.game.instances;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoomBang.Forms;
using BoomBang.game.dao;

namespace BoomBang.game.manager
{
    class UserManager
    {

        public static Dictionary<int, SessionInstance> UsuariosOnline = new Dictionary<int, SessionInstance>();
    
        private static bool usuarioOnline (UserInstance User)
        {
            foreach(SessionInstance sessionOnline in UsuariosOnline.Values.ToList())
            {
                if (sessionOnline.User == User)
                {
                    return true;
                }
            }
            return false;
        }
        public static void IniciarSesion(SessionInstance Session, string user, string pass)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(130);

            Session.User = UserDAO.getUser(user, pass);
            if (Session.User != null)
            {
                if (!usuarioOnline(Session.User))
                {
                    UsuariosOnline.Add(Session.User.id, Session);

                    if (Session.User.timespam_regalo_peque == 0)
                    {
                        Session.User.timespam_regalo_peque = Time.GetCurrentAndAdd(AddType.Minutos, 30);
                    }

                    server.AppendParameter(1);
                    server.AppendParameter(Session.User.nombre);
                    server.AppendParameter(Session.User.avatar);
                    server.AppendParameter(Session.User.colores);
                    server.AppendParameter(Session.User.email);
                    server.AppendParameter(Session.User.edad);
                    server.AppendParameter(2);
                    server.AppendParameter("Hola, Soy nuevo en BoomBang.");
                    server.AppendParameter(0);
                    server.AppendParameter(Session.User.id);
                    server.AppendParameter(Session.User.admin);
                    server.AppendParameter(Session.User.oro);
                    server.AppendParameter(Session.User.plata);
                    server.AppendParameter(200);
                    server.AppendParameter(5);
                    server.AppendParameter(0);
                    server.AppendParameter(-1);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(Session.User.tutorial_islas);
                    server.AppendParameter("ES");
                    server.AppendParameter(1);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter("");
                    server.AppendParameter(0);//Noticia derecha                            
                    server.AppendParameter(0);
                    server.AppendParameter(new object[] { 1, 0 });
                    server.AppendParameter(0);
                    server.AppendParameter(Session.User.cambio_nombre);//Nombre personaje

                    new Thread(() => UserDAO.updateConnection(Session.User, Session.IP));

                    repair_gift(Session);

                    string console = "[UserManager] -> Se ha conectado el usuario " + Session.User.nombre + ".";
                    Emulator.Form.WriteLine(console);
                    Emulator.Form.UpdateTitle();
                }
                else
                {
                    Thread.Sleep(new TimeSpan(0, 0, 0, 0, 500));
                    server.AppendParameter(2);
                }
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 500));
                server.AppendParameter(0);
            }

            Session.SendDataProtected(server);
        }
        public static void Ajustar_Remuneracion(UserInstance User)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("coins_remain", Time.GetDifference(User.coins_remain_double));
                client.ExecuteNonQuery("UPDATE usuarios SET coins_remain = @coins_remain WHERE id = @id");
            }
        }
        public static void Sumar_Cocos(UserInstance User, int Puntos)
        {
            User.puntos_cocos++;
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("puntos_cocos", User.puntos_cocos);
                client.ExecuteNonQuery("UPDATE usuarios SET puntos_cocos = @puntos_cocos WHERE id = @id");
            }
        }
        public static void Sumar_Shurikens(UserInstance User, int Puntos)
        {
            User.puntos_ninja++;
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("puntos_ninja", User.puntos_ninja);
                client.ExecuteNonQuery("UPDATE usuarios SET puntos_ninja = @puntos_ninja WHERE id = @id");
            }
        }
        public static void ActualizarAvatar(UserInstance User, string colores, int avatar)
        {
            if (User != null)
            {
                using (mysql client = new mysql())
                {
                    client.SetParameter("id", User.id);
                    client.SetParameter("colores", colores);
                    client.SetParameter("avatar", avatar);
                    if (client.ExecuteNonQuery("UPDATE usuarios SET colores = @colores, avatar = @avatar WHERE id = @id") == 1)
                    {
                        User.colores = colores;
                        User.avatar = avatar;
                    }
                }
            }
        }
        public static void ActualizarEstadisticas(UserInstance User)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("besos_enviados", User.besos_enviados);
                client.SetParameter("besos_recibidos", User.besos_recibidos);
                client.SetParameter("jugos_enviados", User.jugos_enviados);
                client.SetParameter("jugos_recibidos", User.jugos_recibidos);
                client.SetParameter("flores_enviadas", User.flores_enviadas);
                client.SetParameter("flores_recibidas", User.flores_recibidas);
                client.SetParameter("uppers_enviados", User.uppers_enviados);
                client.SetParameter("uppers_recibidos", User.uppers_recibidos);
                client.SetParameter("cocos_enviados", User.cocos_enviados);
                client.SetParameter("cocos_recibidos", User.cocos_recibidos);
                client.SetParameter("rings_ganados", User.rings_ganados);
                client.SetParameter("senderos_ganados", User.senderos_ganados);
                client.SetParameter("tutorial_islas", User.tutorial_islas);
                client.SetParameter("timespam_desc_cambios", User.timespam_desc_cambios);
                client.SetParameter("timespam_regalo_grande", User.timespam_regalo_grande);
                client.SetParameter("puntos_cocos", User.puntos_cocos);
                client.SetParameter("puntos_ninja", User.puntos_ninja);
                client.SetParameter("timespam_regalo_peque", User.timespam_regalo_peque);
                client.SetParameter("toneos_ring", User.toneos_ring);
                client.SetParameter("contador_baneo", User.contador_baneo);
                client.SetParameter("t_n_p", User.Traje_Ninja_Principal);
                client.SetParameter("torneos_coco", User.torneos_coco);
                client.SetParameter("email_validado", User.ValidarEmail);
                client.SetParameter("email", User.email);
                client.SetParameter("ver_ranking", User.ver_ranking);
                client.ExecuteNonQuery("UPDATE usuarios SET senderos_ganados = @senderos_ganados, rings_ganados = @rings_ganados, besos_enviados = @besos_enviados, besos_recibidos = @besos_recibidos, jugos_enviados = @jugos_enviados, jugos_recibidos = @jugos_recibidos, flores_enviadas = @flores_enviadas, flores_recibidas = @flores_recibidas, uppers_enviados = @uppers_enviados, uppers_recibidos = @uppers_recibidos, cocos_enviados = @cocos_enviados, cocos_recibidos = @cocos_recibidos, tutorial_islas = @tutorial_islas, timespam_desc_cambios = @timespam_desc_cambios, timespam_regalo_grande = @timespam_regalo_grande, puntos_cocos = @puntos_cocos, puntos_ninja = @puntos_ninja, timespam_regalo_peque = @timespam_regalo_peque, toneos_ring = @toneos_ring, contador_baneo = @contador_baneo, t_n_p = @t_n_p, torneos_coco = @torneos_coco, email_validado = @email_validado, email = @email, ver_ranking = @ver_ranking WHERE id = @id");
            }
        }
        public static void Creditos(UserInstance User, bool Oro, bool sumar, int cantidad)
        {
            SessionInstance Session = UserManager.ObtenerSession(User.id);
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("cantidad", cantidad);
                if (Session != null)
                {
                    if (Oro)
                    {
                        if (sumar)
                        {
                            if (client.ExecuteNonQuery("UPDATE usuarios SET oro = (oro + @cantidad) WHERE id = @id") == 1)
                            {
                                User.oro += cantidad;
                                ServerMessage server = new ServerMessage();
                                server.AddHead(162);
                                server.AppendParameter(cantidad);
                                Session.SendData(server);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (client.ExecuteNonQuery("UPDATE usuarios SET oro = (oro - @cantidad) WHERE id = @id") == 1)
                                {
                                    User.oro -= cantidad;
                                    ServerMessage server = new ServerMessage();
                                    server.AddHead(161);
                                    server.AppendParameter(cantidad);
                                    Session.SendData(server);
                                }
                            }
                            catch (Exception ex)
                            {
                                string console = ex.ToString();
                                Emulator.Form.WriteLine(console);
                            }
                        }
                    }
                    else
                    {
                        if (sumar)
                        {
                            if (client.ExecuteNonQuery("UPDATE usuarios SET plata = (plata + @cantidad) WHERE id = @id") == 1)
                            {
                                User.plata += cantidad;
                                ServerMessage server = new ServerMessage();
                                server.AddHead(166);
                                server.AppendParameter(cantidad);
                                Session.SendData(server);
                            }
                        }
                        else
                        {
                            if (client.ExecuteNonQuery("UPDATE usuarios SET plata = (plata - @cantidad) WHERE id = @id") == 1)
                            {
                                User.plata -= cantidad;
                                ServerMessage server = new ServerMessage();
                                server.AddHead(168);
                                server.AppendParameter(cantidad);
                                Session.SendData(server);
                            }
                        }
                    }
                }
            }
        }
        private static void repair_gift(SessionInstance Session)
        {
            ServerMessage server2 = new ServerMessage();
            server2.AddHead(120);
            server2.AddHead(147);
            server2.AddHead(120);
            server2.AppendParameter(1);
            server2.AppendParameter(Time.GetDifference(Session.User.timespam_regalo_grande));
            server2.AppendParameter(1);
            Session.SendData(server2);
        }
        public static SessionInstance ObtenerSession(int user_id)
        {
            if (UsuariosOnline.ContainsKey(user_id))
            {
                return UsuariosOnline[user_id];
            }
            return null;
        }
    }
}

