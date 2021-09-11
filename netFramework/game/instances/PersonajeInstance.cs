using BoomBang.Forms;
using BoomBang.game.instances.manager.pathfinding;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class PersonajeInstance
    {
        public int id { get; set; }
        public int avatar { get; set; }
        public string color_1 { get; set; }
        public string color_2 { get; set; }
        public string color_3 { get; set; }
        public string color_4 { get; set; }
        public string color_5 { get; set; }
        public string color_6 { get; set; }
        public string color_7 { get; set; }
        public int principal { get; set; }
        public Posicion Posicion { get; set; }
        public Trayectoria Trayectoria { get; set; }
        public PathFinder PathFinder { get; set; }
        //Extra Settings
        public bool startWalk = false;
        public bool activeSendUpper = false;

        public PersonajeInstance(DataRow row, int id)
        {
            this.id = id;
            this.avatar = (int)row["avatar_id"];
            this.color_1 = (string)row["color_1"];
            this.color_2 = (string)row["color_2"];
            this.color_3 = (string)row["color_3"];
            this.color_4 = (string)row["color_4"];
            this.color_5 = (string)row["color_5"];
            this.color_6 = (string)row["color_6"];
            this.color_7 = (string)row["color_7"];
            this.principal = (int)row["principal"];
        }

        public string colores
        {
            get
            {
                return this.color_1 +
                    this.color_2 +
                    this.color_3 +
                    this.color_4 +
                    this.color_5 +
                    this.color_6 +
                    this.color_7;
            }
        }
        public bool isBot()
        {
            if (this.principal == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #region UserInstance Packets
        public void updateDescription(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(158);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.bocadillo);
            Session.User.Sala.SendData(server, Session);
        }
        public void sendAlertBadPosition(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(144);
            Session.SendDataProtected(server);
        }
        public void sendAlertUserInteractuando(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(143);
            server.AppendParameter(1);
            Session.SendDataProtected(server);
        }
        public void sendChangeMiradaPosition(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(135);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.Posicion.x);
            server.AppendParameter(Session.User.Posicion.y);
            server.AppendParameter(Session.User.Posicion.z);
            Session.User.Sala.SendData(server, Session);
        }
        public void sendPrivateChat(SessionInstance Session, SessionInstance otherSession, string message)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(136);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(message);
            server.AppendParameter(Session.User.admin == 1 ? 2 : 1);
            Session.SendData(server);
            otherSession.SendData(server);
        }
        public void sendPublicChat(SessionInstance Session, string message)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(133);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(message);
            server.AppendParameter(Session.User.admin == 1 ? 2 : 1);
            Session.User.Sala.SendData(server, Session);
        }
        public void sendAction(SessionInstance Session, int action)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(134);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(action);
            Session.User.Sala.SendData(server, Session);
        }
        #endregion
        #region IA Packets
        public void sendChatMessage(SessionInstance Session, string message)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(133);
            server.AppendParameter(this.id);
            server.AppendParameter(message);
            server.AppendParameter(Session.User.admin == 1 ? 2 : 1);
            Session.SendData(server);
        }
        public void sendSpecialAction(SessionInstance Session, int action)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(139);
            //Second Perosnaje parameters
            server.AppendParameter(action);
            server.AppendParameter(Session.User.IDEspacial);
            server.AppendParameter(Session.User.Posicion.x);
            server.AppendParameter(Session.User.Posicion.y);
            //First Perosnaje parameters
            server.AppendParameter(this.id);
            server.AppendParameter(this.Posicion.x);
            server.AppendParameter(this.Posicion.y);
            Session.SendData(server);
        }
        #endregion
        #region IA Personaje
        public void putAvatarInArea(SessionInstance Session, bool randomPosition)
        {
            if (Session.User.Sala != null)
            {
                if (randomPosition == true)
                {
                    this.Posicion = setSalaPositionRandoom(Session);
                    Session.User.Sala.Map[this.Posicion.y, this.Posicion.x].FijarSession(Session);
                }

                ServerMessage server = new ServerMessage();
                server.AddHead(128);
                server.AddHead(122);
                server.AppendParameter(this.id);
                server.AppendParameter("IA-"+ this.id);
                server.AppendParameter(this.avatar);
                server.AppendParameter(this.colores);
                server.AppendParameter(this.Posicion.x);//EjeX
                server.AppendParameter(this.Posicion.y);//EjeY
                server.AppendParameter(4);//EjeZ
                server.AppendParameter("BoomBang");
                server.AppendParameter(Session.User.edad);
                server.AppendParameter(SalaInstance.MonthDifference(Convert.ToDateTime(Session.User.fecha_registro), DateTime.Now));//Tiempo registrado   MonthDifference(DateTime.Now, Convert.ToDateTime(Session.User.fecha_registro))
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(Session.User.UppertSelect);
                server.AppendParameter(Session.User.UppertLevel());
                server.AppendParameter(Session.User.CocoSelect);
                server.AppendParameter(Session.User.nivel_coco);
                server.AppendParameter(new object[] {
                Session.User.hobby_1,
                Session.User.hobby_2,
                Session.User.hobby_3
            });
                server.AppendParameter(new object[] {
                Session.User.deseo_1,
                Session.User.deseo_2,
                Session.User.deseo_3
            });
                server.AppendParameter(new object[] {
                Session.User.Votos_Legal,
                Session.User.Votos_Sexy,
                Session.User.Votos_Simpatico
            });
                server.AppendParameter(Session.User.bocadillo);
                server.AppendParameter(new object[] {
                Session.User.besos_enviados,
                Session.User.besos_recibidos,
                Session.User.jugos_enviados,
                Session.User.jugos_recibidos,
                Session.User.flores_enviadas,
                Session.User.flores_recibidas,
                Session.User.uppers_enviados,
                Session.User.uppers_recibidos,
                Session.User.cocos_enviados,
                Session.User.cocos_recibidos,
                "0³"
                + Session.User.rings_ganados
                + "³1³1³1³"
                + Session.User.senderos_ganados
                + "³1³1³"
                + (Session.User.nivel_coco + 1)
                + "³"
                + Session.User.puntos_cocos
                + "³1³"
                + Session.User.puntos_coco_limite
                + "³"
                + Session.User.nivel_ninja
                + "³"
                + Session.User.puntos_ninja
                + "³1³"
                + Session.User.puntos_ninja_limite
            });
                server.AppendParameter(Session.User.admin);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(0);
                server.AppendParameter(this.id);//ID
                Session.SendData(server);
            }
        }
        private Posicion setSalaPositionRandoom(SessionInstance Session)
        {
            Point Location = Session.User.Sala.GetRandomPlace();
            while (!Session.User.Sala.Caminable((new Posicion(Location.X, Location.Y))))
            {
                Location = Session.User.Sala.GetRandomPlace();
            }

            bool verifiedPosition = false;
            while (verifiedPosition == false)
            {
                verifiedPosition = true;
                foreach (PersonajeInstance personaje in Session.User.personajesList.ToList())
                {
                    if (personaje.id != this.id && personaje.Posicion != null)
                    {
                        if (personaje.Posicion.x == Location.X && personaje.Posicion.y == Location.Y)
                        {
                            Location = Session.User.Sala.GetRandomPlace();
                            verifiedPosition = false;
                            break;
                        }
                    }
                }
            }
            return new Posicion(Location.X, Location.Y);
        }
        #endregion
        #region IA Send Upper
        public void sendUpper(SessionInstance Session, PersonajeInstance adversario)
        {
            this.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 14);
            adversario.Time_Interactuando = Time.GetCurrentAndAdd(AddType.Segundos, 17);

            this.Trayectoria.DetenerMovimiento();
            adversario.Trayectoria.DetenerMovimiento();

            new Thread(() => upperKick(Session, adversario)).Start();

            ServerMessage server = new ServerMessage();
            server.AddHead(145);
            server.AppendParameter(4);
            server.AppendParameter(this.id);
            server.AppendParameter(this.Posicion.x);
            server.AppendParameter(this.Posicion.y);
            server.AppendParameter(adversario.id);
            server.AppendParameter(adversario.Posicion.x);
            server.AppendParameter(adversario.Posicion.y);
            Session.User.Sala.SendData(server, Session);
        }
        private void upperKick(SessionInstance Session, PersonajeInstance adversario)
        {
            try
            {
                Thread.Sleep(new TimeSpan(0, 0, 16));

                moveUserStartPostionWalkArea(Session, adversario);
                Session.User.Sala.Map[adversario.Posicion.y, adversario.Posicion.x].FijarSession(null);
                adversario.Posicion.x = Session.User.Sala.Puerta.x;
                adversario.Posicion.y = Session.User.Sala.Puerta.y;
                moveUserStartPostionDoorArea(Session, adversario);
            }
            catch
            {
                return;
            }
        }
        private void moveUserStartPostionWalkArea(SessionInstance Session, PersonajeInstance adversario)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(182);
            server.AppendParameter(1);
            server.AppendParameter(adversario.id);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(adversario.Posicion.z);
            server.AppendParameter(750);
            server.AppendParameter(1);
            Session.User.Sala.SendData(server, Session);
        }
        private void moveUserStartPostionDoorArea(SessionInstance Session, PersonajeInstance adversario)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(135);
            server.AppendParameter(adversario.id);
            server.AppendParameter(Session.User.Sala.Puerta.x);
            server.AppendParameter(Session.User.Sala.Puerta.y);
            server.AppendParameter(4);
            Session.User.Sala.SendData(server, Session);
        }
        #endregion
        #region IA Patchfinding
        public void startAutoWalkIA(SessionInstance Session)
        {
            List<string> points = new List<string>();
            points.Add("10124091340814408157081670817708187");
            points.Add("131921318613176131661315613146");
            points.Add("1215411164111771118711197112071121711227");
            points.Add("10212092020819207182061720516205156051460513605126");
            points.Add("06135071350813509135101351113512135131351413515135161351713518135");
            points.Add("171381613815144141541316412174111841019409198");
            points.Add("12121131311414115151");
            points.Add("1416413174121781117810178");
            points.Add("091620915609146091360912609116091060909609086");
            points.Add("10101111111212112137121471215712167121771218712197122071221712227");
            points.Add("1116210152101461013610126");
            points.Add("1113112135");
            points.Add("131411415115161151771518715197");
            points.Add("141821317213166131561314613136131261311613106");
            points.Add("1212113131141411515115167151771518715197");
            points.Add("1419813198");
            points.Add("1218211188");
            points.Add("1019409198");
            points.Add("101831117312163131531314613136131261311613106");
            points.Add("091340814407148");
            points.Add("0716607156");
            points.Add("07146071360712607116");
            points.Add("1012110137");
            points.Add("1112312125131251412515125");

            this.startWalk = true;
            new Thread(() => autoWalkTimer(Session, points)).Start();
        }
        private void autoWalkTimer(SessionInstance Session, List<string>points)
        {
            while (true)
            {
                var random = new Random();
                setWalkPoint(Session, points[random.Next(points.Count)]);
                Thread.Sleep(random.Next(500, 3000));
            }
        }
        private void setWalkPoint(SessionInstance Session, string positions)
        {
            this.Trayectoria = new Trayectoria(Session, this);
            List<Posicion> ListPositions = posicions(positions);
            this.Trayectoria.EndLocation = new Point(ListPositions[ListPositions.Count - 1].x, ListPositions[ListPositions.Count - 1].y);
            this.Trayectoria.IniciarCaminado(ListPositions);
        }
        private List<Posicion> posicions(string cadena)
        {
            List<Posicion> listaPosiciones = new List<Posicion>();
            int countPositions = cadena.Length / 5;
            for (int a = 1; a <= countPositions; a++)
            {
                string obtenerPosicion = cadena.Substring((a * 5) - 5, 5);
                listaPosiciones.Add(new Posicion(
                    Convert.ToInt32(obtenerPosicion.Substring(0, 2)),
                    Convert.ToInt32(obtenerPosicion.Substring(2, 2)),
                    Convert.ToInt32(obtenerPosicion.Substring(4, 1))));
            }
            return listaPosiciones;
        }
        #endregion
        #region Locks
        private double Time_Caminando = Time.TiempoActual();
        public bool PreLock_Caminando
        {
            get
            {
                if (Time.TiempoActual() >= Time_Caminando)
                {
                    return false;
                }
                return true;
            }
            set
            {
                this.Time_Caminando = Time.GetCurrentAndAdd(AddType.Milisegundos, 680);
            }
        }
        public double Time_Interactuando = Time.TiempoActual();
        public bool PreLock_Interactuando
        {
            get
            {
                if (Time.TiempoActual() > Time_Interactuando)
                {
                    return false;
                }
                return true;
            }
        }
        #endregion
    }
}
