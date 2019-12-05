namespace ProyectoArchivos.MainApp.Index
{
    partial class frmPrimaryKey
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
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gridPK = new Telerik.WinControls.UI.RadGridView();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.gridPK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPK.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridPK
            // 
            this.gridPK.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.gridPK.MasterTemplate.AllowAddNewRow = false;
            this.gridPK.MasterTemplate.AllowColumnReorder = false;
            this.gridPK.MasterTemplate.AllowDeleteRow = false;
            this.gridPK.MasterTemplate.AllowDragToGroup = false;
            gridViewDecimalColumn1.FieldName = "dir";
            gridViewDecimalColumn1.HeaderText = "Dirección";
            gridViewDecimalColumn1.Name = "dir";
            gridViewDecimalColumn1.Width = 75;
            gridViewTextBoxColumn1.FieldName = "val";
            gridViewTextBoxColumn1.HeaderText = "Llave";
            gridViewTextBoxColumn1.Name = "val";
            gridViewTextBoxColumn1.Width = 170;
            this.gridPK.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewDecimalColumn1,
            gridViewTextBoxColumn1});
            this.gridPK.MasterTemplate.EnableGrouping = false;
            this.gridPK.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridPK.Name = "gridPK";
            this.gridPK.ReadOnly = true;
            this.gridPK.Size = new System.Drawing.Size(265, 405);
            this.gridPK.TabIndex = 0;
            this.gridPK.ThemeName = "VisualStudio2012Light";
            // 
            // frmPrimaryKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 429);
            this.Controls.Add(this.gridPK);
            this.Name = "frmPrimaryKey";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Índice primario";
            this.ThemeName = "VisualStudio2012Light";
            this.Load += new System.EventHandler(this.frmPrimaryKey_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridPK.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView gridPK;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
    }
}
