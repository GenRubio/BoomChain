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
using BoomBang.game.dao;

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

            Session.User = UserDAO.getUser(name, password);
            if (Session.User != null)
            {
                if (!UserManager.usuarioOnline(Session.User))
                {
                    IniciarSesion(Session);
                    UserManager.UsuariosOnline.Add(Session.User.id, Session);
                }
            }
            else
            {
                string console = "Error al iniciar cliente " + Session.IP;
                Emulator.Form.WriteLine(console);
            }
        }
        private static void IniciarSesion(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(130);
            server.AppendParameter(1);
            server.AppendParameter(Session.User.nombre);
            server.AppendParameter(Session.User.avatar);
            server.AppendParameter(Session.User.colores);
            server.AppendParameter(Session.User.email);
            server.AppendParameter(Session.User.edad);
            server.AppendParameter(2);
            server.AppendParameter("Hola, Soy nuevo en BoomBang.");
            server.AppendParameter(0);
            server.AppendParameter(Session.User.id);
            server.AppendParameter(Session.User.admin);
            server.AppendParameter(Session.User.oro);
            server.AppendParameter(-1);
            server.AppendParameter(200);
            server.AppendParameter(5);
            server.AppendParameter(0);
            server.AppendParameter(-1);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(-1);//tutorial islas
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

            new Thread(() => UserDAO.updateConnection(Session.User, Session.IP));

            string console = "[UserManager] -> Se ha conectado el usuario " + Session.User.nombre + ".";
            Emulator.Form.WriteLine(console);
            Emulator.Form.UpdateTitle();
        }
    }
}
