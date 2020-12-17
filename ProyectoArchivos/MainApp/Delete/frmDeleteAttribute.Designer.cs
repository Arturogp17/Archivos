namespace ProyectoArchivos.MainApp.Delete
{
    partial class frmDeleteAttribute
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.btnCreate = new Telerik.WinControls.UI.RadButton();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.ddlAttributes = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.lblEntity = new Telerik.WinControls.UI.RadLabel();
            this.lblDataType = new Telerik.WinControls.UI.RadLabel();
            this.lblLenght = new Telerik.WinControls.UI.RadLabel();
            this.lblIndexType = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEntity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLenght)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndexType)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(16, 16);
            this.radLabel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.Size = new System.Drawing.Size(42, 23);
            this.radLabel7.TabIndex = 20;
            this.radLabel7.Text = "Tabla";
            this.radLabel7.ThemeName = "VisualStudio2012Light";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(85, 209);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(147, 31);
            this.btnCreate.TabIndex = 19;
            this.btnCreate.Text = "Eliminar";
            this.btnCreate.ThemeName = "VisualStudio2012Light";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(16, 170);
            this.radLabel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(66, 23);
            this.radLabel6.TabIndex = 18;
            this.radLabel6.Text = "Longitud";
            this.radLabel6.ThemeName = "VisualStudio2012Light";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 209);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(147, 31);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.ThemeName = "VisualStudio2012Light";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(16, 131);
            this.radLabel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(79, 23);
            this.radLabel3.TabIndex = 15;
            this.radLabel3.Text = "Tipo indice";
            this.radLabel3.ThemeName = "VisualStudio2012Light";
            // 
            // radLabel4
            // 
            this.radLabel4.AutoSize = false;
            this.radLabel4.Location = new System.Drawing.Point(16, 92);
            this.radLabel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(73, 24);
            this.radLabel4.TabIndex = 13;
            this.radLabel4.Text = "Tipo dato";
            this.radLabel4.ThemeName = "VisualStudio2012Light";
            // 
            // ddlAttributes
            // 
            this.ddlAttributes.DisplayMember = "name";
            this.ddlAttributes.DropDownAnimationEnabled = false;
            this.ddlAttributes.DropDownHeight = 141;
            this.ddlAttributes.ItemHeight = 24;
            radListDataItem1.Text = "Sin tipo de indice";
            radListDataItem2.Text = "Clave de busqueda";
            radListDataItem3.Text = "Indice primario";
            radListDataItem4.Text = "Indice secundario";
            this.ddlAttributes.Items.Add(radListDataItem1);
            this.ddlAttributes.Items.Add(radListDataItem2);
            this.ddlAttributes.Items.Add(radListDataItem3);
            this.ddlAttributes.Items.Add(radListDataItem4);
            this.ddlAttributes.Location = new System.Drawing.Point(105, 51);
            this.ddlAttributes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ddlAttributes.Name = "ddlAttributes";
            this.ddlAttributes.Size = new System.Drawing.Size(268, 31);
            this.ddlAttributes.TabIndex = 21;
            this.ddlAttributes.ThemeName = "VisualStudio2012Light";
            this.ddlAttributes.ValueMember = "id";
            this.ddlAttributes.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlAttributes_SelectedIndexChanged);
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(16, 55);
            this.radLabel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(61, 23);
            this.radLabel5.TabIndex = 22;
            this.radLabel5.Text = "Atributo";
            this.radLabel5.ThemeName = "VisualStudio2012Light";
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = false;
            this.lblEntity.BackColor = System.Drawing.Color.White;
            this.lblEntity.Location = new System.Drawing.Point(105, 14);
            this.lblEntity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(268, 26);
            this.lblEntity.TabIndex = 23;
            this.lblEntity.ThemeName = "VisualStudio2012Light";
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = false;
            this.lblDataType.BackColor = System.Drawing.Color.White;
            this.lblDataType.Location = new System.Drawing.Point(105, 90);
            this.lblDataType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(268, 26);
            this.lblDataType.TabIndex = 23;
            this.lblDataType.ThemeName = "VisualStudio2012Light";
            // 
            // lblLenght
            // 
            this.lblLenght.AutoSize = false;
            this.lblLenght.BackColor = System.Drawing.Color.White;
            this.lblLenght.Location = new System.Drawing.Point(104, 169);
            this.lblLenght.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblLenght.Name = "lblLenght";
            this.lblLenght.Size = new System.Drawing.Size(268, 26);
            this.lblLenght.TabIndex = 25;
            this.lblLenght.ThemeName = "VisualStudio2012Light";
            // 
            // lblIndexType
            // 
            this.lblIndexType.AutoSize = false;
            this.lblIndexType.BackColor = System.Drawing.Color.White;
            this.lblIndexType.Location = new System.Drawing.Point(104, 129);
            this.lblIndexType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblIndexType.Name = "lblIndexType";
            this.lblIndexType.Size = new System.Drawing.Size(268, 26);
            this.lblIndexType.TabIndex = 24;
            this.lblIndexType.ThemeName = "VisualStudio2012Light";
            // 
            // frmDeleteAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 247);
            this.Controls.Add(this.lblLenght);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.lblIndexType);
            this.Controls.Add(this.lblEntity);
            this.Controls.Add(this.ddlAttributes);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel4);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmDeleteAttribute";
            this.Text = "Eliminar atributo";
            this.Load += new System.EventHandler(this.frmDeleteAttribute_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEntity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLenght)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndexType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private Telerik.WinControls.UI.RadButton btnCreate;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadDropDownList ddlAttributes;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel lblEntity;
        private Telerik.WinControls.UI.RadLabel lblDataType;
        private Telerik.WinControls.UI.RadLabel lblLenght;
        private Telerik.WinControls.UI.RadLabel lblIndexType;
    }
}