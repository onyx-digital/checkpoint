using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.Core.Models
{
    /// <summary>
    /// The GitClient Class is a wrapper class for the LibGit2Sharp library. It provides a subset of 
    /// functions relevant to the CheckPoint App.
    /// </summary>
    public class GitClient
    {
        /// <summary>
        /// Clone a remote Git repository into a local directory.
        /// </summary>
        /// <param name="remoteRepoPath">Remote Git repo https path.</param>
        /// <param name="localRepoPath">New local git repo directory path.</param>
        /// <param name="userName">Remote Git repo user name.</param>
        /// <param name="password">Remote Git repo user password.</param>
        /// <returns>Local Git repo path.</returns>
        public string Clone(string remoteRepoPath, string localRepoPath, string userName, 
            string password)
        {
            string cloneRetVal = string.Empty;

            try
            {
                var cloneOptions = new CloneOptions();
                cloneOptions.CredentialsProvider = (_url, _user, _cred) => 
                    new UsernamePasswordCredentials { Username = userName, Password = password };

                cloneRetVal = Repository.Clone(remoteRepoPath, localRepoPath, cloneOptions);
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Clone exception.", e);
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
                using (var repo = new Repository(localRepoPath))
                {
                    Commands.Stage(repo, "*");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Add-All Exception.", e);
            }
        }

        /// <summary>
        /// Create a new repository from a local rooted path.
        /// </summary>
        /// <param name="localRepoPath">Local git repo directory path.</param>
        /// <returns></returns>
        public string Init(string localRepoPath)
        {
            string rootedPath = string.Empty;

            try
            {
                rootedPath = Repository.Init(localRepoPath);
            }
            catch(Exception e)
            {
                throw new Exception("Git Client Init Exception.", e);
            }

            return rootedPath;
        }

        /// <summary>
        /// Commit all staged files to the repository.
        /// </summary>
        /// <param name="localRepoPath">Local git repo directory path.</param>
        /// <param name="authorName">Commit author name.</param>
        /// <param name="authorEmail">Commit author email address.</param>
        /// <param name="message">Commit description.</param>
        public CommitLog Commit(string localRepoPath, string authorName, string authorEmail, 
            string message)
        {
            CommitLog retVal = null;

            try
            {
                using (var repo = new Repository(localRepoPath))
                {
                    // Create the committer's signature and commit
                    Signature author = new Signature(authorName, authorEmail, DateTime.Now);
                    Signature committer = author;

                    // Commit to the repository
                    Commit commitRetVal = repo.Commit(message, author, committer);
                    retVal = CreateCommitLog(commitRetVal);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Commit Exception.", e);
            }
            
            return retVal;
        }

        /// <summary>
        /// Push local commits to a remote Git repository.
        /// </summary>
        /// <param name="localRepoPath">Local git repo directory path.</param>
        /// <param name="userName">Remote Git repo user name.</param>
        /// <param name="password">Remote Git repo user password.</param>
        /// <param name="branchName">Branch to push.</param>
        public string Push(string localRepoPath, string userName, string password, string branchName)
        {
            string retVal = string.Empty;

            try
            {
                using (var repo = new Repository(localRepoPath))
                {
                    LibGit2Sharp.PushOptions options = new LibGit2Sharp.PushOptions();
                    options.CredentialsProvider = new CredentialsHandler(
                        (url, usernameFromUrl, types) =>
                            new UsernamePasswordCredentials()
                            {
                                Username = userName,
                                Password = password
                            });
                    repo.Network.Push(repo.Branches[branchName], options);
                    retVal = repo.Network.Remotes.First().Url;
                }
            }
            catch(Exception e)
            {
                throw new Exception("Git Client Push Exception.", e);
            }

            return retVal;
        }

        /// <summary>
        /// Return a limted log of commits.
        /// </summary>
        /// <param name="localRepoPath">Local git repo directory path.</param>
        /// <returns></returns>
        public List<CommitLog> Log(string localRepoPath)
        {
            List<CommitLog> commitLogs = new List<CommitLog>();

            try
            {
                using (var repo = new Repository(localRepoPath))
                {
                    foreach (Commit commit in repo.Commits)
                    {
                        CommitLog log = CreateCommitLog(commit);

                        commitLogs.Add(log);
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Git Client Log Exception.", e);
            }

            return commitLogs;
        }


        public bool Revert(string localRepoPath, string commitId, string author, string authorEmail)
        {
            bool revertOk = true;

            try
            {
                using (var repo = new Repository(localRepoPath))
                {
                    // Get the restore point commit.
                    Commit restoreCommit = repo.Commits.First(c => c.Id.ToString() == commitId);

                    // Shortlist all commits after the restore point commit.
                    List<Commit> revertList =
                        repo.Commits.Where(c => c.Author.When > restoreCommit.Author.When).ToList();

                    // Order shortlist by decending date time.
                    List<Commit> orderedRevertList =
                        revertList.OrderByDescending(c => c.Author.When).ToList();

                    // Revert all shortlisted and ordered commits.
                    Signature signiture = new Signature(author, authorEmail, DateTime.Now);
                    RevertOptions revertOptions = new RevertOptions();
                    revertOptions.CommitOnSuccess = false;

                    foreach (Commit commit in orderedRevertList)
                    {
                        RevertResult result = repo.Revert(commit, signiture, revertOptions);
                        revertOk = (result.Status == RevertStatus.Reverted) ? revertOk : false;

                        if(revertOk)
                        {
                            string message = string.Format("Revert Commit: {0}", commit.MessageShort);
                            CommitOptions commitOptions = new CommitOptions();
                            repo.Commit(message, signiture, signiture, commitOptions);
                        }

                    }
                }                  
            }
            catch (Exception e)
            {
                throw new Exception("Git Client Revert Exception.", e);
            }

            return revertOk;
        }

        private static CommitLog CreateCommitLog(Commit commit)
        {
            return new CommitLog
            {
                CommitId = commit.Id.ToString(),
                Merge = commit.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray().
                    ToString(),
                Author = commit.Author.Name,
                Email = commit.Author.Email,
                Date = commit.Author.When.DateTime,
                Message = commit.Message
            };
        }
               
        /// <summary>
        /// Model of a commit log summary.
        /// </summary>
        public class CommitLog
        {
            public string CommitId { get; set; }

            public string Merge { get; set; }

            public string Author { get; set; }

            public string Email { get; set; }

            public DateTime Date { get; set; }

            public string Message { get; set; }
        }

    }
}
