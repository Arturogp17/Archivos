﻿using ProyectoArchivos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoArchivos.MainApp.Delete
{
    public partial class frmDeleteAttribute : Form
    {
        List<Attributes> at = new List<Attributes>();
        public string entity = string.Empty;
        public string attr = string.Empty;
        public frmDeleteAttribute(List<Attributes> la)
        {
            InitializeComponent();
            at = la;
        }

        private void frmDeleteAttribute_Load(object sender, EventArgs e)
        {
            ddlAttributes.DataSource = at;
            lblEntity.Text = entity;
            ddlAttributes.SelectedValue = attr;
            ddlAttributes.Enabled = false;
        }

        private void ddlAttributes_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            int position = e.Position;
            if (at[position].dataType == 'C')
                lblDataType.Text = "Cadena";
            else
                lblDataType.Text = "Entero";
            switch(at[position].indexType)
            {
                case 0:
                    lblIndexType.Text = "0 - Sin tipo";
                    break;
                case 2:
                    lblIndexType.Text = "2 - LLave primaria";
                    break;
                case 3:
                    lblIndexType.Text = "3 - Llave foranea";
                    break;
            }
            lblLenght.Text = at[position].length.ToString();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
