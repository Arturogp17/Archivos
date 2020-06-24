namespace ProyectoArchivos.MainApp.Index
{
    partial class frmStaticHash
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.gridBlocks = new Telerik.WinControls.UI.RadGridView();
            this.gridData = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlocks.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridBlocks
            // 
            this.gridBlocks.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.gridBlocks.MasterTemplate.AllowAddNewRow = false;
            this.gridBlocks.MasterTemplate.AllowColumnReorder = false;
            this.gridBlocks.MasterTemplate.AllowColumnResize = false;
            this.gridBlocks.MasterTemplate.AllowDeleteRow = false;
            this.gridBlocks.MasterTemplate.AllowDragToGroup = false;
            this.gridBlocks.MasterTemplate.AllowEditRow = false;
            this.gridBlocks.MasterTemplate.AllowRowReorder = true;
            this.gridBlocks.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "id";
            gridViewTextBoxColumn1.HeaderText = "Bloque";
            gridViewTextBoxColumn1.Name = "id";
            gridViewTextBoxColumn1.Width = 84;
            gridViewTextBoxColumn2.FieldName = "dir";
            gridViewTextBoxColumn2.HeaderText = "Dirección";
            gridViewTextBoxColumn2.Name = "dir";
            gridViewTextBoxColumn2.Width = 138;
            this.gridBlocks.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2});
            this.gridBlocks.MasterTemplate.EnableGrouping = false;
            this.gridBlocks.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridBlocks.Name = "gridBlocks";
            this.gridBlocks.Size = new System.Drawing.Size(240, 427);
            this.gridBlocks.TabIndex = 5;
            this.gridBlocks.ThemeName = "VisualStudio2012Light";
            this.gridBlocks.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridBlocks_CellClick);
            // 
            // gridData
            // 
            this.gridData.Location = new System.Drawing.Point(258, 12);
            // 
            // 
            // 
            this.gridData.MasterTemplate.AllowAddNewRow = false;
            this.gridData.MasterTemplate.AllowColumnReorder = false;
            this.gridData.MasterTemplate.AllowColumnResize = false;
            this.gridData.MasterTemplate.AllowDeleteRow = false;
            this.gridData.MasterTemplate.AllowDragToGroup = false;
            this.gridData.MasterTemplate.AllowEditRow = false;
            this.gridData.MasterTemplate.AllowRowReorder = true;
            this.gridData.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn3.FieldName = "val";
            gridViewTextBoxColumn3.HeaderText = "Clave";
            gridViewTextBoxColumn3.Name = "val";
            gridViewTextBoxColumn3.Width = 116;
            gridViewTextBoxColumn4.FieldName = "dir";
            gridViewTextBoxColumn4.HeaderText = "Dirección";
            gridViewTextBoxColumn4.Name = "dir";
            gridViewTextBoxColumn4.Width = 139;
            this.gridData.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.gridData.MasterTemplate.EnableGrouping = false;
            this.gridData.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.gridData.Name = "gridData";
            this.gridData.Size = new System.Drawing.Size(273, 427);
            this.gridData.TabIndex = 4;
            this.gridData.ThemeName = "VisualStudio2012Light";
            // 
            // frmStaticHash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 443);
            this.Controls.Add(this.gridBlocks);
            this.Controls.Add(this.gridData);
            this.Name = "frmStaticHash";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Hash estática";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.gridBlocks.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadGridView gridBlocks;
        private Telerik.WinControls.UI.RadGridView gridData;
    }
}
