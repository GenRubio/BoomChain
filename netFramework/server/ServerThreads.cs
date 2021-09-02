
using BoomBang.game.instances;
using BoomBang.game.manager;
using System;
using System.Linq;
using System.Threading;
using BoomBang.Forms;

namespace BoomBang.server
{
    class ServerThreads
    {
        public static void Initialize()
        {
            new Thread(Pathfinder).Start();
            new Thread(PathfinderIA).Start();
            new Thread(Ping).Start();
            new Thread(mGamesCall).Start();
        }
        private static void mGamesCall()
        {
            while (true)
            {
                mGamesCall_Manager();
                Thread.Sleep(1000);
            }
        }
        private static void Ping()
        {
            while (true)
            {
                foreach (SessionInstance Session in UserManager.UsuariosOnline.Values.ToList())
                {
                    if (Time.GetDifference(Session.LastPingTime) <= -60 && Session.ping == false)
                    {
                        Session.FinalizarConexion("Ping");
                        continue;
                    }
                }
                Thread.Sleep(1000);
            }
        }
        private static void Pathfinder()
        {
            while (true)
            {
                try
                {
                    foreach (SessionInstance Session in UserManager.UsuariosOnline.Values.ToList())
                    {
                        if (Session.User.Sala == null) continue;
                        if (Session.User.Trayectoria == null) continue;
                        if (Session.User.Trayectoria.Movimientos.Count == 0) continue;
                        if (Session.User.PreLock_Interactuando == true) continue;
                        if (Session.User.PreLock_Caminando == true) continue;
                        if (Session.User.Sala.PathFinder == false) continue;
  
                        Posicion SiguienteMovimiento = Session.User.Trayectoria.SiguienteMovimiento();
                        if (!Session.User.Trayectoria.MovementIsVerifield(SiguienteMovimiento)) continue;
                        if (SiguienteMovimiento.y < Session.User.Sala.MapSizeY && SiguienteMovimiento.x < Session.User.Sala.MapSizeX)
                        {
                            if (Session.User.Sala.Caminable(SiguienteMovimiento))
                            {
                                Session.User.Sala.Map[Session.User.Posicion.y, Session.User.Posicion.x].FijarSession(null);
                                Session.User.PreLock_Caminando = true;
                                Session.User.Posicion = SiguienteMovimiento;
                                Session.User.Sala.Map[Session.User.Posicion.y, Session.User.Posicion.x].FijarSession(Session);
                                ServerMessage server = new ServerMessage();
                                server.AddHead(182);
                                server.AppendParameter(1);
                                server.AppendParameter(Session.User.IDEspacial);
                                server.AppendParameter(SiguienteMovimiento.x);
                                server.AppendParameter(SiguienteMovimiento.y);
                                server.AppendParameter(SiguienteMovimiento.z);
                                server.AppendParameter(750);
                                server.AppendParameter((Session.User.Trayectoria.Movimientos.Count >= 1 ? 1 : 0));
                                Session.User.Sala.SendData(server, Session);


                                if (Session.User.Sala.Sendero != null)
                                {
                                    new Thread(() => Session.User.Sala.Sendero.VerificarMovimiento(Session)).Start();
                                }
                            }
                            else
                            {
                                Session.User.Trayectoria.Movimientos.Clear();
                                Session.User.Trayectoria.BuscarOtroSendero();
                            }
                        }
                        else
                        {
                            Session.User.Trayectoria.Movimientos.Clear();
                            Session.User.Trayectoria.BuscarOtroSendero();
                        }
                    }
                }
                catch
                {
                    continue;
                }
                Thread.Sleep(1);
            }
        }
        private static void PathfinderIA()
        {
            while (true)
            {
                try
                {
                    foreach (SessionInstance Session in UserManager.UsuariosOnline.Values.ToList())
                    {
                        foreach(PersonajeInstance personaje in Session.User.personajesList.ToList())
                        {
                            if (personaje.Trayectoria == null) continue;
                            if (personaje.Trayectoria.Movimientos.Count == 0) continue;
                            if (personaje.PreLock_Interactuando == true) continue;
                            if (personaje.PreLock_Caminando == true) continue;
                            if (Session.User.Sala.PathFinder == false) continue;

                            Posicion SiguienteMovimiento = personaje.Trayectoria.SiguienteMovimiento();
                            if (!personaje.Trayectoria.MovementIsVerifield(SiguienteMovimiento)) continue;
                            if (SiguienteMovimiento.y < Session.User.Sala.MapSizeY && SiguienteMovimiento.x < Session.User.Sala.MapSizeX)
                            {
                                if (Session.User.Sala.Caminable(SiguienteMovimiento))
                                {
                                    Session.User.Sala.Map[personaje.Posicion.y, personaje.Posicion.x].FijarSession(null);
                                    personaje.PreLock_Caminando = true;
                                    personaje.Posicion = SiguienteMovimiento;
                                    Session.User.Sala.Map[personaje.Posicion.y, personaje.Posicion.x].FijarSession(Session);

                                    ServerMessage server = new ServerMessage();
                                    server.AddHead(182);
                                    server.AppendParameter(1);
                                    server.AppendParameter(personaje.id);
                                    server.AppendParameter(SiguienteMovimiento.x);
                                    server.AppendParameter(SiguienteMovimiento.y);
                                    server.AppendParameter(SiguienteMovimiento.z);
                                    server.AppendParameter(750);
                                    server.AppendParameter(personaje.Trayectoria.Movimientos.Count >= 1 ? 1 : 0);
                                    Session.User.Sala.SendData(server, Session);
                                }
                                else
                                {
                                    personaje.Trayectoria.Movimientos.Clear();
                                    personaje.Trayectoria.BuscarOtroSendero();
                                }
                            }
                            else
                            {
                                personaje.Trayectoria.Movimientos.Clear();
                                personaje.Trayectoria.BuscarOtroSendero();
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        private static void mGamesCall_Manager()
        {
            MiniGamesManager.BuscarParticipantes(GameType.Ring, 2);
            MiniGamesManager.BuscarParticipantes(GameType.Ring, 3);
            MiniGamesManager.BuscarParticipantes(GameType.CocosLocos, 8);
            MiniGamesManager.BuscarParticipantes(GameType.CocosLocos, 9);
            MiniGamesManager.BuscarParticipantes(GameType.Sendero, 6);
            MiniGamesManager.BuscarParticipantes(GameType.Sendero, 7);
            MiniGamesManager.BuscarParticipantes(GameType.Camino, 12);
            MiniGamesManager.BuscarParticipantes(GameType.Camino, 13);
        }
    }
}
