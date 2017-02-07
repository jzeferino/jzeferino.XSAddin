using MonoDevelop.Ide.Templates;

namespace jzeferino.XSAddin.Wizard
{
    public class SolutionTemplateWizard : TemplateWizard
    {
        public override string Id => "solution.wizard";

        public override WizardPage GetPage(int pageNumber) => new SolutionTemplateWizardPage(this);

        public override int TotalPages => 1;
    }
}
