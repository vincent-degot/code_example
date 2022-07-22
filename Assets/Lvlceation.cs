using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Lvlceation : MonoBehaviour
{
    public GameObject plafond;
    public GameObject trappe;
    public GameObject echelle;
    public List<GameObject> BigRoom;
    public GameObject wall;
    public GameObject pilipilonne;
    public GameObject secret;
    public List<GameObject> shop;
    public List<GameObject> culDeSac;
    public List<GameObject> coridors;
    public List<GameObject> CornerRoom;
    public List<GameObject> normalRoom;
    public List<GameObject> deco;

    private System.Random rnd;
    public float g = 25f;

    public class salle
    {
        public int X;
        public int Y;
        public int HAUTEUR;
        public List<string> ways;
        public List<List<float>> décore;
        public List<string> pilo;
        public List<int> Room_Info;

        public List<int> Room_InfoMax;
       
       
        public List<bool> PlaceContreLesMures;
        public List<bool> PlaceAuCentre;

        public salle(int x, int y, int h, List<int> info, List<int> MAX)
        {
            Room_InfoMax = MAX;
            X = x;
            Y = y;
            HAUTEUR = h;
            ways = new List<string>();
            décore = new List<List<float>>();
            pilo = new List<string>() {"hd", "hg", "bd", "bg"};
            Room_Info = info;
            PlaceContreLesMures = new List<bool> {true, true, true, true, true, true};
            PlaceAuCentre = new List<bool> {true, true, true, true, true, true, true, true, true};
        }
    }

    public class obj
    {
        public bool ContreUnMur;
        public List<List<float>> Info; //{X,Y,H,Rota}
        public int ID;

        public obj(bool Contr_Le_Mur, List<List<float>> Les_Infos, int objt)
        {
            ID = objt;
            Info = Les_Infos;
            ContreUnMur = Contr_Le_Mur;
        }
    }


    public List<salle> B(int x_b, int y_b, int h, List<int> Infos_Room, List<int> InfosMax,
        int lim_min, int lim_max,
        string TypeOfWay,
        List<salle> salles,
        int secret_chance,
        int chance_to_break)
    {
        int cont = 0;
        salle actuel = new salle(0, 0, h, new List<int> {0, 0, 0, 0}, new List<int> {0, 0, 0, 0});
        if (TypeOfWay == "debut")
        {
            actuel = new salle(x_b, y_b, h, new List<int> {-1, -1, -1, 10,20,-1,-1,18,-1}, new List<int> {0, 0, 0, 2,1,0,0,2,0});
            actuel.ways.Add("debut");
            salles.Add(actuel);
        }

        if (TypeOfWay == "secret")
        {
            bool b = true;
            foreach (salle s in salles)
            {
                if ((s.X == x_b) && (s.Y == y_b))
                {
                    b = false;
                    actuel = s;
                    actuel.ways.Add("echelle");
                }
            }

            if (b)
            {
                actuel = new salle(x_b, y_b, h, new List<int> {-1, -1, -1, 10,20,-1,-1,18,-1}, new List<int> {0, 0, 0, 2,1,0,0,2,0});
                actuel.ways.Add("echelle");
                salles.Add(actuel);
            }
        }
        else
        {
            foreach (salle s in salles)
            {
                if ((s.X == x_b) && (s.Y == y_b))
                {
                    actuel = s;
                }
            }
        }


        bool notblock = true;
        List<salle> directions = new List<salle>();
        List<salle> directions_2 = new List<salle>();

        while (notblock)
        {

            if (TypeOfWay == "alt")
            {
                actuel.ways.Add("alt");
            }

            directions.Clear();
            directions_2.Clear();

            salle Tog = new salle(actuel.X - 1, actuel.Y, h, Infos_Room, InfosMax);
            Tog.ways.Add("d");
            directions.Add(Tog);

            salle Tob = new salle(actuel.X, actuel.Y - 1, h, Infos_Room, InfosMax);
            Tob.ways.Add("h");
            directions.Add(Tob);

            salle Tod = new salle(actuel.X + 1, actuel.Y, h, Infos_Room, InfosMax);
            Tod.ways.Add("g");
            directions.Add(Tod);

            salle Toh = new salle(actuel.X, actuel.Y + 1, h, Infos_Room, InfosMax);
            Toh.ways.Add("b");
            directions.Add(Toh);


            foreach (salle s in directions)
            {
                bool j = true;
                foreach (salle Sa in salles)
                {

                    if ((s.X == Sa.X) && (s.Y == Sa.Y))
                    {
                        j = false;
                    }
                }

                if (j)
                {
                    directions_2.Add(s);
                }
            }

            if ((directions_2.Count == 0) || ((cont >= lim_max)))
            {
                notblock = false;
                if (TypeOfWay == "debut")
                {
                    actuel.ways.Add("fin");
                    actuel.Room_Info =  new List<int> {-1, -1, -1, 10, -1, -1, -1, 18, -1};
                    actuel.Room_InfoMax= new List<int> {0, 0, 0, 2,1,0,0,2,0};

                }
                else
                {
                    if (secret_chance > 0)
                    {
                        int rnd = Random.Range(0, secret_chance);
                        if (rnd == 0)
                        {
                            actuel.ways.Add("secret");
                            actuel.Room_Info = new List<int> {-1, -1, -1, 10, -1, 15, 30, 18, -1};
                            actuel.Room_InfoMax= new List<int> {0, 0, 0, 2, 1, 1, 1, 2, 0};
                        }
                    }
                }

                break;
            }

            int rnnd = Random.Range(0, directions_2.Count);
            switch (directions_2[rnnd].ways[0])
            {
                case "h":
                    actuel.ways.Add("b");
                    break;
                case "b":
                    actuel.ways.Add("h");
                    break;
                case "g":
                    actuel.ways.Add("d");
                    break;
                case "d":
                    actuel.ways.Add("g");
                    break;
            }

            actuel = directions_2[rnnd];
            cont += 1;
            salles.Add(actuel);
            if (cont >= lim_min)
            {
                int rnd = Random.Range(0, chance_to_break);
                if (rnd == 0)
                {
                    notblock = false;
                }
            }
        }

        if (salles.Count < lim_min)
        {
            salles.Clear();
            salles = B(x_b, y_b, h, Infos_Room, InfosMax, lim_min, lim_max, TypeOfWay, salles, secret_chance,
                chance_to_break);
        }

        return salles;
    }

    public void isal(List<salle> chemin, int Proba_BigRoom)
    {
        foreach (salle s in chemin)
        {
            if (!s.ways.Contains("h"))
            {
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y + 1))
                    {
                        sa.ways.Add("b");
                        int rnd = Random.Range(0, 1);
                        if (rnd == 2)
                        {
                            s.ways.Add("h");
                        }
                        else
                        {
                            sa.ways.Add("sb");
                        }
                    }
                }
            }

            if (!s.ways.Contains("b"))
            {
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y - 1))
                    {
                        sa.ways.Add("h");
                        int rnd = Random.Range(0, 1);
                        if (rnd == 2)
                        {
                            s.ways.Add("b");
                        }
                        else
                        {
                            sa.ways.Add("sh");
                        }
                    }
                }
            }

            if (!s.ways.Contains("g"))
            {
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X - 1) && (sa.Y == s.Y))
                    {
                        sa.ways.Add("d");
                        int rnd = Random.Range(0, 1);
                        if (rnd == 2)
                        {
                            s.ways.Add("g");
                        }
                        else
                        {
                            sa.ways.Add("sd");
                        }
                    }
                }
            }

            if (!s.ways.Contains("d"))
            {
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X + 1) && (sa.Y == s.Y))
                    {
                        sa.ways.Add("g");
                        int rnd = Random.Range(0, 1);
                        if (rnd == 2)
                        {
                            s.ways.Add("d");
                        }
                        else
                        {
                            sa.ways.Add("sg");
                        }
                    }
                }
            }

            if (Proba_BigRoom > 0)
            {
                int rnnd = Random.Range(0, Proba_BigRoom);
                if (vid(s) && (rnnd == 0))
                {
                    bool bas = false;
                    bool droite = false;
                    bool bas_droite = false;

                    foreach (salle sa in chemin)
                    {
                        if (vid(sa) && (s.X == sa.X) && (s.Y - 1 == sa.Y))
                        {
                            bas = true;
                        }

                        if (vid(sa) && (s.X + 1 == sa.X) && (s.Y - 1 == sa.Y))
                        {
                            bas_droite = true;
                        }

                        if (vid(sa) && (s.X + 1 == sa.X) && (s.Y == sa.Y))
                        {
                            droite = true;
                        }
                    }

                    if ((bas) && (bas_droite) && (droite))
                    {
                        s.ways.Add("BigRoom");
                        s.ways.Add("hg");
                        s.ways.Add("b");
                        s.ways.Add("d");
                        s.pilo.Remove("bd");
                        foreach (salle sa in chemin)
                        {
                            if ((s.X == sa.X) && (s.Y - 1 == sa.Y))
                            {
                                sa.ways.Add("BigRoom");
                                sa.ways.Add("bg");
                                sa.ways.Add("h");
                                sa.ways.Add("d");
                                sa.pilo.Remove("hd");
                            }

                            if ((s.X + 1 == sa.X) && (s.Y - 1 == sa.Y))
                            {
                                sa.ways.Add("BigRoom");
                                sa.ways.Add("bd");
                                sa.ways.Add("h");
                                sa.ways.Add("g");
                                sa.pilo.Remove("hg");
                            }

                            if ((s.X + 1 == sa.X) && (s.Y == sa.Y))
                            {
                                sa.ways.Add("BigRoom");
                                sa.ways.Add("hd");
                                sa.ways.Add("b");
                                sa.ways.Add("g");
                                sa.pilo.Remove("bg");
                            }
                        }
                    }
                }
            }
        }
    }

    public bool vid(salle s)
    {
        return ((!s.ways.Contains("normalRoom")) && (!s.ways.Contains("coridors")) && (!s.ways.Contains("culDeSac")) &&
                (!s.ways.Contains("BigRoom")) && (!s.ways.Contains("shop")) && (!s.ways.Contains("CornerRoom"))
                && (!s.ways.Contains("debut")) && (!s.ways.Contains("fin")) && (!s.ways.Contains("secret")));
    }

    public bool eg(salle s, List<string> WaysWeWant, List<string> WaysWeDont, List<string> WaysWyNot)
    {
        foreach (string w in WaysWyNot)
        {
            if ((s.ways.Contains(w)) && (!s.ways.Contains("s" + w)))
            {
                return false;
            }
        }

        foreach (string w in WaysWeDont)
        {
            if (s.ways.Contains(w))
            {
                return false;
            }
        }

        int x = 0;
        foreach (string w in WaysWeWant)
        {
            if (s.ways.Contains(w))
            {
                x += 1;
            }
        }

        return WaysWeWant.Count == x;
    }

    public void it(List<salle> chemin)
    {
        float t = 7f;
        float h = 2.5f;
        foreach (salle s in chemin)
        {
            if (!s.ways.Contains("BigRoom"))
            {

                string a = "normalRoom";
                if (eg(s, new List<string>() {"h", "b"}, new List<string>() {"sh", "sb"},
                    new List<string>() {"g", "d"}))
                {
                    a = "coridors";
                    if (!s.ways.Contains("shopg"))
                    {
                        s.décore.Add(new List<float>() {-t, 0, h /*hauteur*/, 180, 1});
                    }
                    else
                    {
                        s.décore.Add(new List<float>() {-t, 3, h /*hauteur*/, 180, 1});
                        s.PlaceContreLesMures[0] = false;
                        s.PlaceContreLesMures[1] = false;
                    }

                    if (!s.ways.Contains("shopd"))
                    {
                        s.décore.Add(new List<float>() {t, 0, h /*hauteur*/, 0, 1});
                    }
                    else
                    {
                        s.décore.Add(new List<float>() {t, -3, h /*hauteur*/, 0, 1});
                        s.PlaceContreLesMures[4] = false;
                        s.PlaceContreLesMures[5] = false;
                    }

                    s.PlaceContreLesMures[2] = false;
                    s.PlaceContreLesMures[3] = false;
                }

                if (eg(s, new List<string>() {"g", "d"}, new List<string>() {"sg", "sd"},
                    new List<string>() {"h", "b"}))
                {
                    a = "coridors";
                    if (!s.ways.Contains("shoph"))
                    {
                        s.décore.Add(new List<float>() {0, t, h /*hauteur*/, -90, 1});
                    }
                    else
                    {
                        s.décore.Add(new List<float>() {3, t, h /*hauteur*/, -90, 1});
                        s.PlaceContreLesMures[2] = false;
                        s.PlaceContreLesMures[3] = false;
                    }

                    s.décore.Add(new List<float>() {0, -t, h /*hauteur*/, 90, 1});
                    s.PlaceContreLesMures[0] = false;
                    s.PlaceContreLesMures[1] = false;
                    s.PlaceContreLesMures[4] = false;
                    s.PlaceContreLesMures[5] = false;
                }

                if (eg(s, new List<string>() {"h"}, new List<string>() {"sh"}, new List<string>() {"b", "d", "g"}))
                {
                    a = "culDeSac";
                    s.Room_Info[5] -= 1;
                    s.Room_Info[6] -= 8;
                    s.décore.Add(new List<float>() {-t, -t, h /*hauteur*/, -45, 1});
                    s.décore.Add(new List<float>() {t, -t, h /*hauteur*/, 45, 1});
                    s.PlaceContreLesMures[2] = false;
                    s.PlaceContreLesMures[3] = false;
                }

                if (eg(s, new List<string>() {"b"}, new List<string>() {"sb"}, new List<string>() {"h", "d", "g"}))
                {
                    a = "culDeSac";
                    s.Room_Info[5] -= 1;
                    s.Room_Info[6] -= 8;
                    s.décore.Add(new List<float>() {t, t, h /*hauteur*/, -45, 1});
                    s.décore.Add(new List<float>() {-t, t, h /*hauteur*/, -135, 1});
                }

                if (eg(s, new List<string>() {"d"}, new List<string>() {"sd"}, new List<string>() {"b", "h", "g"}))
                {
                    a = "culDeSac";
                    s.Room_Info[5] -= 1;
                    s.Room_Info[6] -= 8;
                    s.décore.Add(new List<float>() {-t, -t, h /*hauteur*/, 135, 1});
                    s.décore.Add(new List<float>() {-t, t, h /*hauteur*/, -135, 1});
                    s.PlaceContreLesMures[4] = false;
                    s.PlaceContreLesMures[5] = false;
                }

                if (eg(s, new List<string>() {"g"}, new List<string>() {"sg"}, new List<string>() {"b", "d", "h"}))
                {
                    a = "culDeSac";
                    s.Room_Info[5] -= 1;
                    s.Room_Info[6] -= 8;
                    s.décore.Add(new List<float>() {t, -t, h /*hauteur*/, 45, 1});
                    s.décore.Add(new List<float>() {t, t, h /*hauteur*/, -45, 1});
                    s.PlaceContreLesMures[0] = false;
                    s.PlaceContreLesMures[1] = false;

                }

                if (eg(s, new List<string>() {"h", "d"}, new List<string>() {"sh", "sd"},
                    new List<string>() {"b", "g"}))
                {
                    a = "CornerRoom";
                    s.décore.Add(new List<float>() {-t, -t, h /*hauteur*/, 135, 1});
                    s.PlaceContreLesMures[2] = false;
                    s.PlaceContreLesMures[3] = false;
                    s.PlaceContreLesMures[4] = false;
                    s.PlaceContreLesMures[5] = false;
                }

                if (eg(s, new List<string>() {"b", "d"}, new List<string>() {"sb", "sd"},
                    new List<string>() {"h", "g"}))
                {
                    a = "CornerRoom";
                    s.décore.Add(new List<float>() {-t, t, h /*hauteur*/, -135, 1});
                    s.PlaceContreLesMures[4] = false;
                    s.PlaceContreLesMures[5] = false;
                }

                if (eg(s, new List<string>() {"h", "g"}, new List<string>() {"sh", "sg"},
                    new List<string>() {"b", "d"}))
                {
                    a = "CornerRoom";
                    s.décore.Add(new List<float>() {t, -t, h /*hauteur*/, 45, 1});
                    s.PlaceContreLesMures[2] = false;
                    s.PlaceContreLesMures[3] = false;
                    s.PlaceContreLesMures[0] = false;
                    s.PlaceContreLesMures[1] = false;
                }

                if (eg(s, new List<string>() {"g", "b"}, new List<string>() {"sg", "sb"},
                    new List<string>() {"d", "h"}))
                {
                    a = "CornerRoom";
                    s.décore.Add(new List<float>() {t, t, h /*hauteur*/, -45, 1});
                    s.PlaceContreLesMures[0] = false;
                    s.PlaceContreLesMures[1] = false;
                }

                if (a == "normalRoom")
                {
                    s.PlaceContreLesMures[0] = false;
                    s.PlaceContreLesMures[1] = false;
                    s.PlaceContreLesMures[2] = false;
                    s.PlaceContreLesMures[3] = false;
                    s.PlaceContreLesMures[4] = false;
                    s.PlaceContreLesMures[5] = false;
                    if (LesMURES(s).Contains("h"))
                    {
                        if (s.ways.Contains("shoph"))
                        {
                            s.décore.Add(new List<float>() {3, t, h /*hauteur*/, -90, 1});
                        }
                        else
                        {
                            s.décore.Add(new List<float>() {0, t, h /*hauteur*/, -90, 1});
                            s.PlaceContreLesMures[2] = true;
                            s.PlaceContreLesMures[3] = true;
                        }
                    }

                    if (LesMURES(s).Contains("b"))
                    {
                        s.décore.Add(new List<float>() {0, -t, h /*hauteur*/, 90, 1});
                    }

                    if (LesMURES(s).Contains("g"))
                    {
                        if (s.ways.Contains("shopg"))
                        {
                            s.décore.Add(new List<float>() {-t, 3, h /*hauteur*/, 180, 1});
                        }
                        else
                        {


                            s.décore.Add(new List<float>() {-t, 0, h /*hauteur*/, 180, 1});
                            s.PlaceContreLesMures[0] = true;
                            s.PlaceContreLesMures[1] = true;
                        }
                    }

                    if (LesMURES(s).Contains("d"))
                    {
                        if (s.ways.Contains("shopg"))
                        {
                            s.décore.Add(new List<float>() {t, -3, h /*hauteur*/, 0, 1});
                        }
                        else
                        {
                            s.décore.Add(new List<float>() {t, 0, h /*hauteur*/, 0, 1});
                            s.PlaceContreLesMures[4] = true;
                            s.PlaceContreLesMures[5] = true;
                        }
                    }
                }

                s.ways.Add(a);
            }
            else
            {
                s.PlaceContreLesMures[0] = false;
                s.PlaceContreLesMures[1] = false;
                s.PlaceContreLesMures[2] = false;
                s.PlaceContreLesMures[3] = false;
                s.PlaceContreLesMures[4] = false;
                s.PlaceContreLesMures[5] = false;
                if (LesMURES(s).Contains("h"))
                {
                    s.décore.Add(new List<float>() {0, t, h /*hauteur*/, -90, 1});
                    s.PlaceContreLesMures[2] = true;
                    s.PlaceContreLesMures[3] = true;
                }

                if (LesMURES(s).Contains("b"))
                {
                    s.décore.Add(new List<float>() {0, -t, h /*hauteur*/, 90, 1});
                }

                if (LesMURES(s).Contains("g"))
                {
                    s.décore.Add(new List<float>() {-t, 0, h /*hauteur*/, 180, 1});
                    s.PlaceContreLesMures[0] = true;
                    s.PlaceContreLesMures[1] = true;
                }

                if (LesMURES(s).Contains("d"))
                {
                    s.décore.Add(new List<float>() {t, 0, h /*hauteur*/, 0, 1});
                    s.PlaceContreLesMures[4] = true;
                    s.PlaceContreLesMures[5] = true;
                }
            }
        }
    }

    public void Shop(List<salle> chem)
    {
        bool ThereIsAShop = false;
        foreach (salle s in chem)
        {
            if (vid(s))
            {
                int rnd = Random.Range(0, chem.Count);
                List<string> mures = LesMURES(s);
                mures.Remove("b");
                if ((rnd == 1) && (mures.Count > 0))
                {
                    s.ways.Add("shop");
                    int rnnd = Random.Range(0, mures.Count);
                    s.ways.Add("shop" + mures[rnnd]); 
                    s.Room_Info = new List<int> {-1, -1, -1, 10,-1,-1,-1,5,-1};
                    ThereIsAShop = true;
                    break;
                }
            }
        }

        if (!ThereIsAShop)
        {
            Shop(chem);
        }
    }

    public void Creation(List<salle> chemin, bool MapNeeded)
    {

        int xmax = 0;
        int ymax = 0;
        int xmin = 0;
        int ymin = 0;
        Vector3 vec = new Vector3(0, 0, 0);
        foreach (salle s in chemin)
        {
            if (s.X > xmax)
            {
                xmax = s.X;
            }

            if (s.Y > ymax)
            {
                ymax = s.Y;
            }

            if (s.X < xmin)
            {
                xmin = s.X;
            }

            if (s.Y < ymin)
            {
                ymin = s.Y;
            }

            if (s.ways.Contains("debut"))
            {
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.ways.Contains("secret"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(secret, vec, Quaternion.identity);                
                Instantiate(trappe, vec, Quaternion.identity);
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }


            }

            if (s.ways.Contains("fin"))
            {
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.ways.Contains("normalRoom"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(normalRoom[rnd.Next(normalRoom.Count)], vec, Quaternion.identity);
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.ways.Contains("coridors"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(coridors[rnd.Next(coridors.Count)], vec, Quaternion.identity);
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.ways.Contains("culDeSac"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(culDeSac[rnd.Next(culDeSac.Count)], vec, Quaternion.identity);
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.ways.Contains("BigRoom"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Vector3 vvec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                if (s.ways.Contains("hd"))
                {
                    //Instantiate(BigRoom[2], vec, Quaternion.identity);
                    if (MapNeeded)
                    {
                        Instantiate(plafond, vvec, Quaternion.identity);
                    }
                }

                if (s.ways.Contains("hg"))
                {
                    //Instantiate(BigRoom[3], vec, Quaternion.identity);
                    if (MapNeeded)
                    {
                        Instantiate(plafond, vvec, Quaternion.identity);
                    }
                }

                if (s.ways.Contains("bd"))
                {
                    //Instantiate(BigRoom[0], vec, Quaternion.identity);
                    if (MapNeeded)
                    {
                        Instantiate(plafond, vvec, Quaternion.identity);
                    }
                }

                if (s.ways.Contains("bg"))
                {
                    //Instantiate(BigRoom[1], vec, Quaternion.identity);
                    if (MapNeeded)
                    {
                        Instantiate(plafond, vvec, Quaternion.identity);
                    }
                }

            }

            if (s.ways.Contains("shop"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                //Instantiate(shop[0], vec, Quaternion.identity);
                List<string> mures = LesMURES(s);
                mures.Remove("b");

                if (s.ways.Contains("shoph"))
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g + 6.40f);
                    //Instantiate(shop[1], vec, Quaternion.Euler(0, 180, 0));
                }

                if (s.ways.Contains("shopg"))
                {
                    vec = new Vector3(s.X * g - 6.40f, s.HAUTEUR, s.Y * g);
                    //Instantiate(shop[1], vec, Quaternion.Euler(0, 90, 0));
                }

                if (s.ways.Contains("shopd"))
                {
                    vec = new Vector3(s.X * g + 6.40f, s.HAUTEUR, s.Y * g);
                    //Instantiate(shop[1], vec, Quaternion.Euler(0, -90, 0));
                }

                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }

            }

            if (!s.ways.Contains("h"))
            {
                vec = new Vector3((s.X * g), s.HAUTEUR + 2, s.Y * g + (g / 2f));
                Instantiate(wall, vec, Quaternion.identity);
            }

            if (!s.ways.Contains("b"))
            {
                vec = new Vector3((s.X * g), s.HAUTEUR + 2, s.Y * g - (g / 2f));
                Instantiate(wall, vec, Quaternion.identity);
            }

            if (!s.ways.Contains("g"))
            {
                vec = new Vector3(s.X * g - (g / 2f), s.HAUTEUR + 2, (s.Y * g));
                Instantiate(wall, vec, Quaternion.Euler(0, 90, 0));
            }

            if (!s.ways.Contains("d"))
            {
                vec = new Vector3(s.X * g + (g / 2f), s.HAUTEUR + 2, (s.Y * g));
                Instantiate(wall, vec, Quaternion.Euler(0, 90, 0));
            }

            if (s.ways.Contains("CornerRoom"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(CornerRoom[rnd.Next(CornerRoom.Count)], vec, Quaternion.identity);
                if (MapNeeded)
                {
                    vec = new Vector3(s.X * g, s.HAUTEUR + 20, s.Y * g);
                    Instantiate(plafond, vec, Quaternion.identity);
                }
            }

            if (s.pilo.Contains("hd"))
            {
                vec = new Vector3(s.X * g + (g / 2f), s.HAUTEUR + 2, s.Y * g + (g / 2f));
                Instantiate(pilipilonne, vec, Quaternion.identity);
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y + 1))
                    {
                        sa.pilo.Remove("bd");
                    }

                    if ((sa.X == s.X + 1) && (sa.Y == s.Y))
                    {
                        sa.pilo.Remove("hg");
                    }

                    if ((sa.X == s.X + 1) && (sa.Y == s.Y + 1))
                    {
                        sa.pilo.Remove("bg");
                    }
                }
            }

            if (s.pilo.Contains("hg"))
            {
                vec = new Vector3(s.X * g - (g / 2f), s.HAUTEUR + 2, s.Y * g + (g / 2f));
                Instantiate(pilipilonne, vec, Quaternion.identity);
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y + 1))
                    {
                        sa.pilo.Remove("bg");
                    }

                    if ((sa.X == s.X - 1) && (sa.Y == s.Y))
                    {
                        sa.pilo.Remove("hd");
                    }

                    if ((sa.X == s.X - 1) && (sa.Y == s.Y + 1))
                    {
                        sa.pilo.Remove("bd");
                    }
                }
            }

            if (s.pilo.Contains("bd"))
            {
                vec = new Vector3(s.X * g + (g / 2f), s.HAUTEUR + 2, s.Y * g - (g / 2f));
                Instantiate(pilipilonne, vec, Quaternion.identity);
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y - 1))
                    {
                        sa.pilo.Remove("hd");
                    }

                    if ((sa.X == s.X + 1) && (sa.Y == s.Y))
                    {
                        sa.pilo.Remove("bg");
                    }

                    if ((sa.X == s.X + 1) && (sa.Y == s.Y - 1))
                    {
                        sa.pilo.Remove("hg");
                    }
                }
            }

            if (s.pilo.Contains("bg"))
            {
                vec = new Vector3(s.X * g - (g / 2f), s.HAUTEUR + 2, s.Y * g - (g / 2f));
                Instantiate(pilipilonne, vec, Quaternion.identity);
                foreach (salle sa in chemin)
                {
                    if ((sa.X == s.X) && (sa.Y == s.Y - 1))
                    {
                        sa.pilo.Remove("hg");
                    }

                    if ((sa.X == s.X - 1) && (sa.Y == s.Y))
                    {
                        sa.pilo.Remove("bd");
                    }

                    if ((sa.X == s.X - 1) && (sa.Y == s.Y - 1))
                    {
                        sa.pilo.Remove("hd");
                    }
                }
            }

            if (s.ways.Contains("echelle"))
            {
                vec = new Vector3(s.X * g, s.HAUTEUR, s.Y * g);
                Instantiate(echelle, vec, Quaternion.Euler(0, -90, 0));
            }

            foreach (List<float> li in s.décore)
            {
                vec = new Vector3(s.X * g + li[0], li[2] + s.HAUTEUR, s.Y * g + li[1]);
                //Instantiate(deco[(int) li[4]], vec, Quaternion.Euler(0, li[3], 0));
            }
        }
    }

    public List<salle> SearchSecret(List<salle> chemin, List<int> info, List<int> infomax,
        int min, int max,
        int proba_alt, int proba_break, int altmax, int proba_altbreak)
    {
        int l = 0;
        List<salle> LESSALLES = new List<salle>();
        foreach (salle s in chemin)
        {
            if (s.ways.Contains("secret"))
            {
                l += 1;
                LESSALLES =
                    (CreateMaze(s.X, s.Y, -100000, info, infomax, min, max, LESSALLES, "secret", proba_alt, -1,
                        proba_break, altmax, proba_altbreak, -1, false));
            }
        }

        Debug.Log("ya " + l + " étage cacher!!!");
        return LESSALLES;
    }

    public List<string> LesMURES(salle s)
    {

        List<string> a = new List<string>() {"h", "b", "g", "d"};
        foreach (string str in s.ways)
        {
            if (a.Contains(str))
            {
                a.Remove(str);
            }

            if (str == "sh")
            {
                a.Add("h");
            }

            if (str == "sb")
            {
                a.Add("b");
            }

            if (str == "sd")
            {
                a.Add("d");
            }

            if (str == "sg")
            {
                a.Add("g");
            }

        }

        return a;
    }

    public void
        init_décor(List<salle> chem,
            List<obj> objets) ////////////////////////////////////////////////////////////////////
    {

        List<obj> ObjetsContreLesMures = new List<obj>();
        List<obj> ObjetsAuCentreDeLaSalle = new List<obj>();
        foreach (obj o in objets)
        {
            if (o.ContreUnMur)
            {
                ObjetsContreLesMures.Add(o);
            }
            else
            {
                ObjetsAuCentreDeLaSalle.Add(o);
            }
        }

        List<int> max = new List<int>();
        foreach (salle s in chem)
        {
            List<int> saveinfo = new List<int>();
            foreach (int i in s.Room_Info)
            {
                saveinfo.Add(i);
            }

            max = new List<int>();
            int probaContreLesMures = 1;
            foreach (obj o in ObjetsContreLesMures)
            {
                if (s.Room_Info[o.ID] > 0)
                {
                    probaContreLesMures = probaContreLesMures * s.Room_Info[o.ID];
                }
            }

            int probaAuCentre = 1;
            foreach (obj o in ObjetsAuCentreDeLaSalle)
            {
                if (s.Room_Info[o.ID] > 0)
                {
                    probaAuCentre = probaAuCentre * s.Room_Info[o.ID];
                }
            }


            foreach (int i in s.Room_InfoMax)
            {
                max.Add(0);
            }

            int x = 0;
            foreach (bool b in s.PlaceContreLesMures)
            {
                if (b)
                {
                    int rnd = Random.Range(0, probaContreLesMures);
                    int prb = 0;
                    foreach (obj o in ObjetsContreLesMures)
                    {
                        if (s.Room_Info[o.ID] > 0)
                        {
                            int p = (probaContreLesMures / s.Room_Info[o.ID]);
                            if ((rnd >= prb) &&
                                (rnd < prb + p))
                            {
                                List<float> go = new List<float>();
                               
                                    foreach (float flo in o.Info[x])
                                    {
                                        go.Add(flo);
                                    }
                                go.Add(o.ID);
                                s.décore.Add(go);
                                max[o.ID] += 1;
                                if (max[o.ID] == s.Room_InfoMax[o.ID])
                                {
                                    s.Room_Info[o.ID] = -2;
                                }

                               
                                    break;
                                
                            }

                            prb += p;
                        }
                    }
                }

                x += 1;
            }

            x = 0;
            foreach (bool b in s.PlaceAuCentre)
            {
                if (b)
                {
                    int rnd = Random.Range(0, probaAuCentre);
                    int prb = 0;
                    foreach (obj o in ObjetsAuCentreDeLaSalle)
                    {
                        if (s.Room_Info[o.ID] > 0)
                        {
                            int p = (probaAuCentre / s.Room_Info[o.ID]);
                            if ((rnd >= prb) &&
                                (rnd < prb + p))
                            {
                                List<float> go = new List<float>();
                                if ((o.ID==0)||(o.ID==8))
                                {
                                    foreach (float flo in o.Info[x/2])
                                    {
                                        go.Add(flo);
                                    }  
                                }
                                else
                                {
                                    foreach (float flo in o.Info[x])
                                    {
                                        go.Add(flo);
                                    }
                                }

                                go.Add(o.ID);
                                s.décore.Add(go);
                                max[o.ID] += 1;
                                if (max[o.ID] == s.Room_InfoMax[o.ID])
                                {
                                    s.Room_Info[o.ID] = -2;
                                }

                                if (o.ID != 4)
                                {
                                    break;
                                }
                            }

                            prb += p;
                        }
                    }
                }

                x += 1;
            }

            int y = 0;
            foreach (int i in saveinfo)
            {
                s.Room_Info[y] = i;
                y += 1;
            }
        }

    }

    public List<salle> CreateMaze(int DébutX, int DébutY, int hauteur, List<int> info, List<int> infomax,
        int SalleMin, int SalleMax, List<salle> salles,
        string type,
        int Proba_alt, int Proba_secret, int Proba_break, int AltSallaMax, int AltProba_break,
        int Proba_bigRoom, bool shop)
    {
        rnd = new System.Random();
        salles = B(DébutX, DébutY, hauteur, info, infomax,
            SalleMin, SalleMax, type, salles, Proba_secret, Proba_break);
        List<salle> cheminprincipale = new List<salle>();
        foreach (salle s in salles)
        {
            cheminprincipale.Add(s);
        }

        foreach (salle s in cheminprincipale)
        {
            if (!s.ways.Contains("alt"))
            {
                int rnd = Random.Range(0, Proba_alt);
                if (rnd == 0)
                {
                    salles = B(s.X, s.Y, s.HAUTEUR, info, infomax, 0, AltSallaMax, "alt", salles, Proba_secret,
                        AltProba_break);
                }
            }
        }

        if (shop)
        {
            Shop(salles);
        }

        isal(salles, Proba_bigRoom);

        return salles;
    }

    public List<obj> init_obj()
    {
        obj table = new obj(false, new List<List<float>>
        {
            new List<float> {-2, 2, 0f, Random.Range(0,180)},
            new List<float> {2, 2, 0f, Random.Range(0,180)},
            new List<float> {-2, -2, 0f, Random.Range(0,180)},
            new List<float> {2, -2, 0f, Random.Range(0,180)}
        }, 8);
        obj tableCassé = new obj(false, new List<List<float>>
        {
            new List<float> {-2, 2, 0f, Random.Range(0,180)},
            new List<float> {2, 2, 0f, Random.Range(0,180)},
            new List<float> {-2, -2, 0f, Random.Range(0,180)},
            new List<float> {2, -2, 0f, Random.Range(0,180)}
        }, 0);
        obj Os = new obj(false, new List<List<float>>
        {
            new List<float> {-3, 3, 0.1f, 0},
            new List<float> {0, 3, 0.1f, 0},
            new List<float> {3, 3, 0.1f, 0},
            new List<float> {-3, 0, 0.1f, 0},
            new List<float> {0, 0, 0.1f, 0},
            new List<float> {3, 0, 0.1f, 0},
            new List<float> {-3, -3, 0.1f, 0},
            new List<float> {0, -3, 0.1f, 0},
            new List<float> {3, -3, 0.1f, 0}
        }, 3);
        
        obj tapis = new obj(false, new List<List<float>>
        {
            new List<float> {-3, 3, 0f, 0},
            new List<float> {0, 3, 0f, 0},
            new List<float> {3, 3, 0f, 0},
            new List<float> {-3, 0, 0f, 0},
            new List<float> {0, 0, 0f, 0},
            new List<float> {3, 0, 0f, 0},
            new List<float> {-3, -3, 0f, 0},
            new List<float> {0, -3, 0f, 0},
            new List<float> {3, -3, 0f, 0}
        }, 4);
        obj coffre = new obj(true, new List<List<float>>
        {
            new List<float>() {-6.5f, -3.5f, 0.4f, 180},
            new List<float>() {-6.5f, 3.5f, 0.4f, 180},
            new List<float>() {-3.5f, 6.5f, 0.4f, -90},
            new List<float>() {3.5f, 6.5f, 0.4f, -90},
            new List<float>() {6.5f, 3.5f, 0.4f, 0},
            new List<float>() {6.5f, -3.5f, 0.4f, 0}
        }, 5);
        obj coffreDeFer = new obj(true, new List<List<float>>
        {
            new List<float>() {-6.5f, -3.5f, 0.4f, 180},
            new List<float>() {-6.5f, 3.5f, 0.4f, 180},
            new List<float>() {-3.5f, 6.5f, 0.4f, -90},
            new List<float>() {3.5f, 6.5f, 0.4f, -90},
            new List<float>() {6.5f, 3.5f, 0.4f, 0},
            new List<float>() {6.5f, -3.5f, 0.4f, 0}
        }, 6);
        obj Zombie = new obj(false, new List<List<float>>
        {
            new List<float> {-3, 3, 0.1f, 0},
            new List<float> {0, 3, 0.1f, 0},
            new List<float> {3, 3, 0.1f, 0},
            new List<float> {-3, 0, 0.1f, 0},
            new List<float> {0, 0, 0.1f, 0},
            new List<float> {3, 0, 0.1f, 0},
            new List<float> {-3, -3, 0.1f, 0},
            new List<float> {0, -3, 0.1f, 0},
            new List<float> {3, -3, 0.1f, 0}
        }, 2);
        obj tableaux = new obj(true, new List<List<float>>
        {
            new List<float>() {-7.2f, -3.5f, 2.4f, 90},
            new List<float>() {-7.2f, 3.5f, 2.4f, 90},
            new List<float>() {-3.5f, 7.2f, 2.4f, -180},
            new List<float>() {3.5f, 7.2f, 2.4f, -180},
            new List<float>() {7.2f, 3.5f, 2.4f, -90},
            new List<float>() {7.2f, -3.5f, 2.4f, -90}
        }, 7);
        List<obj> Objets = new List<obj> {tableCassé,coffre, tableaux, Zombie, table,tapis,coffreDeFer,Os};
        return Objets;
    }




    void Start()
    {
        //décore list : 0=tableCassé, 1=torche, 2=Zombie, 3=Os, 4=Tapis, 5=coffre, 6=CoffreDeFer, 7=Tableaux, 8=Table
        //{X,Y,H,Info,Min,Max,Salles,Type,ProbaAlt,ProbaSecret,ProbaBreak,AltMax,ProbaAltBreak,ProbaBigRoom,Shop}
        List<salle> salles = CreateMaze(0, 0, 0, new List<int> {25,-1,10,15,20,10,35,18,24}, new List<int> {1,0,3,4,1,1,1,2,1}, 20, 30,
            new List<salle>(), "debut", 5, 4, 20, 10, 7, 6, true);
        List<salle> secret_floor = (SearchSecret(salles, new List<int> {25,-1,5,10,20,5,15,18,24}, new List<int> {1,0,4,4,1,2,1,2,1}, 4, 8, 2, 2, 3,
            2));
        it(salles);
        //List<obj> Objets = init_obj();
        //init_décor(salles, Objets);
        Creation(salles, true);
        it(secret_floor);
        //init_décor(secret_floor, Objets);
        Creation(secret_floor, false);
    }

}

