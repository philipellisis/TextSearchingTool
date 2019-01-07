Public Class Main
    Private FileArray(0) As String
    Private Delimeter As String = "~|~"
    Private SearchForm As New Form

    Private Sub AddFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFilesToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                Dim PreviousFileSize As Integer

                If UBound(FileArray) = 0 Then
                    PreviousFileSize = 0
                Else
                    PreviousFileSize = UBound(FileArray)
                End If

                ReDim Preserve FileArray(UBound(OpenFileDialog1.FileNames) + PreviousFileSize + 1)

                For i = PreviousFileSize + 1 To PreviousFileSize + UBound(OpenFileDialog1.FileNames) + 1
                    FileArray(i) = OpenFileDialog1.FileNames(i - PreviousFileSize - 1)
                    ListBox1.Items.Add(Split(OpenFileDialog1.FileNames(i - PreviousFileSize - 1), "\")(UBound(Split(OpenFileDialog1.FileNames(i - PreviousFileSize - 1), "\"))))
                Next
            Catch
                MessageBox.Show("Error while adding file, make sure you have selected a valid file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ClearFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub ConfigureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureToolStripMenuItem.Click
        Dim errorNumber As Integer
        SearchForm = SearchSettings
        SearchForm.ShowDialog()
        errorNumber = ValidateFiles()
        If errorNumber = 0 Then
            If SearchSettings.PerformSearches = True Then
                Dim FindFiles As New FindFiles
                FindFiles.GetSetFiles = FileArray

                Try
                    FindFiles.setVariables() = SearchSettings.GetVariables
                    FindFiles.setVariablesSettingsSearch = SearchSettings.GetSearchSettings
                    FindFiles.Main(Label2)
                    If CheckBox1.Checked = True Then OpenOutputFile()
                Catch
                    MessageBox.Show("Error running search routine, check your criteria and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            If SearchSettings.PerformSearches = True Then
                ReturnError(errorNumber)
            End If
        End If
    End Sub

    Private Sub GoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoToolStripMenuItem.Click
        Dim FindFiles As New FindFiles
        Dim errorNumber As Integer
        errorNumber = ValidateFiles()
        If errorNumber = 0 Then
            FindFiles.GetSetFiles = FileArray
            Try
                FindFiles.setVariables() = SearchSettings.GetVariables
                FindFiles.setVariablesSettingsSearch = SearchSettings.GetSearchSettings
                FindFiles.Main(Label2)
                If CheckBox1.Checked = True Then OpenOutputFile()
            Catch
                MessageBox.Show("Error running search routine, check your criteria and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            ReturnError(errorNumber)
        End If
    End Sub

    Private Sub SetOutputFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetOutputFileToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            FileArray(0) = SaveFileDialog1.FileName
            Label1.Text = "Output File: " & Split(SaveFileDialog1.FileName, "\")(UBound(Split(SaveFileDialog1.FileName, "\")))
        End If
    End Sub

    Function ValidateFiles() As Integer
        If Label1.Text = "Output file not specified" Then Return 1
        If UBound(FileArray) < 1 Then Return 2
        Return 0
    End Function
    Sub ReturnError(ByVal errorNum As Integer)
        Dim errorMessage As String = "Unspecified error"
        If errorNum = 1 Then errorMessage = "Please select an output file"
        If errorNum = 2 Then errorMessage = "Please select one or more files to search"

        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub


    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myString As String = Application.StartupPath
        Dim Files(0) As String
        Dim i As Integer = 0
        If IO.File.Exists(myString & "\Defaults.txt") Then
            Try
                Dim fIn As IO.StreamReader = IO.File.OpenText(myString & "\Defaults.txt")
                myString = fIn.ReadLine
                If myString <> "" Then
                    Label1.Text = "Output File: " & Split(myString, "\")(UBound(Split(myString, "\")))
                    FileArray(0) = myString
                End If


                Do While fIn.Peek <> -1
                    myString = fIn.ReadLine
                    If myString <> "" Then
                        ReDim Preserve Files(i)
                        Files(i) = myString
                        i += 1
                    End If
                Loop
                If i > 0 Then
                    Try
                        Dim PreviousFileSize As Integer

                        If UBound(FileArray) = 0 Then
                            PreviousFileSize = 0
                        Else
                            PreviousFileSize = UBound(FileArray)
                        End If

                        ReDim Preserve FileArray(UBound(Files) + PreviousFileSize + 1)

                        For i = PreviousFileSize + 1 To PreviousFileSize + UBound(Files) + 1
                            FileArray(i) = Files(i - PreviousFileSize - 1)
                            ListBox1.Items.Add(Split(Files(i - PreviousFileSize - 1), "\")(UBound(Split(Files(i - PreviousFileSize - 1), "\"))))
                        Next
                    Catch
                        MessageBox.Show("Error while adding file, make sure you have selected a valid file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
                fIn.Close()
            Catch
            End Try
        End If

    End Sub

    Private Sub OpenOutputFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenOutputFileToolStripMenuItem.Click
        OpenOutputFile()
    End Sub

    Private Sub ClearFilesToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearFilesToolStripMenuItem1.Click
        ListBox1.Items.Clear()
        ReDim FileArray(0)
        Label1.Text = "Output file not specified"
    End Sub

    Private Sub SetFilesAsDefaultToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetFilesAsDefaultToolStripMenuItem1.Click
        Dim myString As String = Application.StartupPath
        Try
            Dim fOutn As IO.StreamWriter = IO.File.CreateText(myString & "\Defaults.txt")
            For i = 0 To UBound(FileArray)
                If FileArray(i) <> Nothing Then myString = FileArray(i) Else myString = ""
                fOutn.WriteLine(myString)
            Next
            fOutn.Close()
        Catch
            MessageBox.Show("Couldn't set the files as default", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim variables As New Variables
        Dim SearchSettingsVar As Variables.SettingsSearch()
        Dim i As Integer = 0
        Dim tempString As String
        If OpenFileDialog2.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                Dim fIn As IO.StreamReader = IO.File.OpenText(OpenFileDialog2.FileName)

                variables.OutputLineNumbers = (fIn.ReadLine)
                variables.OutputFileNames = (fIn.ReadLine)
                variables.IgnoreCases = (fIn.ReadLine)
                Do While fIn.Peek <> -1
                    ReDim Preserve SearchSettingsVar(i)
                    tempString = fIn.ReadLine
                    If Split(tempString, Delimeter)(0) <> "" Then SearchSettingsVar(i).Condition = Split(tempString, Delimeter)(0)
                    If Split(tempString, Delimeter)(1) <> "" Then SearchSettingsVar(i).SearchString = Split(tempString, Delimeter)(1)
                    If Split(tempString, Delimeter)(2) <> "" Then SearchSettingsVar(i).SearchBoolean = Split(tempString, Delimeter)(2)
                    If Split(tempString, Delimeter)(3) <> "" Then SearchSettingsVar(i).SearchReturn = Split(tempString, Delimeter)(3)
                    If Split(tempString, Delimeter)(4) <> "" Then SearchSettingsVar(i).SearchOption = CInt(Split(tempString, Delimeter)(4))
                    i = i + 1
                Loop

                SearchSettings.SetSearchSettings(SearchSettingsVar)
                SearchSettings.SetVariables(variables)
                fIn.Close()
            Catch
                MessageBox.Show("Error opening file, make sure the file is of the right type and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim variables As New Variables
        Dim SearchSettingsVar As Variables.SettingsSearch()
        variables = SearchSettings.GetVariables
        SearchSettingsVar = SearchSettings.GetSearchSettings
        If SaveFileDialog2.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                Dim FOut As IO.StreamWriter = IO.File.CreateText(SaveFileDialog2.FileName)
                FOut.WriteLine(variables.OutputLineNumbers)
                FOut.WriteLine(variables.OutputFileNames)
                FOut.WriteLine(variables.IgnoreCases)
                Dim tempstring As New System.Text.StringBuilder
                For i = 0 To UBound(SearchSettingsVar)
                    If SearchSettingsVar(i).Condition <> Nothing Then tempstring.Append(SearchSettingsVar(i).Condition)
                    tempstring.Append(Delimeter)
                    If SearchSettingsVar(i).SearchString <> Nothing Then tempstring.Append(SearchSettingsVar(i).SearchString)
                    tempstring.Append(Delimeter)
                    If SearchSettingsVar(i).SearchBoolean <> Nothing Then tempstring.Append(SearchSettingsVar(i).SearchBoolean)
                    tempstring.Append(Delimeter)
                    If SearchSettingsVar(i).SearchReturn <> Nothing Then tempstring.Append(SearchSettingsVar(i).SearchReturn)
                    tempstring.Append(Delimeter)
                    If SearchSettingsVar(i).SearchOption <> Nothing Then tempstring.Append(SearchSettingsVar(i).SearchOption)
                    FOut.WriteLine(tempstring.ToString)
                    tempstring.Length = 0
                Next
                FOut.Close()
            Catch
                MessageBox.Show("Unable to save file, Make sure that you have first defined a search", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Sub OpenOutputFile()
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(FileArray(0))
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch
            MessageBox.Show("Error opening the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ClearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        ListBox1.Items.Clear()
        ReDim Preserve FileArray(0)
    End Sub
End Class
