using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Lib.Extenstions
{
    public static class SolutionExtensions
    {
        public static bool HasCSharpFile([NotNull]this ISolution solution)
        {
            ICollection<IProject> projects = solution.GetAllProjects();
            if (projects == null)
                return false;

            bool hasCsFiles = false;
            foreach (IProject project in projects)
            {
                if (string.IsNullOrEmpty(project.Name))
                    continue;

                foreach (IProjectItem item in project.GetSubItems())
                {
                    var projFile = item as IProjectFile;
                    if (projFile == null)
                        continue;

                    var cs = projFile.GetPrimaryPsiFile() as ICSharpFile;
                    if (cs != null)
                    {
                        hasCsFiles = true;
                        break;
                    }
                }

                if (hasCsFiles) break;
            }

            return hasCsFiles;
        }

        public static void ProccessCSharpFiles<T>([NotNull] this ISolution solution, [NotNull] Action<T> handler) where T : class, ITreeNode
        {
            if (!solution.HasCSharpFile())
                return;

            ICollection<IProject> projects = solution.GetAllProjects();
            if (projects == null)
                return;

            foreach (IProject project in projects)
            {
                if (string.IsNullOrEmpty(project.Name))
                    continue;

                foreach (IProjectItem item in project.GetSubItems())
                {
                    var projFile = item as IProjectFile;
                    if (projFile == null)
                        continue;

                    var cs = projFile.GetPrimaryPsiFile() as ICSharpFile;
                    if (cs != null)
                    {
                        cs.ProcessChildren(handler);
                    }
                }
            }
        }
    }
}