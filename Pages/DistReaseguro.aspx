<%@ Page Title="" Language="VB" MasterPageFile="SiteMaster.master" AutoEventWireup="false" ClientIDMode="AutoID" CodeFile="DistReaseguro.aspx.vb" Inherits="Pages_DistReaseguro" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/jquery.maskedinput.js"></script>
    <script src="../Scripts/RepDistReaseguro.js"></script>
    <script src="../Scripts/jquery.numeric.js"></script>


 <script type="text/javascript"> 
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);
</script> 
     

    

    <asp:HiddenField runat="server" ID="hid_Ventanas" Value="1|1|1|1|" />
    <asp:HiddenField runat="server" ID="hid_Url" Value="" />
    
    <asp:Button ID="btn_CerrarSesion" runat="server" CssClass="NoDisplay CerrarSesion" />
    <asp:HiddenField ID="hid_CierraSesion" runat="server" Value="0" />

    <table>
        <tr>
            <td style="text-align:start;">
                <div style="width:700px; min-width:700px; height:300px; min-height:300px;  overflow-x:hidden">
                    <div class="panel-heading">
                        <strong>Columnas</strong>
                    </div>
                    <div class="panel-body" >
                        <div class="row">

                             <asp:UpdatePanel runat="server" ID="up_Campos">
                                <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="hid_codReporte" Value="1" />
                                        <table>
                                            <tr>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Generales" OnClick="Seleccionados" class="col-md-1 control-label"  Width="100px" >Generales</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Generales" CssClass="Generales"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdGenerales" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Generales" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Reaseguro" OnClick="Seleccionados" class="col-md-1 control-label"  Width="100px" >Reaseguro</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Reaseguro"  CssClass="Reaseguro" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdReaseguro" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Reaseguro" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Ubicaciones" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px" >Ubicaciones</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Ubicaciones" CssClass="Ubicaciones" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdUbicaciones" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Ubicaciones" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Coberturas" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px" >Coberturas</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Coberturas" CssClass="Coberturas"  ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdCoberturas" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Coberturas" Value="" />
                                                </td>
                                                <td>
                                                    <label style="width:30px"></label>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Configuracion" CssClass="btn btn-primary" data-toggle="modal" data-target="#ConfiguracionModal" Text="Personalizar" />
                                                </td>
                                            </tr>
                                        </table>

                                        <%--<div class="col-md-3">
                                            <div class="panel-heading">
                                                <strong>Póliza:</strong>
                                            </div>
                                            <div class="clear padding5"></div>
                                            <asp:CheckBoxList runat="server" ID="chk_Poliza" AutoPostBack="true" OnSelectedIndexChanged="CambioSeleccion">
                                            </asp:CheckBoxList> 
                                        </div>

                                        <div class="col-md-3">
                                            <div class="panel-heading">
                                                <strong>Reaseguro:</strong>
                                            </div>
                                            <div class="clear padding5"></div>
                                             <asp:CheckBoxList runat="server" ID="chk_Reaseguros"  AutoPostBack="true" OnSelectedIndexChanged="CambioSeleccion">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="panel-heading">
                                                <strong>Ubicaciones:</strong>
                                            </div>
                                            <div class="clear padding5"></div>
                                            <asp:CheckBoxList runat="server" ID="chk_Ubicacion"  AutoPostBack="true" OnSelectedIndexChanged="CambioSeleccion">
                                            </asp:CheckBoxList>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="panel-heading">
                                                <strong>Coberturas:</strong>
                                            </div>
                                            <div class="clear padding5"></div>
                                            <asp:CheckBoxList runat="server" ID="chk_Cobertura"  AutoPostBack="true" OnSelectedIndexChanged="CambioSeleccion">
                                            </asp:CheckBoxList>
                                        </div>--%>
                                </ContentTemplate>
                             </asp:UpdatePanel>
                    
                        </div>
                    </div>

                    <div class="panel-heading">
                        <strong>Consulta</strong>
                    </div>
                    <div class="panel-body" >
                        <div class="row">
                             <asp:UpdatePanel runat="server" ID="upFiltros">
                                <ContentTemplate>
                                     <table>
                                        <tr>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Fecha Emisión</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FechaIni" CssClass="form-control FechaSB" Width="110px" Height="26px" ></asp:TextBox>
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FechaFin" CssClass="form-control FechaSB" Width="110px" Height="26px"  ></asp:TextBox>
                                            </td>
                                            <td>
                                                        <label style="width:20px"></label>
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="175px">Solo Facultativos</asp:label>
                                                        <asp:CheckBox runat="server" ID="chk_Reaseguro" Text="" />
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Fin de Vigencia</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FinVigDe" CssClass="form-control FechaSB" Width="110px" Height="26px" ></asp:TextBox>
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FinVigA" CssClass="form-control FechaSB" Width="110px" Height="26px"  ></asp:TextBox>
                                            </td>
                                            <td>
                                                        
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="80px" >Contrato</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_Contrato" CssClass="form-control" MaxLength="15" Width="110px" Height="26px"  ></asp:TextBox>  
                                            </td>
                                        </tr>
                                    </table>    
                                    <div class="clear padding10"></div>
                                    <table>

                                            <tr>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Poliza" OnClick="Seleccionados"  class="col-md-1 control-label" Width="100px" >Polizas</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Poliza" CssClass="Endosos"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal" data-target="#PolizaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdSufijos" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Sufijos" Value="" />
                                                </td>
                                                <td> 
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Broker" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px" >Brokers</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Broker" CssClass="Broker"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal"  data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdBroker" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Broker" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Compañia" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px" >Compañias</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Compañia"  CssClass="Compañia" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdCompañia" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_Compañia" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_RamoCOntable" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px"  >Ramos Contables</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_RamoContable" CssClass="RamoContable" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdRamoContable" Value="" />
                                                     <asp:HiddenField runat="server" ID="hid_RamoContable" Value="" />
                                                </td>
                                                <td>
                                                     <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Producto" OnClick="Seleccionados" class="col-md-1 control-label" Width="100px" >Productos</asp:LinkButton>
                                                     <asp:ImageButton runat="server" ID="btn_Producto" CssClass="Producto"  ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                     <asp:HiddenField runat="server" ID="hid_IdProducto" Value="" /> 
                                                     <asp:HiddenField runat="server" ID="hid_Producto" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                </ContentTemplate>
                             </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </td>
            <td>
                <label style="width:20px;"></label>
            </td>
            <td style="text-align:start;">
                <div style="width:400px; min-width:400px; height:300px; min-height:300px;  overflow-x:hidden">
                    <div class="panel-heading">
                        <strong>Versiones Generadas</strong>
                    </div>
                    <div class="panel-body" >
                        <div class="row">
                             <asp:UpdatePanel runat="server" ID="upVersiones">
                                <ContentTemplate>
                                    <asp:CheckBox runat="server" ID="chk_Generar" Text="Generar al seleccionar" Checked="true" />
                                    <label style="width:10px"></label>
                                    <asp:CheckBox runat="server" ID="chk_Consultar" Text="Consultar al seleccionar" Checked="true" />
                                    <label style="width:30px"></label>
                                    <asp:ListBox runat="server" ID="lst_Versiones"  Width="100%" Height="170px" AutoPostBack="true" ></asp:ListBox>
                                    <div style="width:100%;  text-align:right">
                                        <asp:Button runat="server" CssClass="btn btn-primary" Width="80px" ID="btn_Nuevo" Text="Crear" />
                                        <asp:Button runat="server" CssClass="btn btn-success" Width="80px" ID="btn_Guardar" data-toggle="modal" data-target="#VersionModal" Text="Guardar" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
        
        
         <%-----------------------------------Sección 2----------------------------------------------------------------------------------------------------%>
       

        <div class="panel-body ventana1" >
            <div class="row">
                <div class="col-md-6">
                    <div class="panel-heading">
                        <strong>Sufijos</strong>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upPoliza">
                                <ContentTemplate>
                                    <asp:HiddenField runat="server" ID="hid_Polizas" Value="" />
                                        
                            
                                    <div class="clear padding10"></div>
                                    <asp:HiddenField runat="server" ID="hid_ClavePol" Value="" />
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </div>
                    </div>                                                
                </div>

                <div class="col-md-6">
                    <div class="panel-heading">
                        <strong>Endosos</strong>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upEndosos">
                                <ContentTemplate>
                                    <asp:HiddenField runat="server" ID="hid_Endosos" Value="" />


                                    <asp:Panel runat="server" ID="pnl_Endosos" Width="415px" Height="143px" ScrollBars="Both">
                                            <asp:GridView runat="server" ID="gvd_Endosos" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chk_SelPol" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Endoso" ItemStyle-CssClass="ClaveCia">
                                                        <ItemTemplate>
                                                            <asp:label runat="server" ID="lbl_nroEndoso" Text='<%# Eval("nro_endoso") %>' Width="30px" Font-Size="11px"></asp:label>
                                                            <asp:HiddenField runat="server" ID="hid_idPv" value='<%# Eval("id_pv") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo">
                                                        <ItemTemplate>
                                                                <asp:textbox runat="server" ID="lbl_GrupoEndoso" Enabled="false" Text='<%# Eval("GrupoEndoso")   %>' Width="160px" CssClass="form-control" Font-Size="9px" Height="26px" ></asp:textbox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Tipo">
                                                        <ItemTemplate>
                                                                <asp:textbox runat="server" ID="lbl_GrupoTipoEndoso" Enabled="false" Text='<%# Eval("TipoEndoso")   %>' Width="160px" CssClass="form-control" Font-Size="9px" Height="26px" ></asp:textbox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </asp:Panel>
                                           
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </div>
                    </div>                                                
                </div>
            </div>
        </div>
       
        <%-----------------------------------Sección 1----------------------------------------------------------------------------------------------------%>
        <%--<div style="width:900px; min-width:900px; overflow-x:hidden">
            <div class="panel-heading">
                <input type="image" src="../Images/collapse.png" id="coVentana0"  />
                <input type="image" src="../Images/expand.png"   id="exVentana0"  />
                <strong>Filtro Broker / Compañia</strong>
            </div>

            <div class="panel-body ventana0" >
                <div class="row">
                    <div class="col-md-6">
                        <div class="panel-heading">
                            <strong>Broker</strong>
                        </div>
                        
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upBroker">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlBroker" Width="415px" Height="130px" ScrollBars="Both">
                                                <asp:GridView runat="server" ID="gvd_Broker" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="chk_SelBro" value="false"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField >
                                                        <asp:TemplateField HeaderText="Clave">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_ClaveBro" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="10px" ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Descripción">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="310px" Font-Size="10px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <div style="width:100%;  text-align:right">
                                                <button type="button" id="btn_AddBroker" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal" >Añadir</button>
                                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="panel-heading">
                            <strong>Compañia</strong>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upCompañia">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlCompañia" Width="415px" Height="130px" ScrollBars="Both">
                                                <asp:GridView runat="server" ID="gvd_Compañia" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="chk_SelCia" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="ClaveCia">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_ClaveCia" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="9.5px" ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Descripción" ItemStyle-CssClass="DescripcionCia">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="310px" Font-Size="9.5px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <div style="width:100%;  text-align:right">
                                                <button type="button" id="btn_AddCia" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal" >Añadir</button>
                                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel> 
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-12">
                    <div class="panel-heading">
                    <input type="image" src="../Images/collapse.png" id="coVentana2"  />
                    <input type="image" src="../Images/expand.png"   id="exVentana2"  />
                    <strong>Filtros Ramos/Productos</strong>
                    </div>
                    <div class="panel-body ventana2" >
                        <asp:UpdatePanel runat="server" ID="upAdicionales">
                            <ContentTemplate>
                                    <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:UpdatePanel runat="server" ID="upRamoContable">
                                                        <ContentTemplate>
                                                            <div class="panel-heading">
                                                                <strong>Ramo Contable</strong>
                                                            </div>
                                                            <div class="clear padding10"></div>
                                                            <asp:Panel runat="server" ID="Panel1" Width="415px" Height="130px" ScrollBars="Both">
                                                                <asp:GridView runat="server" ID="gvd_RamoContable" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                    <asp:HiddenField runat="server" ID="chk_SelRamC" value="false"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField >
                                                                        <asp:TemplateField HeaderText="Clave">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_ClaveRamC" Text='<%# Eval("Clave") %>' Width="80px" Font-Size="10px" ></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Descripción">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="280px" Font-Size="10px"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <EditRowStyle BackColor="#999999" />
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            <div style="width:100%;  text-align:right">
                                                                <button type="button" id="btn_AddRamoContable" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                            </div>
                                     
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:UpdatePanel runat="server" ID="upProducto">
                                                        <ContentTemplate>
                                                            <div class="panel-heading">
                                                                <strong>Producto</strong>
                                                            </div>
                                                            <div class="clear padding10"></div>
                                                            <asp:Panel runat="server" ID="pnl_Producto" Width="415px" Height="130px" ScrollBars="Both">
                                                                <asp:GridView runat="server" ID="gvd_Producto" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                    <asp:HiddenField runat="server" ID="chk_SelPro" value="false"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField >
                                                                        <asp:TemplateField HeaderText="Clave">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_ClavePro" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="10px" ></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Descripción">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="310px" Font-Size="10px"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <EditRowStyle BackColor="#999999" />
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            <div style="width:100%;  text-align:right">
                                                                <button type="button" id="btn_AddProducto" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                            </div>
                                    </div>

                                   
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div>
                </div>
            </div>
        </div>--%>

    <div class="clear padding5"></div>

    <div  style="width:1120px; min-width:1120px; border:5px solid gray; border-width: 2px 0 0 0; text-align:right; padding: 0 0 0 0; "  >
        <asp:UpdatePanel runat="server" ID="upBusqueda">
            <ContentTemplate>
                <div class="form-group">
                    <div class="input-group">
                        <asp:Button runat="server" ID="btn_Generar" Font-Size="14px" CssClass="form-control btn-primary" Text="Generar Reporte" Width="140px" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

     <div class="clear padding20"></div>
     <div class="clear padding20"></div>
     <div class="clear padding20"></div>


