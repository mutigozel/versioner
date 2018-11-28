using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace versioner
{
    class VersionerVB
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
            string file = System.IO.Path.Combine(path, "My Project", "AssemblyInfo.vb");

            if (!System.IO.File.Exists(file))
                return;

            List<string> oldLines = System.IO.File.ReadAllLines(file).ToList();
            List<string> newLines = System.IO.File.ReadAllLines(file).ToList();

            // change lines
            string ptrn1 = @"<Assembly: AssemblyVersion(";
            string line1 = string.Format(@"<Assembly: AssemblyVersion(""{0}"")> ", version);

            string ptrn2 = @"<Assembly: AssemblyFileVersion(";
            string line2 = string.Format(@"<Assembly: AssemblyFileVersion(""{0}"")> ", version);

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

            string ptrn1 = @"<Version>";
            string line1 = string.Format(@"    <Version>{0}</Version>", version);

            string ptrn2 = @"<FileVersion>";
            string line2 = string.Format(@"    <FileVersion>{0}</FileVersion>", version);

            string ptrn3 = @"<AssemblyVersion>";
            string line3 = string.Format(@"    <AssemblyVersion>{0}</AssemblyVersion>", version);

            string ptrn4 = @"<ApplicationVersion>";
            string line4 = string.Format(@"    <ApplicationVersion>{0}</ApplicationVersion>", version);

            for (int i = 1; i < newLines.Count(); i++)
            {
                if (newLines[i].Trim().StartsWith(ptrn1))
                {
                    // nuget packagereference
                    if (newLines[i - 1].Contains("PackageReference"))
                        continue;
                    if (newLines[i].Contains("PackageReference"))
                        continue;

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

                if (newLines[i].Trim().StartsWith(ptrn4))
                {
                    newLines[i] = line4;
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




        //private void ApplySetupProject(string projectFolder, string majorVersion, string minorVersion, string buildVersion)
        //{
        //    string fileName = string.Empty;
        //    System.IO.FileStream fileStream = null;
        //    System.IO.StreamWriter fileWriter = null;
        //    BackupSetupProject(projectFolder);
        //    if (!ChangeSetupProject(projectFolder, majorVersion, minorVersion, buildVersion))
        //    {
        //        RollbackSetupProject(projectFolder);
        //        foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(projectFolder).GetFiles("*.currentautoversion"))
        //        {
        //            System.System.IO.File.Delete(fi.FullName);
        //        }

        //        return;
        //    }

        //    foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(projectFolder).GetFiles("*.currentautoversion"))
        //    {
        //        System.System.IO.File.Delete(fi.FullName);
        //    }

        //    fileName = System.IO.Path.Combine(projectFolder, string.Format("{0}_{1}_{2}.currentautoversion", majorVersion, minorVersion, buildVersion));
        //    fileStream = new System.IO.FileStream(fileName, IO.FileMode.CreateNew);
        //    fileWriter = new System.IO.StreamWriter(fileStream);
        //    fileWriter.Write(string.Format("{0}_{1}_{2}.currentautoversion", majorVersion, minorVersion, buildVersion));
        //    fileWriter.Flush();
        //    fileWriter.Close();
        //    fileStream.Close();
        //}

        //private void BackupSetupProject(string projectFolder)
        //{
        //    string versionFile = string.Empty;
        //    string backupFile = string.Empty;
        //    versionFile = new System.IO.DirectoryInfo(projectFolder).GetFiles("*.vdproj")(0).FullName;
        //    backupFile = versionFile + ".autoversionbackup";
        //    if (System.System.IO.File.Exists(backupFile))
        //    {
        //        System.System.IO.File.Delete(backupFile);
        //    }

        //    System.System.IO.File.Copy(versionFile, backupFile);
        //}

        //private bool ChangeSetupProject(string projectFolder, string majorVersion, string minorVersion, string buildVersion)
        //{
        //    int i = 0;
        //    string fileName = string.Empty;
        //    System.IO.FileStream fileStream = null;
        //    System.IO.StreamReader fileReader = null;
        //    System.IO.StreamWriter fileWriter = null;
        //    List<string> fileLines = new List<string>;
        //    string productVersionLine = string.Empty;
        //    string productCodeLine = string.Empty;
        //    string packageCodeLine = string.Empty;
        //    try
        //    {
        //        fileName = new System.IO.DirectoryInfo(projectFolder).GetFiles("*.vdproj")(0).FullName;
        //        productVersionLine = string.Format("        \"ProductVersion\" = \"8:{0}.{1}.{2}\"", majorVersion, minorVersion, buildVersion);
        //        productCodeLine = "        \"ProductCode\" = \"8:{" + System.Guid.NewGuid.ToString.ToUpperInvariant + "}\"";
        //        packageCodeLine = "        \"PackageCode\" = \"8:{" + System.Guid.NewGuid.ToString.ToUpperInvariant + "}\"";
        //        fileStream = new System.IO.FileStream(fileName, IO.FileMode.Open);
        //        fileReader = new System.IO.StreamReader(fileStream);
        //        while (!fileReader.EndOfStream)
        //        {
        //            fileLines.Add(fileReader.ReadLine);
        //        }

        //        fileReader.Close();
        //        fileStream.Close();
        //        for (i = 0; i <= fileLines.Count - 1; i++)
        //        {
        //            if (fileLines(i).StartsWith("        \"ProductVersion\" = \"8:"))
        //            {
        //                fileLines(i) = productVersionLine;
        //            }
        //            else if (fileLines(i).StartsWith("        \"ProductCode\" = \"8:{"))
        //            {
        //                fileLines(i) = productCodeLine;
        //            }
        //            else if (fileLines(i).StartsWith("        \"PackageCode\" = \"8:{"))
        //            {
        //                fileLines(i) = packageCodeLine;
        //            }
        //        }

        //        fileStream = new System.IO.FileStream(fileName, IO.FileMode.Truncate);
        //        fileWriter = new System.IO.StreamWriter(fileStream);
        //        for (i = 0; i <= fileLines.Count - 1; i++)
        //        {
        //            fileWriter.WriteLine(fileLines(i));
        //        }

        //        fileWriter.Flush();
        //        fileWriter.Close();
        //        fileStream.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileName = System.IO.Path.Combine(projectFolder, "autoversionerror.txt");
        //        if (System.System.IO.File.Exists(fileName))
        //        {
        //            System.System.IO.File.Delete(fileName);
        //        }

        //        fileStream = new System.IO.FileStream(fileName, IO.FileMode.OpenOrCreate);
        //        fileWriter = new System.IO.StreamWriter(fileStream);
        //        fileWriter.Write(ex.Message);
        //        fileWriter.Flush();
        //        fileWriter.Close();
        //        fileStream.Close();
        //        return false;
        //    }
        //}

        //private void RollbackSetupProject(string projectFolder)
        //{
        //    string versionFile = string.Empty;
        //    string backupFile = string.Empty;
        //    versionFile = new System.IO.DirectoryInfo(projectFolder).GetFiles("*.vdproj")(0).FullName;
        //    backupFile = versionFile + ".autoversionbackup";
        //    if (System.System.IO.File.Exists(versionFile))
        //    {
        //        System.System.IO.File.Delete(versionFile);
        //    }

        //    System.System.IO.File.Copy(backupFile, versionFile);
        //}

    }
}
