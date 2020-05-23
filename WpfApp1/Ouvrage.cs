using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Ouvrage
    {
        private String titre;
        private String auteur;
        private String theme;
        private String type;//livre these memoire
        private String barcode;
        private String dispo;//libre reservé pris

        public Ouvrage(string titre, string auteur, string theme, string type, string barcode, string dispo)
        {
            this.Titre = titre;
            this.Auteur = auteur;
            this.Theme = theme;
            this.Type = type;
            this.Barcode = barcode;
            this.Dispo = dispo;
        }

        public string Titre { get => titre; set => titre = value; }
        public string Auteur { get => auteur; set => auteur = value; }
        public string Theme { get => theme; set => theme = value; }
        public string Type { get => type; set => type = value; }
        public string Barcode { get => barcode; set => barcode = value; }
        public string Dispo { get => dispo; set => dispo = value; }
    }
}
