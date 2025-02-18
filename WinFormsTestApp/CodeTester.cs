using System;
using System.IO;
using System.Xml.Linq;
using DarenaSolutions.CCdaToFhirConverter;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir;
namespace WinFormsTestApp
{
    public partial class CodeTester : Form
    {
        public CodeTester()
        {
            InitializeComponent();
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"..\\..\\..\\CCDA_Samples";
                openFileDialog.Filter = "XML files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    CreateFhirBundleFromCcda(filePath);
                }
            }
        }

        void CreateFhirBundleFromCcda(string filename)
        {
            // Load the CCD document
            string ccdaContent = File.ReadAllText(filename);
            txtCcd.Text = ccdaContent;

            //Create a new instance of the converter
            //**Last parameter specifies PatientOnly conversion - don't look for Organization
            var executor = new CCdaToFhirExecutor(null, null, true, true);

            XDocument x = XDocument.Load(filename);

            //Convert the CCD into FHIR JSON bundle
            var bundle = executor.Execute(x);

            // Serialize the FHIR bundles to JSON
            var serializer = new FhirJsonSerializer();
            string json = serializer.SerializeToString(bundle);

            txtFhir.Text = json;
        }
    }
}
