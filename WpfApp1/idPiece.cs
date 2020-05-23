using System.Data;

namespace DetControlTower
{
    public interface idPiece
    {
        DataTable DataTable { get; set; }

        void FillDataGrid();
        void InitializeComponent();
    }
}