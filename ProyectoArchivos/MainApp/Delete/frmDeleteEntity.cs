using ProyectoArchivos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ProyectoArchivos.MainApp.Delete
{
    public partial class frmDeleteEntity : Telerik.WinControls.UI.RadForm
    {
        List<Entity> entities = new List<Entity>();
        public frmDeleteEntity(List<Entity> en)
        {
            InitializeComponent();
            entities = en;
            ddlEntities.DataSource = entities;
        }

        private void btnDetele_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Esta seguro que de sea eliminar esta Entidad?", "Estructura de archivos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ddlEntities_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            foreach(Entity en in entities)
            {
                if(en.name == ddlEntities.SelectedText)
                {
                    gridAttributes.DataSource = en.attributes;
                    lblID.Text = en.id;
                    break;
                }
            }
        }
    }
}
