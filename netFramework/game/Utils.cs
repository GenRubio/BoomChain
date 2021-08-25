using BoomBang.game.instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomBang.game
{
    class Utils
    {
        public static void errorMessage(SessionInstance session)
        {
            ServerMessage server = new ServerMessage();
            server.AddHead(183);
            server.AppendParameter("Ha ocurrido un error. Vuelve a intentarlo mas tarde.");
            session.SendData(server);
        }
        public static bool checkValidCharacters(string texto, bool Register)
        {
            if (Register)
            {
                string[] Permitidos = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "ñ", "z", "x", "c", "v", "b", "n", "m", ",", ".", "-", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M", "@", "!", "=", ":", ".", ",", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string[] Letras = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "ñ", "z", "x", "c", "v", "b", "n", "m", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
                string[] Numeros = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string[] Caracteres = { "@", "!", "=", ":", ".", "," };
                int TotalCount = texto.Length;
                int Letras_Totales = 0;
                for (int id = 0; id < texto.Length; id++)
                {
                    if (Letras.Contains(texto[id].ToString()))
                    {
                        Letras_Totales++;
                        if (Letras_Totales > 11) { return false; }
                    }
                    if (!Permitidos.Contains(texto[id].ToString()))
                    {
                        return false;
                    }
                }
            }
            else
            {
                string[] Permitidos = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "ñ", "z", "x", "c", "v", "b", "n", "m", ",", ".", "-", "_", "á", "é", "í", "ó", "ú", "Á", "É", "Í", "Ó", "Ú", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Ñ", "Z", "X", "C", "V", "B", "N", "M", "{", "}", "[", "]", "@", "/", "-", "+", " ", "*", "'", "!", "#", "$", "%", "&", "(", ")", "=", "?", "¿", "¡", ":", ";", "<", ">", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                int TotalCount = texto.Length;
                for (int id = 0; id < texto.Length; id++)
                {
                    if (!Permitidos.Contains(texto[id].ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
