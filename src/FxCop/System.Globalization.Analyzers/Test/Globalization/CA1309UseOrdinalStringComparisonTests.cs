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

        [Fact]
        public void CA1309StringEqualsInvarianCulture()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        bool r;
        r =      strA.Equals(strB, StringComparison.InvariantCulture);
        r = r || String.Equals(strA, strB, StringComparison.InvariantCulture);
        
    }
}",

            GetCA1309CSharpDefaultResultAt(10, 18,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCulture",
                                                 "string.Equals(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 18,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCulture",
                                                 "string.Equals(string, string, System.StringComparison)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization

Public Class C
    Public Sub M(strA As String, strB As String)
        Dim r As Boolean
        r =      strA.Equals(strB, StringComparison.InvariantCulture)
        r = r Or String.Equals(strA, strB, StringComparison.InvariantCulture)
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(8, 18, "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCulture",
                                                 "Public Overloads Function Equals(value As String, comparisonType As System.StringComparison) As Boolean"),
            GetCA1309BasicDefaultResultAt(9, 18, "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCulture",
                                                 "Public Shared Overloads Function Equals(a As String, b As String, comparisonType As System.StringComparison) As Boolean"));
        }

        [Fact]
        public void CA1309StringEqualsInvarianCultureIgnoreCase()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        bool r;
        r =      strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase);
        r = r || String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase);
        
    }
}",

            GetCA1309CSharpDefaultResultAt(10, 18,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.Equals(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 18,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.Equals(string, string, System.StringComparison)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization

Public Class C
    Public Sub M(strA As String, strB As String)
        Dim r As Boolean
        r =      strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase)
        r = r Or String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase)
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(8, 18, "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function Equals(value As String, comparisonType As System.StringComparison) As Boolean"),
            GetCA1309BasicDefaultResultAt(9, 18, "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Shared Overloads Function Equals(a As String, b As String, comparisonType As System.StringComparison) As Boolean"));
        }

        [Fact]
        public void CA1309StringEqualsMethod()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        bool r;
        r =      C.Equals0(strA, strB);
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
}",

            GetCA1309CSharpDefaultResultAt(10, 18,
                                                 "C.M(string, string)",
                                                 "InvariantCulture",
                                                 "C.Equals0(string, string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 18,
                                                 "C.M(string, string)",
                                                 "InvariantCultureIgnoreCase",
                                                 "C.Equals1(string, string, System.StringComparison)"),
            GetCA1309CSharpInvariantCultureResultAt(32, 67,
                                                "C.Equals0(string, string, System.StringComparison)",
                                                "StringComparison.InvariantCulture"),
            GetCA1309CSharpInvariantCultureResultAt(37, 67,
                                                "C.Equals1(string, string, System.StringComparison)",
                                                "StringComparison.InvariantCultureIgnoreCase"));

            VerifyBasic(@"
Imports System
Imports System.Globalization

Public Class C
    Public Sub M(strA As String, strB As String)
        Dim r As Boolean
        r =      Equals0(strA, strB);
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
End Class ' C",
            GetCA1309BasicDefaultResultAt(8, 18, "Public Sub M(strA As String, strB As String)",
                                                 "InvariantCulture",
                                                 "Public Function Equals0(strA As String, strB As String, [sc As System.StringComparison = InvariantCulture]) As Boolean"),
            GetCA1309BasicDefaultResultAt(9, 18, "Public Sub M(strA As String, strB As String)",
                                                 "InvariantCultureIgnoreCase",
                                                 "Public Function Equals1(strA As String, strB As String, [sc As System.StringComparison = InvariantCultureIgnoreCase]) As Boolean"),
            GetCA1309BasicInvariantCultureResultAt(27, 88, "Public Function Equals0(strA As String, strB As String, [sc As System.StringComparison = InvariantCulture]) As Boolean",
                                                 "StringComparison.InvariantCulture"),
            GetCA1309BasicInvariantCultureResultAt(31, 88, "Public Function Equals1(strA As String, strB As String, [sc As System.StringComparison = InvariantCultureIgnoreCase]) As Boolean",
                                                 "StringComparison.InvariantCultureIgnoreCase"));
        }

        [Fact]
        public void CA1309StringComparisonInvariantCulture()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  String.Compare(strA, strB, StringComparison.InvariantCulture); // 10
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCulture);
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.Compare(string, string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.Compare(string, int, string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  String.Compare(strA, strB, StringComparison.InvariantCulture)
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCulture) ' 10
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Shared Overloads Function Compare(strA As String, strB As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Shared Overloads Function Compare(strA As String, indexA As Integer, strB As String, indexB As Integer, length As Integer, comparisonType As System.StringComparison) As Integer"));
        }

        [Fact]
        public void CA1309StringComparisonInvariantCultureIgnoreCase()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
         
        r  = String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase);
         
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCultureIgnoreCase);
    }
}",

            GetCA1309CSharpDefaultResultAt(11, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.Compare(string, string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(13, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.Compare(string, int, string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
         
        r  = String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase)
         
        r += String.Compare(strA, 0, strB, 0, 0, StringComparison.InvariantCultureIgnoreCase)
        
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(11, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Shared Overloads Function Compare(strA As String, strB As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(13, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Shared Overloads Function Compare(strA As String, indexA As Integer, strB As String, indexB As Integer, length As Integer, comparisonType As System.StringComparison) As Integer"));
        }

        [Fact]
        public void CA1309StringCompareCultureInfoInvariantCuluture()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  String.Compare(strA, strB, false, CultureInfo.InvariantCulture);
        r += String.Compare(strA, 0, strB, 0, 0, true, CultureInfo.InvariantCulture); // 15
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                "C.M(string, string)",
                                                "CultureInfo.InvariantCulture",
                                                "string.Compare(string, string, bool, System.Globalization.CultureInfo)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                 "C.M(string, string)",
                                                 "CultureInfo.InvariantCulture",
                                                 "string.Compare(string, int, string, int, int, bool, System.Globalization.CultureInfo)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  String.Compare(strA, strB, false, CultureInfo.InvariantCulture)
        r += String.Compare(strA, 0, strB, 0, 0, true, CultureInfo.InvariantCulture)
    End Sub
End Class ' C",
           GetCA1309BasicDefaultResultAt(10, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "CultureInfo.InvariantCulture",
                                                "Public Shared Overloads Function Compare(strA As String, strB As String, ignoreCase As Boolean, culture As System.Globalization.CultureInfo) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "CultureInfo.InvariantCulture",
                                                 "Public Shared Overloads Function Compare(strA As String, indexA As Integer, strB As String, indexB As Integer, length As Integer, ignoreCase As Boolean, culture As System.Globalization.CultureInfo) As Integer"));
        }

        [Fact]
        public void CA1309StringCompareStartsWith()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        if ( strA.StartsWith(strB, true, CultureInfo.InvariantCulture)
           | strA.StartsWith(strB, StringComparison.InvariantCulture)
           | strA.StartsWith(strB, StringComparison.InvariantCultureIgnoreCase))
            r=0;
    }
}",
           GetCA1309CSharpDefaultResultAt(10, 14,
                                                "C.M(string, string)",
                                                "CultureInfo.InvariantCulture",
                                                "string.StartsWith(string, bool, System.Globalization.CultureInfo)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCulture",
                                                 "string.StartsWith(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "string.StartsWith(string, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        If ( strA.StartsWith(strB, true, CultureInfo.InvariantCulture) _
          Or strA.StartsWith(strB, StringComparison.InvariantCulture) _
          Or strA.StartsWith(strB, StringComparison.InvariantCultureIgnoreCase)) Then
        r =0 
        End If
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "CultureInfo.InvariantCulture",
                                                "Public Overloads Function StartsWith(value As String, ignoreCase As Boolean, culture As System.Globalization.CultureInfo) As Boolean"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCulture",
                                                 "Public Overloads Function StartsWith(value As String, comparisonType As System.StringComparison) As Boolean"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "Public Overloads Function StartsWith(value As String, comparisonType As System.StringComparison) As Boolean"));
        }

        [Fact]
        public void CA1309StringCompareEndsWith()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        if ( strA.EndsWith(strB, false, CultureInfo.InvariantCulture)
           | strA.EndsWith(strB, StringComparison.InvariantCulture) // 20
           | strA.EndsWith(strB, StringComparison.InvariantCultureIgnoreCase))
        r = 0;
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                 "C.M(string, string)",
                                                 "CultureInfo.InvariantCulture",
                                                 "string.EndsWith(string, bool, System.Globalization.CultureInfo)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.EndsWith(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.EndsWith(string, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        If ( strA.EndsWith(strB, false, CultureInfo.InvariantCulture) _
          Or strA.EndsWith(strB, StringComparison.InvariantCulture) _
          Or strA.EndsWith(strB, StringComparison.InvariantCultureIgnoreCase)) Then
        r = 0
        End If
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "CultureInfo.InvariantCulture",
                                                 "Public Overloads Function EndsWith(value As String, ignoreCase As Boolean, culture As System.Globalization.CultureInfo) As Boolean"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Overloads Function EndsWith(value As String, comparisonType As System.StringComparison) As Boolean"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function EndsWith(value As String, comparisonType As System.StringComparison) As Boolean"));
        }

        [Fact]
        public void CA1309StringCompareIndexOfInvariantCulture()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  strA.IndexOf(""abc"", StringComparison.InvariantCulture);
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCulture);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCulture);
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.IndexOf(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCulture",
                                                 "string.IndexOf(string, int, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.IndexOf(string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  strA.IndexOf(""abc"", StringComparison.InvariantCulture) 
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCulture)
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCulture)
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Overloads Function IndexOf(value As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCulture",
                                                 "Public Overloads Function IndexOf(value As String, startIndex As Integer, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Overloads Function IndexOf(value As String, startIndex As Integer, count As Integer, comparisonType As System.StringComparison) As Integer"));
        }

        [Fact]
        public void CA1309StringCompareIndexOfInvariantCultureIgnoreCase()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  strA.IndexOf(""abc"", StringComparison.InvariantCultureIgnoreCase); 
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCultureIgnoreCase);
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCultureIgnoreCase);
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.IndexOf(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "string.IndexOf(string, int, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.IndexOf(string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  strA.IndexOf(""abc"", StringComparison.InvariantCultureIgnoreCase)
        r += strA.IndexOf(""abc"", 0, StringComparison.InvariantCultureIgnoreCase) 
        r += strA.IndexOf(""abc"", 0, 1, StringComparison.InvariantCultureIgnoreCase)
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function IndexOf(value As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "Public Overloads Function IndexOf(value As String, startIndex As Integer, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function IndexOf(value As String, startIndex As Integer, count As Integer, comparisonType As System.StringComparison) As Integer"));
        }

        [Fact]
        public void CA1309StringCompareLastIndexOfInvariantCulture()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  strA.LastIndexOf(""a"", StringComparison.InvariantCulture);
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCulture);
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCulture);
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.LastIndexOf(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCulture",
                                                 "string.LastIndexOf(string, int, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCulture",
                                                "string.LastIndexOf(string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  strA.LastIndexOf(""a"", StringComparison.InvariantCulture)
        r += strA.LastIndexOf(""a"", 0, StringComparison.InvariantCulture)
        r += strA.LastIndexOf(""a"", 0, 1, StringComparison.InvariantCulture)
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Overloads Function LastIndexOf(value As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCulture",
                                                 "Public Overloads Function LastIndexOf(value As String, startIndex As Integer, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCulture",
                                                "Public Overloads Function LastIndexOf(value As String, startIndex As Integer, count As Integer, comparisonType As System.StringComparison) As Integer"));

        }

        [Fact]
        public void CA1309StringCompareLastIndexOfInvariantCultureIgnoreCase()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C // 5
{
    void M(string strA, string strB)
    {
        int r;
        r =  strA.LastIndexOf(""a"", StringComparison.InvariantCultureIgnoreCase);
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
    }
}",
            GetCA1309CSharpDefaultResultAt(10, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.LastIndexOf(string, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(11, 14,
                                                "C.M(string, string)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "string.LastIndexOf(string, int, System.StringComparison)"),
            GetCA1309CSharpDefaultResultAt(12, 14,
                                                 "C.M(string, string)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "string.LastIndexOf(string, int, int, System.StringComparison)"));

            VerifyBasic(@"

Imports System
Imports System.Globalization


Public Class C ' 5
    Public Sub M(strA As String, strB As String)
        Dim r As Integer
        r =  strA.LastIndexOf(""a"", StringComparison.InvariantCultureIgnoreCase) ' 30
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
    End Sub
End Class ' C",
            GetCA1309BasicDefaultResultAt(10, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function LastIndexOf(value As String, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(11, 14,
                                                "Public Sub M(strA As String, strB As String)",
                                                "StringComparison.InvariantCultureIgnoreCase",
                                                "Public Overloads Function LastIndexOf(value As String, startIndex As Integer, comparisonType As System.StringComparison) As Integer"),
            GetCA1309BasicDefaultResultAt(12, 14,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "StringComparison.InvariantCultureIgnoreCase",
                                                 "Public Overloads Function LastIndexOf(value As String, startIndex As Integer, count As Integer, comparisonType As System.StringComparison) As Integer"));
        }

        [Fact]
        public void CA1309InvariantComparerSortedList()
        {
            VerifyCSharp(@"
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
    }
}",

            GetCA1309CSharpStringComparerResultAt(11, 14,
                                                 "C.Bad()",
                                                 "StringComparer.InvariantCulture",
                                                 "System.Collections.SortedList.SortedList(System.Collections.IComparer)"),
            GetCA1309CSharpStringComparerResultAt(12, 14,
                                                "C.Bad()",
                                                "StringComparer.InvariantCultureIgnoreCase",
                                                "System.Collections.SortedList.SortedList(System.Collections.IComparer)"),
            GetCA1309CSharpStringComparerResultAt(13, 14,
                                                 "C.Bad()",
                                                 "Comparer.DefaultInvariant",
                                                 "System.Collections.SortedList.SortedList(System.Collections.IComparer)"),
            GetCA1309CSharpStringComparerResultAt(14, 14,
                                                "C.Bad()",
                                                "CaseInsensitiveComparer.DefaultInvariant",
                                                "System.Collections.SortedList.SortedList(System.Collections.IComparer)"),
            GetCA1309CSharpDefaultResultAt(15, 14,
                                                 "C.Bad()",
                                                 "new Comparer(CultureInfo.InvariantCulture)",
                                                 "System.Collections.SortedList.SortedList(System.Collections.IComparer)"),
            GetCA1309CSharpDefaultResultAt(16, 14,
                                                "C.Bad()",
                                                "new CaseInsensitiveComparer(CultureInfo.InvariantCulture)",
                                                "System.Collections.SortedList.SortedList(System.Collections.IComparer)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Collections

Module C
    Sub Bad()
        Dim sl As SortedList
        sl = New SortedList(StringComparer.InvariantCulture)
        sl = New SortedList(StringComparer.InvariantCultureIgnoreCase)
        sl = New SortedList(Comparer.DefaultInvariant)
        sl = New SortedList(CaseInsensitiveComparer.DefaultInvariant)
        sl = New SortedList(New Comparer(CultureInfo.InvariantCulture))
        sl = New SortedList(New CaseInsensitiveComparer(CultureInfo.InvariantCulture))
        Console.WriteLine(sl)
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
    End Sub
End Module",

            GetCA1309BasicStringComparerResultAt(9, 14,
                                                 "Public Sub Bad()",
                                                 "StringComparer.InvariantCulture",
                                                 "Public Overloads Sub New(comparer As System.Collections.IComparer)"),
            GetCA1309BasicStringComparerResultAt(10, 14,
                                                "Public Sub Bad()",
                                                "StringComparer.InvariantCultureIgnoreCase",
                                                "Public Overloads Sub New(comparer As System.Collections.IComparer)"),
            GetCA1309BasicStringComparerResultAt(11, 14,
                                                 "Public Sub Bad()",
                                                 "Comparer.DefaultInvariant",
                                                 "Public Overloads Sub New(comparer As System.Collections.IComparer)"),
            GetCA1309BasicStringComparerResultAt(12, 14,
                                                "Public Sub Bad()",
                                                "CaseInsensitiveComparer.DefaultInvariant",
                                                "Public Overloads Sub New(comparer As System.Collections.IComparer)"),
            GetCA1309BasicDefaultResultAt(13, 14,
                                                 "Public Sub Bad()",
                                                 "New Comparer(CultureInfo.InvariantCulture)",
                                                 "Public Overloads Sub New(comparer As System.Collections.IComparer)"),
            GetCA1309BasicDefaultResultAt(14, 14,
                                                "Public Sub Bad()",
                                                "New CaseInsensitiveComparer(CultureInfo.InvariantCulture)",
                                                "Public Overloads Sub New(comparer As System.Collections.IComparer)"));
        }

        [Fact]
        public void CA1309InvariantComparerHashtable()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Collections;

sealed class C
{
    void Bad()
    {
        Hashtable h;
        h = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
        h = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
        Console.WriteLine(h);
   }

    void Good()
    {
        // The following calls are okay as far as CA1309 is concerned
        Hashtable h;
        h = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.Default);
        h = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.CurrentCulture));
        Console.WriteLine(h);
    }
}",

            GetCA1309CSharpStringComparerResultAt(11, 13,
                                                "C.Bad()",
                                                "CaseInsensitiveComparer.DefaultInvariant",
                                                "System.Collections.Hashtable.Hashtable(System.Collections.IHashCodeProvider, System.Collections.IComparer)"),
            GetCA1309CSharpDefaultResultAt(12, 13,
                                                 "C.Bad()",
                                                 "new CaseInsensitiveComparer(CultureInfo.InvariantCulture)",
                                                 "System.Collections.Hashtable.Hashtable(System.Collections.IHashCodeProvider, System.Collections.IComparer)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Collections

Module C
    Sub Bad()
        Dim h As Hashtable
        h = New Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant)
    End Sub

    Sub Good()
        ' The following calls are okay as far as CA1309 is concerned
        Dim h As Hashtable
        h = New Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.Default)
        h = New Hashtable(New CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), New CaseInsensitiveComparer(CultureInfo.CurrentCulture))
        Console.WriteLine(h)
    End Sub
