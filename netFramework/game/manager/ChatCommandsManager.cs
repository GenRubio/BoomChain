using BoomBang.game.instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.manager
{
    class ChatCommandsManager
    {
        public static void checkCommand(SessionInstance Session, string mensaje)
        {
            if (Session.User.admin == 1)
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
            }  
        }
    }
}
