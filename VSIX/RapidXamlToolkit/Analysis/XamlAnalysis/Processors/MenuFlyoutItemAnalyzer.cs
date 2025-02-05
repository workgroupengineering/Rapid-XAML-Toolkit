﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using RapidXaml;
using RapidXamlToolkit.Logging;
using RapidXamlToolkit.VisualStudioIntegration;
using RapidXamlToolkit.XamlAnalysis.CustomAnalysis;

namespace RapidXamlToolkit.XamlAnalysis.Processors
{
    public class MenuFlyoutItemAnalyzer : BuiltInXamlAnalyzer
    {
        public MenuFlyoutItemAnalyzer(IVisualStudioAbstraction vsa, ILogger logger)
            : base(vsa, logger)
        {
        }

        public override string TargetType() => Elements.MenuFlyoutItem;

        public override AnalysisActions Analyze(RapidXamlElement element, ExtraAnalysisDetails extraDetails)
        {
            // TODO: Review MAUI support
            if (!extraDetails.IsFramework(ProjectFramework.Uwp) && !extraDetails.IsFramework(ProjectFramework.WinUI))
            {
                return AnalysisActions.None;
            }

            var result = this.CheckForHardCodedString(Attributes.Text, AttributeType.InlineOrElement, element, extraDetails);

            return result;
        }
    }
}
