using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace versioner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usages :");
                Console.WriteLine(@"    versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.WriteLine(@"    versioner.exe -m 1 -n 1 -y 2010 -p ""c:\project1\project1.vbproj""");
                Console.WriteLine(@"    versioner.exe -m 5 -n 0 -y 2012 -p ""c:\project1\project1.vdproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            Options options = new Options();

            try
            {
                options.Parse(args.ToList());
            }
            catch (Exception)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (options.Major < 0)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Please, enter major version parameter using option -m ...");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (options.Minor < 0)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Please, enter minor version parameter using option -n ...");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (options.BirthYear < 0)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Please, enter birth year parameter using option -y ...");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (options.BirthYear > DateTime.Now.Year)
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Birth year parameter value can not be newer than current year.");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (string.IsNullOrEmpty(options.ProjectFile))
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Please, enter project file parameter using option -p ...");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (string.IsNullOrWhiteSpace(options.ProjectFile))
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Please, enter project file parameter using option -p ...");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            if (!System.IO.File.Exists(options.ProjectFile))
            {
                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine("");
                Console.WriteLine(@"  Project file does not exist.");
                Console.WriteLine("");
                Console.WriteLine(@"  Example Usage : versioner.exe -m 3 -n 2 -y 2015 -p ""c:\project1\project1.csproj""");
                Console.ForegroundColor = Tools.RegularConsoleColor;
                return;
            }

            // csproj
            if (options.ProjectFile.EndsWith("csproj"))
            {
                VersionerCS versionerCS = new VersionerCS();

                versionerCS.Version(options.ProjectFile, options.Version4Str());

                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine(versionerCS.Log);
                Console.ForegroundColor = Tools.RegularConsoleColor;

                return;
            }

            // vbproj
            if (options.ProjectFile.EndsWith("vbproj"))
            {
                VersionerVB versionerVB = new VersionerVB();

                versionerVB.Version(options.ProjectFile, options.Version4Str());

                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine(versionerVB.Log);
                Console.ForegroundColor = Tools.RegularConsoleColor;

                return;
            }

            // vdproj
            if (options.ProjectFile.EndsWith("vdproj"))
            {
                VersionerVD versionerVD = new VersionerVD();

                versionerVD.Version(options.ProjectFile, options.Version3Str());

                Console.ForegroundColor = Tools.IrregularConsoleColor;
                Console.WriteLine(versionerVD.Log);
                Console.ForegroundColor = Tools.RegularConsoleColor;

                return;
            }

            Console.ForegroundColor = Tools.IrregularConsoleColor;
            Console.WriteLine("");
            Console.WriteLine(@"  Project file type is not supported.");
            Console.ForegroundColor = Tools.RegularConsoleColor;
        }
    }
}
