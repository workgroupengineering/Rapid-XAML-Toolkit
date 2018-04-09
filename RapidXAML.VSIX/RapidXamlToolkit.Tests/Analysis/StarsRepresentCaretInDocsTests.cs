﻿// <copyright file="StarsRepresentCaretInDocsTests.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidXamlToolkit.Tests.Analysis
{
    public abstract class StarsRepresentCaretInDocsTests
    {
        public TestContext TestContext { get; set; }

        public Profile DefaultProfile => GetDefaultTestSettings().GetActiveProfile();

        public static Settings GetDefaultTestSettings()
        {
            return new Settings
            {
                ActiveProfileName = "UWP",
                Profiles = new List<Profile>
                {
                    new Profile
                    {
                        Name = "UWP",
                        ClassGrouping = "StackPanel",
                        DefaultOutput = "<TextBlock Text=\"FALLBACK_{NAME}\" />",
                        Mappings = new List<Mapping>
                        {
                            new Mapping
                            {
                                Type = "String",
                                NameContains = "password|pwd",
                                Output = "<PasswordBox Password=\"{x:Bind {NAME}}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBox Text=\"{x:Bind {NAME}, Mode=TwoWay}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" />",
                                IfReadOnly = true,
                            },
                            new Mapping
                            {
                                Type = "int|Integer",
                                NameContains = string.Empty,
                                Output = "<Slider Minimum=\"0\" Maximum=\"100\" x:Name=\"{NAME}\" Value=\"{x:Bind {NAME}, Mode=TwoWay}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "List<string>",
                                NameContains = string.Empty,
                                Output = "<ItemsControl ItemsSource=\"{x:Bind {NAME}}\"></ItemsControl>",
                                IfReadOnly = false,
                            },
                        },
                    },

                    new Profile
                    {
                        Name = "UWP (with labels)",
                        ClassGrouping = "Grid-plus-RowDefs",
                        DefaultOutput = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBlock Text=\"FALLBACK_{NAME}\" Grid.Row=\"{X}\" />",
                        Mappings = new List<Mapping>
                        {
                            new Mapping
                            {
                                Type = "String",
                                NameContains = "password|pwd",
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><PasswordBox Password=\"{x:Bind {NAME}}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBox Text=\"{x:Bind {NAME}, Mode=TwoWay}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = true,
                            },
                            new Mapping
                            {
                                Type = "int|Integer",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><Slider Minimum=\"0\" Maximum=\"100\" x:Name=\"{NAME}\" Value=\"{x:Bind {NAME}, Mode=TwoWay}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "List<string>",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><ItemsControl ItemsSource=\"{x:Bind {NAME}}\"></ItemsControl>",
                                IfReadOnly = false,
                            },
                        },
                    },

                    new Profile
                    {
                        Name = "WPF",
                        ClassGrouping = "StackPanel",
                        DefaultOutput = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBlock Text=\"FALLBACK_{NAME}\" Grid.Row=\"{X}\" />",
                        Mappings = new List<Mapping>
                        {
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBox Text=\"{x:Bind {NAME}, Mode=TwoWay}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = false,
                            },
                            new Mapping
                            {
                                Type = "String",
                                NameContains = string.Empty,
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = true,
                            },
                        },
                    },

                    new Profile
                    {
                        Name = "Xamarin.Forms",
                        ClassGrouping = "StackLayout",
                        DefaultOutput = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><TextBlock Text=\"FALLBACK_{NAME}\" Grid.Row=\"{X}\" />",
                        Mappings = new List<Mapping>
                        {
                            new Mapping
                            {
                                Type = "String",
                                NameContains = "password|pwd",
                                Output = "<TextBlock Text=\"{NAME}\" Grid.Row=\"{X}\"><PasswordBox Password=\"{x:Bind {NAME}}\" Grid.Row=\"{X}\" />",
                                IfReadOnly = false,
                            },
                        },
                    },
                },
            };
        }

        protected void EachPositionBetweenStarsShouldProduceExpected(string code, AnalyzerOutput expected, bool isCSharp, Profile profileOverload)
        {
            var startPos = code.IndexOf("*", StringComparison.Ordinal);
            var endPos = code.LastIndexOf("*", StringComparison.Ordinal) - 1;

            var syntaxTree = isCSharp ? CSharpSyntaxTree.ParseText(code.Replace("*", string.Empty))
                                      : VisualBasicSyntaxTree.ParseText(code.Replace("*", string.Empty));

            Assert.IsNotNull(syntaxTree);

            var semModel = isCSharp ? CSharpCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, ignoreAccessibility: true)
                                    : VisualBasicCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, ignoreAccessibility: true);

            var positionsTested = 0;

            for (var pos = startPos; pos < endPos; pos++)
            {
                IDocumentAnalyzer analyzer = isCSharp ? new CSharpAnalyzer() as IDocumentAnalyzer : new VisualBasicAnalyzer();

                var actual = analyzer.GetSingleItemOutput(syntaxTree.GetRoot(), semModel, pos, profileOverload);

                Assert.AreEqual(expected.OutputType, actual.OutputType, $"Failure at {pos} ({startPos}-{endPos})");
                Assert.AreEqual(expected.Name, actual.Name, $"Failure at {pos} ({startPos}-{endPos})");
                Assert.AreEqual(expected.Output, actual.Output, $"Failure at {pos} ({startPos}-{endPos})");

                positionsTested += 1;
            }

            this.TestContext.WriteLine($"{positionsTested} different positions tested.");
        }

        protected void PositionAtStarShouldProduceExpected(string code, AnalyzerOutput expected, bool isCSharp, Profile profileOverload)
        {
            var pos = code.IndexOf("*", StringComparison.Ordinal);

            var syntaxTree = isCSharp ? CSharpSyntaxTree.ParseText(code.Replace("*", string.Empty))
                                      : VisualBasicSyntaxTree.ParseText(code.Replace("*", string.Empty));

            Assert.IsNotNull(syntaxTree);

            var semModel = isCSharp ? CSharpCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, true)
                                    : VisualBasicCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, true);

            IDocumentAnalyzer analyzer = isCSharp ? new CSharpAnalyzer() as IDocumentAnalyzer : new VisualBasicAnalyzer();

            var actual = analyzer.GetSingleItemOutput(syntaxTree.GetRoot(), semModel, pos, profileOverload);

            Assert.AreEqual(expected.OutputType, actual.OutputType);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Output, actual.Output);
        }

        protected void PositionAtStarShouldProduceExpectedUsingAdditonalFiles(string code, AnalyzerOutput expected, bool isCSharp, Profile profileOverload, params string[] additionalCode)
        {
            var pos = code.IndexOf("*", StringComparison.Ordinal);

            SyntaxTree syntaxTree = null;
            SemanticModel semModel = null;

            var projectId = ProjectId.CreateNewId();
            var documentId = DocumentId.CreateNewId(projectId);

            if (isCSharp)
            {
                var solution = new AdhocWorkspace().CurrentSolution
                                                   .AddProject(projectId, "MyProject", "MyProject", LanguageNames.CSharp)
                                                   .AddDocument(documentId, "MyFile.cs", code.Replace("*", string.Empty));

                foreach (var addCode in additionalCode)
                {
                    solution = solution.AddDocument(DocumentId.CreateNewId(projectId), $"{System.IO.Path.GetRandomFileName()}.cs", addCode);
                }

                var document = solution.GetDocument(documentId);

                semModel = document.GetSemanticModelAsync().Result;
                syntaxTree = document.GetSyntaxTreeAsync().Result;
            }
            else
            {
                var solution = new AdhocWorkspace().CurrentSolution
                                                   .AddProject(projectId, "MyProject", "MyProject", LanguageNames.VisualBasic)
                                                   .AddDocument(documentId, "MyFile.vb", code.Replace("*", string.Empty));

                foreach (var addCode in additionalCode)
                {
                    solution = solution.AddDocument(DocumentId.CreateNewId(projectId), $"{System.IO.Path.GetRandomFileName()}.vb", addCode);
                }

                var document = solution.GetDocument(documentId);

                semModel = document.GetSemanticModelAsync().Result;
                syntaxTree = document.GetSyntaxTreeAsync().Result;
            }

            IDocumentAnalyzer analyzer = isCSharp ? new CSharpAnalyzer() as IDocumentAnalyzer : new VisualBasicAnalyzer();

            var actual = analyzer.GetSingleItemOutput(syntaxTree.GetRoot(), semModel, pos, profileOverload);

            Assert.AreEqual(expected.OutputType, actual.OutputType);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Output, actual.Output);
        }

        protected void SelectionBetweenStarsShouldProduceExpected(string code, AnalyzerOutput expected, bool isCSharp, Profile profileOverload)
        {
            var startPos = code.IndexOf("*", StringComparison.Ordinal);
            var endPos = code.LastIndexOf("*", StringComparison.Ordinal) - 1;

            var syntaxTree = isCSharp ? CSharpSyntaxTree.ParseText(code.Replace("*", string.Empty))
                : VisualBasicSyntaxTree.ParseText(code.Replace("*", string.Empty));

            Assert.IsNotNull(syntaxTree);

            var semModel = isCSharp ? CSharpCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, true)
                : VisualBasicCompilation.Create(string.Empty).AddSyntaxTrees(syntaxTree).GetSemanticModel(syntaxTree, true);

            IDocumentAnalyzer analyzer = isCSharp ? new CSharpAnalyzer() as IDocumentAnalyzer : new VisualBasicAnalyzer();

            var actual = analyzer.GetSelectionOutput(syntaxTree.GetRoot(), semModel, startPos, endPos, profileOverload);

            Assert.AreEqual(expected.OutputType, actual.OutputType);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Output, actual.Output);
        }
    }
}