using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class MapaSalaInstance
    {
        public int id { get; set; }
        public string mapa { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public MapaSalaInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.mapa = (string)row["mapa"];
            this.posX = (int)row["posX"];
            this.posY = (int)row["posY"];
        }
    }
}
