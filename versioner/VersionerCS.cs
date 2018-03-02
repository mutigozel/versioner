using System;
using System.Collections.Generic;
using System.Linq;

namespace versioner
{
    class VersionerCS
    {
        public string Log = string.Empty;

        public void Version(string file, string version)
        {
            string path = (new System.IO.FileInfo(file).Directory.FullName);

            this.Version_AssemblyInfo(path, version);
            this.Version_ProjectFile(file, version);
        }

        public void Version_AssemblyInfo(string path, string version)
        {
            string file = System.IO.Path.Combine(path, "Properties", "AssemblyInfo.cs");

            if (!System.IO.File.Exists(file))
                throw new Exception("AssemblyInfo does not exist.");

            List<string> oldLines = System.IO.File.ReadAllLines(file).ToList();
            List<string> newLines = System.IO.File.ReadAllLines(file).ToList();

            // change lines
            string ptrn1 = @"[assembly: AssemblyVersion(";
            string line1 = string.Format(@"[assembly: AssemblyVersion(""{0}"")]", version);

            string ptrn2 = @"[assembly: AssemblyFileVersion(";
            string line2 = string.Format(@"[assembly: AssemblyFileVersion(""{0}"")]", version);

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
                System.IO.File.WriteAllText(file + "._versioner_backup_", oldContent);
                System.IO.File.WriteAllText(file , newContent);
                this.Log = this.Log + Environment.NewLine + $" ver : {version} - file : {file}";
            }
            catch (Exception)
            {
                throw new Exception("Could not change AssemblyInfo file.");
            }
        }

        public void Version_ProjectFile(string file, string version)
        {
            List<string> oldLines = System.IO.File.ReadAllLines(file).ToList();
            List<string> newLines = System.IO.File.ReadAllLines(file).ToList();

            string ptrn3 = @"<ApplicationVersion>";
            string line3 = string.Format(@"    <ApplicationVersion>{0}</ApplicationVersion>", version);

            for (int i = 0; i < newLines.Count(); i++)
            {
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
                System.IO.File.WriteAllText(file + "._versioner_backup_", oldContent);
                System.IO.File.WriteAllText(file, newContent);
                this.Log = this.Log + Environment.NewLine + $" ver : {version} - file : {file}";
            }
            catch (Exception)
            {
                throw new Exception("Could not change project file.");
            }
        }
    }
}
