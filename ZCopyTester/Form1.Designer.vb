<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.btnTestDirectoryExists = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTestResult = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(162, 49)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.Size = New System.Drawing.Size(229, 20)
        Me.txtSource.TabIndex = 0
        '
        'btnTestDirectoryExists
        '
        Me.btnTestDirectoryExists.Location = New System.Drawing.Point(461, 49)
        Me.btnTestDirectoryExists.Name = "btnTestDirectoryExists"
        Me.btnTestDirectoryExists.Size = New System.Drawing.Size(209, 23)
        Me.btnTestDirectoryExists.TabIndex = 1
        Me.btnTestDirectoryExists.Text = "Test directory exists"
        Me.btnTestDirectoryExists.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(63, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Source:"
        '
        'txtTestResult
        '
        Me.txtTestResult.Location = New System.Drawing.Point(164, 212)
        Me.txtTestResult.Name = "txtTestResult"
        Me.txtTestResult.Size = New System.Drawing.Size(506, 20)
        Me.txtTestResult.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(91, 215)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Test result:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTestResult)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnTestDirectoryExists)
        Me.Controls.Add(Me.txtSource)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSource As TextBox
    Friend WithEvents btnTestDirectoryExists As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtTestResult As TextBox
    Friend WithEvents Label2 As Label
End Class
