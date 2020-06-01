using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy;
using DAO;
using System.Collections.ObjectModel;

namespace Server
{
    public class MachineManagerService : MarshalByRefObject, IMachineManagerService
    {
        private DataAccessObject dao = new DataAccessObject();
        private String message;

        public string Message { get => message; set => message = value; }

        public string getMessage() { return this.Message; }


        public bool AddMachine(Machine machine)
        {
            Boolean added = dao.AddMachine(machine);
            this.Message = dao.Message;

            return added;
        }

        public DataTable GetMachineList(Machine filter, Boolean availableOnly)
        {
            return dao.GetMachineList(filter, availableOnly);
        }

        public DataTable GetPieceList(Piece filter)
        {
            return dao.GetPieceList(filter);
        }

        public ObservableCollection<String> ProviderList()
        {
           return dao.ProviderList();
        }
        public bool EditMachine(Machine machine)
        {
            Boolean edited = dao.EditMachine(machine);
            this.Message = dao.Message;

            return edited;
        }

        public bool RemovePiece(Piece piece)
        {
            Boolean removed = dao.RemovePiece(piece);
            this.Message = dao.Message;

            return removed;
        }

        public bool RemoveMachine(Machine machine)
        {
            Boolean removed = dao.RemoveMachine(machine);
            this.Message = dao.Message;

            return removed;
        }

        public bool FacturerTache(Tache tache)
        {
            Boolean edited = dao.FacturerTache(tache);
            this.Message = dao.Message;

            return edited;
        }

        public bool UpdateMachine(Machine machine)
        {
            Boolean edited = dao.UpdateMachine(machine);
            this.Message = dao.Message;

            return edited;
        }

        public ObservableCollection<string> PieceList()
        {
            return dao.TacheList();
        }

        public bool AddPiece(Piece piece)
        {
            Boolean added = dao.AddPiece(piece);
            this.Message = dao.Message;

            return added;
        }

        public bool AddTache(Tache tache)
        {
            Boolean added = dao.AddTache(tache);
            this.Message = dao.Message;

            return added;
        }

        public ObservableCollection<string> TacheList()
        {
            return dao.TacheList();
        }

        public DataTable GetTacheList(Tache filter)
        {
            return dao.GetTacheList(filter);
        }
    }
}
