using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Reference
{
    public interface IReferenceAdder
    {
        void AddReference(string[] args);
    }

    public class ReferenceAdder : IReferenceAdder
    {
        private readonly IFileSystem _fileSystem;
        private readonly IUnityContainer _container;
        private readonly IConsole _console;
        private readonly List<string> _projects = new List<string>();
        private readonly IList<string> _references = new List<string>();
        private readonly IList<string> _directories = new List<string>();

        public ReferenceAdder(IFileSystem fileSystem, IUnityContainer container, IConsole console)
        {
            _fileSystem = fileSystem;
            _container = container;
            _console = console;
        }

        public void AddReference(string[] args)
        {
            foreach (var argument in args)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    if (IsDirectory(argument))
                    {
                        AddDirectory(argument);
                    }
                    else if (IsProject(argument))
                    {
                        AddProject(argument);
                    }
                    else if (IsReference(argument))
                    {
                        AddReference(argument);
                    }
                }
            }
            AddProjectsInDirectories();
            AddReferenceInProjects(_references, _projects);
        }

        private void AddProjectsInDirectories()
        {
            foreach (var directory in _directories)
            {
                _projects.AddRange(_fileSystem.GetFiles(directory, "*.csproj", SearchOption.AllDirectories));
            }
        }

        private void AddDirectory(string argument)
        {
            _directories.Add(argument);
        }

        private bool IsDirectory(string argument)
        {
            return _fileSystem.DirectoryExists(argument);
        }

        private void AddReferenceInProjects(IList<string> referencesToAdd, IList<string> projects)
        {
            if (referencesToAdd.Count > 0 && projects.Count > 0)
            {
                foreach (var projectPath in _projects)
                {
                    var fileName = Path.GetFileName(projectPath);
                    var parameters = new ParameterOverrides();
                    parameters.Add("path", projectPath);
                    var project = _container.Resolve<IProject>("", parameters);
                    bool projectHasChanged = false;
                    foreach (var reference in referencesToAdd)
                    {
                        if (!ProjectContainsAlreadyTheReference(project, reference))
                        {
                            project.AddItem("Reference", reference, null);
                            projectHasChanged = true;
                        }
                        else
                        {
                            _console.WriteLine(reference + " is already in " + fileName);
                        }
                    }
                    if (projectHasChanged)
                    {
                        project.Save();
                        _console.WriteLine(fileName + " has new references added.");
                    }
                }
            }
        }

        private bool ProjectContainsAlreadyTheReference(IProject project, string reference)
        {
            var references = project.GetItems("Reference");
            return references.Any(x => x.EvaluatedInclude == reference);
        }

        private void AddProject(string argument)
        {
            _projects.Add(argument);
        }

        private bool IsReference(string argument)
        {
            return true;
        }

        private bool IsProject(string argument)
        {
            if (_fileSystem.FileExists(argument))
            {
                var fileName = Path.GetFileName(argument);
                var extension = Path.GetExtension(argument);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extension))
                {
                    return string.Compare(extension, ".csproj", StringComparison.InvariantCultureIgnoreCase) == 0;
                }
            }
            return false;
        }

        private void AddReference(string argument)
        {
            _references.Add(argument);
        }
    }
}