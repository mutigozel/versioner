using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace versioner
{
    public class Options
    {
        public int Major = -1;

        public int Minor = -1;

        public int BirthYear = -1;

        public string ProjectFile = string.Empty;

        public int Revision = DateTime.Now.Hour;

        public string MajorStr()
        {
            if (this.Major < 0)
                return string.Empty;
            return this.Major.ToString("0");
        }

        public string MinorStr()
        {
            if (this.Minor < 0)
                return string.Empty;
            return this.Minor.ToString("0");
        }

        public string BuildStr()
        {
            if (this.BirthYear < 0)
                return string.Empty;
            DateTime birth;
            try
            {
                birth = new DateTime(this.BirthYear, 1, 1);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            TimeSpan span = DateTime.Now - birth;
            int days = Convert.ToInt32(span.TotalDays);
            days = Math.Max(1, days);
            return days.ToString("0");
        }

        public string RevisionStr()
        {
            if (this.Revision < 0)
                return string.Empty;
            return this.Revision.ToString("0");
        }

        public string Version4Str()
        {
            return string.Format("{0}.{1}.{2}.{3}", this.MajorStr(), this.MinorStr(), this.BuildStr(), this.RevisionStr());
        }

        public string Version3Str()
        {
            return string.Format("{0}.{1}.{2}", this.MajorStr(), this.MinorStr(), this.BuildStr());
        }

        public void Parse(List<string> args)
        {
            this.Major = -1;
            this.Minor = -1;
            this.BirthYear = -1;
            this.ProjectFile = string.Empty;

            args.Add("");

            try
            {
                for (int i = 0; i < args.Count() - 1; i++)
                {
                    if (args[i] == "-m")
                    {
                        this.Major = Int32.Parse(args[i + 1]);
                    }
                    if (args[i] == "-n")
                    {
                        this.Minor = Int32.Parse(args[i + 1]);
                    }
                    if (args[i] == "-y")
                    {
                        this.BirthYear = Int32.Parse(args[i + 1]);
                    }
                    if (args[i] == "-p")
                    {
                        this.ProjectFile =  args[i + 1];
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not parse parameters.");
            }
        }

    }
}
