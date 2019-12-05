namespace ProyectoArchivos.MainApp.Index
{
    partial class frmForeignKey
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gridFK = new Telerik.WinControls.UI.RadGridView();
            this.gridFKData = new Telerik.WinControls.UI.RadGridView();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFK
            // 
            this.gridFK.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.gridFK.MasterTemplate.AllowAddNewRow = false;
            this.gridFK.MasterTemplate.AllowColumnReorder = false;
            this.gridFK.MasterTemplate.AllowColumnResize = false;
            this.gridFK.MasterTemplate.AllowDeleteRow = false;
            this.gridFK.MasterTemplate.AllowDragToGroup = false;
            this.gridFK.MasterTemplate.AllowEditRow = false;
            this.gridFK.MasterTemplate.AllowRowReorder = true;
            gridViewTextBoxColumn4.FieldName = "val";
            gridViewTextBoxColumn4.HeaderText = "Clave";
            gridViewTextBoxColumn4.Name = "val";
            gridViewTextBoxColumn4.Width = 200;
            gridViewTextBoxColumn5.FieldName = "dir";
            gridViewTextBoxColumn5.HeaderText = "Dirección";
            gridViewTextBoxColumn5.Name = "dir";
            gridViewTextBoxColumn5.Width = 150;
            this.gridFK.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.gridFK.MasterTemplate.EnableGrouping = false;
            this.gridFK.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.gridFK.Name = "gridFK";
            this.gridFK.Size = new System.Drawing.Size(389, 427);
            this.gridFK.TabIndex = 0;
            this.gridFK.ThemeName = "VisualStudio2012Light";
            this.gridFK.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridFK_CellClick);
            // 
            // gridFKData
            // 
            this.gridFKData.Location = new System.Drawing.Point(407, 12);
            // 
            // 
            // 
            this.gridFKData.MasterTemplate.AllowAddNewRow = false;
            this.gridFKData.MasterTemplate.AllowColumnReorder = false;
            this.gridFKData.MasterTemplate.AllowColumnResize = false;
            this.gridFKData.MasterTemplate.AllowDeleteRow = false;
            this.gridFKData.MasterTemplate.AllowDragToGroup = false;
            this.gridFKData.MasterTemplate.AllowEditRow = false;
            this.gridFKData.MasterTemplate.AllowRowReorder = true;
            gridViewTextBoxColumn6.FieldName = "dir";
            gridViewTextBoxColumn6.HeaderText = "Dirección";
            gridViewTextBoxColumn6.Name = "dir";
            gridViewTextBoxColumn6.Width = 150;
            this.gridFKData.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn6});
            this.gridFKData.MasterTemplate.EnableGrouping = false;
            this.gridFKData.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.gridFKData.Name = "gridFKData";
            this.gridFKData.Size = new System.Drawing.Size(184, 427);
            this.gridFKData.TabIndex = 1;
            this.gridFKData.ThemeName = "VisualStudio2012Light";
            // 
            // frmForeignKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 451);
            this.Controls.Add(this.gridFKData);
            this.Controls.Add(this.gridFK);
            this.Name = "frmForeignKey";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Índice secundario";
            this.ThemeName = "VisualStudio2012Light";
            this.Load += new System.EventHandler(this.frmForeignKey_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridFK.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView gridFK;
        private Telerik.WinControls.UI.RadGridView gridFKData;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
    }
}
