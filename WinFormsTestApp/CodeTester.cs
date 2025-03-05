using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using DarenaSolutions.CCdaToFhirConverter;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir;
using System.Diagnostics;
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

                    if (IsValidXml(filePath))
                    {
                        if (ContainsPatientRoleNode(filePath))
                        {
                            string json = CreateFhirBundleFromCcda(filePath);

                            //Display the JSON FHIR bundle
                            txtFhir.Text = json;
                        }
                        else
                        {
                            txtFhir.Text = "Missing the <patientRole> node";
                        }
                    }
                    else
                    {
                        //detail is now done in IsValidXml()
                        //txtFhir.Text = "*** Not Valid XML ***";
                    }
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

        public bool IsValidXml(string filename)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                return true; // If no exception is thrown, the XML is valid
            }
            catch (XmlException ex)
            {
                string msg = $"Filename: {filename}\r\n" +
                    $"XML is invalid.\r\n" +
                    $"Error Message: {ex.Message}\r\n"; 
                Debug.WriteLine(msg);

                //just here to make it ease for testing
                txtFhir.Text = msg;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Message: {ex.Message}");
            }

            return false;
        }

        public static bool ContainsPatientRoleNode(string filename)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);

                // Check if the <patientRole> node is present
                XmlNode patientRoleNode = xmlDoc.SelectSingleNode("//*[local-name()='patientRole']");
                return patientRoleNode != null;
            }
            catch (XmlException)
            {
                Debug.WriteLine("The XML file is not valid.");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public void ProcessDirectory(string directoryPath)
        {
            try
            {
                txtCcd.Text = "Valid";
                txtFhir.Text = "InValid";

                // Get all files in the directory
                string[] files = Directory.GetFiles(directoryPath);

                // Iterate over each file and call the CheckFile method
                foreach (string file in files)
                {
                    if (IsValidXml(file))
                    {
                        if (ContainsPatientRoleNode(file))
                        {
                            txtCcd.Text = txtCcd.Text + "\r\n" + file;
                        }
                        else
                        {
                            txtFhir.Text = txtFhir.Text + "\r\n" + file + " ** Missing <patientRole> **";
                        }
                    }
                    else
                    {
                        txtFhir.Text = txtFhir.Text + "\r\n" + file + " ** Invalid XML **";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while processing the directory: {ex.Message}");
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
            ProcessDirectory("E:\\AC\\FHIR\\Deidentified_CCDs");
        }
    }
}
