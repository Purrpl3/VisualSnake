Imports System.Timers

Module Module1
    Private myTimer As Timer

    Sub initTimer()
        ' Initialize the timer with a 1-second interval (1000 milliseconds)
        myTimer = New Timer(1000)

        ' Hook up the Elapsed event for the timer
        AddHandler myTimer.Elapsed, AddressOf OnTimedEvent

        ' Start the timer
        myTimer.AutoReset = True
        myTimer.Enabled = True

        Console.WriteLine("Press [Enter] to exit the program.")
        Console.ReadLine()
    End Sub

    ' Define the event handler for the timer
    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime)
    End Sub
End Module
