Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Public Class Conexion
    Public conexion As SqlConnection = New SqlConnection("Data Source= DESKTOP-IKI4BG5\MSSQLSERVER01;Initial Catalog=TiendaMelva; Integrated Security=True")
    Private cmb As SqlCommandBuilder
    Public ds As DataSet = New DataSet()
    Public da As SqlDataAdapter
    Public comand As SqlCommand
    Public dv As New DataView
    Public adap As SqlDataAdapter
    Public datos2 As DataSet
    Public cmd As New SqlCommand

    Dim des As New TripleDESCryptoServiceProvider
    Dim MD5 As New MD5CryptoServiceProvider

    Function MD5Hash(ByVal value As String) As Byte()
        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

    Function Encrypt(ByVal Stringinput As String, ByVal key As String) As String
        des.Key = MD5Hash(key)
        des.Mode = CipherMode.ECB
        Dim buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(Stringinput)
        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
    End Function

    Public Sub conectar()
        Try
            conexion.Open()
            MessageBox.Show("Conexión exitosa")
        Catch ex As Exception
            MessageBox.Show("Error conexion de base de datos")
        Finally
            conexion.Close()
        End Try
    End Sub
    Public Sub Llenar(ByVal sql, ByVal tabla)
        ds.Tables.Clear()
        da = New SqlDataAdapter(sql, conexion)
        cmb = New SqlCommandBuilder(da)
        da.Fill(ds, tabla)
        dv.Table = ds.Tables(0)
    End Sub

    Public Function ingresarUsuario(ID As Integer, nombre As String, apellido As String, username As String, psw As String, rol As String, estado As String, correo As String)
        Try
            conexion.Open()
            comand = New SqlCommand("insertarUsuario", conexion)
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.AddWithValue("@idUsuario", ID)
            comand.Parameters.AddWithValue("@nombre", nombre)
            comand.Parameters.AddWithValue("@apellido", apellido)
            comand.Parameters.AddWithValue("@userName", username)
            comand.Parameters.AddWithValue("@psw", Encrypt(psw, "abc"))
            comand.Parameters.AddWithValue("@rol", rol)
            comand.Parameters.AddWithValue("@estado", estado)
            comand.Parameters.AddWithValue("@correo", correo)
            If comand.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Public Function modificarUsuario(ID As Integer, nombre As String, apellido As String, username As String, psw As String, rol As String, correo As String)
        Try
            conexion.Open()
            comand = New SqlCommand("modificarUsuario", conexion)
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.AddWithValue("@idUsuario", ID)
            comand.Parameters.AddWithValue("@nombre", nombre)
            comand.Parameters.AddWithValue("@apellido", apellido)
            comand.Parameters.AddWithValue("@userName", username)
            comand.Parameters.AddWithValue("@psw", Encrypt(psw, "abc"))
            comand.Parameters.AddWithValue("@rol", rol)
            comand.Parameters.AddWithValue("@correo", correo)
            If comand.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function eliminarUsuario(ID As Integer, rol As String)
        Try
            conexion.Open()
            comand = New SqlCommand("eliminarUsuario", conexion)
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.AddWithValue("@idUsuario", ID)
            comand.Parameters.AddWithValue("@rol", rol)
            If comand.ExecuteNonQuery <> 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        Finally
            conexion.Close()
        End Try
    End Function
    Function busqueda(ByVal tabla, ByVal condicion) As DataTable
        Try
            conexion.Open()
            Dim buscar As String = "select * from " + tabla + " where " + condicion
            Dim cmd As New SqlCommand(buscar, conexion)
            If cmd.ExecuteNonQuery Then
                Dim dt As New DataTable
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                Return dt
            Else
                Return Nothing
            End If
            conexion.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function validarUsuario(userName As String, psw As String)
        Try
            conexion.Open()
            comand = New SqlCommand("validarUsuario", conexion)
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.AddWithValue("@userName", userName)
            comand.Parameters.AddWithValue("@psw", psw)
            If comand.ExecuteNonQuery <> 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        Finally
            conexion.Close()
        End Try
    End Function

End Class
