Imports System.Text.RegularExpressions
Public Class FindFiles
    Private Files As String()
    Private variables As Variables
    Private NumberSearches() As Integer
    Private CurrentBuffer As Integer
    Private BufferSize As Integer
    Private SkipLines As Boolean = False
    Private LineNumber As Long = 0
    Private SearchSettingsVar() As Variables.SettingsSearch

    Structure Buffer
        Dim OutputText As String
        Dim OutputLine As Boolean
        Dim OutputLineNum As Long
        Dim OutputFileName As String
    End Structure
    Public WriteOnly Property setVariables()
        Set(ByVal value)
            variables = value
        End Set
    End Property
    Public WriteOnly Property setVariablesSettingsSearch()
        Set(ByVal value)
            SearchSettingsVar = value
        End Set
    End Property

    Sub Main(ByVal label As Label)
        label.Text = ""
        Dim OutputLine As Boolean = False
        Dim FileInfo As IO.FileInfo
        Dim CurrentFileName As String = ""
        Dim OutputString As String
        Dim FOut As IO.StreamWriter
        Dim FIn As IO.StreamReader
        Dim Buffer(GetBufferSize(SearchSettingsVar)) As Buffer
        Dim NumbertoOr() As Integer
        Dim BufferSearchString As String
        Dim FileLength As Long
        Dim LineSize As Long
        Dim numberFiles As Integer = UBound(Files)


        If variables.DebugMode = True Then
            FOut = IO.File.CreateText("C:\MYTESTFILE.TXT")
            ReDim Files(1)
            Files(0) = "C:\MYTESTFILE.TXT"
            Files(1) = "C:\TEST.TXT"
        Else
            FOut = IO.File.CreateText(Files(0))
        End If




        NumberSearches = CheckBoolean(SearchSettingsVar)
        ReDim NumbertoOr(UBound(NumberSearches))
        For i = 0 To UBound(NumberSearches)
            If i <> 0 Then
                NumbertoOr(i) = NumberSearches(i - 1) + NumbertoOr(i - 1) + 1
            End If
        Next

        For i = 1 To UBound(Files)
            CurrentFileName = Split(Files(i), "\")(UBound(Split(Files(i), "\")))
            LineNumber = 1
            If variables.DebugMode = True Then
                FIn = IO.File.OpenText("C:\TEST.TXT")
            Else
                FIn = IO.File.OpenText(Files(i))
                FileInfo = New IO.FileInfo(Files(i))
                FileLength = FileInfo.Length
            End If
            LineSize = 0
            Do While (FIn.Peek <> -1)
                Buffer(CurrentBuffer).OutputText = FIn.ReadLine
                LineSize += Len(Buffer(CurrentBuffer).OutputText)
                If LineNumber Mod 50000 = 0 Then
                    label.Text = Math.Round(LineSize / FileLength * 100, 0) & " % for file " & i & " of " & numberFiles
                    label.Refresh()
                    Application.DoEvents()
                End If
                Buffer(CurrentBuffer).OutputFileName = CurrentFileName
                Buffer(CurrentBuffer).OutputLineNum = LineNumber
                If variables.IgnoreCases = True Then BufferSearchString = Buffer(CurrentBuffer).OutputText.ToUpper Else BufferSearchString = Buffer(CurrentBuffer).OutputText

                For j = 0 To UBound(NumberSearches) 'how many "or's" do we have to deal with
                    'If Buffer(CurrentBuffer).OutputLine = False Then
                    For k = 0 To NumberSearches(j) 'How many "AND's" in a row?
                        If k = 0 Or OutputLine = True Then ' so if one of the criteria is missed then nothing else is searched on for AND clause
                            Select Case SearchSettingsVar(NumbertoOr(j) + k).Condition
                                Case Is = variables.SearchConditions(0) 'starts with
                                    OutputLine = CheckStartsWith(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString)
                                Case Is = variables.SearchConditions(1) 'ends with
                                    OutputLine = CheckEndsWith(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString)
                                Case Is = variables.SearchConditions(2) 'contains
                                    OutputLine = CheckContains(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString)
                                Case Is = variables.SearchConditions(3) 'is
                                    OutputLine = CheckIs(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString)
                                Case Is = variables.SearchConditions(4) 'is not
                                    If CheckIs(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString) = False Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(5) 'does not contain
                                    If CheckContains(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString) = False Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(6) 'does not start with
                                    If CheckStartsWith(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString) = False Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(7) 'does not end with
                                    If CheckEndsWith(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString) = False Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(8) 'line number greather than
                                    If CInt(SearchSettingsVar(NumbertoOr(j) + k).SearchString) > LineNumber Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(9) 'line number less than
                                    If CInt(SearchSettingsVar(NumbertoOr(j) + k).SearchString) < LineNumber Then OutputLine = True Else OutputLine = False
                                Case Is = variables.SearchConditions(10) 'Regex
                                    OutputLine = CheckRegex(SearchSettingsVar(NumbertoOr(j) + k).SearchString, BufferSearchString)
                            End Select
                        End If
                    Next
                    If OutputLine = True And SkipLines = False Then Buffer(CurrentBuffer).OutputLine = True
                    If OutputLine = True Then
                        If SkipLines = False Then
                            If SearchSettingsVar(NumbertoOr(j)).SearchReturn = variables.SearchReturn(1) Then
                                SetBufferOutput(0, SearchSettingsVar(NumbertoOr(j)).SearchOption, Buffer, FOut)
                            End If
                            If SearchSettingsVar(NumbertoOr(j)).SearchReturn = variables.SearchReturn(2) Then
                                SetBufferOutput(SearchSettingsVar(NumbertoOr(j)).SearchOption, 0, Buffer, FOut)
                            End If
                        End If
                        If SearchSettingsVar(NumbertoOr(j)).SearchReturn = variables.SearchReturn(3) Then
                            SkipLines = True
                            Buffer(CurrentBuffer).OutputLine = False
                        End If

                        If SearchSettingsVar(NumbertoOr(j)).SearchReturn = variables.SearchReturn(4) Then SkipLines = False
                    End If
                    'End If
                    OutputLine = False ' added to reset output line to false after all conditions have been satisfied
                Next
                IncrementBuffer()
                If Buffer(CurrentBuffer).OutputLine = True And Buffer(CurrentBuffer).OutputText <> Nothing And SkipLines = False Then
                    If variables.OutputFileNames = False And variables.OutputLineNumbers = False Then
                        FOut.WriteLine(Buffer(CurrentBuffer).OutputText)
                    Else
                        OutputString = ""
                        If variables.OutputFileNames = True Then OutputString = Buffer(CurrentBuffer).OutputFileName & Chr(9)
                        If variables.OutputLineNumbers = True Then OutputString = OutputString & Buffer(CurrentBuffer).OutputLineNum & Chr(9)
                        FOut.WriteLine(OutputString & Buffer(CurrentBuffer).OutputText)
                    End If
                    Buffer(CurrentBuffer).OutputLine = False
                End If
                LineNumber += 1
            Loop
            FIn.Close()
        Next
        'clean out the buffer
        For i = 0 To BufferSize
            If Buffer(CurrentBuffer).OutputLine = True And Buffer(CurrentBuffer).OutputText <> Nothing And SkipLines = False Then FOut.WriteLine(Buffer(CurrentBuffer).OutputText)
            IncrementBuffer()
        Next


        FOut.Close()
        label.Text = "Done"
    End Sub

    Public Property GetSetFiles() As String()
        Get
            Return Files
        End Get
        Set(ByVal value As String())
            Files = value
        End Set
    End Property
    Function CheckBoolean(ByVal SearchSettingsVar() As Variables.SettingsSearch) As Integer()
        Dim TempBoolean(0) As Integer
        Dim k As Integer = 0
        Dim l As Integer = 0
        For i = 0 To UBound(SearchSettingsVar)
            If SearchSettingsVar(i).SearchBoolean = "OR" Then
                TempBoolean(l) = k
                l += 1
                ReDim Preserve TempBoolean(l)
                k = 0
            ElseIf SearchSettingsVar(i).SearchBoolean = Nothing Then
                TempBoolean(l) = k
                k = 0
                Exit For
            Else
                k += 1
            End If
        Next
        Return TempBoolean

    End Function
    Function GetBufferSize(ByVal SearchSettingsVar() As Variables.SettingsSearch) As Integer
        Dim UpperMax As Integer = 0
        Dim LowerMax As Integer = 0
        For i = 0 To UBound(SearchSettingsVar)
            If SearchSettingsVar(i).SearchReturn = variables.SearchReturn(1) Then 'lines below
                'If SearchSettingsVar(i).SearchOption > LowerMax Then LowerMax = SearchSettingsVar(i).SearchOption
                LowerMax += SearchSettingsVar(i).SearchOption
            End If
            If SearchSettingsVar(i).SearchReturn = variables.SearchReturn(2) Then 'lines above
                'If SearchSettingsVar(i).SearchOption > UpperMax Then UpperMax = SearchSettingsVar(i).SearchOption
                UpperMax += SearchSettingsVar(i).SearchOption
            End If
            'If SearchSettingsVar(i).SearchReturn = variables.SearchReturn(3) Then
            ' If SearchSettingsVar(i).LinesBelow > LowerMax Then LowerMax = SearchSettingsVar(i).LinesBelow
            ' If SearchSettingsVar(i).LinesAbove > UpperMax Then UpperMax = SearchSettingsVar(i).LinesAbove
            ' End If
        Next
        BufferSize = UpperMax + LowerMax
        If UpperMax > 0 And LowerMax > 0 Then BufferSize += 1
        Return BufferSize
    End Function
    Sub IncrementBuffer()
        If CurrentBuffer < BufferSize Then
            CurrentBuffer += 1
        Else
            CurrentBuffer = 0
        End If
    End Sub
    Sub SetBufferOutput(ByVal LinesAbove As Integer, ByVal LinesBelow As Integer, ByRef Buffer() As Buffer, ByVal fOut As IO.StreamWriter)
        Dim loopNum As Integer
        Dim loopEnd As Integer
        For j = 0 To 1
            If j = 0 Then
                loopNum = CurrentBuffer
                loopEnd = BufferSize
            Else
                loopNum = 0
                loopEnd = CurrentBuffer - 1
            End If

            For i = loopNum To loopEnd
                If i <> CurrentBuffer Then
                    If CurrentBuffer - LinesAbove >= 0 And i >= CurrentBuffer - LinesAbove And i <= CurrentBuffer And i >= CurrentBuffer - LinesAbove Then
                        Buffer(i).OutputLine = True
                    ElseIf CurrentBuffer - LinesAbove < 0 And (i > CurrentBuffer - LinesAbove + BufferSize Or i < CurrentBuffer) Then
                        Buffer(i).OutputLine = True
                    ElseIf CurrentBuffer + LinesBelow <= BufferSize And i >= CurrentBuffer And i <= CurrentBuffer + LinesBelow Then
                        If Buffer(i).OutputLine = True And Buffer(i).OutputText <> Nothing Then
                            fOut.WriteLine(Buffer(i).OutputText)
                            Buffer(i).OutputLine = False
                        End If
                        If Buffer(i).OutputLine = False Then Buffer(i).OutputText = Nothing
                        Buffer(i).OutputLine = True
                    ElseIf CurrentBuffer + LinesBelow > BufferSize And (i <= CurrentBuffer + LinesBelow - BufferSize - 1 Or i > CurrentBuffer) Then
                        If Buffer(i).OutputLine = True And Buffer(i).OutputText <> Nothing Then
                            fOut.WriteLine(Buffer(i).OutputText)
                            Buffer(i).OutputLine = False
                        End If
                        If Buffer(i).OutputLine = False Then Buffer(i).OutputText = Nothing
                        Buffer(i).OutputLine = True
                    End If
                End If
            Next
        Next
    End Sub

    Function CheckStartsWith(ByVal StringToCheck As String, ByVal Buffer As String) As Boolean
        CheckStartsWith = False
        If Buffer.StartsWith(StringToCheck) Then Return True
    End Function
    Function CheckContains(ByVal StringToCheck As String, ByVal Buffer As String) As Boolean
        CheckContains = False
        If Buffer.Contains(StringToCheck) Then
            Return True
        End If

    End Function
    Function CheckEndsWith(ByVal StringToCheck As String, ByVal Buffer As String) As Boolean
        CheckEndsWith = False
        If Buffer.EndsWith(StringToCheck) Then Return True
    End Function
    Function CheckIs(ByVal StringToCheck As String, ByVal Buffer As String) As Boolean
        CheckIs = False
        If Buffer = StringToCheck Then Return True
    End Function
    Function CheckRegex(ByVal StringToCheck As String, ByVal Buffer As String) As Boolean
        CheckRegex = False
        If Regex.IsMatch(Buffer, StringToCheck) Then Return True
    End Function


End Class
