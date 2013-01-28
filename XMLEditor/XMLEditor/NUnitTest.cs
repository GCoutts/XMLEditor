using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace XMLEditor
{
    [TestFixture]
    public class NUnitTest : NUnitFormTest
    {
        Form1 formTest;
 
        
        public NUnitTest()
        {

            base.init();

            formTest = new Form1();
            formTest.Show();

            formTest.Name = "TestForm";
            


            
        }
        /// <summary>
        /// Tests the buttons of the program
        /// </summary>
        /// <returns></returns>
        public bool GuiTest()
        {
            ButtonTester openTester = new ButtonTester("btnOpen", "TestForm");
            ExpectFileDialog(openFileDialogHandler);
            openTester.Click();


            ButtonTester saveTester = new ButtonTester("btnSave", "TestForm");
            ExpectFileDialog(saveFileDialogHandler);
            saveTester.Click();


            ButtonTester schemaTester = new ButtonTester("btnSchema", "TestForm");
            ExpectFileDialog(schemaFileDialogHandler);
            schemaTester.Click();





            return true;
        }

        public void TestClose()
        {
            ButtonTester closeTester = new ButtonTester("btnClose", "TestForm");
            closeTester.Click();
        }

        /// <summary>
        /// Tests the validate function to enure it will identify invalid xml
        /// </summary>
        /// <returns></returns>
        public bool UnitTest()
        {
            File testFile = new File();

            string invalid = "<item> \n <title>Hide your heart</title>\n  <price>9.90</price>\n        <quantity>1</quantity>\n  </item>";
            testFile.schemaLoc = "C:\\Users\\Graham\\Documents\\XMLEdit\\shiporder.xsd";
          
            
            if (testFile.validate(invalid))
            {
                return false;
            }



            return true;
        }




        // handles the openFileDailogs
        public void openFileDialogHandler()
        {

            FileDialogTester dialogTester = new FileDialogTester("Open");

            dialogTester.ClickCancel();
        }

        // handles the openFileDailogs
        public void saveFileDialogHandler()
        {

            FileDialogTester dialogTester = new FileDialogTester("Save As");

            dialogTester.ClickCancel();
        }

        // handles the openFileDailogs
        public void schemaFileDialogHandler()
        {

            FileDialogTester dialogTester = new FileDialogTester("Open");

            dialogTester.ClickCancel();
        }
        
    }

}
