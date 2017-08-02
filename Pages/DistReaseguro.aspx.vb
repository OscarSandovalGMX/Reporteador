Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net.Mail



Partial Class Pages_DistReaseguro
    Inherits System.Web.UI.Page
    Private clCatalogo As Catalogo
    Private SelEndosos() As String

    Private Sub Pages_DistReaseguro_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.Timeout = 30

            Master.Titulo = "Reporte de Distribución de Reaseguro-Ubicaciones-Coberturas"
            Session.Add("blnExpiro", "1")
            Session.Add("SelEndosos", SelEndosos)

            Master.MuestraOpciones = True

            Master.Usuario = IIf(Session("Usuario") = "", "Inicia Sesión", Session("Usuario"))
            Master.cod_usuario = Session("cod_usuario")
            Master.cod_suc = Session("cod_suc")
            Master.cod_sector = Session("cod_sector")

            'If Master.cod_usuario = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
            '    Exit Sub
            'End If

            LlenaSucursal()
            BindDummyRow()
            LlenaControlesCampos(1)
            ObtieneVersiones()
            Session.Add("dtCampos", GeneraDatatableCampos)

            Me.btn_DesTodos.Attributes.Add("OnClick", "return SeleccionTodos('" + Me.gvd_Seleccion.ClientID + "',true);")
            Me.btn_DesNinguno.Attributes.Add("OnClick", "return SeleccionTodos('" + Me.gvd_Seleccion.ClientID + "',false);")

            Me.btn_CatTodos.Attributes.Add("OnClick", "return SeleccionTodos('" + Me.gvd_Catalogo.ClientID + "',true);")
            Me.btn_CatNinguno.Attributes.Add("OnClick", "return SeleccionTodos('" + Me.gvd_Catalogo.ClientID + "',false);")
        Else
            If Session("blnExpiro") Is Nothing And hid_CierraSesion.Value = 0 Then
                Mensaje("REPORTES", "La sesión ha expirado por inactividad, debera ingresar sus credenciales nuevamente")
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

    Private Sub ObtieneVersiones()
        clCatalogo = New Catalogo
        lst_Versiones.DataValueField = "Clave"
        lst_Versiones.DataTextField = "Descripcion"
        lst_Versiones.DataSource = clCatalogo.ObtieneCatalogo("Ver", " WHERE cod_reporte IN (" & hid_codReporte.Value & ")")
        lst_Versiones.DataBind()
    End Sub

    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("Clave")
        dummy.Columns.Add("Descripcion")

        dummy.Rows.Add()
        gvd_Catalogo.DataSource = dummy
        gvd_Catalogo.DataBind()
    End Sub

    Private Function ObtieneFiltros() As String
        Dim FiltroBroker As String = "&FiltroBroker="
        Dim FiltroCia As String = "&FiltroCia="
        Dim FiltroPol As String = "&FiltroPol="
        Dim FiltroRamoCont As String = "&FiltroRamoCont="
        Dim FiltroRamoTec As String = "&FiltroRamoTec="
        Dim FiltroFecha As String = "&FecEmision="
        Dim FiltroFechaVig As String = "&FecVigencia="
        Dim FiltroReaseguro As String = "&snReaseguro=" & IIf(chk_Reaseguro.Checked, 1, 0)
        Dim FiltroColumnas As String = "&NumCols="
        Dim FiltroSeleccion As String = "&strFields="
        Dim FiltroContrato As String = "&FiltroContrato=" & txt_Contrato.Text


        FiltroBroker = FiltroBroker & hid_IdBroker.Value
        FiltroCia = FiltroCia & hid_IdCompañia.Value
        FiltroPol = FiltroPol & IIf(Len(hid_IdSufijos.Value) > 0, "|", "") & Replace(hid_IdSufijos.Value, ",", "|,|") & IIf(Len(hid_IdSufijos.Value) > 0, "|", "")
        FiltroRamoCont = FiltroRamoCont & hid_IdRamoContable.Value
        FiltroRamoTec = FiltroRamoTec & hid_IdProducto.Value

        'If IsDate(txt_FechaIni.Text) And IsDate(txt_FechaFin.Text) Then
        '    If CDate(txt_FechaIni.Text) <= CDate(txt_FechaFin.Text) Then
        '        FiltroFecha = FiltroFecha & " AND fec_emi >= ||" & FechaAIngles(txt_FechaIni.Text) & "|| AND fec_emi < ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "||"
        '    End If
        'End If

        If IsDate(txt_FechaIni.Text) Then
            FiltroFecha = FiltroFecha & " AND fec_emi >= ||" & FechaAIngles(txt_FechaIni.Text) & "||"
            If IsDate(txt_FechaFin.Text) Then
                FiltroFecha = FiltroFecha & " AND fec_emi < ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "||"
            End If
        End If

        If IsDate(txt_FinVigDe.Text) Then
            FiltroFechaVig = FiltroFechaVig & " AND fec_vig_hasta >= ||" & FechaAIngles(txt_FinVigDe.Text) & "||"
            If IsDate(txt_FinVigA.Text) Then
                FiltroFechaVig = FiltroFechaVig & " AND fec_vig_hasta < ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FinVigA.Text))) & "||"
            End If
        End If

        'For Each row In gvd_Configuracion.Rows
        '    Dim cod_campo As Integer = TryCast(gvd_Configuracion.Rows(row.rowIndex).FindControl("lbl_Clave"), Label).Text
        '    FiltroSeleccion = FiltroSeleccion & cod_campo & ","
        'Next

        FiltroSeleccion = FiltroSeleccion & hid_IdGenerales.Value &
                          IIf(Len(hid_IdReaseguro.Value) > 0, ",", "") & hid_IdReaseguro.Value &
                          IIf(Len(hid_IdUbicaciones.Value) > 0, ",", "") & hid_IdUbicaciones.Value &
                          IIf(Len(hid_IdCoberturas.Value) > 0, ",", "") & hid_IdCoberturas.Value


        If FiltroSeleccion <> "&strFields=" Then
            FiltroColumnas = FiltroColumnas & UBound(Split(FiltroSeleccion, ",")) + 1
        Else
            FiltroColumnas = FiltroColumnas & 26
        End If

        Return FiltroBroker & FiltroCia & FiltroPol & FiltroRamoCont & FiltroRamoTec & FiltroFecha & FiltroFechaVig & FiltroReaseguro & FiltroColumnas & FiltroSeleccion & FiltroContrato
    End Function

    Private Function InsertaVersion(ByVal descripcion As String) As Integer
        Dim cod_config As Integer = 0
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Comando As SqlClient.SqlCommand
        Dim strCampos As String = ""
        Dim Resultado As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        conn.Open()
        Dim trRep As SqlTransaction
        trRep = conn.BeginTransaction()
        Try
            sSel = "EXEC spI_VersionReporte " & hid_codReporte.Value & ",'" & Master.cod_usuario & "','" & descripcion & "','" & ObtieneFiltros() & "','" & Master.Formato & "'"

            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            cod_config = Convert.ToInt32(Comando.ExecuteScalar())

            For Each row In gvd_Configuracion.Rows
                Dim cod_campo As Integer = TryCast(gvd_Configuracion.Rows(row.rowIndex).FindControl("lbl_Clave"), Label).Text
                strCampos = strCampos & "(@strKey," & cod_campo & "," & row.RowIndex + 1 & "),"
            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "EXEC spI_OfGread 'aCRD_ConfigReporteDet','" & cod_config & "','" & strCampos & "'"
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Resultado = Convert.ToInt32(Comando.ExecuteScalar())

            trRep.Commit()
            conn.Close()
        Catch ex As Exception
            trRep.Rollback()
            conn.Close()
            Mensaje("REPORTES-: Inserta", ex.Message)
            LogError(ex.Message)
            Return -1
        End Try

        Return cod_config

    End Function

    Private Function ActualizaVersion(ByVal cod_config As Integer, ByVal descripcion As String) As Integer
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Comando As SqlClient.SqlCommand
        Dim strCampos As String = ""
        Dim Resultado As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        conn.Open()
        Dim trRep As SqlTransaction
        trRep = conn.BeginTransaction()
        Try
            sSel = "EXEC spU_VersionReporte " & cod_config & "," & hid_codReporte.Value & ",'" & Master.cod_usuario & "','" & descripcion & "','" & ObtieneFiltros() & "','" & Master.Formato & "'"

            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            cod_config = Convert.ToInt32(Comando.ExecuteScalar())

            For Each row In gvd_Configuracion.Rows
                Dim cod_campo As Integer = TryCast(gvd_Configuracion.Rows(row.rowIndex).FindControl("lbl_Clave"), Label).Text
                strCampos = strCampos & "(@strKey," & cod_campo & "," & row.RowIndex + 1 & "),"
            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "DELETE FROM aCRD_ConfigReporteDet WHERE cod_config =" & cod_config
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Convert.ToInt32(Comando.ExecuteScalar())

            sSel = "EXEC spI_OfGread 'aCRD_ConfigReporteDet','" & cod_config & "','" & strCampos & "'"
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Resultado = Convert.ToInt32(Comando.ExecuteScalar())

            trRep.Commit()
            conn.Close()
        Catch ex As Exception
            trRep.Rollback()
            conn.Close()
            Mensaje("REPORTES-: Actualiza", ex.Message)
            LogError(ex.Message)
            Return -1
        End Try

        Return cod_config
    End Function

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
            Dim Datos() As String
            Dim Seleccionados As String = hid_Seleccion.Value
            Dim nro_seccion As Integer

            If Len(Seleccionados) > 0 Then
                Dim Catalogo As String = hid_Catalogo.Value

                Dim hid_Id As HiddenField
                hid_Id = New HiddenField

                Dim hid_Descripcion As HiddenField
                hid_Descripcion = New HiddenField

                Select Case Catalogo
                    Case "Fld1"
                        hid_Id = hid_IdGenerales
                        hid_Descripcion = hid_Generales
                        lnk_Generales.ForeColor = Drawing.Color.Red
                        nro_seccion = 1

                    Case "Fld2"
                        hid_Id = hid_IdReaseguro
                        hid_Descripcion = hid_Reaseguro
                        lnk_Reaseguro.ForeColor = Drawing.Color.Red
                        nro_seccion = 2

                    Case "Fld3"
                        hid_Id = hid_IdUbicaciones
                        hid_Descripcion = hid_Ubicaciones
                        lnk_Ubicaciones.ForeColor = Drawing.Color.Red
                        nro_seccion = 3

                    Case "Fld4"
                        hid_Id = hid_IdCoberturas
                        hid_Descripcion = hid_Coberturas
                        lnk_Coberturas.ForeColor = Drawing.Color.Red
                        nro_seccion = 4

                    Case "Bro"
                        hid_Id = hid_IdBroker
                        hid_Descripcion = hid_Broker
                        lnk_Broker.ForeColor = Drawing.Color.Red

                    Case "Cia"
                        hid_Id = hid_IdCompañia
                        hid_Descripcion = hid_Compañia
                        lnk_Compañia.ForeColor = Drawing.Color.Red

                    Case "RamC"
                        hid_Id = hid_IdRamoContable
                        hid_Descripcion = hid_RamoContable
                        lnk_RamoCOntable.ForeColor = Drawing.Color.Red

                    Case "Pro"
                        hid_Id = hid_IdProducto
                        hid_Descripcion = hid_Producto
                        lnk_Producto.ForeColor = Drawing.Color.Red

                    Case "RamU"
                        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                        txtClaveRam.Text = Split(Datos(0), "~")(0)
                        txtSearchRam.Text = Split(Datos(0), "~")(1)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#CatalogoModal');", True)
                        Exit Sub
                End Select

                Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                For Each dato In Datos
                    hid_Id.Value = hid_Id.Value & IIf(Len(hid_Id.Value) > 0, ",", "") & Split(dato, "~")(0)
                    hid_Descripcion.Value = hid_Descripcion.Value & IIf(Len(hid_Descripcion.Value) > 0, "|", "") & Split(dato, "~")(1)

                    If Catalogo = "Fld1" Or Catalogo = "Fld2" Or Catalogo = "Fld3" Or Catalogo = "Fld4" Then
                        AgregaCampo(Split(dato, "~")(0), Split(dato, "~")(1), nro_seccion)
                    End If
                Next

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

            'Dim Polizas As String = Replace(Replace(hid_Polizas.Value, "==", "'"), ",'''-1'',", "'")

            Dim Polizas As String = "'''" & Replace(hid_IdSufijos.Value, ",", "'',''") & "'''"

            If Len(Polizas) = 0 Then
                Polizas = "'" & Polizas & "'"
            End If

            If IsDate(txt_FechaIni.Text) And IsDate(txt_FechaFin.Text) Then
                If CDate(txt_FechaIni.Text) <= CDate(txt_FechaFin.Text) Then
                    FiltroFecha = " AND fec_emi >= ''" & FechaAIngles(txt_FechaIni.Text) & "'' AND fec_emi < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "''"
                End If
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
                txt_FechaIni.Enabled = False
                txt_FechaFin.Enabled = False
            Else
                gvd_GrupoPolizas.DataSource = Nothing
                gvd_GrupoPolizas.DataBind()
                btn_CancelaSufijo_Click(Me, Nothing)
                'Mensaje("REPORTES-:Busqueda", "No hay resultados que mostrar")
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
            txt_FechaIni.Enabled = True
            txt_FechaFin.Enabled = True
            btn_BuscaSufijo.Enabled = True
            btn_CancelaSufijo.Enabled = False

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_OkPoliza_Click(sender As Object, e As EventArgs) Handles btn_OkPoliza.Click
        Try


            For Each row As GridViewRow In gvd_GrupoPolizas.Rows
                Dim chk_SelPol As CheckBox = DirectCast(row.FindControl("chk_SelPol"), CheckBox)

                If chk_SelPol.Checked = True Then
                    Dim txt_Sucursal As TextBox = DirectCast(row.FindControl("txt_Sucursal"), TextBox)
                    Dim txt_Ramo As TextBox = DirectCast(row.FindControl("txt_Ramo"), TextBox)
                    Dim txt_Poliza As TextBox = DirectCast(row.FindControl("txt_Poliza"), TextBox)
                    Dim txt_Sufijo As TextBox = DirectCast(row.FindControl("txt_Sufijo"), TextBox)
                    Dim txt_NoEndosos As TextBox = DirectCast(row.FindControl("txt_NoEndosos"), TextBox)

                    hid_IdSufijos.Value = hid_IdSufijos.Value & IIf(Len(hid_IdSufijos.Value), ",", "") & txt_Sucursal.Text & "-" & txt_Ramo.Text & "-" & txt_Poliza.Text & "-" & txt_Sufijo.Text
                    hid_Sufijos.Value = hid_Sufijos.Value & IIf(Len(hid_Sufijos.Value), "|", "") & txt_NoEndosos.Text
                    lnk_Poliza.ForeColor = Drawing.Color.Red
                End If
            Next

            btn_BuscaSufijo_Click(Nothing, Nothing)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "Ok Poliza", "BuscaSufijo();", True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    'Private Sub gvd_Poliza_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Poliza.RowCommand
    '    Try
    '        If e.CommandName.Equals("Endoso") Then
    '            Dim sCnn As String
    '            Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
    '            Dim Clave As String = sender.DataKeys(Index)("Clave")
    '            Dim FiltroFecha As String = ""


    '            GuardaIdPv(hid_ClavePol.Value)
    '            hid_ClavePol.Value = Clave


    '            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

    '            If IsDate(txt_FechaIni.Text) And IsDate(txt_FechaFin.Text) Then
    '                If CDate(txt_FechaIni.Text) <= CDate(txt_FechaFin.Text) Then
    '                    FiltroFecha = " AND fec_emi >= ''" & FechaAIngles(txt_FechaIni.Text) & "'' AND fec_emi < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "''"
    '                End If
    '            End If

    '            Dim sSel As String = "spS_ListaEndoso '''" & Clave & "''','" & FiltroFecha & "'"

    '            Dim da As SqlDataAdapter

    '            Dim dtRes As DataTable
    '            dtRes = New DataTable

    '            da = New SqlDataAdapter(sSel, sCnn)

    '            da.Fill(dtRes)

    '            gvd_Endosos.DataSource = dtRes
    '            gvd_Endosos.DataBind()

    '            If Not SelEndosos Is Nothing Then
    '                For Each Row In gvd_Endosos.Rows
    '                    Dim hid_idPv = DirectCast(Row.FindControl("hid_idPv"), HiddenField)

    '                    Dim Endoso = From b In SelEndosos
    '                                 Where b.Contains(hid_idPv.Value)
    '                                 Select b

    '                    If Endoso.Count > 0 Then
    '                        Dim chk_SelPol = DirectCast(Row.FindControl("chk_SelPol"), CheckBox)
    '                        chk_SelPol.Checked = True
    '                    End If
    '                Next
    '            End If

    '        End If
    '    Catch ex As Exception
    '        Mensaje("REPORTES-: ", ex.Message)
    '        LogError(ex.Message)
    '    End Try
    'End Sub

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

    Private Sub LlenaInfoFiltros(ByVal Filtros() As String)
        Dim sCnn As String
        Dim sSel As String


        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        sSel = "spS_InfoFiltros '" & Split(Filtros(1), "=")(1) & "','" &
                                     Split(Filtros(2), "=")(1) & "','" &
                                     Split(Filtros(3), "=")(1) & "','" &
                                     Split(Filtros(4), "=")(1) & "','" &
                                     Split(Filtros(5), "=")(1) & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        llenaGridFiltro(hid_IdBroker, hid_Broker, dt, "Bro", lnk_Broker)
        llenaGridFiltro(hid_IdCompañia, hid_Compañia, dt, "Cia", lnk_Compañia)
        llenaGridFiltro(hid_IdSufijos, hid_Sufijos, dt, "Pol", lnk_Poliza)
        llenaGridFiltro(hid_IdRamoContable, hid_RamoContable, dt, "RamC", lnk_RamoCOntable)
        llenaGridFiltro(hid_IdProducto, hid_Producto, dt, "Pro", lnk_Producto)

        Dim Datos() As String = Split(Filtros(6), "||")
        If UBound(Datos) > 0 Then
            txt_FechaIni.Text = FechaAEspañol(Datos(1))
            If UBound(Datos) > 2 Then
                txt_FechaFin.Text = FechaAEspañol(Datos(3))
            Else
                txt_FechaFin.Text = ""
            End If
        End If

        Datos = Split(Filtros(7), "||")
        If UBound(Datos) > 0 Then
            txt_FinVigDe.Text = FechaAEspañol(Datos(1))
            If UBound(Datos) > 2 Then
                txt_FinVigA.Text = FechaAEspañol(Datos(3))
            Else
                txt_FinVigA.Text = ""
            End If

        End If

        chk_Reaseguro.Checked = CBool(Split(Filtros(8), "=")(1))
        txt_Contrato.Text = Replace(Split(Filtros(11), "=")(1), "|", "")
    End Sub

    Private Sub llenaGridFiltro(ByRef hid_Id As HiddenField, hid_Descripcion As HiddenField, ByVal dtDatos As DataTable, ByVal Sufijo As String, ByRef Link As LinkButton)

        Dim myRow() As Data.DataRow
        myRow = dtDatos.Select("Sufijo ='" & Sufijo & "'")

        For Each item In myRow
            hid_Id.Value = hid_Id.Value & IIf(Len(hid_Id.Value) > 0, ",", "") & item("Clave")
            hid_Descripcion.Value = hid_Descripcion.Value & IIf(Len(hid_Descripcion.Value) > 0, "|", "") & item("Descripcion")
        Next

        If Len(hid_Id.Value) > 0 Then
            Link.ForeColor = Drawing.Color.Red
        End If

    End Sub
    Private Function ConsultaCampos(ByVal cod_config As Integer) As DataTable
        Dim sCnn As String
        Dim sSel As String
        Dim dtCampos As DataTable
        dtCampos = GeneraDatatableCampos()

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        sSel = "spS_CamposReporte " & cod_config

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        For Each Row In dt.Rows
            dtCampos.Rows.Add(Row("Clave"), Row("Descripcion"), Row("posicion"), Row("nro_seccion"))
        Next

        Session("dtCampos") = dtCampos

        Return dtCampos

    End Function

    Private Function ColocaCampos(ByRef Control As CheckBoxList) As Boolean
        Dim cod_campo As Integer

        ColocaCampos = True

        For Each Item In Control.Items
            For Each Row In gvd_Configuracion.Rows
                cod_campo = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("lbl_Clave"), Label).Text
                If cod_campo = Item.value Then
                    Item.selected = True
                    Item.enabled = False
                    Exit For
                End If
            Next
        Next

    End Function

    Private Sub btn_Generar_Click(sender As Object, e As EventArgs) Handles btn_Generar.Click
        Try

            GenerarReporte(ObtieneFiltros())

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub GenerarReporte(ByVal Filtros As String)
        Dim Formato As String = ""

        If Master.Formato <> "NAV" Then
            Formato = "&rs%3AFormat=" & Master.Formato
        End If

        Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2fDistribucionReaseguro" & "&rc:Parameters=false" & "&cod_reporte=" & hid_codReporte.Value & Formato & Filtros
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirReporte", "ImprimirReporte('" & server & "');", True)
    End Sub

    Private Sub LlenaControlesCampos(ByVal cod_reporte As Integer)

        'clCatalogo = New Catalogo
        'chk_Poliza.DataValueField = "Clave"
        'chk_Poliza.DataTextField = "Descripcion"
        'chk_Poliza.DataSource = clCatalogo.ObtieneCatalogo("Fld", " WHERE cod_reporte = " & cod_reporte & " AND nro_seccion = 1")
        'chk_Poliza.DataBind()

        'chk_Reaseguros.DataValueField = "Clave"
        'chk_Reaseguros.DataTextField = "Descripcion"
        'chk_Reaseguros.DataSource = clCatalogo.ObtieneCatalogo("Fld", " WHERE cod_reporte = " & cod_reporte & " AND nro_seccion = 2")
        'chk_Reaseguros.DataBind()

        'chk_Ubicacion.DataValueField = "Clave"
        'chk_Ubicacion.DataTextField = "Descripcion"
        'chk_Ubicacion.DataSource = clCatalogo.ObtieneCatalogo("Fld", " WHERE cod_reporte = " & cod_reporte & " AND nro_seccion = 3")
        'chk_Ubicacion.DataBind()

        'chk_Cobertura.DataValueField = "Clave"
        'chk_Cobertura.DataTextField = "Descripcion"
        'chk_Cobertura.DataSource = clCatalogo.ObtieneCatalogo("Fld", " WHERE cod_reporte = " & cod_reporte & " AND nro_seccion = 4")
        'chk_Cobertura.DataBind()

    End Sub

    Private Function GeneraDatatableCampos() As DataTable
        Dim dtCuotas As DataTable
        dtCuotas = New DataTable

        dtCuotas.Columns.Add("Clave")
        dtCuotas.Columns.Add("Descripcion")
        dtCuotas.Columns.Add("posicion")
        dtCuotas.Columns.Add("nro_seccion")
        Return dtCuotas
    End Function

    Private Function FechaAIngles(ByVal Fecha As String) As String
        If Fecha <> vbNullString Then
            Return String.Format("{0:MM/dd/yyyy}", CDate(Fecha))
        Else
            Return ""
        End If

    End Function

    Private Function FechaAEspañol(ByVal Fecha As String) As String
        Dim Dato() As String = Split(Fecha, "/")
        If UBound(Dato) = 2 Then
            Return Dato(1) & "/" & Dato(0) & "/" & Dato(2)
        Else
            Return ""
        End If

    End Function

    'Protected Sub CambioSeleccion(sender As Object, e As EventArgs)
    '    Try
    '        For Each item In sender.items
    '            If item.selected = True And item.enabled = True Then
    '                AgregaCampo(item.value, item.text)
    '                item.enabled = False
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Mensaje("REPORTES-: ", ex.Message)
    '        LogError(ex.Message)
    '    End Try
    'End Sub

    Private Sub AgregaCampo(ByVal cod_campo As Integer, ByVal Campo As String, ByVal nro_seccion As Integer)
        Dim dtCampos As DataTable
        dtCampos = New DataTable
        dtCampos = Session("dtCampos")

        Dim myRow() As Data.DataRow
        myRow = dtCampos.Select("Clave ='" & cod_campo & "'")

        If myRow.Count = 0 Then
            dtCampos.Rows.Add(cod_campo, Campo, gvd_Configuracion.Rows.Count + 1, nro_seccion)
        End If

        LlenaGridConfiguracion(dtCampos, False)
    End Sub

    Private Sub QuitaCampo(ByVal cod_campo As Integer)
        Dim dtCampos As DataTable
        dtCampos = New DataTable
        dtCampos = Session("dtCampos")

        Dim myRow As Data.DataRow
        For i = dtCampos.Rows.Count - 1 To 0 Step -1
            myRow = dtCampos.Rows(i)
            If myRow("Clave") = cod_campo Then
                myRow.Delete()
            End If
        Next


        'If QuitaSeleccion(chk_Poliza, cod_campo) = False Then
        '    If QuitaSeleccion(chk_Reaseguros, cod_campo) = False Then
        '        If QuitaSeleccion(chk_Ubicacion, cod_campo) = False Then
        '            QuitaSeleccion(chk_Cobertura, cod_campo)
        '        End If
        '    End If
        'End If


        LlenaGridConfiguracion(dtCampos, False)
    End Sub

    'Private Function QuitaSeleccion(ByRef Seccion As CheckBoxList, ByVal cod_campo As Integer) As Boolean
    '    For Each item In Seccion.Items
    '        If item.value = cod_campo Or cod_campo = -1 Then
    '            item.selected = False
    '            item.enabled = True
    '            If cod_campo <> -1 Then
    '                Return True
    '            End If
    '        End If
    '    Next
    '    Return False
    'End Function

    Private Sub LimpiaControles()
        'QuitaSeleccion(chk_Poliza, -1)
        'QuitaSeleccion(chk_Reaseguros, -1)
        'QuitaSeleccion(chk_Ubicacion, -1)
        'QuitaSeleccion(chk_Cobertura, -1)

        hid_IdGenerales.Value = ""
        hid_Generales.Value = ""
        hid_IdReaseguro.Value = ""
        hid_Reaseguro.Value = ""
        hid_IdUbicaciones.Value = ""
        hid_Ubicaciones.Value = ""
        hid_IdCoberturas.Value = ""
        hid_Coberturas.Value = ""
        hid_IdSufijos.Value = ""
        hid_Sufijos.Value = ""
        hid_IdBroker.Value = ""
        hid_Broker.Value = ""
        hid_IdCompañia.Value = ""
        hid_Compañia.Value = ""
        hid_IdRamoContable.Value = ""
        hid_RamoContable.Value = ""
        hid_IdProducto.Value = ""
        hid_Producto.Value = ""
        txt_Contrato.Text = ""
        txt_FinVigDe.Text = ""
        txt_FinVigA.Text = ""

        lnk_Generales.ForeColor = Drawing.Color.DarkBlue
        lnk_Reaseguro.ForeColor = Drawing.Color.DarkBlue
        lnk_Ubicaciones.ForeColor = Drawing.Color.DarkBlue
        lnk_Coberturas.ForeColor = Drawing.Color.DarkBlue
        lnk_Poliza.ForeColor = Drawing.Color.DarkBlue
        lnk_Broker.ForeColor = Drawing.Color.DarkBlue
        lnk_Compañia.ForeColor = Drawing.Color.DarkBlue
        lnk_RamoCOntable.ForeColor = Drawing.Color.DarkBlue
        lnk_Producto.ForeColor = Drawing.Color.DarkBlue

        gvd_Configuracion.DataSource = Nothing
        gvd_Configuracion.DataBind()

        'gvd_Poliza.DataSource = Nothing
        'gvd_Poliza.DataBind()

        'gvd_Broker.DataSource = Nothing
        'gvd_Broker.DataBind()

        'gvd_Compañia.DataSource = Nothing
        'gvd_Compañia.DataBind()

        'gvd_RamoContable.DataSource = Nothing
        'gvd_RamoContable.DataBind()

        'gvd_Producto.DataSource = Nothing
        'gvd_Producto.DataBind()

        txt_FechaIni.Text = vbNullString
        txt_FechaFin.Text = vbNullString

        lst_Versiones.SelectedIndex = -1

        txt_descripcion.Text = ""
    End Sub

    Private Sub LlenaGridConfiguracion(ByVal dtDatos As DataTable, ByVal CambiaPosicion As Boolean)
        Dim intPosicion As Integer = 0

        Dim myRow() As Data.DataRow
        If CambiaPosicion = False Then
            For Each Row In dtDatos.Rows
                intPosicion = intPosicion + 1
                myRow = dtDatos.Select("posicion='" & Row("posicion") & "'")
                If myRow.Count > 0 Then
                    myRow(0)("posicion") = intPosicion
                End If
            Next

            Session("dtCampos") = dtDatos
            gvd_Configuracion.DataSource = dtDatos
            gvd_Configuracion.DataBind()
        Else
            Dim dtCampos As DataTable
            dtCampos = New DataTable
            dtCampos = GeneraDatatableCampos()
            For intPosicion = 1 To dtDatos.Rows.Count
                myRow = dtDatos.Select("posicion='" & intPosicion & "'")

                If myRow.Count > 0 Then
                    dtCampos.Rows.Add(myRow(0)("Clave"),
                                      myRow(0)("Descripcion"),
                                      intPosicion,
                                      myRow(0)("nro_seccion"))
                End If
            Next

            Session("dtCampos") = dtDatos
            gvd_Configuracion.DataSource = dtCampos
            gvd_Configuracion.DataBind()
        End If


        If gvd_Configuracion.Rows.Count > 0 Then
            Dim btn_Subir As ImageButton = TryCast(gvd_Configuracion.Rows(0).FindControl("btn_Subir"), ImageButton)
            Dim btn_Bajar As ImageButton = TryCast(gvd_Configuracion.Rows(gvd_Configuracion.Rows.Count - 1).FindControl("btn_Bajar"), ImageButton)

            btn_Subir.Enabled = False
            btn_Bajar.Enabled = False
        End If

    End Sub

    Private Sub gvd_Configuracion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Configuracion.RowCommand
        Try
            Dim dtCampos As DataTable
            dtCampos = New DataTable
            dtCampos = Session("dtCampos")

            If e.CommandName = "SubeElemento" Then
                Dim posicion As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("posicion")
                Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")

                Dim myRow() As Data.DataRow
                myRow = dtCampos.Select("posicion='" & posicion - 1 & "'")
                myRow(0)("posicion") = posicion

                myRow = dtCampos.Select("Clave='" & cod_campo & "' AND posicion='" & posicion & "'")
                myRow(0)("posicion") = posicion - 1

                LlenaGridConfiguracion(dtCampos, True)
            ElseIf e.CommandName = "BajaElemento" Then
                Dim posicion As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("posicion")
                Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")

                Dim myRow() As Data.DataRow
                myRow = dtCampos.Select("posicion='" & posicion + 1 & "'")
                myRow(0)("posicion") = posicion

                myRow = dtCampos.Select("Clave='" & cod_campo & "' AND posicion='" & posicion & "'")
                myRow(0)("posicion") = posicion + 1

                LlenaGridConfiguracion(dtCampos, True)
            ElseIf e.CommandName = "QuitarElemento" Then
                Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")
                QuitaCampo(cod_campo)
            End If

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_GuardarVer_Click(sender As Object, e As EventArgs) Handles btn_GuardarVer.Click
        Try
            Dim cod_config As Integer = 0

            If hid_version.Value = 0 Then
                cod_config = InsertaVersion(txt_descripcion.Text)
            Else
                cod_config = ActualizaVersion(hid_version.Value, txt_descripcion.Text)
            End If

            If cod_config > 0 Then
                Mensaje("REPORTES-: ", "Se ha almacenado la versión del Reporte")
                hid_version.Value = cod_config
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close", "ClosePopup('#VersionModal');", True)
            End If

            ObtieneVersiones()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub


    Private Sub lst_Versiones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lst_Versiones.SelectedIndexChanged
        Try
            Dim Filtros() As String
            Dim cod_campo As Integer
            Dim Descripcion As String
            Dim nro_seccion As Integer


            If lst_Versiones.SelectedIndex = -1 Then
                lst_Versiones.SelectedIndex = 0
            End If

            Dim strClave() = Split(lst_Versiones.SelectedValue, ";")

            If chk_Consultar.Checked = True Then

                LimpiaControles()

                hid_version.Value = strClave(0)

                Filtros = Split(strClave(1), "&")
                LlenaInfoFiltros(Filtros)

                Master.Formato = strClave(2)

                Session("dtCampos") = GeneraDatatableCampos()
                gvd_Configuracion.DataSource = ConsultaCampos(hid_version.Value)
                gvd_Configuracion.DataBind()

                For Each Row In gvd_Configuracion.Rows
                    cod_campo = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("lbl_Clave"), Label).Text
                    Descripcion = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("lbl_Desc"), Label).Text
                    nro_seccion = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("hid_Seccion"), HiddenField).Value

                    EstableceFiltro(cod_campo, Descripcion, nro_seccion)
                Next

                If Len(hid_IdGenerales.Value) > 0 Then
                    lnk_Generales.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdReaseguro.Value) > 0 Then
                    lnk_Reaseguro.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdUbicaciones.Value) > 0 Then
                    lnk_Ubicaciones.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdUbicaciones.Value) > 0 Then
                    lnk_Coberturas.ForeColor = Drawing.Color.Red
                End If
                'txt_descripcion.Text = lst_Versiones.SelectedItem.Text
            End If

            If chk_Generar.Checked = True Then
                GenerarReporte(strClave(1))
            End If

            lst_Versiones.SelectedIndex = -1
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub EstableceFiltro(ByVal cod_Campo As Integer, ByVal Descripcion As String, ByVal nro_seccion As Integer)
        Select Case nro_seccion
            Case 1
                hid_IdGenerales.Value = hid_IdGenerales.Value & IIf(Len(hid_IdGenerales.Value) > 0, ",", "") & cod_Campo
                hid_Generales.Value = hid_Generales.Value & IIf(Len(hid_Generales.Value) > 0, "|", "") & Descripcion
            Case 2
                hid_IdReaseguro.Value = hid_IdReaseguro.Value & IIf(Len(hid_IdReaseguro.Value) > 0, ",", "") & cod_Campo
                hid_Reaseguro.Value = hid_Reaseguro.Value & IIf(Len(hid_Reaseguro.Value) > 0, "|", "") & Descripcion
            Case 3
                hid_IdUbicaciones.Value = hid_IdUbicaciones.Value & IIf(Len(hid_IdUbicaciones.Value) > 0, ",", "") & cod_Campo
                hid_Ubicaciones.Value = hid_Ubicaciones.Value & IIf(Len(hid_Ubicaciones.Value) > 0, "|", "") & Descripcion
            Case 4
                hid_IdCoberturas.Value = hid_IdCoberturas.Value & IIf(Len(hid_IdCoberturas.Value) > 0, ",", "") & cod_Campo
                hid_Coberturas.Value = hid_Coberturas.Value & IIf(Len(hid_Coberturas.Value) > 0, "|", "") & Descripcion
        End Select
    End Sub

    'Private Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
    '    Try
    '        hid_version.Value = 0
    '        txt_descripcion.Text = vbNullString
    '        LimpiaControles()
    '        Session("dtCampos") = GeneraDatatableCampos()
    '    Catch ex As Exception
    '        Mensaje("REPORTES-: ", ex.Message)
    '        LogError(ex.Message)
    '    End Try
    'End Sub

    Public Sub Seleccionados(sender As Object, e As System.EventArgs)
        Try
            Dim dtSeleccionados As DataTable
            dtSeleccionados = New DataTable
            dtSeleccionados.Columns.Add("Clave")
            dtSeleccionados.Columns.Add("Descripcion")

            gvd_Seleccion.DataSource = Nothing
            gvd_Seleccion.DataBind()

            Dim Clave() As String
            Dim Descripcion() As String

            Dim hid_Id As HiddenField
            hid_Id = New HiddenField

            Dim hid_Descripcion As HiddenField
            hid_Descripcion = New HiddenField

            Select Case sender.text
                Case "Generales"
                    hid_Id = hid_IdGenerales
                    hid_Descripcion = hid_Generales
                    hid_Filtro.Value = "Fld1"

                Case "Reaseguro"
                    hid_Id = hid_IdReaseguro
                    hid_Descripcion = hid_Reaseguro
                    hid_Filtro.Value = "Fld2"

                Case "Ubicaciones"
                    hid_Id = hid_IdUbicaciones
                    hid_Descripcion = hid_Ubicaciones
                    hid_Filtro.Value = "Fld3"

                Case "Coberturas"
                    hid_Id = hid_IdCoberturas
                    hid_Descripcion = hid_Coberturas
                    hid_Filtro.Value = "Fld4"

                Case "Polizas"
                    hid_Id = hid_IdSufijos
                    hid_Descripcion = hid_Sufijos
                    hid_Filtro.Value = "Pol"

                Case "Brokers"
                    hid_Id = hid_IdBroker
                    hid_Descripcion = hid_Broker
                    hid_Filtro.Value = "Bro"

                Case "Compañias"
                    hid_Id = hid_IdCompañia
                    hid_Descripcion = hid_Compañia
                    hid_Filtro.Value = "Cia"

                Case "Ramos Contables"
                    hid_Id = hid_IdRamoContable
                    hid_Descripcion = hid_RamoContable
                    hid_Filtro.Value = "RamC"

                Case "Productos"
                    hid_Id = hid_IdProducto
                    hid_Descripcion = hid_Producto
                    hid_Filtro.Value = "Pro"
            End Select

            Clave = Split(hid_Id.Value, ",")
            Descripcion = Split(hid_Descripcion.Value, "|")

            For i = 0 To UBound(Clave)
                dtSeleccionados.Rows.Add(Clave(i), Descripcion(i))
            Next

            gvd_Seleccion.DataSource = dtSeleccionados
            gvd_Seleccion.DataBind()

            If Len(hid_Id.Value) > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Open", "OpenPopup('#SeleccionModal');", True)
            Else
                Mensaje("REPORTES-: ", "No ha seleccionado ningún elemento de este Concepto")
            End If

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Descartar_Click(sender As Object, e As EventArgs) Handles btn_Descartar.Click
        Try
            Dim Clave As String
            Dim Descripcion As String

            Dim hid_Id As HiddenField
            hid_Id = New HiddenField

            Dim hid_Descripcion As HiddenField
            hid_Descripcion = New HiddenField

            Dim btn_Sender As LinkButton
            btn_Sender = New LinkButton
            Dim Filtro As String = hid_Filtro.Value

            Select Case Filtro
                Case "Fld1"
                    hid_Id = hid_IdGenerales
                    hid_Descripcion = hid_Generales
                    btn_Sender = lnk_Generales

                Case "Fld2"
                    hid_Id = hid_IdReaseguro
                    hid_Descripcion = hid_Reaseguro
                    btn_Sender = lnk_Reaseguro

                Case "Fld3"
                    hid_Id = hid_IdUbicaciones
                    hid_Descripcion = hid_Ubicaciones
                    btn_Sender = lnk_Ubicaciones

                Case "Fld4"
                    hid_Id = hid_IdCoberturas
                    hid_Descripcion = hid_Coberturas
                    btn_Sender = lnk_Coberturas

                Case "Pol"
                    hid_Id = hid_IdSufijos
                    hid_Descripcion = hid_Sufijos
                    btn_Sender = lnk_Poliza

                Case "Bro"
                    hid_Id = hid_IdBroker
                    hid_Descripcion = hid_Broker
                    btn_Sender = lnk_Broker

                Case "Cia"
                    hid_Id = hid_IdCompañia
                    hid_Descripcion = hid_Compañia
                    btn_Sender = lnk_Compañia

                Case "RamC"
                    hid_Id = hid_IdRamoContable
                    hid_Descripcion = hid_RamoContable
                    btn_Sender = lnk_RamoCOntable

                Case "Pro"
                    hid_Id = hid_IdProducto
                    hid_Descripcion = hid_Producto
                    btn_Sender = lnk_Producto
            End Select

            hid_Id.Value = ""
            hid_Descripcion.Value = ""

            For Each Row In gvd_Seleccion.Rows
                Clave = TryCast(gvd_Seleccion.Rows(Row.rowIndex).FindControl("lbl_Clave"), Label).Text
                Descripcion = TryCast(gvd_Seleccion.Rows(Row.rowIndex).FindControl("lbl_Descripcion"), Label).Text
                If TryCast(gvd_Seleccion.Rows(Row.rowIndex).FindControl("chk_Cat"), CheckBox).Checked = False Then
                    hid_Id.Value = hid_Id.Value & IIf(Len(hid_Id.Value) > 0, ",", "") & Clave
                    hid_Descripcion.Value = hid_Descripcion.Value & IIf(Len(hid_Descripcion.Value) > 0, "|", "") & Descripcion
                Else
                    If Filtro = "Fld1" Or Filtro = "Fld2" Or Filtro = "Fld3" Or Filtro = "Fld4" Then
                        QuitaCampo(Clave)
                    End If
                End If
            Next

            If Len(hid_Id.Value) = 0 Then
                btn_Sender.ForeColor = Drawing.Color.DarkBlue
            End If

            Seleccionados(btn_Sender, Nothing)

            If hid_Id.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close", "ClosePopup('#SeleccionModal');", True)
            End If

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Nuevo_Click(sender As Object, e As EventArgs) Handles btn_Nuevo.Click
        Try
            hid_version.Value = 0
            txt_descripcion.Text = vbNullString
            LimpiaControles()
            Session("dtCampos") = GeneraDatatableCampos()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

End Class







