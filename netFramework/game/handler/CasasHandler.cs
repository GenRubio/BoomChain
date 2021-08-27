using BoomBang.game.instances;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    class CasasHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(120143, new ProcessHandler(MyHouseID));
            HandlerManager.RegisterHandler(189174, new ProcessHandler(Actualizar_Terreno));
        }
        private static void Actualizar_Terreno(SessionInstance Session, string[,] Parameters)
        {
            Packet_189_174(Session, Parameters);
        }
        private static void MyHouseID(SessionInstance Session, string[,] Parameters)
        {
            if (CasasManager.GetZoneID(Session.User.id, 1) == 0)
            {
                CasasManager.RegistrarCasa(Session.User);
            }
            Packet_120_123(Session);
        }
        private static void Packet_189_174(SessionInstance Session, string[,] Parameters)
        {
            mysql client = new mysql();
            ServerMessage server = new ServerMessage();
            server.AddHead(189);
            server.AddHead(174);
            server.AppendParameter(int.Parse(Parameters[0, 0]));
            server.AppendParameter(int.Parse(Parameters[1, 0]));
            server.AppendParameter(Parameters[2, 0]);
            server.AppendParameter(Parameters[3, 0]);
            server.AppendParameter(Parameters[4, 0]);
            server.AppendParameter(Parameters[5, 0]);
            if (Session.User.Sala.Escenario.Creador.id == Session.User.id)
            {
                if (int.Parse(Parameters[0, 0]) == 0)
                {
                    client.SetParameter("id", Session.User.Sala.Escenario.id);
                    client.SetParameter("terreno_something_1", int.Parse(Parameters[0, 0]));
                    client.SetParameter("terreno_something_2", int.Parse(Parameters[1, 0]));
                    client.SetParameter("terreno_something_3", Parameters[2, 0]);
                    client.SetParameter("terreno_config", Parameters[3, 0]);
                    client.SetParameter("terreno_colores", Parameters[4, 0]);
                    client.SetParameter("terreno_rgb", Parameters[5, 0]);
                    if (client.ExecuteNonQuery("UPDATE escenarios_privados SET terreno_something_1 = @terreno_something_1, terreno_something_2 = @terreno_something_2, terreno_something_3 = @terreno_something_3, terreno_config = @terreno_config, terreno_colores = @terreno_colores, terreno_rgb = @terreno_rgb WHERE id = @id") == 1)
                    {
                        Session.User.Sala.Escenario.terreno_something_1 = int.Parse(Parameters[0, 0]);
                        Session.User.Sala.Escenario.terreno_something_2 = int.Parse(Parameters[1, 0]);
                        Session.User.Sala.Escenario.terreno_something_3 = Parameters[2, 0];
                        Session.User.Sala.Escenario.terreno_config = Parameters[3, 0];
                        Session.User.Sala.Escenario.terreno_colores = Parameters[4, 0];
                        Session.User.Sala.Escenario.terreno_rgb = Parameters[5, 0];
                        Session.User.Sala.SendData(server);
                    }
                }
                if (int.Parse(Parameters[0, 0]) == 1)
                {
                    client.SetParameter("id", Session.User.Sala.Escenario.id);
                    client.SetParameter("object_something_1", int.Parse(Parameters[0, 0]));
                    client.SetParameter("object_something_2", int.Parse(Parameters[1, 0]));
                    client.SetParameter("object_something_3", Parameters[2, 0]);
                    client.SetParameter("object_config", Parameters[3, 0]);
                    client.SetParameter("object_colores", Parameters[4, 0]);
                    client.SetParameter("object_rgb", Parameters[5, 0]);
                    if (client.ExecuteNonQuery("UPDATE escenarios_privados SET object_something_1 = @object_something_1, object_something_2 = @object_something_2, object_something_3 = @object_something_3, object_config = @object_config, object_colores = @object_colores, object_rgb = @object_rgb WHERE id = @id") == 1)
                    {
                        Session.User.Sala.Escenario.object_something_1 = int.Parse(Parameters[0, 0]);
                        Session.User.Sala.Escenario.object_something_2 = int.Parse(Parameters[1, 0]);
                        Session.User.Sala.Escenario.object_something_3 = Parameters[2, 0];
                        Session.User.Sala.Escenario.object_config = Parameters[3, 0];
                        Session.User.Sala.Escenario.object_colores = Parameters[4, 0];
                        Session.User.Sala.Escenario.object_rgb = Parameters[5, 0];
                        Session.User.Sala.SendData(server);
                    }
                }
            }
        }
        private static void Packet_120_123(SessionInstance Session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(120);
            server.AddHead(143);
            server.AppendParameter(0);
            server.AppendParameter(-1);
            server.AppendParameter(CasasManager.GetZoneID(Session.User.id, 1));
            server.AppendParameter(25);
            Session.SendData(server);
        }
    }
}
