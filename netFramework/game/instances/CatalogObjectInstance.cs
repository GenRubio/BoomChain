using BoomBang.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class CatalogObjectInstance
    {
        public int id;
        public string titulo;
        public string swf_name;
        public string descripcion;
        public int tipo;
        public int price;
        public string colores_hex;
        public string colores_rgb;
        public string espacio_mapabytes;
        public int rotacion;
        public int visible;
        public int active;

        public CatalogObjectInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.titulo = (string)row["titulo"];
            this.swf_name = (string)row["swf_name"];
            try
            {
                this.descripcion = (string)row["descripcion"];
            }
            catch 
            {
                this.descripcion = "";
            }
            this.tipo = (int)row["tipo"];
            this.price = (int)row["price"];
            this.colores_hex = (string)row["colores_hex"];
            this.colores_rgb = (string)row["colores_rgb"];
            this.espacio_mapabytes = (string)row["espacio_mapabytes"];
            this.rotacion = (int)row["rotacion"];
            this.visible = (int)row["visible"];
            this.active = (int)row["active"];
        }
    }
}
