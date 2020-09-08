﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;

namespace RapidXamlToolkit.Configuration
{
    public class RxtSettings
    {
        public string TelemetryKey { get; private set; } = "6136ce82-a6ad-4890-b203-ce2b3d8b8654";

        public Guid LightBulbTelemetryGuid { get; private set; } = new Guid("353898AB-8E5D-4DBF-BF72-0089A67C30A2");

        public bool ExtendedOutputEnabledByDefault { get; private set; } = true;
    }
}
