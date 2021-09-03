using BoomBang.Forms;
using BoomBang.game.instances;
using BoomBang.game.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.SocketsWeb
{
    class CallPackage
    {
        public static void addItemUserBack(SessionInstance Session,
            CatalogObjectInstance Item,
            BuyObjectInstance Compra,
            int amount)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(139);
            server.AppendParameter(Compra.id);
            server.AppendParameter(Item.id);
            server.AppendParameter(Item.colores_hex);
            server.AppendParameter(Item.colores_rgb);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(Item.tam_n);
            server.AppendParameter(Item.espacio_ocupado_n);
            server.AppendParameter(0);
            server.AppendParameter(0);
            server.AppendParameter(amount);
            Session.SendData(server);
        }
    }
}
