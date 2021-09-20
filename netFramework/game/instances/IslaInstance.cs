using BoomBang.game.dao;
using BoomBang.game.manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    public class IslaInstance
    {
        public int id { get; set; }
        public int modelo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public UserInstance Creador { get; set; }
       
        public IslaInstance(DataRow row)
        {
            this.id = (int)row["id"];
            this.modelo = (int)row["modelo"];
            this.nombre = (string)row["nombre"];
            this.descripcion = (string)row["descripcion"];
            this.Creador = UserDAO.getUser((int)row["CreadorID"]);
        }

        public void changeName(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(129);
            server.AppendParameter(1);
            Session.SendData(server);
        }
    }
}
