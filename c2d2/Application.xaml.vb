Imports System.Windows.Shell
Class Application

    Public Sub New()

        Dim jl As New JumpList
        JumpList.SetJumpList(Application.Current, jl)

        Dim SaveToDesktop As New JumpTask
        SaveToDesktop.ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly.Location()
        SaveToDesktop.Title = "Save to Desktop"
        jl.JumpItems.Add(SaveToDesktop)

        Dim SaveAs As New JumpTask
        SaveAs.ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly.Location()
        SaveAs.Title = "Save as..."
        jl.JumpItems.Add(SaveAs)

        jl.Apply()

    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup
        If (Clipboard.ContainsText) Then
            MsgBox("text")
        ElseIf (Clipboard.ContainsImage) Then

            Dim img = Clipboard.GetImage()
            Dim fileStream = New IO.FileStream("c:\0.png", IO.FileMode.Create)
            Dim encoder = New PngBitmapEncoder
            encoder.Frames.Add(BitmapFrame.Create(img))
            encoder.Save(fileStream)

        End If
        End
    End Sub
End Class
