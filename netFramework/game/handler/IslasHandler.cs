using BoomBang.game.instances;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    class IslasHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(189120, new ProcessHandler(CrearIsla));
            HandlerManager.RegisterHandler(189124, new ProcessHandler(MostrarIsla));
            HandlerManager.RegisterHandler(189121, new ProcessHandler(CrearZona));
            HandlerManager.RegisterHandler(189149, new ProcessHandler(EliminarIsla));
            HandlerManager.RegisterHandler(189132, new ProcessHandler(EliminarZona));
            HandlerManager.RegisterHandler(189130, new ProcessHandler(RenombrarZona));
            HandlerManager.RegisterHandler(189129, new ProcessHandler(RenombrarIsla));
            HandlerManager.RegisterHandler(189126, new ProcessHandler(CambiarDescripcion));
            HandlerManager.RegisterHandler(189146, new ProcessHandler(CambiarColores));
        }
        private static void CambiarDescripcion(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null
                && Session.User.PreLock__Proteccion_SQL != true
                && Session.User.Sala != null)
            {
                IslaInstance Isla = IslasManager.ObtenerIsla(int.Parse(Parameters[0, 0]));
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        if (Utils.checkValidCharacters(Parameters[1, 0], false))
                        {
                            new Thread(() => IslasManager.CambiarDescripcion(Isla, Parameters[1, 0])).Start();
                        }
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void CambiarColores(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.PreLock__Proteccion_SQL == true) return;
                if (Session.User.Sala != null)
                {
                    if (EscenariosManager.ControlDeSeguridad(Session.User, Session.User.Sala.Escenario))
                    {
                        new Thread(() => EscenariosManager.CambiarColores(Session.User.Sala.Escenario, Parameters[0, 0], Parameters[1, 0])).Start();
                        Packet_189_146(Session, Parameters[0, 0], Parameters[1, 0]);
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void RenombrarIsla(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.PreLock__Proteccion_SQL == true) return;
                if (Session.User.Sala != null) return;
                IslaInstance Isla = IslasManager.ObtenerIsla(int.Parse(Parameters[0, 0]));
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        if (Utils.checkValidCharacters(Parameters[1, 0], false))
                        {
                            Packet_189_129(Session, Isla, Parameters[1, 0]);
                        }
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void RenombrarZona(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.PreLock__Proteccion_SQL == true) return;
                if (Session.User.Sala != null) return;
                EscenarioInstance Escenario = EscenariosManager.ObtenerEscenario(0, int.Parse(Parameters[1, 0]));
                if (Escenario != null)
                {
                    if (EscenariosManager.ControlDeSeguridad(Session.User, Escenario))
                    {
                        if (Utils.checkValidCharacters(Parameters[2, 0], false))
                        {
                            EscenariosManager.RenombrarEscenario(Escenario, Parameters[2, 0]);
                        }
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void EliminarZona(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                EscenarioInstance Escenario = EscenariosManager.ObtenerEscenario(0, int.Parse(Parameters[0, 0]));
                if (Escenario != null)
                {
                    if (EscenariosManager.ControlDeSeguridad(Session.User, Escenario))
                    {
                        EscenariosManager.EliminarEscenario(Escenario);
                    }
                }
            }
        }
        private static void EliminarIsla(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                IslaInstance Isla = IslasManager.ObtenerIsla(int.Parse(Parameters[0, 0]));
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        new Thread(() => IslasManager.EliminarIsla(Isla)).Start();
                    }
                }
            }
        }
        private static void CrearZona(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.PreLock__Proteccion_SQL == true) return;
                if (Session.User.Sala != null) return;
                IslaInstance Isla = IslasManager.ObtenerIsla(int.Parse(Parameters[0, 0]));
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        if (IslasManager.ZonasIsla(Isla).Count <= 4)
                        {
                            if (Utils.checkValidCharacters(Parameters[1, 0], false))
                            {
                                int ZonaID = IslasManager.Crear_Zona(Isla, Session.User, Parameters[1, 0], int.Parse(Parameters[6, 0]), 
                                    Parameters[7, 0], Parameters[8, 0]);
                                if (ZonaID >= 1)
                                {
                                    EscenarioInstance Escenario = EscenariosManager.ObtenerEscenario(0, ZonaID);
                                    if (Escenario != null)
                                    {
                                        Packet_189_121(Session, Escenario);
                                        Session.User.PreLock__Proteccion_SQL = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void MostrarIsla(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null) return;
                Packet_189_124(Session, int.Parse(Parameters[0, 0]));
            }
        }
        private static void CrearIsla(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.PreLock__Proteccion_SQL == true) return;
                if (Session.User.Sala != null) return;
                Packet_189_120(Session, Parameters[0, 0], int.Parse(Parameters[1, 0]));
                Session.User.PreLock__Proteccion_SQL = true;
            }
        }
        private static void Packet_189_146(SessionInstance Session, string HEX, string Dec)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(146);
            server.AppendParameter(Session.User.Sala.Escenario.id);
            server.AppendParameter(HEX);
            server.AppendParameter(Dec);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_189_129(SessionInstance Session, IslaInstance Isla, string Nombre)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(129);
            if (IslasManager.ObtenerIsla(Nombre) == null)
            {
                new Thread(() => IslasManager.RenombrarIsla(Isla, Nombre)).Start();
                server.AppendParameter(1);
            }
            else
            {
                server.AppendParameter(0);
            }
            Session.SendData(server);
        }
        private static void Packet_189_121(SessionInstance Session, EscenarioInstance Escenario)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(121);
            server.AppendParameter(0);
            server.AppendParameter(Escenario.es_categoria);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(Escenario.id);
            Session.SendData(server);
        }
        private static void Packet_189_124(SessionInstance Session, int IslaID)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(124);
            IslaInstance Isla = IslasManager.ObtenerIsla(IslaID);
            if (Isla != null)
            {
                IslasManager.Diccionario_AñadirIsla(Isla);
                List<EscenarioInstance> Escenarios = IslasManager.ZonasIsla(Isla);
                server.AppendParameter(Isla.id);
                server.AppendParameter(Isla.nombre);
                server.AppendParameter(Isla.descripcion);
                server.AppendParameter(Isla.modelo);
                server.AppendParameter(Isla.uppert);
                server.AppendParameter(Isla.Creador.id);
                server.AppendParameter(Isla.Creador.nombre);
                server.AppendParameter(Isla.Creador.avatar);
                server.AppendParameter(Isla.Creador.colores);
                server.AppendParameter(Isla.mamigos_1);
                server.AppendParameter(Isla.mamigos_2);
                server.AppendParameter(Isla.mamigos_3);
                server.AppendParameter(Isla.mamigos_4);
                server.AppendParameter(Isla.mamigos_5);
                server.AppendParameter(Isla.mamigos_6);
                server.AppendParameter(Isla.mamigos_7);
                server.AppendParameter(Isla.mamigos_8);
                server.AppendParameter(Isla.noverlo_1);
                server.AppendParameter(Isla.noverlo_2);
                server.AppendParameter(Isla.noverlo_3);
                server.AppendParameter(Isla.noverlo_4);
                server.AppendParameter(Isla.noverlo_5);
                server.AppendParameter(Isla.noverlo_6);
                server.AppendParameter(Isla.noverlo_7);
                server.AppendParameter(Isla.noverlo_8);
                server.AppendParameter(Escenarios.Count);
                foreach (EscenarioInstance Escenario in Escenarios)
                {
                    server.AppendParameter(0);
                    server.AppendParameter(Escenario.es_categoria);
                    server.AppendParameter(Escenario.id);
                    server.AppendParameter(Escenario.id);
                    server.AppendParameter(Escenario.nombre);
                    server.AppendParameter(Escenario.modelo);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter(SalasManager.UsuariosEnSala(Escenario));//Visitantes
                    server.AppendParameter(0);
                    if (Isla.noverlo_1.Contains(Session.User.nombre) || Isla.noverlo_2.Contains(Session.User.nombre) || Isla.noverlo_3.Contains(Session.User.nombre) || Isla.noverlo_4.Contains(Session.User.nombre) || Isla.noverlo_5.Contains(Session.User.nombre) || Isla.noverlo_6.Contains(Session.User.nombre) || Isla.noverlo_7.Contains(Session.User.nombre) || Isla.noverlo_8.Contains(Session.User.nombre))
                    {
                        server.AppendParameter(1);//Usuario no puede acceder a la isla
                        server.AppendParameter(1);
                    }
                    else
                    {
                        if (Isla.mamigos_1.Contains(Session.User.nombre) || Isla.mamigos_2.Contains(Session.User.nombre) || Isla.mamigos_3.Contains(Session.User.nombre) || Isla.mamigos_4.Contains(Session.User.nombre) || Isla.mamigos_5.Contains(Session.User.nombre) || Isla.mamigos_6.Contains(Session.User.nombre) || Isla.mamigos_7.Contains(Session.User.nombre) || Isla.mamigos_8.Contains(Session.User.nombre))
                        {
                            server.AppendParameter(0);
                        }
                        else
                        {
                            server.AppendParameter((string.IsNullOrEmpty(Escenario.Clave) ? 0 : 1));
                        }
                    }
                }

            }
            else
            {
                server.AppendParameter(0);
            }
            Session.SendData(server);
        }
        private static void Packet_189_120(SessionInstance Session, string Nombre, int Modelo)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(120);
            if (IslasManager.IslasCreadas(Session.User) < 25)
            {
                if (Nombre == "") { Session.FinalizarConexion("Packet_189_120"); return; }
                if (Utils.checkValidCharacters(Nombre, false))
                {
                    if (IslasManager.ObtenerIsla(Nombre) == null)
                    {
                        server.AppendParameter(IslasManager.CrearIsla(Session.User, Nombre, Modelo));
                    }
                    else
                    {
                        server.AppendParameter(0);
                    }
                }
            }
            else
            {
                server.AppendParameter(0);
            }
            Session.SendData(server);
        }
    }
}
