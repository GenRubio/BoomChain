using BoomBang.game.instances;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.dao
{
    class BuyObjectDAO
    {
        public static bool moveObjectSala(BuyObjectInstance Compra)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            client.SetParameter("posX", Compra.posX);
            client.SetParameter("posY", Compra.posY);
            client.SetParameter("espacio_ocupado", Compra.espacio_ocupado);
            if (client.ExecuteNonQuery("" +
                "UPDATE objetos_comprados SET posX = @posX, posY = @posY, espacio_ocupado = @espacio_ocupado WHERE id = @id") == 1)
            {
                return true;
            }
            return false;
        }
        public static void updateColors(BuyObjectInstance Compra)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            client.SetParameter("colores_hex", Compra.colores_hex);
            client.SetParameter("colores_rgb", Compra.colores_rgb);
            client.ExecuteNonQuery("" +
                "UPDATE objetos_comprados SET colores_hex = @colores_hex, colores_rgb = @colores_rgb WHERE id = @id");
        }
        public static void updatePlantaAgua(BuyObjectInstance Compra)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            client.SetParameter("planta_agua", Compra.Planta_agua);
            client.ExecuteNonQuery("UPDATE objetos_comprados SET planta_agua = @planta_agua WHERE id = @id");
        }
        public static void updatePlantaAguaAndSol(BuyObjectInstance Compra)
        {
            mysql client = new mysql();
            client.SetParameter("id", Compra.id);
            client.SetParameter("planta_agua", Compra.Planta_agua);
            client.SetParameter("planta_sol", Compra.Planta_sol);
            client.ExecuteNonQuery("" +
                "UPDATE objetos_comprados SET planta_agua = @planta_agua, planta_sol = @planta_sol WHERE id = @id");
        }
        public static void addBuyObject(UserInstance User, CatalogObjectInstance Item)
        {
            mysql client = new mysql();
            client.SetParameter("objeto_id", Item.id);
            client.SetParameter("usuario_id", User.id);
            client.SetParameter("colores_hex", Item.colores_hex);
            client.SetParameter("colores_rgb", Item.colores_rgb);
            client.ExecuteNonQuery("" +
                "INSERT INTO objetos_comprados (objeto_id, usuario_id, colores_hex, colores_rgb) " +
                "VALUES (@objeto_id, @usuario_id, @colores_hex, @colores_rgb)");
        }
        public static BuyObjectInstance addAndGetBuyObject(UserInstance User, CatalogObjectInstance Item)
        {
            mysql client = new mysql();
            client.SetParameter("objeto_id", Item.id);
            client.SetParameter("usuario_id", User.id);
            client.SetParameter("colores_hex", Item.colores_hex);
            client.SetParameter("colores_rgb", Item.colores_rgb);
            if (client.ExecuteNonQuery("" +
                "INSERT INTO objetos_comprados (objeto_id, usuario_id, colores_hex, colores_rgb) " +
                "VALUES (@objeto_id, @usuario_id, @colores_hex, @colores_rgb)") == 1)
            {
                client.SetParameter("objeto_id", Item.id);
                client.SetParameter("usuario_id", User.id);
                int buyObjectId = int.Parse(Convert.ToString(client.ExecuteScalar("" +
                    "SELECT MAX(id) FROM objetos_comprados WHERE objeto_id = @objeto_id AND usuario_id = @usuario_id")));

                client.SetParameter("id", buyObjectId);
                DataRow row = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE id = @id");
                if (row != null)
                {
                    return new BuyObjectInstance(row);
                }
            }

            return null;
        }
        public static BuyObjectInstance getBuyObject(int id)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", id);
                DataRow row = client.ExecuteQueryRow("SELECT * FROM objetos_comprados WHERE id = @id");
                if (row != null)
                {
                    return new BuyObjectInstance(row);
                }
            }
            return null;
        }
        public static bool removeObjectSala(BuyObjectInstance Compra)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", Compra.id);
                client.SetParameter("sala_id", 0);
                if (client.ExecuteNonQuery("UPDATE objetos_comprados SET sala_id = @sala_id WHERE id = @id") == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool putObjectInArea(SalaInstance Sala, BuyObjectInstance Compra, BuyObjectInstance newObject)
        {
            using (mysql client = new mysql())
            {
                client.SetParameter("id", Compra.id);
                client.SetParameter("posX", newObject.posX);
                client.SetParameter("posY", newObject.posY);
                client.SetParameter("tam", newObject.tam);
                client.SetParameter("rotation", newObject.rotation);
                client.SetParameter("sala_id", Sala.Escenario.id);
                client.SetParameter("espacio_ocupado", newObject.espacio_ocupado);
                if (client.ExecuteNonQuery("" +
                    "UPDATE objetos_comprados " +
                    "SET posX = @posX, " +
                    "posY = @posY, " +
                    "tam = @tam, " +
                    "rotation = @rotation, " +
                    "espacio_ocupado = @espacio_ocupado, " +
                    "sala_id = @sala_id " +
                    "WHERE id = @id") == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
