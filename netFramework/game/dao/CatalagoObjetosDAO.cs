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
    class CatalagoObjetosDAO
    {
        public static List<CatalogObjectInstance> getCatalagoObjetosList()
        {
            List<CatalogObjectInstance> objetos = new List<CatalogObjectInstance>();
            using (mysql client = new mysql())
            {
                foreach (DataRow row in client.ExecuteQueryTable("SELECT * FROM catalago_objetos WHERE active = 1").Rows)
                {
                    objetos.Add(new CatalogObjectInstance(row));
                }
            }
            return objetos;
        }
        public static CatalogObjectInstance getItem(int id)
        {
            mysql client = new mysql();
            client.SetParameter("id", id);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM catalago_objetos WHERE id = @id AND active = 1 AND visible = 1");
            if (row != null)
            {
                return new CatalogObjectInstance(row);
            }
            return null;
        }
    }
}
