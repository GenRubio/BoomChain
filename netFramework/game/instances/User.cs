using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    class User
    {
        private int id;
        private string uid;
        private string nombre;
        private string password;
        private string metamask_id;
        private bool admin;
        private int credits_gold;
        private int credits_silver;
        private string register_ip;
        private string actual_ip;
        private string last_connection;
        private string created_at;
        public Avatar Avatar;

        public User(DataRow row)
        {
            id = (int)row["id"];
            uid = (string)row["uid"];
            nombre = (string)row["nombre"];
            password = (string)row["password"];
            metamask_id = (string)row["metamask_id"];
            admin = (bool)row["admin"];
            credits_gold = (int)row["credits_gold"];
            credits_silver = (int)row["credits_silver"];
            register_ip = (string)row["register_ip"];
            actual_ip = (string)row["actual_ip"];
            last_connection = (string)row["last_connection"];
            created_at = (string)row["created_at"];
            Avatar = setUserAvatar(row);
        }
        public string getUID()
        {
            return uid;
        }
        public string getCreatedAt()
        {
            return created_at;
        }
        public string getLastConnection()
        {
            return last_connection;
        }
        public string getActualIp()
        {
            return actual_ip;
        }
        public string getRegisterIp()
        {
            return register_ip;
        }
        public int getId()
        {
            return id;
        }
        public string getNombre()
        {
            return nombre;
        }
        public string getPassword()
        {
            return password;
        }
        public string getMetamask()
        {
            return metamask_id;
        }
        public bool getAdmin()
        {
            return admin;
        }
        private Avatar setUserAvatar(DataRow row)
        {
            return new Avatar(row);
        }
        public void addCreditsGold(int value)
        {
            credits_gold = credits_gold + value;
        }
        public void removeCreditsGold(int value)
        {
            credits_gold = credits_gold - value;
        }
        public void addCreditsSilver(int value)
        {
            credits_silver = credits_silver + value;
        }
        public void removeCreditsSilver(int value)
        {
            credits_silver = credits_silver - value;
        }
    }
}
