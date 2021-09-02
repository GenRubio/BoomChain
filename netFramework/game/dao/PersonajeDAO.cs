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
    class PersonajeDAO
    {
        public static List<PersonajeInstance> getUserPersonajes(int userId)
        {
            List<PersonajeInstance> personajes = new List<PersonajeInstance>();
            int setPersonajeId = 3;

            mysql client = new mysql();
            client.SetParameter("usuario_id", userId);
            foreach (DataRow row in client.ExecuteQueryTable("" +
                "SELECT * FROM personajes WHERE usuario_id = @usuario_id AND principal = 0").Rows)
            {
                personajes.Add(new PersonajeInstance(row, setPersonajeId));
                setPersonajeId += 1;
            }

            return personajes;
        }
        public static PersonajeInstance getUserPersonaje(int userId)
        {
            mysql client = new mysql();
            client.SetParameter("usuario_id", userId);
            DataRow row = client.ExecuteQueryRow("" +
                "SELECT * FROM personajes WHERE usuario_id = @usuario_id AND principal = 1 LIMIT 1");
            if (row != null)
            {
                return new PersonajeInstance(row, 2);
            }
            return null;
        }
    }
}
