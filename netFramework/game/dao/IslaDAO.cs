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
    class IslaDAO
    {
        public static void updateName(int id, string name)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            client.SetParameter("nombre", name);
            client.ExecuteNonQuery("UPDATE islas SET nombre = @nombre WHERE id = @id");
        }
        public static IslaInstance getIsla(string name)
        {
            mysql client = new mysql();
            client.SetParameter("nombre", name);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM islas WHERE nombre = @nombre");
            if (row != null)
            {
                return new IslaInstance(row);
            }
            return null;
        }
        public static IslaInstance getIsla(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM islas WHERE id = @id");
            if (row != null)
            {
                return new IslaInstance(row);
            }
            return null;
        }
        public static void updateDescription(int id, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            client.SetParameter("texto", value);
            client.ExecuteNonQuery("UPDATE islas SET descripcion = @texto WHERE id = @id");
        }
        public static int createIsla(int userId, string name, int model)
        {
            mysql client = new mysql();
            client.SetParameter("UserID", userId);
            client.SetParameter("Nombre", name);
            client.SetParameter("Modelo", model);
            if (client.ExecuteNonQuery("INSERT INTO islas (`Nombre`, `Modelo`, `CreadorID`) VALUES (@Nombre, @Modelo, @UserID)") == 1)
            {
                client.SetParameter("UserID", userId);
                int islaId = Convert.ToInt32(client.ExecuteScalar("SELECT MAX(id) FROM islas WHERE CreadorID = @UserID"));
                return islaId;
            }
            return 0;
        }
    }
}
