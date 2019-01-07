using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.Core.Models
{
    public class GitProject
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string RepoLocalPath { get; set; }

        public string RepoBranch { get; set; }

        public bool IsLocalOnly { get; set; }

        public string RepoRemotePath { get; set; }

        public string RepoAccount { get; set; }

        public string RepoPassword { get; set; }

        public DateTime LastOpened { get; set; }

        public double TimesOpened { get; set; }
    }
}
