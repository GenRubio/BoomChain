using BoomBang.game.instances.manager.pathfinding;
using BoomBang.game.manager;
using BoomBang.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoomBang.Forms;

namespace BoomBang.game.instances
{
    public class SessionInstance
    {
        public bool verificandoCuenta = true;
        public bool ping;
        public double LastPingTime = 0;
        public Socket Client { get; set; }
        private byte[] buffer = new byte[2048];
        public string IP { get { return Client.RemoteEndPoint.ToString().Split(':')[0]; } }
        private bool Policy = false;
        public UserInstance User;
        
        public SessionInstance(Socket Client)
        {
            this.Client = Client;
            this.ping = false;
            this.EsperarDatos();
        }
        
        private void EsperarDatos()
        {
            try
            {
                if (this.Client.Connected)
                {
                    Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ProcesarDatos, null);
                }
            }
            catch (Exception)
            {
                this.FinalizarConexion("Datos");
            }
        }
        private void ProcesarDatos(IAsyncResult Result)
        {
            try
            {
                int Length = this.Client.EndReceive(Result);
                if (Length == 0 && Length > buffer.Length)
                {
                    this.FinalizarConexion("ProcesarDatos");
                    return;
                }
                char[] chars = new char[Length];
                Emulator.Encoding.GetChars(buffer, 0, Length, chars, 0);
                string Information = new String(chars);
                if (Information.StartsWith("<policy-file-request/>"))
                {
                    if (this.Policy)
                    {
                        this.FinalizarConexion("ProcesarDatos");
                        return;
                    }
                    this.Policy = true;
                    Client.Send(Emulator.Encoding.GetBytes("<?xml version=\"1.0\"?>\r\n<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n<cross-domain-policy>\r\n<allow-access-from domain=\"*\" to-ports=\"" + Emulator.puerto_server + "\" />\r\n</cross-domain-policy>\0"));
                }
                else
                {
                    if (Information[0] != Convert.ToChar(177)) 
                    { 
                        this.FinalizarConexion("ProcesarDatos");
                        return;
                    }
                    string[] Datas = Information.Split(Convert.ToChar(177));
                    for (int i = 1; i < Datas.Length; i++)
                    {
                        new InvokerManager(this, Convert.ToChar(177) + Datas[i]);
                    }
                }
                this.EsperarDatos();
            }
            catch
            {
                this.FinalizarConexion("ProcesarDatos");
            }
        }
        public void SendDataProtected(ServerMessage server)
        {
            try
            {
                if (this.Client.Connected)
                {
                    this.Client.Send(server.GetMessage());
                }
            }
            catch
            {
                this.FinalizarConexion("SendData");
            }
        }
        public void SendData(ServerMessage server)
        {
            try
            {
                if (this.Client.Connected)
                {
                    this.User.sendDataUser++;
                    this.Client.Send(server.GetMessage());  
                }
            }
            catch
            {
                this.FinalizarConexion("SendData");
            }
        }
        public void FinalizarConexion(string error)
        {
            if (this.Client.Connected)
            {
                this.Client.Shutdown(SocketShutdown.Both);
                this.Client.Disconnect(true);
            }
            if (this.User != null)
            {
                CerrarSesion(this, error);
            }
        }
        private void CerrarSesion(SessionInstance Session, string error)
        {
            if (Session.User != null)
            {
                UserManager.Ajustar_Remuneracion(Session.User);
                MiniGamesManager.CancelarInscripciones(Session.User);
                if (Session.User.Sala != null) SalasManager.Salir_Sala(Session);
                UserManager.UsuariosOnline.Remove(Session.User.id);
                if (Emulator.ver_conexion_usuarios == true)
                {
                    string console = "[UserManager] -> Se ha desconectado " + Session.User.nombre + ". > " + error;
                    Emulator.Form.WriteLine(console);
                }
                Emulator.Form.UpdateTitle();
            }
        }
    }
    public class ServerMessage
    {
        private readonly Encoding Encoding = Encoding.GetEncoding("iso-8859-1");
        private List<byte> Head;
        private List<object[]> Parameters;
        public ServerMessage()
        {
            this.Head = new List<byte>();
            this.Parameters = new List<object[]>();
        }
        public void AddHead(byte data)
        {
            Head.Add(data);
        }
        public void AddHead(byte[] data)
        {
            foreach (byte buffer in data)
            {
                Head.Add(buffer);
            }
        }
        public void AppendParameter(object Parameter)
        {
            Parameters.Add(new object[] { Parameter });
        }
        public void AppendParameter(object[] ParameterGroup)
        {
            Parameters.Add(ParameterGroup);
        }
        public byte[] GetMessage()
        {
            List<byte> Message = new List<byte>();
            Message.Add(0xb1);
            foreach (byte ActualHeader in Head)
            {
                Message.Add(ActualHeader);
                Message.Add(0xb3);
            }
            Message.Add(0xb2);
            foreach (object[] ParameterGroup in Parameters)
            {
                if (ParameterGroup != null)
                {
                    foreach (object Parameter in ParameterGroup)
                    {
                        if (Parameter != null && Convert.ToString(Parameter) != "")
                        {
                            foreach (byte ParameterByte in Encoding.GetBytes(Parameter.ToString()))
                            {
                                Message.Add(ParameterByte);
                            }
                        }
                        Message.Add(0xb3);
                    }
                }
                else
                {
                    Message.Add(0xb3);
                }
                Message.Add(0xb2);
            }
            Message.Add(0xb0);
            return Message.ToArray(); ;
        }
    }
}
