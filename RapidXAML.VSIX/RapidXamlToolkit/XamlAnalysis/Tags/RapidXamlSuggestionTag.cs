﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace RapidXamlToolkit.XamlAnalysis.Tags
{
    // TODO: make these show as screwdriver in the margin, not a lightbuln
    public abstract class RapidXamlSuggestionTag : RapidXamlErrorListTag
    {
        protected RapidXamlSuggestionTag(Span span, ITextSnapshot snapshot)
            : base(span, snapshot)
        {
        }

        public override ITagSpan<IErrorTag> AsErrorTag()
        {
            var span = new SnapshotSpan(this.Snapshot, this.Span);
            return new TagSpan<IErrorTag>(span, new RapidXamlSuggestionAdornmentTag(this.ToolTip));
        }
        }
}
