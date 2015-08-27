// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;

namespace System.Globalization.Analyzers.UnitTests
{                              
    public sealed class CA1309UseOrdinalStringComparisonTests : DiagnosticAnalyzerTestBase
    {
#pragma warning disable CS0219
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureTests_StringEquals_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        bool r;
        r =      strA.Equals(strB, StringComparison.InvariantCulture);
        r = r || String.Equals(strA, strB, StringComparison.InvariantCulture);
        r = r || strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase);
        r = r || String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase);
        r = r || C.Equals0(strA, strB);
        r = r || C.Equals1(strA, strB);

        // The following calls are okay as far as CA1309 is concerned
        r = r || strA.Equals(strB);
        r = r || strA.Equals(strB, StringComparison.CurrentCulture);
        r = r || strA.Equals(strB, StringComparison.CurrentCultureIgnoreCase);
        r = r || strA.Equals(strB, StringComparison.Ordinal);
        r = r || strA.Equals(strB, StringComparison.OrdinalIgnoreCase);
        r = r || String.Equals(strA, strB);
        r = r || String.Equals(strA, strB, StringComparison.Ordinal);
        r = r || String.Equals(strA, strB, StringComparison.OrdinalIgnoreCase);
        r = r || String.Equals(strA, strB, StringComparison.CurrentCulture);
        r = r || String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase);
        StringComparison sc = StringComparison.InvariantCulture;
        r = r || strA.Equals(strB, sc);
        if (((strA == strB) == (strA != strB)) == r)
        {
            Console.WriteLine(""Interesting"");
        }
    }

    static bool Equals0(string a, string b, StringComparison sc = StringComparison.InvariantCulture)
    {
        return a.Equals(b, sc);
    }

    static bool Equals1(string a, string b, StringComparison sc = StringComparison.InvariantCultureIgnoreCase)
    {
        return a.Equals(b, sc);
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 36, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 41, 67)}
                }
            }; 
        }                   

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureTests_StringCompareIndexOf_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  String.Compare(strA, strB, StringComparison.InvariantCulture); // 10
        r += String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase);
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCulture);
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCultureIgnoreCase);
        r += String.Compare(strA, strB, false, CultureInfo.InvariantCulture);
        r += String.Compare(strA, 0, strB, 0, 0, true, CultureInfo.InvariantCulture); // 15
        if ( strA.StartsWith(strB, true, CultureInfo.InvariantCulture)
           | strA.StartsWith(strB, StringComparison.InvariantCulture)
           | strA.StartsWith(strB, StringComparison.InvariantCultureIgnoreCase)
           | strA.EndsWith(strB, false, CultureInfo.InvariantCulture)
           | strA.EndsWith(strB, StringComparison.InvariantCulture) // 20
           | strA.EndsWith(strB, StringComparison.InvariantCultureIgnoreCase))
        r += strA.IndexOf(""abc"", StringComparison.InvariantCulture);
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCulture);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCulture);
        r += strA.IndexOf(""abc"", StringComparison.InvariantCultureIgnoreCase); //25
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCultureIgnoreCase);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", StringComparison.InvariantCulture);
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCulture);
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCulture); // 30
        r += strA.LastIndexOf(""a"", StringComparison.InvariantCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCultureIgnoreCase);

        // The following calls are okay as far as CA1309 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal);
        r += String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase);
        r += String.Compare(strA, strB, StringComparison.CurrentCulture);
        r += String.Compare(strA, strB, StringComparison.CurrentCultureIgnoreCase);
        if ( strA.StartsWith(strB, true, CultureInfo.CurrentCulture)
           | strA.StartsWith(strB, StringComparison.CurrentCulture)
           | strA.StartsWith(strB, StringComparison.CurrentCultureIgnoreCase)
           | strA.EndsWith(strB, false, CultureInfo.CurrentCulture)
           | strA.EndsWith(strB, StringComparison.CurrentCulture)
           | strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase))
        r += strA.IndexOf(""abc"", StringComparison.CurrentCulture);
        r += strA.IndexOf(""abc"", 0, StringComparison.CurrentCulture);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.CurrentCulture);
        r += strA.IndexOf(""abc"", StringComparison.CurrentCultureIgnoreCase);
        r += strA.IndexOf(""abc"", 0, StringComparison.CurrentCultureIgnoreCase);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.CurrentCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", StringComparison.CurrentCulture);
        r += strA.LastIndexOf(""a"", 0, StringComparison.CurrentCulture);
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.CurrentCulture);
        r += strA.LastIndexOf(""a"", StringComparison.CurrentCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", 0, StringComparison.CurrentCultureIgnoreCase);
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.CurrentCultureIgnoreCase);
        if (r != 0)
        {
            Console.WriteLine(""Interesting"");
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 22, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 23, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 24, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 25, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 26, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 27, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 28, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 29, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 30, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 31, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 32, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 33, 14)}
                }
           };                                              
        }      

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureComparerTests_CS()
        {
            string source = @"
using System;
using System.Globalization;
using System.Collections;

sealed class C
{
    void Bad()
    {
        SortedList sl;
        sl = new SortedList(StringComparer.InvariantCulture);
        sl = new SortedList(StringComparer.InvariantCultureIgnoreCase);
        sl = new SortedList(Comparer.DefaultInvariant);
        sl = new SortedList(CaseInsensitiveComparer.DefaultInvariant);
        sl = new SortedList(new Comparer(CultureInfo.InvariantCulture));
        sl = new SortedList(new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
        Console.WriteLine(sl);

        Hashtable h;
        h = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
        h = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
        Console.WriteLine(h);

        System.Collections.Generic.Dictionary<string, int> d;
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.InvariantCulture);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        Console.WriteLine(d);
   }

    void Good()
    {
        // The following calls are okay as far as CA1309 is concerned
        SortedList sl;
        sl = new SortedList(StringComparer.CurrentCulture);
        sl = new SortedList(StringComparer.CurrentCultureIgnoreCase);
        sl = new SortedList(StringComparer.Ordinal);
        sl = new SortedList(StringComparer.OrdinalIgnoreCase);
        sl = new SortedList(Comparer.Default);
        sl = new SortedList(CaseInsensitiveComparer.Default);
        sl = new SortedList(new Comparer(CultureInfo.CurrentCulture));
        sl = new SortedList(new CaseInsensitiveComparer(CultureInfo.CurrentCulture));
        Console.WriteLine(sl);

        Hashtable h;
        h = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.Default);
        h = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.CurrentCulture));
        Console.WriteLine(h);

        System.Collections.Generic.Dictionary<string, int> d;
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.CurrentCulture);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        Console.WriteLine(d);
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 25, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 26, 13)}
                }
            };                                                           
        }

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureTests_StringEquals_VB()
        {
            string source = @"
Imports System
Imports System.Globalization

Public Class C
    Public Sub M(strA As String, strB As String)
        Dim r As Boolean
        r =      strA.Equals(strB, StringComparison.InvariantCulture)
        r = r Or String.Equals(strA, strB, StringComparison.InvariantCulture)
        r = r Or strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase)
        r = r Or String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase)
        r = r Or Equals0(strA, strB);
        r = r Or Equals1(strA, strB);

        ' The following calls are okay as far as CA1309 is concerned
        r = r Or strA.Equals(strB)
        r = r Or strA.Equals(strB, StringComparison.CurrentCulture)
        r = r Or strA.Equals(strB, StringComparison.CurrentCultureIgnoreCase)
        r = r Or strA.Equals(strB, StringComparison.Ordinal)
        r = r Or strA.Equals(strB, StringComparison.OrdinalIgnoreCase)
        r = r Or String.Equals(strA, strB)
        r = r Or String.Equals(strA, strB, StringComparison.Ordinal)
        r = r Or String.Equals(strA, strB, StringComparison.OrdinalIgnoreCase)
        r = r Or String.Equals(strA, strB, StringComparison.CurrentCulture)
        r = r Or String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase)
        If (((strA = strB) = (strA <> strB)) = r) Then
            Console.WriteLine(""Interesting"");
        End If
    End Sub

    Function Equals0(strA As String, strB As String, Optional sc As StringComparison = StringComparison.InvariantCulture) As Boolean
        Return strA.Equals(strB, sc)
    End Function

    Function Equals1(strA As String, strB As String, Optional sc As StringComparison = StringComparison.InvariantCultureIgnoreCase) As Boolean
        Return strA.Equals(strB, sc)
    End Function
