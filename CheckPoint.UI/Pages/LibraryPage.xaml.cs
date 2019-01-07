using CheckPoint.Core.Models;
using CheckPoint.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CheckPoint.UI.Pages
{
    /// <summary>
    /// Interaction functions for the Library Page.
    /// </summary>
    public partial class LibraryPage : Page
    {

        #region Constructors

        public LibraryPage()
        {
            InitializeComponent();
            UpdateProjectIndex();
        }

        #endregion
        
        #region Private Functions
        
        /// <summary>
        /// This function uses a dialog to prompt the user for details of the remote Git repo that
        /// they would like to clone to a local directory. Once cloned the new project is added to 
        /// the CheckPoint Library.
        /// </summary>
        private void CloneProject()
        {
            try
            {
                string projectName = string.Empty;
                string repoPath = string.Empty;
                string branch = string.Empty;
                string remoteUrl = string.Empty;
                string account = string.Empty;
                string password = string.Empty;

                CloneProjectDialog inputDialog = new CloneProjectDialog();
                if (inputDialog.ShowDialog() == true)
                {
                    GitProject newProject = new GitProject
                    {
                        ProjectName = inputDialog.Project,
                        RepoLocalPath = inputDialog.LocalPath,
                        RepoBranch = inputDialog.RepoBranch,
                        RepoRemotePath = inputDialog.RemotePath,
                        RepoAccount = inputDialog.RepoAccount,
                        RepoPassword = inputDialog.RepoPassword,
                        IsLocalOnly = true
                    };

                    App.GitControl.Clone(newProject);
                    CreateProject(newProject);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        /// <summary>
        /// This function uses a dialog to prompt the user for details of a local directory that 
        /// they would like to initialise as a Git repo. Once initialised the new project is added 
        /// to the CheckPoint Library.
        /// </summary>
        private void NewProject()
        {
            try
            {
                string projectName = string.Empty;
                string repoPath = string.Empty;
                string branch = string.Empty;

                NewProjectDialog inputDialog = new NewProjectDialog();
                if (inputDialog.ShowDialog() == true)
                {
                    GitProject newProject = new GitProject
                    {
                        ProjectName = inputDialog.ProjectName,
                        RepoLocalPath = inputDialog.LocalPath,
                        RepoBranch = inputDialog.RepoBranch,
                        IsLocalOnly = true
                    };


                    App.GitControl.Init(newProject.RepoLocalPath);
                    CreateProject(newProject);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        /// <summary>
        /// This function updates the project stats before opening the project.
        /// </summary>
        private void OpenProject()
        {
            GitProject selectedProject = (GitProject)dgProjects.SelectedItem;

            selectedProject.LastOpened = DateTime.Now;
            selectedProject.TimesOpened += 1;
            App.ProjectControl.UpdateProject(selectedProject);

            this.NavigationService.Navigate(new ProjectPage(selectedProject));
        }

        /// <summary>
        /// This function adds a new git project to the CheckPoint Project Library.
        /// </summary>
        /// <param name="newProject">Git project to be added to the library.</param>
        private void CreateProject(GitProject newProject)
        {
            try
            {
                App.ProjectControl.CreateProject(newProject);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        /// <summary>
        /// This function retrieves the latest Project Library details and updates the Project 
        /// Index.
        /// </summary>
        private void UpdateProjectIndex()
        {
            try
            {
                List<GitProject> projects = App.ProjectControl.ReadProjects();
                dgProjects.ItemsSource = projects;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
        
        #endregion


        #region Event Handlers

        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            CloneProject();
            UpdateProjectIndex();
        }
        
        private void BtnNewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProject();
            UpdateProjectIndex();
        }

        private void BtnProjectOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenProject();
        }

        private void DgProjects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenProject();
        }
        
        #endregion               
    }
}
