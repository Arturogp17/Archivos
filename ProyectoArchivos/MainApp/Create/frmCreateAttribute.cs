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
        public string idEntityFK;
        public string idAttrFK;
        public string idEntDestFK;
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
            switch(a.indexType)
            {
                case 0:
                    ddlIndexType.SelectedItem = (RadListDataItem)ddlIndexType.Items[0];
                    break;
                case 2:
                    ddlIndexType.SelectedItem = (RadListDataItem)ddlIndexType.Items[1];
                    break;
                case 3:
                    ddlIndexType.SelectedItem = (RadListDataItem)ddlIndexType.Items[2];
                    break;
                case 5:
                    ddlIndexType.SelectedItem = (RadListDataItem)ddlIndexType.Items[3];
                    break;
            }
            switch(a.dataType)
            {
                case 'C':
                    ddlDataType.SelectedItem = (RadListDataItem)ddlDataType.Items[0];
                    ntxtLength.Enabled = true;
                    break;
                case 'E':
                    ddlDataType.SelectedItem = (RadListDataItem)ddlDataType.Items[1];
                    ntxtLength.Enabled = false;
                    break;
                case 'D':
                    ddlDataType.SelectedItem = (RadListDataItem)ddlDataType.Items[2];
                    ntxtLength.Enabled = false;
                    break;
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
                if (indexType == "3")
                {
                    idEntityFK = ddlEntidad.SelectedValue.ToString();
                    idAttrFK = lblIdD.Text;
                    if (ddlTablaDestino.SelectedValue == null)
                    {
                        MessageBox.Show("Selecciona una tabla destino para la llave foranea", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        idEntDestFK = ddlTablaDestino.SelectedValue.ToString();
                    }
                }
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
            switch (ddlDataType.SelectedItem.Text)
            {
                case "Cadena":
                    ntxtLength.Enabled = true;
                    break;
                case "Entero":
                    ntxtLength.Value = 4;
                    ntxtLength.Enabled = false;
                    break;
                case "Decimal":
                    ntxtLength.Value = 4;
                    ntxtLength.Enabled = false;
                    break;
            }
        }

        private bool existe(string name)
        {
            foreach (Entity e in entities)
            {
                if (e.id == ddlEntidad.SelectedValue.ToString())
                {
                    foreach (Attributes a in e.attributes)
                    {
                        if (a.name == name)
                            return true;
                    }
                }
            }
            return false;
        }

        private void ddlIndexType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            List<Entity> selEntities = new List<Entity>();
            if(ddlIndexType.SelectedItem != null)
            {
                if(ddlIndexType.SelectedItem.Text.Contains("3"))
                {
                    ddlTablaDestino.Enabled = true;
                    foreach (var en in entities)
                    {
                        if (en.id != idEntity)
                        {
                            foreach (var a in en.attributes)
                            {
                                if (a.indexType == 2)
                                    selEntities.Add(en);
                            }
                        }
                    }
                }
            }
            ddlTablaDestino.DataSource = selEntities;
        }

        private void ddlTablaDestino_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            string text = string.Empty;
            if(ddlTablaDestino.SelectedValue != null)
            {
                foreach (var en in entities)
                {
                    if(ddlTablaDestino.SelectedValue.ToString() == en.id)
                    {
                        foreach (var a in en.attributes)
                        {
                            if (a.indexType == 2)
                                text = a.name;
                        }
                    }
                }
            }
            txtAttDestino.Text = text;
        }

        private void frmCreateAttribute_Load(object sender, EventArgs e)
        {
            ddlEntidad.SelectedValue = idEntity;
            ddlEntidad.Enabled = false;
        }
    }
}
