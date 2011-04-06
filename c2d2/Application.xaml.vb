Imports System.Windows.Shell
Class Application

    Public Sub New()

        Dim jl As New JumpList
        JumpList.SetJumpList(Application.Current, jl)

        Dim SaveAs As New JumpTask
        SaveAs.ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly.Location()
        SaveAs.Title = "Save as..."
        SaveAs.Arguments = "-saveas"

        jl.JumpItems.Add(SaveAs)

        Dim Configuration As New JumpTask
        Configuration.ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly.Location()
        Configuration.Title = "Configuration"
        Configuration.CustomCategory = "Settings"
        Configuration.Arguments = "-config"

        jl.JumpItems.Add(Configuration)
        jl.Apply()

        If My.Settings.defaultDestination = "" Or Not IO.Directory.Exists(My.Settings.defaultDestination) Then
            My.Settings.defaultDestination = My.Computer.FileSystem.SpecialDirectories.Desktop
            My.Settings.Save()
        End If

    End Sub

    Enum cbType
        text = 1
        image = 2
    End Enum

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup

        Dim cbt As New cbType

        If e.Args.Contains("-config") Then
            Dim wind As New configurationWindow
            wind.ShowDialog()
            End
        End If

        If (Clipboard.ContainsText) Then
            cbt = cbType.text
        ElseIf (Clipboard.ContainsImage) Then
            cbt = cbType.image
        Else
            MsgBox("Clipboard Contents Not Supported")
            End
        End If

        Dim filepath As String = ""
        If e.Args.Contains("-saveas") Then
            Dim dlg As New Microsoft.Win32.SaveFileDialog
            dlg.AddExtension = True

            If dlg.ShowDialog Then
                filepath = dlg.FileName
            Else
                End
            End If
        Else
            filepath = smallest_not_on_desktop(cbt)
        End If

        If filepath.Length > 6 And IO.Path.GetFileName(filepath).Length > 3 And IO.Directory.Exists(IO.Path.GetDirectoryName(filepath)) Then

            If IO.File.Exists(filepath) Then
                MsgBox("Are you sure you want to overwrite " & IO.Path.GetFileName(filepath), MsgBoxStyle.OkCancel, "Warning")
            End If

            Select Case cbt
                Case cbType.text

                    My.Computer.FileSystem.WriteAllText(filepath, Clipboard.GetText, False, System.Text.Encoding.UTF8)

                Case cbType.image

                    Dim img As Imaging.BitmapSource = Clipboard.GetImage()
                    Dim fileStream As New IO.FileStream(filepath, IO.FileMode.Create)
                    Dim encoder As New PngBitmapEncoder
                    encoder.Frames.Add(BitmapFrame.Create(img))
                    encoder.Save(fileStream)

                Case Else
                    MsgBox("Nuh uh uh!")
                    End
            End Select
        Else
            MsgBox("There was an issue with the path")
            End
        End If
        'kill the program
        End
    End Sub

    Private Function smallest_not_on_desktop(ByVal type As cbType) As String
        Dim j As Integer = 0
        Dim ext As String = If(type = cbType.image, ".png", ".txt")
        While IO.File.Exists(My.Settings.defaultDestination & "\" & j & ext)
            j += 1
        End While
        Return My.Settings.defaultDestination & "\" & j & ext
    End Function

End Class