End Module",


            GetCA1309BasicStringComparerResultAt(9, 13,
                                                "Public Sub Bad()",
                                                "CaseInsensitiveComparer.DefaultInvariant",
                                                "Public Overloads Sub New(hcp As System.Collections.IHashCodeProvider, comparer As System.Collections.IComparer)"));
        }

        [Fact]
        public void CA1309InvariantComparerDictionary()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Collections;

sealed class C
{
    void Bad()
    {
        System.Collections.Generic.Dictionary<string, int> d;
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.InvariantCulture);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        Console.WriteLine(d);
   }

    void Good()
    {
        // The following calls are okay as far as CA1309 is concerned
        System.Collections.Generic.Dictionary<string, int> d;
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.CurrentCulture);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
        d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        Console.WriteLine(d);
    }
}",

           GetCA1309CSharpStringComparerResultAt(11, 13,
                                                 "C.Bad()",
                                                 "StringComparer.InvariantCulture",
                                                 "System.Collections.Generic.Dictionary<string, int>.Dictionary(System.Collections.Generic.IEqualityComparer<string>)"),
            GetCA1309CSharpStringComparerResultAt(12, 13,
                                                "C.Bad()",
                                                "StringComparer.InvariantCultureIgnoreCase",
                                                "System.Collections.Generic.Dictionary<string, int>.Dictionary(System.Collections.Generic.IEqualityComparer<string>)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Collections

Module C
    Sub Bad()
        Dim d As System.Collections.Generic.Dictionary(Of String, Integer)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.InvariantCulture)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.InvariantCultureIgnoreCase)
        Console.WriteLine(d)
    End Sub

    Sub Good()
        ' The following calls are okay as far as CA1309 is concerned
        Dim d As System.Collections.Generic.Dictionary(Of String, Integer)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.CurrentCulture)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.CurrentCultureIgnoreCase)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.Ordinal)
        d = New System.Collections.Generic.Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Console.WriteLine(d)
    End Sub
