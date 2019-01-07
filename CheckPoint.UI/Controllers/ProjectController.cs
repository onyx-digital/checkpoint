using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CheckPoint.Core.Models;
using CheckPoint.UI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPoint.UI.Controllers
{
    public class ProjectController
    {
        const string PROJECT_DATA_DIRECTORY = @"Data/";
        const string PROJECT_DATA_PATH = PROJECT_DATA_DIRECTORY + @"ProjectData.json";

        public ProjectController()
        {
            InitialiseProjectsFile();
        }

        public void InitialiseProjectsFile()
        {
            try
            {
                if (!File.Exists(PROJECT_DATA_PATH))
                {
                    if (!Directory.Exists(PROJECT_DATA_DIRECTORY))
                    {
                        System.IO.Directory.CreateDirectory(PROJECT_DATA_DIRECTORY);
                    }

                    // Set default values.
                    List<GitProject> projectDefaults = ProjectDataDefaults.GetDefaults();
                    string jsonData = JsonConvert.SerializeObject(projectDefaults,
                        Formatting.Indented);

                    using (StreamWriter file = new StreamWriter(PROJECT_DATA_PATH, true))
                    {
                        file.WriteLine(jsonData);
                    }
                }
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while initialising ProjectsData.json file. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }
        }

        public List<GitProject> ReadProjects()
        {
            List<GitProject> projectsList = new List<GitProject>();

            try
            {
                using (StreamReader file = new StreamReader(PROJECT_DATA_PATH))
                {
                    string jsonData = file.ReadToEnd();

                    projectsList = JsonConvert.DeserializeObject<List<GitProject>>(jsonData);
                }
            }
            catch(Exception e)
            {
                string exMessage = "An error occurred while reading ProjectsData.json file. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }

            return projectsList;
        }

        public void CreateProject(GitProject newProject)
        {
            try
            {
                bool fileExists = File.Exists(PROJECT_DATA_PATH);

                if (fileExists)
                {
                    // Add a new project to the current collection and rewrite the file.
                    List<GitProject> projects = ReadProjects();

                    newProject.ProjectId = Guid.NewGuid();
                    projects.Add(newProject);

                    string jsonData = JsonConvert.SerializeObject(projects, Formatting.Indented);

                    using (StreamWriter file = new StreamWriter(PROJECT_DATA_PATH, false))
                    {
                        file.WriteLine(jsonData);
                    }
                }
                else
                {
                    string exMessage = "ProjectData.json file does not exists.";
                    App.AppLogger.Error(exMessage);
                    throw new Exception(exMessage);
                }
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while appending to ProjectsData.json file. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }
        }
        
        public void UpdateProject(GitProject editedProject)
        {
            try
            {
                bool fileExists = File.Exists(PROJECT_DATA_PATH);

                if (fileExists)
                {
                    List<GitProject> projects = ReadProjects();
                    GitProject projectMatch = projects.First(p => p.ProjectId == editedProject.ProjectId);                    
                    projects.Remove(projectMatch);
                    projects.Add(editedProject);

                    string jsonData = JsonConvert.SerializeObject(projects, Formatting.Indented);

                    using (StreamWriter file = new StreamWriter(PROJECT_DATA_PATH, false))
                    {
                        file.WriteLine(jsonData);
                    }
                }
                else
                {
                    string exMessage = "ProjectData.json file does not exists.";
                    App.AppLogger.Error(exMessage);
                    throw new Exception(exMessage);
                }
            }
            catch (Exception e)
            {
                string exMessage = "An error occurred while updating the ProjectsData.json file. Check the Error Log for detailed exception info.";
                App.AppLogger.Error(e, exMessage);
                throw new Exception(exMessage);
            }
        }

    }
}
