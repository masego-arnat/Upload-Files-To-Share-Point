using Microsoft.SharePoint.Client;
using System;
using System.Net;

namespace hello
{
    class Program
    {
        public static void Context_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            e.WebRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
        }

        static void Main(string[] args)
        {
            try
            {
                using (ClientContext ctxremote = new ClientContext("http://spdev2016/sites/CDRS"))
                {
                    ctxremote.Credentials = new NetworkCredential("sp.admin", "!GeS@437!@#", "green");
                    ctxremote.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(Context_ExecutingWebRequest);

                    List list = ctxremote.Web.Lists.GetByTitle("Attachments 2023");
                    ctxremote.Load(list);

                    ctxremote.ExecuteQueryAsync().Wait();

                    string filePath = @"C:\Users\romario.arnat\Downloads\- LandsDeedsCustomers (1).pdf";

                    // Read the file contents
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    /*
                    TODO: Implement File upload
                    Read the article at https://vitalyzhukov.com/en/csom-upload-document
                    And implement a way to upload a pdf file to the 'Attachments 2023' list
                    */
                    // Information about the file
                    var fileInformation = new FileCreationInformation
                    {
                        // Server relative url of the document
                        Url = "fileName2.pdf",
                        // Overwrite file if it's already exist
                        Overwrite = true,
                        // Content of the file
                        Content = fileBytes
                    };
                    // Upload the file to root folder of the Document library
                    list.RootFolder.Files.Add(fileInformation);

                    ctxremote.ExecuteQueryAsync().Wait();

                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}