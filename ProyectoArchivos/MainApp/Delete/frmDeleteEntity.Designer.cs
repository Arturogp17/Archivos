namespace ProyectoArchivos.MainApp.Delete
{
    partial class frmDeleteEntity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn25 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn26 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn27 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn28 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn29 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn30 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn31 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn32 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.lblID = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.gridAttributes = new Telerik.WinControls.UI.RadGridView();
            this.btnDetele = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.ddlEntities = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDetele)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(33, 10);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(17, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "ID";
            this.radLabel1.ThemeName = "VisualStudio2012Light";
            // 
            // lblID
            // 
            this.lblID.AutoSize = false;
            this.lblID.BackColor = System.Drawing.Color.White;
            this.lblID.Location = new System.Drawing.Point(74, 10);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(154, 18);
            this.lblID.TabIndex = 1;
            this.lblID.ThemeName = "VisualStudio2012Light";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(252, 10);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(48, 18);
            this.radLabel4.TabIndex = 2;
            this.radLabel4.Text = "Nombre";
            this.radLabel4.ThemeName = "VisualStudio2012Light";
            // 
            // gridAttributes
            // 
            this.gridAttributes.AutoScroll = true;
            this.gridAttributes.Location = new System.Drawing.Point(8, 37);
            // 
            // 
            // 
            this.gridAttributes.MasterTemplate.AllowAddNewRow = false;
            this.gridAttributes.MasterTemplate.AllowColumnReorder = false;
            this.gridAttributes.MasterTemplate.AllowDeleteRow = false;
            gridViewTextBoxColumn25.FieldName = "id";
            gridViewTextBoxColumn25.HeaderText = "ID";
            gridViewTextBoxColumn25.Name = "id";
            gridViewTextBoxColumn25.ReadOnly = true;
            gridViewTextBoxColumn26.FieldName = "name";
            gridViewTextBoxColumn26.HeaderText = "Nombre";
            gridViewTextBoxColumn26.Name = "name";
            gridViewTextBoxColumn26.ReadOnly = true;
            gridViewTextBoxColumn26.Width = 150;
            gridViewTextBoxColumn27.FieldName = "dataType";
            gridViewTextBoxColumn27.HeaderText = "Tipo";
            gridViewTextBoxColumn27.Name = "dataType";
            gridViewTextBoxColumn27.ReadOnly = true;
            gridViewTextBoxColumn27.Width = 82;
            gridViewTextBoxColumn28.DataType = typeof(int);
            gridViewTextBoxColumn28.FieldName = "length";
            gridViewTextBoxColumn28.HeaderText = "Longitud";
            gridViewTextBoxColumn28.Name = "length";
            gridViewTextBoxColumn28.ReadOnly = true;
            gridViewTextBoxColumn28.Width = 82;
            gridViewTextBoxColumn29.DataType = typeof(long);
            gridViewTextBoxColumn29.FieldName = "address";
            gridViewTextBoxColumn29.HeaderText = "Dirección";
            gridViewTextBoxColumn29.Name = "address";
            gridViewTextBoxColumn29.ReadOnly = true;
            gridViewTextBoxColumn29.Width = 82;
            gridViewTextBoxColumn30.FieldName = "indexType";
            gridViewTextBoxColumn30.HeaderText = "Tipo índice";
            gridViewTextBoxColumn30.Name = "indexType";
            gridViewTextBoxColumn30.ReadOnly = true;
            gridViewTextBoxColumn30.Width = 82;
            gridViewTextBoxColumn31.DataType = typeof(long);
            gridViewTextBoxColumn31.FieldName = "indexAddress";
            gridViewTextBoxColumn31.HeaderText = "Dir. indice";
            gridViewTextBoxColumn31.Name = "indexAddress";
            gridViewTextBoxColumn31.ReadOnly = true;
            gridViewTextBoxColumn31.Width = 82;
            gridViewTextBoxColumn32.DataType = typeof(long);
            gridViewTextBoxColumn32.FieldName = "nextAttributeAddress";
            gridViewTextBoxColumn32.HeaderText = "Sig. atributo";
            gridViewTextBoxColumn32.Name = "nextAttributeAddress";
            gridViewTextBoxColumn32.ReadOnly = true;
            gridViewTextBoxColumn32.Width = 82;
            this.gridAttributes.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn25,
            gridViewTextBoxColumn26,
            gridViewTextBoxColumn27,
            gridViewTextBoxColumn28,
            gridViewTextBoxColumn29,
            gridViewTextBoxColumn30,
            gridViewTextBoxColumn31,
            gridViewTextBoxColumn32});
            this.gridAttributes.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.gridAttributes.Name = "gridAttributes";
            this.gridAttributes.ReadOnly = true;
            this.gridAttributes.ShowGroupPanel = false;
            this.gridAttributes.Size = new System.Drawing.Size(712, 253);
            this.gridAttributes.TabIndex = 9;
            this.gridAttributes.ThemeName = "VisualStudio2012Light";
            // 
            // btnDetele
            // 
            this.btnDetele.Location = new System.Drawing.Point(494, 296);
            this.btnDetele.Name = "btnDetele";
            this.btnDetele.Size = new System.Drawing.Size(110, 24);
            this.btnDetele.TabIndex = 10;
            this.btnDetele.Text = "Eliminar";
            this.btnDetele.ThemeName = "VisualStudio2012Light";
            this.btnDetele.Click += new System.EventHandler(this.btnDetele_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(610, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.ThemeName = "VisualStudio2012Light";
            // 
            // ddlEntities
            // 
            this.ddlEntities.DisplayMember = "name";
            this.ddlEntities.Location = new System.Drawing.Point(306, 7);
            this.ddlEntities.Name = "ddlEntities";
            this.ddlEntities.Size = new System.Drawing.Size(154, 24);
            this.ddlEntities.TabIndex = 12;
            this.ddlEntities.ThemeName = "VisualStudio2012Light";
            this.ddlEntities.ValueMember = "id";
            this.ddlEntities.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlEntities_SelectedIndexChanged);
            // 
            // frmDeleteEntity
            // 
            this.AcceptButton = this.btnDetele;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(732, 327);
            this.Controls.Add(this.ddlEntities);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDetele);
            this.Controls.Add(this.gridAttributes);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.radLabel1);
            this.Name = "frmDeleteEntity";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmDeleteEntity";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDetele)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadGridView gridAttributes;
        private Telerik.WinControls.UI.RadButton btnDetele;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadDropDownList ddlEntities;
        public Telerik.WinControls.UI.RadLabel lblID;
    }
}
