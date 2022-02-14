using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuplicitFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AddFolder_Click(object sender, EventArgs e)
        {
            var od = new FolderBrowserDialog();
            if (od.ShowDialog() == DialogResult.OK)
            {
                comboBox1.Items.Add(od.SelectedPath);
            }
        }

        private void RemoveFolder_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex >= 0 && comboBox1.Items?[comboBox1.SelectedIndex] != null)
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            }
            
            if (comboBox1.Items?.Count <= 0)
            {
                comboBox1.Text = null;
            }
        }

        private void CheckDuplicity_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            EnableForm(false);

            try
            {
                var tasks = GetTasks(comboBox1.Items.Cast<string>());

                Task.WhenAll(tasks)
                    .GetAwaiter()
                    .OnCompleted(() => DisplayData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                EnableForm(true);
            }
        }

        private static void GetDuplicityForDirectory(string directoryPath) 
            => Data.GetInstance().Add(Engine.CheckDuplicity(directoryPath));

        private static IEnumerable<Task> GetTasks(IEnumerable<string> coll)
        {
            var tasks = new List<Task>();

            foreach (var item in coll)
            {
                tasks.Add(Task.Run(() => GetDuplicityForDirectory(item)));
            }

            return tasks;
        }

        private void DisplayData()
        {
            foreach (var item in Data.GetInstance().GetDict().OrderByDescending(o => o.Key))
            {
                var node = new TreeNode(item.Key.ToFriendlySizeString() + $" ({item.Value.Count})");
                node.Nodes.AddRange(item.Value.Select(s => new TreeNode(s)).ToArray());
                treeView1.Nodes.Add(node);
            }

            MessageBox.Show($"Nalezeno [{ Data.GetInstance().GetDict().Count}] možných duplicit", "Informace", MessageBoxButtons.OK, MessageBoxIcon.Information);
            EnableForm(true);
        }

        private void EnableForm(bool enable)
        {
            button1.Enabled = enable;
            button2.Enabled = enable;
            button3.Enabled = enable;

            Refresh();
        }
    }
}