End Class ' C";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 8, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 31, 88)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 35, 88)}
                }
            };                                               
        }

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureTests_StringCompareIndexOf_VB()
        {
            string source = @"
Imports System
Imports System.Globalization

Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  String.Compare(strA, strB, StringComparison.InvariantCulture)
        r += String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase)
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCulture) ' 10
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCultureIgnoreCase)
        r += String.Compare(strA, strB, false, CultureInfo.InvariantCulture)
        r += String.Compare(strA, 0, strB, 0, 0, true, CultureInfo.InvariantCulture)
        If ( strA.StartsWith(strB, true, CultureInfo.InvariantCulture) _
          Or strA.StartsWith(strB, StringComparison.InvariantCulture) _
          Or strA.StartsWith(strB, StringComparison.InvariantCultureIgnoreCase) _
          Or strA.EndsWith(strB, false, CultureInfo.InvariantCulture) _
          Or strA.EndsWith(strB, StringComparison.InvariantCulture) _
          Or strA.EndsWith(strB, StringComparison.InvariantCultureIgnoreCase)) Then
        r += strA.IndexOf(""abc"", StringComparison.InvariantCulture) ' 20
        End If
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCulture)
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCulture)
        r += strA.IndexOf(""abc"", StringComparison.InvariantCultureIgnoreCase)
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCultureIgnoreCase) ' 25
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCultureIgnoreCase)
        r += strA.LastIndexOf(""a"", StringComparison.InvariantCulture)
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCulture)
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCulture)
        r += strA.LastIndexOf(""a"", StringComparison.InvariantCultureIgnoreCase) ' 30
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCultureIgnoreCase)
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCultureIgnoreCase)

        ' The following calls are okay as far as CA1309 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal)
        r += String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase)
        r += String.Compare(strA, strB, StringComparison.CurrentCulture)
        r += String.Compare(strA, strB, StringComparison.CurrentCultureIgnoreCase)
        If ( strA.StartsWith(strB, true, CultureInfo.CurrentCulture)
           | strA.StartsWith(strB, StringComparison.CurrentCulture)
           | strA.StartsWith(strB, StringComparison.CurrentCultureIgnoreCase)
           | strA.EndsWith(strB, false, CultureInfo.CurrentCulture)
           | strA.EndsWith(strB, StringComparison.CurrentCulture)
           | strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase)) Then
        r += strA.IndexOf(""abc"", StringComparison.CurrentCulture)
        EndIf
        r += strA.IndexOf(""abc"", 0, StringComparison.CurrentCulture)
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.CurrentCulture)
        r += strA.IndexOf(""abc"", StringComparison.CurrentCultureIgnoreCase)
        r += strA.IndexOf(""abc"", 0, StringComparison.CurrentCultureIgnoreCase)
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.CurrentCultureIgnoreCase)
        r += strA.LastIndexOf(""a"", StringComparison.CurrentCulture)
        r += strA.LastIndexOf(""a"", 0, StringComparison.CurrentCulture)
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.CurrentCulture)
        r += strA.LastIndexOf(""a"", StringComparison.CurrentCultureIgnoreCase)
        r += strA.LastIndexOf(""a"", 0, StringComparison.CurrentCultureIgnoreCase)
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.CurrentCultureIgnoreCase)
        If (r <> 0) Then
            Console.WriteLine(""Interesting"")
        End If
    End Sub
