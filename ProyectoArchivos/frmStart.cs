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

        public frmStart()
        {
            InitializeComponent();
        }

        public void CreateFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if(saveFile.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFile.FileName;
                file = new FileStream(saveFile.FileName, FileMode.Create);
                file.Close();
            }
        }

        public void OpenFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            if(open.ShowDialog() == DialogResult.OK)
            {
                entities.Clear();
                fileName = open.FileName;
                file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                file.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(file);

                header = br.ReadInt64();
                if(header != -1)
                {
                    ReadNextEntity(header, br, file);
                    btnCreateAttribute.Enabled = true;
                    btnDeleteAtribute.Enabled = true;
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

        private void btnCreateEntity_Click(object sender, EventArgs e)
        {
            if (fileName == "")
                CreateFile();
            string id = string.Empty;
                if (entities.Count == 0)
                {
                    id = "00001";
                }
                else
                {
                    id = Convert.ToString(String.Format("{0:00000}", Convert.ToInt32(entities.Last().id) + 1));
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
                        RefreshGrids();
                        btnCreateAttribute.Enabled = true;
                        btnDeleteAtribute.Enabled = true;
                        btnDeleteEntity.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se puede repetir el nombre de una entidad", "Diccionario de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnCreateEntity_Click(sender, e);
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
            for(int i = 0; i < entities.Count - 1; i++)
            {
                entities[i].nextEntityAddress = entities[i + 1].address;
            }
            entities.Last().nextEntityAddress = -1;
            entities = entities.OrderBy(o => o.id).ToList();
        }

        public void RefreshGrids()
        {
            gridEntities.Rows.Clear();
            gridAttributes.Rows.Clear();
            foreach(Entity e in entities)
            {
                GridViewRowInfo row = gridEntities.Rows.AddNew();
                row.Cells["id"].Value = e.id;
                row.Cells["name"].Value = e.name;
                row.Cells["address"].Value = e.address;
                row.Cells["attributeAddress"].Value = e.attributeAddress;
                row.Cells["dataAddress"].Value = e.dataAddress;
                row.Cells["nextEntityAddress"].Value = e.nextEntityAddress;
                foreach (Attributes a in e.attributes)
                {
                    row = gridAttributes.Rows.AddNew();
                    row.Cells["id"].Value = a.id;
                    row.Cells["name"].Value = a.name;
                    row.Cells["dataType"].Value = a.dataType.ToString();
                    row.Cells["length"].Value = a.length;
                    row.Cells["address"].Value = a.address;
                    row.Cells["indexType"].Value = a.indexType.ToString();
                    row.Cells["indexAddress"].Value = a.indexAddress;
                    row.Cells["nextAttributeAddress"].Value = a.nextAttributeAddress;
                }
            }
            Save_Click(this, null);
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
                        if (en.attributes.Count > 0)
                            en.attributes.Last().nextAttributeAddress = a.address;
                        else
                            en.attributeAddress = a.address;
                        en.attributes.Add(a);
                        break;
                    }
                }
                ca.Dispose();
                RefreshGrids();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);

            bw.Write(header);
            foreach (Entity en in entities)
            {
                byte[] id = new byte[5];
                byte[] name = new byte[35];
                Encoding.ASCII.GetBytes(en.id, 0, 5, id, 0);
                Encoding.ASCII.GetBytes(en.name, 0, en.name.Length, name, 0);
                bw.Write(id);
                bw.Write(name);
                bw.Write(en.address);
                bw.Write(en.attributeAddress);
                bw.Write(en.dataAddress);
                bw.Write(en.nextEntityAddress);
                foreach (Attributes a in en.attributes)
                {
                    byte[] idA= new byte[5];
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
                }
            }
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
                    break;
                }
            }
            Save_Click(sender, e);
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
                        break;
                    }
                }
            }
            Save_Click(sender, e);
        }

        private void gridAttributes_DoubleClick(object sender, EventArgs e)
        {
            string ent = string.Empty;
            Attributes at = new Attributes();
            foreach (Entity en in entities)
            {
                foreach (Attributes a in en.attributes)
                {
                    if (a.id == gridAttributes.SelectedRows[0].Cells["id"].Value.ToString())
                    {
                        ent = en.name;
                        at = a;
                        break;
                    }
                }
                if (ent != "")
                    break;
            }
            frmCreateAttribute fe = new frmCreateAttribute(ent, at);
            if(fe.ShowDialog() == DialogResult.OK)
            {
                at.name = fe.name;
                at.dataType = fe.dataType;
                at.length = fe.length;
                at.indexType = Convert.ToInt32(fe.indexType);
                RefreshGrids();
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
                        entities[i].attributes.Clear();
                        entities.RemoveAt(i);
                        break;
                    }
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
    }
}
