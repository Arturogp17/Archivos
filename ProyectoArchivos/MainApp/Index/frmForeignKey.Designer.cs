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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gridFKData = new Telerik.WinControls.UI.RadGridView();
            this.gridFK = new Telerik.WinControls.UI.RadGridView();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFKData
            // 
            this.gridFKData.Location = new System.Drawing.Point(398, 2);
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
            this.gridFKData.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "dir";
            gridViewTextBoxColumn1.HeaderText = "Dirección";
            gridViewTextBoxColumn1.Name = "dir";
            gridViewTextBoxColumn1.Width = 165;
            this.gridFKData.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1});
            this.gridFKData.MasterTemplate.EnableGrouping = false;
            this.gridFKData.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridFKData.Name = "gridFKData";
            this.gridFKData.Size = new System.Drawing.Size(184, 427);
            this.gridFKData.TabIndex = 3;
            this.gridFKData.ThemeName = "VisualStudio2012Light";
            // 
            // gridFK
            // 
            this.gridFK.Location = new System.Drawing.Point(3, 2);
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
            this.gridFK.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn2.FieldName = "dir";
            gridViewTextBoxColumn2.HeaderText = "Dirección";
            gridViewTextBoxColumn2.Name = "dir";
            gridViewTextBoxColumn2.Width = 159;
            gridViewTextBoxColumn3.FieldName = "val";
            gridViewTextBoxColumn3.HeaderText = "Clave";
            gridViewTextBoxColumn3.Name = "val";
            gridViewTextBoxColumn3.Width = 212;
            this.gridFK.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.gridFK.MasterTemplate.EnableGrouping = false;
            this.gridFK.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.gridFK.Name = "gridFK";
            this.gridFK.Size = new System.Drawing.Size(389, 427);
            this.gridFK.TabIndex = 2;
            this.gridFK.ThemeName = "VisualStudio2012Light";
            this.gridFK.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridFK_CellClick);
            // 
            // frmForeignKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 431);
            this.Controls.Add(this.gridFKData);
            this.Controls.Add(this.gridFK);
            this.Name = "frmForeignKey";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmForeignKey";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFKData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView gridFKData;
        private Telerik.WinControls.UI.RadGridView gridFK;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
    }
}
