using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Enseignant : Emprunteur
    {
        private int mat;
        private string grade;

        public Enseignant(String n, String p, String e, String pseu, String pa, int m, String g)
            : base(n, p, e, pseu, pa)
        {
            Mat = m;
            Grade = g;
        }

        public int Mat { get => mat; set => mat = value; }
        public string Grade { get => grade; set => grade = value; }
    }
}
