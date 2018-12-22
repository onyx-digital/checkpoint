using CheckPoint.Core.Models;
using System;
using System.Collections.Generic;
using static CheckPoint.Core.Models.GitClient;

namespace CheckPoint.UI.Controllers
{
    public class GitController
    {
        private GitClient _GitClient;

        public GitController()
        {
            _GitClient = new GitClient();
        }

        public void Backup(GitProject project, string authorName, string authorEmail, 
            string message)
        {
            try
            {
                Add(project.RepoLocalPath);
                Commit(project.RepoLocalPath, authorName, authorEmail, message);
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while backing up. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }
        }

        /// <summary>
        /// Clone a remote Git repository into a local directory.
        /// </summary>
        /// <param name="remoteRepoPath">Remote Git repo https path.</param>
        /// <param name="localRepoPath">New local git repo directory path.</param>
        /// <param name="userName">Remote Git repo user name.</param>
        /// <param name="password">Remote Git repo user password.</param>
        /// <returns>Local Git repo path.</returns>
        public string Clone(GitProject project)
        {
            string cloneRetVal = string.Empty;

            try
            {
                cloneRetVal = _GitClient.Clone(project.RepoRemotePath, project.RepoLocalPath,
                    project.RepoAccount, project.RepoPassword);

                string logMsg = string.Format("Remote Git Repo cloned to path: {0}",
                    cloneRetVal);
                App.AppLogger.Info(logMsg);
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while cloning. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }

            return cloneRetVal;
        }

        /// <summary>
        /// Stage all files in a repository that is currently checked out.
        /// </summary>
        /// <param name="localRepoPath">Local git repo directory path.</param>
        public void Add(string localRepoPath)
        {
            try
            {
                _GitClient.Add(localRepoPath);

                string logMsg = string.Format("All new and modified files staged for path: {0}",
                    localRepoPath);
                App.AppLogger.Info(logMsg);
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while adding files. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }
        }


        public CommitLog Commit(string localRepoPath, string authorName, string authorEmail,
            string message)
        {
            CommitLog retVal = null;

            try
            {
                retVal = _GitClient.Commit(localRepoPath, authorName, authorEmail, message);

                string logMsg = string.Format("Commit Successful, ID:{0}, Author:{1}, Message:{2}", 
                    retVal.CommitId, retVal.Author, retVal.Message);
                App.AppLogger.Info(logMsg);
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred during commit. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }

            return retVal;
        }

        public string Push(string localRepoPath, string userName, string password, string branchName)
        {
            string retVal = string.Empty;

            try
            {
                retVal = _GitClient.Push(localRepoPath, userName, password, branchName);

                string logMsg = string.Format("Successful push to: {0}", retVal);
                App.AppLogger.Info(logMsg);
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred during push. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }

            return retVal;
        }

        public List<CommitLog> Log(string localRepoPath)
        {
            List<CommitLog> commitLogs = new List<CommitLog>();

            try
            {
                commitLogs = _GitClient.Log(localRepoPath);
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Log Exception.", e);
            }

            return commitLogs;
        }

        public bool Revert(string localRepoPath, string commitId, string author, string authorEmail)
        {
            bool revertOk = false; 

            try
            {
                revertOk = _GitClient.Revert(localRepoPath, commitId, author, authorEmail);
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Log Exception.", e);
            }

            return revertOk;
        }

        public string Init(string localRepoPath)
        {
            string initPath = string.Empty;

            try
            {
                initPath = _GitClient.Init(localRepoPath);
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Init Exception.", e);
            }

            return initPath;
        }
    }

}
