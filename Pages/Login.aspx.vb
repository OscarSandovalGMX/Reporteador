﻿Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data
Imports System.Web.Services
Imports System.DirectoryServices.AccountManagement
Imports System.DirectoryServices
Imports System.Net
Imports System.Net.Dns
Imports System.IO

Partial Class Pages_Login
    Inherits System.Web.UI.Page

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            Dim usuarioNT As String
            Dim ConsultaBD As ConsultaBD
            ConsultaBD = New ConsultaBD

            usuarioNT = ConsultaBD.ConsultaUsuarioNT(hid_usuario.Value)

            If usuarioNT <> "" Then
                If IsAuthenticated("GMX.COM.MX", usuarioNT, hid_contraseña.Value) Then
                    If ConsultaBD.ConsultaUsuario(usuarioNT) <> "" Then
                        ConsultaBD.InsertaBitacora(usuarioNT, Master.HostName, "Inicio de Sesión (Reportes)", "Inicio Exitoso")
                        Master.VisibleBtnCerrar = False
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Inicio.aspx');", True)
                    Else
                        Mensaje("Login (Reportes)", "No cuenta con acceso a SII")
                        ConsultaBD.InsertaBitacora(usuarioNT, Master.HostName, "Inicio Erroneo (Reportes)", "No cuenta con acceso a SII")
                    End If
                Else
                    Mensaje("Login (Reportes)", "Usuario y/o Contraseña de Red incorrectos o su cuenta esta bloqueada")
                    ConsultaBD.InsertaBitacora(usuarioNT, Master.HostName, "Inicio Erroneo (Reportes)", "Usuario y/o Contraseña de Red incorrectos o su cuenta esta bloqueada")
                End If
            Else
                Mensaje("Login", "Usuario y/o Contraseña incorrectos")
                If Len(hid_usuario.Value) Then
                    ConsultaBD.InsertaBitacora(hid_usuario.Value, Master.HostName, "Inicio Erroneo (Reportes)", "Usuario y/o Contraseña incorrectos")
                End If
            End If


        Catch ex As Exception
            Mensaje("REPORTES-:LOGIN", ex.Message)
        End Try

    End Sub

    Private Sub Pages_Login_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.MuestraBarra = False

            Header.Title = "GMX - Login"
            'Obtiene el usuario del sistema
            'hid_usuario.Value = System.Environment.UserName
            Master.VisibleBtnCerrar = False
            Master.HostName = Session("HostName")

            'Dim usuario() = Split(System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(), "\")

            'If UBound(usuario) > 0 Then
            '    hid_usuario.Value = usuario(1)
            'End If

        End If
    End Sub

    Private Function Mensaje(ByVal strSegmento As String, ByVal strMsg As String) As Boolean
        hid_Mensaje.Value = strSegmento & ":" & vbCrLf & strMsg
        Return True
    End Function

    <System.Web.Services.WebMethod>
    Public Shared Function IsAuthenticated(ByVal Domain As String, ByVal username As String, ByVal pwd As String) As Boolean
        Dim Success As Boolean = False
        Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://" & Domain, username, pwd)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
        Catch
            Success = False
        End Try
        Return Success
    End Function
End Class
