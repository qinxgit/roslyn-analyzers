// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{                 
    public sealed class CA1307SpecifyStringComparisonTests : DiagnosticAnalyzerTestBase
    {

        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1307ShouldUseOverloadsWithExplicitStringComparisionParameterTests()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C //5
{
    void M(string strA, string strB)
    {
        bool b;
        int r; // 10
        r =  String.Compare(strA, strB);
        r += String.Compare(strA, 0, strB, 0, 1);
        r += strA.IndexOf(strB);
        r += strA.IndexOf(""abc"", 0);
        r += ""xyz"".IndexOf(strB, 0, 1); //15
        r += strA.CompareTo(strB);
        r += strA.CompareTo((object)strB);
        r += ""uvw"".CompareTo(strB);
        b =      strA.StartsWith(strB);
        b = b || strA.EndsWith(strB); // 20

        // The following calls are okay as far as CA1307 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal);
        r += String.Compare(strA, strB, StringComparison.CurrentCulture);
        r += String.Compare(strA, 0, strB, 0, 1, StringComparison.CurrentCulture);
        r += String.Compare(strA, 0, strB, 0, 1, StringComparison.Ordinal);
        r += strA.IndexOf(strB, StringComparison.Ordinal);
        r += strA.IndexOf(strB, 0, StringComparison.CurrentCultureIgnoreCase);
        r += strA.IndexOf(strB, 0, 1, StringComparison.CurrentCulture);
        if ((r == 0) || b
            || String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase)
            || strA.Equals(strB, StringComparison.Ordinal)
            || String.Equals(strA, strB)
            || strA.Equals(strB)
            || strA.StartsWith(strB, StringComparison.CurrentCulture)
            || strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase)
        ) {
            Console.WriteLine(strA == strB);
        }
    }
}",
            GetCA1307CSharpDefaultResultAt(11, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(12, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(13, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(14, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(15, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(16, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(17, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(18, 14, "", "", ""),
            GetCA1307CSharpDefaultResultAt(19, 18, "", "", ""),
            GetCA1307CSharpDefaultResultAt(20, 18, "", "", ""));

            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
        Dim b As Boolean
        Dim r As Integer ' 10
        r =  String.Compare(strA, strB)
        r += String.Compare(strA, 0, strB, 0, 1)
        r += strA.IndexOf(strB)
        r += strA.IndexOf(""abc"", 0)
        r += ""xyz"".IndexOf(strB, 0, 1) ' 15
        r += strA.CompareTo(strB)
        r += strA.CompareTo(DirectCast(strB, Object))
        r += ""uvw"".CompareTo(strB)
        b =      strA.StartsWith(strB)
        b = b Or strA.EndsWith(strB) ' 20

        ' The following calls are okay as far as CA1307 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal)
        r += String.Compare(strA, strB, StringComparison.CurrentCulture)
        ' Roslyn gives back wrong method info on the following two calls
        'r += String.Compare(strA, strB, 0, 1, StringComparison.CurrentCulture)
        'r += String.Compare(strA, strB, 0, 1, StringComparison.Ordinal)
        r += strA.IndexOf(strB, StringComparison.Ordinal)
        r += strA.IndexOf(strB, 0, StringComparison.CurrentCultureIgnoreCase)
        r += strA.IndexOf(strB, 0, 1, StringComparison.CurrentCulture)
        If ((r = 0) Or b _
            Or String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase) _
            Or strA.Equals(strB, StringComparison.Ordinal) _
            Or String.Equals(strA, strB) _
            Or strA.Equals(strB) _
            Or strA.StartsWith(strB, StringComparison.CurrentCulture) _
            Or strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase)) Then
                Console.WriteLine(strA == strB)
        End If
    End Sub
End Module",
            GetCA1307BasicDefaultResultAt(11, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(12, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(13, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(14, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(15, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(16, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(17, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(18, 14, "", "", ""),
            GetCA1307BasicDefaultResultAt(19, 18, "", "", ""),
            GetCA1307BasicDefaultResultAt(20, 18, "", "", ""));
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpCA1304DiagnosticAnalyzer();
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new VisualBasicCA1304DiagnosticAnalyzer();
        }

        internal static string CA1307Name = CA1304DiagnosticAnalyzer.RuleId1307;

        private static DiagnosticResult GetCA1307CSharpDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            //Because the behavior of '{0}' could vary based on the current user's locale settings, replace this call in '{1}' 
            // with a call to '{2}'. If the result of '{2}' will be displayed to the user, such as when sorting a list of items 
            // for display in a list box, specify 'StringComparison.CurrentCulture' or 'StringComparison.CurrentCultureIgnoreCase'
            // as the 'StringComparison' parameter. If comparing case-insensitive identifiers, such as file paths, environment 
            // variables, or registry keys and values, specify 'StringComparison.OrdinalIgnoreCase'. Otherwise, if comparing 
            // case-sensitive identifiers, specify 'StringComparison.Ordinal'.
            var message = string.Format(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDiagnosis, callee, caller, preferred);
            return GetCSharpResultAt(line, column, CA1307Name, message);
        }

        private static DiagnosticResult GetCA1307BasicDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDiagnosis, callee, caller, preferred);
            return GetBasicResultAt(line, column, CA1307Name, message);
        }
    }
}
