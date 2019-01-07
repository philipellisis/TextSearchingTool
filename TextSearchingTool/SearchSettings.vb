
Public Class SearchSettings
    Private variables As New Variables
    Private SearchSettingsVar() As Variables.SettingsSearch
    Private PerformSearch As Boolean
    Dim outDG As DataGridView


    Private Sub SearchSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        If outDG Is Nothing Then
            outDG = DataGridView1
            Dim DGCondition As New DataGridViewComboBoxColumn
            DGCondition.HeaderText = "Condition"
            DGCondition.Name = "Condition"
            DGCondition.Width = 150
            For i = 0 To UBound(variables.SearchConditions)
                DGCondition.Items.Add(variables.SearchConditions(i))
            Next
            outDG.Columns.Add(DGCondition)

            outDG.Columns.Add("String", "String")
            outDG.Columns(1).Width = 200
            Dim DGBoolean As New DataGridViewComboBoxColumn
            DGBoolean.HeaderText = "Boolean"
            DGBoolean.Name = "Boolean"
            DGBoolean.Width = 60
            For i = 0 To UBound(variables.SearchBoolean)
                DGBoolean.Items.Add(variables.SearchBoolean(i))
            Next
            outDG.Columns.Add(DGBoolean)

            Dim DGReturn As New DataGridViewComboBoxColumn
            DGReturn.HeaderText = "Return"
            DGReturn.Name = "Return"
            DGReturn.Width = 135
            For i = 0 To UBound(variables.SearchReturn)
                DGReturn.Items.Add(variables.SearchReturn(i))
            Next
            outDG.Columns.Add(DGReturn)

            outDG.Columns.Add("SearchOption", "Search Option")
            'outDG.Columns.Add("LinesBelow", "Lines Below")
        End If

        outDG.RowCount = 1
        Dim tempSearchOption As String = ""
        If SearchSettingsVar Is Nothing Then
        Else
            For i = 0 To UBound(SearchSettingsVar)
                If SearchSettingsVar(i).Condition Is Nothing Then
                Else
                    If SearchSettingsVar(i).SearchOption = 0 Then tempSearchOption = Nothing Else tempSearchOption = CStr(SearchSettingsVar(i).SearchOption)
                    outDG.Rows.Add(SearchSettingsVar(i).Condition, SearchSettingsVar(i).SearchString, SearchSettingsVar(i).SearchBoolean, SearchSettingsVar(i).SearchReturn, tempSearchOption)
                    If i > 0 Then
                        If outDG.Item(2, i - 1).Value = "AND" Then
                            UpdateDG(i, "AND")
                        ElseIf outDG.Item(2, i - 1).Value = "OR" Then
                            UpdateDG(i, "OR")
                        Else
                            UpdateDG(i)
                        End If
                    End If
                End If
            Next
            CBCase.Checked = variables.IgnoreCases
            CBFileName.Checked = variables.OutputFileNames
            CBLineNum.Checked = variables.OutputLineNumbers
        End If


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim Create(2) As Variables.SettingsCreate
        Dim errorNum As Integer = CheckVariables()
        If errorNum > 0 Then
            ReturnError(errorNum)
        Else
            initializeVariables()
            PerformSearch = True
            Me.Hide()
        End If

    End Sub
    Public Function GetSearchSettings()
        Return SearchSettingsVar
    End Function

    Public Function GetVariables()
        Return variables
    End Function
    Public Sub SetSearchSettings(ByVal sentSearchSettingsVar() As Variables.SettingsSearch)
        SearchSettingsVar = sentSearchSettingsVar
    End Sub

    Public Sub SetVariables(ByVal sentVariables As Variables)
        variables = sentVariables
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim errorNum As Integer = CheckVariables()
        If errorNum > 0 Then
            ReturnError(errorNum)
        Else
            initializeVariables()
            PerformSearch = False
            Me.Hide()
        End If

    End Sub
    Public Property PerformSearches() As Boolean
        Get
            Return PerformSearch
        End Get
        Set(ByVal value As Boolean)
            PerformSearch = value
        End Set
    End Property
    Sub initializeVariables()
        ReDim SearchSettingsVar(outDG.RowCount - 1)
        For i = 0 To outDG.RowCount - 1
            SearchSettingsVar(i).Condition = outDG.Item(0, i).Value
            SearchSettingsVar(i).SearchString = outDG.Item(1, i).Value
            If CBCase.Checked = True And outDG.Item(1, i).Value <> Nothing Then SearchSettingsVar(i).SearchString = SearchSettingsVar(i).SearchString.ToUpper
            SearchSettingsVar(i).SearchBoolean = outDG.Item(2, i).Value
            SearchSettingsVar(i).SearchReturn = outDG.Item(3, i).Value
            SearchSettingsVar(i).SearchOption = CInt(outDG.Item(4, i).Value)
            'SearchSettingsVar(i).LinesBelow = CInt(outDG.Item(5, i).Value)
        Next

        variables.OutputFileNames = CBFileName.Checked
        variables.OutputLineNumbers = CBLineNum.Checked
        variables.IgnoreCases = CBCase.Checked
    End Sub
    Function CheckVariables() As Integer

        If outDG.RowCount = 0 Then Return 1
        Dim mystring As String = "hello"
        For i = 0 To outDG.RowCount - 1
            'first line in search must have a criteria to search by
            If i = 0 And outDG.Item(0, i).Value = Nothing Then Return 2
            'if you are saying you want to return a number of lines above or below, you must supply how many
            If outDG.Item(3, i).Value = "Return n Lines Below" Or outDG.Item(3, i).Value = "Return n Lines Above" Then
                If IsNumeric(outDG.Item(4, i).Value) Then
                Else
                    Return 3
                End If
            End If

            'you have to have criteria if you are going to search on something
            If outDG.Item(0, i).Value <> Nothing And outDG.Item(1, i).Value = Nothing Then Return 4

        Next
        Return 0
    End Function
    Sub ReturnError(ByVal errorNum As Integer)
        Dim errorMessage As String = "Unspecified error"

        If errorNum = 1 Then errorMessage = "Please supply some criteria to search by"
        If errorNum = 2 Then errorMessage = "You must have some search criteria for the first line"
        If errorNum = 3 Then errorMessage = "Please provide the number of lines you would like to search above or below for the given criteria"
        If errorNum = 4 Then errorMessage = "No criteria found for one of your search lines"
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub


    Private Sub DataGridView1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If outDG.Item(2, DataGridView1.CurrentRow.Index).Value = "AND" Then
            UpdateDG(DataGridView1.CurrentRow.Index + 1, "AND")
        ElseIf outDG.Item(2, DataGridView1.CurrentRow.Index).Value = "OR" Then
            UpdateDG(DataGridView1.CurrentRow.Index + 1, "OR")
        Else
            UpdateDG(DataGridView1.CurrentRow.Index + 1)
        End If
    End Sub
    Sub UpdateDG(ByVal rowNum As Integer, Optional ByVal Input As String = "NONE")
        If Input = "AND" Then
            outDG.Item(0, rowNum).ReadOnly = False
            outDG.Item(0, rowNum).Style.BackColor = Color.White
            outDG.Item(1, rowNum).ReadOnly = False
            outDG.Item(1, rowNum).Style.BackColor = Color.White
            outDG.Item(2, rowNum).ReadOnly = False
            outDG.Item(2, rowNum).Style.BackColor = Color.White
            outDG.Item(3, rowNum).ReadOnly = True
            outDG.Item(3, rowNum).Style.BackColor = Color.LightGray
            outDG.Item(4, rowNum).ReadOnly = True
            outDG.Item(4, rowNum).Style.BackColor = Color.LightGray
        ElseIf Input = "OR" Then
            outDG.Item(0, rowNum).ReadOnly = False
            outDG.Item(0, rowNum).Style.BackColor = Color.White
            outDG.Item(1, rowNum).ReadOnly = False
            outDG.Item(1, rowNum).Style.BackColor = Color.White
            outDG.Item(2, rowNum).ReadOnly = False
            outDG.Item(2, rowNum).Style.BackColor = Color.White
            outDG.Item(3, rowNum).ReadOnly = False
            outDG.Item(3, rowNum).Style.BackColor = Color.White
            outDG.Item(4, rowNum).ReadOnly = False
            outDG.Item(4, rowNum).Style.BackColor = Color.White
        Else
            outDG.Item(0, rowNum).ReadOnly = True
            outDG.Item(0, rowNum).Style.BackColor = Color.LightGray
            outDG.Item(1, rowNum).ReadOnly = True
            outDG.Item(1, rowNum).Style.BackColor = Color.LightGray
            outDG.Item(2, rowNum).ReadOnly = True
            outDG.Item(2, rowNum).Style.BackColor = Color.LightGray
            outDG.Item(3, rowNum).ReadOnly = True
            outDG.Item(3, rowNum).Style.BackColor = Color.LightGray
            outDG.Item(4, rowNum).ReadOnly = True
            outDG.Item(4, rowNum).Style.BackColor = Color.LightGray
        End If

    End Sub

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        If outDG.RowCount > 1 Then
            If outDG.Item(2, outDG.RowCount - 2).Value = "AND" Then
                UpdateDG(outDG.RowCount - 1, "AND")
            ElseIf outDG.Item(2, outDG.RowCount - 2).Value = "OR" Then
                UpdateDG(outDG.RowCount - 1, "OR")
            Else
                UpdateDG(outDG.RowCount - 1)
            End If
        End If
    End Sub
End Class