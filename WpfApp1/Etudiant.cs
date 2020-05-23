using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Etudiant : Emprunteur
    {
        private int numC;
        private String specialite;
        private String niveau ;

        public Etudiant(String n, String p, String e, String pseu, String pa, int num, String s, String ni)
            :base(n,p,e,pseu,pa)//super()
        {
            NumC = num;
            Specialite = s;
            Niveau = ni;

        }

        public int NumC { get => numC; set => numC = value; }
        public string Specialite { get => specialite; set => specialite = value; }
        public string Niveau { get => niveau; set => niveau = value; }
    }
}
