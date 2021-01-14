using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HeaviestSubgraphConnected.Helper;
using static HeaviestSubgraphConnected.Program;

namespace HeaviestSubgraphConnected
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeListView();
            
        }

        //Initialise Listview for Subgraph
        public void InitializeListView()
        {

            lvwHeaviestSub.View = View.Details;
            this.lvwHeaviestSub.BackColor = System.Drawing.SystemColors.Control;
            //Add columns to listview
            lvwHeaviestSub.Columns.Add("From", 100);
            lvwHeaviestSub.Columns.Add("To", 100);
            lvwHeaviestSub.Columns.Add("Weight", 50);
            lvwHeaviestSub.GridLines = true;

            //Sort the items in the list in ascending order.
            lvwHeaviestSub.Sorting = SortOrder.Ascending;

            //Select the item and subitems when selection is made.
            lvwHeaviestSub.FullRowSelect = true;
        }

        //print the results on the screen
        public void UpdateText(List<int> lstMaxWeight, subGraph[] lstSubGraph,int k)
        {

            try
            {
                string sLine="";
                //display K value
                txtKValue.Text = k.ToString();
                
                StreamWriter sw = new StreamWriter(outputFile);   
            
                int i = 0;
                //add startvertices if more than one to a list
                //foreach (var s in lstMaxWeight)
                //{
                i = lstMaxWeight[0];
                lstStartVertex.Items.Add(lstSubGraph[i].n);
                lstBestSolution.Items.Add(lstSubGraph[i].weight);

                string strItem1, strItem2;
                foreach (var w in lstSubGraph[i].subgraphVertices)
                {
                    termID.TryGetValue(w.Item1, out strItem1);
                    termID.TryGetValue(w.Item2, out strItem2);

                    //Add the items to the ListView.
                    ListViewItem item1 = new ListViewItem(strItem1, 0);
                    item1.Checked = true;
                    item1.SubItems.Add(strItem2);
                    item1.SubItems.Add(w.Item3.ToString());
                    lvwHeaviestSub.Items.AddRange(new ListViewItem[] { item1 });

                    sLine = w.Item1.ToString() + " " + w.Item2.ToString() + " " + w.Item3.ToString();
                    lbSubgraphNodes.Items.Add(sLine);

                    sw.WriteLine(strItem1 + " " + strItem2 + " " + w.Item3.ToString());
                    //sw.WriteLine(sLine);
                }
                //}

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            
            //Execution Time
            TimeSpan time = TimeSpan.FromMilliseconds(Watch.ElapsedMilliseconds);
            txtProcessingTime.Text = time.ToString(@"hh\:mm\:ss\:fff");
            txtFileName.Text = outputFile;


            using (StreamWriter w = File.AppendText("Stats.txt"))
            {
                string sLine = outputFile + " " + lstBestSolution.Items[0].ToString() + " " + lstStartVertex.Items[0].ToString() + " " + time.ToString(@"hh\:mm\:ss\:fff"); ;
                w.WriteLine(sLine);
            }
                        
        }

    }
}
