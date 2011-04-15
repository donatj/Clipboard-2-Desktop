Public Class ConfigurationWindow

    Private Sub ConfigurationWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        defaultSaveLocationTxt.Text = My.Settings.defaultDestination
        defaultPrefixTxt.Text = My.Settings.defaultPrefix
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FolderBrowserDialog1.SelectedPath = defaultSaveLocationTxt.Text
        If FolderBrowserDialog1.ShowDialog() Then
            defaultSaveLocationTxt.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub CancelBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        Me.Close()
    End Sub

    Private Sub SaveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        If IO.Directory.Exists(defaultSaveLocationTxt.Text) Then
            My.Settings.defaultDestination = defaultSaveLocationTxt.Text
            My.Settings.Save()
            Me.Close()
        Else
            MsgBox("Path not Found")
        End If
    End Sub
End Class