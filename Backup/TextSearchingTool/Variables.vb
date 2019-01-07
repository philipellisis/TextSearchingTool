
Public Class Variables
    Private OutputLineNumber As Boolean
    Private OutputFileName As Boolean
    Private IgnoreCase As Boolean
    Friend DebugMode As Boolean = False
    '1 = Evaluate condition
    '2 = Print lines until end of line
    '3 = continue until second condition met
    Public SearchConditions() As String = {"Starts With", "Ends With", "Contains", "Is", "Is Not", "Does Not Contain", "Does Not Start With", "Does Not End With", "Line Number Greater Than", "Line Number Less Than", "RegEx"}
    Public SearchBoolean() As String = {"OR", "AND"}
    Public SearchReturn() As String = {"Return Current Line", "Return n Lines Below", "Return n Lines Above", "Stop All Output", "Start Output"}
    Public Property OutputLineNumbers() As Boolean
        Get
            Return OutputLineNumber
        End Get
        Set(ByVal value As Boolean)
            OutputLineNumber = value
        End Set
    End Property
    Public Property OutputFileNames() As Boolean
        Get
            Return OutputFileName
        End Get
        Set(ByVal value As Boolean)
            OutputFileName = value
        End Set
    End Property
    Public Property IgnoreCases() As Boolean
        Get
            Return IgnoreCase
        End Get
        Set(ByVal value As Boolean)
            IgnoreCase = value
        End Set
    End Property

    Structure SettingsSearch
        Dim Condition As String
        Dim SearchString As String
        Dim SearchBoolean As String
        Dim SearchReturn As String
        Dim SearchOption As Integer
    End Structure

End Class
