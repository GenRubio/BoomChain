using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class PersonajeInstance
    {
        public int avatar { get; set; }
        public string color_1 { get; set; }
        public string color_2 { get; set; }
        public string color_3 { get; set; }
        public string color_4 { get; set; }
        public string color_5 { get; set; }
        public string color_6 { get; set; }
        public string color_7 { get; set; }
        public int principal { get; set; }

        public PersonajeInstance(DataRow row)
        {
            this.avatar = (int)row["avatar_id"];
            this.color_1 = (string)row["color_1"];
            this.color_2 = (string)row["color_2"];
            this.color_3 = (string)row["color_3"];
            this.color_4 = (string)row["color_4"];
            this.color_5 = (string)row["color_5"];
            this.color_6 = (string)row["color_6"];
            this.principal = (int)row["principal"];
        }

        public string colores
        {
            get
            {
                return this.color_1 +
                    this.color_2 +
                    this.color_3 +
                    this.color_4 +
                    this.color_5 +
                    this.color_6 +
                    this.color_7;
            }
        }
    }
}
