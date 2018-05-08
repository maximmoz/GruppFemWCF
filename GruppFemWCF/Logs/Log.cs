using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GruppFemWCF.Logs
{
    public class Log
    {
        public void CreateLogFile(string errorMessage)
        {
            CreateFolder();


            string LogDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\FindIt\Logs");
            string LogPath = Path.Combine( LogDirectoryPath + @"\Log.txt");

            using (StreamWriter sw = new StreamWriter(LogPath, true)) // lägg till true som inparameter för att appenda i filen istället för att skapa flera filer
            {
                sw.WriteLine(errorMessage + " " + DateTime.Now.ToString());
            }

        }


        private void CreateFolder()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/FindIt/Logs");
            try
            {
                if (Directory.Exists(path))
                {
                    return;
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

    }
}