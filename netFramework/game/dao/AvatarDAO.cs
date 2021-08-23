using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.dao
{
    class AvatarDAO
    {
        public static void updateVotosSimpatico(int userId, int value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("votos_simpatico", value);
            client.ExecuteNonQuery("UPDATE usuarios SET votos_simpatico = @votos_simpatico WHERE id = @id");
        }
        public static void updateVotosLegal(int userId, int value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("votos_legal", value);
            client.ExecuteNonQuery("UPDATE usuarios SET votos_legal = @votos_legal WHERE id = @id");
        }
        public static void updateVotosSexy(int userId, int value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("votos_sexy", value);
            client.ExecuteNonQuery("UPDATE usuarios SET votos_sexy = @votos_sexy WHERE id = @id");
        }
        public static void updateDeseo3(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("deseo_3", value);
            client.ExecuteNonQuery("UPDATE usuarios SET deseo_3 = @deseo_3 WHERE id = @id");
        }
        public static void updateDeseo2(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("deseo_1", value);
            client.ExecuteNonQuery("UPDATE usuarios SET deseo_2 = @deseo_2 WHERE id = @id");
        }
        public static void updateDeseo1(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("deseo_1", value);
            client.ExecuteNonQuery("UPDATE usuarios SET deseo_1 = @deseo_1 WHERE id = @id");
        }
        public static void updateHobby3(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("hobby_3", value);
            client.ExecuteNonQuery("UPDATE usuarios SET hobby_3 = @hobby_3 WHERE id = @id");
        }
        public static void updateHobby2(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("hobby_2", value);
            client.ExecuteNonQuery("UPDATE usuarios SET hobby_2 = @hobby_2 WHERE id = @id");
        }
        public static void updateHobby1(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("hobby_1", value);
            client.ExecuteNonQuery("UPDATE usuarios SET hobby_1 = @hobby_1 WHERE id = @id");
        }
        public static void updateDescription(int userId, string value)
        {
            mysql client = new mysql();
            client.SetParameter("id", userId);
            client.SetParameter("description", value);
            client.ExecuteNonQuery("UPDATE usuarios SET bocadillo = @description WHERE id = @id");
        }
    }
}
