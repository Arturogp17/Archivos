using ProyectoArchivos.MainApp.Classes;
using ProyectoArchivos.MainApp.Create;
using ProyectoArchivos.MainApp.Delete;
using ProyectoArchivos.MainApp.Index;
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
        frmBinaryTree binaryTree;
        frmForeignKey foreignKey;
        frmStaticHash staticHash;
        List<Object> oldRegister;
        List<Relation> relations;
        bool errorSQL = false;

        public frmStart()
        {
            InitializeComponent();
            binaryTree = null;
            foreignKey = null;
            staticHash = null;
        }

        /// <summary>
        /// Limpia los controles y las variables globales, para utilizarlas en un nuevo archivo sin cerrar el programa
        /// </summary>
        private void ClearData()
        {
            binaryTree = null;
            foreignKey = null;
            entities = new List<Entity>();
            header = -1;
            lastAddress = 8;
            fileName = string.Empty;
            path = String.Empty;
            allowWrite = true;
            gridAddRegister.Rows.Clear();
            gridAddRegister.Columns.Clear();
            gridAttributes.Rows.Clear();
            gridEntities.Rows.Clear();
            gridRegisters.Rows.Clear();
            gridRegisters.Columns.Clear();
            relations = new List<Relation>();
            if (binaryTree != null) binaryTree.Dispose();
            if (staticHash != null) staticHash.Dispose();
            if (foreignKey != null) foreignKey.Dispose();
        }


        public int CreateFile()
        {
            ClearData();
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
            if (open.ShowDialog() == DialogResult.OK)
            {
                entities.Clear();
                relations = new List<Relation>();
                fileName = open.FileName;
                path = Path.GetDirectoryName(fileName);
                file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                file.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(file);

                header = br.ReadInt64();
                txtHeader.Text = header.ToString();
                if (header != -1)
                {
                    ReadNextEntity(header, br, file);
                    btnCreateAttribute.Enabled = false;
                    btnDeleteAttribute.Enabled = false;
                    btnDeleteEntity.Enabled = true;
                }
                foreach (Entity e in entities)
                {
                    if (e.address + 72 > lastAddress)
                        lastAddress = e.address + 72;
                    foreach (Attributes a in e.attributes)
                    {
                        if (a.address + 73 > lastAddress)
                            lastAddress = a.address + 73;
                    }
                }
                file.Close();
                ReadRelations();
                entities = entities.OrderBy(o => o.id).ToList();
                RefreshGrids();
            }
        }

        private void ReadRelations()
        {
            if (File.Exists(fileName + ".rlt"))
            {
                file = File.Open(fileName + ".rlt", FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader br = new BinaryReader(file);
                file.Seek(0, SeekOrigin.Begin);
                while (file.Position < file.Length)
                {
                    string aux = Encoding.ASCII.GetString(br.ReadBytes(17));
                    List<string> la = aux.Split('-').ToList();
                    Relation r = new Relation(la);
                    relations.Add(r);
                }
                file.Close();
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
            foreach (Entity e in entities)
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
            foreach (Entity e in entities)
            {
                if (e.name == name)
                    return true;
            }
            return false;
        }

        public void SetNextEntityDirection()
        {
            if (entities.Count > 0)
            {
                entities = entities.OrderBy(o => o.name).ToList();
                header = entities.First().address;
                WriteHeader();
                txtHeader.Text = header.ToString();
                for (int i = 0; i < entities.Count - 1; i++)
                {
                    entities[i].nextEntityAddress = entities[i + 1].address;
                    WriteEntity(entities[i]);
                }
                entities.Last().nextEntityAddress = -1;
                WriteEntity(entities.Last());
                entities = entities.OrderBy(o => o.id).ToList();
            }
        }

        public void RefreshGrids()
        {
            gridEntities.Rows.Clear();
            gridAttributes.Rows.Clear();
            gridRegisters.Rows.Clear();
            gridRegisters.Columns.Clear();
            gridAddRegister.Rows.Clear();
            gridAddRegister.Columns.Clear();
            foreach (Entity e in entities)
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
            ca.idEntity = Convert.ToString(gridEntities.SelectedRows[0].Cells["id"].Value);
            if (ca.ShowDialog() == DialogResult.OK)
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
                foreach (Entity en in entities)
                {
                    if (en.id == ca.idEntity)
                    {
                        if (a.indexType == 1)
                        {
                            foreach (var item in en.attributes)
                            {
                                if (item.indexType == 1)
                                {
                                    MessageBox.Show("Solo puede haber una clave de busqueda por entidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ca.Dispose();
                                    return;
                                }
                            }

                        }
                        switch (a.indexType)
                        {
                            case 3:
                                //foreignKey = new frmForeignKey(a, path);
                                NewRelationship(ca.idEntityFK, ca.idAttrFK, ca.idEntDestFK);
                                Relation r = new Relation(ca.idEntityFK, ca.idAttrFK, ca.idEntDestFK);
                                relations.Add(r);
                                break;
                            case 4:
                                binaryTree = new frmBinaryTree(a, path, fileName);
                                break;
                            case 5:
                                staticHash = new frmStaticHash(a, path);
                                break;
                        }
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
                ca.Dispose();
            }
        }

        private void NewRelationship(string tablaOrigen, string attrOrigen, string tablaDest)
        {
            FileStream file = new FileStream(fileName + ".rlt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            string text = tablaOrigen + "-" + attrOrigen + "-" + tablaDest;
            byte[] id = new byte[17];
            Encoding.ASCII.GetBytes(text, 0, 17, id, 0);
            file.Seek(file.Length, SeekOrigin.Begin);
            bw.Write(id);
            file.Close();
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
            foreach (Entity en in entities)
            {
                if (e.Row.Cells["id"].Value.ToString() == en.id)
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
            foreach (Entity en in entities)
            {
                foreach (Attributes a in en.attributes)
                {
                    if (e.Row.Cells["id"].Value.ToString() == a.id)
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
            if (fe.ShowDialog() == DialogResult.OK)
            {
                if (at.indexType == 2)
                {
                    foreach (var item in relations)
                    {
                        if (item.tablaDestino == ent.id)
                        {
                            foreach (var en in entities)
                            {
                                if (en.id == item.tablaOrigen)
                                {
                                    foreach (var a in en.attributes)
                                    {
                                        if (a.id == item.attrOrigen)
                                        {
                                            a.name = fe.name;
                                            WriteAttribute(a);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
                        DeleteEntity(entities[i].id);
                        foreach (var a in entities[i].attributes)
                        {
                            DeleteAttribute(a.id);
                        }
                        entities[i].nextEntityAddress = -1;
                        entities[i].attributes.Clear();
                        entities.RemoveAt(i);
                        SetNextEntityDirection();
                        break;
                    }
                }
                foreach (Entity en in entities)
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
                    file = new FileStream(fileName, FileMode.Create);
                    file.Close();
                    WriteHeader();
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
            if (gridEntities.SelectedRows.Count() == 0 || gridAttributes.SelectedRows.Count() == 0)
            {
                MessageBox.Show("Selecciona una tabla y un atributo para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<Attributes> la = new List<Attributes>();
            foreach (Entity en in entities)
            {
                if (en.id == gridEntities.SelectedRows[0].Cells["id"].Value.ToString())
                {
                    foreach (Attributes a in en.attributes)
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
            }
            if (la.Count > 0)
            {
                frmDeleteAttribute da = new frmDeleteAttribute(la);
                da.entity = gridEntities.SelectedRows[0].Cells["name"].Value.ToString();
                da.attr = gridAttributes.SelectedRows[0].Cells["id"].Value.ToString();
                if (da.ShowDialog() == DialogResult.OK)
                {
                    switch (gridAttributes.SelectedRows[0].Cells["indexType"].Value.ToString())
                    {
                        case "2":
                            foreach (var item in relations)
                            {
                                if (item.tablaDestino == gridEntities.SelectedRows[0].Cells["id"].Value.ToString())
                                {
                                    MessageBox.Show("No se puede eliminar este atributo ya que tiene una relacion con otra tabla", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            break;
                        case "3":
                            for (int i = 0; i < relations.Count; i++)
                            {
                                if (relations[i].tablaOrigen == gridEntities.SelectedRows[0].Cells["id"].Value.ToString() &&
                                    relations[i].attrOrigen == gridAttributes.SelectedRows[0].Cells["id"].Value.ToString())
                                {
                                    relations.RemoveAt(i);
                                    FileStream file = new FileStream(fileName + ".rlt", FileMode.Create, FileAccess.Write, FileShare.None);
                                    file.Close();
                                    foreach (var item in relations)
                                    {
                                        NewRelationship(item.tablaOrigen, item.attrOrigen, item.tablaDestino);
                                    }
                                    break;
                                }
                            }
                            break;
                    }

                    foreach (Entity en in entities)
                    {
                        if (en.id == gridEntities.SelectedRows[0].Cells["id"].Value.ToString())
                        {
                            for (int i = 0; i < en.attributes.Count; i++)
                            {
                                if (en.attributes[i].id == gridAttributes.SelectedRows[0].Cells["id"].Value.ToString())
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
        }

        public void UpdateGridAttributes()
        {
            gridAttributes.Rows.Clear();
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
            foreach (var a in en.attributes)
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
                switch (a.indexType)
                {
                    case 0:
                        row.Cells["indiceString"].Value = "Sin tipo";
                        break;
                    case 2:
                        row.Cells["indiceString"].Value = "Llave Primaria";
                        break;
                    case 3:
                        row.Cells["indiceString"].Value = "Llave Foranea";
                        break;
                    case 5:
                        row.Cells["indiceString"].Value = "Hash";
                        break;
                }
            }
        }

        private void gridEntities_CellClick(object sender, GridViewCellEventArgs e)
        {
            string entity = gridEntities.SelectedRows[0].Cells["id"].Value.ToString();
            foreach (Entity en in entities)
            {
                if (en.id == entity)
                {
                    if (en.attributes.Count > 0)
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
                        switch (a.indexType)
                        {
                            case 0:
                                row.Cells["indiceString"].Value = "Sin tipo";
                                break;
                            case 2:
                                row.Cells["indiceString"].Value = "Llave Primaria";
                                break;
                            case 3:
                                row.Cells["indiceString"].Value = "Llave Foranea";
                                break;
                            case 5:
                                row.Cells["indiceString"].Value = "Hash";
                                break;
                        }
                    }
                    if (en.dataAddress != -1)
                    {
                        btnDataFile.Enabled = false;
                        allowWrite = false;
                        ReadRegisters(en);

                        //foreach (var a in en.attributes)
                        //{
                        //    switch (a.indexType)
                        //    {
                        //        case 3:
                        //            foreignKey = new frmForeignKey(a, path);
                        //            foreignKey.OpenFile();
                        //            break;
                        //        case 4:
                        //            binaryTree = new frmBinaryTree(a, path, fileName);
                        //            binaryTree.ExistingFile();
                        //            break;
                        //        case 5:
                        //            staticHash = new frmStaticHash(a, path);
                        //            staticHash.OpenFile();
                        //            break;
                        //    }
                        //}
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
            c.IsVisible = false;
            GridViewTextBoxColumn ca = new GridViewTextBoxColumn();
            ca.HeaderText = "Dirección";
            ca.Name = "dir";
            ca.ReadOnly = true;
            ca.Width = 75;
            ca.IsVisible = false;
            gridRegisters.MasterTemplate.Columns.Add(c);
            gridAddRegister.MasterTemplate.Columns.Add(ca);
            foreach (Attributes a in en.attributes)
            {
                string name = string.Empty;
                switch (a.indexType)
                {

                    case 2:
                        name = a.name + "(PK)";
                        break;
                    case 3:
                        name = a.name + "(FK)";
                        break;
                    default:
                        name = a.name;
                        break;
                }
                switch (a.dataType)
                {
                    case 'C':
                        size += a.length;
                        c = new GridViewTextBoxColumn();
                        c.HeaderText = name;
                        c.Name = a.name;
                        c.ReadOnly = false;
                        c.Width = 100;
                        c.DataType = typeof(string);
                        c.MaxLength = a.length;
                        ca = new GridViewTextBoxColumn();
                        ca.HeaderText = name;
                        ca.Name = a.name;
                        ca.ReadOnly = false;
                        ca.Width = 100;
                        ca.DataType = typeof(string);
                        ca.MaxLength = a.length;
                        gridRegisters.MasterTemplate.Columns.Add(c);
                        gridAddRegister.MasterTemplate.Columns.Add(ca);
                        break;
                    case 'E':
                        size += 4;
                        GridViewDecimalColumn d = new GridViewDecimalColumn();
                        d.DecimalPlaces = 0;
                        d.HeaderText = name;
                        d.Name = a.name;
                        d.Width = 100;
                        d.ReadOnly = false;
                        d.DataType = typeof(int);
                        GridViewDecimalColumn da = new GridViewDecimalColumn();
                        da.DecimalPlaces = 0;
                        da.HeaderText = name;
                        da.Name = a.name;
                        da.Width = 100;
                        da.ReadOnly = false;
                        da.DataType = typeof(int);
                        gridRegisters.MasterTemplate.Columns.Add(d);
                        gridAddRegister.MasterTemplate.Columns.Add(da);
                        break;
                    case 'D':
                        size += 4;
                        GridViewDecimalColumn e = new GridViewDecimalColumn();
                        e.DecimalPlaces = 4;
                        e.HeaderText = name;
                        e.Name = a.name;
                        e.Width = 100;
                        e.ReadOnly = false;
                        e.DataType = typeof(decimal);
                        e.Step = 0.0001m;
                        GridViewDecimalColumn ea = new GridViewDecimalColumn();
                        ea.DecimalPlaces = 4;
                        ea.HeaderText = name;
                        ea.Name = a.name;
                        ea.Width = 100;
                        ea.ReadOnly = false;
                        ea.DataType = typeof(decimal);
                        ea.Step = 0.0001m;
                        gridRegisters.MasterTemplate.Columns.Add(e);
                        gridAddRegister.MasterTemplate.Columns.Add(ea);
                        break;
                }
            }

            c = new GridViewTextBoxColumn();
            c.HeaderText = "Sig.registro";
            c.Name = "sig";
            c.ReadOnly = true;
            c.Width = 75;
            c.IsVisible = false;
            ca = new GridViewTextBoxColumn();
            ca.HeaderText = "Sig.registro";
            ca.Name = "sig";
            ca.ReadOnly = true;
            ca.Width = 75;
            ca.IsVisible = false;
            gridRegisters.MasterTemplate.Columns.Add(c);
            gridAddRegister.MasterTemplate.Columns.Add(ca);

            string p = Path.Combine(path, en.id + ".dat");
            FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryReader br = new BinaryReader(file);
            if (file.Length > 0)
            {
                long next = en.dataAddress;
                while (next != -1)
                {
                    GridViewRowInfo row = gridRegisters.Rows.AddNew();
                    file.Seek(next, SeekOrigin.Begin);
                    row.Cells[0].Value = br.ReadInt64().ToString();
                    foreach (Attributes a in en.attributes)
                    {
                        switch (a.dataType)
                        {
                            case 'C':
                                row.Cells[a.name].Value = Encoding.ASCII.GetString(br.ReadBytes(a.length));
                                break;
                            case 'E':
                                row.Cells[a.name].Value = br.ReadInt32();
                                break;
                            case 'D':
                                row.Cells[a.name].Value = br.ReadDecimal();
                                break;
                        }
                    }

                    next = br.ReadInt64();
                    row.Cells["sig"].Value = next.ToString();
                }
            }


            allowWrite = true;

            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            gridAddRegister.Rows[0].Cells[0].Value = file.Length;
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;
            file.Close();
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
                    c.IsVisible = false;
                    GridViewTextBoxColumn ca = new GridViewTextBoxColumn();
                    ca.HeaderText = "Dirección";
                    ca.Name = "dir";
                    ca.ReadOnly = true;
                    ca.Width = 50;
                    ca.IsVisible = false;
                    gridRegisters.MasterTemplate.Columns.Add(c);
                    gridAddRegister.MasterTemplate.Columns.Add(ca);
                    foreach (Attributes a in en.attributes)
                    {
                        string nameL = string.Empty;
                        switch (a.indexType)
                        {
                            case 2:
                                nameL = a.name + "(PK)";
                                break;
                            case 3:
                                nameL = a.name + "(FK)";
                                break;
                            default:
                                nameL = a.name;
                                break;
                        }
                        switch (a.dataType)
                        {
                            case 'C':
                                size += a.length;
                                c = new GridViewTextBoxColumn();
                                c.HeaderText = nameL;
                                c.Name = a.name;
                                c.ReadOnly = false;
                                c.Width = 100;
                                c.DataType = typeof(string);
                                c.MaxLength = a.length;
                                ca = new GridViewTextBoxColumn();
                                ca.HeaderText = nameL;
                                ca.Name = a.name;
                                ca.ReadOnly = false;
                                ca.Width = 100;
                                ca.DataType = typeof(string);
                                ca.MaxLength = a.length;
                                gridRegisters.MasterTemplate.Columns.Add(c);
                                gridAddRegister.MasterTemplate.Columns.Add(ca);
                                break;
                            case 'E':
                                size += 4;
                                GridViewDecimalColumn d = new GridViewDecimalColumn();
                                d.DecimalPlaces = 0;
                                d.HeaderText = nameL;
                                d.Name = a.name;
                                d.Width = 100;
                                d.ReadOnly = false;
                                d.DataType = typeof(int);
                                GridViewDecimalColumn da = new GridViewDecimalColumn();
                                da.DecimalPlaces = 0;
                                da.HeaderText = nameL;
                                da.Name = a.name;
                                da.Width = 100;
                                da.ReadOnly = false;
                                da.DataType = typeof(int);
                                gridRegisters.MasterTemplate.Columns.Add(d);
                                gridAddRegister.MasterTemplate.Columns.Add(da);
                                break;
                            case 'D':
                                size += 4;
                                GridViewDecimalColumn e1 = new GridViewDecimalColumn();
                                e1.DecimalPlaces = 4;
                                e1.HeaderText = nameL;
                                e1.Name = a.name;
                                e1.Width = 100;
                                e1.ReadOnly = false;
                                e1.DataType = typeof(decimal);
                                e1.Step = 0.0001m;
                                GridViewDecimalColumn ea = new GridViewDecimalColumn();
                                ea.DecimalPlaces = 4;
                                ea.HeaderText = nameL;
                                ea.Name = a.name;
                                ea.Width = 100;
                                ea.ReadOnly = false;
                                ea.DataType = typeof(decimal);
                                ea.Step = 0.0001m;
                                gridRegisters.MasterTemplate.Columns.Add(e1);
                                gridAddRegister.MasterTemplate.Columns.Add(ea);
                                break;
                        }

                    }
                    c = new GridViewTextBoxColumn();
                    c.HeaderText = "Sig.registro";
                    c.Name = "sig";
                    c.ReadOnly = true;
                    c.Width = 60;
                    c.IsVisible = false;
                    ca = new GridViewTextBoxColumn();
                    ca.HeaderText = "Sig.registro";
                    ca.Name = "sig";
                    ca.ReadOnly = true;
                    ca.Width = 60;
                    ca.IsVisible = false;
                    gridRegisters.MasterTemplate.Columns.Add(c);
                    gridAddRegister.MasterTemplate.Columns.Add(ca);
                }
            }

            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            gridAddRegister.Rows[0].Cells[0].Value = file.Length;
            file.Close();
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;
        }

        private Entity FindEntity(string id)
        {
            foreach (Entity e in entities)
            {
                if (e.id == id)
                    return e;
            }
            return null;
        }


        public void RegistersNextAddrs(Entity e)
        {
            allowWrite = false;
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
            //foreach (Attributes a in e.attributes)
            //{
            //    switch (a.indexType)
            //    {
            //        case 1:
            //            RegistersSearchKey(e);
            //            return;
            //        case 2:
            //            PrimaryKey(en);
            //            break;
            //    }
            //}
            if (gridRegisters.RowCount > 1)
            {
                for (int i = 0; i < gridRegisters.RowCount - 1; i++)
                {
                    gridRegisters.Rows[i].Cells[gridRegisters.ColumnCount - 1].Value = gridRegisters.Rows[i + 1].Cells[0].Value;
                    if (i + 1 == gridRegisters.RowCount - 1)
                    {
                        gridRegisters.Rows[i + 1].Cells[gridRegisters.ColumnCount - 1].Value = -1;
                    }
                }
            }
            allowWrite = true;
        }

        public void RegistersSearchKey(Entity e)
        {
            int indice = 1;
            int type = 0;
            foreach (Attributes a in e.attributes)
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
                            switch (en.attributes[j - 1].dataType)
                            {
                                case 'C':
                                    byte[] reg = new byte[en.attributes[j - 1].length];
                                    string val = Convert.ToString(gridRegisters.Rows[i].Cells[j].Value);
                                    int len = en.attributes[j - 1].length;
                                    Encoding.ASCII.GetBytes(val, 0, val.Length, reg, 0);
                                    bw.Write(reg);
                                    break;
                                case 'E':
                                    bw.Write(Convert.ToInt32(gridRegisters.Rows[i].Cells[j].Value.ToString()));
                                    break;
                                case 'D':
                                    bw.Write(Convert.ToDecimal(gridRegisters.Rows[i].Cells[j].Value.ToString()));
                                    break;
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
                    lr[k][lr[k].Count - 1] = lr[k + 1][0];
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
                if (String.IsNullOrWhiteSpace(Convert.ToString(gridAddRegister.Rows[0].Cells[i].Value)))
                {
                    MessageBox.Show("No se puede agregar un registro hasta que la informacion este completa", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int count = 1;
            foreach (var a in en.attributes)
            {
                if (a.indexType == 3)
                {
                    foreach (var item in relations)
                    {
                        if (item.tablaOrigen == en.id && item.attrOrigen == a.id)
                        {
                            if (!CheckData(FindEntity(item.tablaDestino), gridAddRegister.SelectedRows[0].Cells[count].Value))
                            {
                                MessageBox.Show("La clave foranea que estas instentando ingresar no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                count++;
            }

            for (int i = 0; i < gridRegisters.RowCount; i++)
            {
                bool repeated = true;
                for (int j = 1; j < gridRegisters.ColumnCount - 1; j++)
                {
                    if (gridAddRegister.Rows[0].Cells[j].Value.ToString() != gridRegisters.Rows[i].Cells[j].Value.ToString().Trim('\0'))
                        repeated = false;
                }
                if (repeated)
                {
                    MessageBox.Show("El Registro que estas intentando insertar ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            count = 1;
            foreach (var item in en.attributes)
            {
                if (item.indexType == 2)
                {
                    for (int i = 0; i < gridRegisters.RowCount; i++)
                    {
                        if (gridRegisters.Rows[i].Cells[count].Value.ToString() == gridAddRegister.Rows[0].Cells[count].Value.ToString())
                        {
                            MessageBox.Show("Esta clave primaria ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                count++;
            }

            GridViewRowInfo row = gridRegisters.Rows.AddNew();
            for (int i = 0; i < gridAddRegister.ColumnCount; i++)
            {
                row.Cells[i].Value = gridAddRegister.Rows[0].Cells[i].Value;
            }
            int size = 16;

            foreach (Attributes a in en.attributes)
            {
                if (a.dataType == 'C')
                    size += a.length;
                else
                    size += 4;
            }
            gridAddRegister.Rows.Clear();
            gridAddRegister.Rows.AddNew();
            file.Close();
            gridAddRegister.Rows[0].Cells[gridAddRegister.ColumnCount - 1].Value = -1;

            count = 1;
            foreach (var a in en.attributes)
            {
                //switch (a.indexType)
                //{
                //    case 3:
                //        foreignKey.Insert(row.Cells[count].Value, Convert.ToInt64(row.Cells[0].Value));
                //        break;
                //    case 4:
                //        binaryTree.Insert(Convert.ToInt32(row.Cells[count].Value), Convert.ToInt64(row.Cells[0].Value));
                //        break;
                //    case 5:
                //        staticHash.Insert(Convert.ToInt32(row.Cells[count].Value), Convert.ToInt64(row.Cells[0].Value));
                //        break;
                //}
                count++;
            }
            RegistersNextAddrs(en);
            WriteRegister(size);
            UpdateGridAttributes();

            string p = Path.Combine(path, en.id + ".dat");
            file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryReader br = new BinaryReader(file);
            gridAddRegister.Rows[0].Cells[0].Value = file.Length;
            file.Close();
        }

        private bool CheckData(Entity en, object val)
        {
            bool result = false;

            if (File.Exists(Path.Combine(path, en.id + ".dat")))
            {
                List<List<object>> registers = new List<List<object>>();
                string p = Path.Combine(path, en.id + ".dat");
                FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader br = new BinaryReader(file);
                if (file.Length > 0)
                {
                    long next = en.dataAddress;
                    while (next != -1)
                    {
                        List<object> lo = new List<object>();
                        file.Seek(next, SeekOrigin.Begin);
                        lo.Add(br.ReadInt64().ToString());
                        foreach (Attributes a in en.attributes)
                        {
                            switch (a.dataType)
                            {
                                case 'C':
                                    lo.Add(Encoding.ASCII.GetString(br.ReadBytes(a.length)));
                                    break;
                                case 'E':
                                    lo.Add(br.ReadInt32());
                                    break;
                                case 'D':
                                    lo.Add(br.ReadDecimal());
                                    break;
                            }
                        }

                        next = br.ReadInt64();
                        lo.Add(next.ToString());
                        registers.Add(lo);
                    }
                }
                file.Close();
                int count = 1;
                Attributes attr = new Attributes();
                foreach (var at in en.attributes)
                {
                    if (at.indexType == 2)
                    {
                        attr = at;
                        break;
                    }
                    count++;
                }
                foreach (var list in registers)
                {
                    switch (attr.dataType)
                    {
                        case 'C':
                            if (list[count].ToString() == val.ToString())
                                return true;
                            break;
                        case 'E':
                            if (Convert.ToInt32(list[count]) == Convert.ToInt32(val))
                                return true;
                            break;
                        case 'D':
                            if (Convert.ToDecimal(list[count]) == Convert.ToDecimal(val))
                                return true;
                            break;
                    }
                }
            }

            return result;
        }

        private void ModifyRegisters(Entity en, string attrID, object newValue, object oldValue)
        {
            if (File.Exists(Path.Combine(path, en.id + ".dat")))
            {
                List<List<object>> registers = new List<List<object>>();
                string p = Path.Combine(path, en.id + ".dat");
                FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader br = new BinaryReader(file);
                if (file.Length > 0)
                {
                    long next = en.dataAddress;
                    while (next != -1)
                    {
                        List<object> lo = new List<object>();
                        file.Seek(next, SeekOrigin.Begin);
                        lo.Add(br.ReadInt64().ToString());
                        foreach (Attributes a in en.attributes)
                        {
                            switch (a.dataType)
                            {
                                case 'C':
                                    lo.Add(Encoding.ASCII.GetString(br.ReadBytes(a.length)));
                                    break;
                                case 'E':
                                    lo.Add(br.ReadInt32());
                                    break;
                                case 'D':
                                    lo.Add(br.ReadDecimal());
                                    break;
                            }
                        }

                        next = br.ReadInt64();
                        lo.Add(next.ToString());
                        registers.Add(lo);
                    }
                }
                file.Close();
                int count = 1;
                Attributes at = new Attributes();
                foreach (var item in en.attributes)
                {
                    if (item.id == attrID)
                    {
                        at = item;
                        break;
                    }
                    count++;
                }
                foreach (List<object> list in registers)
                {
                    switch (at.dataType)
                    {
                        case 'C':
                            if (list[count].ToString() == oldValue.ToString())
                            {
                                list[count] = newValue.ToString();
                                WriteRegister(list, en);
                            }
                            break;
                        case 'E':
                            if (Convert.ToInt32(list[count]) == Convert.ToInt32(oldValue))
                            {
                                list[count] = Convert.ToInt32(newValue);
                                WriteRegister(list, en);
                            }
                            break;
                        case 'D':
                            if (Convert.ToDecimal(list[count]) == Convert.ToDecimal(oldValue))
                            {
                                list[count] = Convert.ToDecimal(newValue);
                                WriteRegister(list, en);
                            }
                            break;
                    }
                }
            }
        }

        private void WriteRegister(List<object> lo, Entity en)
        {
            string p = Path.Combine(path, en.id + ".dat");
            FileStream file = new FileStream(p, FileMode.Open, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            file.Seek(Convert.ToInt64(lo[0]), SeekOrigin.Begin);
            for (int j = 0; j < lo.Count; j++)
            {
                if (j == 0 || j == lo.Count - 1)
                {
                    long id;
                    if (Convert.ToString(lo[j]) != "")
                        id = Convert.ToInt64(lo[j]);
                    else
                        id = 0;

                    bw.Write(id);
                }
                else
                {
                    switch (en.attributes[j - 1].dataType)
                    {
                        case 'C':
                            byte[] reg = new byte[en.attributes[j - 1].length];
                            string val = Convert.ToString(lo[j]);
                            int len = en.attributes[j - 1].length;
                            Encoding.ASCII.GetBytes(val, 0, val.Length, reg, 0);
                            bw.Write(reg);
                            break;
                        case 'E':
                            bw.Write(Convert.ToInt32(lo[j]));
                            break;
                        case 'D':
                            bw.Write(Convert.ToDecimal(lo[j]));
                            break;
                    }
                }
            }
            file.Close();

        }

        private void gridRegisters_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            List<object> newRegister = new List<object>();
            for (int i = 0; i < gridRegisters.ColumnCount; i++)
            {
                newRegister.Add(e.Row.Cells[i].Value);
            }
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells[0].Value.ToString());
            int count = 1;
            foreach (Attributes a in en.attributes)
            {
                switch (a.indexType)
                {
                    case 2:
                        List<Entity> checkList = new List<Entity>();
                        foreach (var item in relations)
                        {
                            if (item.tablaDestino == en.id)
                            {
                                Entity auxEnt = FindEntity(item.tablaOrigen);
                                ModifyRegisters(auxEnt, item.attrOrigen, newRegister[count], oldRegister[count]);
                            }
                        }

                        break;
                    case 3:
                        break;
                }
                //switch (a.indexType)
                //{
                //    case 1://Clave de busqueda
                //        RegistersNextAddrs(en);
                //        break;
                //    case 3://Clave secundaria
                //        if (oldRegister[count].ToString() != newRegister[count].ToString())
                //        {
                //            foreignKey.Delete(oldRegister[count], Convert.ToInt64(oldRegister[0]));
                //            foreignKey.Insert(newRegister[count], Convert.ToInt64(newRegister[0]));
                //        }
                //        break;
                //    case 4://Arbol binario
                //        if (Convert.ToInt32(oldRegister[count]) != Convert.ToInt32(newRegister[count]))
                //        {
                //            binaryTree.Delete(Convert.ToInt32(oldRegister[count]), Convert.ToInt64(oldRegister[0]));
                //            binaryTree.Insert(Convert.ToInt32(newRegister[count]), Convert.ToInt64(newRegister[0]));
                //        }
                //        break;
                //    case 5://Hash estatica
                //        if (Convert.ToInt32(oldRegister[count]) != Convert.ToInt32(newRegister[count]))
                //        {
                //            staticHash.Delete(Convert.ToInt32(oldRegister[count]), Convert.ToInt64(oldRegister[0]));
                //            staticHash.Insert(Convert.ToInt32(newRegister[count]), Convert.ToInt64(newRegister[0]));
                //        }
                //        break;
                //}
                count++;
            }
            int size = 16;

            foreach (Attributes a in en.attributes)
            {
                if (a.dataType == 'C')
                    size += a.length;
                else
                    size += 4;
            }

            WriteRegister(size);
        }

        private void DeletePrimaryReg(Entity ent, Attributes attr, object val)
        {
            List<string> ents = new List<string>();
            List<string> attrs = new List<string>();
            foreach (var item in relations)
            {
                if (item.tablaDestino == ent.id)
                {
                    ents.Add(item.tablaOrigen);
                    attrs.Add(item.attrOrigen);
                }
            }
            for (int i = 0; i < ents.Count; i++)
            {
                Entity en = FindEntity(ents[i]);
                if (File.Exists(Path.Combine(path, en.id + ".dat")))
                {
                    List<List<object>> registers = new List<List<object>>();
                    string p = Path.Combine(path, en.id + ".dat");
                    FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
                    BinaryReader br = new BinaryReader(file);
                    if (file.Length > 0)
                    {
                        long next = en.dataAddress;
                        while (next != -1)
                        {
                            List<object> lo = new List<object>();
                            file.Seek(next, SeekOrigin.Begin);
                            lo.Add(br.ReadInt64().ToString());
                            foreach (Attributes a in en.attributes)
                            {
                                switch (a.dataType)
                                {
                                    case 'C':
                                        lo.Add(Encoding.ASCII.GetString(br.ReadBytes(a.length)));
                                        break;
                                    case 'E':
                                        lo.Add(br.ReadInt32());
                                        break;
                                    case 'D':
                                        lo.Add(br.ReadDecimal());
                                        break;
                                }
                            }

                            next = br.ReadInt64();
                            lo.Add(next.ToString());
                            registers.Add(lo);
                        }
                    }
                    file.Close();
                    int indexer = 1;
                    Attributes at = new Attributes();
                    foreach (var a in en.attributes)
                    {
                        if (a.id == attrs[i])
                        {
                            at = a;
                            break;
                        }
                        indexer++;
                    }
                    List<int> deleteIndex = new List<int>();
                    for (int j = 0; j < registers.Count; j++)
                    {
                        switch (at.dataType)
                        {
                            case 'C':
                                if (registers[j][indexer].ToString() == val.ToString())
                                    deleteIndex.Add(i);
                                break;
                            case 'E':
                                if (Convert.ToInt32(registers[j][indexer]) == Convert.ToInt32(val))
                                    deleteIndex.Add(i);
                                break;
                            case 'D':
                                if (Convert.ToDecimal(registers[j][indexer]) == Convert.ToDecimal(val))
                                    deleteIndex.Add(i);
                                break;
                        }
                    }
                    foreach (var del in deleteIndex)
                    {
                        registers.RemoveAt(del);
                    }
                    if (registers.Count > 0)
                        en.dataAddress = Convert.ToInt64(registers[0][0]);
                    else
                        en.dataAddress = -1;
                    WriteEntity(en);
                    p = Path.Combine(path, en.id + ".dat");
                    file = new FileStream(p, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                    file.Close();
                    RegistersNextAddrs(en, registers);
                    foreach (var reg in registers)
                    {
                        WriteRegister(reg, en);
                    }
                }
            }
        }

        public void RegistersNextAddrs(Entity e, List<List<Object>> lo)
        {
            if (lo.Count > 1)
            {
                for (int i = 0; i < lo.Count - 1; i++)
                {
                    lo[i][lo[i].Count - 1] = lo[i + 1][0];
                    if (i + 1 == lo.Count - 1)
                    {
                        lo[i + 1][lo[i].Count - 1] = -1;
                    }
                }
            }
        }

        private void btnDelReg_Click(object sender, EventArgs e)
        {
            if (gridRegisters.RowCount > 0)
            {
                for (int i = 0; i < gridRegisters.RowCount; i++)
                {
                    if (gridRegisters.SelectedRows[0].Cells[0].Value == gridRegisters.Rows[i].Cells[0].Value)
                    {
                        Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
                        int indexer = 1;
                        foreach (var a in en.attributes)
                        {
                            switch (a.indexType)
                            {
                                case 2:
                                    if (MessageBox.Show("Esta seguro que desea eliminar este registro con PK? Todos los registros que tengan relacion con este tambien seran eliminados", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                    {
                                        DeletePrimaryReg(en, a, gridRegisters.SelectedRows[0].Cells[indexer].Value);
                                    }
                                    else
                                        return;
                                    break;
                                    //case 4:
                                    //    binaryTree.Delete(Convert.ToInt32(gridRegisters.SelectedRows[0].Cells[indexer].Value), Convert.ToInt64(gridRegisters.SelectedRows[0].Cells[0].Value));
                                    //    break;

                                    //case 3:
                                    //    foreignKey.Delete(gridRegisters.SelectedRows[0].Cells[indexer].Value, Convert.ToInt64(gridRegisters.SelectedRows[0].Cells[0].Value));
                                    //    break;

                                    //case 5:
                                    //    staticHash.Delete(Convert.ToInt32(gridRegisters.SelectedRows[0].Cells[indexer].Value), Convert.ToInt64(gridRegisters.SelectedRows[0].Cells[0].Value));
                                    //    break;
                            }
                            indexer++;
                        }
                        gridRegisters.Rows.RemoveAt(i);
                        int size = 16;

                        foreach (Attributes a in en.attributes)
                        {
                            if (a.dataType == 'C')
                                size += a.length;
                            else
                                size += 4;
                        }
                        //for (int j = 0; j < gridRegisters.RowCount; j++)
                        //{
                        //    gridRegisters.Rows[j].Cells[0].Value = j * size;
                        //}
                        if (gridRegisters.RowCount > 0)
                            en.dataAddress = Convert.ToInt64(gridRegisters.Rows[0].Cells[0].Value);
                        else
                            en.dataAddress = -1;
                        WriteEntity(en);
                        gridEntities.SelectedRows[0].Cells["dataAddress"].Value = en.dataAddress;
                        string p = Path.Combine(path, en.id + ".dat");
                        FileStream file = new FileStream(p, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                        file.Close();
                        RegistersNextAddrs(en);
                        WriteRegister(size);
                        gridEntities_CellClick(this, null);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error", "Antes de eliminar un registro, agrega uno o abre un archivo que contenga registros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public int NextAddress(int size)
        {
            int max = 0;
            for (int i = 0; i < gridRegisters.RowCount; i++)
            {
                if (Convert.ToInt64(gridRegisters.Rows[i].Cells[0].Value) > max)
                {
                    max = Convert.ToInt32(gridRegisters.Rows[i].Cells[0].Value);
                }
            }
            return max + size;
        }

        private void btnPK_Click(object sender, EventArgs e)
        {
            bool pk = false;
            Entity en = FindEntity(gridEntities.SelectedRows[0].Cells["id"].Value.ToString());
            foreach (var a in en.attributes)
            {
                if (a.indexType == 2)
                {
                    pk = true;
                    break;
                }
            }
            if (pk)
            {
                frmPrimaryKey fpk = new frmPrimaryKey(PrimaryKey(en));
                fpk.ShowDialog();
                fpk.Dispose();
            }
            else
            {
                MessageBox.Show("Esta entidad no contiene ningun atributo de tipo índice primario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private List<Register> PrimaryKey(Entity en)
        {
            string p = Path.Combine(path, en.id + ".pk");
            FileStream file = new FileStream(p, FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            int indexPK = 1;
            int size = 8;
            int sizeDat = 0;
            int type = 0; ;
            foreach (var a in en.attributes)
            {
                if (a.indexType == 2)
                {
                    size += a.length;
                    sizeDat = a.length;
                    if (a.dataType == 'C')
                        type = 0;
                    else
                        type = 1;
                    break;
                }
                else
                    indexPK++;
            }
            List<Register> lpk = new List<Register>();
            for (int i = 0; i < gridRegisters.RowCount; i++)
            {
                Register r = new Register();
                r.dir = Convert.ToInt64(gridRegisters.Rows[i].Cells[0].Value);
                r.val = gridRegisters.Rows[i].Cells[indexPK].Value.ToString();
                lpk.Add(r);
            }
            if (type == 0)
                lpk = lpk.OrderBy(o => o.val).ToList();
            else
            {
                lpk = lpk.OrderBy(o => Convert.ToInt32(o.val)).ToList();
            }
            int maxReg = (int)Math.Floor(2048.0 / Convert.ToDouble(size));
            int sizePK = (int)Math.Ceiling(Convert.ToDouble(gridRegisters.RowCount) / Convert.ToDouble(maxReg));
            sizePK = sizePK * 2048;

            foreach (Register r in lpk)
            {
                bw.Write(r.dir);
                if (type == 0)
                {
                    byte[] name = new byte[sizeDat];
                    Encoding.ASCII.GetBytes(Convert.ToString(r.val), 0, Convert.ToString(r.val).Length, name, 0);
                    bw.Write(name);
                }
                else
                {
                    bw.Write(Convert.ToInt32(r.val));
                }
            }
            bw.Write(Convert.ToInt64(-1));
            byte[] bPK = new byte[sizePK - file.Length];
            bw.Write(bPK);
            bw.Close();
            file.Close();
            return lpk;
        }

        private void btnFK_Click(object sender, EventArgs e)
        {
            if (foreignKey != null)
            {
                foreignKey.ShowDialog();
            }
        }

        private void btnBinaryTree_Click(object sender, EventArgs e)
        {
            if (binaryTree != null)
            {
                binaryTree.ShowDialog();
            }
        }

        private void btnHash_Click(object sender, EventArgs e)
        {
            if (staticHash != null)
            {
                staticHash.ShowDialog();
            }
        }

        private void gridRegisters_CellClick(object sender, GridViewCellEventArgs e)
        {
            oldRegister = new List<object>();
            for (int i = 0; i < gridRegisters.ColumnCount; i++)
            {
                oldRegister.Add(e.Row.Cells[i].Value);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro que desea eliminar esta BD?", "Eliminar BD", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var en in entities)
                {
                    DeleteEntity(en.id);
                    foreach (var a in en.attributes)
                    {
                        DeleteAttribute(a.id);
                    }
                }
                File.Delete(fileName);
                File.Delete(fileName + ".rlt");
                ClearData();
            }
            else
                return;
        }

        private void DeleteEntity(string name)
        {
            File.Delete(path + "\\" + name + ".dat");
        }
        private void DeleteAttribute(string name)
        {
            File.Delete(path + "\\" + name + ".pk");
            File.Delete(path + "\\" + name + ".fk");
        }

        private void btnModifyDB_Click(object sender, EventArgs e)
        {
            frmModifyName frmMN = new frmModifyName();
            frmMN.name = Path.GetFileName(fileName);
            if (frmMN.ShowDialog() == DialogResult.OK)
            {
                string fileNameAux = path + "\\" + frmMN.name;
                File.Move(fileName, fileNameAux);
                fileName = fileNameAux;
            }
        }

        private void txtSQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnExec_Click(this, null);
        }

        private string TableExists(string name)
        {
            foreach (var en in entities)
            {
                if (en.name.ToLower() == name)
                    return en.id;
            }
            return string.Empty;
        }

        private string FormatText(string text)
        {
            string txt = text;
            List<int> remove = new List<int>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(' || text[i] == ')' || text[i] == ',')
                    remove.Add(i);
            }
            for (int i = remove.Count - 1; i >= 0; i--)
            {
                txt = txt.Remove(remove[i], 1);
            }
            return txt;
        }
        string Table = string.Empty;
        string TableJoin = string.Empty;
        List<string> attrTable = new List<string>();
        List<string> attrJoin = new List<string>();
        string condicion;
        string condicionJoin;
        Entity enJoin = new Entity();
        Entity enTable = new Entity();
        public bool SeparateText(string text)
        {
            text = FormatText(text);
            Table = string.Empty;
            TableJoin = string.Empty;
            attrTable = new List<string>();
            attrJoin = new List<string>();
            condicion = string.Empty;
            condicionJoin = string.Empty;
            enJoin = new Entity();
            enTable = new Entity();
            List<string> auxAttr = new List<string>();
            int remove = 0;

            List<string> texto = text.Split(' ').ToList();
            texto.RemoveAt(0);

            for (int i = 0; i < texto.Count; i++)
            {
                if (texto[i] == "from")
                {
                    remove = i;
                    break;
                }
                auxAttr.Add(texto[i]);
            }
            for (int i = remove; i >= 0; i--)
                texto.RemoveAt(i);
            Table = texto[0];
            texto.RemoveAt(0);
            if (texto.Count > 0)
            {
                if (texto[0] == "inner")
                    texto.RemoveAt(0);
                if (texto[0] == "join")
                {
                    texto.RemoveAt(0);
                    TableJoin = texto[0];
                    texto.RemoveAt(0);
                    string cond = string.Empty;
                    if (texto[0] == "on")
                    {
                        texto.RemoveAt(0);
                    }
                    else
                    {
                        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    int index = 0;
                    for (; index < texto.Count && texto[index] != "where"; index++)
                    {
                        cond += texto[index] + " ";
                    }
                    condicionJoin = cond;
                    cond = string.Empty;
                    for (int i = index - 1; i >= 0; i--)
                    {
                        texto.RemoveAt(i);
                    }
                }
            }
            if (texto.Count > 0)
            {
                if (texto[0] == "where")
                {
                    string cond = string.Empty;
                    texto.RemoveAt(0);
                    int index = 0;
                    for (; index < texto.Count; index++)
                    {
                        cond += texto[index] + " ";
                    }
                    condicion = cond;
                    for (int i = 0; i < index; i++)
                    {
                        texto.RemoveAt(0);
                    }
                }
            }
            if (texto.Count > 0)
            {
                MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                foreach (var item in auxAttr)
                {
                    if (item == "*")
                    {
                        attrJoin.Add(item);
                        attrTable.Add(item);
                        break;
                    }
                    if (item.Contains("."))
                    {
                        List<string> attr = item.Split('.').ToList();
                        if (attr.Count == 2)
                        {
                            if (attr[0] == Table)
                            {
                                if(attrExists(FindEntity(TableExists(Table)), attr[1]) && !attrTable.Contains(attr[1]))
                                    attrTable.Add(attr[1]);
                                else
                                {
                                    MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                            }
                            else
                            {
                                if (attr[0] == TableJoin)
                                {
                                    if (attrExists(FindEntity(TableExists(TableJoin)), attr[1]) && !attrJoin.Contains(attr[1]))
                                        attrJoin.Add(attr[1]);
                                    else
                                    {
                                        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Query invalido, no se reconoce la tabla '" + attr[0] + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {

                        if (attrExists(FindEntity(TableExists(Table)), item))
                        {
                            if (!string.IsNullOrWhiteSpace(TableJoin) && attrExists(FindEntity(TableExists(TableJoin)), item))
                            {
                                MessageBox.Show("Query invalido, el atributo '" + item + "' existe en multiples tablas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            if(attrTable.Contains(item))
                            {
                                MessageBox.Show("Query invalido, el atributo '" + item + "' se repite", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                                attrTable.Add(item);
                        }
                        else
                        {
                            if ( !string.IsNullOrWhiteSpace(TableJoin) && attrExists(FindEntity(TableExists(TableJoin)), item))
                                attrJoin.Add(item);
                            else
                            {
                                MessageBox.Show("Query invalido, el atributo '" + item + "' no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
                enTable = FindEntity(TableExists(Table));
                if (enTable == null)
                {
                    MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (TableJoin != string.Empty)
                {
                    enJoin = FindEntity(TableExists(TableJoin));
                    if (enJoin == null)
                    {
                        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                //if(!(attrTable.Contains("*")))
                //{
                //    if (!attrExists(enTable, attrTable))
                //    {
                //        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
                if (enJoin != null && !(attrTable.Contains("*")))
                {
                    if (!attrExists(enJoin, attrJoin))
                    {
                        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            if(!string.IsNullOrWhiteSpace(condicionJoin))
            {
                List<string> lt = condicionJoin.Trim().Split(' ').ToList();
                if(lt.Count == 3 && lt[1] == "=")
                {
                    if(!attrExists(FindEntity(TableExists(Table)), lt[0]) )
                    {
                        MessageBox.Show("Query invalido, no existe el atributo '" + lt[0] + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (!attrExists(FindEntity(TableExists(TableJoin)), lt[2]))
                    {
                        MessageBox.Show("Query invalido, no existe el atributo '" + lt[2] + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool attrExists(Entity en, List<string> attr)
        {
            bool result = true;
            foreach (var at in attr)
            {
                bool find = false;
                foreach (var a in en.attributes)
                {
                    if (a.name.ToLower() == at)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            }
            return result;
        }
        private bool attrExists(Entity en, string at)
        {
            bool result = true;
                bool find = false;
                foreach (var a in en.attributes)
                {
                    if (a.name.ToLower() == at)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            return result;
        }
        private void btnExec_Click(object sender, EventArgs e)
        {
            string text = txtSQL.Text.ToLower();
            text = text.Trim(new char[] { '(', ')', ',' });

            string join = string.Empty;
            List<string> atributos = new List<string>();
            string condicion = string.Empty;
            string joinCond = string.Empty;
            List<string> ls = new List<string>();
            ls = text.Split(' ').ToList();
            if (ls.Count >= 4)
            {
                if (ls[0] == "select")
                {
                    if (ls.Contains("from"))
                    {

                        string entID = string.Empty;
                        for (int i = ls.Count - 1; i >= 1; i--)
                        {
                            if (ls[i - 1] == "from")
                                entID = ls[i];
                            if (ls[i - 1] == "join")
                                join = ls[i];
                        }
                        if (ls.Contains("where"))
                        {
                            int index = ls.IndexOf("where");
                            index++;
                            for (; index < ls.Count; index++)
                            {
                                condicion += ls[index] + " ";
                            }
                            condicion = condicion.TrimEnd();
                            if (condicion.Split(' ').Count() != 3)
                            {
                                MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            switch (condicion.Split(' ')[1])
                            {
                                case "=":
                                case "<":
                                case ">":
                                case ">=":
                                case "<=":
                                case "<>":
                                    break;
                                default:
                                    MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                            }
                            int valorNumerico;
                            if (!int.TryParse(condicion.Split(' ')[2], out valorNumerico))
                            {
                                MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (!int.TryParse(condicion.Split(' ')[2], out valorNumerico))
                            {
                                MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        Entity en = FindEntity(TableExists(entID));
                        if (en == null)
                        {
                            MessageBox.Show("No existe la tabla '" + ls[ls.Count - 1] + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        for (int i = 1; i < ls.Count && ls[i] != "from"; i++)
                        {
                            atributos.Add(ls[i]);
                        }
                        if (!SeparateText(text))
                            return;
                        List<List<Object>> registers = ExecuteSQLSimple(enTable, attrTable, condicion, attrJoin, condicionJoin, enJoin);
                        //List<List<Object>> registers = ExecuteSQLSimple(en, atributos, condicion);

                        gridResultSQL.Rows.Clear();
                        gridResultSQL.Columns.Clear();
                        if (atributos.Contains("*"))
                        {
                            foreach (var r in en.attributes)
                            {
                                
                                GridViewTextBoxColumn c = new GridViewTextBoxColumn();
                                c.HeaderText = r.name;
                                c.Name = r.name;
                                c.ReadOnly = true;
                                c.Width = 100;
                                c.DataType = typeof(string);
                                gridResultSQL.MasterTemplate.Columns.Add(c);

                            }
                            if(enJoin != null)
                            {
                                foreach (var r in enJoin.attributes)
                                {
                                    int longitud = 7;
                                    Guid miGuid = Guid.NewGuid();
                                    string token = miGuid.ToString().Replace("-", string.Empty).Substring(0, longitud);
                                    GridViewTextBoxColumn c = new GridViewTextBoxColumn();
                                    c.HeaderText = r.name;
                                    c.Name = token;
                                    c.ReadOnly = true;
                                    c.Width = 100;
                                    c.DataType = typeof(string);
                                    gridResultSQL.MasterTemplate.Columns.Add(c);

                                }
                            }
                        }
                        else
                        {
                            foreach (var r in attrTable)
                            {
                                GridViewTextBoxColumn c = new GridViewTextBoxColumn();
                                c.HeaderText = r;
                                c.Name = r;
                                c.ReadOnly = true;
                                c.Width = 100;
                                c.DataType = typeof(string);
                                gridResultSQL.MasterTemplate.Columns.Add(c);
                            }
                            if(enJoin != null)
                            {
                                foreach (var r in attrJoin)
                                {
                                    int longitud = 7;
                                    Guid miGuid = Guid.NewGuid();
                                    string token = miGuid.ToString().Replace("-", string.Empty).Substring(0, longitud);
                                    GridViewTextBoxColumn c = new GridViewTextBoxColumn();
                                    c.HeaderText = r;
                                    c.Name = token;
                                    c.ReadOnly = true;
                                    c.Width = 100;
                                    c.DataType = typeof(string);
                                    gridResultSQL.MasterTemplate.Columns.Add(c);
                                }
                            }
                        }
                        foreach (var reg in registers)
                        {
                            GridViewRowInfo row = gridResultSQL.Rows.AddNew();
                            for (int i = 0; i < reg.Count; i++)
                            {
                                row.Cells[i].Value = reg[i].ToString();
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("No se reconoce el comando '" + text.Split(' ')[0] + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Query invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private List<List<Object>> ExecuteSQLSimple(Entity en, List<string> atributos, string condicion = null, List<string> atributosJoin = null, string condicionJoin = null, Entity enJoin = null)
        {
            List<List<Object>> result = new List<List<object>>();
            if (File.Exists(Path.Combine(path, en.id + ".dat")))
            {
                List<List<object>> registers = new List<List<object>>();
                string p = Path.Combine(path, en.id + ".dat");
                FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader br = new BinaryReader(file);
                if (file.Length > 0)
                {
                    long next = en.dataAddress;
                    while (next != -1)
                    {
                        List<object> lo = new List<object>();
                        file.Seek(next, SeekOrigin.Begin);
                        lo.Add(br.ReadInt64().ToString());
                        foreach (Attributes a in en.attributes)
                        {
                            switch (a.dataType)
                            {
                                case 'C':
                                    lo.Add(Encoding.ASCII.GetString(br.ReadBytes(a.length)));
                                    break;
                                case 'E':
                                    lo.Add(br.ReadInt32());
                                    break;
                                case 'D':
                                    lo.Add(br.ReadDecimal());
                                    break;
                            }
                        }

                        next = br.ReadInt64();
                        lo.Add(next.ToString());
                        registers.Add(lo);
                    }
                }
                file.Close();
                List<string> cond = condicion.Split(' ').ToList();
                int index = 1;
                int indexL = -1;
                int indexR = -1;

                List<int> remove = new List<int>();
                if (!string.IsNullOrWhiteSpace(condicion))
                {
                    foreach (var a in en.attributes)
                    {
                        if (a.name.ToLower() == cond[0])
                        {
                            indexL = index;
                        }
                        if (a.name.ToLower() == cond[2])
                        {
                            indexR = index;
                        }
                        index++;
                    }
                    for (int i = 0; i < registers.Count; i++)
                    {
                        switch (cond[1])
                        {
                            case "=":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) == Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) == Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) == Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) == Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                            case ">":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) > Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) > Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) > Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) > Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                            case "<":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) < Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) < Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) < Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) < Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                            case ">=":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) >= Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) >= Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) >= Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) >= Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                            case "<=":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) <= Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) <= Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) <= Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) <= Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                            case "<>":
                                if (indexL != -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) != Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR != -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) != Convert.ToDecimal(registers[i][indexR])))
                                        remove.Add(i);
                                }
                                if (indexL != -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(registers[i][indexL]) != Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                if (indexL == -1 && indexR == -1)
                                {
                                    if (!(Convert.ToDecimal(cond[0]) != Convert.ToDecimal(cond[2])))
                                        remove.Add(i);
                                }
                                break;
                        }
                    }
                    for (int i = remove.Count - 1; i >= 0; i--)
                    {
                        registers.RemoveAt(remove[i]);
                    }
                    remove.Clear();
                }
                for (int i = 0; i < registers.Count; i++)
                {
                    registers[i].RemoveAt(0);
                    registers[i].RemoveAt(registers[i].Count - 1);
                }
                if (enJoin != null && !string.IsNullOrWhiteSpace(enJoin.id))
                    registers = ExecuteJoin(registers, enJoin, attrJoin, condicionJoin, en);
                if (enJoin != null)
                {
                    if (!attrJoin.Contains("*"))
                    {
                        index = 0;
                        remove.Clear();
                        foreach (var a in enJoin.attributes)
                        {
                            if (!attrJoin.Contains(a.name.ToLower()))
                                remove.Add(index);
                            index++;
                        }
                        remove.Sort();
                        for (int i = 0; i < registers.Count; i++)
                        {
                            for (int j = remove.Count - 1; j >= 0; j--)
                            {
                                registers[i].RemoveAt(en.attributes.Count + remove[j]);
                            }
                        }
                    }
                }
                if (!atributos.Contains("*"))
                {
                    
                    remove.Clear();
                    index = 0;
                    foreach (var a in en.attributes)
                    {
                        if (!atributos.Contains(a.name.ToLower()))
                            remove.Add(index);
                        index++;
                    }
                    remove.Sort();
                    for (int i = 0; i < registers.Count; i++)
                    {
                        for (int j = remove.Count - 1; j >= 0; j--)
                        {
                            registers[i].RemoveAt(remove[j]);
                        }
                    }
                }


                result = registers;
            }
            return result;
        }

        private List<List<Object>> CopyList (List<List<Object>> l)
        {
            List<List<Object>> result = new List<List<object>>();
            foreach (var item in l)
            {
                List<Object> lo = new List<object>();
                foreach (var o in item)
                {
                    Object ob = new object();
                    ob = o;
                    lo.Add(ob);
                }
                result.Add(lo);
            }
            return result;
        }

        private List<List<Object>> ExecuteJoin(List<List<Object>> regs, Entity enJoin, List<string> attrJoin, string condicion, Entity eTable)
        {
            List<List<Object>> result = new List<List<object>>();
            if (File.Exists(Path.Combine(path, enJoin.id + ".dat")))
            {
                List<List<object>> registers = new List<List<object>>();
                string p = Path.Combine(path, enJoin.id + ".dat");
                FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader br = new BinaryReader(file);
                if (file.Length > 0)
                {
                    long next = enJoin.dataAddress;
                    while (next != -1)
                    {
                        List<object> lo = new List<object>();
                        file.Seek(next, SeekOrigin.Begin);
                        lo.Add(br.ReadInt64().ToString());
                        foreach (Attributes a in enJoin.attributes)
                        {
                            switch (a.dataType)
                            {
                                case 'C':
                                    lo.Add(Encoding.ASCII.GetString(br.ReadBytes(a.length)));
                                    break;
                                case 'E':
                                    lo.Add(br.ReadInt32());
                                    break;
                                case 'D':
                                    lo.Add(br.ReadDecimal());
                                    break;
                            }
                        }

                        next = br.ReadInt64();
                        lo.Add(next.ToString());
                        registers.Add(lo);
                    }
                }
                file.Close();
                List<string> cond = condicion.Trim().Split(' ').ToList();
                foreach (var item in registers)
                {
                    item.RemoveAt(0);
                    item.RemoveAt(item.Count - 1);
                }
                
                if (cond.Count == 3)
                {
                    int indexTable = -1;
                    int indexJoin = -1;
                    int index = 0;
                    if (cond[2].Contains("."))
                    {
                        cond[2] = cond[2].Split('.')[1];
                    }
                    foreach (var item in enJoin.attributes)
                    {
                        if (item.name.ToLower() == cond[2])
                            break;
                        index++;
                    }
                    indexJoin = index;
                    
                    index = 0;
                    if(cond[0].Contains("."))
                    {
                        cond[0] = cond[0].Split('.')[1];
                    }
                    foreach (var item in eTable.attributes)
                    {
                        if (item.name.ToLower() == cond[0])
                            break;
                        index++;
                    }
                    indexTable = index;

                    foreach (var reg in regs)
                    {
                        List<List<Object>> lAux = CopyList(registers);
                        switch (cond[1])
                        {
                            case "=":
                                index = 0;
                                List<int> del = new List<int>();
                                foreach (var rj in lAux)
                                {
                                    if (!(Convert.ToDecimal(reg[indexTable]) == Convert.ToDecimal(rj[indexJoin])))
                                        del.Add(index);
                                    index++;
                                }
                                for (int i = del.Count -1; i >= 0; i--)
                                {
                                    lAux.RemoveAt(del[i]);
                                }
                                foreach (var r in lAux)
                                {
                                    List<Object> lo = new List<object>();
                                    foreach (var rt in reg)
                                    {
                                        lo.Add(rt);
                                    }
                                    foreach (var rj in r)
                                    {
                                        lo.Add(rj);
                                    }
                                    result.Add(lo);
                                }
                                break;
                            case ">":

                                break;
                            case "<":

                                break;
                            case ">=":

                                break;
                            case "<=":

                                break;
                            case "<>":

                                break;
                        }
                    }
                }
            }
            return result;
        }

        private void gridEntities_Click(object sender, EventArgs e)
        {

        }
    }
}
