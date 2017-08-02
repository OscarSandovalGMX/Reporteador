Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net.Mail

Partial Class Pages_GarantiaPago
    Inherits System.Web.UI.Page
    Private clCatalogo As Catalogo
    Private SelEndosos() As String

    Private Sub Pages_GarantiaPago_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.Titulo = "Reporte de Garantias de Pago"

            Session.Add("blnExpiro", "1")

            Master.MuestraOpciones = True

            Master.Usuario = IIf(Session("Usuario") = "", "Inicia Sesión", Session("Usuario"))
            Master.cod_usuario = Session("cod_usuario")
            Master.cod_suc = Session("cod_suc")
            Master.cod_sector = Session("cod_sector")

            If Master.cod_usuario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
            End If

            LlenaSucursal()
            LlenaMoneda()
            BindDummyRow()
        Else
            If Session("blnExpiro") Is Nothing And hid_CierraSesion.Value = 0 Then
                Mensaje("ORDEN DE PAGO", "La sesión ha expirado por inactividad, debera ingresar sus credenciales nuevamente")
                Dim ConsultaBD As ConsultaBD
                ConsultaBD = New ConsultaBD
                ConsultaBD.InsertaBitacora(ConsultaBD.ConsultaUsuarioNT(Master.cod_usuario), Master.HostName, "Expiró Sesión", "La sesión ha expirado por inactividad, debere ingresar sus credenciales nuevamente")
                hid_CierraSesion.Value = 1
                System.Threading.Thread.Sleep(3000) ' 3 segundos
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
            End If
        End If
    End Sub
    Private Sub LlenaSucursal()
        clCatalogo = New Catalogo
        ddl_SucursalPol.DataValueField = "Clave"
        ddl_SucursalPol.DataTextField = "Descripcion"
        ddl_SucursalPol.DataSource = clCatalogo.ObtieneCatalogo("Suc")
        ddl_SucursalPol.DataBind()
    End Sub

    Private Sub LlenaMoneda()
        clCatalogo = New Catalogo

        ddl_Moneda.DataValueField = "Clave"
        ddl_Moneda.DataTextField = "Descripcion"
        ddl_Moneda.DataSource = clCatalogo.ObtieneCatalogo("Mon")
        ddl_Moneda.DataBind()

        ddl_Moneda.Items.Add("TODAS")
    End Sub

    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("Clave")
        dummy.Columns.Add("Descripcion")

        dummy.Rows.Add()
        gvd_Catalogo.DataSource = dummy
        gvd_Catalogo.DataBind()
    End Sub

    <System.Web.Services.WebMethod>
    Public Shared Function ObtieneDatos(ByVal Consulta As String) As List(Of Catalogo)
        Dim sCnn As String

        Consulta = Replace(Consulta, "==", "'")


        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(Consulta, sCnn)
        da.Fill(dt)

        Dim Lista = New List(Of Catalogo)

        Dim varCatalogo As Catalogo


        For Each dr In dt.Rows
            varCatalogo = New Catalogo
            varCatalogo.Catalogo(dr("Clave"), dr("Descripcion"))
            Lista.Add(varCatalogo)
        Next

        Return Lista
    End Function

    Private Function LogError(ByVal strError As String) As Boolean
        Dim ConsultaBD As ConsultaBD
        ConsultaBD = New ConsultaBD
        ConsultaBD.InsertaLogError(Master.cod_usuario, Master.HostName, strError)
        Return True
    End Function

    Private Function Mensaje(ByVal strSegmento As String, ByVal strMsg As String) As Boolean
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Mensaje", "EvaluaMensaje('" & strSegmento & "','" & Replace(Replace(strMsg, "'", ""), vbCrLf, " ") & "');", True)
        'hid_Mensaje.Value = strSegmento & ":" & vbCrLf & strMsg
        Return True
    End Function

    Private Sub btn_OkCatalogo_Click(sender As Object, e As EventArgs) Handles btn_OkCatalogo.Click
        Try

            Dim dtGridView As DataTable
            Dim Datos() As String
            dtGridView = New DataTable
            Dim Seleccionados As String = hid_Seleccion.Value
            If Len(Seleccionados) > 0 Then
                Dim Catalogo As String = hid_Catalogo.Value

                Dim GvdFiltro As GridView
                GvdFiltro = Nothing
                Select Case Catalogo

                    Case "RamC"
                        GvdFiltro = gvd_RamoContable

                    Case "Pro"
                        GvdFiltro = gvd_Producto

                    Case "RamU"
                        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                        txtClaveRam.Text = Split(Datos(0), "~")(0)
                        txtSearchRam.Text = Split(Datos(0), "~")(1)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#CatalogoModal');", True)
                        Exit Sub
                End Select

                dtGridView.Columns.Add("Clave")
                dtGridView.Columns.Add("Descripcion")

                For Each row As GridViewRow In GvdFiltro.Rows
                    Dim Elemento = DirectCast(row.FindControl("chk_Sel" & Catalogo), HiddenField)
                    If Elemento.Value <> "true" Then
                        Dim Fila As DataRow = dtGridView.NewRow()
                        Fila("Clave") = DirectCast(row.FindControl("lbl_Clave" & Catalogo), Label).Text
                        Fila("Descripcion") = DirectCast(row.FindControl("lbl_Desc"), Label).Text
                        dtGridView.Rows.Add(Fila)
                    End If
                Next


                Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                For Each dato In Datos
                    Dim Fila As DataRow = dtGridView.NewRow()
                    Fila("Clave") = Split(dato, "~")(0)
                    Fila("Descripcion") = Split(dato, "~")(1)
                    dtGridView.Rows.Add(Fila)
                Next

                GvdFiltro.DataSource = dtGridView
                GvdFiltro.DataBind()
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#CatalogoModal');", True)

        Catch ex As Exception
            Mensaje("REPORTES-: OK Catalogo", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_BuscaSufijo_Click(sender As Object, e As EventArgs) Handles btn_BuscaSufijo.Click
        Try
            Dim sCnn As String
            Dim FiltroFecha As String = ""

            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

            Dim Polizas As String = Replace(Replace(hid_Polizas.Value, "==", "'"), ",'''-1'',", "'")

            If Len(Polizas) = 0 Then
                Polizas = "'" & Polizas & "'"
            End If

            Dim sSel As String = "spS_ListaSufijo " & ddl_SucursalPol.SelectedValue & "," & IIf(txtClaveRam.Text = "", 1, txtClaveRam.Text) & "," & IIf(txt_NoPoliza.Text = "", 1, txt_NoPoliza.Text) & "," & Polizas & ",'" & FiltroFecha & "'"

            Dim da As SqlDataAdapter

            Dim dtRes As DataTable
            dtRes = New DataTable

            da = New SqlDataAdapter(sSel, sCnn)

            da.Fill(dtRes)

            If dtRes.Rows.Count > 0 Then
                gvd_GrupoPolizas.DataSource = dtRes
                gvd_GrupoPolizas.DataBind()
                ddl_SucursalPol.Enabled = False
                txtClaveRam.Enabled = False
                txtSearchRam.Enabled = False
                txt_NoPoliza.Enabled = False
                btn_BuscaSufijo.Enabled = False
                btn_CancelaSufijo.Enabled = True
            Else
                gvd_GrupoPolizas.DataSource = Nothing
                gvd_GrupoPolizas.DataBind()
                btn_CancelaSufijo_Click(Me, Nothing)
                Mensaje("REPORTES-:Busqueda", "No hay resultados que mostrar")
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_CancelaSufijo_Click(sender As Object, e As EventArgs) Handles btn_CancelaSufijo.Click
        Try
            gvd_GrupoPolizas.DataSource = Nothing
            gvd_GrupoPolizas.DataBind()
            ddl_SucursalPol.Enabled = True
            txtClaveRam.Enabled = True
            txtSearchRam.Enabled = True
            txt_NoPoliza.Enabled = True
            btn_BuscaSufijo.Enabled = True
            btn_CancelaSufijo.Enabled = False
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_OkPoliza_Click(sender As Object, e As EventArgs) Handles btn_OkPoliza.Click
        Try
            Dim dtGridView As DataTable
            dtGridView = New DataTable
            dtGridView.Columns.Add("Clave")
            dtGridView.Columns.Add("Descripcion")

            For Each row As GridViewRow In gvd_Poliza.Rows
                Dim Elemento = DirectCast(row.FindControl("chk_SelPol"), HiddenField)
                If Elemento.Value <> "true" Then
                    Dim Fila As DataRow = dtGridView.NewRow()
                    Fila("Clave") = DirectCast(row.FindControl("lbl_ClavePol"), Label).Text
                    Fila("Descripcion") = DirectCast(row.FindControl("lbl_DescripcionPol"), Label).Text
                    dtGridView.Rows.Add(Fila)
                End If
            Next

            For Each row As GridViewRow In gvd_GrupoPolizas.Rows
                Dim NewRow As DataRow = dtGridView.NewRow()
                Dim chk_SelPol As CheckBox = DirectCast(row.FindControl("chk_SelPol"), CheckBox)


                If chk_SelPol.Checked = True Then
                    Dim txt_Sucursal As TextBox = DirectCast(row.FindControl("txt_Sucursal"), TextBox)
                    Dim txt_Ramo As TextBox = DirectCast(row.FindControl("txt_Ramo"), TextBox)
                    Dim txt_Poliza As TextBox = DirectCast(row.FindControl("txt_Poliza"), TextBox)
                    Dim txt_Sufijo As TextBox = DirectCast(row.FindControl("txt_Sufijo"), TextBox)
                    Dim txt_NoEndosos As TextBox = DirectCast(row.FindControl("txt_NoEndosos"), TextBox)


                    NewRow("Clave") = txt_Sucursal.Text & "-" & txt_Ramo.Text & "-" & txt_Poliza.Text & "-" & txt_Sufijo.Text
                    NewRow("Descripcion") = txt_NoEndosos.Text
                    dtGridView.Rows.Add(NewRow)
                End If
            Next

            gvd_Poliza.DataSource = dtGridView
            gvd_Poliza.DataBind()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Ok Poliza", "BuscaSufijo();", True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Poliza_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Poliza.RowCommand
        Try
            If e.CommandName.Equals("Endoso") Then
                Dim sCnn As String
                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                Dim Clave As String = sender.DataKeys(Index)("Clave")
                Dim FiltroFecha As String = ""


                GuardaIdPv(hid_ClavePol.Value)
                hid_ClavePol.Value = Clave


                sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

                Dim sSel As String = "spS_ListaEndoso '''" & Clave & "''','" & FiltroFecha & "'"

                Dim da As SqlDataAdapter

                Dim dtRes As DataTable
                dtRes = New DataTable

                da = New SqlDataAdapter(sSel, sCnn)

                da.Fill(dtRes)

                gvd_Endosos.DataSource = dtRes
                gvd_Endosos.DataBind()

                If Not SelEndosos Is Nothing Then
                    For Each Row In gvd_Endosos.Rows
                        Dim hid_idPv = DirectCast(Row.FindControl("hid_idPv"), HiddenField)

                        Dim Endoso = From b In SelEndosos
                                     Where b.Contains(hid_idPv.Value)
                                     Select b

                        If Endoso.Count > 0 Then
                            Dim chk_SelPol = DirectCast(Row.FindControl("chk_SelPol"), CheckBox)
                            chk_SelPol.Checked = True
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Function GuardaIdPv(ByVal Poliza As String) As Boolean
        Dim strIdPv As String = ""
        Dim blnActualiza As Boolean = False
        Dim ClavePol As String = ""

        If Len(Poliza) > 0 Then
            For Each Row In gvd_Endosos.Rows
                Dim chk_SelPol = DirectCast(Row.FindControl("chk_SelPol"), CheckBox)
                If chk_SelPol.Checked = True Then
                    Dim hid_idPv = DirectCast(Row.FindControl("hid_idPv"), HiddenField)
                    strIdPv = strIdPv & "," & hid_idPv.Value
                End If
            Next

            If Len(strIdPv) > 0 Then
                strIdPv = Mid(strIdPv, 2, Len(strIdPv) - 1)
            End If

            SelEndosos = Session("SelEndosos")

            If Not SelEndosos Is Nothing Then

                For i = 0 To UBound(SelEndosos)
                    ClavePol = Split(SelEndosos(i), "|")(0)
                    If ClavePol = Poliza Then
                        SelEndosos(i) = ClavePol & "|" & strIdPv
                        blnActualiza = True
                        Exit For
                    End If
                Next

                If blnActualiza = False Then
                    ReDim Preserve SelEndosos(UBound(SelEndosos) + 1)
                    SelEndosos(UBound(SelEndosos)) = Poliza & "|" & strIdPv
                End If
            Else
                SelEndosos = {Poliza & "|" & strIdPv}
            End If

            Session("SelEndosos") = SelEndosos
        End If

        Return True
    End Function

    Private Function ObtieneElementos(ByRef Gvd As GridView, ByVal Catalogo As String, ByVal blnTexto As Boolean) As String
        Dim strDatos As String = ""
        For Each row As GridViewRow In Gvd.Rows
            Dim Elemento = DirectCast(row.FindControl("chk_Sel" & Catalogo), HiddenField)
            If Elemento.Value <> "true" Then
                strDatos = strDatos & IIf(blnTexto, ",'", ",") & DirectCast(row.FindControl("lbl_Clave" & Catalogo), Label).Text & IIf(blnTexto, "'", "")
            End If
        Next

        If Len(strDatos) > 0 Then
            strDatos = Mid(strDatos, 2, Len(strDatos) - 1)
        End If

        Return strDatos
    End Function

    Private Function FechaAIngles(ByVal Fecha As String) As String
        If Fecha <> vbNullString Then
            Return String.Format("{0:MM/dd/yyyy}", CDate(Fecha))
        Else
            Return ""
        End If

    End Function

    Private Function ObtieneFiltros() As String
        Dim Elementos As String
        Dim FiltroPol As String = "&FiltroPol="
        Dim FiltroRamoCont As String = "&FiltroRamoCont="
        Dim FiltroRamoTec As String = "&FiltroRamoTec="
        Dim FiltroFechaIni As String = "&FechaGarIni=" & FechaAIngles(txt_FechaIni.Text)
        Dim FiltroFechaFin As String = "&FechaGarFin=" & FechaAIngles(txt_FechaFin.Text)
        Dim FiltroSalida As String = "&intSalida=" & opt_Garantias.SelectedValue


        Elementos = ObtieneElementos(gvd_Poliza, "Pol", True)
        If Elementos <> "" Then
            FiltroPol = FiltroPol & Replace(Elementos, "'", "|")
        End If

        Elementos = ObtieneElementos(gvd_RamoContable, "RamC", False)
        If Elementos <> "" Then
            FiltroRamoCont = FiltroRamoCont & Elementos
        End If

        Elementos = ObtieneElementos(gvd_Producto, "Pro", False)
        If Elementos <> "" Then
            FiltroRamoTec = FiltroRamoTec & Elementos
        End If

        Return FiltroPol & FiltroRamoCont & FiltroRamoTec & FiltroFechaIni & FiltroFechaFin & FiltroSalida
    End Function

    Private Sub btn_Generar_Click(sender As Object, e As EventArgs) Handles btn_Generar.Click

        Dim Formato As String = ""
        Dim Moneda As String


        Try
            If Not IsDate(txt_FechaIni.Text) And Not IsDate(txt_FechaFin.Text) Then
                Mensaje("REPORTES-: ", "Se debe establecer un rango de fechas válido")
                Exit Sub
            End If

            If Master.Formato <> "NAV" Then
                Formato = "&rs%3AFormat=" & Master.Formato
            End If

            Moneda = "&intMoneda=" & IIf(ddl_Moneda.SelectedIndex = 2, -1, ddl_Moneda.SelectedValue)
            Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2f"

            If chk_Estimacion.Checked = False Then
                server = server & "GarantiasOP" & "&rc:Parameters=false" & Formato & ObtieneFiltros() & Moneda
            Else
                server = server & "EstimacionOP" & "&rc:Parameters=false" & Formato & ObtieneFiltros() & Moneda
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirReporte", "ImprimirReporte('" & server & "');", True)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub chk_Estimacion_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Estimacion.CheckedChanged
        Try
            If chk_Estimacion.Checked = True Then
                opt_Garantias.Enabled = False
            Else
                opt_Garantias.Enabled = True
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub
End Class
