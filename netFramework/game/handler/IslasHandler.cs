using BoomBang.game.dao;
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
            HandlerManager.RegisterHandler(189124, new ProcessHandler(MostrarIsla));
            HandlerManager.RegisterHandler(189121, new ProcessHandler(CrearZona));
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
                && Session.User.Sala == null)
            {
                int islaId = int.Parse(Parameters[0, 0]);
                string description = Parameters[1, 0];

                IslaInstance Isla = IslasManager.ObtenerIsla(islaId);
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        if (Utils.checkValidCharacters(description, false))
                        {
                            new Thread(() => IslasManager.CambiarDescripcion(Isla, description)).Start();
                        }
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void CambiarColores(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala != null && Session.User.PreLock__Proteccion_SQL != true)
            {
                if (EscenariosManager.ControlDeSeguridad(Session.User, Session.User.Sala.Escenario))
                {
                    string colorsHEX = Parameters[0, 0];
                    string colorsRGB = Parameters[1, 0];

                    Session.User.Sala.Escenario.color_1 = colorsHEX;
                    Session.User.Sala.Escenario.color_2 = colorsRGB;

                    new Thread(() => EscenarioDAO.updateColors(Session.User.Sala.Escenario.id, colorsHEX, colorsRGB)).Start();

                    Session.User.Sala.Escenario.changeColors(Session);

                    Session.User.PreLock__Proteccion_SQL = true;
                }
            }
        }
        private static void RenombrarIsla(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala == null && Session.User.PreLock__Proteccion_SQL != true)
            {
                int islaId = int.Parse(Parameters[0, 0]);
                string name = Parameters[1, 0];

                IslaInstance Isla = IslasManager.ObtenerIsla(islaId);
                if (Isla != null)
                {
                    if (IslasManager.ControlDeSeguridad(Session.User, Isla))
                    {
                        if (Utils.checkValidCharacters(name, false))
                        {
                            new Thread(() => IslaDAO.updateName(Isla.id, name)).Start();

                            Isla.changeName(Session);
                        }
                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void RenombrarZona(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala == null && Session.User.PreLock__Proteccion_SQL != true)
            {
                int escenarioId = int.Parse(Parameters[1, 0]);
                string name = Parameters[2, 0];
                EscenarioInstance Escenario = EscenariosManager.ObtenerEscenario(0, escenarioId);
                if (Escenario != null && Escenario.es_categoria == 0)
                {
                    if (EscenariosManager.ControlDeSeguridad(Session.User, Escenario))
                    {
                        if (Utils.checkValidCharacters(name, false))
                        {
                            EscenarioDAO.updateName(Escenario.id, name);

                            SalaInstance Sala = SalasManager.ObtenerSala(Escenario);
                            if (Sala != null)
                            {
                                Sala.Escenario.nombre = name;
                            }
                        }

                        Session.User.PreLock__Proteccion_SQL = true;
                    }
                }
            }
        }
        private static void EliminarZona(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala == null)
            {
                int escenarioId = int.Parse(Parameters[0, 0]);

                EscenarioInstance Escenario = EscenariosManager.ObtenerEscenario(0, escenarioId);
                if (Escenario != null)
                {
                    if (EscenariosManager.ControlDeSeguridad(Session.User, Escenario))
                    {
                        foreach(BuyObjectInstance objectEscenario in BuyObjectDAO.islaObjects(Escenario.id).ToList()){
                            objectEscenario.removeObjectSala(Session);
                        }

                        BuyObjectDAO.deleteObjectsEscenario(Escenario.id);
                        EscenarioDAO.deleteEscenarioPrivado(Escenario.id);

                        SalaInstance Sala = SalasManager.ObtenerSala(Escenario);
                        if (Sala != null)
                        {
                            SalasManager.EliminarSala(Sala);
                        }
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
                server.AppendParameter(-1);
                server.AppendParameter(Isla.Creador.id);
                server.AppendParameter(Isla.Creador.nombre);
                server.AppendParameter(Isla.Creador.Personaje.avatar);
                server.AppendParameter(Isla.Creador.Personaje.colores);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
                server.AppendParameter(-1);
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
                    server.AppendParameter(0);
                    server.AppendParameter(0);
                    server.AppendParameter((string.IsNullOrEmpty(Escenario.Clave) ? 0 : 1));
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
