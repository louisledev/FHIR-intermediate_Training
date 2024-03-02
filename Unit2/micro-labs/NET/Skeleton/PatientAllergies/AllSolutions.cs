using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FhirIntermediateMaEx
{
    public partial class AllSolutions : Form
    {
        public AllSolutions()
        {
            InitializeComponent();
        }

        private void AllSolutions_Load(object sender, EventArgs e)
        {
            Type formType = typeof(Form);
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                if (formType.IsAssignableFrom(type))
                {
                    string item = type.Name;
                    if (item != this.Name)
                    { this.comboAllSolutions.Items.Add(item); }
                }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Type formType = typeof(Form);
            string item = this.comboAllSolutions.Text;
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                if (formType.IsAssignableFrom(type))
                {
                    if (item == type.Name)
                    {
                        Form frmShow = (Form)Assembly.GetExecutingAssembly().CreateInstance(type.ToString());
                        frmShow.Show();
                    }
                }
        }
    }
}
