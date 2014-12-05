using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Lib.Extenstions
{
    public static class ProjectExtensions
    {
        public static void ProcessCSharpNodes<T>([NotNull] this IProject project, [NotNull] Action<T> handler) where T : class, ITreeNode
        {
            foreach (IProjectItem item in project.GetSubItemsRecursively())
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