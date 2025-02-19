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

                    ClearTextboxes();

                    string json = CreateFhirBundleFromCcda(filePath);

                    //Display the JSON FHIR bundle
                    txtFhir.Text = json;
                }
            }
        }

        /// <summary>
        /// Takes a CCD in XML format and returns a FHIR JSON bundle
        /// </summary>
        /// <param name="filename"></param>
        string CreateFhirBundleFromCcda(string filename)
        {
            try
            {
                // Load the CCD document
                string ccdSource = File.ReadAllText(filename);

                //Make any modifications to the XML source CCD content here
                string ccdCleaned = CleanCcdLanguage(ccdSource);

                //let us see the original content
                txtCcd.Text = ccdCleaned;

                //Create a new instance of the converter
                //**Last parameter specifies PatientOnly conversion - don't look for Organization
                var executor = new CCdaToFhirExecutor(null, null, true, true);

                //The converter expects an XDocument
                XDocument x = XDocument.Parse(ccdCleaned);

                //more cleanup - look for missing attribute in administrativeGenderCode
                x = CleanGender(x);

                //Convert the CCD into FHIR JSON bundle
                var bundle = executor.Execute(x);

                // Serialize the FHIR bundles to JSON
                var serializer = new FhirJsonSerializer();
                string json = serializer.SerializeToString(bundle);

                return json;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Fixes a problem with variants of French that fail in the converter
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        string CleanCcdLanguage(string original)
        {
            //The converter fails on LanguageCode if value is "fr-CA" or "fr-CN"
            //So we replace it with "fr"
            string ccdModified = original.Replace("fr-CA", "fr");
            ccdModified = ccdModified.Replace("fr-CN", "fr");

            return ccdModified;
        }

        /// <summary>
        ///The converter fails on if the AdministrativeGenderCode doesn't include the DisplayName
        ///So we add the display name for each gender if it's missing
        /// </summary>
        /// <param name="x">XDocument of complete CCD</param>
        /// <returns>Modified (if needed) XDocument of complete CCD</returns>
        XDocument CleanGender(XDocument x)
        {
            // Define the namespace or the node search will fail
            XNamespace ns = "urn:hl7-org:v3";

            //get the complete administrativeGenderCode node
            var genderCodeElement = x.Descendants(ns + "administrativeGenderCode").FirstOrDefault();

            if (genderCodeElement != null)
            {
                XAttribute displayNameAttribute = genderCodeElement.Attribute("displayName");

                //the value doesn't exist we've got a problem so add it to the node
                if (displayNameAttribute == null)
                {
                    //retrieve the value of the code attribute so we can add the appropriate displayName
                    XAttribute codeAttribute = genderCodeElement.Attribute("code");
                    if (codeAttribute != null)
                    {
                        switch (codeAttribute.Value)
                        {
                            case "F":
                                genderCodeElement.Add(new XAttribute("displayName", "Female"));
                                break;
                            case "M":
                                genderCodeElement.Add(new XAttribute("displayName", "Male"));
                                break;
                            case "Other":
                                genderCodeElement.Add(new XAttribute("displayName", "Other"));
                                break;
                            case "Unknown":
                                genderCodeElement.Add(new XAttribute("displayName", "Unknown"));
                                break;
                        }
                    }
                }
            }

            return x;
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
