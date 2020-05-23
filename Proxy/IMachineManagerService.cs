using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace Proxy
{
    public interface IMachineManagerService
    {
        String getMessage();
        Boolean RemoveMachine(Machine machine);
        Boolean  AddMachine(Machine machine);
        DataTable GetMachineList(Machine filter, Boolean availableOnly);
        Boolean EditMachine(Machine machine);
        ObservableCollection<String> ProviderList();
        ObservableCollection<String> PieceList();
        Boolean UpdateMachine(Machine machine );
        bool AddPiece(Piece piece);
        DataTable GetPieceList(Piece filter);
        bool RemovePiece(Piece piece);
        ObservableCollection<string> TacheList();
        bool AddTache(Tache tache);
    }
}
