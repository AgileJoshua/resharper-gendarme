using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Design.MarkAssemblyWith
{
    internal class MarkAssemblyWithDaemonProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public MarkAssemblyWithDaemonProcess([NotNull] IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public IDaemonProcess DaemonProcess { get { return _daemonProcess; } }

        public void Execute(Action<DaemonStageResult> committer)
        {
            if (!_daemonProcess.FullRehighlightingRequired)
                return;

            var hightlighings = new List<HighlightingInfo>();

            _daemonProcess.Solution.ForEachProject(p => AnalyzeProject(p, hightlighings));

            committer(new DaemonStageResult(hightlighings));
        }

        private void AnalyzeProject(IProject p, IList<HighlightingInfo> hightlighings)
        {
            var sections = new List<IAttributeSection>();
            ICSharpFile assemblyInfoFile = null;

            foreach (IProjectItem item in p.GetSubItemsRecursively())
            {
                var projFile = item as IProjectFile;
                if (projFile == null)
                    continue;

                var cs = projFile.GetPrimaryPsiFile() as ICSharpFile;
                if (cs != null && !cs.Sections.IsEmpty)
                {
                    sections.AddRange(cs.SectionsEnumerable);
                    assemblyInfoFile = cs;
                }
            }

            CheckAttributeExistence(typeof(ComVisibleAttribute), sections, hightlighings, assemblyInfoFile);
            CheckAttributeExistence(typeof(CLSCompliantAttribute), sections, hightlighings, assemblyInfoFile);
            CheckAttributeExistence(typeof(AssemblyVersionAttribute), sections, hightlighings, assemblyInfoFile);
        }

        private void CheckAttributeExistence(Type attributeType, IList<IAttributeSection> sections, IList<HighlightingInfo> highlightings, ICSharpFile assemblyInfo)
        {
            if (!sections.Any() || assemblyInfo == null)
                return;

            bool isAttributeExists = false;
            string attributeName = attributeType.Name.Replace(typeof(Attribute).Name, string.Empty);
            foreach (IAttributeSection section in sections)
            {
                foreach (IAttribute attr in section.Attributes)
                {
                    if (attr.Name == null || string.IsNullOrEmpty(attr.Name.ShortName))
                        continue;

                    if (attr.Name.ShortName.Equals(attributeName))
                    {
                        isAttributeExists = true;
                        break;
                    }
                }

                if (isAttributeExists)
                    break;
            }

            if (!isAttributeExists)
                highlightings.Add(new HighlightingInfo(assemblyInfo.GetDocumentRange(), new MarkAssemblyWithHighlighting(attributeName)));

        }
    }
}