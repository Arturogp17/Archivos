namespace ProyectoArchivos
{
    partial class frmStart
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn13 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn14 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.radContextMenu1 = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.newFile = new Telerik.WinControls.UI.RadMenuItem();
            this.open = new Telerik.WinControls.UI.RadMenuItem();
            this.Save = new Telerik.WinControls.UI.RadMenuItem();
            this.btnCreateEntity = new Telerik.WinControls.UI.RadButton();
            this.btnCreateAttribute = new Telerik.WinControls.UI.RadButton();
            this.btnDeleteAtribute = new Telerik.WinControls.UI.RadButton();
            this.btnDeleteEntity = new Telerik.WinControls.UI.RadButton();
            this.gridEntities = new Telerik.WinControls.UI.RadGridView();
            this.gridAttributes = new Telerik.WinControls.UI.RadGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.genericItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateEntity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateAttribute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteAtribute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteEntity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes.MasterTemplate)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.genericItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // radMenu1
            // 
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(733, 20);
            this.radMenu1.TabIndex = 0;
            this.radMenu1.ThemeName = "VisualStudio2012Light";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.newFile,
            this.open,
            this.Save});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "Archivo";
            // 
            // newFile
            // 
            this.newFile.AccessibleDescription = "newFile";
            this.newFile.AccessibleName = "newFile";
            this.newFile.Name = "newFile";
            this.newFile.Text = "Nuevo";
            this.newFile.Click += new System.EventHandler(this.newFile_Click);
            // 
            // open
            // 
            this.open.AccessibleDescription = "open";
            this.open.AccessibleName = "open";
            this.open.Name = "open";
            this.open.Text = "Abrir";
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Text = "Guardar";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // btnCreateEntity
            // 
            this.btnCreateEntity.Location = new System.Drawing.Point(6, 21);
            this.btnCreateEntity.Name = "btnCreateEntity";
            this.btnCreateEntity.Size = new System.Drawing.Size(110, 38);
            this.btnCreateEntity.TabIndex = 1;
            this.btnCreateEntity.Text = "Crear entidad";
            this.btnCreateEntity.ThemeName = "VisualStudio2012Light";
            this.btnCreateEntity.Click += new System.EventHandler(this.btnCreateEntity_Click);
            // 
            // btnCreateAttribute
            // 
            this.btnCreateAttribute.Enabled = false;
            this.btnCreateAttribute.Location = new System.Drawing.Point(6, 21);
            this.btnCreateAttribute.Name = "btnCreateAttribute";
            this.btnCreateAttribute.Size = new System.Drawing.Size(110, 38);
            this.btnCreateAttribute.TabIndex = 2;
            this.btnCreateAttribute.Text = "Crear atributo";
            this.btnCreateAttribute.ThemeName = "VisualStudio2012Light";
            this.btnCreateAttribute.Click += new System.EventHandler(this.btnCreateAttribute_Click);
            // 
            // btnDeleteAtribute
            // 
            this.btnDeleteAtribute.Enabled = false;
            this.btnDeleteAtribute.Location = new System.Drawing.Point(6, 65);
            this.btnDeleteAtribute.Name = "btnDeleteAtribute";
            this.btnDeleteAtribute.Size = new System.Drawing.Size(110, 38);
            this.btnDeleteAtribute.TabIndex = 4;
            this.btnDeleteAtribute.Text = "Eliminar atributo";
            this.btnDeleteAtribute.ThemeName = "VisualStudio2012Light";
            // 
            // btnDeleteEntity
            // 
            this.btnDeleteEntity.Enabled = false;
            this.btnDeleteEntity.Location = new System.Drawing.Point(6, 65);
            this.btnDeleteEntity.Name = "btnDeleteEntity";
            this.btnDeleteEntity.Size = new System.Drawing.Size(110, 38);
            this.btnDeleteEntity.TabIndex = 3;
            this.btnDeleteEntity.Text = "Eliminar entidad";
            this.btnDeleteEntity.ThemeName = "VisualStudio2012Light";
            this.btnDeleteEntity.Click += new System.EventHandler(this.btnDeleteEntity_Click);
            // 
            // gridEntities
            // 
            this.gridEntities.AutoScroll = true;
            this.gridEntities.Location = new System.Drawing.Point(12, 170);
            // 
            // 
            // 
            this.gridEntities.MasterTemplate.AllowAddNewRow = false;
            this.gridEntities.MasterTemplate.AllowColumnReorder = false;
            this.gridEntities.MasterTemplate.AllowDragToGroup = false;
            this.gridEntities.MasterTemplate.AllowRowResize = false;
            gridViewTextBoxColumn1.FieldName = "id";
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.Name = "id";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn2.FieldName = "name";
            gridViewTextBoxColumn2.HeaderText = "Nombre";
            gridViewTextBoxColumn2.Name = "name";
            gridViewTextBoxColumn2.Width = 200;
            gridViewTextBoxColumn3.DataType = typeof(long);
            gridViewTextBoxColumn3.FieldName = "address";
            gridViewTextBoxColumn3.HeaderText = "Dirección";
            gridViewTextBoxColumn3.Name = "address";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 85;
            gridViewTextBoxColumn4.DataType = typeof(long);
            gridViewTextBoxColumn4.FieldName = "attributeAddress";
            gridViewTextBoxColumn4.HeaderText = "Dir. atributo";
            gridViewTextBoxColumn4.Name = "attributeAddress";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.Width = 85;
            gridViewTextBoxColumn5.DataType = typeof(long);
            gridViewTextBoxColumn5.FieldName = "dataAddress";
            gridViewTextBoxColumn5.HeaderText = "Dir. datos";
            gridViewTextBoxColumn5.Name = "dataAddress";
            gridViewTextBoxColumn5.ReadOnly = true;
            gridViewTextBoxColumn5.Width = 85;
            gridViewTextBoxColumn6.DataType = typeof(long);
            gridViewTextBoxColumn6.FieldName = "nextEntityAddress";
            gridViewTextBoxColumn6.HeaderText = "Sig. entidad";
            gridViewTextBoxColumn6.Name = "nextEntityAddress";
            gridViewTextBoxColumn6.ReadOnly = true;
            gridViewTextBoxColumn6.Width = 85;
            this.gridEntities.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6});
            this.gridEntities.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridEntities.Name = "gridEntities";
            this.gridEntities.ShowGroupPanel = false;
            this.gridEntities.Size = new System.Drawing.Size(607, 253);
            this.gridEntities.TabIndex = 7;
            this.gridEntities.ThemeName = "VisualStudio2012Light";
            this.gridEntities.CellEndEdit += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridEntities_CellEndEdit);
            // 
            // gridAttributes
            // 
            this.gridAttributes.AutoScroll = true;
            this.gridAttributes.Location = new System.Drawing.Point(9, 458);
            // 
            // 
            // 
            this.gridAttributes.MasterTemplate.AllowAddNewRow = false;
            this.gridAttributes.MasterTemplate.AllowColumnReorder = false;
            this.gridAttributes.MasterTemplate.AllowDeleteRow = false;
            gridViewTextBoxColumn7.FieldName = "id";
            gridViewTextBoxColumn7.HeaderText = "ID";
            gridViewTextBoxColumn7.Name = "id";
            gridViewTextBoxColumn7.ReadOnly = true;
            gridViewTextBoxColumn8.FieldName = "name";
            gridViewTextBoxColumn8.HeaderText = "Nombre";
            gridViewTextBoxColumn8.Name = "name";
            gridViewTextBoxColumn8.ReadOnly = true;
            gridViewTextBoxColumn8.Width = 150;
            gridViewTextBoxColumn9.FieldName = "dataType";
            gridViewTextBoxColumn9.HeaderText = "Tipo";
            gridViewTextBoxColumn9.Name = "dataType";
            gridViewTextBoxColumn9.ReadOnly = true;
            gridViewTextBoxColumn9.Width = 82;
            gridViewTextBoxColumn10.DataType = typeof(int);
            gridViewTextBoxColumn10.FieldName = "length";
            gridViewTextBoxColumn10.HeaderText = "Longitud";
            gridViewTextBoxColumn10.Name = "length";
            gridViewTextBoxColumn10.ReadOnly = true;
            gridViewTextBoxColumn10.Width = 82;
            gridViewTextBoxColumn11.DataType = typeof(long);
            gridViewTextBoxColumn11.FieldName = "address";
            gridViewTextBoxColumn11.HeaderText = "Dirección";
            gridViewTextBoxColumn11.Name = "address";
            gridViewTextBoxColumn11.ReadOnly = true;
            gridViewTextBoxColumn11.Width = 82;
            gridViewTextBoxColumn12.FieldName = "indexType";
            gridViewTextBoxColumn12.HeaderText = "Tipo índice";
            gridViewTextBoxColumn12.Name = "indexType";
            gridViewTextBoxColumn12.ReadOnly = true;
            gridViewTextBoxColumn12.Width = 82;
            gridViewTextBoxColumn13.DataType = typeof(long);
            gridViewTextBoxColumn13.FieldName = "indexAddress";
            gridViewTextBoxColumn13.HeaderText = "Dir. indice";
            gridViewTextBoxColumn13.Name = "indexAddress";
            gridViewTextBoxColumn13.ReadOnly = true;
            gridViewTextBoxColumn13.Width = 82;
            gridViewTextBoxColumn14.DataType = typeof(long);
            gridViewTextBoxColumn14.FieldName = "nextAttributeAddress";
            gridViewTextBoxColumn14.HeaderText = "Sig. atributo";
            gridViewTextBoxColumn14.Name = "nextAttributeAddress";
            gridViewTextBoxColumn14.ReadOnly = true;
            gridViewTextBoxColumn14.Width = 82;
            this.gridAttributes.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12,
            gridViewTextBoxColumn13,
            gridViewTextBoxColumn14});
            this.gridAttributes.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.gridAttributes.Name = "gridAttributes";
            this.gridAttributes.ShowGroupPanel = false;
            this.gridAttributes.Size = new System.Drawing.Size(712, 253);
            this.gridAttributes.TabIndex = 8;
            this.gridAttributes.ThemeName = "VisualStudio2012Light";
            this.gridAttributes.CellEndEdit += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridAttributes_CellEndEdit);
            this.gridAttributes.DoubleClick += new System.EventHandler(this.gridAttributes_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Diccionario de datos - Entidades";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 442);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Diccionario de datos - Atributos";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreateEntity);
            this.groupBox1.Controls.Add(this.btnDeleteEntity);
            this.groupBox1.Location = new System.Drawing.Point(15, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 112);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entidad";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCreateAttribute);
            this.groupBox2.Controls.Add(this.btnDeleteAtribute);
            this.groupBox2.Location = new System.Drawing.Point(150, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(129, 112);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Atributo";
            // 
            // genericItemBindingSource
            // 
            this.genericItemBindingSource.DataSource = typeof(ProyectoArchivos.MainApp.Classes.genericItem);
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 719);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gridAttributes);
            this.Controls.Add(this.gridEntities);
            this.Controls.Add(this.radMenu1);
            this.Name = "frmStart";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Diccionario de datos";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateEntity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateAttribute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteAtribute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteEntity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.genericItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private Telerik.WinControls.UI.RadContextMenu radContextMenu1;
        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem newFile;
        private Telerik.WinControls.UI.RadMenuItem open;
        private Telerik.WinControls.UI.RadMenuItem Save;
        private Telerik.WinControls.UI.RadButton btnCreateEntity;
        private Telerik.WinControls.UI.RadButton btnCreateAttribute;
        private Telerik.WinControls.UI.RadButton btnDeleteAtribute;
        private Telerik.WinControls.UI.RadButton btnDeleteEntity;
        private Telerik.WinControls.UI.RadGridView gridEntities;
        private Telerik.WinControls.UI.RadGridView gridAttributes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.BindingSource genericItemBindingSource;
    }
}