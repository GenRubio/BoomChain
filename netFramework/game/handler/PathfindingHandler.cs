using BoomBang.game.instances;
using BoomBang.game.instances.manager;
using BoomBang.game.instances.manager.pathfinding;
using BoomBang.game.manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    class PathfindingHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(135, new ProcessHandler(MirarZ));
            HandlerManager.RegisterHandler(182, new ProcessHandler(FijarSendero));
        }
        private static void FijarSendero(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    if (Session.User.PreLock_Interactuando == true) return;
                    if (Session.User.Sala.PathFinder == false) return;
                    Session.User.Trayectoria = new Trayectoria(Session);
                    List<Posicion> ListPositions = posicions(Parameters[1, 0]);
                    Session.User.Trayectoria.EndLocation = new Point(ListPositions[ListPositions.Count - 1].x, ListPositions[ListPositions.Count - 1].y);
                    Session.User.Trayectoria.IniciarCaminado(ListPositions);
                    if (Session.User.PreLock_Acciones_Ficha == true) { Session.User.Time_Acciones_Ficha = 0; }
                }
            }
        }
        private static List<Posicion> posicions(string cadena)
        {
            List<Posicion> listaPosiciones = new List<Posicion>();
            int countPositions = cadena.Length / 5;
            for(int a = 1; a <= countPositions; a++)
            {
                string obtenerPosicion = cadena.Substring((a * 5) - 5 , 5);
                listaPosiciones.Add(new Posicion(
                    Convert.ToInt32(obtenerPosicion.Substring(0, 2)),
                    Convert.ToInt32(obtenerPosicion.Substring(2, 2)),
                    Convert.ToInt32(obtenerPosicion.Substring(4, 1))));
            }
            return listaPosiciones;
        }
        public static void Reprar_Mirada_Z (SessionInstance Session)
        {
            Session.User.Posicion.z = 4;
            Packet_135(Session, Session.User.Posicion.x, Session.User.Posicion.y, Session.User.Posicion.z);
        }
        static void MirarZ(SessionInstance Session, string[,] Parameters)
        {
    
            if (Session.User.PreLock_Mirada == true) return;
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    int Z = int.Parse(Parameters[1, 0]);
                    if (Z >= 1 && Z <= 8)
                    {
                        Session.User.PreLock_Mirada = true;
                        Session.User.Posicion.z = Z;
                        Packet_135(Session, Session.User.Posicion.x, Session.User.Posicion.y, Session.User.Posicion.z);
                    }
                }
            }
        }
        private static void Packet_135(SessionInstance Session, int x, int y, int z)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(135);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(x);
            server.AppendParameter(y);
            server.AppendParameter(z);
            Session.User.Sala.SendData(server, Session);
        }
    }
}
