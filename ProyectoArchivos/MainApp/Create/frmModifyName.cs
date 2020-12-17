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
    public partial class frmModifyName : Telerik.WinControls.UI.RadForm
    {
        public string name;
        public frmModifyName()
        {
            InitializeComponent();
            
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            name = txtName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmModifyName_Load(object sender, EventArgs e)
        {
            txtName.Text = name;
            
        }
    }
}
