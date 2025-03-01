Option Explicit On
Imports System.Windows.Threading

Class MainWindow
    Private moveDirection As String = "Right"
    Private WithEvents gameTimer As New DispatcherTimer()
    Private windowHeight As Integer = 410
    Private windowWidth As Integer = 785
    Private snakeBody As New List(Of Rectangle) ' List to store snake body segments
    'Private MoveVar As Integer = 2 ' Smaller movement increment for smoother movement
    Private pendingDirection As String = moveDirection ' Store the new direction when the restriction applies
    Private moveStored As Byte = 4
    Private bypassClose As Boolean = False ' Flag to bypass the closing dialogue
    Private bypassTutorial As Boolean = False ' Flag to bypass the tutorial
    ' MainWindow Code
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        AddHandler Me.Closing, AddressOf MainWindow_Closing
        If Not bypassTutorial Then
            bypassClose = True
            bypassTutorial = True

            Dim result As MessageBoxResult = MessageBox.Show("Do you wish to open tutorial page?", "Tutorial Dialogue", MessageBoxButton.YesNo, MessageBoxImage.Question)
            If result = MessageBoxResult.Yes Then
                Dim newForm As New Window1()
                newForm.Show()

                WindowState = WindowState.Minimized
            End If
        End If
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If bypassClose Then
            bypassClose = False
        Else
            Dim result As MessageBoxResult = MessageBox.Show("Are you sure you want to quit?", "Confirm Quit", MessageBoxButton.OKCancel, MessageBoxImage.Question)

            If result = MessageBoxResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    ' Startup
    Sub Start() Handles Me.MouseLeftButtonDown
        Dim newLeft As Integer = Math.Round(CInt(Rnd() * (windowWidth - Fruit.Width)) / 20) * 20
        Dim newTop As Integer = Math.Round(CInt(Rnd() * (windowHeight - Fruit.Height)) / 20) * 20
        Fruit.Margin = New Thickness(newLeft, newTop, 0, 0)

        STT.Text = newLeft
        GTT.Text = newTop

        ' Initialize and start the game timer with a higher frequency
        gameTimer.Interval = TimeSpan.FromMilliseconds(200) ' Used to set interval to 0.001 seconds
        gameTimer.Start()

        ' Initialize the snake with one body segment (the head)
        snakeBody.Clear()
        snakeBody.Add(SnakeHead)
    End Sub
    Private Sub GameOver()
        gameTimer.Stop()
        MessageBox.Show("Game Over! Your score is: " & Score.Text, "Game Over")
        snakeCanvas.Children.Clear()
        snakeBody.Clear()
        gameTimer.Stop()

        Score.Text = 0
        SnakeHead.Margin = New Thickness(windowWidth / 2, windowHeight / 2 - 20, windowHeight / 2, windowHeight / 2 - 20)
        Start()
    End Sub

    Private Sub gameTimer_Tick(sender As Object, e As EventArgs) Handles gameTimer.Tick
        MoveRestrict() ' Check if the snake can change direction
        MoveSnake()
        CheckCollision()
        CheckSelfCollision()
        Boundary()
    End Sub

    ' Movement
    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.Key
            Case Key.Left
                pendingDirection = "Left"
            Case Key.Right
                pendingDirection = "Right"
            Case Key.Up
                pendingDirection = "Up"
            Case Key.Down
                pendingDirection = "Down"
        End Select
        e.Handled = True ' Prevent the event from bubbling up
    End Sub

    Private Sub MoveSnake()
        ' Move the rest of the body segments
        For i As Integer = snakeBody.Count - 1 To 1 Step -1
            snakeBody(i).Margin = snakeBody(i - 1).Margin
        Next
        Dim MoveVar As Integer = 20
        ' Move the snake head
        Dim currentMargin As Thickness = SnakeHead.Margin
        Select Case moveDirection
            Case "Left"
                If Not moveStored = 1 Then
                    SnakeHead.Margin = New Thickness(currentMargin.Left - MoveVar, currentMargin.Top, currentMargin.Right + MoveVar, currentMargin.Bottom)
                    moveStored = 0
                End If
            Case "Right"
                If Not moveStored = 0 Then
                    SnakeHead.Margin = New Thickness(currentMargin.Left + MoveVar, currentMargin.Top, currentMargin.Right - MoveVar, currentMargin.Bottom)
                    moveStored = 1
                End If
            Case "Up"
                If Not moveStored = 3 Then
                    SnakeHead.Margin = New Thickness(currentMargin.Left, currentMargin.Top - MoveVar, currentMargin.Right, currentMargin.Bottom + MoveVar)
                    moveStored = 2
                End If
            Case "Down"
                If Not moveStored = 2 Then
                    SnakeHead.Margin = New Thickness(currentMargin.Left, currentMargin.Top + MoveVar, currentMargin.Right, currentMargin.Bottom - MoveVar)
                    moveStored = 3
                End If
        End Select
        Direction.Text = moveDirection
        DirectionNumber.Text = moveStored
    End Sub

    Private Sub MoveRestrict()
        Dim currentMargin As Thickness = SnakeHead.Margin

        ' Check if both X and Y coordinates are multiples of 20
        If currentMargin.Left Mod 20 = 0 AndAlso currentMargin.Top Mod 20 = 0 Then
            moveDirection = pendingDirection
        End If
    End Sub

    ' Collision and Window Boundary
    Private Sub CheckCollision()
        Dim tolerance As Integer = 10
        Dim FruitAttained As Integer = Integer.Parse(Score.Text)
        If Math.Abs(SnakeHead.Margin.Left - Fruit.Margin.Left) <= tolerance And Math.Abs(SnakeHead.Margin.Top - Fruit.Margin.Top) <= tolerance Then
            NewFruit()
            FruitAttained += 1
            Score.Text = FruitAttained.ToString()
            AddBodySegment() ' Add a new body segment
        End If
    End Sub

    Private Sub CheckSelfCollision()
        For i As Integer = 1 To snakeBody.Count - 1
            If SnakeHead.Margin.Left = snakeBody(i).Margin.Left AndAlso SnakeHead.Margin.Top = snakeBody(i).Margin.Top Then
                GameOver()
                Exit For
            End If
        Next
    End Sub

    Private Sub Boundary()
        If SnakeHead.Margin.Left < 0 Or SnakeHead.Margin.Left > windowWidth Or SnakeHead.Margin.Top < 0 Or SnakeHead.Margin.Top > windowHeight Then
            GameOver()
        End If
    End Sub
    ' Fruit
    Private Sub NewFruit()
        Dim placeVar As Integer = 20
        Dim newLeft As Integer = Math.Round(CInt(Rnd() * (windowWidth - Fruit.Width)) / placeVar) * placeVar
        Dim newTop As Integer = Math.Round(CInt(Rnd() * (windowHeight - Fruit.Height)) / placeVar) * placeVar

        If newLeft > Width Then
            newLeft = SnakeHead.Margin.Top - 200
        ElseIf newLeft < 0 Then
            newLeft = SnakeHead.Margin.Top + 200
        End If
        If newTop > Height Then
            newTop = SnakeHead.Margin.Left + 200
        ElseIf newTop < 0 Then
            newTop = SnakeHead.Margin.Left - 200
        End If

        STT.Text = newLeft
        GTT.Text = newTop

        RNGFruit()
        Fruit.Margin = New Thickness(newLeft, newTop, 0, 0)
    End Sub
    Private Sub RNGFruit()
        If moveDirection = "Left" Then
            Rnd()
        End If
    End Sub

    ' Body
    Private Sub AddBodySegment()
        ' Position the new segment based on the current direction
        Dim newSegment As New Rectangle With {
            .Width = SnakeHead.Width,
            .Height = SnakeHead.Height,
            .Fill = SnakeHead.Fill
        }

        Dim lastSegment As Rectangle = snakeBody.Last()
        Dim offset As Integer = 20

        Select Case moveDirection
            Case "Left"
                newSegment.Margin = New Thickness(lastSegment.Margin.Left + offset, lastSegment.Margin.Top, 0, 0)
            Case "Right"
                newSegment.Margin = New Thickness(lastSegment.Margin.Left - offset, lastSegment.Margin.Top, 0, 0)
            Case "Up"
                newSegment.Margin = New Thickness(lastSegment.Margin.Left, lastSegment.Margin.Top + offset, 0, 0)
            Case "Down"
                newSegment.Margin = New Thickness(lastSegment.Margin.Left, lastSegment.Margin.Top - offset, 0, 0)
        End Select

        snakeCanvas.Children.Add(newSegment)
        snakeBody.Add(newSegment)
    End Sub
End Class
' Thank you
