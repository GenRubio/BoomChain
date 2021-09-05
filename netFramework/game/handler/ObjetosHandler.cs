using BoomBang.game.dao;
using BoomBang.game.instances;
using BoomBang.game.manager;
using BoomBang.SocketsWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.handler
{
    class ObjetosHandler
    {
        public static void Start()
        {
            HandlerManager.RegisterHandler(189134, new ProcessHandler(buyObject));
            HandlerManager.RegisterHandler(189136, new ProcessHandler(putObjectSala));
            HandlerManager.RegisterHandler(189140, new ProcessHandler(removeObjectSala));
            HandlerManager.RegisterHandler(189142, new ProcessHandler(updateColorsSala));
            HandlerManager.RegisterHandler(189145, new ProcessHandler(moveObjectSala));
        }
        private static void moveObjectSala(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            int Objeto_ID = int.Parse(Parameters[1, 0]);
            int x = int.Parse(Parameters[2, 0]);
            int y = int.Parse(Parameters[3, 0]);
            string ocupe = Parameters[4, 0];

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Compra.objeto_id != Objeto_ID) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id)
                {
                    Session.FinalizarConexion("MoverObjeto");
                    return;
                }

                Compra.posX = x;
                Compra.posY = y;
                Compra.espacio_ocupado = ocupe;

                if (BuyObjectDAO.moveObjectSala(Compra))
                {
                    Session.User.Sala.EliminarChutas(Session.User.Sala.ObjetosEnSala[Compra.id]);
                    Session.User.Sala.ObjetosEnSala[Compra.id].posX = x;
                    Session.User.Sala.ObjetosEnSala[Compra.id].posY = y;
                    Session.User.Sala.ObjetosEnSala[Compra.id].espacio_ocupado = ocupe;
                    Session.User.Sala.FijarChutas(Session.User.Sala.ObjetosEnSala[Compra.id]);

                    Compra.moveObjectSala(Session);
                }
            }
        }
        private static void updateColorsSala(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);
            string hex = Parameters[1, 0];
            string rgb = Parameters[2, 0];

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (!Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id)
                {
                    Session.FinalizarConexion("CambiarColores");
                    return;
                }

                Session.User.Sala.ObjetosEnSala[Compra.id].colores_hex = hex;
                Session.User.Sala.ObjetosEnSala[Compra.id].colores_rgb = rgb;
                Compra.colores_hex = hex;
                Compra.colores_rgb = rgb;

                Compra.updateColors(Session, Parameters);

                new Thread(() => BuyObjectDAO.updateColors(Compra)).Start();
            }
        }
        private static void removeObjectSala(SessionInstance Session, string[,] Parameters)
        {
            int Compra_ID = int.Parse(Parameters[0, 0]);

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(Compra_ID);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Session.User.Sala.Escenario.Creador.id != Session.User.id)
                {
                    Session.FinalizarConexion("Quitar_Objeto");
                    return;
                }
                if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id))
                {
                    if (BuyObjectDAO.removeObjectSala(Compra))
                    {
                        Session.User.Sala.EliminarChutas(Compra);
                        Session.User.Sala.ObjetosEnSala.Remove(Compra.id);

                        CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(Compra.objeto_id);
                        if (Item != null)
                        {
                            Compra.addObjectBack(Session);
                            Compra.removeObjectSala(Session);

                            if (Item.Planta != null)
                            {
                                Compra.Planta_agua = 0;
                                Compra.Planta_sol = 0;
                                BuyObjectDAO.updatePlantaAguaAndSol(Compra);
                            }
                        }
                    }
                }
            }
        }
        private static void putObjectSala(SessionInstance Session, string[,] Parameters)
        {
            int compra_id = int.Parse(Parameters[0, 0]);
            int Zona_ID = int.Parse(Parameters[1, 0]);
            int Objeto_id = int.Parse(Parameters[2, 0]);
            int x = int.Parse(Parameters[3, 0]);
            int y = int.Parse(Parameters[4, 0]);
            string tam = Parameters[5, 0];
            int rotation = int.Parse(Parameters[6, 0]);
            string Espacio_Ocupado = Parameters[7, 0];
            string Colores_Hex = Parameters[8, 0];
            string Colores_Rgb = Parameters[9, 0];

            if (Session.User.Sala.Escenario.Creador.id != Session.User.id)
            {
                Session.FinalizarConexion("PonerObjeto");
                return;
            }

            BuyObjectInstance Compra = BuyObjectDAO.getBuyObject(compra_id);
            if (Compra != null)
            {
                if (Compra.usuario_id != Session.User.id) return;
                if (Compra.sala_id != 0) return;
                if (Compra.id != compra_id) return;
                if (Compra.objeto_id != Objeto_id) return;
                if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id)) return;

                BuyObjectInstance newObject = new BuyObjectInstance(compra_id,
                    Objeto_id,
                    Zona_ID,
                    Session.User.id,
                    x,
                    y,
                    Colores_Hex,
                    Colores_Rgb,
                    rotation,
                    tam,
                    Espacio_Ocupado,
                    0,
                    0);

                if (BuyObjectDAO.putObjectInArea(Session.User.Sala, Compra, newObject))
                {
                    Compra.posX = newObject.posX;
                    Compra.posY = newObject.posY;
                    Compra.sala_id = Session.User.Sala.id;
                    Compra.espacio_ocupado = newObject.espacio_ocupado;
                    Compra.tam = newObject.tam;
                    Compra.putObjectSala(Session);
                    Compra.removeObjectBack(Session);

                    CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(Compra.objeto_id);
                    if (Item != null && Item.Planta != null)
                    {
                        Compra.Planta_agua = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.limit_riegue_time);
                        Compra.Planta_sol = Time.GetCurrentAndAdd(AddType.Horas, Item.Planta.creation_time);
                        new Thread(() => Compra.regarPlanta(Session)).Start();

                        BuyObjectDAO.updatePlantaAguaAndSol(Compra);
                    }

                    Session.User.Sala.ObjetosEnSala.Add(Compra.id, Compra);
                    if (Session.User.Sala.ObjetosEnSala.ContainsKey(Compra.id))
                    {
                        Session.User.Sala.FijarChutas(Compra);
                        /*
                        0,0 > Posicion central
                        0,1 > Trajectoria derecha - abajo
                        1,0 > Trajectoria derecha - arriba
                        -1,0 > Trajectoria esquerda - abajo
                        0, -1 > Trajectoria esquerda - arriba
                        1,1 > Derecha
                        */
                    }
                }
            }
        }
        private static void buyObject(SessionInstance Session, string[,] Parameters)
        {
            if (Session.User != null && Session.User.Sala != null)
            {
                int objeto_id = int.Parse(Parameters[0, 0]);

                CatalogObjectInstance Item = CatalagoObjetosDAO.getItem(objeto_id);
                if (Item != null)
                {
                    BuyObjectInstance buyObject = BuyObjectDAO.addAndGetBuyObject(Session.User, Item);
                    if (buyObject != null)
                    {
                        CallPackage.addItemUserBack(Session, Item, buyObject, 1);
                    }
                }
            }
        }
    }
}
