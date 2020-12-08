using ProyectoArchivos.MainApp.Classes;
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
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ProyectoArchivos.MainApp.Index
{
    public partial class frmForeignKey : Telerik.WinControls.UI.RadForm
    {
        private List<Register> headers;
        private Attributes fkAttribute;
        private string path;
        private string pathFK;
        BinaryReader Reader;
        BinaryWriter Writer;
        bool exists = false;

        public frmForeignKey(Attributes a, string path)
        {
            InitializeComponent();
            fkAttribute = a;
            headers = new List<Register>();
            this.path = path;
            pathFK = Path.Combine(path, fkAttribute.id + ".fk");
            CreateFile();
        }

        public bool Insert(Object value, long dir)
        {
            try
            {
                if (headers.Count == 0)
                {
                    Register gi = new Register();
                    gi.dir = 2048;
                    gi.val = value;
                    gi.bloque = new List<long>();
                    gi.bloque.Add(dir);
                    headers.Add(gi);
                    WriteHeaders();
                }
                else
                {
                    Register header = SearchHeader(value);
                    if (header != null)
                    {
                        header.bloque.Add(dir);
                        WriteBlock(header);
                    }
                    else
                    {
                        Register gi = new Register();
                        gi.dir = GetFileSize();
                        gi.val = value;
                        gi.bloque = new List<long>();
                        gi.bloque.Add(dir);
                        headers.Add(gi);
                        WriteHeaders();
                    }
                }
                ShowData();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Delete(object value, long dir)
        {
            Register d = SearchHeader(value);
            if (d != null)
            {
                d.bloque.Remove(dir);
                ShowData();
                WriteBlock(d);
            }
        }

        private Register SearchHeader(object value)
        {
            foreach (var h in headers)
            {
                if(fkAttribute.dataType == 'C')
                {
                    if (Convert.ToString(h.val).Trim('\0') == value.ToString())
                        return h;
                }
                else
                {
                    if ((int)h.val == (int)value)
                    {
                        return h;
                    }
                }
                
            }
            return null;
        }
        
        private long GetFileSize()
        {
            long size;
            Reader = new BinaryReader(File.OpenRead(pathFK));
            size = Reader.BaseStream.Length;
            Reader.Close();
            return size;
        }

        private void ShowData()
        {
            gridFK.DataSource = null;
            gridFK.Rows.Clear();
            gridFK.DataSource = headers.OrderBy(o=>o.val).ToList();
            gridFK_CellClick(this, null);
        }

        private void WriteBlock(Register r)
        {
            FileStream file = new FileStream(pathFK, FileMode.Open, FileAccess.Write, FileShare.None);
            Writer = new BinaryWriter(file);
            Writer.Seek((int)r.dir, SeekOrigin.Begin);
            foreach (var v in r.bloque)
            {
                Writer.Write(v);
            }
            Writer.Write((long)-1);
            byte[] b = new byte[2040 - (r.bloque.Count * 8 + 8)];
            Writer.Write(b);
            Writer.Write((long)-1);
            Writer.Close();
        }

        private void WriteHeaders()
        {
            if (!exists)
                CreateFile();
            FileStream file = new FileStream(pathFK, FileMode.Open, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            file.Seek(0, SeekOrigin.Begin);
            int counterByte = 0;
            foreach (var r in headers)
            {
                if (fkAttribute.dataType == 'C')
                {
                    byte[] name = new byte[fkAttribute.length];
                    Encoding.ASCII.GetBytes(Convert.ToString(r.val), 0, Convert.ToString(r.val).Length, name, 0);
                    counterByte += fkAttribute.length;
                    bw.Write(name);
                    bw.Write(r.dir);
                    counterByte += fkAttribute.length + 8;
                }
                else
                {
                    bw.Write(Convert.ToInt32(r.val));
                    bw.Write(r.dir);
                    counterByte += 12;
                }
            }
            byte[] b = new byte[2040 - counterByte];
            bw.Write(b);
            bw.Write((long)-1);

            foreach (var r in headers)
            {
                bw.Seek((int)r.dir, SeekOrigin.Begin);
                foreach (var v in r.bloque)
                {
                    bw.Write(v);
                }
                bw.Write((long)-1);
                b = new byte[2040 - (r.bloque.Count * 8 + 8)];
                bw.Write(b);
                bw.Write((long)-1);
            }
            bw.Close();
            
        }

        private void CreateFile()
        {
            Writer = new BinaryWriter(File.Create(pathFK));
            Writer.Close();

            exists = true;
        }

        private void gridFK_CellClick(object sender, GridViewCellEventArgs e)
        {
            foreach (var r in headers)
            {
                if (r.val == gridFK.SelectedRows[0].Cells[1].Value)
                {
                    gridFKData.Rows.Clear();
                    foreach (var b in r.bloque)
                    {
                        
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(gridFKData.MasterView);
                        rowInfo.Cells[0].Value = b;
                        gridFKData.Rows.Add(rowInfo);
                    }
                    return;
                }
            }
        }

        public void OpenFile()
        {
            FileStream file = new FileStream(pathFK, FileMode.Open, FileAccess.Read, FileShare.None);
            Reader = new BinaryReader(file);
            if (file.Length > 0)
            {
                Register r = new Register();
                r.dir = -1;
                while (true)
                {
                    r = new Register();
                    if (fkAttribute.dataType == 'C')
                    {
                        r.val = Encoding.ASCII.GetString(Reader.ReadBytes(fkAttribute.length));
                    }
                    else
                    {
                        r.val = Reader.ReadInt32();
                    }
                    r.dir = Reader.ReadInt64();
                    r.bloque = new List<long>();
                    if (r.dir == 0)
                        break;
                    headers.Add(r);

                }
                Reader.Close();

                foreach (var h in headers)
                {
                    ReadBlock(h);
                }
                ShowData();
            }
            else
            {
                Reader.Close();
                file.Close();
            }
        }

        private void ReadBlock(Register r)
        {
            FileStream file = new FileStream(pathFK, FileMode.Open, FileAccess.Read, FileShare.None);
            Reader = new BinaryReader(file);
            Reader.BaseStream.Seek(r.dir, SeekOrigin.Begin);
            long val = 0;

            while(true)
            {
                val = Reader.ReadInt64();
                if (val == -1)
                {
                    Reader.Close();
                    break;
                }
                r.bloque.Add(val);
            }
        }
    }
}
