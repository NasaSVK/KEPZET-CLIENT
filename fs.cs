using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BarcodeReader_ce_CF2
{
    class fs
    {
        private string logFileName = @"\Program Files\barcodereader_ce_cf2\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

        public List<string> config = new List<string>();

        public int
            serverIp = 0,
            serverPort = 1,
            localPort = 2,
            userPass = 3,
            adminPass = 4,
            lightTime = 5,
            audioVolume = 6,
            allowManual = 7,
            dbConnStr = 8;

        public fs()
        {

        }
        //private methods
        private void checkIfLogExists()
        {
            if (!System.IO.File.Exists(logFileName))
            {
                using (FileStream createdFile = new FileStream(logFileName, FileMode.Create))
                {
                    createdFile.Close();
                }

            }
        }

        //public methods
        public void logData(string data)
        {
            checkIfLogExists();
            using (StreamWriter output = File.AppendText(logFileName))
            {

                output.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + data);
            }
        }

        public void readConfig()
        {
            using (StreamReader sr = new StreamReader(@"\Program Files\barcodereader_ce_cf2\config.txt"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    config.Add(line);
                }
                sr.Close();

            }

        }

        public int getHall {
            get { return config.Count == 0 ? -1 : Int32.Parse(config[config.Count - 1]);} 
        }

        public void writeConfig(List<string> c)
        {
            using (FileStream createdFile = File.Create(@"\Program Files\barcodereader_ce_cf2\config.txt"))
            {
                createdFile.Close();
            }
            using (StreamWriter output = File.AppendText(@"\Program Files\barcodereader_ce_cf2\config.txt"))
            {
                foreach (string line in c)
                    output.WriteLine(line);
            }
        }

    }
}
