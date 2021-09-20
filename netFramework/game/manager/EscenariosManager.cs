using BoomBang.game.dao;
using BoomBang.game.instances;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.manager
{
    public class EscenariosManager
    {
        public static bool ControlDeSeguridad(UserInstance User, EscenarioInstance Escenario)
        {
            if (Escenario.Creador.id == User.id)
            {
                return true;
            }
            return false;
        }
        public static EscenarioInstance ObtenerEscenario(int es_categoria, int id)
        {
            if (es_categoria == 1) // publico
            {
                return EscenarioDAO.getEscenarioPublic(id);
            }
            if (es_categoria == 0) // Privado
            {
                return EscenarioDAO.getEscenarioPrivate(id);
            }
            return null;
        }
    }
}
