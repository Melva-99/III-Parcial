Imports System.Text.RegularExpressions
Public Class Form1
    Dim Conexion As New Conexion
    Dim dt As New DataTable
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Conexion.conectar()
        Llenar()
    End Sub
    Private Sub Llenar()
        Conexion.Llenar("select idUsuario as 'ID', nombre as 'Nombre', apellido as 'Apellido', nombreUsuario as 'Usuario', psw as 'Contraseña', rol as 'Rol', estado as 'Estado', correo as 'Correo Eléctronico' from usuario", "usuario")
        DataUsuario.DataSource = Conexion.ds.Tables("usuario")
    End Sub
    Private Function validarCorreo(ByVal isCorreo As String) As Boolean
        Return Regex.IsMatch(isCorreo, "^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$")
    End Function
    Private Sub limpiar()
        txtCod.Clear()
        txtNombre.Clear()
        txtApellido.Clear()
        txtUsuario.Clear()
        txtContra.Clear()
        txtCorreo.Clear()
    End Sub
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If validarCorreo(LCase(txtCorreo.Text)) = False Then
            MessageBox.Show("Correo invalido, *username@midominio.com*", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCorreo.Focus()
            txtCorreo.SelectAll()
        Else
            insertarUsuario()
            'MessageBox.Show("Correo valido", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Conexion.conexion.Close()
        End If
    End Sub
    Private Sub insertarUsuario()
        Dim ID As Integer
        Dim nombre, apellido, username, psw, correo, rol, estado As String
        ID = txtCod.Text
        nombre = txtNombre.Text
        apellido = txtApellido.Text
        username = txtUsuario.Text
        psw = txtContra.Text
        correo = txtCorreo.Text
        estado = "activo"
        rol = cmbRol.Text
        Try
            If Conexion.ingresarUsuario(ID, convertMayus(nombre), convertMayus(apellido), username, psw, rol, estado, LCase(correo)) Then
                MessageBox.Show("Guardado", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Llenar()
            Else
                MessageBox.Show("Error al guardar", "Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        eliminarUsuario()
    End Sub
    Private Sub eliminarUsuario()
        Dim ID As Integer
        Dim rol As String
        ID = txtCod.Text
        rol = cmbRol.Text
        Try
            If Conexion.eliminarUsuario(ID, rol) Then
                MessageBox.Show("Eliminado", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Llenar()
            Else
                MessageBox.Show("Error al eliminar", "Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub modificarUsuaurio()
        Dim ID As Integer
        Dim nombre, apellido, username, psw, correo, rol As String
        ID = txtCod.Text
        nombre = txtNombre.Text
        apellido = txtApellido.Text
        username = txtUsuario.Text
        psw = txtContra.Text
        correo = txtCorreo.Text
        rol = cmbRol.Text
        Try
            If Conexion.modificarUsuario(ID, nombre, apellido, username, psw, rol, correo) Then
                MessageBox.Show("Modificado Exitosamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Llenar()
                Conexion.conexion.Close()
            Else
                MessageBox.Show("Error al modificar", "Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        modificarUsuaurio()
    End Sub

    Private Sub DataUsuario_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataUsuario.CellClick
        Dim FilaActual As Integer
        FilaActual = DataUsuario.CurrentRow.Index
        txtCod.Text = DataUsuario.Rows(FilaActual).Cells(0).Value
        txtNombre.Text = DataUsuario.Rows(FilaActual).Cells(1).Value
        txtApellido.Text = DataUsuario.Rows(FilaActual).Cells(2).Value
        txtUsuario.Text = DataUsuario.Rows(FilaActual).Cells(3).Value
        txtContra.Text = DataUsuario.Rows(FilaActual).Cells(4).Value
        cmbRol.Text = DataUsuario.Rows(FilaActual).Cells(5).Value
        txtCorreo.Text = DataUsuario.Rows(FilaActual).Cells(7).Value
    End Sub
    Private Sub buscar()
        Try
            dt = Conexion.busqueda(" usuario ", " nombreUsuario like '%" + txtBuscar.Text + "%'")
            If dt.Rows.Count <> 0 Then
                DataUsuario.DataSource = dt
                Conexion.conexion.Close()
            Else
                DataUsuario.DataSource = Nothing
                Conexion.conexion.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        buscar()
    End Sub
    Function convertMayus(ByVal text As String)
        Return StrConv(text, VbStrConv.ProperCase)
    End Function
End Class
