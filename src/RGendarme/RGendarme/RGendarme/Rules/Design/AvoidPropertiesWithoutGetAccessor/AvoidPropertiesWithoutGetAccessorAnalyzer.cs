﻿using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.AvoidPropertiesWithoutGetAccessor
{
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] { typeof(AvoidPropertiesWithoutGetAccessorHighlighting) })]
    public class AvoidPropertiesWithoutGetAccessorAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration>
    {
        private readonly ISettingsStore _settings;

        public AvoidPropertiesWithoutGetAccessorAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool isGetDefined = false;

#warning refator it 
            // TODO: use AccessorDeclarationsEnumerable.FirstOrDefault(accessor => accessor.Kind == AccessorKind.GETTER);
            foreach (IAccessorDeclaration accessor in element.AccessorDeclarationsEnumerable)
            {
                if (accessor.NameIdentifier == null) continue;

                string name = accessor.NameIdentifier.Name;
                if (!string.IsNullOrEmpty(name) && name.Equals("get"))
                {
                    isGetDefined = true;
                    break;
                }
            }

            if (!isGetDefined)
            {
                consumer.AddHighlighting(new AvoidPropertiesWithoutGetAccessorHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}