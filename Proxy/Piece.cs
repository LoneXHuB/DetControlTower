using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Piece
    {
        private String refference;
        private String description;
        private DateTime dateAjout;
        private Int32 id;

        public Int32 Id { set; get; }
        public String Refference { set; get; }
        public String Description { set; get; }
        public DateTime DateAjout { set; get; }

        public Piece(int id, string reference)
        {
            this.Id = id;
            this.Refference = reference;
        }

        public Piece (String refference , String description)
        {
            this.Refference = refference;
            this.Description = description;
        }

        public Piece(string refference, string description, DateTime dateAjout) : this(refference, description)
        {
            this.DateAjout = dateAjout;
        }
    }
}
