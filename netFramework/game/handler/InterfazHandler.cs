using BoomBang.Forms;
using BoomBang.game.dao;
using BoomBang.game.instances;
using BoomBang.game.instances.manager;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoomBang.game.handler
{
    class InterfazHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(133, new ProcessHandler(ChatPublico));
            HandlerManager.RegisterHandler(136, new ProcessHandler(ChatPrivado));
            HandlerManager.RegisterHandler(134, new ProcessHandler(Acciones));
            HandlerManager.RegisterHandler(145, new ProcessHandler(UppertPower));
            HandlerManager.RegisterHandler(131, new ProcessHandler(CambiarCoco));
            HandlerManager.RegisterHandler(149, new ProcessHandler(Coco));
            HandlerManager.RegisterHandler(129, new ProcessHandler(Cambiar_Uppercut));
            HandlerManager.RegisterHandler(137, new ProcessHandler(Enviar_Interaccion));
            HandlerManager.RegisterHandler(141, new ProcessHandler(Cancelar_Interaccion));
            HandlerManager.RegisterHandler(140, new ProcessHandler(Rechazar_Interaccion));
            HandlerManager.RegisterHandler(139, new ProcessHandler(Aceptar_Interaccion));
            HandlerManager.RegisterHandler(158, new ProcessHandler(updateDescription));
            HandlerManager.RegisterHandler(156, new ProcessHandler(updateHobbies));
            HandlerManager.RegisterHandler(157, new ProcessHandler(updateDeseos));
            HandlerManager.RegisterHandler(167, new ProcessHandler(Votos_Restantes));
            HandlerManager.RegisterHandler(155, new ProcessHandler(Votos));
            HandlerManager.RegisterHandler(210125, new ProcessHandler(None_210_125));
            HandlerManager.RegisterHandler(142, new ProcessHandler(unknown));
            HandlerManager.RegisterHandler(138, new ProcessHandler(unknown));
        }
        private static void unknown(SessionInstance Session, string[,] Parameter){}
        static void None_210_125(SessionInstance Session, string[,] Parameter)
        {
             Packet_210_125(Session);
        }
        private static void Votos(SessionInstance Session, string[,] Parameters)
        {
            if (Session != null
                && Session.User != null
                && Session.User.Sala != null
                && Session.User.VotosRestantes >= 1)
            {
                int userId = int.Parse(Parameters[0, 0]);
                int tipoVoto = int.Parse(Parameters[1, 0]);
                int cantidad = int.Parse(Parameters[2, 0]);

                SessionInstance OtherSession = Session.User.Sala.ObtenerSession(userId);

                if (OtherSession != null
                    && OtherSession.User.Sala != null
                    && Session.User.Sala.Escenario.id == OtherSession.User.Sala.Escenario.id
                    && Session.User.id != OtherSession.User.id)
                {
                    updateVotos(OtherSession, Parameters);
                    sendVotosPacket(Session, userId, tipoVoto, cantidad);
                }
            }
        }
        private static void sendVotosPacket(SessionInstance Session, int UserID, int Box_ID, int Value)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(155);
            server.AppendParameter(UserID);
            server.AppendParameter(Box_ID);
            server.AppendParameter(Value);
            Session.User.Sala.SendData(server, Session);
        }
        private static void updateVotos(SessionInstance Session, string[,] Parameter)
        {
            int tipoVoto = int.Parse(Parameter[1, 0]);
            int cantidad = int.Parse(Parameter[2, 0]);

            switch (tipoVoto)
            {
                case 1:
                    Session.User.Votos_Legal += int.Parse(Parameter[2, 0]);
                    new Thread(() => AvatarDAO.updateVotosLegal(Session.User.id, Session.User.Votos_Legal)).Start();
                    break;
                case 2:
                    Session.User.Votos_Sexy += int.Parse(Parameter[2, 0]);
                    new Thread(() => AvatarDAO.updateVotosSexy(Session.User.id, Session.User.Votos_Sexy)).Start();
                    break;
                case 3:
                    Session.User.Votos_Simpatico += int.Parse(Parameter[2, 0]);
                    new Thread(() => AvatarDAO.updateVotosSimpatico(Session.User.id, Session.User.Votos_Simpatico)).Start();
                    break;
            }
        }
        static void Votos_Restantes(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    Packet_167(Session);
                }
            }
        }
        private static void updateDeseos(SessionInstance Session, string[,] Parameters)
        {
            int deseo = int.Parse(Parameters[1, 0]);
            string value = Parameters[2, 0];

            if (Session.User != null
                && Session.User.Sala != null
                && Session.User.PreLock_Ficha != true
                && Utils.checkValidCharacters(value, false) == true)
            {
                switch (deseo)
                {
                    case 1:
                        new Thread(() => AvatarDAO.updateDeseo1(Session.User.id, value)).Start();
                        Session.User.deseo_1 = value;
                        break;
                    case 2:
                        new Thread(() => AvatarDAO.updateDeseo2(Session.User.id, value)).Start();
                        Session.User.deseo_2 = value;
                        break;
                    case 3:
                        new Thread(() => AvatarDAO.updateDeseo3(Session.User.id, value)).Start();
                        Session.User.deseo_3 = value;
                        break;
                    default:
                        return;
                }

                updateUserDeseosPacket(Session, value, deseo);
                Session.User.PreLock_Ficha = true;
            }
        }
        private static void updateUserDeseosPacket(SessionInstance Session, string value, int deseo)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(157);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(deseo);
            server.AppendParameter(value);
            Session.User.Sala.SendData(server, Session);
        }

        private static void updateHobbies(SessionInstance Session, string[,] Parameters)
        {
            int hobby = int.Parse(Parameters[1, 0]);
            string value = Parameters[2, 0];

            if (Session.User != null
                && Session.User.Sala != null
                && Session.User.PreLock_Ficha != true
                && Utils.checkValidCharacters(value, false) == true)
            {
                switch (hobby)
                {
                    case 1:
                        new Thread(() => AvatarDAO.updateHobby1(Session.User.id, value)).Start();
                        Session.User.hobby_1 = value;
                        break;
                    case 2:
                        new Thread(() => AvatarDAO.updateHobby2(Session.User.id, value)).Start();
                        Session.User.hobby_2 = value;
                        break;
                    case 3:
                        new Thread(() => AvatarDAO.updateHobby3(Session.User.id, value)).Start();
                        Session.User.hobby_3 = value;
                        break;
                    default:
                        return;
                }
                updateUserHobbysPacket(Session, value, hobby);
                Session.User.PreLock_Ficha = true;
            }
            else
            {
                Utils.errorMessage(Session);
            }
        }
        private static void updateUserHobbysPacket(SessionInstance Session, string value, int hobby)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(156);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(hobby);
            server.AppendParameter(value);
            Session.User.Sala.SendData(server, Session);
        }
        private static void updateDescription(SessionInstance Session, string[,] Parameters)
        {
            string description = Parameters[1, 0];

            if (Session.User != null
                && Session.User.Sala != null
                && Session.User.PreLock_Ficha != true
                && Utils.checkValidCharacters(description, false) == true)
            {
                new Thread(() => AvatarDAO.updateDescription(Session.User.id, description)).Start();

                Session.User.bocadillo = description;
                Session.User.PreLock_Ficha = true;

                updateUserDescriptionPacket(Session);
            }
            else
            {
                Utils.errorMessage(Session);
            }
        }
        private static void updateUserDescriptionPacket(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(158);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.bocadillo);
            Session.User.Sala.SendData(server, Session);
        }
        static void Aceptar_Interaccion(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    if (Session.User.Sala.Escenario.es_categoria == 2) return;
                   Aceptar_Interaccion_Manager(Session, Parameters);
                }
            }
        }
        static void Rechazar_Interaccion(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    if (Session.User.Sala.Escenario.es_categoria == 2) return;
                    SessionInstance OtherSession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[1, 0]));
                    if (OtherSession != null)
                    {
                        if (Session.User.Sala.EliminarInteraccion(OtherSession.User.IDEspacial, Session.User.IDEspacial))
                        {
                             Packet_140(Session, OtherSession, int.Parse(Parameters[0, 0]));
                        }
                    }
                }
            }
        }
        static void Cancelar_Interaccion(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    SessionInstance OtherSession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[2, 0]));
                    if (OtherSession != null)
                    {
                        if (Session.User.Sala.Escenario.es_categoria == 2) return;
                        if (Session.User.Sala.Escenario.id != OtherSession.User.Sala.Escenario.id) return;
                        if (Session.User.Sala.EliminarInteraccion(Session.User.IDEspacial, OtherSession.User.IDEspacial))
                        {
                             Packet_141(Session, OtherSession, int.Parse(Parameters[0, 0]));
                        }
                    }
                }
            }
        }
        static void Enviar_Interaccion(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    SessionInstance OtherSession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[2, 0]));
                    if (OtherSession != null)
                    {
                        if (Session.User.Sala.Escenario.es_categoria == 2) return;
                        if (Session.User.Sala.Escenario.id != OtherSession.User.Sala.Escenario.id) return;
                        if (Session.User.Sala.AñadirInteraccion(Session.User.IDEspacial, OtherSession.User.IDEspacial))
                        {
                             Packet_137(Session, OtherSession, int.Parse(Parameters[0, 0]));
                        }
                    }
                }
            }
        }
        static void Cambiar_Uppercut(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    int Nivel = int.Parse(Parameters[0, 0]);
                    if (Nivel < 0) return;
                    if (Nivel > Session.User.UppertLevel()) return;
                    Session.User.UppertSelect = Nivel;
                     Packet_129(Session);
                }
            }
        }
        static void CambiarCoco(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    int nivel = int.Parse(Parameters[0, 0]);
                    if (nivel >= 0 && nivel <= (Session.User.nivel_coco + 1))
                    {
                        Session.User.CocoSelect = nivel;
                         Packet_131(Session, Session.User.CocoSelect);
                    }
                }
            }
        }
        static void Coco(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    if (Session.User.Sala.Escenario.coco == -1) { return; }
                    if (Session.User.Sala.Escenario.es_categoria == 2)
                    {
                        if (Session.User.Sala.Ring != null)
                        {
                            if (Session.User.Sala.Ring.Iniciado == false) return;
                            if (Session.User.Sala.Ring.Participando(Session) == false) return;
                        }
                        if (Session.User.Sala.Cocos != null)
                        {
                            if (Session.User.Sala.Cocos.Iniciado == false) return;
                            if (Session.User.Sala.Cocos.Participando(Session) == false) return;
                            if (Session.User.Sala.PathFinder == false) return;
                        }
                        if (Session.User.Sala.Sendero != null)
                        {
                            if (Session.User.Sala.Sendero.Iniciado == false) return;
                            if (Session.User.Sala.Sendero.Participando(Session) == false) return;
                            if (Session.User.Sala.PathFinder == false) return;
                        }
                        if (Session.User.Sala.Camino != null)
                        {
                            if (Session.User.Sala.Camino.Iniciado == false) return;
                            if (Session.User.Sala.Camino.Participando(Session) == false) return;
                            if (Session.User.Sala.PathFinder == false) return;
                        }
                    }
                    if (Session.User.oro >= 0)
                    {
                        if (Session.User.Sala.Escenario.anti_coco == true) { NotificacionesManager.NotifiChat(Session, "Sabio: Los cocos estan desactivados en esta Sala."); return;  }
                        SessionInstance MySession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[0, 0]));
                        SessionInstance OtherSession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[1, 0]));
                        if (MySession != null && OtherSession != null)
                        {
                            if (MySession.User.id == OtherSession.User.id) { Session.FinalizarConexion("Coco"); return; }
                            if (MySession.User.id != Session.User.id) { Session.FinalizarConexion("Coco"); return; }
                            if (MySession.User.Sala.Escenario.id != OtherSession.User.Sala.Escenario.id) return;
                            if (OtherSession.User.PreLock_Interactuando != true)
                            {
                                Coco(Session, OtherSession, 0);
                            }
                            else
                            {
                                 Packet_143(Session);
                            }
                        }
                    }
                }
            }
        }
        public static List<int> Cada_X_Goldens = new List<int>();
        private static void UppertPower(SessionInstance Session, string[,] Parameters)
        {
            try
            {
                if (Session.User != null)
                {
                    if (Session.User.Sala != null)
                    {
                        if (Session.User.Sala.Usuarios.ContainsKey(int.Parse(Parameters[1, 0]))
                            && Session.User.Sala.Usuarios.ContainsKey(int.Parse(Parameters[4, 0])))
                        {
                            SessionInstance OtherSession = Session.User.Sala.Usuarios[int.Parse(Parameters[4, 0])];
                            if (Session.User.Posicion.x != int.Parse(Parameters[2, 0])
                                || Session.User.Posicion.y != int.Parse(Parameters[3, 0]))
                            {
                                return;
                            }
                            if (OtherSession.User.Posicion.x != int.Parse(Parameters[5, 0])
                                || OtherSession.User.Posicion.y != int.Parse(Parameters[6, 0]))
                            {
                                return;
                            }
                            if (Session.User.Sala.Escenario.uppert == -1
                                || Session.User.IDEspacial == int.Parse(Parameters[4, 0]))
                            {
                                Session.FinalizarConexion("UppertPower");
                                return;
                            }

                            int Derecha = Session.User.Posicion.x - OtherSession.User.Posicion.x;
                            int Izquierda = Session.User.Posicion.y - OtherSession.User.Posicion.y;
                            if (Derecha == 1 && Izquierda == 1)
                            {
                                if (Session.User.PreLock_Interactuando != true
                                    && OtherSession.User.PreLock_Interactuando != true)
                                {
                                    Session.User.Trayectoria.DetenerMovimiento();
                                    OtherSession.User.Trayectoria.DetenerMovimiento();
                                    UppertPower(Session, OtherSession, Parameters);

                                }
                                else
                                {
                                    Packet_143(Session);
                                }
                            }
                            else
                            {
                                Packet_144(Session);
                            }
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private static void Acciones(SessionInstance Session, string[,] Parameters)
        {
            int accion = int.Parse(Parameters[1, 0]);

            if (Session.User != null
                && Session.User.Sala != null
            
                && Session.User.PreLock_Acciones_Ficha != true
                && accion >= 1 && accion <= 8)
            {
                switch (accion)
                {
                    case 1:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 2);
                        break;
                    case 2:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 3);
                        break;
                    case 3:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 3);
                        break;
                    case 4:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 2);
                        break;
                    case 5:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 2);
                        break;
                    case 6:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 4);
                        break;
                    case 7:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 3);
                        break;
                    case 8:
                        Session.User.Time_Acciones_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 8);
                        break;
                }
                Session.User.Personaje.sendAction(Session, accion);
            }
        }
        private static void ChatPrivado(SessionInstance Session, string[,] Parameters)
        {
            int userId = int.Parse(Parameters[0, 0]);
            string mensaje = Parameters[1, 0];

            if (Session.User.Sala != null
               && Session.User.PreLock_BloqueoChat == false
               && Utils.checkValidCharacters(mensaje, false))
            {
                SessionInstance otherSession = Session.User.Sala.ObtenerSession(userId);
                if (otherSession != null)
                {
                    Session.User.Personaje.sendPrivateChat(Session, otherSession, mensaje);
                }
                Session.User.PreLock_BloqueoChat = true;
            }
        }
        private static void ChatPublico(SessionInstance Session, string[,] Parameters)
        {
            string mensaje = Parameters[1, 0];

            if (Session.User.Sala != null
                && Session.User.PreLock_BloqueoChat == false
                && Utils.checkValidCharacters(mensaje, false))
            {
                Session.User.Personaje.sendPublicChat(Session, mensaje);

                if (Session.User.admin == 1)
                {
                    adminChatCommands(Session, mensaje);
                }

                Session.User.PreLock_BloqueoChat = true;
            }
            else
            {
                Utils.errorMessage(Session);
            }
        }
        private static void adminChatCommands(SessionInstance Session, string mensaje)
        {
            if (mensaje == "putAvatars")
            {
                foreach (PersonajeInstance personaje in Session.User.personajesList.ToList())
                {
                    personaje.putAvatarInArea(Session, true);
                }
            }
            if (mensaje == "chatAvatar")
            {
                Session.User.Personaje.sendChatMessage(Session, "Hola");
            }
            if (mensaje == "action")
            {
                Session.User.Personaje.sendAction(Session, 1);
            }
            if (mensaje == "specialAction")
            {
                Session.User.Personaje.sendSpecialAction(Session, 1);
            }
            if (mensaje == "autoWalk")
            {
                foreach (PersonajeInstance personaje in Session.User.personajesList.ToList())
                {
                    personaje.startAutoWalkIA(Session);
                }
            }
            if (mensaje == "sendUpper")
            {
                foreach (PersonajeInstance personaje in Session.User.personajesList.ToList())
                {
                    personaje.activeSendUpper = true;
                }
            }
            if (mensaje == "objeto")
            {
                ServerMessage server = new ServerMessage();
                server.AddHead(189);
                server.AddHead(136);
                server.AppendParameter(2246);
                server.AppendParameter(84);
                server.AppendParameter(1);
                server.AppendParameter(Session.User.id);
                server.AppendParameter(11);
                server.AppendParameter(11);
                server.AppendParameter(0);
                server.AppendParameter("tam_n");
                server.AppendParameter("");
                server.AppendParameter("14,19");
                server.AppendParameter("37F2FF");
                server.AppendParameter("72,72,100");
                server.AppendParameter("0");
                server.AppendParameter("0");
                server.AppendParameter("");
                Session.User.Sala.SendData(server, Session);
            }
        }
        private static void UppertPower(SessionInstance Session, SessionInstance OtherSession, string[,] Parameters)
        {
            if (OtherSession.User.Posicion.x == Session.User.Sala.Puerta.x && Session.User.Sala.Usuarios[int.Parse(Parameters[4, 0])].User.Posicion.y == Session.User.Sala.Puerta.y || Session.User.Posicion.x == Session.User.Sala.Puerta.x && Session.User.Posicion.y == Session.User.Sala.Puerta.y)
            {
                return;
            }
            Session.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 14);
            OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 17);
            Session.User.Trayectoria.DetenerMovimiento();
            OtherSession.User.Trayectoria.DetenerMovimiento();

            if (Session.User.Sala.Escenario.es_categoria == 2)
            {
                if (Session.User.Sala.Ring != null)
                {
                    if (Session.User.Sala.Ring.Iniciado == false) return;
                    Session.User.Sala.Ring.Descalificar(OtherSession);
                    new Thread(() => Uppert_Kick(OtherSession, Session.User.Sala, false)).Start();
                    Session.User.PreLock_Disfraz = true;

                    Session.User.uppers_enviados++;
                    OtherSession.User.uppers_recibidos++;
                    Session.User.Sala.ActualizarEstadisticas(Session.User);
                    Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
                }
            }
            else
            {
                new Thread(() => Uppert_Kick(OtherSession, Session.User.Sala)).Start();
                Session.User.PreLock_Disfraz = true;
               
                Session.User.uppers_enviados++;
                OtherSession.User.uppers_recibidos++;
                Session.User.Sala.ActualizarEstadisticas(Session.User);
                Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
            }
            if (Session.User.uppers_enviados == 25 || Session.User.uppers_enviados == 50 || Session.User.uppers_enviados == 100 || Session.User.uppers_enviados == 200 || Session.User.uppers_enviados == 500 || Session.User.uppers_enviados == 1500 || Session.User.uppers_enviados == 3000 || Session.User.uppers_enviados == 6000 || Session.User.uppers_enviados == 9000)
            {
                NotificacionesManager.NotifiChat(Session, "Sabio: Felicidades has subido de Upper :)");
                Session.User.UppertSelect = Session.User.UppertLevel();
                NotificacionesManager.Juegos(Session, 2);
            }
            Packet_145(Session, OtherSession);
        }
        private static void Coco(SessionInstance Session, SessionInstance OtherSession, int coco)
        {
            bool coco_viejo = false;
            if (OtherSession.User.Posicion.x == Session.User.Sala.Puerta.x && OtherSession.User.Posicion.y == Session.User.Sala.Puerta.y || Session.User.Posicion.x == Session.User.Sala.Puerta.x && Session.User.Posicion.y == Session.User.Sala.Puerta.y)
            {
                return;
            }
            OtherSession.User.Trayectoria.DetenerMovimiento();
            if (Session.User.Sala.Escenario.es_categoria != 2)
            {
                switch (Session.User.CocoSelect)
                {
                    case 0:
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 6);
                        coco = 35;
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 6, 0), 35, Session.User.Sala)).Start();
                        break;
                    case 1:
                        coco = 40;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 2);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 2, 0), 40, Session.User.Sala)).Start();
                        break;
                    case 2:
                        coco = 39;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 4750);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 4, 750), 39, Session.User.Sala)).Start();
                        break;
                    case 3:
                        coco = 38;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 9300);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 9, 300), 38, Session.User.Sala)).Start();
                        break;
                    case 4:
                        coco = 32;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 7);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 7, 0), 32, Session.User.Sala)).Start();
                        break;
                    case 5:
                        coco = 34;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 14800);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 14, 800), 34, Session.User.Sala)).Start();
                        break;
                    case 6:
                        coco = 37;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 13200);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 13, 200), 37, Session.User.Sala)).Start();
                        break;
                    case 7:
                        coco = 33;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 9800);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 9, 800), 33, Session.User.Sala)).Start();
                        break;
                    case 8:
                        coco = 36;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 13700);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 13, 700), 36, Session.User.Sala)).Start();
                        break;
                    case 9:
                        coco = 41;
                        OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Milisegundos, 16800);
                        new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 16, 800), 41, Session.User.Sala)).Start();
                        break;

                    default: return;
                }
            }
            else
            {
                if (Session.User.CocosRestantes == 0) return;
                Session.User.CocosRestantes--;
                if (Session.User.CocosRestantes == 0)
                {
                    ServerMessage server3 = new ServerMessage();
                    server3.AddHead(175);
                    server3.AppendParameter(new object[] { 5, -1, 0 });
                    Session.SendData(server3);
                }
                coco = 35;
                OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 6);
                new Thread(() => Coco_Thread(OtherSession, new TimeSpan(0, 0, 0, 6, 0), 35, Session.User.Sala)).Start();
            }
            if (!coco_viejo == true) { Packet_184_120(Session, OtherSession, coco); }
            else { Packet_149(OtherSession); }
            if (Session.User.Sala.Escenario.es_categoria != 2)
            {
                OtherSession.User.Trayectoria.DetenerMovimiento();
                OtherSession.User.cocos_recibidos++;
                Session.User.cocos_enviados++;
                Session.User.Sala.ActualizarEstadisticas(Session.User);
                Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
            }
        }
        private static void Aceptar_Interaccion_Manager(SessionInstance Session, string[,] Parameters)
        {
            SessionInstance OtherSession = Session.User.Sala.ObtenerSession(int.Parse(Parameters[1, 0]));
            if (OtherSession != null)
            {
                if (!Session.User.Sala.ValidarInteraccion(OtherSession.User.IDEspacial, Session.User.IDEspacial)) return;
                if (!Session.User.PreLock_Interactuando && !OtherSession.User.PreLock_Interactuando)
                {
                    int Derecha = Session.User.Posicion.x - OtherSession.User.Posicion.x;
                    int Izquierda = Session.User.Posicion.y - OtherSession.User.Posicion.y;
                    if (Derecha == 1 && Izquierda == 1 || Derecha == -1 && Izquierda == -1)
                    {
                        Session.User.Sala.EliminarInteraccion(OtherSession.User.IDEspacial, Session.User.IDEspacial);
                        switch (int.Parse(Parameters[0, 0]))
                        {
                            case 1:
                                Session.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 3);
                                OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 3);
                                Session.User.Trayectoria.DetenerMovimiento();
                                OtherSession.User.Trayectoria.DetenerMovimiento();
                                Session.User.besos_recibidos++;
                                OtherSession.User.besos_enviados++;
                                Session.User.Sala.ActualizarEstadisticas(Session.User);
                                Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
                                break;
                            case 2:
                                Session.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 11);
                                OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 11);
                                Session.User.Trayectoria.DetenerMovimiento();
                                OtherSession.User.Trayectoria.DetenerMovimiento();
                                Session.User.jugos_recibidos++;
                                OtherSession.User.jugos_enviados++;
                                Session.User.Sala.ActualizarEstadisticas(Session.User);
                                Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
                                break;
                            case 3:
                                Session.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 4);
                                OtherSession.User.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 4);
                                Session.User.Trayectoria.DetenerMovimiento();
                                OtherSession.User.Trayectoria.DetenerMovimiento();
                                Session.User.flores_recibidas++;
                                OtherSession.User.flores_enviadas++;
                                Session.User.Sala.ActualizarEstadisticas(Session.User);
                                Session.User.Sala.ActualizarEstadisticas(OtherSession.User);
                                break;
                        }
                        Packet_139(Session, OtherSession, int.Parse(Parameters[0, 0]));
                    }
                }
                else
                {
                    Packet_143(Session);
                }
            }
        }
     
        public static void Coco_Thread(SessionInstance Session, TimeSpan Tiempo, int modelo, SalaInstance Sala, Posicion Posicion = null)
        {
            Thread.Sleep(Tiempo);
            if (Session.User != null)
            {
                if (Session.User.Sala != null)
                {
                    if (Session.User.Sala.id == Sala.id && Session.User.Sala.Escenario.es_categoria == Sala.Escenario.es_categoria)
                    {
                        Packet_184_121(Session, modelo);
                    }
                }
            }
            if (Posicion != null)
            {
                Session.User.Posicion = Posicion;
                Packet_182(Session, Session.User.Posicion.x, Session.User.Posicion.y, Session.User.Posicion.z);
            }
        }
        static void Uppert_Kick(SessionInstance Session, SalaInstance Sala, bool kick = true)
        {
            try
            {
                Thread.Sleep(new TimeSpan(0, 0, 16));
                if (Session.User.Sala.id == Sala.id && Session.User.Sala.Escenario.es_categoria == Sala.Escenario.es_categoria)
                {
                    if (kick)
                    {
                       SalasManager.Salir_Sala(Session, true);
                    }
                    else
                    {
                        Session.User.Sala.Map[Session.User.Posicion.y, Session.User.Posicion.x].FijarSession(null);
                        Session.User.Posicion = new Posicion(0, 0, 4);

                        Packet_182(Session, Session.User.Posicion.x, Session.User.Posicion.y, Session.User.Posicion.z);
                    }
                }
            }
            catch
            {
                return;
            } 
        }
        private static void Packet_210_125(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(210);
            server.AddHead(125);
            server.AppendParameter(1);
            server.AppendParameter(1);
            Session.SendData(server);
        }
        private static void Packet_167(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(167);
            server.AppendParameter(Session.User.VotosRestantes);
            Session.User.Sala.SendData(server, Session);
        }
       
        private static void Packet_143(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(143);
            server.AppendParameter(1);
            Session.SendDataProtected(server);
        }
        private static void Packet_140(SessionInstance Session, SessionInstance OtherSession, int InteraccionID)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(140);
            server.AppendParameter(InteraccionID);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(1);
            OtherSession.SendDataProtected(server);
        }
        private static void Packet_141(SessionInstance Session, SessionInstance OtherSession, int InteraccionID)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(141);
            server.AppendParameter(InteraccionID);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(1);
            OtherSession.SendDataProtected(server);
        }
        private static void Packet_137(SessionInstance Session, SessionInstance OtherSession, int InteraccionID)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(137);
            server.AppendParameter(InteraccionID);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(1);
            OtherSession.SendDataProtected(server);
        }
        private static void Packet_129(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(129);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(Session.User.UppertSelect);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_131(SessionInstance Session, int nivel)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(131);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(nivel);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_144(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(144);
            Session.SendDataProtected(server);
        }
       
        private static void Packet_145(SessionInstance Session, SessionInstance OtherSession)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(145);
            server.AppendParameter(4);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.Posicion.x);
            server.AppendParameter(Session.User.Posicion.y);
            server.AppendParameter(OtherSession.User.IDEspacial);
            server.AppendParameter(OtherSession.User.Posicion.x);
            server.AppendParameter(OtherSession.User.Posicion.y);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_184_120(SessionInstance Session, SessionInstance OtherSession, int coco)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(184);
            server.AddHead(120);
            server.AppendParameter(OtherSession.User.id);
            server.AppendParameter(0);
            server.AppendParameter(coco);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_139(SessionInstance Session, SessionInstance OtherSession, int InteraccionID)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(139);
            server.AppendParameter(InteraccionID);
            server.AppendParameter(OtherSession.User.IDEspacial);
            server.AppendParameter(OtherSession.User.Posicion.x);
            server.AppendParameter(OtherSession.User.Posicion.y);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.Posicion.x);
            server.AppendParameter(Session.User.Posicion.y);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_149(SessionInstance Session)
        {
            ServerMessage cocold = new ServerMessage();
            cocold.AddHead(149);
            cocold.AppendParameter(1);
            cocold.AppendParameter(Session.User.IDEspacial);
            cocold.AppendParameter(1);
            Session.User.Sala.SendData(cocold);
        }
        private static void Packet_184_121(SessionInstance Session, int modelo)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(184);
            server.AddHead(121);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(0);
            server.AppendParameter(modelo);
            Session.User.Sala.SendData(server, Session);
        }
        private static void Packet_182(SessionInstance Session, int x, int y, int z)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(182);
            server.AppendParameter(1);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(x);
            server.AppendParameter(y);
            server.AppendParameter(z);
            server.AppendParameter(750);
            server.AppendParameter(1);
            Session.User.Sala.SendData(server, Session);
        }
    }
}
