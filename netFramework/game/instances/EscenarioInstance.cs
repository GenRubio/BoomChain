using BoomBang.game.dao;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class EscenarioInstance
    {
        //Escenario
        public bool anti_coco = false;
        public bool anti_efecto = false;
        public int id { get; private set; }
        public string nombre { get; set; }
        public int modelo { get; set; }
        public int categoria { get; set; }
        public int es_categoria { get; set; }
        public int max_visitantes { get; set; }
        public int uppert { get; set; }
        public int coco { get; set; }
        public int visible { get; set; }
        public int sub_escenarios { get; set; }
        public int IrAlli { get; set; }

        //Privados
        public string color_1 { get; set; }
        public string color_2 { get; set; }
        public int IslaID { get; set; }
        public UserInstance Creador { get; set; }
        public string Clave = string.Empty;

        public int Ultima_Sala;
        public EscenarioInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.nombre = (string)row["nombre"];
            this.modelo = (int)row["modelo"];
            this.categoria = (int)row["categoria"];
            this.es_categoria = (int)row["es_categoria"];
            this.max_visitantes = (int)row["max_visitantes"];
            this.uppert = (int)row["uppert"];
            this.coco = (int)row["coco"];
            this.visible = (int)row["visible"];
            this.IrAlli = (int)row["IrAlli"];
            if (es_categoria == 0 || es_categoria == 9)
            {
                if (categoria == 2 || es_categoria == 9)
                {
                    this.IslaID = (int)row["IslaID"];
                    this.color_1 = (string)row["color_1"];
                    this.color_2 = (string)row["color_2"];
                    this.Creador = UserDAO.getUser((int)row["CreadorID"]);
                    this.Clave = row["clave"].ToString();
                    this.Ultima_Sala = (int)row["Ultima_Sala"];
                }
            }
        }
    }
}
