using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace jzeferino.XSAddin
{
    public static class Extensions
    {
        public static async Task SaveSolutionAsync(this Solution solution)
        {
            ProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetLoadProgressMonitor(true);
            try
            {
                await solution.SaveAsync(monitor);
                monitor.ReportSuccess(GettextCatalog.GetString("Success."));
            }
            catch (Exception ex)
            {
                monitor.ReportError(GettextCatalog.GetString("Save failed."), ex);
            }
            finally
            {
                monitor.Dispose();
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
