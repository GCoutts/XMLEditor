using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace XMLEditor
{
    public class File
    {
        public string schemaLoc;

        /// <summary>
        /// C'tor
        /// </summary>
        public File()
        {
            schemaLoc = "";
        }


        /// <summary>
        /// Return the content of an xml file if one is chosen
        /// </summary>
        /// <param name="ret"></param>
        /// <returns></returns>
        public bool Open(ref string ret)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = "C:\\Users\\Graham\\Documents\\XMLEdit";
            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                   
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        XmlTextReader reader = new XmlTextReader(openFileDialog1.FileName);

                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element: // next node is the beining of an element
                                    {
                                        
                                        ret += "<" + reader.Name;
                                        bool isEmpty = reader.IsEmptyElement;

                                        while (reader.MoveToNextAttribute()) // Read the attributes.
                                            ret +=" " + reader.Name + "='" + reader.Value + "'";

                                        if (isEmpty)// if the element is empty
                                        {
                                            ret += "/>\n";
                                        }
                                        else // if the element contains something
                                        {
                                            ret += ">\n";
                                        }
                                        break;
                                    }

                                case XmlNodeType.Text:  // the content of an element
                                    {
                                        ret += "\t" + reader.Value + "\n";
                                        break;
                                    }
                                case XmlNodeType.EndElement:// closing tag.
                                    {
                                        ret += "</" + reader.Name + ">\n";
                                        break;
                                    }
                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return false;
                }
                
            }



            return true;
        }
        /// <summary>
        /// Accepts a string to be converted into a byte array
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// Accepts a byte array and converst it to a string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        /// <summary>
        /// Accepts the text box content to be validated and saved to an xml file
        /// </summary>
        /// <param name="content"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Save(string content, ref string error)
        {
            byte[] write = GetBytes(content);
            Stream myStream = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //XmlTextWriter writer = new XmlTextWriter();

            if (schemaLoc != "" && !validate(content))// validates the richtextbox content if a schema has been loaded
            {
                error = "XML not valid based on schema!";
                return false;
            }




            // sets up the settins for the savefiledialog
            saveFileDialog.InitialDirectory = "C:\\Users\\Graham\\Documents\\XMLEdit";
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.Title = "Save";

            // Did the user click ok?
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // can the file be opened>
                if((myStream = saveFileDialog.OpenFile()) != null)
                {
                    // write the content of the text box to the file
                    myStream.Write(write,0,write.Length);
                    myStream.Close();
                    
                }


                
            }

            return true;
        }




        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new Exception();
        }
        /// <summary>
        /// Accepts Xml as a string to be validated against the schema the user selected earlier
        /// </summary>
        /// <param name="sxml"></param>
        /// <returns></returns>
        public bool validate(string sxml)
        {
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.LoadXml(sxml);
                xmld.Schemas.Add(null, @"C:\\Users\\Graham\\Documents\\XMLEdit\\shiporder.xsd");
                xmld.Validate(ValidationCallBack);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// after the user selects a xsd file in the file dialog, the path to the chosen file
        /// is stored to be used to validate the xml at a later time.
        /// </summary>
        /// <returns></returns>
        public bool LoadSchema()
        {
            // creates the file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // sets the settings of the dialog
            openFileDialog.InitialDirectory = "C:\\Users\\Graham\\Documents\\XMLEdit";
            openFileDialog.Filter = "XSD files (*.xsd)|*.xsd|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;


            // Has the user pressed OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    if (openFileDialog.CheckFileExists)
                    {
                        // stores the path of the selected schema
                        schemaLoc = openFileDialog.FileName;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return false;
                }
            }
            return false;
        }






    }
}
