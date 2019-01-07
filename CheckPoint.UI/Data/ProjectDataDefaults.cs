using CheckPoint.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.UI.Data
{
    public static class ProjectDataDefaults
    {
        public static List<GitProject> GetDefaults()
        {
            List<GitProject> projectList = new List<GitProject>();
            
            //projectList.Add(new GitProject
            //{
            //    ProjectName = "Development Project A",
            //    RepoLocalPath = @"C:\Temp\OnyxCheckPoint\DevProjectA",
            //    RepoRemotePath = @"https://chris-obeirne@bitbucket.org/onyxdigital/onyx-checkpoint-prototype-push.git",
            //    RepoBranch = "master"
            //});

            //projectList.Add(new GitProject
            //{
            //    ProjectName = "Development Project B",
            //    RepoLocalPath = @"C:\Temp\OnyxCheckPoint\DevProjectB",
            //    RepoRemotePath = @"https://chris-obeirne@bitbucket.org/onyxdigital/onyx-checkpoint-prototype-push.git",
            //    RepoBranch = "master"
            //});

            //projectList.Add(new GitProject
            //{
            //    ProjectName = "Development Project C",
            //    RepoLocalPath = @"C:\Temp\OnyxCheckPoint\DevProjectC",
            //    RepoRemotePath = @"https://chris-obeirne@bitbucket.org/onyxdigital/onyx-checkpoint-prototype-push.git",
            //    RepoBranch = "master"
            //});

            return projectList;
        }
    }
}
