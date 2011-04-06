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

        jl.Apply()

    End Sub

    Enum cbType
        text = 1
        image = 2
    End Enum

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup
        Dim cbt As New cbType

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
                MsgBox(filepath)
            Else
                End
            End If
        Else
            filepath = smallest_not_on_desktop(cbt)
        End If

        If filepath.Length > 6 And IO.Path.GetFileName(filepath).Length > 3 And IO.Directory.Exists(IO.Path.GetDirectoryName(filepath)) Then
            Select Case cbt
                Case cbType.text

                    My.Computer.FileSystem.WriteAllText(filepath, Clipboard.GetText, False, System.Text.Encoding.UTF8)

                Case cbType.image

                    Dim img As Imaging.BitmapSource = Clipboard.GetImage()
                    Dim fileStream As New IO.FileStream("c:\0.png", IO.FileMode.Create)
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
        While IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & j & ext)
            j += 1
        End While
        Return My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & j & ext
    End Function

End Class
