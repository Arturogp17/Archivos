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

namespace ProyectoArchivos.MainApp.Index
{
    public partial class frmBinaryTree : Telerik.WinControls.UI.RadForm
    {
        private int leafSize;

        private BinaryReader Reader;
        private BinaryWriter Writer;
        private long Ultima_direccion;
        private List<Node> primaryTree;
        private Node root;
        private Node parent;
        private long lastAddress;

        private string pathTree;
        private string path;
        private bool exists;
        private string fileName;
        private Attributes attribTree;

        public frmBinaryTree(Attributes atribArbolPRIM, string path, string fileName)
        {
            InitializeComponent();
            this.path = path;
            this.fileName = fileName;
            leafSize = 4;
            primaryTree = new List<Node>();
            lastAddress = -1;
            root = null; parent = null;
            attribTree = atribArbolPRIM;
            pathTree = Path.Combine(path, attribTree.id + ".bt");
            SetHeadersGrid(leafSize);
        }

        public void Insert(int data, long dir)
        {
            if (exists)
            {
                Node leafAux = null;

                if (primaryTree.Count > 0)//Verifica si ya existen hojas en el arbol
                {
                    leafAux = FindInsertLeaf(data); //regresa la hoja donde deberia ser insertado

                    if (leafAux != null)
                    {
                        long repeated = Repeated(data, leafAux);

                        if (repeated == -1)//es un nuevo dato
                        {
                            //busca busca en la hoja donde se inserta en nuevo dato(nodo), la posicion
                            long pos = InsertLeaf(leafAux, data, dir);//Si hay espacio se recorre y deja la posicion

                            if (pos != -1)//Si existe la posicion donde insertarse lo hace
                            {
                                //Actualizar datos de la hoja en el archivo index (Escribir)
                                WriteFile(leafAux);

                                UpdateGrid();
                            }
                            else//Si no existe una posicion, se debe dividir el nodo
                            {
                                long dir_b2 = DivideLeaf(leafAux, data, dir);

                                UpdateGrid();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("ERROR", "NO se encontro HOJA donde Insertar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Escribe en el atributo la dir a esta primer hoja pos si se queda hasta aqui
                    attribTree.indexAddress = 0;
                    WriteAttribute(attribTree);

                    leafAux = new Node('H', (long)0);

                    leafAux.nodePK[0].P = dir;
                    leafAux.nodePK[0].K = data;

                    //Escribe hoja en el archivo
                    WriteFile(leafAux);
                    //Agregar al árbol
                    primaryTree.Add(leafAux);

                    UpdateGrid();
                }
            }
            else
            {
                CreateFile();
                Insert(data, dir);
            }
        }

        private long InsertLeaf(Node leaf, int newData, long dir)
        {
            if (leaf.nodePK[leafSize - 1].K == -1)//Verifica que exista espacio en la hoja para insertar y recorre para dejar el lugar
            {
                for (int p = 0; p < leafSize; p++)//Busca el lugar donde debe insertarse
                {
                    if (leaf.nodePK[p].K != -1)
                    {
                        if (newData < leaf.nodePK[p].K)//Hay que recorrer los nodos para dejar el espacio
                        {
                            for (int b = leafSize - 1; b > p; b--)
                            {
                                if (b - 1 >= 0)
                                {
                                    leaf.nodePK[b].P = leaf.nodePK[b - 1].P;
                                    leaf.nodePK[b].K = leaf.nodePK[b - 1].K;
                                }
                            }

                            leaf.nodePK[p].P = dir;
                            leaf.nodePK[p].K = newData;

                            p = leafSize;
                        }
                    }
                    else//Existe un lugar directo
                    {
                        leaf.nodePK[p].P = dir;
                        leaf.nodePK[p].K = newData;

                        p = leafSize;
                    }
                }

                return dir;
            }
            else
            {
                return -1;
            }
        }

        private Node FindInsertLeaf(int dato)
        {
            if (primaryTree.Count > 1)
            {
                if (root != null)
                {
                    bool exchange = false;
                    Node newLeaf = null;
                    parent = newLeaf = ChangeLeaf(root.nodeAddress);

                    while (newLeaf.type != 'H')
                    {
                        exchange = false;

                        for (int dir = 0; dir < leafSize; dir++)
                        {
                            if (dato < newLeaf.nodePK[dir].K || newLeaf.nodePK[dir].K == -1)
                            {
                                exchange = true;
                                parent = newLeaf;
                                newLeaf = ChangeLeaf(newLeaf.nodePK[dir].P);
                                dir = leafSize;
                            }
                        }

                        if (!exchange)
                        {
                            parent = newLeaf;
                            newLeaf = ChangeLeaf(newLeaf.next);
                        }
                    }

                    return newLeaf;
                }
            }
            else
            {
                return primaryTree[0];
            }

            return null;
        }

        private Node ChangeLeaf(long dir)
        {
            for (int PK = 0; PK < primaryTree.Count; PK++)
            {
                if (primaryTree[PK].nodeAddress == dir)
                { return primaryTree[PK]; }
            }
            return null;
        }

        private long DivideLeaf(Node leaf, int data, long dir)
        {
            /*Crea Nnueva Hoja*/
            Reader = new BinaryReader(File.OpenRead(pathTree));
            long endFile = Reader.BaseStream.Length;
            Reader.Close();

            Node newLeaf = new Node('H', endFile);

            /*Pasa la mitad de los nodos*/
            int pk = 0;
            for (int n = leafSize / 2; n < leafSize; n++)
            {
                newLeaf.nodePK[pk].P = leaf.nodePK[n].P;
                newLeaf.nodePK[pk].K = leaf.nodePK[n].K;

                leaf.nodePK[n].P = -1;
                leaf.nodePK[n].K = -1;

                pk++;
            }

            //Enlaza la hoja nueva con el sig de la hoja actual
            newLeaf.next = leaf.next;
            //Enlaza la hoja actual con la nueva
            leaf.next = newLeaf.nodeAddress;

            WriteFile(leaf);
            WriteFile(newLeaf);

            long dir_bloq2k = -1;
            /*Verifica donde se inserta el nuevo valor (en hoja actual o en hoja nueva)*/
            if (data > leaf.nodePK[leafSize / 2 - 1].K)
            {
                //Se inserta en la nueva hoja
                dir_bloq2k = InsertLeaf(newLeaf, data, dir);
            }
            else
            {
                //Se inserta en la hoja actual
                dir_bloq2k = InsertLeaf(leaf, data, dir);
                //Se "manda" el ultimo nodo de la hoja actual a la hoja nueva
                long pk_p = leaf.nodePK[leafSize / 2].P; leaf.nodePK[leafSize / 2].P = -1;
                int pk_k = leaf.nodePK[leafSize / 2].K; leaf.nodePK[leafSize / 2].K = -1;

                for (int nv = leafSize / 2 - 1; nv >= 0; nv--)
                {
                    newLeaf.nodePK[nv + 1].P = newLeaf.nodePK[nv].P;
                    newLeaf.nodePK[nv + 1].K = newLeaf.nodePK[nv].K;
                }

                newLeaf.nodePK[0].P = pk_p;
                newLeaf.nodePK[0].K = pk_k;
            }

            //Agrega la nueva hoja al arbol
            primaryTree.Add(newLeaf);
            WriteFile(leaf);
            WriteFile(newLeaf);

            /*Inserta el apuntador a la nueva hoja en el padre correspondiente (Intermedio o Raiz)*/
            if (root != null)
            {
                if (parent != null && parent.nodePK[leafSize - 1].K == -1)
                {
                    InsertInParent(parent, newLeaf.nodePK[0].K, newLeaf.nodeAddress);
                }
                else if (parent != null)
                {
                    DivideParent(parent, newLeaf.nodePK[0].K, newLeaf.nodeAddress);
                }
            }
            else
            {
                /*Crea Raiz*/
                Reader = new BinaryReader(File.OpenRead(pathTree));
                root = new Node('R', Reader.BaseStream.Length);
                Reader.Close();
                root.nodePK[0].P = leaf.nodeAddress;
                root.nodePK[0].K = newLeaf.nodePK[0].K;
                root.nodePK[1].P = newLeaf.nodeAddress;

                primaryTree.Add(root);
                WriteFile(root);
                attribTree.indexAddress = root.nodeAddress;
                WriteAttribute(attribTree);
            }

            return dir_bloq2k;
        }

        private void InsertInParent(Node parentLeaf, int data, long dir)
        {
            for (int n = 0; n < leafSize; n++)
            {
                if (parentLeaf.nodePK[n].K > -1)
                {
                    if (data < parentLeaf.nodePK[n].K)
                    {
                        for (int m = leafSize - 2; m >= n; m--)
                        {
                            if (m >= leafSize - 2)
                            {
                                parentLeaf.next = parentLeaf.nodePK[m + 1].P;
                                parentLeaf.nodePK[m + 1].K = parentLeaf.nodePK[m].K;
                            }
                            else
                            {
                                parentLeaf.nodePK[m + 1].K = parentLeaf.nodePK[m].K;
                                parentLeaf.nodePK[m + 2].P = parentLeaf.nodePK[m + 1].P;
                            }
                        }

                        parentLeaf.nodePK[n].K = data;

                        if (n + 1 < leafSize)
                        {
                            parentLeaf.nodePK[n + 1].P = dir;
                        }
                        else
                        {
                            parentLeaf.next = dir;
                        }

                        n = leafSize;
                    }
                }
                else
                {
                    parentLeaf.nodePK[n].K = data;

                    if (n + 1 < leafSize)
                    {
                        parentLeaf.nodePK[n + 1].P = dir;
                    }
                    else
                    {
                        parentLeaf.next = dir;
                    }

                    n = leafSize;
                }
            }

            WriteFile(parentLeaf);
        }

        private void DivideParent(Node parentLeaf, int data, long dir)
        {
            /*Crea una nueva Hoja Padre (I)*/
            Reader = new BinaryReader(File.OpenRead(pathTree));
            Node newParent = new Node('I', Reader.BaseStream.Length);
            Reader.Close();

            /*Pasa la mitad de los nodos*/
            int pk = 0;
            for (int n = leafSize / 2; n < leafSize; n++)
            {
                newParent.nodePK[pk].P = parentLeaf.nodePK[n].P;
                newParent.nodePK[pk].K = parentLeaf.nodePK[n].K;

                if (n > leafSize / 2) { parentLeaf.nodePK[n].P = -1; }
                parentLeaf.nodePK[n].K = -1;

                pk++;
            }

            newParent.nodePK[pk].P = parentLeaf.next;
            parentLeaf.next = -1;

            //Actualiza datos
            primaryTree.Add(newParent);
            WriteFile(newParent);
            parentLeaf.type = 'I';
            WriteFile(parentLeaf);

            Node grandParent = SearchGrandParent(parentLeaf);//Busca el Padre de la hoja padre actual

            //Verifica de que lado debiera ir el nuevo dato (padre actual o padre nuevo)
            if (data < parentLeaf.nodePK[leafSize / 2 - 1].K)
            {
                //Va en el padre actual
                InsertInParent(parentLeaf, data, dir);
                int parentData = parentLeaf.nodePK[leafSize / 2].K;

                parentLeaf.nodePK[leafSize / 2].K = -1;
                parentLeaf.nodePK[leafSize / 2 + 1].P = -1;
                WriteFile(parentLeaf);

                if (grandParent != null)
                {
                    if (grandParent.nodePK[leafSize - 1].K == -1) //Aun hay espacio en el padre del padre
                    {
                        InsertInParent(grandParent, parentData, newParent.nodeAddress);
                    }
                    else { DivideParent(grandParent, parentData, newParent.nodeAddress); }
                }
                else
                {
                    /*Crea una raíz nueva*/
                    Reader = new BinaryReader(File.OpenRead(pathTree));
                    Node newRoot = new Node('R', Reader.BaseStream.Length);
                    Reader.Close();
                    
                    newRoot.nodePK[0].P = parentLeaf.nodeAddress;
                    newRoot.nodePK[0].K = parentData;
                    newRoot.nodePK[1].P = newParent.nodeAddress;

                    primaryTree.Add(newRoot);
                    WriteFile(newRoot);
                    attribTree.indexAddress = newRoot.nodeAddress;
                    WriteAttribute(attribTree);
                    root = newRoot;
                }
            }
            else
            {
                //Va en el padre nuevo
                InsertInParent(newParent, data, dir);
                int parentData = newParent.nodePK[0].K;

                newParent.nodePK[0].P = -1;
                newParent.nodePK[0].K = -1;

                for (int np = 0; np <= leafSize / 2; np++)
                {
                    newParent.nodePK[np].P = newParent.nodePK[np + 1].P;
                    newParent.nodePK[np].K = newParent.nodePK[np + 1].K;
                }

                newParent.nodePK[leafSize / 2].K = -1;
                newParent.nodePK[leafSize / 2 + 1].P = -1;

                WriteFile(newParent);

                if (grandParent != null)
                {
                    if (grandParent.nodePK[leafSize - 1].K == -1) //Aun hay espacio en el padre del padre
                    {
                        InsertInParent(grandParent, parentData, newParent.nodeAddress);
                    }
                    else
                    {
                        DivideParent(grandParent, parentData, newParent.nodeAddress);
                    }
                }
                else
                {
                    /*Crea una raíz nueva*/
                    Reader = new BinaryReader(File.OpenRead(pathTree));
                    Node newRoot = new Node('R', Reader.BaseStream.Length);
                    Reader.Close();
                    
                    newRoot.nodePK[0].P = parentLeaf.nodeAddress;
                    newRoot.nodePK[0].K = parentData;
                    newRoot.nodePK[1].P = newParent.nodeAddress;

                    primaryTree.Add(newRoot);
                    WriteFile(newRoot);
                    attribTree.indexAddress = newRoot.nodeAddress;
                    WriteAttribute(attribTree);
                    root = newRoot;
                }
            }
        }

        private long Repeated(int data, Node leaf)
        {
            for (int h = 0; h < leafSize; h++)
            {
                if (leaf.nodePK[h].K == data)
                {
                    return leaf.nodePK[h].P;
                }
            }

            return (long)-1;
        }

        private Node SearchGrandParent(Node parent)
        {
            Node grandParent = null;

            for (int pp = 0; pp < primaryTree.Count; pp++)
            {
                if (primaryTree[pp].type == 'R' || primaryTree[pp].type == 'I')
                {
                    for (int dp = 0; dp < primaryTree[pp].nodePK.Length; dp++)
                    {
                        if (primaryTree[pp].nodePK[dp].P == parent.nodeAddress || primaryTree[pp].next == parent.nodeAddress)
                        {
                            grandParent = primaryTree[pp];
                            return grandParent;
                        }
                    }
                }
            }

            return grandParent;
        }

        private void CreateFile()
        {

            Writer = new BinaryWriter(File.Create(pathTree));
            Writer.Close();
            
            exists = true;
            
        }

        public int ExistingFile()
        {
            //MessageBox.Show("Ya existe"); 
            if (File.Exists(pathTree))
            {
                exists = true;
                //LoadTree();
                LoadTree();
                return 1;
            }
            else
                return 0;
        }
        
        public void Delete(int value, long dirP)
        {
            if (exists)
            {
                Node LeafAux = null;

                if (primaryTree.Count > 0)//Verifica si ya existen hojas en el arbol
                {
                    LeafAux = FindInsertLeaf(value); //regresa la hoja donde deberia encontrarse el dato

                    if (LeafAux != null)
                    {
                        int res = -1;
                        //Se tiene que eliminar el dato de la hoja
                        if (LeafAux.nodePK[leafSize / 2].K != -1)
                        {
                            //Existen más de la mitad de elementos ELIMINACION NORMAL
                            res = NormalDelete(LeafAux, value, dirP);
                            WriteFile(LeafAux);//Actualiza los datos de la hoja en el archivo

                            UpdateGrid();
                        }
                        else
                        {
                            //Hacer una eliminacion "especial" (prestamo o fusion)
                            res = NormalDelete(LeafAux, value, dirP);
                            WriteFile(LeafAux);

                            if (root != null && res != 0)
                            {
                                LoanDelete(LeafAux);
                            }

                            UpdateGrid();
                        }
                    }
                }
            }
        }

        private int NormalDelete(Node leaf, int data, long dir)
        {
            bool find = false;
            for (int nd = 0; nd < leafSize; nd++)
            {
                if (leaf.nodePK[nd].K == data && leaf.nodePK[nd].P == dir)
                {
                    leaf.nodePK[nd].P = -1;
                    leaf.nodePK[nd].K = -1;

                    int prev = nd;

                    for (int i = nd + 1; i < leafSize; i++)
                    {
                        leaf.nodePK[prev].P = leaf.nodePK[i].P;
                        leaf.nodePK[prev].K = leaf.nodePK[i].K;

                        prev++;
                    }

                    leaf.nodePK[leafSize - 1].P = -1;
                    leaf.nodePK[leafSize - 1].K = -1;

                    nd = leafSize;

                    find = true;
                }
            }

            if (!find)
            {
                MessageBox.Show("No se encontro el nodo con los datos: " + System.Environment.NewLine +
                    "P: " + dir + System.Environment.NewLine +
                    "K: " + data + System.Environment.NewLine,
                    "NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return 0;
            }
            else { return 1; }
        }

        private void LoanDelete(Node leaf)
        {
            Node rightLeaf = FindRightLeaf(leaf, parent);

            if (rightLeaf != null && rightLeaf.nodePK[leafSize / 2].K != -1)//Verifica que puede prestar
            {
                //Guarda los datos (P y K) del nodo que se va a prestar 
                long dir = rightLeaf.nodePK[0].P;
                int value = rightLeaf.nodePK[0].K;
                //Elimina el nodo que va a prestar del hermano derecho y ordena
                NormalDelete(rightLeaf, value, dir);
                WriteFile(rightLeaf);
                //Agregar el dato a la hoja actual
                leaf.nodePK[leafSize / 2 - 1].P = dir;
                leaf.nodePK[leafSize / 2 - 1].K = value;
                WriteFile(leaf);

                //Cambiar el apuntador padre del hermano derecho
                for (int pp = 0; pp < leafSize; pp++)
                {
                    if (parent.nodePK[pp].P == rightLeaf.nodeAddress)
                    {
                        parent.nodePK[pp - 1].K = rightLeaf.nodePK[0].K;
                    }
                }

                if (parent.next == rightLeaf.nodeAddress)
                {
                    parent.nodePK[leafSize - 1].K = rightLeaf.nodePK[0].K;
                }

                WriteFile(parent);
            }
            else
            {
                Node leftLeaf = FindLeftLeaf(leaf, parent);

                if (leftLeaf != null && leftLeaf.nodePK[leafSize / 2].K != -1)//Verifica que puede prestar
                {
                    //Checa el cual nodo va a prestar (nodos a partir de la mitad)
                    long dir = -1;
                    int value = -1;

                    for (int nd = leafSize - 1; nd >= 0; nd--)
                    {
                        if (leftLeaf.nodePK[nd].K != -1)
                        {
                            dir = leftLeaf.nodePK[nd].P;
                            value = leftLeaf.nodePK[nd].K;

                            leftLeaf.nodePK[nd].P = -1;
                            leftLeaf.nodePK[nd].K = -1;

                            nd = -1;
                        }
                    }

                    //Actualiza datos hermano
                    WriteFile(leftLeaf);
                    //Actualiza datos hoja actual
                    for (int h = leafSize / 2 - 1; h > 0; h--) //Recorre para dejar espacio en la hoja actual para el nuevo dato
                    {
                        leaf.nodePK[h].P = leaf.nodePK[h - 1].P;
                        leaf.nodePK[h].K = leaf.nodePK[h - 1].K;
                    }
                    leaf.nodePK[0].P = dir;
                    leaf.nodePK[0].K = value;

                    WriteFile(leaf);

                    //Cambiar el apuntador padre de la hoja actual
                    for (int p = 0; p < leafSize; p++)
                    {
                        if (parent.nodePK[p].P == leftLeaf.nodeAddress)
                        {
                            parent.nodePK[p].K = leaf.nodePK[0].K;
                            p = leafSize;
                        }
                    }
                    WriteFile(parent);

                }
                else//Debe hacer una fusion
                {
                    if (rightLeaf != null)//se fusiona con su hermano derecho
                    {
                        MergeRightLeaf(leaf, rightLeaf, parent);
                    }
                    else if (leftLeaf != null)//se fusiona con su hermano izquierdo
                    {
                        MergeLeftLeaf(leaf, leftLeaf, parent);
                    }
                    else
                    {
                        MessageBox.Show("ERROR", "Esto No deberia pasar Hay un PROBLEMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private Node FindLeftLeaf(Node leaf, Node parentLeaf)
        {
            Node leftLeaf = null;
            bool parent = false;
            long leafDir = -1;
            //Verifica que tenga un hermano inzquierdo
            for (int p = 0; p < leafSize; p++)
            {
                if (parentLeaf.nodePK[p].P == leaf.nodeAddress)
                {
                    if (p - 1 >= 0)
                    {
                        leafDir = parentLeaf.nodePK[p - 1].P;
                        parent = true;
                    }
                    else { parent = false; }

                    p = leafSize;
                }
            }

            if (parentLeaf.next == leaf.nodeAddress)
            {
                parent = true;

                leafDir = parentLeaf.nodePK[leafSize - 1].P;
            }

            if (parent)
            {
                for (int ab = 0; ab < primaryTree.Count; ab++) //Busca al hermano en el árbol
                {
                    if (primaryTree[ab].type == 'H' && primaryTree[ab].nodeAddress == leafDir)
                    {
                        leftLeaf = primaryTree[ab];
                        ab = primaryTree.Count;
                    }
                }
            }

            return leftLeaf;
        }

        private Node FindRightLeaf(Node leaf, Node parentLeaf)
        {
            Node rightLeaf = null;
            bool parent = false;

            if (leaf.next != -1)
            {
                for (int p = 0; p < leafSize; p++) //Verifica que su hermano sea del mismo padre
                {
                    if (parentLeaf.nodePK[p].P == leaf.next)
                    {
                        parent = true;
                        p = leafSize;
                    }
                }

                if (parentLeaf.next == leaf.next) { parent = true; }

                if (parent)
                {
                    for (int ab = 0; ab < primaryTree.Count; ab++) //Busca al hermano en el árbol
                    {
                        if (primaryTree[ab].type == 'H' && primaryTree[ab].nodeAddress == leaf.next)
                        {
                            rightLeaf = primaryTree[ab];
                            ab = primaryTree.Count;
                        }
                    }
                }
            }

            return rightLeaf;
        }

        private void MergeLeftLeaf(Node leaf, Node leftLeaf, Node parentLeaf)
        {
            //Pasa los datos de la hoja actual al hermano izquierdo
            int pos = leafSize / 2;
            for (int nd = 0; nd < leafSize / 2 - 1; nd++)
            {
                leftLeaf.nodePK[pos].P = leaf.nodePK[nd].P;
                leftLeaf.nodePK[pos].K = leaf.nodePK[nd].K;

                leaf.nodePK[nd].P = -1;
                leaf.nodePK[nd].K = -1;

                pos++;
            }

            leftLeaf.next = leaf.next;

            //Actualizar datos
            WriteFile(leftLeaf);
            WriteFile(leaf);

            //Actualiza al padre recorriendolo
            for (int p = 0; p <= leafSize; p++)
            {
                if (p < leafSize)
                {
                    if (parentLeaf.nodePK[p].P == leaf.nodeAddress)
                    {
                        parentLeaf.nodePK[p - 1].K = -1;

                        for (int ps = p + 1; ps <= leafSize; ps++)
                        {
                            if (ps == leafSize)
                            {
                                parentLeaf.nodePK[ps - 1].P = parentLeaf.next;
                                parentLeaf.nodePK[ps - 1].K = -1;
                                parentLeaf.next = -1;
                            }
                            else
                            {
                                parentLeaf.nodePK[p].P = parentLeaf.nodePK[ps].P;
                                parentLeaf.nodePK[p].K = parentLeaf.nodePK[ps].K;
                                p++;
                            }
                        }
                    }
                }
                else
                {
                    if (parentLeaf.next == leaf.nodeAddress)
                    {
                        parentLeaf.nodePK[p - 1].K = -1;
                        parentLeaf.next = -1;
                    }
                }
            }

            if (parent.type == 'I' && parent.nodePK[leafSize / 2 - 1].K == -1)//Verifica que hay almenos n/2 nodos en la hoja padre
            {
                //Fusion entre padres
                WriteFile(parentLeaf);
                DeleteLeaf(leaf);

                LoanParent(parentLeaf);
            }
            else
            {
                if (parent.type == 'R' && parent.nodePK[0].K != -1)
                {
                    WriteFile(parentLeaf);
                    DeleteLeaf(leaf);
                }
                else
                {
                    DeleteLeaf(parentLeaf);
                    DeleteLeaf(leaf);

                    root = null;
                    attribTree.indexAddress = leftLeaf.nodeAddress;
                    WriteAttribute(attribTree);
                }

            }
        }

        private void LoanParent(Node leafParent)
        {
            UpdateGrid();
            MessageBox.Show("");
            //Busca al padre del padre
            Node grandParent = SearchGrandParent(leafParent);

            //Busca hermano derecho del padre
            Node rightParentLeaf = FindRightParentLeaf(leafParent, grandParent);

            if (rightParentLeaf != null && rightParentLeaf.nodePK[leafSize / 2].K != -1)
            {
                //Toma el primer nodo padre del padre_derecho
                long dirRightLeaf = rightParentLeaf.nodePK[0].P;
                int valueRightLeaf = rightParentLeaf.nodePK[0].K;
                //

                //Ordena datos
                //Actualiza al hermano derecho recorriendolo
                int p = 0;

                for (int ps = p + 1; ps <= leafSize; ps++)
                {
                    if (ps == leafSize)
                    {
                        rightParentLeaf.nodePK[ps - 1].P = rightParentLeaf.next;
                        rightParentLeaf.nodePK[ps - 1].K = -1;
                        rightParentLeaf.next = -1;
                    }
                    else
                    {
                        rightParentLeaf.nodePK[p].P = rightParentLeaf.nodePK[ps].P;
                        rightParentLeaf.nodePK[p].K = rightParentLeaf.nodePK[ps].K;
                        p++;
                    }
                }
                WriteFile(rightParentLeaf);//Actualiza

                //Toma el dato de padre del padre
                int valueGrandparent = -1;
                int posLeaf = -1;
                for (p = 0; p <= leafSize; p++)
                {
                    if (p < leafSize)
                    {
                        if (grandParent.nodePK[p].P == rightParentLeaf.nodeAddress)
                        {
                            valueGrandparent = grandParent.nodePK[p - 1].K;
                            posLeaf = p - 1;
                            p = leafSize;
                        }
                    }
                    else if (grandParent.next == rightParentLeaf.nodeAddress)
                    {
                        valueGrandparent = grandParent.nodePK[p - 1].K;
                        posLeaf = p - 1;
                    }
                }

                //Actualiza al padre del padr
                grandParent.nodePK[posLeaf].K = valueRightLeaf;
                WriteFile(grandParent);//Actualiza

                //Escribe el nodo prestado en el padre actual
                leafParent.nodePK[leafSize / 2 - 1].K = valueGrandparent;
                leafParent.nodePK[leafSize / 2].P = dirRightLeaf;

                //Actualizar Datos
                WriteFile(leafParent);
            }
            else
            {
                Node LeftParentLeaf = findLeftParentLeaf(leafParent, grandParent);
                if (LeftParentLeaf != null && LeftParentLeaf.nodePK[leafSize / 2].K != -1)
                {
                    //Busca el nodo que va aprestar el hermano izquierdo
                    long dirLeaf = -1;
                    int valueLeaf = -1;
                    for (int u = leafSize - 1; u >= leafSize / 2; u--)
                    {
                        if (LeftParentLeaf.nodePK[u].K != -1)
                        {
                            if (u + 1 < leafSize)
                            {
                                dirLeaf = LeftParentLeaf.nodePK[u + 1].P;
                                LeftParentLeaf.nodePK[u + 1].P = -1;
                            }
                            else
                            {
                                dirLeaf = LeftParentLeaf.next;
                                LeftParentLeaf.next = -1;
                            }

                            valueLeaf = LeftParentLeaf.nodePK[u].K;
                            LeftParentLeaf.nodePK[u].K = -1;
                            u = 0;
                        }
                    }

                    //Actualiza hermano que presta
                    WriteFile(LeftParentLeaf);

                    //Busca el apuntador en el padre del padre
                    int valueParent = -1;
                    for (int p = 0; p < leafSize; p++)
                    {
                        if (grandParent.nodePK[p].P == LeftParentLeaf.nodeAddress)
                        {
                            valueParent = grandParent.nodePK[p].K;
                            //Cambia el apuntador padre del padre 
                            grandParent.nodePK[p].K = valueLeaf;
                            p = leafSize;
                        }
                    }

                    //Actualiza padre del padre
                    WriteFile(grandParent);

                    //Recorre el padre actual para insertar los valores de prestamo
                    for (int p = leafSize / 2; p > 0; p--)
                    {
                        leafParent.nodePK[p].P = leafParent.nodePK[p - 1].P;
                        leafParent.nodePK[p].K = leafParent.nodePK[p - 1].K;
                    }
                    leafParent.nodePK[0].P = dirLeaf;
                    leafParent.nodePK[0].K = valueParent;

                    //Actualizar el padre actual
                    WriteFile(leafParent);
                }
                else
                {
                    //FUSION de padres
                    if (rightParentLeaf != null)
                    {
                        MergeRightParent(leafParent, grandParent, rightParentLeaf);
                    }
                    else if (LeftParentLeaf != null)
                    {
                        MergeLeftParents(leafParent, grandParent, LeftParentLeaf);
                    }
                }
            }
        }

        private void MergeRightParent(Node parentLeaf, Node grandParentLeaf, Node rightLeaf)
        {
            //Baja el Padre del padre
            int valueParent = -1;
            int posParent = -1;
            for (int p = 0; p < leafSize; p++)
            {
                if (grandParentLeaf.nodePK[p].P == parentLeaf.nodeAddress)
                {
                    valueParent = grandParentLeaf.nodePK[p].K;
                    posParent = p;
                    p = leafSize;
                }
            }
            //Reordena Padre del padre (Recorrerlo)
            for (int p = posParent + 1; p <= leafSize; p++)
            {
                if (p < leafSize)
                {
                    grandParentLeaf.nodePK[posParent].K = grandParentLeaf.nodePK[p].K;
                    if (p + 1 < leafSize)
                    {
                        grandParentLeaf.nodePK[p].P = grandParentLeaf.nodePK[p + 1].P;
                    }
                    else
                    {
                        grandParentLeaf.nodePK[p].P = grandParentLeaf.next;
                    }

                    posParent++;
                }
                else
                {
                    grandParentLeaf.nodePK[p - 1].K = -1;
                    grandParentLeaf.next = -1;
                }
            }

            //Actualizar Padre del padre
            WriteFile(grandParentLeaf);

            //Pasar los datos del padre hermano al padre actual
            parentLeaf.nodePK[leafSize / 2 - 1].K = valueParent;
            int ph = 0;
            for (int kp = leafSize / 2; kp <= leafSize; kp++)
            {
                if (kp < leafSize)
                {
                    parentLeaf.nodePK[kp].P = rightLeaf.nodePK[ph].P;
                    rightLeaf.nodePK[ph].P = -1;
                    parentLeaf.nodePK[kp].K = rightLeaf.nodePK[ph].K;
                    rightLeaf.nodePK[ph].K = -1;
                    ph++;
                }
                else
                {
                    parentLeaf.next = rightLeaf.nodePK[ph].P;
                }
            }

            //Actualiza Padre Actual
            WriteFile(parentLeaf);

            //Actualizar hoja a borarr
            WriteFile(rightLeaf);
            //Borrar del arbol el padre_hermano
            DeleteLeaf(rightLeaf);

            //Verifica que el padre padre tenga los Nodos suficientes
            if (grandParentLeaf.type == 'R')
            {
                if (grandParentLeaf.nodePK[0].K == -1)//Si ya no tine elementos se convierte en el nuevo padre
                {
                    parentLeaf.type = 'R';
                    WriteFile(parentLeaf);
                    attribTree.indexAddress = parentLeaf.nodeAddress;
                    WriteAttribute(attribTree);
                    DeleteLeaf(grandParentLeaf);

                    root = parentLeaf;
                }
            }
            else if (grandParentLeaf.type == 'I')
            {
                if (grandParentLeaf.nodePK[leafSize / 2 - 1].K == -1)//
                {
                    LoanParent(grandParentLeaf);
                }
            }
        }

        private void MergeLeftParents(Node leafParent, Node grandParent, Node leftLeaf)
        {
            //Baja el Padre del padre
            int valueParent = -1; int pos_p = -1;
            for (int p = 0; p < leafSize; p++)
            {
                if (grandParent.nodePK[p].P == leftLeaf.nodeAddress)
                {
                    valueParent = grandParent.nodePK[p].K;
                    pos_p = p;
                    p = leafSize;
                }
            }
            //Reordena Padre del padre (Recorrerlo)
            for (int p = pos_p + 1; p <= leafSize; p++)
            {
                if (p < leafSize)
                {
                    grandParent.nodePK[pos_p].K = grandParent.nodePK[p].K;
                    if (p + 1 < leafSize)
                    { grandParent.nodePK[p].P = grandParent.nodePK[p + 1].P; }
                    else
                    { grandParent.nodePK[p].P = grandParent.next; }

                    pos_p++;
                }
                else
                {
                    grandParent.nodePK[p - 1].K = -1;
                    grandParent.next = -1;
                }
            }
            //Actualizar Padre del padre
            WriteFile(grandParent);

            //Pasar los datos del padre actual al padre hermano
            leftLeaf.nodePK[leafSize / 2].K = valueParent;
            pos_p = 0;
            for (int kp = leafSize / 2 + 1; kp <= leafSize; kp++)
            {
                if (kp < leafSize)
                {
                    leftLeaf.nodePK[kp].P = leafParent.nodePK[pos_p].P;
                    leftLeaf.nodePK[kp].K = leafParent.nodePK[pos_p].K;
                    pos_p++;
                }
                else
                {
                    leftLeaf.next = leafParent.nodePK[pos_p].P;
                }
            }

            //Actualizar padre hermano
            WriteFile(leftLeaf);

            //Elimina padre actual
            DeleteLeaf(leafParent);

            //Verifica que el padre padre tenga los Nodos suficientes
            if (grandParent.type == 'R')
            {
                if (grandParent.nodePK[0].K == -1)//Si ya no tine elementos se convierte en el nuevo padre
                {
                    leftLeaf.type = 'R';
                    WriteFile(leftLeaf);
                    attribTree.indexAddress = leftLeaf.nodeAddress;
                    WriteAttribute(attribTree);
                    DeleteLeaf(grandParent);

                    root = leftLeaf;
                }
            }
            else if (grandParent.type == 'I')
            {
                if (grandParent.nodePK[leafSize / 2 - 1].K == -1)//
                {
                    LoanParent(grandParent);
                }
            }
        }

        private Node findLeftParentLeaf(Node parentLeaf, Node grandParent)
        {
            Node rightParent = null;
            long dirLeftLeaf = -1;

            for (int i = 0; i < leafSize; i++)//Busca el apuntador al padre actual
            {
                if (grandParent.nodePK[i].P == parentLeaf.nodeAddress)
                {
                    //El apuntador que anterior es el hermano izquierdo
                    if (i - 1 >= 0)//Busca en el Sig como Hermano derecho
                    {
                        dirLeftLeaf = grandParent.nodePK[i - 1].P;
                    }
                    i = leafSize;
                }
            }

            //NOTA:
            //Si p - 1 es menor a 0 no existe hermano izquierdo

            if (dirLeftLeaf != -1)
            {
                for (int j = 0; j < primaryTree.Count; j++)
                {
                    if (primaryTree[j].type == 'I' && primaryTree[j].nodeAddress == dirLeftLeaf)
                    {
                        rightParent = primaryTree[j];
                        j = primaryTree.Count;
                    }
                }
            }
            return rightParent;
        }

        private Node FindRightParentLeaf(Node parentLeaf, Node grandParent)
        {
            Node rightParent = null;
            long dirRightLeaf = -1;

            for (int i = 0; i < leafSize; i++)//Busca el apuntador al padre actual
            {
                if (grandParent.nodePK[i].P == parentLeaf.nodeAddress)
                {
                    //El apuntador que sigue es el hermano derecho
                    if (i + 1 == leafSize)//Busca en el Sig como Hermano derecho
                    {
                        dirRightLeaf = grandParent.next;
                    }
                    else
                    {
                        dirRightLeaf = grandParent.nodePK[i + 1].P;
                    }

                    i = leafSize;
                }
            }

            //NOTA:
            //No busco en Sig por que si ese es al padre actual entonce no existe un hermano derecho

            if (dirRightLeaf != -1)
            {
                for (int j = 0; j < primaryTree.Count; j++)
                {
                    if (primaryTree[j].type == 'I' && primaryTree[j].nodeAddress == dirRightLeaf)
                    {
                        rightParent = primaryTree[j];
                        j = primaryTree.Count;
                    }
                }
            }


            return rightParent;
        }

        private void MergeRightLeaf(Node leaf, Node rightLeaf, Node parentLeaf)
        {
            //Pasa los datos del hermano derecho a la hoja actual
            int pos = leafSize / 2 - 1;
            for (int nd = 0; nd < leafSize / 2; nd++)
            {
                leaf.nodePK[pos].P = rightLeaf.nodePK[nd].P;
                leaf.nodePK[pos].K = rightLeaf.nodePK[nd].K;

                rightLeaf.nodePK[nd].P = -1;
                rightLeaf.nodePK[nd].K = -1;

                pos++;
            }

            leaf.next = rightLeaf.next;

            //Actualizar datos
            WriteFile(leaf);
            WriteFile(rightLeaf);

            //Actualiza al padre recorriendolo
            for (int p = 0; p < leafSize; p++)
            {
                if (parentLeaf.nodePK[p].P == rightLeaf.nodeAddress)
                {
                    if (p - 1 >= 0) { parentLeaf.nodePK[p - 1].K = parentLeaf.nodePK[p].K; }

                    for (int ps = p + 1; ps <= leafSize; ps++)
                    {
                        if (ps == leafSize)
                        {
                            parentLeaf.nodePK[ps - 1].P = parentLeaf.next;
                            parentLeaf.nodePK[ps - 1].K = -1;
                            parentLeaf.next = -1;
                        }
                        else
                        {
                            parentLeaf.nodePK[p].P = parentLeaf.nodePK[ps].P;
                            parentLeaf.nodePK[p].K = parentLeaf.nodePK[ps].K;
                            p++;
                        }
                    }
                }
            }

            if (parentLeaf.type == 'I' && parentLeaf.nodePK[leafSize / 2 - 1].K == -1)//Verifica que hay almenos n/2 nodos en la hoja padre
            {
                WriteFile(parentLeaf);
                DeleteLeaf(rightLeaf);

                LoanParent(parentLeaf);
            }
            else
            {
                if (parentLeaf.type == 'R' && parentLeaf.nodePK[0].K != -1)
                {
                    WriteFile(parentLeaf);
                    DeleteLeaf(rightLeaf);
                }
                else
                {
                    DeleteLeaf(parentLeaf);
                    DeleteLeaf(rightLeaf);

                    root = null;
                    attribTree.indexAddress = leaf.nodeAddress;
                    WriteAttribute(attribTree);
                }

            }
        }

        private void DeleteLeaf(Node leaf)
        {
            for (int i = 0; i < primaryTree.Count; i++)
            {
                if (primaryTree[i].nodeAddress == leaf.nodeAddress)
                {
                    primaryTree.RemoveAt(i);
                    i = primaryTree.Count;
                }
            }
        }
        
        private void LoadTree()
        {
            long rootL = attribTree.indexAddress;
            
            Reader = new BinaryReader(File.OpenRead(pathTree));
            Reader.BaseStream.Seek(rootL, SeekOrigin.Begin);

            Node rootNode = new Node(Reader.ReadChar(), Reader.ReadInt64());

            for (int k = 0; k < rootNode.nodePK.Length; k++)
            {
                rootNode.nodePK[k].P = Reader.ReadInt64();
                rootNode.nodePK[k].K = Reader.ReadInt32();
            }
            rootNode.next = Reader.ReadInt64();
            primaryTree.Add(rootNode);

            int count = 0;
            long dirLeaf = rootNode.nodePK[count].P;

            while (dirLeaf != -1 && count <= 4)
            {
                ReadNext(dirLeaf);

                count++;
                if (count < 4)
                    dirLeaf = rootNode.nodePK[count].P;
                else
                    dirLeaf = rootNode.next;
            }

            Reader.Close();
            root = rootNode;
            UpdateGrid();
        }

        private void ReadNext(long dir)
        {
            Node auxLeaf = null;

            Reader.BaseStream.Seek(dir, SeekOrigin.Begin);
            auxLeaf = new Node(Reader.ReadChar(), Reader.ReadInt64());

            if (auxLeaf.nodeAddress > Ultima_direccion) { Ultima_direccion = auxLeaf.nodeAddress; }

            for (int k = 0; k < auxLeaf.nodePK.Length; k++)
            {
                auxLeaf.nodePK[k].P = Reader.ReadInt64();
                auxLeaf.nodePK[k].K = Reader.ReadInt32();
            }

            auxLeaf.next = Reader.ReadInt64();
            primaryTree.Add(auxLeaf);

            if (auxLeaf.type == 'I')
            {
                for (int i = 0; i < auxLeaf.nodePK.Length; i++)
                {
                    if (auxLeaf.nodePK[i].P != -1)
                    {
                        ReadNext(auxLeaf.nodePK[i].P);
                    }
                }

                if (auxLeaf.next != -1)
                    ReadNext(auxLeaf.next); 
            }
        }

        private void WriteFile(Node leaf)
        {
            Writer = new BinaryWriter(File.OpenWrite(pathTree));
            Writer.Seek((int)leaf.nodeAddress, SeekOrigin.Begin);
            Writer.Write(leaf.type);
            Writer.Write(leaf.nodeAddress);

            for (int w = 0; w < leafSize; w++)
            {
                Writer.Write(leaf.nodePK[w].P);
                Writer.Write(leaf.nodePK[w].K);
            }

            Writer.Write(leaf.next);

            Writer.Close();
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

        private void SetHeadersGrid(int size)
        {
            gridTree.Columns.Add("Tipo", "Tipo");
            gridTree.Columns.Add("Direccion", "Direccion");

            for (int i = 1; i <= size; i++)
            {
                gridTree.Columns.Add("P" + i.ToString(), "P" + i.ToString());
                gridTree.Columns.Add("K" + i.ToString(), "k" + i.ToString());
                if (i == size)
                {
                    gridTree.Columns.Add("P" + (size + 1).ToString(), "P" + (size + 1).ToString());
                }
            }
        }

        private void UpdateGrid()
        {
            gridTree.Rows.Clear();

            char tipo = ' ';
            long dirDato = -1;
            long p1, p2, p3, p4; p1 = p2 = p3 = p4 = 0;
            int k1, k2, k3, k4; k1 = k2 = k3 = k4 = 0;
            long sig = -1;

            if (primaryTree.Count != 0)
            {
                for (int i = 0; i < primaryTree.Count; i++)
                {
                    tipo = primaryTree[i].type;
                    dirDato = primaryTree[i].nodeAddress;

                    p1 = primaryTree[i].nodePK[0].P;
                    k1 = primaryTree[i].nodePK[0].K;

                    p2 = primaryTree[i].nodePK[1].P;
                    k2 = primaryTree[i].nodePK[1].K;

                    p3 = primaryTree[i].nodePK[2].P;
                    k3 = primaryTree[i].nodePK[2].K;

                    p4 = primaryTree[i].nodePK[3].P;
                    k4 = primaryTree[i].nodePK[3].K;

                    sig = primaryTree[i].next;

                    gridTree.Rows.Add(new object[] { tipo, dirDato, p1, k1, p2, k2, p3, k3, p4, k4, sig });
                    
                }
                PaintGrid();
            }
        }

        private void gridTree_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            
        }

        public void PaintGrid()
        {
            for (int i = 0; i < gridTree.ColumnCount - 1; i++)
            {
                string name = gridTree.Columns[i].Name;
                for (int j = 0; j < gridTree.RowCount; j++)
                {
                    if(gridTree.Rows[j].Cells[i].Value.ToString() != "-1" && name.Contains("K"))
                    {
                        switch(gridTree.Rows[j].Cells[0].Value.ToString())
                        {
                            case "H":
                                gridTree.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(113, 203, 74);
                                break;

                            case "I":
                                gridTree.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(226, 68, 236);
                                break;

                            case "R":
                                gridTree.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(96, 213, 220);
                                break;
                        }
                        gridTree.Rows[j].Cells[i].Style.CustomizeFill = true;
                    }
                }
            }
        }
    }
}
