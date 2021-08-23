using BoomBang.Forms;
using BoomBang.game.instances;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.dao
{
    class UserDAO
    {
        public static UserInstance getUser(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM usuarios WHERE id = @id");
            if (row != null)
            {
                return new UserInstance(row);
            }
            return null;
        }
        public static UserInstance getUser(string name, string password)
        {
            mysql client = new mysql();
            client.SetParameter("nombre", name);
            client.SetParameter("token_uid", password);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM usuarios WHERE nombre = @nombre AND token_uid = @token_uid LIMIT 1");
            if (row != null)
            {
                return new UserInstance(row);
            }
            return null;
        }
        public static UserInstance getUser(string name)
        {
            mysql client = new mysql();
            client.SetParameter("nombre", name);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM usuarios WHERE nombre = @nombre");
            if (row != null)
            {
                return new UserInstance(row);
            }
            return null;
        }
        public static void updateConnection(UserInstance User, string ip)
        {
            mysql client = new mysql();
            client.SetParameter("id", User.id);
            client.SetParameter("ip", ip);
            client.SetParameter("time", DateTime.Now);
            client.ExecuteNonQuery("UPDATE usuarios SET ultima_conexion = @time, ip_actual = @ip WHERE id = @id");
        }
    }
}
