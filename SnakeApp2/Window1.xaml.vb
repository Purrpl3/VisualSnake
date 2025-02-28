Imports System.ComponentModel

Public Class Window1

    Private Sub Window1_Closing(sender As Object, e As CancelEventArgs)
        Dim mainWindow = Application.Current.Windows.OfType(Of MainWindow)().FirstOrDefault()
        mainWindow.WindowState = WindowState.Normal ' Not working currently

        Close()
    End Sub
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ' Show the OpenFileDialog
        'If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
        ' Load the selected image into the PictureBox
        'ImagePreview.Fill = Image.FromFile(OpenFileDialog1.FileName)
        ' Set the SizeMode for the PictureBox
        'PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        'End If
    End Sub
End Class