End Class ' C";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 8, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 15, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 16, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 17, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 18, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 19, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 20, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 22, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 23, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 24, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 25, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 26, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 27, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 28, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 29, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 30, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 31, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 32, 14)}
                },
           };                                                
        }

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1309ReviewUsesOfInvariantCultureComparerTests_VB()
        {
            string source = @"
Imports System
Imports System.Globalization
Imports System.Collections

Modlue C
    Sub Bad()
        Dim sl As SortedList
        sl = New SortedList(StringComparer.InvariantCulture)
        sl = New SortedList(StringComparer.InvariantCultureIgnoreCase)
        sl = New SortedList(Comparer.DefaultInvariant)
        sl = New SortedList(CaseInsensitiveComparer.DefaultInvariant)
        sl = New SortedList(New Comparer(CultureInfo.InvariantCulture))
        sl = New SortedList(New CaseInsensitiveComparer(CultureInfo.InvariantCulture))
        Console.WriteLine(sl)

        Dim h As Hashtable
        h = New Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant)
        h = New Hashtable(New CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), New CaseIngitksensitiveComparer(CultureInfo.InvariantCulture))
        Console.WriteLine(h)

        Dim d As System.Collections.Generic.Dictionary(Of String, Integer)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.InvariantCulture)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.InvariantCultureIgnoreCase)
        Console.WriteLine(d)
    End Sub

    Sub Good()
        ' The following calls are okay as far as CA1309 is concerned
        Dim sl As SortedList
        sl = New SortedList(StringComparer.CurrentCulture)
        sl = New SortedList(StringComparer.CurrentCultureIgnoreCase)
        sl = New SortedList(StringComparer.Ordinal)
        sl = New SortedList(StringComparer.OrdinalIgnoreCase)
        sl = New SortedList(Comparer.Default)
        sl = New SortedList(CaseInsensitiveComparer.Default)
        sl = New SortedList(New Comparer(CultureInfo.CurrentCulture))
        sl = New SortedList(New CaseInsensitiveComparer(CultureInfo.CurrentCulture))
        Console.WriteLine(sl)

        Dim h As Hashtable
        h = New Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.Default)
        h = New Hashtable(New CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), New CaseInsensitiveComparer(CultureInfo.CurrentCulture))
        Console.WriteLine(h)

        Dim d As System.Collections.Generic.Dictionary(Of String, Integer)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.CurrentCulture)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.CurrentCultureIgnoreCase)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.Ordinal)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Console.WriteLine(d)
    End Sub
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 18, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 19, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 23, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1309,
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 24, 13)}
                }
            };                                           
        }
#pragma warning restore CS0219

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpCA1304DiagnosticAnalyzer();
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new VisualBasicCA1304DiagnosticAnalyzer();
        }

        internal static string CA1309Name = CA1304DiagnosticAnalyzer.RuleId1309;

        private static DiagnosticResult GetCSharpUseOrdinalStringComparerResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparerDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }

        private static DiagnosticResult GetBasicUseOrdinalStringComparerResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparerDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }
        private static DiagnosticResult GetCSharpUseOrdinalStringComparisonDefaultResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDefaultDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }

        private static DiagnosticResult GetBasicUseOrdinalStringComparisonDefaultResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDefaultDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }
        private static DiagnosticResult GetCSharpUseOrdinalStringComparisonResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }

        private static DiagnosticResult GetBasicUseOrdinalStringComparisonResultAt(int line, int column, params string[] arguments)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDiagnosis, arguments);
            return GetCSharpResultAt(line, column, CA1309Name, message);
        }
    }
}                              
