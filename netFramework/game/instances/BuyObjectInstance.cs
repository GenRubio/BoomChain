using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class BuyObjectInstance
    {
        public int id { get; private set; }
        public int objeto_id { get; set; }
        public int sala_id { get; set; }
        public int usuario_id { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public string colores_hex { get; set; }
        public string colores_rgb { get; set; }
        public int rotation { get; set; }
        public string tam { get; set; }
        public string espacio_ocupado { get; set; }
        public double Planta_sol { get; set; }
        public double Planta_agua { get; set; }
        public BuyObjectInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.objeto_id = (int)row["objeto_id"];
            this.sala_id = (int)row["sala_id"];
            this.usuario_id = (int)row["usuario_id"];
            this.posX = (int)row["posX"];
            this.posY = (int)row["posY"];
            this.colores_hex = (string)row["colores_hex"];
            this.colores_rgb = (string)row["colores_rgb"];
            this.rotation = (int)row["rotation"];
            this.tam = (string)row["tam"];
            this.espacio_ocupado = (string)row["espacio_ocupado"];
            this.Planta_sol = (int)row["planta_sol"];
            this.Planta_agua = (int)row["planta_agua"];
        }
        public BuyObjectInstance(int id,
            int objeto_id,
            int sala_id,
            int usuario_id,
            int posX,
            int posY,
            string colores_hex,
            string colores_rgb,
            int rotation,
            string tam,
            string espacio_ocupado,
            int Planta_sol,
            int Planta_agua)
        {
            this.id = id;
            this.objeto_id = objeto_id;
            this.sala_id = sala_id;
            this.usuario_id = usuario_id;
            this.posX = posX;
            this.posY = posY;
            this.colores_hex = colores_hex;
            this.colores_rgb = colores_rgb;
            this.rotation = rotation;
            this.tam = tam;
            this.espacio_ocupado = espacio_ocupado;
            this.Planta_sol = Planta_sol;
            this.Planta_agua = Planta_agua;
        }
        public void regarPlanta(SessionInstance Session)
        {
            Thread.Sleep(new TimeSpan(0, 0, 1));

            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(173);
            server.AppendParameter(this.id);
            server.AppendParameter((86400 - Time.GetDifference(this.Planta_agua)) / 12);
            server.AppendParameter(Time.GetDifference(this.Planta_agua));
            server.AppendParameter((604800 - Time.GetDifference(this.Planta_sol)) / 4);
            server.AppendParameter(Time.GetDifference(this.Planta_sol));
            server.AppendParameter(1);
            Session.User.Sala.SendData(server);
        }
        public void removeObjectBack(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(169);
            server.AppendParameter(this.id);
            server.AppendParameter(this.objeto_id);
            server.AppendParameter(1);
            Session.SendData(server);
        }
        public void putObjectSala(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(136);
            server.AppendParameter(this.id);
            server.AppendParameter(this.objeto_id);
            server.AppendParameter(Session.User.Sala.Escenario.id);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(this.posX);
            server.AppendParameter(this.posY);
            server.AppendParameter(this.rotation);
            server.AppendParameter(this.tam);
            server.AppendParameter("");
            server.AppendParameter(this.espacio_ocupado);
            server.AppendParameter(this.colores_hex);
            server.AppendParameter(this.colores_rgb);
            server.AppendParameter("0");
            server.AppendParameter("0");
            server.AppendParameter("");
            Session.User.Sala.SendData(server, Session);
        }
    }
}
