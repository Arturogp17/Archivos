using ProyectoArchivos.MainApp.Classes;
using ProyectoArchivos.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProyectoArchivos.MainApp.Create
{
    public partial class frmCreateAttribute : Telerik.WinControls.UI.RadForm
    {
        public List<Entity> entities = new List<Entity>();
        public string idEntity;
        public string name;
        public char dataType;
        public string indexType;
        public int id = 0;
        public int length;

        public frmCreateAttribute(List<Entity> e, int id)
        {
            InitializeComponent();
            this.id = id;
            entities = e;
            ddlEntidad.DataSource = entities;
            lblIdD.Text = Convert.ToString(String.Format("{0:00000}", id));
        }

        public frmCreateAttribute(string entity, Attributes a)
        {
            InitializeComponent();
            ddlEntidad.Text = entity;
            ddlEntidad.Enabled = false;
            lblIdD.Text = a.id;
            txtName.Text = a.name;
            ddlIndexType.SelectedItem = (RadListDataItem)ddlIndexType.Items[a.indexType];
            if (a.dataType == 'C')
            {
                ddlDataType.SelectedItem = (RadListDataItem)ddlDataType.Items[0];
                ntxtLength.Enabled = true;
            }
            else
            {
                ddlDataType.SelectedItem = (RadListDataItem)ddlDataType.Items[1];
                ntxtLength.Enabled = false;
            }
            ntxtLength.Value = a.length;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!existe(txtName.Text))
            {
                name = txtName.Text;
                idEntity = Convert.ToString(ddlEntidad.SelectedValue);
                dataType = ddlDataType.SelectedItem.Text[0];
                indexType = ddlIndexType.SelectedItem.Text[0].ToString();
                length = Convert.ToInt32(ntxtLength.Value);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("No se puede repetir el nombre de un atributo", "Diccionario de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ddlDataType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if(ddlDataType.SelectedItem.Text == "Cadena")
            {
                ntxtLength.Enabled = true;
            }
            else
            {
                ntxtLength.Value = 4;
                ntxtLength.Enabled = false;
            }
        }

        private bool existe(string name)
        {
            foreach (Entity e in entities)
            {
                foreach (Attributes a in e.attributes)
                {
                    if (a.name == name)
                        return true;
                }
            }
            return false;
        }
    }
}
