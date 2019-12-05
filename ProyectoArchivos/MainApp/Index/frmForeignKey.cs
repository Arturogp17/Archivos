using ProyectoArchivos.MainApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.IO;

namespace ProyectoArchivos.MainApp.Index
{
    public partial class frmForeignKey : Telerik.WinControls.UI.RadForm
    {
        List<Register> lr = new List<Register>();
        List<Register> bloque = new List<Register>();
        string path;
        int sizeDat;

        public frmForeignKey(List<Register> l, string p, int s)
        {
            InitializeComponent();
            lr = l;
            path = p;
            sizeDat = s;
        }

        private void frmForeignKey_Load(object sender, EventArgs e)
        {
            long sizeFK = 2048;
            foreach (var r in lr)
            {
                if(bloque.Count == 0)
                {
                    Register reg = new Register();
                    reg.val = r.val;
                    reg.dir = sizeFK;
                    reg.bloque = new List<Register>();
                    Register regFK = new Register();
                    regFK.dir = r.dir;
                    reg.bloque.Add(regFK);
                    sizeFK += 2048;
                    bloque.Add(reg);
                }
                else
                {
                    int index = SearchBlock((string)r.val);
                    if(index != -1)
                    {
                        Register regFK = new Register();
                        regFK.dir = r.dir;
                        bloque[index].bloque.Add(regFK);
                    }
                    else
                    {
                        Register reg = new Register();
                        reg.val = r.val;
                        reg.dir = sizeFK;
                        reg.bloque = new List<Register>();
                        Register regFK = new Register();
                        regFK.dir = r.dir;
                        reg.bloque.Add(regFK);
                        sizeFK += 2048;
                        bloque.Add(reg);
                    }
                }
            }
            bloque = bloque.OrderBy(o => o.val).ToList();
            gridFK.DataSource = bloque;
            sizeFK = 2048;
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(file);
            foreach (var r in bloque)
            {
                if(sizeDat > 4)
                {
                    byte[] name = new byte[sizeDat];
                    Encoding.ASCII.GetBytes(Convert.ToString(r.val), 0, Convert.ToString(r.val).Length, name, 0);
                    bw.Write(name);
                }
                else
                    bw.Write(Convert.ToInt32(r.val));
                bw.Write(r.dir);
            }
            bw.Write(Convert.ToInt64(-1));
            byte[] bPK = new byte[sizeFK - file.Length];
            bw.Write(bPK);
            sizeFK += 2048;
            foreach (var r in bloque)
            {
                foreach (var b in r.bloque)
                {
                    bw.Write(b.dir);
                }
                bw.Write(Convert.ToInt64(-1));
                bPK = new byte[sizeFK - file.Length];
                bw.Write(bPK);
                sizeFK += 2048;
            }
            file.Close();
        }

        public int SearchBlock(string name)
        {
            for (int i = 0; i < bloque.Count; i++)
            {
                if ((string)bloque[i].val == name)
                    return i;
            }
            return -1;
        }

        private void gridFK_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            foreach (var r in bloque)
            {
                if(r.val == gridFK.SelectedRows[0].Cells[0].Value)
                {
                    gridFKData.DataSource = r.bloque;
                    return;
                }
            }
        }
    }
}
