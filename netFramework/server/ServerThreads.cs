
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
            new Thread(Ping).Start();
            new Thread(mGamesCall).Start();
            new Thread(ObjetosSistem).Start();
        }
        private static void ObjetosSistem()
        {
            while (true)
            {
                ObjetosSistem_Manager();
                Thread.Sleep(1000);
            }
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
                                new Thread(() => ConcursosManager.BuscarObjetoCaido(Session, Session.User.Sala)).Start();
                                new Thread(() => TrampasManager.BuscarTrampa(Session)).Start();
                                //new Thread(() => SalasManager.bosque_oso(Session)).Start();
                                if (Session.User.Sala.Sendero != null) new Thread(() => Session.User.Sala.Sendero.VerificarMovimiento(Session)).Start();
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
        private static void ObjetosSistem_Manager()
        {
            foreach (SalaInstance salas_publicas in SalasManager.Salas_Publicas.Values)
            {
                if (salas_publicas.Escenario.modelo == salas_publicas.Escenario.id)
                {
                    if (salas_publicas.Visitantes >= 2)
                    {
                        if (salas_publicas.Escenario.segundos_cofre > 0) { salas_publicas.Escenario.segundos_cofre--; }
                        else//Los contadores de cofres
                        {
                            int cofre_type = new Random().Next(1, 20);
                            if (cofre_type == 10) { ConcursosManager.Lanzar_Objeto(salas_publicas, 1, "segundos_cofre"); }//Oro
                            else if (cofre_type == 5 || cofre_type == 9) { ConcursosManager.Lanzar_Objeto(salas_publicas, 29, "segundos_cofre"); }//Caja Pocion
                            else
                            {
                                ConcursosManager.Lanzar_Objeto(salas_publicas, 2, "segundos_cofre");// Cofre Plata
                            }
                            //Output.WriteLine("Cofre lanzado en area " + salas_publicas.Escenario.nombre);
                        }
                        if (salas_publicas.Escenario.id == 9)//Los contadores de coco - shuriken en Igloo
                        {
                            if (salas_publicas.Escenario.segundos_coco_igloo > 0) { salas_publicas.Escenario.segundos_coco_igloo--; }
                            else//Contador de Coco
                            {
                                ConcursosManager.Lanzar_Objeto(salas_publicas, 5, "segundos_coco_igloo");//Coco

                                string console = "Item coco lanzado en area " + salas_publicas.Escenario.nombre;
                                Emulator.Form.WriteLine(console);
                            }
                            if (salas_publicas.Escenario.segundos_shuriken_igloo > 0) { salas_publicas.Escenario.segundos_shuriken_igloo--; }
                            else//Contador de Shurikens
                            {
                                ConcursosManager.Lanzar_Objeto(salas_publicas, 6, "segundos_shuriken_igloo");//Shuriken
                            }
                        }
                    }
                    ///****************************************************************************************************************************************************************
                    if (salas_publicas.Visitantes > 2)//Los contadores de eventos semanales cocos-shurikens
                    {
                        if (salas_publicas.Escenario.tipo_evento != 0 && salas_publicas.Escenario.segundos_evento_semanal > 0) { salas_publicas.Escenario.segundos_evento_semanal--; }
                        else
                        {
                            if (salas_publicas.Escenario.tipo_evento == 1) { ConcursosManager.Lanzar_Objeto(salas_publicas, 5, "segundos_evento_semanal"); }//Coco
                            if (salas_publicas.Escenario.tipo_evento == 2) { ConcursosManager.Lanzar_Objeto(salas_publicas, 6, "segundos_evento_semanal"); }//Shuriken
                        }
                    }
                    ///****************************************************************************************************************************************************************
                    if (salas_publicas.Escenario.id >= 26 && salas_publicas.Escenario.id <= 55 || salas_publicas.Escenario.id >= 57 && salas_publicas.Escenario.id <= 74 || salas_publicas.Escenario.id >= 78 && salas_publicas.Escenario.id <= 96)
                    {
                        if (salas_publicas.Escenario.segundos_items_cmb > 0) { salas_publicas.Escenario.segundos_items_cmb--; }//Contadores areas Cementerio - Bosque - Madirguera
                        else
                        {
                            if (salas_publicas.Escenario.id >= 26 && salas_publicas.Escenario.id <= 55) //Items Cementerio
                            {
                                int Type = new Random().Next(1, 3);
                                int ListaObjetos = 0;
                                if (Type == 1) { ListaObjetos = new Random().Next(7, 13); }
                                if (Type == 2) { ListaObjetos = new Random().Next(14, 22); }
                                ConcursosManager.Lanzar_Objeto(salas_publicas, ListaObjetos, "segundos_items_cmb");
                            }
                            if (salas_publicas.Escenario.id >= 57 && salas_publicas.Escenario.id <= 74)//Items Bosque
                            {
                                int Objeto_Bosque = new Random().Next(1, 6);
                                if (Objeto_Bosque == 3)
                                {
                                    ConcursosManager.Lanzar_Objeto(salas_publicas, 22, "segundos_items_cmb");
                                } //Donut congelado
                                else
                                {
                                    ConcursosManager.Lanzar_Objeto(salas_publicas, 23, "segundos_items_cmb");
                                } //Bola de nieve
                            }
                            if (salas_publicas.Escenario.id >= 78 && salas_publicas.Escenario.id <= 96)//Items Madriguera
                            {
                                ConcursosManager.Lanzar_Objeto(salas_publicas, 24, "segundos_items_cmb");
                            }
                        }
                    }
                }
            }
            foreach (SalaInstance salas_privadas in SalasManager.Salas_Privadas.Values)
            {
                if (salas_privadas.Visitantes > 0)//Items que caen en islas
                {
                    if (salas_privadas.Escenario.segundos_cofre > 0) { salas_privadas.Escenario.segundos_cofre--; }
                    else
                    {
                        int cofre_type = new Random().Next(1, 20);
                        if (cofre_type == 10) { ConcursosManager.Lanzar_Objeto(salas_privadas, 1, "segundos_cofre"); }//Oro
                        else if (cofre_type == 5 || cofre_type == 9) { ConcursosManager.Lanzar_Objeto(salas_privadas, 29, "segundos_cofre"); }//Caja Pocion
                        else
                        {
                            ConcursosManager.Lanzar_Objeto(salas_privadas, 2, "segundos_cofre");// Cofre Plata
                        }
                    }
                }
            }
        }
    }
}
