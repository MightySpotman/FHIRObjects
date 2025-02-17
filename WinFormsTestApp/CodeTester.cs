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
            CreateFhirBundleFromCcda("Transition_of_Care_Referral_Summary.xml");
        }

        static void CreateFhirBundleFromCcda(string filename)
        {
            // Load the CCD-A document
            string ccdaContent = File.ReadAllText(filename);

            var executor = new CCdaToFhirExecutor();

            //var cCda = XDocument.Parse(cCdaText); // or
            XDocument x = XDocument.Load(filename);

            var bundle = executor.Execute(x);


            // Convert the CCD-A document to FHIR bundles
            // DarenaSolutions.CCdaToFhirConverter converter = new CCdaToFhirConverter();
            //var bundles = converter.Convert(ccdaContent);

            // Serialize the FHIR bundles to JSON
            var serializer = new FhirJsonSerializer();
            //foreach (var bundle in bundles)
            //{
            string json = serializer.SerializeToString(bundle);
            //txtTest1.Text = json;
            Console.WriteLine(json);
            //}
        }

        //void test()
        //{
        //    Patient p = new Patient();

        //    var serializer = new FhirJsonSerializer();
        //    string json = serializer.SerializeToString(p);
        //    // p.Gender = "Male";
        //    // Set the textbox text to the JSON representation
        //    txtTest1.Text = json;
        //}
    }
}
