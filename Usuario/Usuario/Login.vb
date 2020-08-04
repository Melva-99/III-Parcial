Public Class Login
    Dim conexion As New Conexion
    Private Sub ingresarMenu()
        Dim userName, psw As String
        userName = txtUsuario.text
        psw = txtPsw.text
        Try
            If (conexion.validarUsuario(userName, psw)) Then
                MsgBox("Correcto")
                Form1.Show()
                Me.Hide()
            Else
                MsgBox("Usuario/Contraseña inválida")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
        ingresarMenu()
    End Sub
End Class