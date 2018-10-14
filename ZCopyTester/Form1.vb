Imports GenericClassLibrary.FileSystem

Public Class Form1
    Private Sub BtnTestDirectoryExists_Click(sender As Object, e As EventArgs) Handles btnTestDirectoryExists.Click
        txtTestResult.Text = ""
        Dim existsFolder As Boolean = New DirectoryActions().Exists(txtSource.Text)
        txtTestResult.Text = existsFolder.ToString

    End Sub
End Class
