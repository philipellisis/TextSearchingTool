<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SearchSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SearchSettings))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.CBCase = New System.Windows.Forms.CheckBox
        Me.CBLineNum = New System.Windows.Forms.CheckBox
        Me.CBFileName = New System.Windows.Forms.CheckBox
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(768, 258)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.CBCase)
        Me.TabPage1.Controls.Add(Me.CBLineNum)
        Me.TabPage1.Controls.Add(Me.CBFileName)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(760, 232)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Search Output"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'CBCase
        '
        Me.CBCase.AutoSize = True
        Me.CBCase.Checked = True
        Me.CBCase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBCase.Location = New System.Drawing.Point(6, 204)
        Me.CBCase.Name = "CBCase"
        Me.CBCase.Size = New System.Drawing.Size(83, 17)
        Me.CBCase.TabIndex = 3
        Me.CBCase.Text = "Ignore Case"
        Me.CBCase.UseVisualStyleBackColor = True
        '
        'CBLineNum
        '
        Me.CBLineNum.AutoSize = True
        Me.CBLineNum.Location = New System.Drawing.Point(6, 181)
        Me.CBLineNum.Name = "CBLineNum"
        Me.CBLineNum.Size = New System.Drawing.Size(171, 17)
        Me.CBLineNum.TabIndex = 2
        Me.CBLineNum.Text = "Append Line Number to output"
        Me.CBLineNum.UseVisualStyleBackColor = True
        '
        'CBFileName
        '
        Me.CBFileName.AutoSize = True
        Me.CBFileName.Location = New System.Drawing.Point(6, 158)
        Me.CBFileName.Name = "CBFileName"
        Me.CBFileName.Size = New System.Drawing.Size(153, 17)
        Me.CBFileName.TabIndex = 1
        Me.CBFileName.Text = "Append Filename to output"
        Me.CBFileName.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(6, 18)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(748, 134)
        Me.DataGridView1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(672, 288)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(108, 26)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Perform Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(558, 288)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(108, 26)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Return to Form"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'SearchSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 326)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SearchSettings"
        Me.Text = "Search Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CBLineNum As System.Windows.Forms.CheckBox
    Friend WithEvents CBFileName As System.Windows.Forms.CheckBox
    Friend WithEvents CBCase As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
