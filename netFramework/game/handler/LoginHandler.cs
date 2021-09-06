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
            string metamask = Parameters[0, 0];
            string password = Parameters[1, 0];

            UserInstance user = UserDAO.getUser(metamask, password);
            if (user != null && user.Personaje != null)
            {
                if (UserManager.usuarioOnline(user) == false)
                {
                    Session.User = user;

                    Session.User.loginGame(Session);

                    new Thread(() => UserDAO.updateConnection(Session.User, Session.IP));
                    UserManager.UsuariosOnline.Add(Session.User.id, Session);

                    sendFormMessage("[UserManager] -> Se ha conectado el usuario " + Session.User.nombre + ".");
                    Emulator.Form.UpdateTitle();
                }
            }
            else
            {
                sendFormMessage("Error al iniciar cliente " + Session.IP);
            }
        }
        private static void sendFormMessage(string message)
        {
            string console = message;
            Emulator.Form.WriteLine(console);
        }
    }
}
