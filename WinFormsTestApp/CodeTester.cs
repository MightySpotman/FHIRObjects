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
            try
            {
                ClearTextboxes();

                // Load the CCD document
                string ccdSource = File.ReadAllText(filename);

                //Make any modifications to the CCD content here
                string ccdCleaned = CleanCcdLanguage(ccdSource);
                ccdCleaned = CleanGender(ccdCleaned);

                //let us see the original content
                txtCcd.Text = ccdCleaned;

                //Create a new instance of the converter
                //**Last parameter specifies PatientOnly conversion - don't look for Organization
                var executor = new CCdaToFhirExecutor(null, null, true, true);

                //The converter expects an XDocument
                XDocument x = XDocument.Parse(ccdCleaned);

                //Convert the CCD into FHIR JSON bundle
                var bundle = executor.Execute(x);

                // Serialize the FHIR bundles to JSON
                var serializer = new FhirJsonSerializer();
                string json = serializer.SerializeToString(bundle);

                //Display the JSON FHIR bundle
                txtFhir.Text = json;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string CleanCcdLanguage(string original)
        {
            //The converter fails on LanguageCode if value is "fr-CA" or "fr-CN"
            //So we replace it with "fr"
            string ccdModified = original.Replace("fr-CA", "fr");
            ccdModified = ccdModified.Replace("fr-CN", "fr");

            return ccdModified;
        }

        string CleanGender(string original)
        {
            //The converter fails on if the AdministrativeGenderCode doesn't include the DisplayName
            //So we add the display name for each gender

            //**check what happens if the display name is already there**

            string ccdModified = original.Replace("<administrativeGenderCode code=\"F\"", "<administrativeGenderCode code=\"F\" displayName=\"Female\"");
            ccdModified = original.Replace(@"<administrativeGenderCode code=""M""", @"<administrativeGenderCode code=""M"" displayname=""Male""");

            return ccdModified;
        }

        private void btnClearTextboxes_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
        }

        private void ClearTextboxes()
        {
            txtCcd.Text = "";
            txtFhir.Text = "";
        }
    }
}
