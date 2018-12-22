using CheckPoint.Core.Models;
using CheckPoint.UI.Dialogs;
using CheckPoint.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;      
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CheckPoint.Core.Models.GitClient;
                                                                                                                      
namespace CheckPoint.UI.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        private GitProject _Project;

        public ProjectPage(GitProject project)
        {
            InitializeComponent();

            _Project = project;

            SetHeader();
            UpdateLog();
        }

        private void SetHeader()
        {
            string header = string.Format("Project :: {0}", _Project.ProjectName);
            txbHeader.Text = header;
        }

        private void UpdateLog()
        {
            try
            {
                List<CommitLog> logs = App.GitControl.Log(_Project.RepoLocalPath);
                dgCommits.ItemsSource = logs;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void Commit()
        {
            try
            {
                string author = string.Empty;
                string email = string.Empty;
                string message = string.Empty;

                CommitDialog inputDialog = new CommitDialog();
                if (inputDialog.ShowDialog() == true)
                {
                    author = inputDialog.Author;
                    email = inputDialog.Email;
                    message = inputDialog.Message;

                    App.GitControl.Backup(_Project, author, email, message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CheckPoint Commit failed");
            }
        }

        private void Revert(CommitLog commit)
        {
            try
            {
                string author = string.Empty;
                string email = string.Empty;
                string message = string.Empty;

                RevertDialog inputDialog = new RevertDialog();
                inputDialog.RevertMessage = commit.Message.Trim();

                if (inputDialog.ShowDialog() == true)
                {
                    author = inputDialog.Author;
                    email = inputDialog.Email;

                    App.GitControl.Revert(_Project.RepoLocalPath, commit.CommitId, author, email);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Revert failed");
            }
        }

        private void Restore(CommitLog commit)
        {
            bool revertOk = false;

            try
            {
                revertOk = App.GitControl.Revert(Constants.DEFAULT_LOCAL_REPO_PATH, commit.CommitId,
                    Constants.DEFAULT_NAME, Constants.DEFAULT_EMAIL);

                if (revertOk)
                {
                    App.GitControl.Push(Constants.DEFAULT_LOCAL_REPO_PATH, Constants.DEFAULT_USERNAME,
                        Constants.DEFAULT_PASSWORD, Constants.DEFAULT_BRANCH);

                    lblReverStatus.Content = string.Format("Successfully restored commit: {0}",
                        commit.CommitId);
                }
                else
                {
                    lblReverStatus.Content = string.Format("Restore failed.");
                }
            }
            catch(Exception e)
            {
                lblReverStatus.Content = string.Format("Restore failed.");
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            CommitLog selectedCommmit = (CommitLog)dgCommits.SelectedItem;

            Restore(selectedCommmit);

            UpdateLog();
        }

        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            GitProject test = (GitProject)e.ExtraData;
        }

        private void BtnCommit_Click(object sender, RoutedEventArgs e)
        {
            Commit();
            UpdateLog();
        }

        private void BtnRevert_Click(object sender, RoutedEventArgs e)
        {
            Revert((CommitLog)dgCommits.SelectedItem);
            UpdateLog();
        }
    }
}
