using System;
namespace jzeferino.XSAddin.Wizard
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class GtkSolutionTemplateWizardWidget : Gtk.Bin, ISolutionTemplateWizardView, IDisposable
    {
        public GtkSolutionTemplateWizardWidget()
        {
            Build();
        }

        private SolutionTemplateWizardPage _wizardPage;
        public SolutionTemplateWizardPage WizardPage
        {
            get { return _wizardPage; }
            set
            {
                _wizardPage = value;
                SetupViewCallbacks();
                SetupOriginalState();
            }
        }

        private void SetupViewCallbacks()
        {
            checkboxGit.Toggled += GitCheckChanged;
            checkboxReadme.Toggled += ReadmeCheckChanged;
            checkboxCake.Toggled += CakeCheckChanged;
            checkboxSharedCode.Toggled += SharedCodeCheckChanged;
            checkboxAndroidProject.Toggled += AndroidProjectCheckChanged;
            checkboxiOSProject.Toggled += iOSProjectCheckChanged;
            checkboxUnitTestsProject.Toggled += UnitTestsProjectChanged;
            entryApplicationIdentifier.Changed += EntryApplicationIdentifierChanged;
            entryApplicationName.Changed += EntryApplicationNameChanged;
        }

        private void GitCheckChanged(object sender, EventArgs e) => WizardPage.HasGitignore = checkboxGit.Active;
        private void ReadmeCheckChanged(object sender, EventArgs e) => WizardPage.HasReadme = checkboxReadme.Active;
        private void CakeCheckChanged(object sender, EventArgs e) => WizardPage.HasCake = checkboxCake.Active;
        private void SharedCodeCheckChanged(object sender, EventArgs e) => WizardPage.HasSharedCode = checkboxSharedCode.Active;
        private void AndroidProjectCheckChanged(object sender, EventArgs e) => WizardPage.HasAndroidProject = checkboxAndroidProject.Active;
        private void iOSProjectCheckChanged(object sender, EventArgs e) => WizardPage.HasiOSProject = checkboxiOSProject.Active;
        private void UnitTestsProjectChanged(object sender, EventArgs e) => WizardPage.HasUnitTestsProject = checkboxUnitTestsProject.Active;
        private void EntryApplicationIdentifierChanged(object sender, EventArgs e) => WizardPage.ApplicationIdentifier = entryApplicationIdentifier.Text;
        private void EntryApplicationNameChanged(object sender, EventArgs e) => WizardPage.ApplicationName = entryApplicationName.Text;

        private void SetupOriginalState()
        {
            checkboxiOSProject.Active = checkboxAndroidProject.Active = true;

            checkboxSharedCode.Active = true;
            checkboxSharedCode.Sensitive = false;

            radioForms.Sensitive = false;

            //Force the text to be updated in wizard.
            EntryApplicationIdentifierChanged(null, null);
            EntryApplicationNameChanged(null, null);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (checkboxGit != null)
            {
                checkboxGit.Toggled += GitCheckChanged;
            }
            if (checkboxReadme != null)
            {
                checkboxReadme.Toggled += ReadmeCheckChanged;
            }
            if (checkboxCake != null)
            {
                checkboxCake.Toggled += CakeCheckChanged;
            }
            if (checkboxSharedCode != null)
            {
                checkboxSharedCode.Toggled += SharedCodeCheckChanged;
            }
            if (checkboxAndroidProject != null)
            {
                checkboxAndroidProject.Toggled += AndroidProjectCheckChanged;
            }
            if (checkboxiOSProject != null)
            {
                checkboxiOSProject.Toggled += iOSProjectCheckChanged;
            }
            if (checkboxUnitTestsProject != null)
            {
                checkboxUnitTestsProject.Toggled += UnitTestsProjectChanged;
            }
        }
    }
}
