using BoomBang.game.handler;
using BoomBang.game.instances;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoomBang.Forms;
using BoomBang.game.dao;

namespace BoomBang.game.manager
{
    class UserManager
    {

        public static Dictionary<int, SessionInstance> UsuariosOnline = new Dictionary<int, SessionInstance>();
        public static bool usuarioOnline (UserInstance User)
        {
            foreach(SessionInstance sessionOnline in UsuariosOnline.Values.ToList())
            {
                if (sessionOnline.User.id == User.id)
                {
                    return true;
                }
            }
            return false;
        }
        
        public static void Sumar_Cocos(UserInstance User, int Puntos)
        {
            User.puntos_cocos++;
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("puntos_cocos", User.puntos_cocos);
                client.ExecuteNonQuery("UPDATE usuarios SET puntos_cocos = @puntos_cocos WHERE id = @id");
            }
        }
        public static void Sumar_Shurikens(UserInstance User, int Puntos)
        {
            User.puntos_ninja++;
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("puntos_ninja", User.puntos_ninja);
                client.ExecuteNonQuery("UPDATE usuarios SET puntos_ninja = @puntos_ninja WHERE id = @id");
            }
        }
       
        public static void ActualizarEstadisticas(UserInstance User)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", User.id);
                client.SetParameter("besos_enviados", User.besos_enviados);
                client.SetParameter("besos_recibidos", User.besos_recibidos);
                client.SetParameter("jugos_enviados", User.jugos_enviados);
                client.SetParameter("jugos_recibidos", User.jugos_recibidos);
                client.SetParameter("flores_enviadas", User.flores_enviadas);
                client.SetParameter("flores_recibidas", User.flores_recibidas);
                client.SetParameter("uppers_enviados", User.uppers_enviados);
                client.SetParameter("uppers_recibidos", User.uppers_recibidos);
                client.SetParameter("cocos_enviados", User.cocos_enviados);
                client.SetParameter("cocos_recibidos", User.cocos_recibidos);
                client.SetParameter("rings_ganados", User.rings_ganados);
                client.SetParameter("senderos_ganados", User.senderos_ganados);
                client.SetParameter("puntos_cocos", User.puntos_cocos);
                client.SetParameter("puntos_ninja", User.puntos_ninja);
                client.SetParameter("email", User.email);
                client.ExecuteNonQuery("UPDATE usuarios SET senderos_ganados = @senderos_ganados, rings_ganados = @rings_ganados, besos_enviados = @besos_enviados, besos_recibidos = @besos_recibidos, jugos_enviados = @jugos_enviados, jugos_recibidos = @jugos_recibidos, flores_enviadas = @flores_enviadas, flores_recibidas = @flores_recibidas, uppers_enviados = @uppers_enviados, uppers_recibidos = @uppers_recibidos, cocos_enviados = @cocos_enviados, cocos_recibidos = @cocos_recibidos, puntos_cocos = @puntos_cocos, puntos_ninja = @puntos_ninja, email = @email WHERE id = @id");
            }
        }
    }
}

