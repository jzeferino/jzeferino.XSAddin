using System;
namespace jzeferino.XSAddin.Wizard
{
    public interface ISolutionTemplateWizardView : IDisposable
    {
        SolutionTemplateWizardPage WizardPage
        {
            get;
            set;
        }
    }
}
