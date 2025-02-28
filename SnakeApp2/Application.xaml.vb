Imports System.Windows.Threading

Class Application
    ' Handle the Startup event
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        ' Initialization code here
        MessageBox.Show("Welcome to the Snake Game! Click page to start moving the snake.")
    End Sub

    ' Handle the Exit event
    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.Exit
        ' Cleanup code here
        MessageBox.Show("Thank you for playing the Snake Game!")
    End Sub

    ' Handle unhandled exceptions
    Private Sub Application_DispatcherUnhandledException(sender As Object, e As DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        ' Error handling code here
        MessageBox.Show($"An unhandled exception occurred: {e.Exception.Message}")
        e.Handled = True ' Prevents the application from closing
    End Sub
End Class