<!-- Modal -->
    <div id="CatalogoModal" style="width:400px; height:640px"  class="modal">
          <div class="modal-content">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><label id="lblCatalogo" style="color:darkblue;">Catálogo</label></h4>
                    <asp:HiddenField runat="server" ID="hid_Control" Value="" />
               </div>

               <div class="modal-body" style="height:598px">
                   <asp:UpdatePanel runat="server" ID="upCatalogo">
                       <ContentTemplate>

                        <div class="input-group">
                            <asp:label runat="server" class="col-md-1 control-label" Width="50px">Filtro:</asp:label>
                            <input type="text" id="txtFiltrar" class="form-control" style="width:290px; height:26px; font-size:12px;" />
                        </div>

                           
                          <asp:Panel runat="server" ID="pnlCatalogo" Width="100%" Height="520px" ScrollBars="Vertical">
                              <asp:GridView runat="server" ID="gvd_Catalogo" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                  <Columns>
                                       <asp:TemplateField HeaderText="">
                                          <ItemTemplate>
                                              
                                          </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:BoundField  ItemStyle-Width="90px" ItemStyle-Height="10px" DataField="Clave" HeaderText="Clave" HeaderStyle-HorizontalAlign="Center"  />
                                       <asp:BoundField ItemStyle-Width="320px" ItemStyle-Height="10px" DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-HorizontalAlign="Center"  />
                                  </Columns>
                               
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                          </asp:Panel>

                          
                 
                          <div style="width:100%; text-align:right;">
                             <asp:ImageButton ID="btn_CatTodos" 
                                               runat="server" 
                                               ImageUrl="~/Images/BotonTodos.png"
                                               ToolTip="Seleccionar Todos"/>
                             <asp:ImageButton ID="btn_CatNinguno" 
                                               runat="server" 
                                               ImageUrl ="~/Images/BotonNinguno.png" 
                                               ToolTip="Borrar Seleccion" />
                             <label style="width:100px"></label>
                             <asp:Button runat="server" id="btn_OkCatalogo" class="btn btn-success" Text="Aceptar"  style="height:30px; width:80px;" />
                             <button type="button" id="btn_CnlCatalogo" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cancelar</button>
                             <asp:HiddenField runat="server" ID="hid_Seleccion" Value="" />
                             <asp:HiddenField runat="server" ID="hid_Catalogo" Value="" />
                          </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          </div>
    </div>

    <!-- Modal -->
    <div id="PolizaModal" style="width:500px; height:520px"  class="modalPoliza">
          <%--<div class="modal-content">--%>
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title"><label id="lblPoliza" style="color:darkblue;">Pólizas</label></h4>
               </div>

               <div class="modal-body" style="height:508px">
                   <asp:UpdatePanel runat="server" ID="upPolizas">
                       <ContentTemplate>

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:label runat="server" class="col-md-1 control-label" Width="80px">Sucursal</asp:label>
                                    <asp:DropDownList runat="server" ID="ddl_SucursalPol" CssClass="form-control panelPoliza" Width="370px" Height="28px"></asp:DropDownList>
                                </div>
                            </div>  

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:label runat="server" class="col-md-1 control-label" Width="80px">Ramo</asp:label>
                                    <asp:TextBox runat="server" ID="txtClaveRam" CssClass="form-control panelPoliza cod_ramo"  Width="70px" Height="26px" ></asp:TextBox>
                                    <button type="button" id="btn_SelRam" class="btn btn-info panelPoliza" data-toggle="modal"  style="Width:36px; Height:26px;" data-target="#EsperaModal" >...</button>
                                    <asp:TextBox runat="server" ID="txtSearchRam" CssClass="form-control panelPoliza desc_ramo" Width="262px" Height="26px" ></asp:TextBox>     
                                </div>
                            </div>  

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:label runat="server" class="col-md-1 control-label" Width="80px">Póliza</asp:label>
                                    <asp:TextBox runat="server" ID="txt_NoPoliza" CssClass="form-control panelPoliza NroPol"  Width="70px" Height="26px" ></asp:TextBox>
                                    <asp:label runat="server"  Width="10px"></asp:label>
                                    <asp:Button runat="server" ID="btn_BuscaSufijo" CssClass="form-control btn-primary" Text="Busca Sufijos" Width="130px" Height="26px"  />
                                    <asp:Button runat="server" ID="btn_CancelaSufijo" CssClass="form-control btn-danger" Text="Cancelar" Width="130px" Height="26px"  />
                                    <%--<asp:ImageButton runat="server" ID="btn_BuscaEndoso" ImageUrl="~/Images/buscaPol-icon.png" />
                                    <asp:ImageButton runat="server" ID="btn_CancelaEndoso" ImageUrl="~/Images/cancelaPol-icon.png" Enabled="false"/>--%>
                                </div>
                            </div> 
                            <div  style="width:100%; height:290px; overflow-y: scroll;  overflow-x: scroll; ">
                                <asp:GridView runat="server" ID="gvd_GrupoPolizas" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chk_SelPol" />
                                                    <asp:HiddenField runat="server" ID="hid_IdPv" Value="" />
                                            </ItemTemplate>
                                        </asp:TemplateField >

                                         <asp:TemplateField HeaderText="Sucursal">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_Sucursal" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("cod_suc")   %>'  Width="30px" Height="26px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Ramo">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_Ramo" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("cod_ramo")   %>'  Width="30px" Height="26px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Poliza">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_Poliza" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("nro_pol")   %>'  Width="70px" Height="26px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sufijo">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_Sufijo" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("aaaa_endoso")   %>'  Width="30px" Height="26px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Endosos">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_NoEndosos" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("nro_endosos")   %>'  Width="30px" Height="26px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>


                            <div style="width:100%; text-align:right;">
                                 <asp:Button runat="server" id="btn_OkPoliza" class="btn btn-success" Text="Agregar"  style="height:30px; width:80px;" />
                                 <button type="button" id="btn_CnlPoliza" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          <%--</div>--%>
    </div>

    
    

    
    <div id="EsperaModal" style="width:150px; height:95px"  class="modalWait">
        <img src="../Images/gmx_mini.png" />
        Procesando.....
    </div>

     <!-- Modal -->
    <div id="MensajeModal" style="width:400px; height:185px"  class="modalAutoriza">
         <%-- <div class="modal-content">--%>
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title">
                         <asp:Label runat="server" ID="lbl_TitMensaje" Text="" Width="400px" style="color:darkblue;"></asp:Label>
                     </h4>
               </div>

               <div class="modal-body" style="height:143px">
                   <asp:UpdatePanel runat="server" ID="upMensaje">
                       <ContentTemplate>
                            <asp:label ID="txt_Mensaje" runat="server" BorderStyle="None" Width="368px" Height="80px" TextMode="MultiLine"></asp:label>
                            <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right;">
                                 <button type="button" id="btn_CnlMensaje" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          <%--</div>--%>
    </div>

     <!-- Modal -->
    <div id="VersionModal" style="width:400px; height:185px"  class="modalAutoriza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                         Guardar Versión
                     </h4>
               </div>

               <div class="modal-body" style="height:143px">
                   <asp:UpdatePanel runat="server" ID="upModVersion">
                       <ContentTemplate>
                            <asp:HiddenField runat="server" ID="hid_version" Value="0" />
                            <asp:label runat="server" class="col-md-1 control-label" Width="90px">Descripción:</asp:label>
                            <asp:TextBox runat="server" ID="txt_descripcion" TextMode="MultiLine" Width="275px" Height="80px"></asp:TextBox>
                            <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right;">
                                 <asp:Button runat="server"  CssClass="btn btn-success" Width="80px" ID="btn_GuardarVer" Text="Aceptar" />
                                 <asp:Button runat="server" CssClass="btn btn-danger" Width="80px"  data-dismiss="modal" ID="btn_CancelarVer" Text="Cancelar" />
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>

    <!-- Modal -->
    <div id="SeleccionModal" style="width:400px;"  class="modal">
               <div class="modal-body" >
                   <asp:UpdatePanel runat="server" ID="upSeleccion">
                       <ContentTemplate>
                        <asp:HiddenField runat="server" ID="hid_Filtro" Value="" />
                        <asp:Panel runat="server" ID="pnl_Seleccion" Width="100%"  ScrollBars="Vertical">
                              <asp:GridView runat="server" ID="gvd_Seleccion" ShowHeader="false" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk_Cat" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Clave">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbl_Clave" Text='<%# Eval("Clave") %>' Width="80px" Font-Size="10px" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbl_Descripcion" Text='<%# Eval("Descripcion") %>' Width="250px" Font-Size="10px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                          </asp:Panel>

                          <div style="width:100%; text-align:right;">
                              <asp:ImageButton ID="btn_DesTodos" 
                                               runat="server" 
                                               ImageUrl="~/Images/BotonTodos.png"
                                               ToolTip="Seleccionar Todos"/>
                              <asp:ImageButton ID="btn_DesNinguno" 
                                               runat="server" 
                                               ImageUrl ="~/Images/BotonNinguno.png" 
                                               ToolTip="Borrar Seleccion" />
                               <label style="width:100px"></label>
                               <asp:Button runat="server" id="btn_Descartar" class="btn btn-primary" Text="Descartar"  style="height:30px; width:90px;" />
                               <button style="height:30px; width:90px;" data-dismiss="modal" class="btn btn-danger">Cerrar</button>
                          </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>
   
    <div id="ConfiguracionModal" style="width:500px; height:520px"  class="modalPoliza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title"><label  style="color:darkblue;">Configuración de Campos</label></h4>
               </div>

               <div class="modal-body" style="height:508px">
                   <asp:UpdatePanel runat="server" ID="upConfiguracion">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlConfiguracion" Height="450px" Width="470px" ScrollBars="Vertical">
                                    <asp:GridView runat="server" ID="gvd_Configuracion" DataKeyNames="posicion,Clave,Descripcion" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandName="QuitarElemento"  runat="server" ImageUrl="~/Images/Quitar.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Clave">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Clave" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="11px" ></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hid_posicion" Value='<%# Eval("posicion") %>'></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="hid_Seccion" Value='<%# Eval("nro_seccion") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Campo de Base de Datos">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="270px" Font-Size="11px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btn_Subir" CommandName="SubeElemento" ImageUrl="~/Images/icono_arriba.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btn_Bajar" CommandName="BajaElemento" ImageUrl="~/Images/icono_abajo.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>
</asp:Content>

