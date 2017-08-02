<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SiteMaster.master" AutoEventWireup="false" CodeFile="Reporteador.aspx.vb" Inherits="Pages_Reporteador" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
        <script src="../Scripts/jquery.maskedinput.js"></script>
        <script src="../Scripts/Reporteadores.js"></script>
        <script src="../Scripts/jquery.numeric.js"></script>
        
        <script type="text/javascript"> 
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);
        </script> 
        
        <asp:HiddenField runat="server" ID="hid_Ventanas" Value="1|1|1|1|" />
        <asp:HiddenField runat="server" ID="hid_Url" Value="" />
        <asp:Button ID="btn_CerrarSesion" runat="server" CssClass="NoDisplay CerrarSesion" />
        <asp:HiddenField ID="hid_CierraSesion" runat="server" Value="0" />

     <asp:UpdatePanel runat="server" ID="upListaVersiones">
        <ContentTemplate>
            <div class="row" style="width:1120px; min-width:1120px;">
                <div class="col-md-6">
                    <asp:HiddenField runat="server" ID="hid_codReporte" Value="2" />
                    <asp:HiddenField runat="server" ID="hid_version" Value="0" />
                    <asp:HiddenField runat="server" ID="hid_Temporal" Value="0" />
                    <asp:Label ForeColor="DarkBlue" runat="server" ID="lbl_Version" Font-Size="16px" Font-Bold="true" class="col-md-1 control-label" Text="Versión:"  Width="80px" ></asp:Label>
                    <asp:Label ForeColor="Red" runat="server" ID="lbl_DescVersion" Font-Size="16px" Text="Nueva Versión"  class="col-md-1 control-label"  Width="400px" ></asp:Label>
                </div>
                 <div class="col-md-6" style="padding-left:155px">
                     <asp:Button runat="server" ID="btn_Versiones" CssClass="form-control btn btn-primary" data-toggle="modal" data-target="#VersionesModal" Text="Versiones" Width="130px" />
                     <asp:Button runat="server" ID="btn_BI" CssClass="form-control btn btn-primary" data-toggle="modal" data-target="#VistasModal" Text="BI GMX" Width="130px" />
                </div>
            </div>
        </ContentTemplate>
     </asp:UpdatePanel>
    
    

 
    <table>
        <tr>
            <td style="text-align:start;">
                <div style="width:800px; min-width:960px;  overflow-x:hidden">
                    <div class="panel-heading">
                        <strong>Columnas</strong>
                    </div>
                    <div class="panel-body" >
                        <div class="row">

                             <asp:UpdatePanel runat="server" ID="up_Campos">
                                <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Generales" OnClick="Seleccionados" class="col-md-1 control-label"  Width="110px" >Generales</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Generales" CssClass="Seccion1"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion1" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Generales" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Reaseguro" OnClick="Seleccionados" class="col-md-1 control-label"  Width="110px" >Reaseguro</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Reaseguro"  CssClass="Seccion2" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion2" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Reaseguro" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Siniestros" OnClick="Seleccionados" class="col-md-1 control-label"  Width="100px" >Siniestros</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Siniestros"  CssClass="Seccion3" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion3" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Siniestros" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Cumulos" OnClick="Seleccionados" class="col-md-1 control-label" Width="90px" >Cúmulos</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Cúmulos" CssClass="Seccion4" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion4" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Cumulos" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Cobranzas" OnClick="Seleccionados" class="col-md-1 control-label" Width="110px" >Cobranzas</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Cobranzas" CssClass="Seccion5"  ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion5" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Cobranzass" Value="" />
                                                     </div>
                                                </td>
                                               
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Contabilidad" OnClick="Seleccionados" class="col-md-1 control-label"  Width="110px" >Contabilidad</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="Contabilidad" CssClass="Seccion6"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal" data-target="" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSeccion6" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Contabilidad" Value="" />
                                                     </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="clear padding10"></div>
                                        <div style="width:100%; text-align:right;">
                                            <asp:Button runat="server" ID="btn_Configuracion" Visible="false" Width="130px" CssClass="btn btn-info" data-toggle="modal" data-target="#ConfiguracionModal" Text="Personalizar" />
                                            <asp:Button runat="server" ID="btn_AgregaFormula" Visible="false" Width="130px" CssClass="btn btn-info" data-toggle="modal" data-target="#FormulasModal" Text="Fórmulas" />
                                            <asp:Button runat="server" ID="btn_Agrupador" Width="130px" CssClass="btn btn-info" Text="Agrupadores" />
                                        </div>
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
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Fecha Emisión</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FechaIni" CssClass="form-control FechaSB" Width="110px" Height="26px" ></asp:TextBox>
                                                        <div class="clear padding5"></div>
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FechaFin" CssClass="form-control FechaSB" Width="110px" Height="26px"  ></asp:TextBox>
                                                        <div class="clear padding5"></div>
                                            </td>
                                            <td>
                                                        
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Fin de Vigencia</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FinVigDe" CssClass="form-control FechaSB" Width="110px" Height="26px" ></asp:TextBox>
                                            </td>
                                            <td>
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_FinVigA" CssClass="form-control FechaSB" Width="110px" Height="26px"  ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>    
                                    <div class="clear padding5"></div>
                                    <table>

                                            <tr>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Poliza" OnClick="Seleccionados"  class="col-md-1 control-label" Width="130px" >Polizas</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="btn_Poliza" CssClass="Endosos"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal" data-target="#PolizaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdSufijos" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Sufijos" Value="" />
                                                     </div>
                                                </td>
                                                <td> 
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Broker" OnClick="Seleccionados" class="col-md-1 control-label" Width="130px" >Brokers</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="btn_Broker" CssClass="Broker"  ImageUrl="~/Images/buscaPol-icon.png"  data-toggle="modal"  data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdBroker" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Broker" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Compañia" OnClick="Seleccionados" class="col-md-1 control-label" Width="130px" >Compañias</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="btn_Compañia"  CssClass="Compañia" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdCompañia" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_Compañia" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_RamoCOntable" OnClick="Seleccionados" class="col-md-1 control-label" Width="165px"  >Ramos Contables</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="btn_RamoContable" CssClass="RamoContable" ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdRamoContable" Value="" />
                                                         <asp:HiddenField runat="server" ID="hid_RamoContable" Value="" />
                                                     </div>
                                                </td>
                                                <td>
                                                     <div class="panel-heading">
                                                         <asp:LinkButton ForeColor="DarkBlue" runat="server" ID="lnk_Producto" OnClick="Seleccionados" class="col-md-1 control-label" Width="130px" >Productos</asp:LinkButton>
                                                         <asp:ImageButton runat="server" ID="btn_Producto" CssClass="Producto"  ImageUrl="~/Images/buscaPol-icon.png" data-toggle="modal" data-target="#EsperaModal" />
                                                         <asp:HiddenField runat="server" ID="hid_IdProducto" Value="" /> 
                                                         <asp:HiddenField runat="server" ID="hid_Producto" Value="" />
                                                     </div>
                                                </td>

                                            </tr>
                                        </table>
                                        <div class="clear padding10"></div>
                                        <div style="width:100%; text-align:right;">
                                             <asp:Button runat="server" ID="btn_AgregaConsulta" Width="130px" CssClass="btn btn-info" data-toggle="modal" data-target="#FiltrosModal" Text="Consultas" />
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
       
    <div class="clear padding5"></div>

     <asp:UpdatePanel runat="server" ID="upGuardar">
          <ContentTemplate>
                <div class="row">
                    <div class="col-md-6" style="width:520px; min-width:520px; border:5px solid gray; border-width: 2px 0 0 0;"  >
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btn_Generar" Font-Size="16px" CssClass="form-control btn-primary" Text="Generar Reporte" Width="160px" />
                                </td>
                                <td style="width:5px;">

                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btn_Actualizar" Font-Size="16px" CssClass="form-control btn-success" Enabled="false" Text="Actualizar Temporal" Width="160px" />
                                </td>
                            </tr>
                        </table> 
                        
                    </div>
                    <div class="col-md-6" style="width:600px; min-width:600px; border:5px solid gray; border-width: 2px 0 0 0; padding-left:60px;"  >                         
                         <asp:Button runat="server" CssClass="btn btn-primary" Width="130px" ID="btn_Nuevo" Text="Nueva Versión" />
                         <asp:Button runat="server" CssClass="btn btn-primary" Width="130px" ID="btn_GuardarVer" data-toggle="modal" data-target="#GuardaVersionModal" Text="Guardar Versión" />
                         <asp:Button runat="server" CssClass="btn btn-danger" Width="130px" ID="btn_EliminaVer" Text="Elimina Versión" />
                    </div>
                </div>
          </ContentTemplate>
    </asp:UpdatePanel>


                           





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
                            <asp:label runat="server"  class="col-md-1 control-label" Width="50px">Filtro:</asp:label>
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
                         <asp:Label runat="server" ID="lbl_TitMensaje" Text="" Width="350px" style="color:darkblue;"></asp:Label>
               </div>

               <div class="modal-body" style="height:143px">
                   <asp:UpdatePanel runat="server" ID="upMensaje">
                       <ContentTemplate>
                            <asp:label ID="txt_Mensaje" runat="server" BorderStyle="None" Width="368px" Height="80px" TextMode="MultiLine"></asp:label>
                            <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right; border-top:inset;border-top-width:1px; padding-top:3px;">
                                 <button type="button" id="btn_CnlMensaje" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          <%--</div>--%>
    </div>

    <div id="ReplicaModal" style="width:400px; height:185px"  class="modalAutoriza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
               </div>


               <div class="modal-body" style="height:143px">
                   <asp:UpdatePanel runat="server" ID="upReplicar">
                       <ContentTemplate>
                            <label>¿Cuantas veces desea replicar la(s) condicion(es) seleccionada(s)?</label>
                            <div class="clear padding10"></div>
                            <asp:label runat="server" class="col-md-1 control-label"  Width="80px">#Veces</asp:label>
                            <asp:DropDownList runat="server" ID="ddl_Replicas" Width="60px" Height="24px" CssClass="form-control">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                           <div class="clear padding5"></div>
                           <div style="width:100%; text-align:right; border-top:inset;border-top-width:1px; padding-top:3px;">
                                 <asp:Button runat="server" ID="btn_AplicaReplica" class="btn btn-success" Height="30px" Width="80px" Text="Aplicar" />
                                 <button type="button" id="btn_CierraReplica" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>

     <!-- Modal -->
    <div id="GuardaVersionModal" style="width:400px; height:235px"  class="modalAutoriza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                         Guardar Versión
                     </h4>
               </div>
               <asp:UpdatePanel runat="server" ID="upModVersion">
                   <ContentTemplate>
                       <div class="modal-body" style="height:153px">
                            <asp:CheckBox runat="server" Font-Size="13px" Width="365px" ID="chk_Temporal" Text="Ambiente Temporal Agrupadores" CssClass="form-control" />
                          
                            <div class="clear padding5"></div>

                            <asp:label runat="server" class="col-md-1 control-label" Width="90px">Descripción:</asp:label>
                            <asp:TextBox runat="server" ID="txt_descripcion" TextMode="MultiLine" Width="275px" Height="90px"></asp:TextBox>
                            
                            <div class="clear padding5"></div>
                      </div>
                      <div style="width:100%; padding-left:215px; border-top:inset;border-top-width:1px; padding-top:3px;">
                            <asp:Button runat="server" ID="btn_ConfirmaVersion" class="btn btn-success" Height="30px" Width="80px" Text="Guardar" />
                            <asp:Button runat="server" ID="btn_CierraVersion" class="btn btn-danger" Height="30px" Width="80px" Text="Cancelar" />
                      </div>
                    </ContentTemplate>
              </asp:UpdatePanel>
    </div>

    <!-- Modal -->
    <div id="SeleccionModal" style="width:400px; height:640px"  class="modal">
               <div class="modal-body" >
                   <asp:UpdatePanel runat="server" ID="upSeleccion">
                       <ContentTemplate>
                        <asp:HiddenField runat="server" ID="hid_Filtro" Value="" />
                        <asp:Panel runat="server" ID="pnl_Seleccion" Width="100%" Height="570px" ScrollBars="Vertical">
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
   
    <div id="ConfiguracionModal" style="width:420px; height:640px"  class="modalPoliza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title"><label  style="color:darkblue;">Columnas Agregadas</label></h4>
               </div>

               <div class="modal-body" style="height:598px">
                   <asp:UpdatePanel runat="server" ID="upConfiguracion">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlConfiguracion" Height="550px" Width="490px" ScrollBars="Vertical">
                                    <asp:GridView runat="server" ID="gvd_Configuracion" DataKeyNames="posicion,Clave,Descripcion" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chk_Sel" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Clave">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Clave" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="8.5px" ></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hid_posicion" Value='<%# Eval("posicion") %>'></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="hid_Seccion" Value='<%# Eval("nro_seccion") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Columna">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="270px" Font-Size="8.5px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          <%-- <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btn_Subir" CommandName="SubeElemento" ImageUrl="~/Images/icono_arriba.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btn_Bajar" CommandName="BajaElemento" ImageUrl="~/Images/icono_abajo.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        
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
                   <div style="width:100%; text-align:right;">
                        <asp:Button runat="server" id="btn_OkConfig" class="btn btn-success" Text="Aceptar"  style="height:30px; width:80px;" />
                   </div>
              </div>     
    </div>

    <div id="VersionesModal" style="width:900px; height:500px;" class="modalExhibiciones">
        <div class="modal-header" style="height:40px">
            <button type="button" class="close"  data-dismiss="modal">&times;</button>
            <h4 class="modal-title"><label  style="color:darkblue;">Versiones Generadas</label></h4>
        </div>
        <div class="panel-body" >
            <div class="row">
                <asp:UpdatePanel runat="server" ID="upVersiones">
                    <ContentTemplate>
                        <div style="height:400px; overflow-y:scroll; overflow-x:hidden;">
                            <asp:GridView runat="server" ID="gvd_Versiones"  AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk_SelCol" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID">
                                        <ItemTemplate>
                                            <asp:label runat="server" ID="lbl_Config" ForeColor="Red" Font-Bold="true" CssClass="form-control Centro" Text='<%# Eval("cod_config") %>' Height="24px" Width="30px" Font-Size="10px"></asp:label>
                                            <asp:hiddenfield runat="server" ID="hid_Reporte"  Value='<%# Eval("cod_reporte") %>'></asp:hiddenfield>
                                            <asp:hiddenfield runat="server" ID="hid_Url"  Value='<%# Eval("Url") %>'></asp:hiddenfield>
                                            <asp:hiddenfield runat="server" ID="hid_Filtros"  Value='<%# Eval("filtros") %>'></asp:hiddenfield>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:linkbutton runat="server" ID="lbl_Consultar" CommandName="Version" ForeColor="darkblue" Font-Bold="true" CssClass="form-control Centro" Text="Consultar" Height="24px" Width="70px" Font-Size="10px"></asp:linkbutton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txt_Fecha" CssClass="form-control Centro" Text='<%# Eval("fecha") %>' Height="24px" Width="70px" Font-Size="10px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txt_Usuario" CssClass="form-control" Text='<%# Eval("Usuario") %>' Height="24px" Width="180px" Font-Size="10px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reporte">
                                        <ItemTemplate>
                                            <asp:textbox runat="server" ID="txt_Reporte" Enabled="false" CssClass="form-control" Text='<%# Eval("descripcion") %>' Height="24px" Width="380px" Font-Size="10px"></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Temporal">
                                        <ItemTemplate>
                                            <div style="text-align:center">
                                                <asp:CheckBox runat="server" ID="chk_Temporal" AutoPostBack="true"  Checked='<%# Eval("Temporal") %>' CssClass="Temporal" OnCheckedChanged="chk_Temporal_CheckedChanged" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Formato">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddl_Formato" AutoPostBack="true" OnSelectedIndexChanged="ddl_Formato_SelectedIndexChanged"  CssClass="form-control" Height="24px" Width="70px" Font-Size="10px">
                                                <asp:ListItem Text="NAV" value="NAV"></asp:ListItem>
                                                <asp:ListItem Text="EXCEL" value="EXCEL"></asp:ListItem>
                                                <asp:ListItem Text="PDF" value="PDF"></asp:ListItem>
                                                <asp:ListItem Text="WORD" value="WORD"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:label runat="server" ID="lbl_formato" Visible="false" CssClass="form-control" Text='<%# Eval("formato") %>' Height="24px" Width="70px" Font-Size="10px"></asp:label>
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
                        </div>   
                        <div style="width:100%; border-top:inset;border-top-width:1px; padding-top:3px; padding-left:720px" >
                                <asp:Button runat="server" ID="btn_GenerarMultiple" class="btn btn-success" Height="30px" Width="80px" Text="Generar" />
                                <button type="button" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="MultivaloresModal" style="width:300px; height:350px"  class="modalAutoriza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                         Valores Multiples
                     </h4>
               </div>
              
               <div class="modal-body" style="height:308px">
                   <asp:UpdatePanel runat="server" ID="upValoresMultiples">
                       <ContentTemplate>
                           <asp:Label runat="server" ID="lbl_errorDato" Text="Debe elegir la opción Alfanúmerico" ForeColor="Red" Visible="false"></asp:Label>
                           <asp:HiddenField runat="server" ID="hid_RowCondicion" Value="-1" />
                           <asp:HiddenField runat="server" ID="hid_Todos" Value="0" />
                           <asp:RadioButtonList runat="server" ID="opt_TipoDatoFiltro" AutoPostBack="true" CssClass="rbl" RepeatDirection="Horizontal">
                               <asp:ListItem Text="Númericos" Selected="true" Value="N"/>
                               <asp:ListItem Text="Alfanúmericos" Value="A" />
                           </asp:RadioButtonList>
                             
                           <div style="height:180px; overflow-y:scroll; overflow-x:hidden;">
                                <asp:GridView runat="server" ID="gvd_Multiples" ShowHeader="false" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chk_SelCol" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor" ItemStyle-CssClass="ClaveCia">
                                            <ItemTemplate>
                                                <asp:textbox runat="server" ID="txt_Valor" CssClass="form-control" Text='<%# Eval("Valor") %>' Height="24px" Width="235px" Font-Size="10px"></asp:textbox>
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
                            </div>   
                           <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right;">
                                 <asp:Button runat="server" ID="btn_AñadirValor" class="btn btn-info" Font-Size="9px" Height="24px" Width="50px" Text="Añadir" />
                                 <asp:Button runat="server" ID="btn_QuitarValor" class="btn btn-danger" Font-Size="9px" Height="24px" Width="50px" Text="Quitar" />
                            </div>
                            <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right; border-top:inset;border-top-width:1px; padding-top:3px;">
                                 <asp:Button runat="server" ID="btn_AplicaValores" class="btn btn-success" Height="30px" Width="80px" Text="Aplicar" />
                                 <button type="button" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>

    <div id="AgrupacionModal" style="width:1300px; min-width:1300px; min-height:700px; height:700px"  class="modalOrdenPago">
        <div class="modal-header" style="height:40px">
            <button type="button" class="close"  data-dismiss="modal">&times;</button>
                <h4 class="modal-title"><label id="lblGroup" style="color:darkblue;">Agrupadores</label></h4>
        </div>

        <div class="modal-body" style="height:700px">
            <asp:UpdatePanel runat="server" ID="upAgrupador">
                <ContentTemplate>
                    <asp:CheckBoxList runat="server" ID="chk_Listado" Visible="false">
                    </asp:CheckBoxList> 
                   <table>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Generales</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;" >
                                    <asp:CheckBoxList runat="server" ID="chk_Generales" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Reaseguro</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;" >
                                    <asp:CheckBoxList runat="server" ID="chk_Reaseguro" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Siniestros</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_Siniestros" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Cúmulos</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_Cumulos" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Cobranzas</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_Cobranzas" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Contabilidad</strong>
                               </div>
                               <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_Contabilidad" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                   </table>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="row">
                <div  class="col-md-6" style="width:550px; min-width:550px; border:5px solid gray; border-width: 1px 0 0 0;"  >
                    <asp:UpdatePanel runat="server" ID="upAcciones">
                        <ContentTemplate>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:Button runat="server" ID="btn_Filas" Font-Size="12px" CssClass="form-control btn-primary" Text="Mover a Filas" Width="160px" />
                                    <asp:Button runat="server" ID="btn_Valores" Font-Size="12px" CssClass="form-control btn-success" Text="Mover a Valores ∑" Width="160px" />
                                    <asp:Button runat="server" ID="btn_Filtro" Font-Size="12px" CssClass="form-control btn-info" Text="Añadir a Filtrado" Width="160px" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="col-md-6"   style="width:700px; min-width:700px; border:5px solid gray; border-width: 1px 0 0 0;" >
                    <asp:UpdatePanel runat="server" ID="upGenerar">
                        <ContentTemplate>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:label runat="server" class="col-md-1 control-label" Font-Bold="true" Width="110px">Agrupaciones:</asp:label>
                                    <asp:DropDownList runat="server" Font-Size="13px" AutoPostBack="true" Height="35px" ID="ddl_Agrupaciones" CssClass="form-control" Width="240px" ></asp:DropDownList>
                                    <asp:Button runat="server" ID="btn_GuardarAgrupacion" Font-Size="12px" CssClass="form-control btn-success" Text="Guardar" Width="160px" />
                                    <asp:Button runat="server" ID="btn_GenerarAgrupacion" Font-Size="12px" CssClass="form-control btn-primary" Text="Generar" Width="160px" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            

             

            <asp:UpdatePanel runat="server" ID="upFilasValores">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <div style="width:273px;">
                                    <div class="panel-heading">
                                        <strong>Filas</strong>
                                    </div>
                                    <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                        <asp:GridView runat="server" ID="gvd_Filas" ShowHeader="false" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk_SelCol" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fila" ItemStyle-CssClass="ClaveCia">
                                                    <ItemTemplate>
                                                        <asp:label runat="server" ID="lbl_Columna" Text='<%# Eval("Columna") %>' Width="190px" Font-Size="11px"></asp:label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk_Parcial" />
                                                        <asp:HiddenField runat="server" ID="hid_Codigo" Value='<%# Eval("cod_campo") %>' />
                                                        <asp:HiddenField runat="server" ID="hid_seccion" Value='<%# Eval("Seccion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:label runat="server" ID="lbl_Parcial" Text=" ∑ " Font-Size="14px" Font-Bold="true"></asp:label>
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
                                    </div>   
                                    <div style="width:100%; padding-left:161px">
                                        <asp:Button runat="server" ID="btn_FiltroFila" Font-Size="11px" CssClass="btn btn-info" Text="Filtro" /> 
                                        <asp:Button runat="server" ID="btn_QuitaFilas" Font-Size="11px" CssClass="btn btn-danger" Text="Quitar" />
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div style="width:273px;">
                                    <div class="panel-heading">
                                        <strong>Valores ∑</strong>
                                    </div>
                                    <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                                        <asp:GridView runat="server" ID="gvd_Valores" ShowHeader="false" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk_SelCol" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valores" ItemStyle-CssClass="ClaveCia">
                                                    <ItemTemplate>
                                                        <asp:label runat="server" ID="lbl_Columna" Text='<%# Eval("Columna") %>' Width="220px" Font-Size="11px"></asp:label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hid_Codigo" Value='<%# Eval("cod_campo") %>' />
                                                        <asp:HiddenField runat="server" ID="hid_seccion" Value='<%# Eval("Seccion") %>' />
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
                                    </div>   
                                    <div style="width:100%; padding-left:161px">
                                        <asp:Button runat="server" ID="btn_FiltroValor" Font-Size="11px" CssClass="btn btn-info" Text="Filtro" />    
                                        <asp:Button runat="server" ID="btn_QuitaValores" Font-Size="11px" CssClass="btn btn-danger" Text="Quitar" />   
                                    </div>
                                </div>
                            </td>

                            <td style="vertical-align:top;">
                                <div style="width:682px;">
                                    <div class="panel-heading">
                                        <strong>Filtros</strong>
                                    </div>
                                    <div class="panel-body" style="height:250px; overflow-y:scroll; overflow-x:scroll;">
                                        <asp:GridView runat="server" ID="gvd_Filtros" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk_SelCol" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbl_Where" CssClass="form-control" Width="75px" Text="EN DONDE: " Font-Bold="true" ForeColor="DarkBlue" Height="24px" Visible="false" ></asp:Label>
                                                        <asp:DropDownList CssClass="form-control" ForeColor="DarkBlue" Font-Bold="true" Width="75px" Height="24px" runat="server" ID="ddl_Union" Visible="true" >
                                                            <asp:ListItem Text="Y" Value="AND" ></asp:ListItem>
                                                            <asp:ListItem Text="O" Value="OR"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField runat="server" ID="hid_Union" Value='<%# Eval("Union") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filtro" ItemStyle-CssClass="ClaveCia">
                                                    <ItemTemplate>
                                                        <asp:label runat="server" Height="24px" CssClass="form-control" ID="lbl_Columna" Text='<%# Eval("Columna") %>' Width="170px" Font-Size="11px"></asp:label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hid_Codigo" Value='<%# Eval("cod_campo") %>' />
                                                        <asp:HiddenField runat="server" ID="hid_seccion" Value='<%# Eval("Seccion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:DropDownList CssClass="form-control" Width="130px" Font-Bold="true" ForeColor="DarkBlue" Height="24px" runat="server" ID="ddl_Operador" >
                                                            <asp:ListItem Text="IGUAL A" Value="="></asp:ListItem>
                                                            <asp:ListItem Text="MENOR A" Value="<"></asp:ListItem>
                                                            <asp:ListItem Text="MAYOR A" Value=">"></asp:ListItem>
                                                            <asp:ListItem Text="MENOR O IGUAL A" Value="<="></asp:ListItem>
                                                            <asp:ListItem Text="MAYOR O IGUAL A" Value=">="></asp:ListItem>
                                                            <asp:ListItem Text="DIFERENTE A" Value="<>"></asp:ListItem>
                                                            <asp:ListItem Text="COMIENCE CON.." Value="LIKE @%"></asp:ListItem>
                                                            <asp:ListItem Text="TERMINE CON..." Value="LIKE %@"></asp:ListItem>
                                                            <asp:ListItem Text="CONTENGA..." Value="LIKE %@%"></asp:ListItem>
                                                            <asp:ListItem Text="NO CONTENGA..." Value="NOT LIKE %@%"></asp:ListItem>
                                                            <asp:ListItem Text="DENTRO DE" Value="IN(@)"></asp:ListItem>
                                                            <asp:ListItem Text="NO DENTRO DE" Value="NOT IN(@)"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField runat="server" ID="hid_Operador" Value='<%# Eval("Operador") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Condiciones">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox CssClass="form-control" Height="24px"  runat="server" ID="txt_Condicion" Enabled="false" Text='<%# Eval("Condicion") %>' Width="220px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button runat="server" Height="24px" CommandName="Multiples" CssClass="btn btn-info" Font-Bold="true" Text="+" Font-Size="10px" />
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                    </div>   
                                    <div style="width:100%; padding-left:553px">
                                        <asp:Button runat="server" ID="btn_ReplicaFiltro" Font-Size="11px" CssClass="btn btn-info" Text="Replicar" />    
                                        <asp:Button runat="server" ID="btn_QuitaFiltro" Font-Size="11px" CssClass="btn btn-danger" Text="Quitar" />   
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

   <div id="FiltrosModal" style="width:1300px; min-width:1300px; min-height:700px; height:700px"  class="modalOrdenPago">
            <div class="panel-heading">
                <button type="button" class="close"  data-dismiss="modal">&times;</button>
                <strong>Consultas</strong>
            </div>
            <div style="padding-left:30px">
            <asp:UpdatePanel runat="server" ID="up_AllGenerales">
                <ContentTemplate>
                   <table>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Generales</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;" >
                                    <asp:CheckBoxList runat="server" ID="chk_AllGenerales" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Reaseguro</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;" >
                                    <asp:CheckBoxList runat="server" ID="chk_AllReaseguro" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Siniestros</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_AllSiniestros" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                            </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Cúmulos</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_AllCumulos" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Cobranzas</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_AllCobranzas" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                       <td>
                           <div style="width:205px;">
                               <div class="panel-heading">
                                   <strong>Contabilidad</strong>
                               </div>
                               <div class="panel-body" style="height:320px; overflow-y:scroll; overflow-x:hidden;">
                                    <asp:CheckBoxList runat="server" ID="chk_AllContanbilidad" Font-Size="9px">
                                    </asp:CheckBoxList> 
                               </div>
                           </div>
                       </td>
                   </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
             <div  class="col-md-6" style="width:100%; min-width:100%; border:5px solid gray; border-width: 1px 0 0 0;"  >
                <asp:UpdatePanel runat="server" ID="upConsulta">
                    <ContentTemplate>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:Button runat="server" ID="btn_AddConsulta" Font-Size="12px" CssClass="form-control btn-info" Text="Añadir a Consulta" Width="160px" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

             <asp:UpdatePanel runat="server" ID="up_Consultas">
                    <ContentTemplate>
                    <div class="panel-body" style="width:100%;height:230px; overflow-y:scroll; overflow-x:hidden;">
                        <asp:GridView runat="server" ID="gvd_Consulta" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk_SelCol" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Where" CssClass="form-control" Width="100px" Text="EN DONDE: " Font-Bold="true" ForeColor="DarkBlue" Height="24px" Visible="false" ></asp:Label>
                                        <asp:DropDownList CssClass="form-control" ForeColor="DarkBlue" Font-Bold="true" Width="100px" Height="24px" runat="server" ID="ddl_Union" Visible="true" >
                                            <asp:ListItem Text="Y" Value="AND" ></asp:ListItem>
                                            <asp:ListItem Text="O" Value="OR"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hid_Union" Value='<%# Eval("Union") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filtro" ItemStyle-CssClass="ClaveCia">
                                    <ItemTemplate>
                                        <asp:label runat="server" Height="24px" CssClass="form-control" ID="lbl_Columna" Text='<%# Eval("Columna") %>' Width="350px" Font-Size="12px"></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hid_Codigo" Value='<%# Eval("cod_campo") %>' />
                                        <asp:HiddenField runat="server" ID="hid_seccion" Value='<%# Eval("Seccion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:DropDownList CssClass="form-control" Width="200px" Font-Bold="true" ForeColor="DarkBlue" Height="24px" runat="server" Font-Size="12px" ID="ddl_Operador" >
                                            <asp:ListItem Text="IGUAL A" Value="="></asp:ListItem>
                                            <asp:ListItem Text="MENOR A" Value="<"></asp:ListItem>
                                            <asp:ListItem Text="MAYOR A" Value=">"></asp:ListItem>
                                            <asp:ListItem Text="MENOR O IGUAL A" Value="<="></asp:ListItem>
                                            <asp:ListItem Text="MAYOR O IGUAL A" Value=">="></asp:ListItem>
                                            <asp:ListItem Text="DIFERENTE A" Value="<>"></asp:ListItem>
                                            <asp:ListItem Text="COMIENCE CON.." Value="LIKE @%"></asp:ListItem>
                                            <asp:ListItem Text="TERMINE CON..." Value="LIKE %@"></asp:ListItem>
                                            <asp:ListItem Text="CONTENGA..." Value="LIKE %@%"></asp:ListItem>
                                            <asp:ListItem Text="NO CONTENGA..." Value="NOT LIKE %@%"></asp:ListItem>
                                            <asp:ListItem Text="DENTRO DE" Value="IN(@)"></asp:ListItem>
                                            <asp:ListItem Text="NO DENTRO DE" Value="NOT IN(@)"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hid_Operador" Value='<%# Eval("Operador") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condiciones">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox CssClass="form-control" Height="24px"  runat="server" ID="txt_Condicion" Enabled="false" Text='<%# Eval("Condicion") %>' Font-Size="12px" Width="550px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Multiples" Height="24px" CommandName="Multiples" CssClass="btn btn-info" Font-Bold="true" Text="+" Font-Size="10px" />
                                                </td>
                                            </tr>
                                        </table>
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
                    </div>   
                    <div style="width:100%; padding-left:1150px">
                        <asp:Button runat="server" ID="btn_ReplicaConsulta" Font-Size="11px" CssClass="btn btn-info" Text="Replicar" />    
                        <asp:Button runat="server" ID="btn_QuitaConsulta" Font-Size="11px" CssClass="btn btn-danger" Text="Quitar" />   
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>

    <div id="FormulasModal" style="width:1300px; min-width:1300px; min-height:700px; height:700px"  class="modalOrdenPago">
            <div class="panel-heading">
                <button type="button" class="close"  data-dismiss="modal">&times;</button>
                <strong>Formúlas</strong>
            </div>

            <asp:UpdatePanel runat="server" ID="upFormulas">
               <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hid_rowFormula" Value="0" />
                    <div class="panel-body" style="width:100%;height:600px; overflow-y:scroll; overflow-x:hidden;">
                        <asp:GridView runat="server" ID="gvd_Formulas" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk_SelCol" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" ItemStyle-CssClass="ClaveCia">
                                    <ItemTemplate>
                                        <asp:textbox runat="server" Height="24px" CssClass="form-control" ID="txt_Columna" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("Columna") %>' Width="250px" Font-Size="12px"></asp:textbox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="ClaveCia">
                                    <ItemTemplate>
                                        <asp:label runat="server" Height="24px" CssClass="form-control" Font-Bold="true" ForeColor="DarkBlue" Text="=" Font-Size="13px"></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Formula">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox CssClass="form-control Formula" Height="24px"  runat="server" ID="txt_Formula" BackColor="LightGray" Text='<%# Eval("Formula") %>' Font-Bold="true" Font-Size="12px" Width="945px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Multiples" Height="24px" CommandName="Multiples" CssClass="btn btn-info" Font-Bold="true" Text="+" Font-Size="10px" />
                                                </td>
                                            </tr>
                                        </table>
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
                    </div>   
                    <div style="width:100%; padding-left:1150px">
                        <asp:Button runat="server" ID="btn_NuevaFormula" Font-Size="11px" CssClass="btn btn-info" Text="Agregar" />    
                        <asp:Button runat="server" ID="btn_QuitaFormula" Font-Size="11px" CssClass="btn btn-danger" Text="Quitar" />   
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>

    <div id="SelFormulaModal" style="width:400px; height:640px"  class="modalPoliza">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title"><label  style="color:darkblue;">Columnas</label></h4>
               </div>
                
               <div class="modal-body" style="height:598px">
                   <asp:UpdatePanel runat="server" ID="up_ColFormula">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddl_seccion" ForeColor="DarkBlue" AutoPostBack="true" Width="370px" Font-Size="13px" CssClass="form-control">
                                <asp:ListItem Text="Generales" Value="Ge"></asp:ListItem>
                                <asp:ListItem Text="Reaseguro" Value="Re"></asp:ListItem>
                                <asp:ListItem Text="Siniestros" Value="Si"></asp:ListItem>
                                <asp:ListItem Text="Cúmulos" Value="Cu"></asp:ListItem>
                                <asp:ListItem Text="Cobranzas" Value="Cb"></asp:ListItem>
                                <asp:ListItem Text="Contabilidad" Value="Co"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="panel-body" style="height:510px; overflow-y:scroll; overflow-x:hidden;" >
                                <asp:CheckBoxList runat="server" ID="chk_ForGenerales" Font-Size="9px">
                                </asp:CheckBoxList> 
                                <asp:CheckBoxList runat="server" ID="chk_ForReaseguro" Visible="false" Font-Size="9px">
                                </asp:CheckBoxList> 
                                <asp:CheckBoxList runat="server" ID="chk_ForSiniestros" Visible="false"  Font-Size="9px">
                                </asp:CheckBoxList> 
                                <asp:CheckBoxList runat="server" ID="chk_ForCumulos" Visible="false"  Font-Size="9px">
                                </asp:CheckBoxList> 
                                <asp:CheckBoxList runat="server" ID="chk_ForCobranzas" Visible="false"  Font-Size="9px">
                                </asp:CheckBoxList> 
                                <asp:CheckBoxList runat="server" ID="chk_ForContabilidad" Visible="false"  Font-Size="9px">
                                </asp:CheckBoxList> 
                            </div>
                            <div style="width:100%; text-align:right;">
                                <asp:Button runat="server" id="btn_OkSelFormula" class="btn btn-success" Text="Aceptar"  style="height:30px; width:80px;" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </div>     
    </div>

    <div id="ResultadoModal" style="width:1300px; min-width:1300px; min-height:700px; height:700px"  class="modalOrdenPago">
        <div class="modal-header" style="height:40px">
            <h4 class="modal-title"><label  style="color:darkblue;">Resultado</label></h4>
        </div>
        <div class="panel-body" >
            <div class="row">
                <asp:UpdatePanel runat="server" ID="upResultado">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="hid_Paginas" Value="-1" />
                        <div style="height:600px; width:100%; overflow-y:scroll; overflow-x:scroll;">
                            <asp:GridView runat="server" AllowPaging="true" PageSize="1000" ID="gvd_Resultado" Font-Size="12.5px"  AutoGenerateColumns="true" ShowHeader="true" ForeColor="#333333" GridLines="Vertical"  
                                ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="gridview" />
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="Primero" LastPageText="Ultimo" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </div>   
                        <div style="width:100%; border-top:inset;border-top-width:1px; padding-top:3px; padding-left:720px" >
                             <asp:Button runat="server" ID="btn_CerrarResultado" Font-Size="11px" CssClass="btn btn-danger" Text="Cerrar" />   
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div id="VistasModal" style="width:1350px; min-width:1350px; min-height:680px; height:680px"  class="modalOrdenPago">
        <div class="modal-header" style="height:40px">
            <button type="button" class="close"  data-dismiss="modal">&times;</button>
            <h4 class="modal-title"><label  style="color:darkblue;">Business Intelligence GMX</label></h4>
        </div>
        <div class="modal-body" style="height:640px">

            <table>
                <tr>
                    <td style="width:1100px; height:590px">
                          <div id="div_tabla1" runat="server" visible="true">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla1">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla1" Text="Tabla1" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla1">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla1" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                          </div>

                          <div id="div_tabla2" runat="server" visible="true">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla2">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla2" Text="Tabla2" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla2">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla2" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla3" runat="server" visible="true">
                            <div  style="width:180px; height:250px"  class="modalTabla Tabla3">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla3" Text="Tabla3" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla3">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla3" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla4" runat="server" visible="true">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla4">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla4" Text="Tabla4" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla4">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla4" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla5" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla5">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla5" Text="Tabla5" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla5">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla5" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla6" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla6">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla6" Text="Tabla6" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla6">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla6" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla7" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla7">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla7" Text="Tabla7" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla7">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla7" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla8" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla8">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla8" Text="Tabla8" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla8">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla8" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>


                           <div id="div_tabla9" runat="server" visible="false">
                            <div id="" style="width:180px; height:250px"  class="modalTabla Tabla9">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla9" Text="Tabla9" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla9">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla9" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla10" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla10">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla10" Text="Tabla10" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla10">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla10" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla11" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla11">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla11" Text="Tabla11" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla11">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla11" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>

                           <div id="div_tabla12" runat="server" visible="false">
                            <div style="width:180px; height:250px"  class="modalTabla Tabla12">
                                <div class="tabla-header" style="height:27px">
                                    <asp:Label runat="server" ID="lbl_Tabla12" Text="Tabla12" ></asp:Label>
                                </div>
                                <div class="Barras">
                                   <asp:UpdatePanel runat="server" ID="up_Tabla12">
                                        <ContentTemplate>
                                           <asp:CheckBoxList runat="server" ID="lst_Tabla12" >
                                           </asp:CheckBoxList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                           </div>
                    </td>
                    <td style="width:200px; border-left:solid; border-color:lightgray;">
                         <asp:UpdatePanel runat="server" ID="upProyecto">
                            <ContentTemplate>
                                <div style="height:590px;">
                                        <h5>Proyecto</h5>
                                        <asp:LinkButton runat="server" ID="lnk_Vistas" Text="Vistas y Tablas" ></asp:LinkButton>
                                        <br />
                                        <asp:LinkButton runat="server" ID="lnk_Cubos" Text="Cubos" ></asp:LinkButton>
                                        <br />
                                        <asp:LinkButton runat="server" ID="lnk_Dimensiones" Text="Dimensiones" ></asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table> 
            <asp:UpdatePanel runat="server" ID="upBotones">
               <ContentTemplate>
                        <asp:Button runat="server" ID="btn_NuevoProyecto" Text="Nuevo Proyecto" />    
               </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>

