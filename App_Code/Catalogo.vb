Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Catalogo
    Private _Clave As String
    Private _Descripcion As String

    Public Sub Catalogo(ByVal clave As String, ByVal descripcion As String)
        Me.Clave = clave
        Me.Descripcion = descripcion
    End Sub

    Public Property Clave() As String
        Get
            Return _Clave
        End Get
        Set(ByVal value As String)
            _Clave = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property

    Public Function ObtieneCatalogo(ByVal Catalogo As String, Optional ByVal Condicion As String = "", Optional ByVal strSel As String = "") As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_CatalogosOP '" & Catalogo & "','" & Condicion & "','" & strSel & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneVersiones(ByVal cod_reporte As Integer, ByVal cod_config As Integer, ByVal cod_usuario As String) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_VersionReporte " & cod_reporte & "," & cod_config & ",'" & cod_usuario & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneNumCuotas(ByVal id_pv As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_NumCuotas " & id_pv

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneAgrupacion(ByVal cod_reporte As Integer, ByVal cod_config As Integer, Optional ByVal cod_agrupacion As Integer = -1) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_AgrupacionReporte " & cod_reporte & "," & cod_config & "," & cod_agrupacion

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneFilasXAgrupacion(ByVal cod_agrupacion As Integer, ByVal snValor As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_FilasXAgrupacion " & cod_agrupacion & "," & snValor

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneFiltrosXAgrupacion(ByVal cod_agrupacion As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_FiltrosXAgrupacion " & cod_agrupacion

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneConsultas(ByVal cod_reporte As Integer, ByVal cod_config As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_Consultas " & cod_reporte & "," & cod_config

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneFormulas(ByVal cod_reporte As Integer, ByVal cod_config As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_Formulas " & cod_reporte & "," & cod_config

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ValidaTemporal(ByVal cod_reporte As Integer, ByVal cod_config As Integer) As Integer
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_ValidaTemporal " & cod_reporte & "," & cod_config

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt.Rows(0)("sn_Existe")
    End Function

    Public Function ObtieneAgrupador(ByVal cod_reporte As Integer, ByVal cod_config As Integer, ByVal strFilas As String, ByVal strValores As String, ByVal strFiltros As String, ByVal strParciales As String) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_GeneraAgrupacion " & cod_reporte & "," & cod_config & ",'" & strFilas & "','" & strValores & "','" & strFiltros & "','" & strParciales & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneDetalleCampo(ByVal strCampo As String) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_DetalleColumnas '" & strCampo & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

End Class

