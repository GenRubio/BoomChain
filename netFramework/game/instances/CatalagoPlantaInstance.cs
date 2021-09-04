using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class CatalagoPlantaInstance
    {
        public int creation_time;
        public int limit_riegue_time;
        public CatalagoPlantaInstance(DataRow row)
        {
            this.creation_time = (int)row["creation_time"];
            this.limit_riegue_time = (int)row["limit_riegue_time"];
        }
    }
}
