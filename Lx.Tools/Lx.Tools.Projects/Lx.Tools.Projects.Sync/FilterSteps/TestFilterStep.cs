using System;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    /// <summary>
    /// Filter source file related to project depending whether it's a test project or not
    /// </summary>
    internal class TestFilterStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            return files.Where(x => FilterTest(attributes.IsTest, x)).ToArray();
        }

        private bool FilterTest(bool isTest, string sourceFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFile);
            if (!string.IsNullOrEmpty(fileName))
            {
                var res = fileName.ToLower().Contains("test");
                return isTest ? res : !res;
            }
            return false;
        }
    }
}