using MonoDevelop.Ide.Templates;

namespace jzeferino.XSAddin.Wizard
{
    public class SolutionTemplateWizardPage : WizardPage
    {
        private SolutionTemplateWizard _wizard;
        private ISolutionTemplateWizardView _view;

        #region User customization

        public enum SolutionType
        {
            XamarinNative,
            XamarinForms
        }

        // Parameter constants.
        public const string HasSharedCodeParameter = "HasSharedCode";
        public const string HasReadmeParameter = "HasReadme";
        public const string HasGitignoreParameter = "HasGitignore";
        public const string HasCakeParameter = "HasCake";
        public const string HasAndroidProjectParameter = "HasAndroidProject";
        public const string HasiOSProjectParameter = "HasiOSProject";
        public const string HasUnitTestsProjectParameter = "HasUnitTestsProject";
        public const string HasFolderStructureParameter = "HasFolderStructure";
        public const string ApplicationIdentifierParameter = "ApplicationIdentifier";
        public const string ApplicationNameParameter = "ApplicationName";

        // Backing fields.
        private bool _hasReadme;
        private bool _hasGitignore;
        private bool _hasCake;
        private bool _hasSharedCode;
        private bool _hasAndroidProject;
        private bool _hasiOSProject;
        private bool _hasUnitTestsProject;
        private bool _hasFolderStructureParameter;
        private string _applicationIdentifier;
        private string _applicationName;

        public SolutionType XamarinSolutionType { get; set; }

        public bool HasSharedCode
        {
            get { return _hasSharedCode; }
            set { _wizard.Parameters[HasSharedCodeParameter] = (_hasSharedCode = value).ToString(); }
        }

        public bool HasReadme
        {
            get { return _hasReadme; }
            set { _wizard.Parameters[HasReadmeParameter] = (_hasReadme = value).ToString(); }
        }

        public bool HasGitignore
        {
            get { return _hasGitignore; }
            set { _wizard.Parameters[HasGitignoreParameter] = (_hasGitignore = value).ToString(); }
        }

        public bool HasCake
        {
            get { return _hasCake; }
            set { _wizard.Parameters[HasCakeParameter] = (_hasCake = value).ToString(); }
        }

        public bool HasAndroidProject
        {
            get { return _hasAndroidProject; }
            set { _wizard.Parameters[HasAndroidProjectParameter] = (_hasAndroidProject = value).ToString(); }
        }

        public bool HasiOSProject
        {
            get { return _hasiOSProject; }
            set { _wizard.Parameters[HasiOSProjectParameter] = (_hasiOSProject = value).ToString(); }
        }

        public bool HasUnitTestsProject
        {
            get { return _hasUnitTestsProject; }
            set { _wizard.Parameters[HasUnitTestsProjectParameter] = (_hasUnitTestsProject = value).ToString(); }
        }

        public bool FolderStructure
        {
            get { return _hasFolderStructureParameter; }
            set { _wizard.Parameters[HasFolderStructureParameter] = (_hasFolderStructureParameter = value).ToString(); }
        }

        public string ApplicationIdentifier
        {
            get { return _applicationIdentifier; }
            set
            {
                _wizard.Parameters[ApplicationIdentifierParameter] = (_applicationIdentifier = value);
                UpdateCanMoveNextPage();
            }
        }

        public string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _wizard.Parameters[ApplicationNameParameter] = (_applicationName = value);
                UpdateCanMoveNextPage();
            }
        }
        #endregion

        public SolutionTemplateWizardPage(SolutionTemplateWizard wizard)
        {
            _wizard = wizard;
            CanMoveToNextPage = false;
        }

        protected override object CreateNativeWidget<T>()
        {
            if (_view == null)
            {
                _view = new GtkSolutionTemplateWizardWidget();
                _view.WizardPage = this;
            }
            return _view;
        }

        public override string Title => $"Configure your {_wizard.Parameters["TemplateName"]}";

        private void UpdateCanMoveNextPage() => CanMoveToNextPage = (!string.IsNullOrWhiteSpace(_applicationIdentifier) && !string.IsNullOrWhiteSpace(_applicationName));

        protected override void Dispose(bool disposing)
        {
            if (_view != null)
            {
                _view.Dispose();
                _view = null;
            }
        }
    }
}
