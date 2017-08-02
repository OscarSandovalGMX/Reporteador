Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net.Mail

Partial Class Pages_Reporteador
    Inherits System.Web.UI.Page

    Private clCatalogo As Catalogo
    Private SelEndosos() As String
    Private dtFormula As DataTable
    Private dtResultado As DataTable
    Private trRep As SqlTransaction
    Private Enum EnumEstado As Integer
        Nuevo = 0
        Cambio = 1
    End Enum

    Private Sub Pages_DistReaseguro_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.Timeout = 300

            Master.Titulo = "Reporteador por Area"
            Session.Add("blnExpiro", "1")
            Session.Add("SelEndosos", SelEndosos)
            Session.Add("dtResultado", Nothing)

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
            LlenaColumnasAll()
            BindDummyRow()
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

    Private Sub LlenaListado(ByRef chk_Listado As CheckBoxList, ByVal myRow() As Data.DataRow)
        chk_Listado.Items.Clear()
        For Each item In myRow
            chk_Listado.Items.Add(item("Clave") & "~" & item("Descripcion"))
        Next
    End Sub
    Private Sub LlenaColumnasAll()
        Dim dtColumnas As DataTable
        dtColumnas = New DataTable

        clCatalogo = New Catalogo
        dtColumnas = clCatalogo.ObtieneCatalogo("Rep", " WHERE cod_reporte IN(" & hid_codReporte.Value & ") AND cod_seccion IN (1,2,3,4,5,6)", "")

        LlenaListado(chk_AllGenerales, dtColumnas.Select("Adicional ='" & 1 & "' AND Clave NOT IN(1,21,30,52,54)"))
        LlenaListado(chk_AllReaseguro, dtColumnas.Select("Adicional ='" & 2 & "' AND Clave NOT IN(1,21,30,52,54)"))
        LlenaListado(chk_AllSiniestros, dtColumnas.Select("Adicional ='" & 3 & "' AND Clave NOT IN(1,21,30,52,54)"))
        LlenaListado(chk_AllCumulos, dtColumnas.Select("Adicional ='" & 4 & "' AND Clave NOT IN(1,21,30,52,54)"))
        LlenaListado(chk_AllCobranzas, dtColumnas.Select("Adicional ='" & 5 & "' AND Clave NOT IN(1,21,30,52,54)"))
        LlenaListado(chk_AllContanbilidad, dtColumnas.Select("Adicional ='" & 6 & "' AND Clave NOT IN(1,21,30,52,54)"))
    End Sub

    Private Sub LlenaColumnasFor()
        Dim dtColumnas As DataTable
        dtColumnas = New DataTable

        clCatalogo = New Catalogo
        dtColumnas = clCatalogo.ObtieneCatalogo("Rep", " WHERE cod_reporte IN(" & hid_codReporte.Value & ") AND cod_seccion IN (1,2,3,4,5,6)", "")

        LlenaListado(chk_ForGenerales, dtColumnas.Select("Adicional ='" & 1 & "'"))
        LlenaListado(chk_ForReaseguro, dtColumnas.Select("Adicional ='" & 2 & "'"))
        LlenaListado(chk_ForSiniestros, dtColumnas.Select("Adicional ='" & 3 & "'"))
        LlenaListado(chk_ForCumulos, dtColumnas.Select("Adicional ='" & 4 & "'"))
        LlenaListado(chk_ForCobranzas, dtColumnas.Select("Adicional ='" & 5 & "'"))
        LlenaListado(chk_ForContabilidad, dtColumnas.Select("Adicional ='" & 6 & "'"))
    End Sub

    Private Sub ObtieneVersiones()
        clCatalogo = New Catalogo
        gvd_Versiones.DataSource = clCatalogo.ObtieneVersiones(hid_codReporte.Value, -1, "")
        gvd_Versiones.DataBind()
    End Sub

    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("Clave")
        dummy.Columns.Add("Descripcion")

        dummy.Rows.Add()
        gvd_Catalogo.DataSource = dummy
        gvd_Catalogo.DataBind()
    End Sub

    Private Function ObtieneFiltros(ByVal blnGuardar As Boolean) As String
        'Dim TipoSalida As String = "&TipoSalida=1"
        Dim FiltroBroker As String = "&FiltroBroker="
        Dim FiltroCia As String = "&FiltroCia="
        Dim FiltroPol As String = "&FiltroPol="
        Dim FiltroRamoCont As String = "&FiltroRamoCont="
        Dim FiltroRamoTec As String = "&FiltroRamoTec="
        Dim FiltroFecha As String = "&FecEmision="
        Dim FiltroFechaVig As String = "&FecVigencia="
        'Dim FiltroColumnas As String = "&NumCols="
        Dim FiltroSeleccion As String = "&strFields="
        Dim strCampo As String = ""
        Dim strCondicion As String = ""
        Dim condicion As String = ""
        Dim Parametro As String = ""

        Dim sn_Temporal As String = "&sn_Temporal=" & hid_Temporal.Value
        Dim cod_reporte As String = "&cod_reporte=" & hid_codReporte.Value
        Dim cod_config As String = "&cod_config=" & hid_version.Value
        Dim FiltroConsulta As String = "&FiltroConsulta="



        FiltroBroker = FiltroBroker & hid_IdBroker.Value
        FiltroCia = FiltroCia & hid_IdCompañia.Value
        FiltroPol = FiltroPol & IIf(Len(hid_IdSufijos.Value) > 0, "|", "") & Replace(hid_IdSufijos.Value, ",", "|,|") & IIf(Len(hid_IdSufijos.Value) > 0, "|", "")
        FiltroRamoCont = FiltroRamoCont & hid_IdRamoContable.Value
        FiltroRamoTec = FiltroRamoTec & hid_IdProducto.Value

        If IsDate(txt_FechaIni.Text) Then
            FiltroFecha = FiltroFecha & " pvh.fec_emi >= ||" & FechaAIngles(txt_FechaIni.Text) & "||"
            If IsDate(txt_FechaFin.Text) Then
                FiltroFecha = FiltroFecha & " And pvh.fec_emi <= ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "||"
            End If
        Else
            If IsDate(txt_FechaFin.Text) Then
                FiltroFecha = FiltroFecha & " pvh.fec_emi <= ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FechaFin.Text))) & "||"
            End If
        End If

        If IsDate(txt_FinVigDe.Text) Then
            FiltroFechaVig = FiltroFechaVig & " pvh.fec_vig_hasta >= ||" & FechaAIngles(txt_FinVigDe.Text) & "||"
            If IsDate(txt_FinVigA.Text) Then
                FiltroFechaVig = FiltroFechaVig & " And fec_vig_hasta <= ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FinVigA.Text))) & "||"
            End If
        Else
            If IsDate(txt_FinVigA.Text) Then
                FiltroFechaVig = FiltroFechaVig & " pvh.fec_vig_hasta <= ||" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txt_FinVigA.Text))) & "||"
            End If
        End If


        FiltroSeleccion = FiltroSeleccion & hid_IdSeccion1.Value &
                          IIf(Len(hid_IdSeccion2.Value) > 0, ",", "") & hid_IdSeccion2.Value &
                          IIf(Len(hid_IdSeccion3.Value) > 0, ",", "") & hid_IdSeccion3.Value &
                          IIf(Len(hid_IdSeccion4.Value) > 0, ",", "") & hid_IdSeccion4.Value &
                          IIf(Len(hid_IdSeccion5.Value) > 0, ",", "") & hid_IdSeccion5.Value

        'If Len(FiltroSeleccion) > 0 Then
        '    FiltroColumnas = FiltroColumnas & UBound(Split(FiltroSeleccion, ",")) + 1
        'Else
        '    FiltroColumnas = FiltroColumnas & 165
        'End If

        For Each row In gvd_Consulta.Rows
            Dim hid_Codigo As HiddenField = DirectCast(row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(row.FindControl("lbl_Columna"), Label)
            Dim ddl_Union As DropDownList = DirectCast(row.FindControl("ddl_Union"), DropDownList)
            Dim ddl_Operador As DropDownList = DirectCast(row.FindControl("ddl_Operador"), DropDownList)
            Dim txt_Condicion As TextBox = DirectCast(row.FindControl("txt_Condicion"), TextBox)

            If ddl_Operador.Enabled = True Then
                strCampo = strCampo & IIf(Len(strCampo) > 0, ",", "") & hid_Codigo.Value

                If Len(strCondicion) > 0 And row.RowIndex > 0 Then
                    strCondicion = strCondicion & " " & ddl_Union.SelectedValue & " "
                Else
                    strCondicion = " "
                End If

                strCondicion = strCondicion & "[Field" & hid_Codigo.Value & "] "

                If IsDate(Replace(txt_Condicion.Text, "''", "")) Then
                    Parametro = "''" & FechaAIngles(Replace(txt_Condicion.Text, "''", "")) & "''"
                Else
                    Parametro = txt_Condicion.Text
                End If

                Select Case ddl_Operador.SelectedValue
                    Case "LIKE @%", "LIKE %@", "LIKE %@%"
                        condicion = Replace(Replace(Replace(ddl_Operador.SelectedValue, "@", Replace(Parametro, "''", "")), "", ""), "LIKE ", "LIKE ''") & "''"
                    Case "IN(@)", "NOT IN(@)"
                        condicion = Replace(Replace(ddl_Operador.SelectedValue, "@", Parametro), "", "")
                    Case Else
                        condicion = ddl_Operador.SelectedValue & Parametro
                End Select
                strCondicion = strCondicion & condicion
            End If
        Next
        If Len(strCampo) > 0 Then
            FiltroSeleccion = FiltroSeleccion & IIf(FiltroSeleccion = "&strFields=", "", ",") & strCampo
        End If

        FiltroConsulta = FiltroConsulta & Replace(DetalleColumnas(strCondicion, strCampo), "'", "|")

        strCampo = ""
        For Each row In gvd_Formulas.Rows
            Dim nombre As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Columna"), TextBox).Text
            Dim formula As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Formula"), TextBox).Text
            strCampo = strCampo & IIf(Len(strCampo) > 0, ",", "") & "=" & formula
        Next

        If blnGuardar = False Then
            Return FiltroBroker & FiltroCia & FiltroPol & FiltroRamoCont & FiltroRamoTec & FiltroFecha & FiltroFechaVig & FiltroSeleccion & sn_Temporal & cod_reporte & cod_config & FiltroConsulta
        Else
            Return FiltroBroker & FiltroCia & FiltroPol & FiltroRamoCont & FiltroRamoTec & FiltroFecha & FiltroFechaVig & FiltroSeleccion & FiltroConsulta
        End If

    End Function

    Private Function DetalleColumnas(ByVal Condicion As String, ByVal strCampo As String) As String
        Dim Campo As String
        Dim Consulta As Catalogo
        Consulta = New Catalogo

        dtResultado = New DataTable
        dtResultado = Consulta.ObtieneDetalleCampo(strCampo)

        For Each Row In dtResultado.Rows
            Campo = Replace(Replace(Row("campo"), "'", "''"), "+", ":")
            Condicion = Replace(Condicion, "[Field" & Row("cod_campo") & "]", Campo)
        Next

        Return Condicion
    End Function

    Private Function InsertaVersion(ByVal descripcion As String) As Integer
        Dim cod_config As Integer = 0
        Dim cod_agrupacion As Integer = 0
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Comando As SqlClient.SqlCommand
        Dim strCampos As String = ""
        Dim Resultado As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        conn.Open()
        trRep = conn.BeginTransaction()
        Try
            sSel = "EXEC spI_VersionReporte " & hid_codReporte.Value & ",'" & Master.cod_usuario & "','" & descripcion & "','" & ObtieneFiltros(True) & "','" & Master.Formato & "'," & IIf(chk_Temporal.Checked = True, -1, 0)

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


            'AGRUPACIONES
            If gvd_Filas.Rows.Count > 0 Or gvd_Valores.Rows.Count > 0 Or gvd_Filtros.Rows.Count > 0 Then
                InsertaAgrupacion(conn, sCnn)
            End If

            'CONSULTAS
            strCampos = ""
            For Each row In gvd_Consulta.Rows
                Dim cod_campo As Integer = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
                Dim union As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("ddl_Union"), DropDownList).SelectedValue
                Dim operador As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("ddl_Operador"), DropDownList).SelectedValue
                Dim consulta As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("txt_Condicion"), TextBox).Text


                If Not IsNumeric(consulta) Then
                    consulta = Replace(consulta, "''", "''''''''")
                End If

                strCampos = strCampos & "(@strKey," & row.RowIndex + 1 & "," & cod_campo & ",''" & union & "'',''" & operador & "'',''" & consulta & "''),"
            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "EXEC spI_OfGread 'cCON_Consultas','" & hid_codReporte.Value & "," & cod_config & "','" & strCampos & "'"
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Resultado = Convert.ToInt32(Comando.ExecuteScalar())


            'FORMULAS
            strCampos = ""
            For Each row In gvd_Formulas.Rows
                Dim nombre As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Columna"), TextBox).Text
                Dim formula As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Formula"), TextBox).Text
                strCampos = strCampos & "(@strKey," & row.RowIndex + 1 & ",''" & nombre & "'',''" & formula & "''),"
            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "EXEC spI_OfGread 'cFOR_Formulas','" & hid_codReporte.Value & "," & cod_config & "','" & strCampos & "'"
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
        Dim cod_agrupacion As Integer = 0
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Comando As SqlClient.SqlCommand
        Dim strCampos As String = ""
        Dim Resultado As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        conn.Open()
        trRep = conn.BeginTransaction()
        Try
            sSel = "EXEC spU_VersionReporte " & cod_config & "," & hid_codReporte.Value & ",'" & Master.cod_usuario & "','" & descripcion & "','" & ObtieneFiltros(True) & "','" & Master.Formato & "'," & IIf(chk_Temporal.Checked = True, -1, 0)

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

            'AGRUPACIONES
            If gvd_Filas.Rows.Count > 0 Or gvd_Valores.Rows.Count > 0 Or gvd_Filtros.Rows.Count > 0 Then
                InsertaAgrupacion(conn, sCnn)
            End If

            'CONSULTAS
            strCampos = ""
            For Each row In gvd_Consulta.Rows
                Dim cod_campo As Integer = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
                Dim union As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("ddl_Union"), DropDownList).SelectedValue
                Dim operador As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("ddl_Operador"), DropDownList).SelectedValue
                Dim consulta As String = TryCast(gvd_Consulta.Rows(row.rowIndex).FindControl("txt_Condicion"), TextBox).Text
                'strCampos = strCampos & "(@strKey," & row.RowIndex + 1 & "," & cod_campo & ",''" & union & "'',''" & operador & "'',''" & consulta & "''),"

                If Not IsNumeric(consulta) Then
                    consulta = Replace(consulta, "''", "''''''''")
                End If

                strCampos = strCampos & "(@strKey," & row.RowIndex + 1 & "," & cod_campo & ",''" & union & "'',''" & operador & "'',''" & consulta & "''),"

            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "DELETE FROM cCON_Consultas WHERE cod_reporte =" & hid_codReporte.Value & " AND cod_config =" & cod_config
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Convert.ToInt32(Comando.ExecuteScalar())

            sSel = "EXEC spI_OfGread 'cCON_Consultas','" & hid_codReporte.Value & "," & cod_config & "','" & strCampos & "'"
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Resultado = Convert.ToInt32(Comando.ExecuteScalar())

            'FORMULAS
            strCampos = ""
            For Each row In gvd_Formulas.Rows
                Dim nombre As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Columna"), TextBox).Text
                Dim formula As String = TryCast(gvd_Formulas.Rows(row.rowIndex).FindControl("txt_Formula"), TextBox).Text
                strCampos = strCampos & "(@strKey," & row.RowIndex + 1 & ",''" & nombre & "'',''" & formula & "''),"
            Next

            If Len(strCampos) > 0 Then
                strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
            End If

            sSel = "DELETE FROM cFOR_Formulas WHERE cod_reporte =" & hid_codReporte.Value & " AND cod_config =" & cod_config
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trRep
            Convert.ToInt32(Comando.ExecuteScalar())

            sSel = "EXEC spI_OfGread 'cFOR_Formulas','" & hid_codReporte.Value & "," & cod_config & "','" & strCampos & "'"
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
            Dim cod_campo As Integer = 0
            Dim campo As String = ""
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
                        hid_Id = hid_IdSeccion1
                        hid_Descripcion = hid_Generales
                        lnk_Generales.ForeColor = Drawing.Color.Red
                        nro_seccion = 1
                        AgregaColumna(chk_Generales, Seleccionados)

                    Case "Fld2"
                        hid_Id = hid_IdSeccion2
                        hid_Descripcion = hid_Reaseguro
                        lnk_Reaseguro.ForeColor = Drawing.Color.Red
                        nro_seccion = 2
                        AgregaColumna(chk_Reaseguro, Seleccionados)

                    Case "Fld3"
                        hid_Id = hid_IdSeccion3
                        hid_Descripcion = hid_Siniestros
                        lnk_Siniestros.ForeColor = Drawing.Color.Red
                        nro_seccion = 3
                        AgregaColumna(chk_Siniestros, Seleccionados)

                    Case "Fld4"
                        hid_Id = hid_IdSeccion4
                        hid_Descripcion = hid_Cumulos
                        lnk_Cumulos.ForeColor = Drawing.Color.Red
                        nro_seccion = 4
                        AgregaColumna(chk_Cumulos, Seleccionados)

                    Case "Fld5"
                        hid_Id = hid_IdSeccion5
                        hid_Descripcion = hid_Cobranzass
                        lnk_Cobranzas.ForeColor = Drawing.Color.Red
                        nro_seccion = 5
                        AgregaColumna(chk_Cobranzas, Seleccionados)

                    Case "Bro"
                        hid_Id = hid_IdBroker
                        hid_Descripcion = hid_Broker
                        lnk_Broker.ForeColor = Drawing.Color.Red
                        cod_campo = 52
                        campo = "COD BROKER"

                    Case "Cia"
                        hid_Id = hid_IdCompañia
                        hid_Descripcion = hid_Compañia
                        lnk_Compañia.ForeColor = Drawing.Color.Red
                        cod_campo = 54
                        campo = "COD CIA"

                    Case "RamC"
                        hid_Id = hid_IdRamoContable
                        hid_Descripcion = hid_RamoContable
                        lnk_RamoCOntable.ForeColor = Drawing.Color.Red
                        cod_campo = 21
                        campo = "COD RAMO CONTABLE"

                    Case "Pro"
                        hid_Id = hid_IdProducto
                        hid_Descripcion = hid_Producto
                        lnk_Producto.ForeColor = Drawing.Color.Red
                        cod_campo = 30
                        campo = "COD RAMO COMERCIAL"

                    Case "RamU"
                        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                        txtClaveRam.Text = Split(Datos(0), "~")(0)
                        txtSearchRam.Text = Split(Datos(0), "~")(1)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Catalogo", "ClosePopup('#CatalogoModal');", True)
                        Exit Sub
                End Select

                Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                For Each dato In Datos
                    hid_Id.Value = hid_Id.Value & IIf(Len(hid_Id.Value) > 0, ",", "") & Split(dato, "~")(0)
                    hid_Descripcion.Value = hid_Descripcion.Value & IIf(Len(hid_Descripcion.Value) > 0, "|", "") & Split(dato, "~")(1)

                    If Catalogo = "Fld1" Or Catalogo = "Fld2" Or Catalogo = "Fld3" Or Catalogo = "Fld4" Or Catalogo = "Fld5" Then
                        AgregaCampo(Split(dato, "~")(0), Split(dato, "~")(1), nro_seccion)
                    End If
                Next

                If Catalogo = "Bro" Or Catalogo = "Cia" Or Catalogo = "RamC" Or Catalogo = "Pro" Then
                    AgregaConsulta(cod_campo, campo, hid_Id)
                End If

            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Catalogo", "ClosePopup('#CatalogoModal');", True)

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

                    AgregaConsulta(1, "LLAVE-POL", hid_IdSufijos)
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

    End Sub

    Private Function FechaAEspañol(ByVal Fecha As String) As String
        Dim Dato() As String = Split(Fecha, "/")
        If UBound(Dato) = 2 Then
            Return Dato(1) & "/" & Dato(0) & "/" & Dato(2)
        Else
            Return ""
        End If

    End Function

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
        sSel = "spS_CamposReporte " & cod_config & "," & hid_codReporte.Value

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
            GenerarReporte(ObtieneFiltros(False))

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Actualizar_Click(sender As Object, e As EventArgs) Handles btn_Actualizar.Click
        Try
            GenerarReporte(ObtieneFiltros(False))

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

        Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2fReporteadorBI" & "&rc:Parameters=false" & Formato & Filtros
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirReporte", "ImprimirReporte('" & server & "');", True)
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


    Private Sub AgregaColumna(ByRef chkColumnas As CheckBoxList, ByVal Seleccionados As String)
        Dim Datos() As String

        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
        For Each dato In Datos
            chkColumnas.Items.Add(dato)
        Next
    End Sub

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

    Private Sub AgregaConsulta(ByVal cod_campo As Integer, ByVal campo As String, ByRef hid_Id As HiddenField)
        Dim dtSeleccion As DataTable
        dtSeleccion = New DataTable
        dtSeleccion.Columns.Add("cod_campo")
        dtSeleccion.Columns.Add("Columna")
        dtSeleccion.Columns.Add("Seccion")
        dtSeleccion.Columns.Add("Union")
        dtSeleccion.Columns.Add("Operador")
        dtSeleccion.Columns.Add("Condicion")

        For Each Row In gvd_Consulta.Rows
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            If cod_campo <> hid_Codigo.Value Then
                Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
                Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)

                Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
                Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
                Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)
                dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, txt_Condicion.Text)
            End If
        Next
        dtSeleccion.Rows.Add(cod_campo, campo, 0, "AND", "IN(@)", hid_Id.Value)
        gvd_Consulta.DataSource = dtSeleccion
        gvd_Consulta.DataBind()
    End Sub

    Private Sub QuitaElemento(ByVal cod_campo As Integer)
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

        LlenaGridConfiguracion(dtCampos, False)
    End Sub

    Private Sub LimpiaControles()
        hid_IdSeccion1.Value = ""
        hid_Generales.Value = ""
        hid_IdSeccion2.Value = ""
        hid_Reaseguro.Value = ""
        hid_IdSeccion3.Value = ""
        hid_Siniestros.Value = ""
        hid_IdSeccion4.Value = ""
        hid_Cumulos.Value = ""
        hid_IdSeccion5.Value = ""
        hid_Cobranzass.Value = ""
        hid_IdSeccion6.Value = ""
        hid_Contabilidad.Value = ""
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

        lnk_Generales.ForeColor = Drawing.Color.DarkBlue
        lnk_Reaseguro.ForeColor = Drawing.Color.DarkBlue
        lnk_Siniestros.ForeColor = Drawing.Color.DarkBlue
        lnk_Cumulos.ForeColor = Drawing.Color.DarkBlue
        lnk_Cobranzas.ForeColor = Drawing.Color.DarkBlue
        lnk_Contabilidad.ForeColor = Drawing.Color.DarkBlue

        lnk_Poliza.ForeColor = Drawing.Color.DarkBlue
        lnk_Broker.ForeColor = Drawing.Color.DarkBlue
        lnk_Compañia.ForeColor = Drawing.Color.DarkBlue
        lnk_RamoCOntable.ForeColor = Drawing.Color.DarkBlue
        lnk_Producto.ForeColor = Drawing.Color.DarkBlue

        gvd_Configuracion.DataSource = Nothing
        gvd_Configuracion.DataBind()

        gvd_Formulas.DataSource = Nothing
        gvd_Formulas.DataBind()

        ddl_seccion.SelectedValue = "Ge"
        ddl_seccion_SelectedIndexChanged(Me, Nothing)

        chk_Generales.Items.Clear()
        chk_Siniestros.Items.Clear()
        chk_Siniestros.Items.Clear()
        chk_Cumulos.Items.Clear()
        chk_Cobranzas.Items.Clear()
        chk_Contabilidad.Items.Clear()
        chk_Listado.Items.Clear()

        ddl_Agrupaciones.Items.Clear()

        gvd_Filas.DataSource = Nothing
        gvd_Filas.DataBind()

        gvd_Valores.DataSource = Nothing
        gvd_Valores.DataBind()

        gvd_Filtros.DataSource = Nothing
        gvd_Filtros.DataBind()

        txt_FechaIni.Text = vbNullString
        txt_FechaFin.Text = vbNullString

        txt_FinVigDe.Text = vbNullString
        txt_FinVigA.Text = vbNullString

        gvd_Consulta.DataSource = Nothing
        gvd_Consulta.DataBind()

        'lst_Versiones.SelectedIndex = -1
        txt_descripcion.Text = ""
        chk_Temporal.Checked = False
        hid_Temporal.Value = 0
        btn_Actualizar.Enabled = False

        hid_version.Value = 0
        lbl_DescVersion.Text = "Nueva Versión"

        Session("dtCampos") = GeneraDatatableCampos()

        LlenaColumnasAll()
        LlenaColumnasFor()
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


        'If gvd_Configuracion.Rows.Count > 0 Then
        '    Dim btn_Subir As ImageButton = TryCast(gvd_Configuracion.Rows(0).FindControl("btn_Subir"), ImageButton)
        '    Dim btn_Bajar As ImageButton = TryCast(gvd_Configuracion.Rows(gvd_Configuracion.Rows.Count - 1).FindControl("btn_Bajar"), ImageButton)

        '    btn_Subir.Enabled = False
        '    btn_Bajar.Enabled = False
        'End If

    End Sub

    'Private Sub gvd_Configuracion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Configuracion.RowCommand
    '    Try
    '        Dim dtCampos As DataTable
    '        dtCampos = New DataTable
    '        dtCampos = Session("dtCampos")

    '        If e.CommandName = "SubeElemento" Then
    '            Dim posicion As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("posicion")
    '            Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")

    '            Dim myRow() As Data.DataRow
    '            myRow = dtCampos.Select("posicion='" & posicion - 1 & "'")
    '            myRow(0)("posicion") = posicion

    '            myRow = dtCampos.Select("Clave='" & cod_campo & "' AND posicion='" & posicion & "'")
    '            myRow(0)("posicion") = posicion - 1

    '            LlenaGridConfiguracion(dtCampos, True)
    '        ElseIf e.CommandName = "BajaElemento" Then
    '            Dim posicion As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("posicion")
    '            Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")

    '            Dim myRow() As Data.DataRow
    '            myRow = dtCampos.Select("posicion='" & posicion + 1 & "'")
    '            myRow(0)("posicion") = posicion

    '            myRow = dtCampos.Select("Clave='" & cod_campo & "' AND posicion='" & posicion & "'")
    '            myRow(0)("posicion") = posicion + 1

    '            LlenaGridConfiguracion(dtCampos, True)
    '        ElseIf e.CommandName = "QuitarElemento" Then
    '            Dim cod_campo As Integer = gvd_Configuracion.DataKeys(e.CommandSource.NamingContainer.RowIndex)("Clave")
    '            QuitaCampo(cod_campo)
    '        End If

    '    Catch ex As Exception
    '        Mensaje("REPORTES-: ", ex.Message)
    '        LogError(ex.Message)
    '    End Try
    'End Sub

    Private Sub btn_GuardarVer_Click(sender As Object, e As EventArgs) Handles btn_GuardarVer.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Open", "OpenPopup('#VersionModal');", True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub



    Private Sub EstableceFiltro(ByVal cod_Campo As Integer, ByVal Descripcion As String, ByVal nro_seccion As Integer)
        Select Case nro_seccion
            Case 1
                hid_IdSeccion1.Value = hid_IdSeccion1.Value & IIf(Len(hid_IdSeccion1.Value) > 0, ",", "") & cod_Campo
                hid_Generales.Value = hid_Generales.Value & IIf(Len(hid_Generales.Value) > 0, "|", "") & Descripcion
            Case 2
                hid_IdSeccion2.Value = hid_IdSeccion2.Value & IIf(Len(hid_IdSeccion2.Value) > 0, ",", "") & cod_Campo
                hid_Reaseguro.Value = hid_Reaseguro.Value & IIf(Len(hid_Reaseguro.Value) > 0, "|", "") & Descripcion
            Case 3
                hid_IdSeccion3.Value = hid_IdSeccion3.Value & IIf(Len(hid_IdSeccion3.Value) > 0, ",", "") & cod_Campo
                hid_Siniestros.Value = hid_Siniestros.Value & IIf(Len(hid_Siniestros.Value) > 0, "|", "") & Descripcion
            Case 4
                hid_IdSeccion4.Value = hid_IdSeccion4.Value & IIf(Len(hid_IdSeccion4.Value) > 0, ",", "") & cod_Campo
                hid_Cumulos.Value = hid_Cumulos.Value & IIf(Len(hid_Cumulos.Value) > 0, "|", "") & Descripcion
            Case 5
                hid_IdSeccion5.Value = hid_IdSeccion5.Value & IIf(Len(hid_IdSeccion5.Value) > 0, ",", "") & cod_Campo
                hid_Cobranzass.Value = hid_Cobranzass.Value & IIf(Len(hid_Cobranzass.Value) > 0, "|", "") & Descripcion
        End Select
    End Sub


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
                    hid_Id = hid_IdSeccion1
                    hid_Descripcion = hid_Generales
                    hid_Filtro.Value = "Fld1"

                Case "Reaseguro"
                    hid_Id = hid_IdSeccion2
                    hid_Descripcion = hid_Reaseguro
                    hid_Filtro.Value = "Fld2"

                Case "Siniestros"
                    hid_Id = hid_IdSeccion3
                    hid_Descripcion = hid_Siniestros
                    hid_Filtro.Value = "Fld3"

                Case "Cúmulos"
                    hid_Id = hid_IdSeccion4
                    hid_Descripcion = hid_Cumulos
                    hid_Filtro.Value = "Fld4"

                Case "Cobranzas"
                    hid_Id = hid_IdSeccion5
                    hid_Descripcion = hid_Cobranzass
                    hid_Filtro.Value = "Fld5"

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
            Dim cod_campo As Integer = 0
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
                    hid_Id = hid_IdSeccion1
                    hid_Descripcion = hid_Generales
                    btn_Sender = lnk_Generales

                Case "Fld2"
                    hid_Id = hid_IdSeccion2
                    hid_Descripcion = hid_Reaseguro
                    btn_Sender = lnk_Reaseguro

                Case "Fld3"
                    hid_Id = hid_IdSeccion3
                    hid_Descripcion = hid_Siniestros
                    btn_Sender = lnk_Siniestros

                Case "Fld4"
                    hid_Id = hid_IdSeccion4
                    hid_Descripcion = hid_Cumulos
                    btn_Sender = lnk_Cumulos

                Case "Fld5"
                    hid_Id = hid_IdSeccion5
                    hid_Descripcion = hid_Cobranzass
                    btn_Sender = lnk_Cobranzas

                Case "Pol"
                    hid_Id = hid_IdSufijos
                    hid_Descripcion = hid_Sufijos
                    btn_Sender = lnk_Poliza
                    cod_campo = 1

                Case "Bro"
                    hid_Id = hid_IdBroker
                    hid_Descripcion = hid_Broker
                    btn_Sender = lnk_Broker
                    cod_campo = 52

                Case "Cia"
                    hid_Id = hid_IdCompañia
                    hid_Descripcion = hid_Compañia
                    btn_Sender = lnk_Compañia
                    cod_campo = 54

                Case "RamC"
                    hid_Id = hid_IdRamoContable
                    hid_Descripcion = hid_RamoContable
                    btn_Sender = lnk_RamoCOntable
                    cod_campo = 21

                Case "Pro"
                    hid_Id = hid_IdProducto
                    hid_Descripcion = hid_Producto
                    btn_Sender = lnk_Producto
                    cod_campo = 30
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
                    If Filtro = "Fld1" Or Filtro = "Fld2" Or Filtro = "Fld3" Or Filtro = "Fld4" Or Filtro = "Fld5" Then
                        QuitaElemento(Clave)
                    End If
                End If
            Next

            If Len(hid_Id.Value) = 0 Then
                btn_Sender.ForeColor = Drawing.Color.DarkBlue
            End If

            Seleccionados(btn_Sender, Nothing)

            If hid_Id.Value = "" Then
                If cod_campo > 0 Then
                    QuitaCampos(gvd_Consulta, True, False, cod_campo)
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close Seleccion", "ClosePopup('#SeleccionModal');", True)
            Else
                If cod_campo > 0 Then
                    ActualizaCondiciones(cod_campo, hid_Id.Value)
                End If
            End If

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub ActualizaCondiciones(ByVal cod_campo As Integer, ByVal Condiciones As String)
        For Each Row In gvd_Consulta.Rows
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            If cod_campo = hid_Codigo.Value Then
                Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)
                txt_Condicion.Text = Condiciones
            End If
        Next
    End Sub

    Private Sub btn_Nuevo_Click(sender As Object, e As EventArgs) Handles btn_Nuevo.Click
        Try
            LimpiaControles()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub ObtieneCampos(ByRef gvd_FilaValor As GridView, Optional ByVal blnFiltro As Boolean = False, Optional ByVal bln_Todos As Boolean = False)
        Dim Seleccion() As String
        Dim dtSeleccion As DataTable
        dtSeleccion = New DataTable
        dtSeleccion.Columns.Add("cod_campo")
        dtSeleccion.Columns.Add("Columna")
        dtSeleccion.Columns.Add("Seccion")
        dtSeleccion.Columns.Add("Union")
        dtSeleccion.Columns.Add("Operador")
        dtSeleccion.Columns.Add("Condicion")

        For Each Row In gvd_FilaValor.Rows
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
            Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)

            If blnFiltro = True Then
                Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
                Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
                Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)
                dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, txt_Condicion.Text)
            Else
                dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value)
            End If
        Next

        If bln_Todos = False Then
            Seleccion = Split(0 & "|" & EvaluaListado(chk_Generales, 1, blnFiltro) & "|" &
                                                    EvaluaListado(chk_Reaseguro, 2, blnFiltro) & "|" &
                                                    EvaluaListado(chk_Siniestros, 3, blnFiltro) & "|" &
                                                    EvaluaListado(chk_Cumulos, 4, blnFiltro) & "|" &
                                                    EvaluaListado(chk_Cobranzas, 5, blnFiltro) & "|" &
                                                    EvaluaListado(chk_Contabilidad, 6, blnFiltro), "|")
        Else
            Seleccion = Split(0 & "|" & EvaluaListado(chk_AllGenerales, 1, blnFiltro) & "|" &
                                                    EvaluaListado(chk_AllReaseguro, 2, blnFiltro) & "|" &
                                                    EvaluaListado(chk_AllSiniestros, 3, blnFiltro) & "|" &
                                                    EvaluaListado(chk_AllCumulos, 4, blnFiltro) & "|" &
                                                    EvaluaListado(chk_AllCobranzas, 5, blnFiltro) & "|" &
                                                    EvaluaListado(chk_AllContanbilidad, 6, blnFiltro), "|")
        End If


        If UBound(Seleccion) > 0 Then
            For i = 1 To UBound(Seleccion)
                If Len(Seleccion(i)) > 0 Then
                    dtSeleccion.Rows.Add(Split(Seleccion(i), "~")(0), Split(Seleccion(i), "~")(1), Split(Seleccion(i), "~")(2), "AND", "=", "")
                End If
            Next
        End If

        gvd_FilaValor.DataSource = dtSeleccion
        gvd_FilaValor.DataBind()
    End Sub

    Private Sub QuitaCampos(ByRef gvd_FilaValor As GridView, Optional ByVal blnFiltro As Boolean = False, Optional ByVal blnTodos As Boolean = False, Optional ByVal cod_campo As Integer = 0)
        Dim dtSeleccion As DataTable
        dtSeleccion = New DataTable
        dtSeleccion.Columns.Add("cod_campo")
        dtSeleccion.Columns.Add("Columna")
        dtSeleccion.Columns.Add("Seccion")
        dtSeleccion.Columns.Add("Union")
        dtSeleccion.Columns.Add("Operador")
        dtSeleccion.Columns.Add("Condicion")

        For Each Row In gvd_FilaValor.Rows
            Dim chk_SelCol As CheckBox = DirectCast(Row.FindControl("chk_SelCol"), CheckBox)
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
            Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)


            If chk_SelCol.Checked = True Or hid_Codigo.Value = cod_campo Then
                If blnFiltro = False Then
                    Select Case hid_seccion.Value
                        Case 1
                            chk_Generales.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                        Case 2
                            chk_Reaseguro.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                        Case 3
                            chk_Siniestros.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                        Case 4
                            chk_Cumulos.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                        Case 5
                            chk_Cobranzas.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                        Case 6
                            chk_Contabilidad.Items.Add(hid_Codigo.Value & "~" & lbl_Columna.Text)
                    End Select
                End If

                If blnTodos = True Then
                    DescartaValores(hid_Codigo.Value)
                End If
            Else
                If blnFiltro = False Then
                    dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value)
                Else
                    Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
                    Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
                    Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)
                    dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, txt_Condicion.Text)
                End If
            End If
        Next

        gvd_FilaValor.DataSource = dtSeleccion
        gvd_FilaValor.DataBind()
    End Sub

    Private Sub DescartaValores(ByVal cod_campo As Integer)
        Dim hid_Id As HiddenField
        hid_Id = New HiddenField

        Dim hid_Descripcion As HiddenField
        hid_Descripcion = New HiddenField

        Select Case cod_campo
            Case 1
                hid_IdSufijos.Value = ""
                hid_Sufijos.Value = ""
                lnk_Poliza.ForeColor = Drawing.Color.DarkBlue

            Case 52
                hid_IdBroker.Value = ""
                hid_Broker.Value = ""
                lnk_Broker.ForeColor = Drawing.Color.DarkBlue

            Case 54
                hid_IdCompañia.Value = ""
                hid_Compañia.Value = ""
                lnk_Compañia.ForeColor = Drawing.Color.DarkBlue


            Case 21
                hid_IdRamoContable.Value = ""
                hid_RamoContable.Value = ""
                lnk_RamoCOntable.ForeColor = Drawing.Color.DarkBlue

            Case 30
                hid_IdProducto.Value = ""
                hid_Producto.Value = ""
                lnk_Producto.ForeColor = Drawing.Color.DarkBlue

        End Select

        hid_Id.Value = ""
        hid_Descripcion.Value = ""
    End Sub

    Private Function EvaluaListado(ByRef Listado As CheckBoxList, ByVal nro_seccion As Integer, ByVal blnFiltro As Boolean) As String
        chk_Listado.Items.Clear()

        Dim Seleccion As String = vbNullString
        For Each item In Listado.Items
            If item.selected = True Then
                Seleccion = Seleccion & IIf(Len(Seleccion) > 0, "|", "") & item.Text & "~" & nro_seccion
                If blnFiltro = True Then item.selected = False
            Else
                chk_Listado.Items.Add(item.Text)
            End If
        Next

        If blnFiltro = False Then
            Listado.Items.Clear()
            For Each item In chk_Listado.Items
                Listado.Items.Add(item.Text)
            Next
        End If

        Return Seleccion
    End Function

    Private Sub btn_Filas_Click(sender As Object, e As EventArgs) Handles btn_Filas.Click
        Try
            ObtieneCampos(gvd_Filas)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Valores_Click(sender As Object, e As EventArgs) Handles btn_Valores.Click
        Try
            ObtieneCampos(gvd_Valores)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Filtro_Click(sender As Object, e As EventArgs) Handles btn_Filtro.Click
        Try
            ObtieneCampos(gvd_Filtros, True)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitaFilas_Click(sender As Object, e As EventArgs) Handles btn_QuitaFilas.Click
        Try
            QuitaCampos(gvd_Filas)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitaValores_Click(sender As Object, e As EventArgs) Handles btn_QuitaValores.Click
        Try
            QuitaCampos(gvd_Valores)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitaFiltro_Click(sender As Object, e As EventArgs) Handles btn_QuitaFiltro.Click
        Try

            QuitaCampos(gvd_Filtros, True)

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_FiltroFila_Click(sender As Object, e As EventArgs) Handles btn_FiltroFila.Click
        Try
            AgregaFiltro(gvd_Filas)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_FiltroValor_Click(sender As Object, e As EventArgs) Handles btn_FiltroValor.Click
        Try
            AgregaFiltro(gvd_Valores)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub AgregaFiltro(ByRef gvd_FilaValor As GridView)
        Dim dtSeleccion As DataTable
        dtSeleccion = New DataTable
        dtSeleccion.Columns.Add("cod_campo")
        dtSeleccion.Columns.Add("Columna")
        dtSeleccion.Columns.Add("Seccion")
        dtSeleccion.Columns.Add("Union")
        dtSeleccion.Columns.Add("Operador")
        dtSeleccion.Columns.Add("Condicion")

        For Each Row In gvd_Filtros.Rows
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
            Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)
            Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
            Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
            Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)
            dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, txt_Condicion.Text)
        Next

        For Each Row In gvd_FilaValor.Rows
            Dim chk_SelCol As CheckBox = DirectCast(Row.FindControl("chk_SelCol"), CheckBox)
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
            Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)

            If chk_SelCol.Checked = True Then
                dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, "AND", "=", "")
            End If
        Next

        gvd_Filtros.DataSource = dtSeleccion
        gvd_Filtros.DataBind()
    End Sub

    Private Sub gvd_Filtros_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_Filtros.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lbl_Where As Label = TryCast(e.Row.FindControl("lbl_Where"), Label)
                Dim ddl_Union As DropDownList = TryCast(e.Row.FindControl("ddl_Union"), DropDownList)
                Dim ddl_Operador As DropDownList = DirectCast(e.Row.FindControl("ddl_Operador"), DropDownList)
                Dim hid_Union As HiddenField = TryCast(e.Row.FindControl("hid_Union"), HiddenField)
                Dim hid_Operador As HiddenField = DirectCast(e.Row.FindControl("hid_Operador"), HiddenField)

                If e.Row.RowIndex = 0 Then
                    lbl_Where.Visible = True
                    ddl_Union.Visible = False
                End If
                ddl_Union.SelectedValue = hid_Union.Value
                ddl_Operador.SelectedValue = hid_Operador.Value
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_AplicaReplica_Click(sender As Object, e As EventArgs) Handles btn_AplicaReplica.Click
        Try
            If hid_Todos.Value = 0 Then
                ReplicarCondiciones(gvd_Filtros)
            Else
                ReplicarCondiciones(gvd_Consulta)
            End If

        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub ReplicarCondiciones(ByRef gvd_Condiciones As GridView)
        Dim i As Integer
        Dim dtSeleccion As DataTable
        dtSeleccion = New DataTable
        dtSeleccion.Columns.Add("cod_campo")
        dtSeleccion.Columns.Add("Columna")
        dtSeleccion.Columns.Add("Seccion")
        dtSeleccion.Columns.Add("Union")
        dtSeleccion.Columns.Add("Operador")
        dtSeleccion.Columns.Add("Condicion")

        For Each Row In gvd_Condiciones.Rows
            Dim chk_SelCol As CheckBox = DirectCast(Row.FindControl("chk_SelCol"), CheckBox)
            Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
            Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
            Dim hid_seccion As HiddenField = DirectCast(Row.FindControl("hid_seccion"), HiddenField)
            Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
            Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
            Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)

            dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, txt_Condicion.Text)

            If chk_SelCol.Checked = True Then
                For i = 1 To ddl_Replicas.SelectedValue
                    dtSeleccion.Rows.Add(hid_Codigo.Value, lbl_Columna.Text, hid_seccion.Value, ddl_Union.SelectedValue, ddl_Operador.SelectedValue, "")
                Next
            End If
        Next
        gvd_Condiciones.DataSource = dtSeleccion
        gvd_Condiciones.DataBind()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Replica", "ClosePopup('#ReplicaModal');", True)
    End Sub
    Private Sub gvd_Filtros_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Filtros.RowCommand
        Try
            If e.CommandName = "Multiples" Then
                hid_Todos.Value = 0

                Dim dtValor As DataTable
                dtValor = New DataTable
                dtValor.Columns.Add("Valor")

                gvd_Multiples.DataSource = Nothing
                gvd_Multiples.DataBind()

                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                hid_RowCondicion.Value = Index
                Dim txt_Condicion As TextBox = CType(gvd_Filtros.Rows(Index).FindControl("txt_Condicion"), TextBox)
                Dim Valores() As String = Split(Replace(txt_Condicion.Text, "'", ""), ",")

                For Each valor In Valores
                    dtValor.Rows.Add(valor)
                Next

                gvd_Multiples.DataSource = dtValor
                gvd_Multiples.DataBind()

                If InStr(txt_Condicion.Text, "'") > 0 Then
                    opt_TipoDatoFiltro.SelectedValue = "A"
                Else
                    opt_TipoDatoFiltro.SelectedValue = "N"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal", "OpenPopup('#MultivaloresModal');", True)
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_AñadirValor_Click(sender As Object, e As EventArgs) Handles btn_AñadirValor.Click
        Try
            Dim dtValor As DataTable
            dtValor = New DataTable
            dtValor.Columns.Add("Valor")

            For Each Row In gvd_Multiples.Rows
                Dim txt_Valor As TextBox = DirectCast(Row.FindControl("txt_Valor"), TextBox)
                dtValor.Rows.Add(txt_Valor.Text)
            Next
            dtValor.Rows.Add("")

            gvd_Multiples.DataSource = dtValor
            gvd_Multiples.DataBind()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitarValor_Click(sender As Object, e As EventArgs) Handles btn_QuitarValor.Click
        Try
            Dim dtValor As DataTable
            dtValor = New DataTable
            dtValor.Columns.Add("Valor")

            For Each Row In gvd_Multiples.Rows
                Dim chk_SelCol As CheckBox = DirectCast(Row.FindControl("chk_SelCol"), CheckBox)
                Dim txt_Valor As TextBox = DirectCast(Row.FindControl("txt_Valor"), TextBox)
                If chk_SelCol.Checked = False Then
                    dtValor.Rows.Add(txt_Valor.Text)
                End If
            Next

            gvd_Multiples.DataSource = dtValor
            gvd_Multiples.DataBind()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_AplicaValores_Click(sender As Object, e As EventArgs) Handles btn_AplicaValores.Click
        Try
            Dim txt_Condicion As TextBox
            Dim Condicion As String = vbNullString
            lbl_errorDato.Visible = False
            For Each Row In gvd_Multiples.Rows
                Dim txt_Valor As TextBox = DirectCast(Row.FindControl("txt_Valor"), TextBox)

                If Not IsNumeric(txt_Valor.Text) And opt_TipoDatoFiltro.SelectedValue = "N" Then
                    lbl_errorDato.Visible = True
                Else
                    Condicion = Condicion & IIf(Len(Condicion) > 0, ",", "") & IIf(opt_TipoDatoFiltro.SelectedValue = "A", "''", "") & txt_Valor.Text & IIf(opt_TipoDatoFiltro.SelectedValue = "A", "''", "")

                    If hid_Todos.Value = 0 Then
                        txt_Condicion = CType(gvd_Filtros.Rows(hid_RowCondicion.Value).FindControl("txt_Condicion"), TextBox)
                    Else
                        txt_Condicion = CType(gvd_Consulta.Rows(hid_RowCondicion.Value).FindControl("txt_Condicion"), TextBox)
                    End If

                    txt_Condicion.Text = Condicion
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Multivalores", "ClosePopup('#MultivaloresModal');", True)
                End If
            Next
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub opt_TipoDatoFiltro_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opt_TipoDatoFiltro.SelectedIndexChanged
        Try
            lbl_errorDato.Visible = False
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub
    Private Sub btn_AddConsulta_Click(sender As Object, e As EventArgs) Handles btn_AddConsulta.Click
        Try
            ObtieneCampos(gvd_Consulta, True, True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Consulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_Consulta.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hid_Codigo As HiddenField = TryCast(e.Row.FindControl("hid_Codigo"), HiddenField)
                Dim lbl_Where As Label = TryCast(e.Row.FindControl("lbl_Where"), Label)
                Dim ddl_Union As DropDownList = TryCast(e.Row.FindControl("ddl_Union"), DropDownList)
                Dim ddl_Operador As DropDownList = DirectCast(e.Row.FindControl("ddl_Operador"), DropDownList)
                Dim hid_Union As HiddenField = TryCast(e.Row.FindControl("hid_Union"), HiddenField)
                Dim hid_Operador As HiddenField = DirectCast(e.Row.FindControl("hid_Operador"), HiddenField)
                Dim btn_Multiples As Button = DirectCast(e.Row.FindControl("btn_Multiples"), Button)


                If hid_Codigo.Value = 1 Or hid_Codigo.Value = 52 Or hid_Codigo.Value = 54 Or hid_Codigo.Value = 21 Or hid_Codigo.Value = 30 Then
                    ddl_Union.Enabled = False
                    ddl_Operador.Enabled = False
                    btn_Multiples.Enabled = False
                End If

                If e.Row.RowIndex = 0 Then
                    lbl_Where.Visible = True
                    ddl_Union.Visible = False
                End If

                ddl_Union.SelectedValue = hid_Union.Value
                ddl_Operador.SelectedValue = hid_Operador.Value
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Consulta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Consulta.RowCommand
        Try
            If e.CommandName = "Multiples" Then

                hid_Todos.Value = 1

                Dim dtValor As DataTable
                dtValor = New DataTable
                dtValor.Columns.Add("Valor")

                gvd_Multiples.DataSource = Nothing
                gvd_Multiples.DataBind()

                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                hid_RowCondicion.Value = Index
                Dim txt_Condicion As TextBox = CType(gvd_Consulta.Rows(Index).FindControl("txt_Condicion"), TextBox)
                Dim Valores() As String = Split(Replace(txt_Condicion.Text, "'", ""), ",")

                For Each valor In Valores
                    dtValor.Rows.Add(valor)
                Next

                gvd_Multiples.DataSource = dtValor
                gvd_Multiples.DataBind()

                If InStr(txt_Condicion.Text, "'") > 0 Then
                    opt_TipoDatoFiltro.SelectedValue = "A"
                Else
                    opt_TipoDatoFiltro.SelectedValue = "N"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal", "OpenPopup('#MultivaloresModal');", True)
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitaConsulta_Click(sender As Object, e As EventArgs) Handles btn_QuitaConsulta.Click
        Try
            QuitaCampos(gvd_Consulta, True, True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub


    Private Sub btn_NuevaFormula_Click(sender As Object, e As EventArgs) Handles btn_NuevaFormula.Click
        Try
            dtFormula = New DataTable
            dtFormula.Columns.Add("Columna")
            dtFormula.Columns.Add("Formula")

            For Each Row In gvd_Formulas.Rows
                Dim txt_Columna As TextBox = DirectCast(Row.FindControl("txt_Columna"), TextBox)
                Dim txt_Formula As TextBox = DirectCast(Row.FindControl("txt_Formula"), TextBox)
                dtFormula.Rows.Add(txt_Columna.Text, txt_Formula.Text)
            Next

            dtFormula.Rows.Add("", "")

            gvd_Formulas.DataSource = dtFormula
            gvd_Formulas.DataBind()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Formulas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Formulas.RowCommand
        Try
            If e.CommandName = "Multiples" Then
                hid_rowFormula.Value = e.CommandSource.NamingContainer.RowIndex

                LlenaColumnasFor()

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal", "OpenPopup('#SelFormulaModal');", True)
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub ddl_seccion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_seccion.SelectedIndexChanged
        Try
            chk_ForGenerales.Visible = False
            chk_ForReaseguro.Visible = False
            chk_ForSiniestros.Visible = False
            chk_ForCumulos.Visible = False
            chk_ForCobranzas.Visible = False
            chk_ForContabilidad.Visible = False

            Select Case ddl_seccion.SelectedValue
                Case "Ge"
                    chk_ForGenerales.Visible = True
                Case "Re"
                    chk_ForReaseguro.Visible = True
                Case "Si"
                    chk_ForSiniestros.Visible = True
                Case "Cu"
                    chk_ForCumulos.Visible = True
                Case "Cb"
                    chk_ForCobranzas.Visible = True
                Case "Co"
                    chk_ForContabilidad.Visible = True
            End Select
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_OkSelFormula_Click(sender As Object, e As EventArgs) Handles btn_OkSelFormula.Click
        Try
            Dim Seleccion() As String
            Dim txt_Formula As TextBox
            Dim Formula As String = vbNullString

            Seleccion = Split(0 & "|" & EvaluaListado(chk_ForGenerales, 1, True) & "|" &
                                        EvaluaListado(chk_ForReaseguro, 2, True) & "|" &
                                        EvaluaListado(chk_ForSiniestros, 3, True) & "|" &
                                        EvaluaListado(chk_ForCumulos, 4, True) & "|" &
                                        EvaluaListado(chk_ForCobranzas, 5, True) & "|" &
                                        EvaluaListado(chk_ForContabilidad, 6, True), "|")

            If UBound(Seleccion) > 0 Then
                For i = 1 To UBound(Seleccion)
                    If Len(Seleccion(i)) > 0 Then
                        Formula = Formula & IIf(Len(Formula) > 0, " ", "") & "[" & Split(Seleccion(i), "~")(1) & "]"
                    End If
                Next
            End If

            txt_Formula = CType(gvd_Formulas.Rows(hid_rowFormula.Value).FindControl("txt_Formula"), TextBox)

            txt_Formula.Text = txt_Formula.Text & " " & Formula
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close SelFormula", "ClosePopup('#SelFormulaModal');", True)


        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitaFormula_Click(sender As Object, e As EventArgs) Handles btn_QuitaFormula.Click
        Try
            dtFormula = New DataTable
            dtFormula.Columns.Add("Columna")
            dtFormula.Columns.Add("Formula")

            For Each Row In gvd_Formulas.Rows
                Dim chk_SelCol As CheckBox = DirectCast(Row.FindControl("chk_SelCol"), CheckBox)
                Dim txt_Columna As TextBox = DirectCast(Row.FindControl("txt_Columna"), TextBox)
                Dim txt_Formula As TextBox = DirectCast(Row.FindControl("txt_Formula"), TextBox)
                If chk_SelCol.Checked = False Then
                    dtFormula.Rows.Add(txt_Columna.Text, txt_Formula.Text)
                End If
            Next

            gvd_Formulas.DataSource = dtFormula
            gvd_Formulas.DataBind()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_ConfirmaVersion_Click(sender As Object, e As EventArgs) Handles btn_ConfirmaVersion.Click
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
                lbl_DescVersion.Text = txt_descripcion.Text
                hid_Temporal.Value = IIf(chk_Temporal.Checked = True, -1, 0)
                btn_Actualizar.Enabled = chk_Temporal.Checked
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close Version", "ClosePopup('#GuardaVersionModal');", True)
            End If

            ObtieneVersiones()
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_CierraVersion_Click(sender As Object, e As EventArgs) Handles btn_CierraVersion.Click
        Try
            txt_descripcion.Text = vbNullString
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close Version", "ClosePopup('#GuardaVersionModal');", True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Versiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Versiones.RowCommand
        Try
            If e.CommandName = "Version" Then
                Dim cod_campo As Integer
                Dim strCampos As String = "0"
                Dim strAgregados As String = "-1"
                Dim Descripcion As String
                Dim nro_seccion As Integer
                Dim Filtros() As String
                Dim Consulta As Catalogo
                Consulta = New Catalogo

                Dim dtColumnas As DataTable
                dtColumnas = New DataTable

                LimpiaControles()

                Dim lbl_Config As Label = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("lbl_Config"), Label)
                Dim hid_Url As HiddenField = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("hid_Url"), HiddenField)
                Dim hid_Filtros As HiddenField = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("hid_Filtros"), HiddenField)
                Dim ddl_Formato As DropDownList = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("ddl_Formato"), DropDownList)
                Dim txt_Reporte As TextBox = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("txt_Reporte"), TextBox)
                Dim chk_Temp As CheckBox = CType(gvd_Versiones.Rows(e.CommandSource.NamingContainer.RowIndex).FindControl("chk_Temporal"), CheckBox)

                hid_version.Value = lbl_Config.Text
                txt_descripcion.Text = txt_Reporte.Text
                lbl_DescVersion.Text = txt_Reporte.Text
                chk_Temporal.Checked = chk_Temp.Checked
                btn_Actualizar.Enabled = chk_Temp.Checked
                hid_Temporal.Value = IIf(chk_Temp.Checked = True, -1, 0)

                'Filtros
                Filtros = Split(hid_Filtros.Value, "&")
                LlenaInfoFiltros(Filtros)

                Master.Formato = ddl_Formato.SelectedValue

                Session("dtCampos") = GeneraDatatableCampos()
                gvd_Configuracion.DataSource = ConsultaCampos(hid_version.Value)
                gvd_Configuracion.DataBind()

                For Each Row In gvd_Configuracion.Rows
                    cod_campo = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("lbl_Clave"), Label).Text
                    Descripcion = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("lbl_Desc"), Label).Text
                    nro_seccion = TryCast(gvd_Configuracion.Rows(Row.rowIndex).FindControl("hid_Seccion"), HiddenField).Value
                    strCampos = strCampos & "," & cod_campo
                    EstableceFiltro(cod_campo, Descripcion, nro_seccion)
                Next

                If Len(hid_IdSeccion1.Value) > 0 Then
                    lnk_Generales.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdSeccion2.Value) > 0 Then
                    lnk_Reaseguro.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdSeccion3.Value) > 0 Then
                    lnk_Siniestros.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdSeccion4.Value) > 0 Then
                    lnk_Cumulos.ForeColor = Drawing.Color.Red
                End If
                If Len(hid_IdSeccion5.Value) > 0 Then
                    lnk_Cobranzas.ForeColor = Drawing.Color.Red
                End If

                'COnsulta de Formulas
                gvd_Formulas.DataSource = Consulta.ObtieneFormulas(hid_codReporte.Value, hid_version.Value)
                gvd_Formulas.DataBind()


                'Consulta de Agrupaciones
                ddl_Agrupaciones.DataValueField = "Clave"
                ddl_Agrupaciones.DataTextField = "Descripcion"
                ddl_Agrupaciones.DataSource = Consulta.ObtieneAgrupacion(hid_codReporte.Value, hid_version.Value)
                ddl_Agrupaciones.DataBind()

                If ddl_Agrupaciones.Items.Count > 1 Then
                    ddl_Agrupaciones.SelectedIndex = 1
                    ddl_Agrupaciones_SelectedIndexChanged(Me, Nothing)
                End If
                'COnsulta de Consultas
                gvd_Consulta.DataSource = Consulta.ObtieneConsultas(hid_codReporte.Value, hid_version.Value)
                gvd_Consulta.DataBind()

                clCatalogo = New Catalogo
                dtColumnas = clCatalogo.ObtieneCatalogo("Rep", " WHERE cod_reporte IN(" & hid_codReporte.Value & ") AND cod_seccion IN (1,2,3,4,5,6)", "")

                For Each row In gvd_Filas.Rows
                    cod_campo = TryCast(gvd_Filas.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
                    strAgregados = strAgregados & "," & cod_campo
                Next

                For Each row In gvd_Valores.Rows
                    cod_campo = TryCast(gvd_Valores.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
                    strAgregados = strAgregados & "," & cod_campo
                Next

                LlenaListado(chk_Generales, dtColumnas.Select("Adicional ='" & 1 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))
                LlenaListado(chk_Reaseguro, dtColumnas.Select("Adicional ='" & 2 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))
                LlenaListado(chk_Siniestros, dtColumnas.Select("Adicional ='" & 3 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))
                LlenaListado(chk_Cumulos, dtColumnas.Select("Adicional ='" & 4 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))
                LlenaListado(chk_Cobranzas, dtColumnas.Select("Adicional ='" & 5 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))
                LlenaListado(chk_Contabilidad, dtColumnas.Select("Adicional ='" & 6 & "' AND Clave IN(" & strCampos & ") AND Clave NOT IN(" & strAgregados & ")"))

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Close Versiones", "ClosePopup('#VersionesModal');", True)
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_GenerarMultiple_Click(sender As Object, e As EventArgs) Handles btn_GenerarMultiple.Click
        Try
            Dim url As String = vbNullString
            Dim Formato As String = vbNullString
            Dim strReporte As String = vbNullString

            For Each row In gvd_Versiones.Rows
                Dim chk_SelCol As CheckBox = DirectCast(row.FindControl("chk_SelCol"), CheckBox)
                Dim lbl_Config As Label = DirectCast(row.FindControl("lbl_Config"), Label)
                Dim hid_Url As HiddenField = DirectCast(row.FindControl("hid_Url"), HiddenField)
                Dim hid_Filtros As HiddenField = DirectCast(row.FindControl("hid_Filtros"), HiddenField)
                Dim ddl_Formato As DropDownList = DirectCast(row.FindControl("ddl_Formato"), DropDownList)
                Dim chk_Temp As CheckBox = DirectCast(row.FindControl("chk_Temporal"), CheckBox)

                Formato = ""
                If ddl_Formato.SelectedValue <> "NAV" Then
                    Formato = "&rs%3AFormat=" & ddl_Formato.SelectedValue
                End If

                If chk_SelCol.Checked = True Then
                    strReporte = "&cod_reporte=" & hid_codReporte.Value & "&sn_Temporal=" & IIf(chk_Temp.Checked = True, -1, 0) & "&cod_config=" & lbl_Config.Text
                    url = url & IIf(Len(url) > 0, "|=|", "") & hid_Url.Value & "&rc:Parameters=false" & Formato & hid_Filtros.Value & strReporte
                End If
            Next
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirReportes", "ImprimirReportes('" & url & "');", True)
        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub ddl_Agrupaciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Agrupaciones.SelectedIndexChanged
        Try
            Dim Consulta As Catalogo
            Consulta = New Catalogo

            If ddl_Agrupaciones.SelectedIndex = 0 Then
                gvd_Filtros.DataSource = Nothing
                gvd_Filtros.DataBind()
            Else
                gvd_Filas.DataSource = Consulta.ObtieneFilasXAgrupacion(ddl_Agrupaciones.SelectedValue, 0)
                gvd_Filas.DataBind()

                gvd_Valores.DataSource = Consulta.ObtieneFilasXAgrupacion(ddl_Agrupaciones.SelectedValue, -1)
                gvd_Valores.DataBind()

                gvd_Filtros.DataSource = Consulta.ObtieneFiltrosXAgrupacion(ddl_Agrupaciones.SelectedValue)
                gvd_Filtros.DataBind()
            End If



        Catch ex As Exception
            Mensaje("REPORTES-: ", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub EdoControl(ByVal Estado As Integer)
        Select Case Estado
            Case EnumEstado.Nuevo
                ddl_Agrupaciones.Enabled = False
                btn_GuardarAgrupacion.Enabled = False
                btn_GenerarAgrupacion.Enabled = False
            Case EnumEstado.Cambio
                ddl_Agrupaciones.Enabled = True
                btn_GuardarAgrupacion.Enabled = True
                btn_GenerarAgrupacion.Enabled = True
        End Select
    End Sub

    Private Function InsertaAgrupacion(ByVal conn As SqlConnection, ByVal sCnn As String) As Boolean
        Dim Comando As SqlClient.SqlCommand
        Dim sSel As String
        Dim strCampos As String = ""
        Dim cod_agrupacion As Integer
        Dim Resultado As Integer

        If ddl_Agrupaciones.SelectedValue = 0 Then
            sSel = "EXEC spI_Agrupacion " & hid_codReporte.Value & "," & hid_version.Value & ",'Agrupación " & Today.ToString("dd/MM/yyyy") & "'"
        Else
            sSel = "EXEC spU_Agrupacion " & ddl_Agrupaciones.SelectedValue & ",'Agrupación " & Today.ToString("dd/MM/yyyy") & "'"
        End If
        Comando = New SqlClient.SqlCommand(sSel, conn)
        Comando.Transaction = trRep
        cod_agrupacion = Convert.ToInt32(Comando.ExecuteScalar())

        ddl_Agrupaciones.Items.Add(New ListItem("Agrupacion " & Today.ToString("dd/MM/yyyy"), cod_agrupacion))
        ddl_Agrupaciones.SelectedValue = cod_agrupacion

        strCampos = ""
        For Each row In gvd_Filas.Rows
            Dim cod_campo As Integer = TryCast(gvd_Filas.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
            strCampos = strCampos & "(@strKey," & cod_campo & ",0),"
        Next

        For Each row In gvd_Valores.Rows
            Dim cod_campo As Integer = TryCast(gvd_Valores.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
            strCampos = strCampos & "(@strKey," & cod_campo & ",-1),"
        Next

        If Len(strCampos) > 0 Then
            strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
        End If

        sSel = "EXEC spI_OfGread 'rColumnaXAgrupacion','" & cod_agrupacion & "','" & strCampos & "'"
        Comando = New SqlClient.SqlCommand(sSel, conn)
        Comando.Transaction = trRep
        Resultado = Convert.ToInt32(Comando.ExecuteScalar())

        strCampos = ""
        For Each row In gvd_Filtros.Rows
            Dim cod_campo As Integer = TryCast(gvd_Filtros.Rows(row.rowIndex).FindControl("hid_Codigo"), HiddenField).Value
            Dim union As String = TryCast(gvd_Filtros.Rows(row.rowIndex).FindControl("ddl_Union"), DropDownList).SelectedValue
            Dim operador As String = TryCast(gvd_Filtros.Rows(row.rowIndex).FindControl("ddl_Operador"), DropDownList).SelectedValue
            Dim condicion As String = TryCast(gvd_Filtros.Rows(row.rowIndex).FindControl("txt_Condicion"), TextBox).Text

            condicion = Replace(condicion, "''", "''''''''")
            strCampos = strCampos & "(@strKey," & cod_campo & ",''" & union & "'',''" & operador & "'',''" & condicion & "''),"
        Next

        If Len(strCampos) > 0 Then
            strCampos = Mid(strCampos, 1, Len(strCampos) - 1)
        End If

        sSel = "EXEC spI_OfGread 'rFiltroXAgrupacion','" & cod_agrupacion & "','" & strCampos & "'"
        Comando = New SqlClient.SqlCommand(sSel, conn)
        Comando.Transaction = trRep
        Resultado = Convert.ToInt32(Comando.ExecuteScalar())


        Return True
    End Function

    Private Sub btn_GenerarAgrupacion_Click(sender As Object, e As EventArgs) Handles btn_GenerarAgrupacion.Click
        Try
            Dim strFilas As String = ""
            Dim strValores As String = ""
            Dim strCondicion As String = ""
            Dim strParciales As String = ""
            Dim condicion = ""

            For Each Row In gvd_Filas.Rows
                Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
                Dim chk_Parcial As CheckBox = DirectCast(Row.FindControl("chk_Parcial"), CheckBox)
                strFilas = strFilas & IIf(Len(strFilas) > 0, ",", "") & hid_Codigo.Value
                If chk_Parcial.Checked = True Then
                    strParciales = strParciales & IIf(Len(strParciales) > 0, ",", "") & hid_Codigo.Value
                End If
            Next


            For Each Row In gvd_Valores.Rows
                Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
                strValores = strValores & IIf(Len(strValores) > 0, ",", "") & hid_Codigo.Value
            Next


            For Each Row In gvd_Filtros.Rows
                Dim hid_Codigo As HiddenField = DirectCast(Row.FindControl("hid_Codigo"), HiddenField)
                Dim lbl_Columna As Label = DirectCast(Row.FindControl("lbl_Columna"), Label)
                Dim ddl_Union As DropDownList = DirectCast(Row.FindControl("ddl_Union"), DropDownList)
                Dim ddl_Operador As DropDownList = DirectCast(Row.FindControl("ddl_Operador"), DropDownList)
                Dim txt_Condicion As TextBox = DirectCast(Row.FindControl("txt_Condicion"), TextBox)

                If Row.RowIndex = 0 Then
                    strCondicion = " WHERE "
                Else
                    strCondicion = strCondicion & " " & ddl_Union.SelectedValue & " "
                End If
                'Buscar cod_campo en configuracion
                'Obtener el número de posicion
                For Each RowFiltro In gvd_Configuracion.Rows
                    Dim lbl_Clave As Label = DirectCast(RowFiltro.FindControl("lbl_Clave"), Label)

                    If hid_Codigo.Value = lbl_Clave.Text Then
                        Dim hid_posicion As HiddenField = DirectCast(RowFiltro.FindControl("hid_posicion"), HiddenField)
                        strCondicion = strCondicion & "Campo" & hid_posicion.Value & " "
                        Exit For
                    End If
                Next

                Select Case ddl_Operador.SelectedValue
                    Case "LIKE ''@%''", "LIKE ''%@''", "LIKE ''%@%''"
                        condicion = Replace(Replace(Replace(ddl_Operador.SelectedValue, "@", Replace(txt_Condicion.Text, "''", "")), "", ""), "''", "''''")
                    Case "IN(@)", "NOT IN(@)"
                        condicion = Replace(Replace(Replace(ddl_Operador.SelectedValue, "@", txt_Condicion.Text), "", ""), "''", "''''")
                    Case Else
                        condicion = ddl_Operador.SelectedValue & IIf(IsNumeric(txt_Condicion.Text), txt_Condicion.Text, "''" & txt_Condicion.Text & "''")
                End Select
                strCondicion = strCondicion & condicion
            Next

            Dim Consulta As Catalogo
            Consulta = New Catalogo

            Dim dtResultado As DataTable = Consulta.ObtieneAgrupador(hid_codReporte.Value, hid_version.Value, strFilas, strValores, strCondicion, strParciales)

            dtResultado = AgrupaConceptos(dtResultado, 0)

            Session("dtResultado") = dtResultado

            gvd_Resultado.DataSource = dtResultado
            gvd_Resultado.DataBind()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Open", "OpenPopup('#ResultadoModal');", True)
        Catch ex As Exception
            Mensaje("REPORTES-: btn_GenerarAgrupacion_Click", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_GuardarAgrupacion_Click(sender As Object, e As EventArgs) Handles btn_GuardarAgrupacion.Click
        Dim sCnn As String = ""
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        conn.Open()
        trRep = conn.BeginTransaction()
        Try
            If gvd_Filas.Rows.Count > 0 Or gvd_Valores.Rows.Count > 0 Or gvd_Filtros.Rows.Count > 0 Then
                InsertaAgrupacion(conn, sCnn)
            End If
            trRep.Commit()
            conn.Close()

            Mensaje("REPORTES-: ", "Los cambios han sido aplicados")
        Catch ex As Exception
            trRep.Rollback()
            conn.Close()
            Mensaje("REPORTES-: btn_GuardarAgrupacion_Click", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub
    Public Sub chk_Temporal_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim sCnn As String = ""
            Dim sSel As String
            Dim Comando As SqlClient.SqlCommand

            Dim gr As GridViewRow = DirectCast(DirectCast(DirectCast(sender, CheckBox).Parent, DataControlFieldCell).Parent, GridViewRow)
            Dim chk_Temp As CheckBox = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("chk_Temporal"), CheckBox)
            Dim cod_config As Integer = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("lbl_Config"), Label).Text
            Dim cod_reporte As Integer = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("hid_Reporte"), HiddenField).Value



            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Dim conn As SqlConnection = New SqlConnection(sCnn)

            conn.Open()
            sSel = "spD_Temporal " & cod_reporte & "," & cod_config & ",'" & Master.cod_usuario & "'," & IIf(chk_Temp.Checked = True, -1, 0)
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Convert.ToInt32(Comando.ExecuteScalar())
            conn.Close()

            If cod_config = hid_version.Value Then
                hid_Temporal.Value = IIf(chk_Temp.Checked = True, -1, 0)
                chk_Temporal.Checked = chk_Temp.Checked
                btn_Actualizar.Enabled = chk_Temp.Checked
            End If

            Mensaje("REPORTES-:", IIf(chk_Temp.Checked = True, "Es necesario Generar nuevamente el Reporte para que se configure de forma automática el Ambiente Temporal", "EL Ambiente Temporal ha sido eliminado"))

        Catch ex As Exception
            Mensaje("REPORTES-: chk_Temporal_CheckedChanged", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Versiones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_Versiones.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lbl_formato As Label = TryCast(e.Row.FindControl("lbl_formato"), Label)
                Dim ddl_Formato As DropDownList = TryCast(e.Row.FindControl("ddl_Formato"), DropDownList)
                ddl_Formato.SelectedValue = lbl_formato.Text
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: gvd_Versiones_RowDataBound", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub
    Protected Sub ddl_Formato_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim sCnn As String = ""
            Dim sSel As String
            Dim Comando As SqlClient.SqlCommand

            Dim gr As GridViewRow = DirectCast(DirectCast(DirectCast(sender, DropDownList).Parent, DataControlFieldCell).Parent, GridViewRow)

            Dim cod_config As Integer = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("lbl_Config"), Label).Text
            Dim cod_reporte As Integer = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("hid_Reporte"), HiddenField).Value
            Dim Reporte As String = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("txt_Reporte"), TextBox).Text
            Dim Filtros As String = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("hid_Filtros"), HiddenField).Value
            Dim sn_Temporal As Integer = IIf(TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("chk_Temporal"), CheckBox).Checked = True, -1, 0)
            Dim lbl_formato As Label = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("lbl_formato"), Label)
            Dim ddl_Formato As DropDownList = TryCast(gvd_Versiones.Rows(gr.RowIndex).FindControl("ddl_Formato"), DropDownList)

            lbl_formato.Text = ddl_Formato.SelectedValue
            If cod_config = hid_version.Value Then
                Master.Formato = lbl_formato.Text
            End If

            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Dim conn As SqlConnection = New SqlConnection(sCnn)
            conn.Open()

            sSel = "EXEC spU_VersionReporte " & cod_config & "," & cod_reporte & ",'" & Master.cod_usuario & "','" & Reporte & "','" & Filtros & "','" & lbl_formato.Text & "'," & sn_Temporal
            Comando = New SqlClient.SqlCommand(sSel, conn)
            Convert.ToInt32(Comando.ExecuteScalar())

            conn.Close()

        Catch ex As Exception
            Mensaje("REPORTES-: ddl_Formato_SelectedIndexChanged", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_Agrupador_Click(sender As Object, e As EventArgs) Handles btn_Agrupador.Click
        Try
            Dim Consulta As Catalogo
            Consulta = New Catalogo

            If Consulta.ValidaTemporal(hid_codReporte.Value, hid_version.Value) = -1 Then
                EdoControl(EnumEstado.Cambio)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Open", "OpenPopup('#AgrupacionModal');", True)
            Else
                EdoControl(EnumEstado.Nuevo)
                If hid_version.Value = 0 Then
                    Mensaje("REPORTES", "Para generar Agrupadores, debe guardar la versión del Reporte con Ambiente Temporal habilitado")
                Else
                    If hid_Temporal.Value = 0 Then
                        Mensaje("REPORTES", "Para generar Agrupadores, debe habilitar el Ambiente Temporal")
                    Else
                        Mensaje("REPORTES", "Para generar Agrupadores, debe Generar el Reporte nuevamente para crear el Ambiente Temporal")
                    End If
                End If
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: btn_Agrupador_Click", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub btn_CerrarResultado_Click(sender As Object, e As EventArgs) Handles btn_CerrarResultado.Click
        Try
            hid_Paginas.Value = "-1"
            Session("dtResultado") = Nothing
            gvd_Resultado.DataSource = Nothing
            gvd_Resultado.DataBind()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Catalogo", "ClosePopup('#ResultadoModal');", True)
        Catch ex As Exception
            Mensaje("REPORTES-: btn_CerrarResultado_Click", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Sub gvd_Resultado_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_Resultado.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim numColumna As Integer
                Dim Columnas = gvd_Filas.Rows.Count + 2
                Dim alineacion As String = ""

                For Each cell As TableCell In e.Row.Cells
                    alineacion = ""

                    numColumna = 0
                    If Left(cell.Text, 6) = "[TOTAL" Then
                        For Each cell2 As TableCell In e.Row.Cells

                            If numColumna = 0 Then
                                cell2.Text = ""
                            End If

                            If IsNumeric(cell2.Text) Then
                                If InStr(cell2.Text, ".") Then
                                    cell2.Text = String.Format("{0:#,#0.00}", CDbl(cell2.Text))
                                    alineacion = " text-align: right;"
                                Else
                                    alineacion = " text-align: center;"
                                End If
                            End If

                            cell2.Attributes.Add("style", "color: white; font-weight: bold; background-color: darkblue;" & alineacion)

                            If numColumna > 1 And numColumna < Columnas Then
                                cell2.Text = ""
                            End If

                            numColumna = numColumna + 1

                            If cell2.Text.Length > 0 Then
                                cell2.Text = "<nobr>" + cell2.Text + "</nobr>"
                            End If
                        Next
                        Exit For
                    End If



                    If Right(cell.Text, 3) = ".00" Then
                        cell.Text = Replace(cell.Text, ".00", "")
                    End If

                    If Right(cell.Text, 3) = ".01" Then
                        cell.Text = ""
                    End If


                    If IsNumeric(cell.Text) Then
                        If InStr(cell.Text, ".") Then
                            cell.Text = String.Format("{0:#,#0.00}", CDbl(cell.Text))
                            alineacion = " text-align: right;"
                        Else
                            alineacion = " text-align: center;"
                        End If
                    End If

                    cell.Attributes.Add("style", alineacion)


                    If cell.Text.Length > 0 Then
                        cell.Text = "<nobr>" + cell.Text + "</nobr>"
                    End If
                Next
            Else
                For Each cell As TableCell In e.Row.Cells
                    cell.Attributes.Add("style", " text-align: center;")

                    If cell.Text.Length > 0 Then
                        cell.Text = "<nobr>" + cell.Text + "</nobr>"
                    End If
                Next
            End If
        Catch ex As Exception
            Mensaje("REPORTES-: gvd_Resultado_RowDataBound", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub

    Private Function AgrupaConceptos(ByVal dt As DataTable, ByVal Pagina As Integer) As DataTable
        Dim inicio As Integer
        Dim fin As Integer
        Dim Paginas() As String

        Paginas = Split(hid_Paginas.Value, ",")


        For Each pag In Paginas
            If pag = Pagina Then
                Return dt
            End If
        Next

        inicio = (Pagina * 1000) + 1
        fin = (Pagina * 1000) + 1000

        Dim myRow() As Data.DataRow
        myRow = dt.Select("No >= " & inicio & " AND No <= " & fin)

        Dim Campo(gvd_Filas.Rows.Count) As String

        For Each Row In myRow
            For i = 1 To UBound(Campo)
                If CStr(Row(i + 1)) <> CStr(Campo(i)) Then
                    Campo(i) = Row(i + 1)
                    For j = i + 1 To UBound(Campo)
                        Campo(j) = ""
                    Next
                Else
                    Row(i + 1) = DBNull.Value
                End If
            Next
        Next

        hid_Paginas.Value = hid_Paginas.Value & IIf(Len(hid_Paginas.Value) > 0, ",", "") & Pagina

        Return dt
    End Function

    Private Sub gvd_Resultado_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvd_Resultado.PageIndexChanging
        Try
            gvd_Resultado.PageIndex = e.NewPageIndex

            dtResultado = Session("dtResultado")
            dtResultado = AgrupaConceptos(dtResultado, e.NewPageIndex)

            Session("dtResultado") = dtResultado


            gvd_Resultado.DataSource = dtResultado
            gvd_Resultado.DataBind()
        Catch ex As Exception
            Mensaje("REPORTES-: gvd_Resultado_PageIndexChanging", ex.Message)
            LogError(ex.Message)
        End Try
    End Sub



End Class
