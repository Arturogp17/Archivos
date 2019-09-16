using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ProyectoArchivos.MainApp.Create
{
    public partial class frmCreateEntity : Telerik.WinControls.UI.RadForm
    {
        public string name;
        public frmCreateEntity(string id)
        {
            InitializeComponent();
            lblID.Text = id;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            name = txtName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
