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
    class EscenarioDAO
    {
        public static void deleteEscenarioPrivado(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            client.ExecuteNonQuery("DELETE FROM escenarios_privados WHERE id = @id");
        }
        public static List<EscenarioInstance> getEscenariosAreas()
        {
            List<EscenarioInstance> escenarios = new List<EscenarioInstance>();

            mysql client = new mysql();
            client.SetParameter("visible", 1);
            foreach (DataRow row in client.ExecuteQueryTable("" +
                "SELECT * FROM escenarios_publicos WHERE visible = @visible ORDER BY Prioridad ASC").Rows)
            {
                escenarios.Add(new EscenarioInstance(row));
            }

            return escenarios;
        }
        public static Dictionary<int, BuyObjectInstance> getEscenarioObjects(int id)
        {
            Dictionary<int, BuyObjectInstance> objects = new Dictionary<int, BuyObjectInstance>();

            mysql client = new mysql();
            client.SetParameter("sala_id", id);
            foreach (DataRow row in client.ExecuteQueryTable("SELECT * FROM objetos_comprados WHERE sala_id = @sala_id").Rows)
            {
                BuyObjectInstance Item = new BuyObjectInstance(row);
                objects.Add(Item.id, Item);
            }
            return objects;
        }
        public static EscenarioInstance getEscenarioPrivate(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM escenarios_privados WHERE id = @id");
            if (row != null)
            {
                return new EscenarioInstance(row);
            }
            return null;
        }
        public static EscenarioInstance getEscenarioPublic(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM escenarios_publicos WHERE id = @id");
            if (row != null)
            {
                return new EscenarioInstance(row);
            }
            return null;
        }
        public static void updateColors(int id, string color_1, string color_2)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            client.SetParameter("color_1", color_1);
            client.SetParameter("color_2", color_2);
            client.ExecuteNonQuery("UPDATE escenarios_privados SET color_1 = @color_1, color_2 = @color_2 WHERE id = @id");
        }
        public static void updateName(int id, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            client.SetParameter("nombre", value);
            client.ExecuteNonQuery("UPDATE escenarios_privados SET nombre = @nombre WHERE id = @id");
        }
    }
}
