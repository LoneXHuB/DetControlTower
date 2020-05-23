using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Emprunteur
    {
        protected String nom;
        protected String prenom;
        protected String email;
        protected String pseudo;
        protected String pass;
        protected int passrx; //le nombre de fois qu'il a reserver un livre mais il n'a pas passé le prendre
        protected Boolean bloque;//dans la base j'ai mis le type int 

        public Emprunteur(String n,String p,String e,String pseu, String pa)
        {
            Nom = n;
            Prenom = p;
            Email = e;
            Pseudo = pseu;
            Pass = pa;
            Passrx = 0;
            Bloque = false;
        }

        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Email { get => email; set => email = value; }
        public string Pseudo { get => pseudo; set => pseudo = value; }
        public string Pass { get => pass; set => pass = value; }
        public int Passrx { get => passrx; set => passrx = value; }
        public bool Bloque { get => bloque; set => bloque = value; }
    }
}
