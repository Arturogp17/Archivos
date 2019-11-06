using ProyectoArchivos.MainApp.Classes;
using ProyectoArchivos.MainApp.Create;
using ProyectoArchivos.MainApp.Delete;
using ProyectoArchivos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace ProyectoArchivos
{
    public partial class frmStart : Telerik.WinControls.UI.RadForm
    {
        List<Entity> entities = new List<Entity>();
        long header = -1;
        long lastAddress = 8;
        string fileName = string.Empty;
        FileStream file;
        string path = String.Empty;
        bool allowWrite = true;

        public frmStart()
        {
            InitializeComponent();
        }

        public int CreateFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFile.FileName;
                path = Path.GetDirectoryName(fileName);
                file = new FileStream(saveFile.FileName, FileMode.Create);
                file.Close();
                return 0;
            }
                return 1;
        }

        public void OpenFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            if(open.ShowDialog() == DialogResult.OK)
            {
                entities.Clear();
                fileName = open.FileName;
                path = Path.GetDirectoryName(fileName);
                file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                file.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(file);

                header = br.ReadInt64();
                txtHeader.Text = header.ToString();
                if(header != -1)
                {
                    ReadNextEntity(header, br, file);
                    btnCreateAttribute.Enabled = false;
                    btnDeleteAttribute.Enabled = false;
                    btnDeleteEntity.Enabled = true;
                }
                foreach(Entity e in entities)
                {
                    if (e.address + 72 > lastAddress)
                        lastAddress = e.address + 72;
                    foreach(Attributes a in e.attributes)
                    {
                        if (a.address + 73 > lastAddress)
                            lastAddress = a.address + 73;
                    }
                }
                file.Close();
                entities = entities.OrderBy(o => o.id).ToList();
                RefreshGrids();
            }
        }

        public void ReadNextEntity(long dir, BinaryReader br, FileStream file)
        {
            file.Seek(dir, SeekOrigin.Begin);
            Entity e = new Entity();
            e.id = Encoding.ASCII.GetString(br.ReadBytes(5));
            e.name = Encoding.ASCII.GetString(br.ReadBytes(35));
            e.name = e.name.Split('\0')[0];
            e.address = br.ReadInt64();
            e.attributeAddress = br.ReadInt64();
            e.dataAddress = br.ReadInt64();
            e.nextEntityAddress = br.ReadInt64();
            entities.Add(e);
            if (e.attributeAddress != -1)
                ReadNextAttribute(e.attributeAddress, br, file, e);

            if (e.nextEntityAddress != -1)
                ReadNextEntity(e.nextEntityAddress, br, file);
        }

        public void ReadNextAttribute(long dir, BinaryReader br, FileStream file, Entity e)
        {
            file.Seek(dir, SeekOrigin.Begin);
            Attributes a = new Attributes();
            a.id = Encoding.ASCII.GetString(br.ReadBytes(5));
            a.name = Encoding.ASCII.GetString(br.ReadBytes(35));
            a.name = a.name.Split('\0')[0];
            a.dataType = br.ReadChar();
            a.length = br.ReadInt32();
            a.address = br.ReadInt64();
            a.indexType = br.ReadInt32();
            if (a.indexType == 1)
                e.cve_busqueda++;
            a.indexAddress = br.ReadInt64();
            a.nextAttributeAddress = br.ReadInt64();
            e.attributes.Add(a);
            if (a.nextAttributeAddress != -1)
                ReadNextAttribute(a.nextAttributeAddress, br, file, e);
        }

        private void newFile_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        private int GetNextEntityID()
        {
            int id = 0;
            foreach(Entity e in entities)
            {
                if (Convert.ToInt32(e.id) > id)
                    id = Convert.ToInt32(e.id);
            }
            return id + 1;
        }

        private void btnCreateEntity_Click(object sender, EventArgs e)
        {
            int cf = 0;
            if (fileName == "")
            {
                cf = CreateFile();
            }
            if (cf == 0)
            {
                string id = string.Empty;
                if (entities.Count == 0)
                {
                    id = "00001";
                }
                else
                {
                    id = Convert.ToString(String.Format("{0:00000}", GetNextEntityID()));
                }
                frmCreateEntity ce = new frmCreateEntity(id);
                if (ce.ShowDialog() == DialogResult.OK)
                {
                    if (!Existe(ce.name))
                    {
                        Entity entity = new Entity();
                        entity.name = ce.name;
                        entity.id = id;
                        entity.attributeAddress = -1;
                        entity.dataAddress = -1;
                        entity.address = lastAddress;
                        lastAddress += 72;
                        entities.Add(entity);
                        SetNextEntityDirection();
                        WriteHeader();
                        WriteEntity(entity);
                        RefreshGrids();
                        btnCreateAttribute.Enabled = true;
                        btnDeleteAttribute.Enabled = true;
                        btnDeleteEntity.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se puede repetir el nombre de una entidad", "Diccionario de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnCreateEntity_Click(sender, e);
                    }
                }
            }
        }

        private bool Existe(string name)
        {
            foreach(Entity e in entities)
            {
                if (e.name == name)
                    return true;
            }
            return false;
        }

        public void SetNextEntityDirection()
        {
            entities = entities.OrderBy(o => o.name).ToList();
            header = entities.First().address;
            WriteHeader();
            txtHeader.Text = header.ToString();
            for(int i = 0; i < entities.Count - 1; i++)
            {
                entities[i].nextEntityAddress = entities[i + 1].address;
                WriteEntity(entities[i]);
            }
            entities.Last().nextEntityAddress = -1;
            WriteEntity(entities.Last());
            entities = entities.OrderBy(o => o.id).ToList();
        }

        public void RefreshGrids()
        {
            gridEntities.Rows.Clear();
            foreach(Entity e in entities)
            {
                GridViewRowInfo row = gridEntities.Rows.AddNew();
                row.Cells["id"].Value = e.id;
                row.Cells["name"].Value = e.name;
                row.Cells["address"].Value = e.address;
                row.Cells["attributeAddress"].Value = e.attributeAddress;
                row.Cells["dataAddress"].Value = e.dataAddress;
                row.Cells["nextEntityAddress"].Value = e.nextEntityAddress;
            }
        }

        private void btnCreateAttribute_Click(object sender, EventArgs e)
        {
            int id = 1;
            if (gridAttributes.Rows.Count > 0)
                id = Convert.ToInt32(gridAttributes.Rows[gridAttributes.Rows.Count - 1].Cells["id"].Value) + 1;
            frmCreateAttribute ca = new frmCreateAttribute(entities, id);
            if(ca.ShowDialog() == DialogResult.OK)
            {
                Attributes a = new Attributes();
                a.id = Convert.ToString(String.Format("{0:00000}", id));
                a.name = ca.name;
                a.dataType = ca.dataType;
                a.length = ca.length;
                a.address = lastAddress;
                lastAddress += 73;
                a.indexType = Convert.ToInt32(ca.indexType);
                a.indexAddress = -1;
                a.nextAttributeAddress = -1;
                foreach(Entity en in entities)
                {
                    if (en.id == ca.idEntity)
                    {
                        if (a.indexType == 1 && en.cve_busqueda > 0)
                        {
                            MessageBox.Show("Error", "Solo puede haber una clave de busqueda por entidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ca.Dispose();
                            return;
                        }
                        else
                        {
                            if (a.indexType == 1)
                                en.cve_busqueda++;
                            if (en.attributes.Count > 0)
                            {
                                en.attributes.Last().nextAttributeAddress = a.address;
                                WriteAttribute(en.attributes.Last());
                            }
                            else
                                en.attributeAddress = a.address;
                            en.attributes.Add(a);
                            WriteEntity(en);
                            WriteAttribute(a);
                            gridEntities_CellClick(this, null);
                            break;
                        }
                    }
                }
                ca.Dispose();
            }
        }

        private void WriteHeader()
        {
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            file.Seek(0, SeekOrigin.Begin);
            bw.Write(header);
            file.Close();
        }

        private void WriteEntity(Entity en)
        {
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            byte[] id = new byte[5];
            byte[] name = new byte[35];
            Encoding.ASCII.GetBytes(en.id, 0, 5, id, 0);
            Encoding.ASCII.GetBytes(en.name, 0, en.name.Length, name, 0);
            file.Seek(en.address, SeekOrigin.Begin);
            bw.Write(id);
            bw.Write(name);
            bw.Write(en.address);
            bw.Write(en.attributeAddress);
            bw.Write(en.dataAddress);
            bw.Write(en.nextEntityAddress);
            file.Close();
        }

        private void WriteAttribute(Attributes a)
        {
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            file.Seek(a.address, SeekOrigin.Begin);
            byte[] idA = new byte[5];
            byte[] nameA = new byte[35];
            Encoding.ASCII.GetBytes(a.id, 0, 5, idA, 0);
            Encoding.ASCII.GetBytes(a.name, 0, a.name.Length, nameA, 0);
            bw.Write(idA);
            bw.Write(nameA);
            bw.Write(a.dataType);
            bw.Write(a.length);
            bw.Write(a.address);
            bw.Write(a.indexType);
            bw.Write(a.indexAddress);
            bw.Write(a.nextAttributeAddress);
            file.Close();
        }

        private void gridEntities_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            string name = e.Value.ToString();
            foreach(Entity en in entities)
            {
                if(e.Row.Cells["id"].Value.ToString() == en.id)
                {
                    en.name = name;
                    SetNextEntityDirection();
                    RefreshGrids();
                    break;
                }
            }
            
        }

        private void gridAttributes_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            foreach(Entity en in entities)
            {
                foreach(Attributes a in en.attributes)
                {
                    if(e.Row.Cells["id"].Value.ToString() == a.id)
                    {
                        a.name = e.Row.Cells["name"].Value.ToString();
                        a.dataType = Convert.ToChar(e.Row.Cells["dataType"].Value);
                        if (a.dataType == 'C')
                            a.length = Convert.ToInt32(e.Row.Cells["length"].Value);
                        else
                        {
                            a.length = 4;
                            e.Row.Cells["length"].Value = 4;
                        }
                        a.indexType = Convert.ToInt32(e.Row.Cells["indexType"].Value);
                        WriteAttribute(a);
                        break;
                    }
                }
            }
        }

        private void gridAttributes_DoubleClick(object sender, EventArgs e)
        {
            Entity ent = new Entity();
            Attributes at = new Attributes();
            foreach (Entity en in entities)
            {
                foreach (Attributes a in en.attributes)
                {
                    if (a.id == gridAttributes.SelectedRows[0].Cells["id"].Value.ToString())
                    {
                        ent = en;
                        at = a;
                        break;
                    }
                }
                if (ent.name != "")
                    break;
            }
            frmCreateAttribute fe = new frmCreateAttribute(ent.name, at);
            if(fe.ShowDialog() == DialogResult.OK)
            {
                at.name = fe.name;
                at.dataType = fe.dataType;
                at.length = fe.length;
                at.indexType = Convert.ToInt32(fe.indexType);
                WriteAttribute(at);
                WriteEntity(ent);
                gridEntities_CellClick(this, null);
            }
            
        }

        private void btnDeleteEntity_Click(object sender, EventArgs e)
        {
            frmDeleteEntity fd = new frmDeleteEntity(entities);
            if (fd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].id == fd.lblID.Text)
                    {
                        entities[i].nextEntityAddress = -1;
                        entities[i].attributes.Clear();
                        entities.RemoveAt(i);
                        SetNextEntityDirection();
                        break;
                    }
                }
                foreach(Entity en in entities)
                {
                    WriteEntity(en);
                }
                if (entities.Count > 0)
                {
                    lastAddress = 8;
                    foreach (Entity ent in entities)
                    {
                        if (ent.address + 72 > lastAddress)
                            lastAddress = ent.address + 72;
                        foreach (Attributes a in ent.attributes)
                        {
                            if (a.address + 73 > lastAddress)
                            {
                                lastAddress = a.address + 73;
                            }
                        }
                    }
                    entities = entities.OrderBy(o => o.name).ToList();
                    header = entities.First().address;
                    entities = entities.OrderBy(o => o.id).ToList();
                }
                else
                {
                    header = -1;
                    lastAddress = 8;
                }
                RefreshGrids();
            }
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void btnDeleteAttribute_Click(object sender, EventArgs e)
        {
            List<Attributes> la = new List<Attributes>();
            foreach(Entity en in entities)
            {
                foreach(Attributes a in en.attributes)
                {
                    Attributes an = new Attributes();
                    an.entity = en.name;
                    an.id = a.id;
                    an.name = a.name;
                    an.dataType = a.dataType;
                    an.indexType = a.indexType;
                    an.length = a.length;
                    la.Add(an);
                }
            }
            if (la.Count > 0)
            {
                frmDeleteAttribute da = new frmDeleteAttribute(la);
                if (da.ShowDialog() == DialogResult.OK)
                {
                    foreach (Entity en in entities)
                    {
                        for (int i = 0; i < en.attributes.Count; i++)
                        {
                            if (en.attributes[i].id == da.lblID.Text)
                            {
                                if (i == 0)
                                {
                                    if (en.attributes.Count > 1)
                                        en.attributeAddress = en.attributes[i + 1].address;
                                    else
                                        en.attributeAddress = -1;
                                    WriteEntity(en);
                                }
                                else
                                {
                                    if (en.attributes.Count > i + 1)
                                        en.attributes[i - 1].nextAttributeAddress = en.attributes[i + 1].address;
                                    else
                                        en.attributes[i - 1].nextAttributeAddress = -1;
                                    WriteAttribute(en.attributes[i - 1]);
                                }
                                en.attributes[i].nextAttributeAddress = -1;
                                WriteAttribute(en.attributes[i]);
                                en.attributes.RemoveAt(i);
                                da.Dispose();
                                gridEntities_CellClick(this, null);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void gridEntities_CellClick(object sender, GridViewCellEventArgs e)
        {
            string entity = gridEntities.SelectedRows[0].Cells["id"].Value.ToString();
            foreach(Entity en in entities)
            {
                if(en.id == entity)
                { 
                    if(en.attributes.Count > 0)
                        btnDataFile.Enabled = true;
                    else
                        btnDataFile.Enabled = false;
                    gridAttributes.Rows.Clear();
                    foreach (Attributes a in en.attributes)
                    {
                        GridViewRowInfo row = gridAttributes.Rows.AddNew();
                        row.Cells["id"].Value = a.id;
                        row.Cells["name"].Value = a.name;
                        row.Cells["dataType"].Value = a.dataType.ToString();
                        row.Cells["length"].Value = a.length;
                        row.Cells["address"].Value = a.address;
                        row.Cells["indexType"].Value = a.indexType.ToString();
                        row.Cells["indexAddress"].Value = a.indexAddress;
                        row.Cells["nextAttributeAddress"].Value = a.nextAttributeAddress;
                    }
                    if(en.dataAddress != -1)
                    {
                        btnDataFile.Enabled = false;
                        allowWrite = false;
                        ReadRegisters(en);
                        
                        allowWrite = true;
                    }
                    else
                    {
                        btnCreateAttribute.Enabled = true;
                        btnDeleteAttribute.Enabled = true;
                    }
                    return;
                }
            }
        }

        private void ReadRegisters(Entity en)
        {
            int size = 16;
            gridRegisters.Rows.Clear();
            gridRegisters.Columns.Clear();
            gridAddRegister.Rows.Clear();
            gridAddRegister.Columns.Clear();
            btnDataFile.Enabled = false;
            btnCreateAttribute.Enabled = false;
            btnDeleteAttribute.Enabled = false;
            GridViewTextBoxColumn c = new GridViewTextBoxColumn();
            c.HeaderText = "Dirección";
            c.Name = "dir";
            c.ReadOnly = true;
            c.Width = 75;
            GridViewTextBoxColumn ca = new GridViewTextBoxColumn();
            ca.HeaderText = "Dirección";
            ca.Name = "dir";
            ca.ReadOnly = true;
            ca.Width = 75;
            gridRegisters.MasterTemplate.Columns.Add(c);
            gridAddRegister.MasterTemplate.Columns.Add(ca);
            foreach (Attributes a in en.attributes)
            {
                if (a.dataType == 'C')
                {
                    size += a.length;
                    c = new GridViewTextBoxColumn();
                    c.HeaderText = a.name;
                    c.Name = a.name;
                    c.ReadOnly = false;
                    c.Width = 100;
                    c.DataType = typeof(string);
                    c.MaxLength = a.length;
                    ca = new GridViewTextBoxColumn();
                    ca.HeaderText = a.name;
                    ca.Name = a.name;
                    ca.ReadOnly = false;
                    ca.Width = 100;
                    ca.DataType = typeof(string);
                    ca.MaxLength = a.length;
                    gridRegisters.MasterTemplate.Columns.Add(c);
                    gridAddRegister.MasterTemplate.Columns.Add(ca);
                }
                else
                {
                    size += 4;
                    GridViewDecimalColumn d = new GridViewDecimalColumn();
                    d.DecimalPlaces = 0;
                    d.HeaderText = a.name;
                    d.Name = a.name;
                    d.Width = 100;
                    d.ReadOnly = false;
                    d.DataType = typeof(int);
                    GridViewDecimalColumn da = new GridViewDecimalColumn();
                    da.DecimalPlaces = 0;
                    da.HeaderText = a.name;
                    da.Name = a.name;
                    da.Width = 100;
                    da.ReadOnly = false;
                    da.DataType = typeof(int);
                    gridRegisters.MasterTemplate.Columns.Add(d);
                    gridAddRegister.MasterTemplate.Columns.Add(da);
                }

            }
            
            c = new GridViewTextBoxColumn();
            c.HeaderText = "Sig.registro";
            c.Name = "sig";
            c.ReadOnly = true;
            c.Width = 75;
            ca = new GridViewTextBoxColumn();
            ca.HeaderText = "Sig.registro";
            ca.Name = "sig";
            ca.ReadOnly = true;
            ca.Width = 75;
            gridRegisters.MasterTemplate.Columns.Add(c);
            gridAddRegister.MasterTemplate.Columns.Add(ca);

            string p = Path.Combine(path, en.id + ".dat");
            FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryReader br = new BinaryReader(file);
            while (file.Position < file.Length)
            {
                GridViewRowInfo row = gridRegisters.Rows.AddNew();

                row.Cells[0].Value = br.ReadInt64().ToString();
                foreach (Attributes a in en.attributes)
                {
                    if (a.dataType == 'C')
                    {
                        row.Cells[a.name].Value = Encoding.ASCII.GetString(br.ReadBytes(a.length));
                    }
                    else
                    {
                        row.Cells[a.name].Value = br.ReadInt32();
                    }
                }
                row.Cells["sig"].Value = br.ReadInt64().ToString();
            }
            file.Close();
            allowWrite = true;

            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            gridAddRegister.Rows[0].Cells[0].Value = gridRegisters.RowCount * size;
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;
        }

        private void btnDataFile_Click(object sender, EventArgs e)
        {
            int size = 16;
            string name = gridEntities.SelectedRows[0].Cells["id"].Value.ToString();
            string p = Path.Combine(path, name + ".dat");
            FileStream file = new FileStream(p, FileMode.Create);
            gridRegisters.Rows.Clear();
            gridRegisters.Columns.Clear();
            
            gridAddRegister.Columns.Clear();
            gridAddRegister.Rows.Clear();
            foreach (Entity en in entities)
            {
                if (en.id == name)
                {
                    en.dataAddress = 0;
                    WriteEntity(en);
                    GridViewTextBoxColumn c = new GridViewTextBoxColumn();
                    c.HeaderText = "Dirección";
                    c.Name = "dir";
                    c.ReadOnly = true;
                    c.Width = 50;
                    GridViewTextBoxColumn ca = new GridViewTextBoxColumn();
                    ca.HeaderText = "Dirección";
                    ca.Name = "dir";
                    ca.ReadOnly = true;
                    ca.Width = 50;
                    gridRegisters.MasterTemplate.Columns.Add(c);
                    gridAddRegister.MasterTemplate.Columns.Add(ca);
                    foreach (Attributes a in en.attributes)
                    {
                        if (a.dataType == 'C')
                        {
                            size += a.length;
                            c = new GridViewTextBoxColumn();
                            c.HeaderText = a.name;
                            c.Name = a.name;
                            c.ReadOnly = false;
                            c.Width = 100;
                            c.DataType = typeof(string);
                            c.MaxLength = a.length;
                            ca = new GridViewTextBoxColumn();
                            ca.HeaderText = a.name;
                            ca.Name = a.name;
                            ca.ReadOnly = false;
                            ca.Width = 100;
                            ca.DataType = typeof(string);
                            ca.MaxLength = a.length;
                            gridRegisters.MasterTemplate.Columns.Add(c);
                            gridAddRegister.MasterTemplate.Columns.Add(ca);
                        }
                        else
                        {
                            size += 4;
                            GridViewDecimalColumn d = new GridViewDecimalColumn();
                            d.DecimalPlaces = 0;
                            d.HeaderText = a.name;
                            d.Name = a.name;
                            d.Width = 100;
                            d.ReadOnly = false;
                            d.DataType = typeof(int);
                            GridViewDecimalColumn da = new GridViewDecimalColumn();
                            da.DecimalPlaces = 0;
                            da.HeaderText = a.name;
                            da.Name = a.name;
                            da.Width = 100;
                            da.ReadOnly = false;
                            da.DataType = typeof(int);
                            gridRegisters.MasterTemplate.Columns.Add(d);
                            gridAddRegister.MasterTemplate.Columns.Add(da);
                        }
                        
                    }
                    c = new GridViewTextBoxColumn();
                    c.HeaderText = "Sig.registro";
                    c.Name = "sig";
                    c.ReadOnly = true;
                    c.Width = 60;
                    ca = new GridViewTextBoxColumn();
                    ca.HeaderText = "Sig.registro";
                    ca.Name = "sig";
                    ca.ReadOnly = true;
                    ca.Width = 60;
                    gridRegisters.MasterTemplate.Columns.Add(c);
                    gridAddRegister.MasterTemplate.Columns.Add(ca);
                }
            }
            file.Close();
            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            gridAddRegister.Rows[0].Cells[0].Value = gridRegisters.RowCount * size;
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;
        }

        private Entity FindEntity(string id)
        {
            foreach(Entity e in entities)
            {
                if (e.id == id)
                    return e;
            }
            return null;
        }
        

        public void RegistersNextAddrs(Entity e)
        {
            allowWrite = false;
            foreach (Attributes a in e.attributes)
            {
                if (a.indexType == 1)
                {
                    RegistersSearchKey(e);
                    return;
                }
            }
            if (gridRegisters.RowCount > 1)
            {
                for (int i = 0; i < gridRegisters.RowCount; i++)
                {
                    gridRegisters.Rows[i].Cells[gridRegisters.ColumnCount - 1].Value = gridRegisters.Rows[i + 1].Cells[0].Value;
                }
            }
            else
            {
                gridRegisters.Rows[0].Cells[gridRegisters.ColumnCount - 1].Value = -1;
            }
            allowWrite = true;
        }

        public void RegistersSearchKey(Entity e)
        {
            int indice = 1;
            int type = 0;
            foreach(Attributes a in e.attributes)
            {
                if (a.indexType == 1)
                {
                    if (a.dataType == 'C')
                        type = 1;
                    break;
                }
                else
                    indice++;
            }
            List<Register> lr = new List<Register>();
            for (int i = 0; i < gridRegisters.RowCount; i++)
            {
                Register r = new Register();
                r.dir = Convert.ToInt64(gridRegisters.Rows[i].Cells[0].Value);
                if (type == 0)
                    r.val = Convert.ToInt32(gridRegisters.Rows[i].Cells[indice].Value);
                else
                    r.val = Convert.ToString(gridRegisters.Rows[i].Cells[indice].Value);
                lr.Add(r);
            }
            lr = lr.OrderBy(o => o.val).ToList();
            if (lr.Count > 1)
            {
                for (int i = 0; i < lr.Count - 1; i++)
                {
                    lr[i].nextDir = lr[i + 1].dir;
                }
            }
            lr.Last().nextDir = -1;
            e.dataAddress = lr[0].dir;
            WriteEntity(e);
            gridEntities.SelectedRows[0].Cells[4].Value = lr[0].dir;
            foreach (Register r in lr)
            {
                for (int i = 0; i < gridRegisters.RowCount; i++)
                {
                    if (Convert.ToInt64(gridRegisters.Rows[i].Cells[0].Value) == r.dir)
                    {
                        gridRegisters.Rows[i].Cells[gridRegisters.ColumnCount - 1].Value = r.nextDir;
                        break;
                    }
                }
            }
        }

        private void WriteRegister(int size)
        {
            if (gridRegisters.RowCount > 0)
            {
                Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
                string p = Path.Combine(path, en.id + ".dat");
                FileStream file = new FileStream(p, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryWriter bw = new BinaryWriter(file);
                for (int i = 0; i < gridRegisters.RowCount; i++)
                {
                    file.Seek(Convert.ToInt64(Convert.ToString(gridRegisters.Rows[i].Cells[0].Value)), SeekOrigin.Begin);
                    for (int j = 0; j < gridRegisters.ColumnCount; j++)
                    {
                        if (j == 0 || j == gridRegisters.ColumnCount - 1)
                        {
                            long id;
                            if (Convert.ToString(gridRegisters.Rows[i].Cells[j].Value) != "")
                                id = Convert.ToInt64(Convert.ToString(gridRegisters.Rows[i].Cells[j].Value));
                            else
                                id = 0;

                            bw.Write(id);
                        }
                        else
                        {
                            if (en.attributes[j - 1].dataType == 'C')
                            {
                                byte[] reg = new byte[en.attributes[j - 1].length];
                                string val = Convert.ToString(gridRegisters.Rows[i].Cells[j].Value);
                                int len = en.attributes[j - 1].length;
                                Encoding.ASCII.GetBytes(val, 0, val.Length, reg, 0);
                                bw.Write(reg);
                            }
                            else
                            {
                                bw.Write(Convert.ToInt32(gridRegisters.Rows[i].Cells[j].Value.ToString()));
                            }
                        }
                    }
                }
                file.Close();
            }
        }

        private void ReadSearchKey(int size)
        {
            if (gridRegisters.Rows[gridRegisters.RowCount - 1].Cells[1].Value != null)
            {
                allowWrite = false;
                List<List<string>> lm = new List<List<string>>();

                Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
                if (en.cve_busqueda > 0)
                {
                    for (int i = 0; i < gridRegisters.RowCount; i++)
                    {
                        List<string> l = new List<string>();
                        for (int j = 0; j < gridRegisters.ColumnCount; j++)
                        {
                            l.Add(Convert.ToString(gridRegisters.Rows[i].Cells[j].Value));
                        }
                        lm.Add(l);
                    }
                }
                int index = 1;
                int type = 0;
                lm[lm.Count - 1][0] = ((lm.Count - 1) * size).ToString();
                for (int i = 0; i < en.attributes.Count; i++)
                {
                    if (en.attributes[i].indexType == 1)
                    {
                        index += i;
                        if (en.attributes[i].dataType == 'C')
                            type = 1;
                        break;
                    }
                }
                List<List<string>> lr = new List<List<string>>();
                if (type == 1)
                {
                    for (int i = 0; i < lm.Count; i++)
                    {
                        if (i == 0)
                        {
                            lr.Add(lm[i]);
                        }
                        else
                        {
                            if (lr.Count == 1)
                            {
                                lr.Insert(0, lm[i]);
                            }
                            else
                            {
                                for (int j = 0; j < lr.Count; j++)
                                {
                                    if (String.Compare(lm[i][index], lr[j][index]) == -1)
                                    {
                                        lr.Insert(j, lm[i]);
                                        break;
                                    }
                                    else
                                    {
                                        if (String.Compare(lm[i][index], lr[j][index]) == 0)
                                        {
                                            if (String.Compare(lm[i][0], lr[j][0]) == 1)
                                            {
                                                lr.Insert(j, lm[i]);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    for (int i = 0; i < lm.Count; i++)
                    {
                        if (i == 0)
                        {
                            lr.Add(lm[i]);
                        }
                        else
                        {
                            if (lr.Count == 1)
                            {
                                if (Convert.ToInt32(lm[i][index]) <= Convert.ToInt32(lr[0][index]))
                                    lr.Insert(0, lm[i]);
                                else
                                    lr.Add(lm[i]);
                            }
                            else
                            {
                                bool insert = false;
                                for (int j = 0; j < lr.Count; j++)
                                {
                                    if (Convert.ToInt32(lm[i][index]) <= Convert.ToInt32(lr[j][index]))
                                    {
                                        lr.Insert(j, lm[i]);
                                        insert = true;
                                        break;
                                    }
                                }
                                if (!insert)
                                    lr.Add(lm[i]);
                            }
                        }
                    }
                }

                for (int k = 0; k < lr.Count - 1 && lr.Count > 1; k++)
                {
                    lr[k][lr[k].Count - 1] = lr[k+1][0];
                }

                lr[lr.Count - 1][lr[lr.Count - 1].Count - 1] = "-1";
                
                for (int i = 0; i < lr.Count; i++)
                {
                    GridViewRowInfo row = gridRegisters.Rows[i];
                    row.Cells[0].Value = lr[i][0];
                    for (int j = 1; j < lr[i].Count - 1; j++)
                    {
                        if (en.attributes[j - 1].dataType == 'C')
                            row.Cells[j].Value = lr[i][j];
                        else
                            row.Cells[j].Value = Convert.ToInt32(lr[i][j]);
                    }
                    row.Cells[lr[i].Count - 1].Value = lr[i][lr[i].Count - 1];
                }
                allowWrite = true;
            }
        }
        
        private void btnAddReg_Click(object sender, EventArgs e)
        {
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
            
            for (int i = 0; i < gridAddRegister.ColumnCount; i++)
            {
                if(String.IsNullOrWhiteSpace(Convert.ToString(gridAddRegister.Rows[0].Cells[i].Value)))
                {
                    MessageBox.Show("No se puede agregar un registro hasta que la informacion este completa", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            GridViewRowInfo row = gridRegisters.Rows.AddNew();
            for (int i = 0; i < gridAddRegister.ColumnCount; i++)
            {
                row.Cells[i].Value = gridAddRegister.Rows[0].Cells[i].Value;
            }
            int size = 16;
            
            foreach(Attributes a in en.attributes)
            {
                if (a.dataType == 'C')
                    size += a.length;
                else
                    size += 4;
            }
            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            gridAddRegister.Rows[0].Cells[0].Value = gridRegisters.RowCount * size;
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;
            RegistersNextAddrs(en);
            WriteRegister(size);
        }

        private void gridRegisters_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells[0].Value.ToString());
            foreach(Attributes a in en.attributes)
            {
                if(a.indexType == 1)
                {
                    RegistersNextAddrs(en);
                }
            }
        }
    }
}
