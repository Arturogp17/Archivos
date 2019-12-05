using ProyectoArchivos.MainApp.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ProyectoArchivos.MainApp.Index
{
    public partial class frmPrimaryKey : Telerik.WinControls.UI.RadForm
    {
        public frmPrimaryKey(List<Register> lpk)
        {
            InitializeComponent();
            gridPK.DataSource = lpk;
        }

        private void frmPrimaryKey_Load(object sender, EventArgs e)
        {

        }
    }
}
