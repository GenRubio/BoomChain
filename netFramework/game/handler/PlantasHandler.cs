using BoomBang.game.dao;
using BoomBang.game.instances;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    public class PlantasHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(189165, new ProcessHandler(regarPlanta));
        }
        private static void regarPlanta(SessionInstance Session, string[,] Parameters)
        {
            if (Middleware.sala(Session))
            {
                int compra_id = int.Parse(Parameters[0, 0]);

                BuyObjectInstance Compra = getCompra(compra_id);
                if (Compra != null)
                {
                    CatalogObjectInstance Item = getItem(Compra.objeto_id);
                    if (Item != null && Item.Planta != null)
                    {
                        if (Time.GetDifference(Compra.Planta_agua) > 0)
                        {
                            Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.limit_riegue_time);
                            BuyObjectDAO.updatePlantaAgua(Compra);
                        }
                        else
                        {
                            Compra.Planta_sol = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.creation_time);
                            Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.limit_riegue_time);
                            BuyObjectDAO.updatePlantaAguaAndSol(Compra);
                        }
                    }

                    ServerMessage server = new ServerMessage();
                    server.AddHead(189);
                    server.AddHead(165);
                    server.AppendParameter(compra_id);
                    Session.User.Sala.SendData(server, Session);

                    new Thread(() => Compra.regarPlanta(Session)).Start();
                }
            }
        }
        private static CatalogObjectInstance getItem(int id)
        {
            return CatalagoObjetosDAO.getItem(id);
        }
        private static BuyObjectInstance getCompra(int id)
        {
            return BuyObjectDAO.getBuyObject(id);
        }
    }
}
