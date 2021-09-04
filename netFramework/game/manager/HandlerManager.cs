using BoomBang.game.handler;
using BoomBang.game.instances;
using System;
using System.Collections.Generic;
using BoomBang.Forms;

namespace BoomBang.game.manager
{
    public delegate void ProcessHandler(SessionInstance Session, string[,] Parameters);
    public class HandlerManager
    {
        public static Dictionary<int, ProcessHandler> Handlers = new Dictionary<int, ProcessHandler>();
        public static int Contrador_Goldens = 0;
        public static void Initialize()
        {
            try
            {
                LoginHandler.Start();
                FlowerHandler.Start();
                NavigatorHandler.Start();
                CatalogoHandler.Start();
                PlantasHandler.Start();
                PathfindingHandler.Start();
                InterfazHandler.Start();
                IslasHandler.Start();
                MiniGamesHandler.Start();
                PingHandler.Start();
              
                string console = "Se han registrado " + Handlers.Count + " handlers.";
                Emulator.Form.WriteLine(console);
                listas.automatic_lists_row();
            }
            catch(Exception e)
            {
                Emulator.Form.WriteLine(e.Message, "error");
            }
        }
        public static void RegisterHandler(int Header, ProcessHandler Handler)
        {
            if (!Handlers.ContainsKey(Header))
            {
                Handlers.Add(Header, Handler);
            }
        }
    }
}
