namespace ProyectoArchivos.MainApp.Index
{
    partial class frmBinaryTree
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.gridTree = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTree.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTree
            // 
            this.gridTree.Location = new System.Drawing.Point(0, 2);
            // 
            // 
            // 
            this.gridTree.MasterTemplate.AllowAddNewRow = false;
            this.gridTree.MasterTemplate.AllowDragToGroup = false;
            this.gridTree.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.gridTree.MasterTemplate.EnableGrouping = false;
            this.gridTree.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridTree.Name = "gridTree";
            this.gridTree.Size = new System.Drawing.Size(806, 417);
            this.gridTree.TabIndex = 0;
            this.gridTree.ThemeName = "VisualStudio2012Light";
            this.gridTree.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.gridTree_CellFormatting);
            // 
            // frmBinaryTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 420);
            this.Controls.Add(this.gridTree);
            this.Name = "frmBinaryTree";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmBinaryTree";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.gridTree.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadGridView gridTree;
    }
}
