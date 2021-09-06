using BoomBang.game.dao;
using BoomBang.game.instances.manager.pathfinding;
using BoomBang.game.manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class UserInstance
    {
        public int id { get; private set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int edad { get; set; }
        public string ip_registro { get; set; }
        public string ip_actual { get; set; }
        string mfecha_registro;
        public string fecha_registro { get { return mfecha_registro; } }
        public int oro { get; set; }
        public int admin { get; set; }
        public string bocadillo { get; set; }
        public string hobby_1 { get; set; }
        public string hobby_2 { get; set; }
        public string hobby_3 { get; set; }
        public string deseo_1 { get; set; }
        public string deseo_2 { get; set; }
        public string deseo_3 { get; set; }
        int mVotos_Legal;
        public int Votos_Legal { get { return mVotos_Legal; } set { mVotos_Legal = value; } }
        int mVotos_Sexy;
        public int Votos_Sexy { get { return mVotos_Sexy; } set { mVotos_Sexy = value; } }
        int mVotos_Simpatico;
        public int Votos_Simpatico { get { return mVotos_Simpatico; } set { mVotos_Simpatico = value; } }
        int mVotosRestantes = 5;
        public int VotosRestantes { get { return mVotosRestantes; } set { mVotosRestantes = value; } }
        public int besos_enviados { get; set; }
        public int besos_recibidos { get; set; }
        public int jugos_enviados { get; set; }
        public int jugos_recibidos { get; set; }
        public int flores_enviadas { get; set; }
        public int flores_recibidas { get; set; }
        public int uppers_enviados { get; set; }
        public int uppers_recibidos { get; set; }
        public int cocos_enviados { get; set; }
        public int cocos_recibidos { get; set; }
        public int rings_ganados { get; set; }
        public int senderos_ganados { get; set; }
        public int puntos_cocos { get; set; }
        public int puntos_ninja { get; set; }
        public string ultima_conexion { get; set; }


        public int IDEspacial { get; set; }
        public SalaInstance Sala { get; set; }
        public Posicion Posicion { get; set; }
        public bool Caminando { get; set; }
        public int UppertSelect { get; set; }
        public int CocoSelect { get; set; }
        public Trayectoria Trayectoria { get; set; }
        public PathFinder PathFinder { get; set; }
        //MiniGames
        public int mGame12ActualPoints { get; set; }
        public Inscripcion CaminoNinja { get; set; }
        public Inscripcion SenderoOculto { get; set; }
        public Inscripcion CocosLocos { get; set; }
        public GameType Game { get; set; }
        public bool Jugando = false;
        public int CocosRestantes = 0;
        //ObjectManager
        public ObjectEditor ObjectEditor { get; set; }
        public string levelup = "";
        public string token_uid = "";
        public PersonajeInstance Personaje;
        public List<PersonajeInstance> personajesList;

        public UserInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.nombre = (string)row["nombre"];
            this.password = (string)row["password"];
            this.email = (string)row["email"];
            this.edad = (int)row["edad"];
            this.ip_registro = (string)row["ip_registro"];
            this.ip_actual = (string)row["ip_actual"];
            this.oro = (int)row["oro"];
            this.admin = (int)row["admin"];
            this.bocadillo = (string)row["bocadillo"];
            this.hobby_1 = (string)row["hobby_1"];
            this.hobby_2 = (string)row["hobby_2"];
            this.hobby_3 = (string)row["hobby_3"];
            this.deseo_1 = (string)row["deseo_1"];
            this.deseo_2 = (string)row["deseo_2"];
            this.deseo_3 = (string)row["deseo_3"];
            this.mVotos_Legal = int.Parse(row["votos_legal"].ToString());
            this.mVotos_Sexy = int.Parse(row["votos_sexy"].ToString());
            this.mVotos_Simpatico = int.Parse(row["votos_simpatico"].ToString());
            this.besos_enviados = (int)row["besos_enviados"];
            this.besos_recibidos = (int)row["besos_recibidos"];
            this.jugos_enviados = (int)row["jugos_enviados"];
            this.jugos_recibidos = (int)row["jugos_recibidos"];
            this.flores_enviadas = (int)row["flores_enviadas"];
            this.flores_recibidas = (int)row["flores_recibidas"];
            this.uppers_enviados = (int)row["uppers_enviados"];
            this.uppers_recibidos = (int)row["uppers_recibidos"];
            this.cocos_enviados = (int)row["cocos_enviados"];
            this.cocos_recibidos = (int)row["cocos_recibidos"];
            this.rings_ganados = (int)row["rings_ganados"];
            this.senderos_ganados = (int)row["senderos_ganados"];
            this.puntos_cocos = (int)row["puntos_cocos"];
            this.puntos_ninja = (int)row["puntos_ninja"];
            this.ultima_conexion = (string)row["ultima_conexion"];
            this.mfecha_registro = row["fecha_registro"].ToString();
            this.UppertSelect = UppertLevel();
            this.CocoSelect = nivel_coco;
            this.token_uid = (string)row["token_uid"];
            this.personajesList = setUserPersonajes();
            this.Personaje = setUserPersonaje();
        }
        private List<PersonajeInstance> setUserPersonajes()
        {
            return PersonajeDAO.getUserPersonajes(this.id);
        }
        private PersonajeInstance setUserPersonaje()
        {
            return PersonajeDAO.getUserPersonaje(this.id);
        }

        #region Packets
        public void loginGame(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(130);
            server.AppendParameter(1);
            server.AppendParameter(this.nombre);
            server.AppendParameter(this.Personaje.avatar);
            server.AppendParameter(this.Personaje.colores);
            server.AppendParameter(this.email);
            server.AppendParameter(this.edad);
            server.AppendParameter(2);
            server.AppendParameter("Hola, Soy nuevo en BoomBang.");
            server.AppendParameter(0);
            server.AppendParameter(this.id);
            server.AppendParameter(this.admin);
            server.AppendParameter(this.oro);
            server.AppendParameter(-1);
            server.AppendParameter(200);
            server.AppendParameter(5);
            server.AppendParameter(0);
            server.AppendParameter(-1);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(0);//tutorial islas
            server.AppendParameter("ES");
            server.AppendParameter(1);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter("");
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(new object[] { 1, 0 });
            server.AppendParameter(0);
            server.AppendParameter(-1);
            Session.SendDataProtected(server);
        }
        #endregion
        #region Data Levels
        public int UppertLevel()
        {
           if (rings_ganados >= 1 && rings_ganados <= 9) return 1;
           if (rings_ganados >= 10 && rings_ganados <= 24) return 2;
           if (rings_ganados >= 25 && rings_ganados <= 49) return 3;
           if (rings_ganados >= 50 && rings_ganados <= 99) return 4;
           if (rings_ganados >= 100 && rings_ganados <= 199) return 5;
           if (rings_ganados >= 200 && rings_ganados <= 499) return 6;
           if (rings_ganados >= 500 && rings_ganados <= 999) return 7;
           if (rings_ganados >= 1000 && rings_ganados <= 1999) return 8;
           if (rings_ganados >= 2000) return 9;
           return 0;
        }
        public int CocoLevel()
        {
            if (puntos_cocos >= 10 && puntos_cocos <= 19) return 1;
            if (puntos_cocos >= 20 && puntos_cocos <= 49) return 2;
            if (puntos_cocos >= 50 && puntos_cocos <= 99) return 3;
            if (puntos_cocos >= 100 && puntos_cocos <= 149) return 4;
            if (puntos_cocos >= 150 && puntos_cocos <= 199) return 5;
            if (puntos_cocos >= 200 && puntos_cocos <= 299) return 6;
            if (puntos_cocos >= 300 && puntos_cocos <= 399) return 7;
            if (puntos_cocos >= 400 && puntos_cocos <= 599) return 8;
            if (puntos_cocos >= 600) return 9;
            return 0;
        }
        public int NinjaLevel()
        {
            if (puntos_ninja >= 400 && puntos_ninja <= 409) return 1;
            if (puntos_ninja >= 410 && puntos_ninja <= 419) return 2;
            if (puntos_ninja >= 420 && puntos_ninja <= 449) return 3;
            if (puntos_ninja >= 450 && puntos_ninja <= 499) return 4;
            if (puntos_ninja >= 500 && puntos_ninja <= 549) return 5;
            if (puntos_ninja >= 550 && puntos_ninja <= 599) return 6;
            if (puntos_ninja >= 600 && puntos_ninja <= 699) return 7;
            if (puntos_ninja >= 700 && puntos_ninja <= 799) return 8;
            if (puntos_ninja >= 800 && puntos_ninja <= 999) return 9;
            if (puntos_ninja >= 1000) return 10;
            return 0;
        }
        public int nivel_coco
        {
            get
            {
                if (puntos_cocos >= 10 && puntos_cocos <= 19) return 1;
                if (puntos_cocos >= 20 && puntos_cocos <= 49) return 2;
                if (puntos_cocos >= 50 && puntos_cocos <= 99) return 3;
                if (puntos_cocos >= 100 && puntos_cocos <= 149) return 4;
                if (puntos_cocos >= 150 && puntos_cocos <= 199) return 5;
                if (puntos_cocos >= 200 && puntos_cocos <= 299) return 6;
                if (puntos_cocos >= 300 && puntos_cocos <= 399) return 7;
                if (puntos_cocos >= 400 && puntos_cocos <= 599) return 8;
                if (puntos_cocos >= 600) return 9;
                return 0;
            }
        }
        public int puntos_coco_limite
        {
            get
            {
                if (puntos_cocos >= 10 && puntos_cocos <= 19) return 20;
                if (puntos_cocos >= 20 && puntos_cocos <= 49) return 50;
                if (puntos_cocos >= 50 && puntos_cocos <= 99) return 100;
                if (puntos_cocos >= 100 && puntos_cocos <= 149) return 150;
                if (puntos_cocos >= 150 && puntos_cocos <= 199) return 200;
                if (puntos_cocos >= 200 && puntos_cocos <= 299) return 300;
                if (puntos_cocos >= 300 && puntos_cocos <= 399) return 400;
                if (puntos_cocos >= 400 && puntos_cocos <= 599) return 600;
                if (puntos_cocos >= 600) return 600;
                return 10;
            }
        }
        public int nivel_ninja
        {
            get
            {
                if (puntos_ninja >= 400 && puntos_ninja <= 409) return 1;
                if (puntos_ninja >= 410 && puntos_ninja <= 419) return 2;
                if (puntos_ninja >= 420 && puntos_ninja <= 449) return 3;
                if (puntos_ninja >= 450 && puntos_ninja <= 499) return 4;
                if (puntos_ninja >= 500 && puntos_ninja <= 549) return 5;
                if (puntos_ninja >= 550 && puntos_ninja <= 599) return 6;
                if (puntos_ninja >= 600 && puntos_ninja <= 699) return 7;
                if (puntos_ninja >= 700 && puntos_ninja <= 799) return 8;
                if (puntos_ninja >= 800 && puntos_ninja <= 999) return 9;
                if (puntos_ninja >= 1000) return 10;
                return 0;
            }
        }
        public int puntos_ninja_limite
        {
            get
            {
                if (puntos_ninja >= 0 && puntos_ninja <= 399) return 400;
                if (puntos_ninja >= 400 && puntos_ninja <= 409) return 410;
                if (puntos_ninja >= 410 && puntos_ninja <= 419) return 420;
                if (puntos_ninja >= 420 && puntos_ninja <= 449) return 450;
                if (puntos_ninja >= 450 && puntos_ninja <= 499) return 500;
                if (puntos_ninja >= 500 && puntos_ninja <= 549) return 550;
                if (puntos_ninja >= 550 && puntos_ninja <= 599) return 600;
                if (puntos_ninja >= 600 && puntos_ninja <= 699) return 700;
                if (puntos_ninja >= 700 && puntos_ninja <= 799) return 800;
                if (puntos_ninja >= 800 && puntos_ninja <= 999) return 1000;
                if (puntos_ninja >= 1000) return 1000;
                return 400;
            }
        }
        #endregion
        #region Locks
        private double Time_SendUppert = Time.TiempoActual();
        public bool PreLock_EnviandoUppert
        {
            get
            {
                if (Time.TiempoActual() > Time_SendUppert)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_SendUppert = Time.GetCurrentAndAdd(AddType.Milisegundos, 175);
            }
            
        }
        private double Time_Notifi = Time.TiempoActual();
        public bool PreLock_Notificacion_IzDer
        {
            get
            {
                if (Time.TiempoActual() > Time_Notifi)
                {
                    return false;
                }
                return true;
            }
        }
        private double Time_Uppert_Click = Time.TiempoActual();
        public bool PreLockUpperClick
        {
            get
            {
                if (Time.TiempoActual() > Time_Uppert_Click)
                {
                    return false;
                }
                return true;
            }
        }
        private double Time_Mirada = Time.TiempoActual();
        public bool PreLock_Mirada
        {
            get
            {
                if (Time.TiempoActual() > Time_Mirada)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Mirada = Time.GetCurrentAndAdd(AddType.Milisegundos, 200);
            }
        }
        private double Time_Acciones = Time.TiempoActual();
        public bool PreLock_Acciones
        {
            get
            {
                if (Time.TiempoActual() > Time_Acciones)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Acciones = Time.GetCurrentAndAdd(AddType.Milisegundos, 100);
            }
        }

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
        
        private double Time_Bloqueo_chat = Time.TiempoActual();
        public bool PreLock_BloqueoChat
        {
            get
            {
                if (Time.TiempoActual() > Time_Bloqueo_chat)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Bloqueo_chat = Time.GetCurrentAndAdd(AddType.Segundos, 1);
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
        private double Time_Ficha = Time.TiempoActual();
        public bool PreLock_Ficha
        {
            get
            {
                if (Time.TiempoActual() > Time_Ficha)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Ficha = Time.GetCurrentAndAdd(AddType.Segundos, 1);
            }
        }
        public double Time_BlockLatency = Time.TiempoActual();
        public bool Prelock_Latency
        {
            get
            {
                if (Time.TiempoActual() > Time_BlockLatency)
                {
                    return false;
                }
                return true;
            }
        }
        private double Time_Upper_Espera = Time.TiempoActual();
        public bool PreLock_Upper_Espera
        {
            get
            {
                if (Time.TiempoActual() > Time_Upper_Espera)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Upper_Espera = Time.GetCurrentAndAdd(AddType.Milisegundos, 156);
            }
        }
        private double Time_Disfraz = Time.TiempoActual();
        public bool PreLock_Disfraz
        {
            get
            {
                if (Time.TiempoActual() > Time_Disfraz)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Disfraz = Time.GetCurrentAndAdd(AddType.Segundos, 6);
            }
        }
        private double Time_Proteccion_SQL = Time.TiempoActual();
        public bool PreLock__Proteccion_SQL
        {
            get
            {
                if (Time.TiempoActual() > Time_Proteccion_SQL)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Proteccion_SQL = Time.GetCurrentAndAdd(AddType.Segundos, 2);
            }
        }
        public double Time_Acciones_Ficha = Time.TiempoActual();
        public bool PreLock_Acciones_Ficha
        {
            get
            {
                if (Time.TiempoActual() > Time_Acciones_Ficha)
                {
                    return false;
                }
                return true;
            }
        }
        private double Time_Sapmm_Areas = Time.TiempoActual();
        public bool PreLock__Spamm_Areas
        {
            get
            {
                if (Time.TiempoActual() > Time_Sapmm_Areas)
                {
                    return false;
                }
                return true;
            }
            set
            {
                Time_Sapmm_Areas = Time.GetCurrentAndAdd(AddType.Segundos, 2);
            }
        }
        public double Time_Cambio_Colores = Time.TiempoActual();
        public bool PreLock_Cambio_Colores
        {
            get
            {
                if (Time.TiempoActual() > Time_Cambio_Colores)
                {
                    return false;
                }
                return true;
            }
        }
        #endregion
    }
}
