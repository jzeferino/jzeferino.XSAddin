using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Projects;
using MonoDevelop.Core;
using System.IO;
using System;
using Gtk;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using jzeferino.XSAddin;

namespace jzeferino.XSAddin.Wizard
{
    public class SolutionTemplateWizard : TemplateWizard
    {
        // Solution folders
        private const string Src = "src";
        private const string Android = "android";
        private const string Ios = "ios";
        private const string Shared = "shared";
        private const string Tests = "tests";
        private const string CopySolutionLevelItemsProject = "CopySolutionLevelItemsProject";

        public override string Id => "solution.wizard";

        public override WizardPage GetPage(int pageNumber) => new SolutionTemplateWizardPage(this);

        public override int TotalPages => 1;

        public override void ConfigureWizard()
        {
            base.ConfigureWizard();

            Parameters[Src] = Src;
            Parameters[Android] = Android;
            Parameters[Ios] = Ios;
            Parameters[Shared] = Shared;
            Parameters[Tests] = Tests;
            Parameters[CopySolutionLevelItemsProject] = CopySolutionLevelItemsProject;
        }

        public override async void ItemsCreated(IEnumerable<IWorkspaceFileObject> items)
        {
            string projectName = Parameters["UserDefinedProjectName"];

            var solution = items.OfType<Solution>().First();

            if (solution == null)
            {
                throw new NullReferenceException("Solution can't be null");
            }

            await RemoveCopySolutionLevelItemsDummyProject(solution);
            RemoveDefaultGeneratedProject(solution, projectName);
            await CreateAndArrangeSolutinFolders(solution);
        }

        private async Task RemoveCopySolutionLevelItemsDummyProject(Solution solution)
        {
            var copySolutionLevelItemsProject = solution.FindProjectByName(CopySolutionLevelItemsProject);
            if (copySolutionLevelItemsProject != null)
            {
                solution.RootFolder.Items.Remove(copySolutionLevelItemsProject);
                await solution.SaveSolutionAsync();

                Directory.Delete(copySolutionLevelItemsProject.BaseDirectory, true);
            }
        }

        private void RemoveDefaultGeneratedProject(Solution solution, string projectName)
        {
            var path = Path.Combine(solution.BaseDirectory, projectName);
            if (Directory.Exists(path))
            {
                Directory.Delete(Path.Combine(solution.BaseDirectory, projectName), true);
            }
        }

        private async Task CreateAndArrangeSolutinFolders(Solution solution)
        {
            if (string.IsNullOrEmpty(Parameters[SolutionTemplateWizardPage.HasFolderStructureParameter]))
            {
                return;
            }

            var srcFolder = new SolutionFolder();
            srcFolder.Name = Src;
            solution.RootFolder.Items.Add(srcFolder);

            AddInsideSrcSolutionFolder(solution, Shared, srcFolder);
            AddInsideSrcSolutionFolder(solution, Android, srcFolder);
            AddInsideSrcSolutionFolder(solution, Ios, srcFolder);
            AddInsideSrcSolutionFolder(solution, Tests, srcFolder);

            await solution.SaveSolutionAsync();
        }

        private void AddInsideSrcSolutionFolder(Solution solution, string partialPath, SolutionFolder outterSolutionFolder = null)
        {
            var projects = solution.Items.Where(p => p.BaseDirectory.ToString().Contains(partialPath));
            if (!projects.IsNullOrEmpty())
            {
                var solutionFolder = new SolutionFolder();
                solutionFolder.Name = partialPath;
                solutionFolder.Items.AddRange(projects);

                (outterSolutionFolder ?? solution.RootFolder).Items.Add(solutionFolder);
            }
        }
    }
}
