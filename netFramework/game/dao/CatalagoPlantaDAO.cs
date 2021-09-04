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
    class CatalagoPlantaDAO
    {
        public static CatalagoPlantaInstance getPlanta(int id)
        {
            mysql client = new mysql();
            client.SetParameter("catalago_objeto_id", id);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM catalago_plantas WHERE catalago_objeto_id = @catalago_objeto_id LIMIT 1");
            if (row != null)
            {
                return new CatalagoPlantaInstance(row);
            }
            return null;
        }
    }
}
