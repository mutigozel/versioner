using System;
using System.Collections.Generic;
using System.Linq;

namespace versioner
{
    class VersionerVD
    {
        public string Log = string.Empty;

        public void Version(string file, string version)
        {
            string path = (new System.IO.FileInfo(file).Directory.FullName);

            this.Version_ProjectFile(file, version);
        }

        public void Version_ProjectFile(string file, string version)
        {
            List<string> oldLines = System.IO.File.ReadAllLines(file).ToList();
            List<string> newLines = System.IO.File.ReadAllLines(file).ToList();

            string ptrn1 = @"""ProductVersion"" = ""8:";
            string line1 = string.Format(@"        ""ProductVersion"" = ""8:{0}""", version);

            string ptrn2 = @"""ProductCode"" = ""8:{";
            string line2 = string.Format(@"        ""ProductCode"" = ""8:{{{0}}}""", System.Guid.NewGuid().ToString().ToUpperInvariant());

            string ptrn3 = @"""PackageCode"" = ""8:{";
            string line3 = string.Format(@"        ""PackageCode"" = ""8:{{{0}}}""", System.Guid.NewGuid().ToString().ToUpperInvariant());

            for (int i = 0; i < newLines.Count(); i++)
            {
                if (newLines[i].Trim().StartsWith(ptrn1))
                {
                    newLines[i] = line1;
                    continue;
                }

                if (newLines[i].Trim().StartsWith(ptrn2))
                {
                    newLines[i] = line2;
                    continue;
                }

                if (newLines[i].Trim().StartsWith(ptrn3))
                {
                    newLines[i] = line3;
                    continue;
                }
            }

            string oldContent = string.Join(Tools.Nln, oldLines.ToArray());
            string newContent = string.Join(Tools.Nln, newLines.ToArray());

            if (oldContent == newContent)
            {
                return;
            }

            // backup and write
            try
            {
                System.IO.File.WriteAllText(file + "._versioner_backup_", oldContent, System.Text.Encoding.UTF8);
                System.IO.File.WriteAllText(file, newContent, System.Text.Encoding.UTF8);
                this.Log = this.Log + Environment.NewLine + $" ver : {version} - file : {file}";
            }
            catch (Exception)
            {
                throw new Exception("Could not change project file.");
            }
        }
    }
}
