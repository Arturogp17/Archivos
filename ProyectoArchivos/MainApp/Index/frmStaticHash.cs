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
    public partial class frmStaticHash : Telerik.WinControls.UI.RadForm
    {
        private List<Register> Blocks;
        private Attributes hashA;
        private string path;
        private string pathHS;
        BinaryReader Reader;
        BinaryWriter Writer;
        bool exists = false;
        int blocksNum = 7;
        int blockSize = 42;

        public frmStaticHash(Attributes a, string path)
        {
            InitializeComponent();
            hashA = a;
            Blocks = new List<Register>();
            this.path = path;
            pathHS = Path.Combine(path, hashA.id + ".hs");
        }

        private void CreateBlocks()
        {
            for (int i = 0; i < blocksNum; i++)
            {
                Register r = new Register();
                r.id = i;
                r.dir = -1;
                r.values = new List<List<Register>>();
                r.addr = new List<long>();
                r.addrNext = new List<long>();
                Blocks.Add(r);
            }
            WriteBlocks();
        }

        private void ShowData()
        {
            gridBlocks.DataSource = null;
            gridBlocks.Rows.Clear();
            gridBlocks.DataSource = Blocks;
            gridBlocks_CellClick(this, null);
        }

        public bool Insert(int value, long dir)
        {
            if (!exists)
                CreateFile();
            Register b = FindBlock(value);
                
            if(b.dir == -1)
            {
                CreateBlock(b, value, dir);
            }
            else
            {
                bool added = false;
                for (int i = 0; i < b.values.Count; i++)
                {
                    if(b.values[i].Count < blockSize)
                    {
                        Register r = new Register();
                        r.dir = dir;
                        r.val = value;
                        r.next = -1;
                        r.address = b.addr[i];
                        b.values[i].Add(r);
                        b.values[i] = b.values[i].OrderBy(o => o.val).ToList();
                        added = true;
                        WriteValues(b.values[i], i, b);
                        WriteBlocks();
                        break;
                    }
                }
                    
                if(!added)
                {
                    AddBlock(b, value, dir);
                }
            }
            
            ShowData();
            return true;
        }

        public void Delete(int value, long dir)
        {
            Register b = FindBlock(value);
            bool removed = false;
            for (int i = 0; i < b.values.Count && !removed; i++)
            {
                for (int j = 0; j < b.values[i].Count; j++)
                {
                    if((int)b.values[i][j].val == value)
                    {
                        b.values[i].Remove(b.values[i][j]);
                        removed = true;
                        WriteValues(b.values[i], i, b);
                        break;
                    }
                }
            }
            ShowData();
        }

        public void OpenFile()
        {
            exists = true;
            Blocks = new List<Register>();
            FileStream file = new FileStream(pathHS, FileMode.Open, FileAccess.Read, FileShare.None);
            if (file.Length > 0)
            {
                Reader = new BinaryReader(file);
                for (int i = 0; i < blocksNum; i++)
                {
                    Register r = new Register();
                    r.id = i;
                    r.dir = Reader.ReadInt64();
                    r.values = new List<List<Register>>();
                    r.addr = new List<long>();
                    r.addrNext = new List<long>();
                    Blocks.Add(r);
                }

                foreach (var b in Blocks)
                {
                    
                    long next = b.dir;
                    while (next != -1)
                    {
                        b.addr.Add(next);
                        List<Register> lr = new List<Register>();
                        Reader.BaseStream.Seek(next, SeekOrigin.Begin);
                        for (int i = 0; i < blockSize; i++)
                        {
                            Register r = new Register();
                            r.val = Reader.ReadInt32();
                            r.dir = Reader.ReadInt64();
                            r.address = next;
                            if ((int)r.val != -1 && r.dir != -1)
                            {
                                lr.Add(r);
                            }
                        }
                        next = Reader.ReadInt64();
                        b.addrNext.Add(next);
                        b.values.Add(lr);
                    }
                }
                Reader.Close();
                file.Close();
                exists = true;
                ShowData();
            }
            else
            {
                file.Close();
                CreateBlocks();
            }
        }

        private void AddBlock(Register block, int value, long dir)
        {
            Reader = new BinaryReader(File.OpenRead(pathHS));
            long size = Reader.BaseStream.Length;
            Reader.Close();
            block.addrNext[block.values.Count - 1] = size;
            List <Register> lr = new List<Register>();
            Register r = new Register();
            r.address = size;
            r.val = value;
            r.dir = dir;
            block.addrNext.Add(-1);
            lr.Add(r);
            WriteValues(block.values.Last(), block.values.Count - 1, block);
            block.values.Add(lr);
            block.addr.Add(size);
            WriteValues(lr, block.values.Count - 1, block);
            WriteBlocks();
        }

        private void WriteValues(List<Register> lr, int index, Register block)
        {
            FileStream file = new FileStream(pathHS, FileMode.Open, FileAccess.Write, FileShare.None);
            Writer = new BinaryWriter(file);
            Writer.BaseStream.Seek(block.addr[index], SeekOrigin.Begin);
            foreach (var r in lr)
            {
                Writer.Write((int)r.val);
                Writer.Write(r.dir);
            }
            int result = blockSize - lr.Count;
            for (int i = 0; i < result; i++)
            {
                Writer.Write((int)-1);
                Writer.Write((long)-1);
            }
            Writer.Write(block.addrNext[index]);
            Writer.Close();
            file.Close();
        }

        private void WriteBlocks()
        {
            if (!exists)
                CreateFile();
            FileStream file = new FileStream(pathHS, FileMode.Open, FileAccess.Write, FileShare.None);
            Writer = new BinaryWriter(file);
            foreach (var b in Blocks)
            {
                Writer.Write(b.dir);
            }
            Writer.Close();
            file.Close();
        }

        private void CreateFile()
        {
            FileStream file = new FileStream(pathHS, FileMode.Create, FileAccess.Write, FileShare.None);
            Writer = new BinaryWriter(file);
            Writer.Close();
            file.Close();
            exists = true;
            CreateBlocks();
            
        }

        private void CreateBlock(Register block, int value, long dir)
        {
            Reader = new BinaryReader(File.OpenRead(pathHS));
            long size = Reader.BaseStream.Length;
            Reader.Close();
            block.dir = size;
            List<Register> lr = new List<Register>();
            Register r = new Register();
            r.val = value;
            r.dir = dir;
            r.address = size;
            block.addrNext.Add(-1);
            lr.Add(r);
            block.values.Add(lr);
            block.addr.Add(size);
            WriteBlocks();
            WriteValues(lr, block.values.Count - 1, block);
        }

        private void gridBlocks_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            gridData.Rows.Clear();
            int extraRows = 0;
            if (gridBlocks.SelectedRows.Count > 0)
            {
                Register b = Blocks[Convert.ToInt32(gridBlocks.SelectedRows[0].Cells[0].Value)];
                for (int i = 0; i < b.values.Count; i++)
                {
                    extraRows = blockSize - b.values[i].Count;
                    for (int j = 0; j < b.values[i].Count; j++)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(gridData.MasterView);
                        
                        rowInfo.Cells[0].Value = b.values[i][j].val;
                        rowInfo.Cells[1].Value = b.values[i][j].dir;
                        gridData.Rows.Add(rowInfo);
                    }
                    for (int k = 0; k < extraRows; k++)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(gridData.MasterView);
                        gridData.Rows.Add(rowInfo);
                    }
                }
            }
        }

        private Register FindBlock(int value)
        {
            return Blocks[value % blocksNum];
        }
    }
}
