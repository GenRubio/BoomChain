using BoomBang.game.instances;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoomBang.Forms;

namespace BoomBang.game.handler
{
    class LoginHandler
    {
        
        public static void Start()
        {
            HandlerManager.RegisterHandler(120130, iniciar_sesion);
        }
        private static void iniciar_sesion(SessionInstance Session, string[,] Parameters)
        {
            string name = Parameters[0, 0];
            string password = Parameters[1, 0];
            if (name != "" && password != "")
            {
                UserManager.IniciarSesion(Session, name, password);
            }
            else
            {
                string console = "Error al iniciar cliente " + Session.IP;
                Emulator.Form.WriteLine(console);
            }
        }
    }
}
