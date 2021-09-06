using BoomBang.game.instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game
{
    class Middleware
    {
        /*public static bool objectSalaInteraction()
        {

        }*/
        public static bool auth(SessionInstance Session)
        {
            if (Session.User != null && Session.User.Personaje != null)
            {
                return true;
            }
            else
            {
                Session.FinalizarConexion("Middleware auth");
                return false;
            }
        }
        public static bool sala(SessionInstance Session)
        {
            if (Session.User != null && Session.User.Personaje != null && Session.User.Sala != null)
            {
                return true;
            }
            else
            {
                Session.FinalizarConexion("Middleware sala");
                return false;
            }
        }
        public static bool salaNone(SessionInstance Session)
        {
            if (Session.User != null && Session.User.Personaje != null && Session.User.Sala == null)
            {
                return true;
            }
            else
            {
                Session.FinalizarConexion("Middleware salaNone");
                return false;
            }
        }
    }
}
