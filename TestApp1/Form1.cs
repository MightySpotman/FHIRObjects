using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Patient p = new Patient();
            //p.Name[0].Given = "";
            var serializer = new FhirJsonSerializer();
            string json = serializer.SerializeToString(p);
           // p.Gender = "Male";
            // Set the textbox text to the JSON representation
            textBox1.Text = json;
        }
    }
}
