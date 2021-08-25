using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoomBang.game.instances
{
    class Avatar
    {
        private int avatar;
        private string colores;
        private int ficha_color;
        private string descripcion;
        private string hobby_1;
        private string hobby_2;
        private string hobby_3;
        private string deseo_1;
        private string deseo_2;
        private string deseo_3;
        private int votos_sexy;
        private int votos_legal;
        private int votos_simpatico;
        private int votos_restantes;
        private int besos_recibidos;
        private int besos_enviados;
        private int jugos_recibidos;
        private int jugos_enviados;
        private int flores_recibidos;
        private int flores_enviados;
        private int uppers_recibidos;
        private int uppers_enviados;
        private int cocos_recibidos;
        private int cocos_enviados;
        private int puntos_ring;
        private int puntos_senderos;
        private int puntos_cocos;
        private int puntos_ninja;
        private int upperSelected;
        private int cocoSelected;

        public Avatar(DataRow row)
        {
            avatar = (int)row["avatar"];
            colores = (string)row["colores"];
            ficha_color = (int)row["ficha_color"];
            descripcion = (string)row["descripcion"];
            hobby_1 = (string)row["hobby_1"];
            hobby_2 = (string)row["hobby_2"];
            hobby_3 = (string)row["hobby_3"];
            deseo_1 = (string)row["deseo_1"];
            deseo_2 = (string)row["deseo_2"];
            deseo_3 = (string)row["deseo_3"];
            votos_sexy = (int)row["votos_sexy"];
            votos_legal = (int)row["votos_legal"];
            votos_simpatico = (int)row["votos_simpatico"];
            votos_restantes = (int)row["votos_restantes"];
            besos_recibidos = (int)row["besos_recibidos"];
            besos_enviados = (int)row["besos_enviados"];
            jugos_recibidos = (int)row["jugos_recibidos"];
            jugos_enviados = (int)row["jugos_enviados"];
            flores_recibidos = (int)row["flores_recibidos"];
            flores_enviados = (int)row["flores_enviados"];
            uppers_recibidos = (int)row["uppers_recibidos"];
            uppers_enviados = (int)row["uppers_enviados"];
            cocos_recibidos = (int)row["cocos_recibidos"];
            cocos_enviados = (int)row["cocos_enviados"];
            puntos_ring = (int)row["puntos_ring"];
            puntos_senderos = (int)row["puntos_senderos"];
            puntos_cocos = (int)row["puntos_cocos"];
            puntos_ninja = (int)row["puntos_ninja"];
            upperSelected = setSelectedUpper();
            cocoSelected = setSelectedCoco();
        }
        public int getCocoSelected()
        {
            return cocoSelected;
        }
        public int getUpperSelected()
        {
            return upperSelected;
        }
        public int getPuntosNinja()
        {
            return puntos_ninja;
        }
        public int getPuntosSendero()
        {
            return puntos_senderos;
        }
        public int getPuntosRing()
        {
            return puntos_ring;
        }
        public int getCocosEnviados()
        {
            return cocos_enviados;
        }
        public int getCocosRecibidos()
        {
            return cocos_recibidos;
        }
        public int getUppersEnviados()
        {
            return uppers_enviados;
        }
        public int getUppersRecibidos()
        {
            return uppers_recibidos;
        }
        public int getFloresEnviadas()
        {
            return flores_enviados;
        }
        public int getFloresRecibidas()
        {
            return flores_recibidos;
        }
        public int getJugosEnviados()
        {
            return jugos_enviados;
        }
        public int getJugosRecibidos()
        {
            return jugos_recibidos;
        }
        public int getBesosEnviados()
        {
            return besos_enviados;
        }
        public int getBesosRecibidos()
        {
            return besos_recibidos;
        }
        public int getVotosRestantes()
        {
            return votos_restantes;
        }
        public int getVotosSimpatico()
        {
            return votos_simpatico;
        }
        public int getVotosLegal()
        {
            return votos_legal;
        }
        public string getDeseo3()
        {
            return deseo_3;
        }
        public string getDeseo2()
        {
            return deseo_2;
        }
        public string getDeseo1()
        {
            return deseo_1;
        }
        public string getHobby3()
        {
            return hobby_3;
        }
        public string getHobby2()
        {
            return hobby_2;
        }
        public string getHobby1()
        {
            return hobby_1;
        }
        public string getDescripcion()
        {
            return descripcion;
        }
        public int getFichaColor()
        {
            return ficha_color;
        }
        private int setSelectedCoco()
        {
            if (puntos_cocos >= 10 && puntos_cocos <= 19) return 1;
            if (puntos_cocos >= 20 && puntos_cocos <= 49) return 2;
            if (puntos_cocos >= 50 && puntos_cocos <= 99) return 3;
            if (puntos_cocos >= 100 && puntos_cocos <= 149) return 4;
            if (puntos_cocos >= 150 && puntos_cocos <= 199) return 5;
            if (puntos_cocos >= 200 && puntos_cocos <= 299) return 6;
            if (puntos_cocos >= 300 && puntos_cocos <= 399) return 7;
            if (puntos_cocos >= 400 && puntos_cocos <= 599) return 8;
            if (puntos_cocos >= 600) return 9;
            return 0;
        }
        private int setSelectedUpper()
        {
            if (puntos_ring >= 1 && puntos_ring <= 9) return 1;
            if (puntos_ring >= 10 && puntos_ring <= 24) return 2;
            if (puntos_ring >= 25 && puntos_ring <= 49) return 3;
            if (puntos_ring >= 50 && puntos_ring <= 99) return 4;
            if (puntos_ring >= 100 && puntos_ring <= 199) return 5;
            if (puntos_ring >= 200 && puntos_ring <= 499) return 6;
            if (puntos_ring >= 500 && puntos_ring <= 999) return 7;
            if (puntos_ring >= 1000 && puntos_ring <= 1999) return 8;
            if (puntos_ring >= 2000) return 9;
            return 0;
        }
        public void addPuntosNinja(int value)
        {
            puntos_ninja = puntos_ninja + value;
        }
        public void addPuntosCocos(int value)
        {
            puntos_cocos = puntos_cocos + value;
        }
        public void addPuntosSendero()
        {
            puntos_senderos = puntos_senderos + 1;
        }
        public void addPuntosRing()
        {
            puntos_ring = puntos_ring + 1;
        }
        public void addCocosEnviados()
        {
            cocos_enviados = cocos_enviados + 1;
        }
        public void addCocosRecibidos()
        {
            cocos_recibidos = cocos_recibidos + 1;
        }
        public void addUppersEnviados()
        {
            uppers_enviados = uppers_enviados + 1;
        }
        public void addUppersRecibidos()
        {
            uppers_recibidos = uppers_recibidos + 1;
        }
        public void addFloresEnviadas()
        {
            flores_enviados = flores_enviados + 1;
        }
        public void addFloresRecibidas()
        {
            flores_recibidos = flores_recibidos + 1;
        }
        public void addJugosEnviados()
        {
            jugos_enviados = jugos_enviados + 1;
        }
        public void addJugosRecibidos()
        {
            jugos_recibidos = jugos_recibidos + 1;
        }
        public void addBesosEnviados()
        {
            besos_enviados = besos_enviados + 1;
        }
        public void addBesosRecibidos()
        {
            besos_recibidos = besos_recibidos + 1;
        }
        public void removeVotosRestantes(int value)
        {
            votos_restantes = votos_restantes - value;
        }
        public void addVotosRestantes(int value)
        {
            votos_restantes = votos_restantes + value;
        }
        public void removeVotosSimpatico(int value)
        {
            votos_simpatico = votos_simpatico - value;
        }
        public void addVotosSimpatico(int value)
        {
            votos_simpatico = votos_simpatico + value;
        }
        public void removeVotosLegal(int value)
        {
            votos_legal = votos_legal - value;   
        }
        public void addVotosLegal(int value)
        {
            votos_legal = votos_legal + value;
        }
        public void updateDeseo3(string value)
        {
            deseo_3 = value;
        }
        public void updateDeseo2(string value)
        {
            deseo_2 = value;
        }
        public void updateDeseo1(string value)
        {
            deseo_1 = value;
        }
        public void updateHobby3(string value)
        {
            hobby_3 = value;
        }
        public void updateHobby2(string value)
        {
            hobby_2 = value;
        }
        public void updateHobby1(string value)
        {
            hobby_1 = value;
        }

        public void updateDescripcion(string value)
        {
            descripcion = value;
        }
        public void updateColores(string value)
        {
            colores = value;
        }

        public void updateAvatar(int value)
        {
            if (value == 12)
            {
                if (getNinjaLevel() > 0)
                {
                    avatar = 12;
                }
                else
                {
                    avatar = 1;
                }
            }
            else if (value < 0 || value > 14)
            {
                avatar = 1;
            }
            else
            {
                avatar = value;
            }
        }

        public int getAvatar()
        {
            if (avatar == 12)
            {
                if (getNinjaLevel() > 0)
                {
                    return 12;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return avatar;
            }
        }

        public string getColores()
        {
            if (avatar == 12)
            {
                string EXTRA_COLORS = "AAAAAAAAAAAAAAAAAA";
                return "000000"
                    + colores.Substring(0, 6)
                    + getSinturonColorNinja(getNinjaLevel())
                    + colores.Substring(18, 6)
                    + EXTRA_COLORS;
            }
            else
            {
                return colores;
            }
        }

        private string getSinturonColorNinja(int ninjaLevel)
        {
            switch (ninjaLevel)
            {
                case 0:
                    return "C9CACF";
                case 1:
                    return "FF0000";
                case 2:
                    return "FF3399";
                case 3:
                    return "FF6600";
                case 4:
                    return "00CC00";
                case 5:
                    return "0066CC";
                case 6:
                    return "FFFFFF";
                case 7:
                    return "660099";
                case 8:
                    return "653232";
                case 9:
                    return "222222";
                case 10:
                    return "f2b100";
                default:
                    return "FFFFFF";
            }
        }

        private int getNinjaLevel()
        {
            if (puntos_ninja >= 400 && puntos_ninja <= 409) return 1;
            if (puntos_ninja >= 410 && puntos_ninja <= 419) return 2;
            if (puntos_ninja >= 420 && puntos_ninja <= 449) return 3;
            if (puntos_ninja >= 450 && puntos_ninja <= 499) return 4;
            if (puntos_ninja >= 500 && puntos_ninja <= 549) return 5;
            if (puntos_ninja >= 550 && puntos_ninja <= 599) return 6;
            if (puntos_ninja >= 600 && puntos_ninja <= 699) return 7;
            if (puntos_ninja >= 700 && puntos_ninja <= 799) return 8;
            if (puntos_ninja >= 800 && puntos_ninja <= 999) return 9;
            if (puntos_ninja >= 1000) return 10;
            return 0;
        }
    }
}
