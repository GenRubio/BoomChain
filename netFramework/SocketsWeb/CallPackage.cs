using BoomBang.Forms;
using BoomBang.game.instances;
using BoomBang.game.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.SocketsWeb
{
    class CallPackage
    {
        public static void changeFichaUser(SessionInstance session, string[] parameters)
        {
            try
            {
                session.User.edad = int.Parse(parameters[1]);
                SalasManager.Salir_Sala(session, true);

            }
            catch
            {
                Emulator.Form.WriteLine("Error change ficha");
            }
           
        }
        public static void alert(SessionInstance session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(183);
            server.AppendParameter("Hola");
            session.SendData(server);
        }

        public static void changeNinja(SessionInstance session, string[] parameters)
        {
            if (session.User.PreLock_Interactuando == true) return;
            if (session.User.PreLock_Disfraz == true) { return; }
            if (session.User.ModoNinja == true) return;

            session.User.NinjaColores_Sala = ninjaColors(session, int.Parse(parameters[1]));
            session.User.ModoNinja = true;
            session.User.Trayectoria.DetenerMovimiento();
            session.User.PreLock_Disfraz = true;

            ServerMessage server = new ServerMessage();
            server.AddHead(125);
            server.AddHead(120);
            server.AppendParameter(session.User.id);
            server.AppendParameter(12);
            server.AppendParameter(session.User.NinjaColores_Sala);
            server.AppendParameter(1);
            server.AppendParameter(1);
            server.AppendParameter(1);
            session.User.Sala.SendData(server);
        }

        private static string ninjaColors(SessionInstance session, int ninja)
        {
            string EXTRA_COLORS = "AAAAAAAAAAAAAAAAAA";
            return "000000" + session.User.colores.Substring(0, 6) + ObtenerCinta(ninja) + session.User.colores.Substring(18, 6) + EXTRA_COLORS;
        }

        private static string ObtenerCinta(int NinjaLevel)
        {
            switch (NinjaLevel)
            {
                case 0:
                    return "C9CACF";
                case 1:
                    return "FF0000";
                case 2:
                    return "FF3399";
                case 3:
                    return "FF6600";
                case 4:
                    return "00CC00";
                case 5:
                    return "0066CC";
                case 6:
                    return "FFFFFF";
                case 7:
                    return "660099";
                case 8:
                    return "653232";
                case 9:
                    return "222222";
                case 10:
                    return "f2b100";
                default:
                    return "FFFFFF";
            }
        }
    }
}
