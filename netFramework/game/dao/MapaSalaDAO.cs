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
    class MapaSalaDAO
    {
        public static MapaSalaInstance getGame(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM mapas_mgame WHERE id = @id");
            if (row != null)
            {
                return new MapaSalaInstance(row);
            }
            return null;
        }
        public static MapaSalaInstance getPublico(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM mapas_publicos WHERE id = @id");
            if (row != null)
            {
                return new MapaSalaInstance(row);
            }
            return null;
        }
        public static MapaSalaInstance getPrivado(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("SELECT * FROM mapas_privados WHERE id = @id");
            if (row != null)
            {
                return new MapaSalaInstance(row);
            }
            return null;
        }
    }
}