End Module",

            GetCA1309BasicStringComparerResultAt(9, 13,
                                                 "Public Sub Bad()",
                                                 "StringComparer.InvariantCulture",
                                                 "Public Overloads Sub New(comparer As System.Collections.Generic.IEqualityComparer(Of String))"),
            GetCA1309BasicStringComparerResultAt(10, 13,
                                                 "Public Sub Bad()",
                                                 "StringComparer.InvariantCultureIgnoreCase",
                                                 "Public Overloads Sub New(comparer As System.Collections.Generic.IEqualityComparer(Of String))"));
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

        private static DiagnosticResult GetCA1309CSharpDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            //{0} passes {1} as the 'StringComparison' parameter to {2}.
            //To perform a non - linguistic comparison, specify 'StringComparison.Ordinal' or 'StringComparison.OrdinalIgnoreCase' instead.

            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDiagnosis, callee, caller, preferred);
            return GetCSharpResultAt(line, column, CA1309Name, message);

        }

        private static DiagnosticResult GetCA1309CSharpInvariantCultureResultAt(int line, int column, string callee, string caller)
        {
            //{0} uses {1} as the default argument corresponding to the optional 'StringComparison' parameter. 
            //To perform a non-linguistic comparison, specify 'StringComparison.Ordinal' or 'StringComparison.OrdinalIgnoreCase' instead.

            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDefaultDiagnosis, callee, caller);
            return GetCSharpResultAt(line, column, CA1309Name, message);

        }

        private static DiagnosticResult GetCA1309CSharpStringComparerResultAt(int line, int column, string callee, string caller, string preferred)
        {
            //{0}' passes '{1}' as the 'StringComparer' parameter to '{2}'. 
            //To perform a non-linguistic comparison, specify 'StringComparer.Ordinal' or 'StringComparer.OrdinalIgnoreCase' instead.

            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparerDiagnosis, callee, caller, preferred);
            return GetCSharpResultAt(line, column, CA1309Name, message);

        }

        private static DiagnosticResult GetCA1309BasicDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDiagnosis, callee, caller, preferred);
            return GetBasicResultAt(line, column, CA1309Name, message);
        }

        private static DiagnosticResult GetCA1309BasicInvariantCultureResultAt(int line, int column, string callee, string caller)
        {
            //{0} uses {1} as the default argument corresponding to the optional 'StringComparison' parameter. 
            //To perform a non-linguistic comparison, specify 'StringComparison.Ordinal' or 'StringComparison.OrdinalIgnoreCase' instead.

            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDefaultDiagnosis, callee, caller);
            return GetBasicResultAt(line, column, CA1309Name, message);

        }

        private static DiagnosticResult GetCA1309BasicStringComparerResultAt(int line, int column, string callee, string caller, string preferred)
        {
            //{0}' passes '{1}' as the 'StringComparer' parameter to '{2}'. 
            //To perform a non-linguistic comparison, specify 'StringComparer.Ordinal' or 'StringComparer.OrdinalIgnoreCase' instead.

            var message = string.Format(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparerDiagnosis, callee, caller, preferred);
            return GetBasicResultAt(line, column, CA1309Name, message);

        }
    }
}
