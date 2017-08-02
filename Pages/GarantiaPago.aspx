<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SiteMaster.master" AutoEventWireup="false" CodeFile="GarantiaPago.aspx.vb" Inherits="Pages_GarantiaPago" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/jquery.maskedinput.js"></script>
    <script src="../Scripts/jquery.numeric.js"></script>
    <script src="../Scripts/GarantiaPago.js"></script>

     <script type="text/javascript"> 
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);
    </script> 
     
    <asp:HiddenField runat="server" ID="hid_Ventanas" Value="1|1|" />
    <asp:HiddenField runat="server" ID="hid_Url" Value="" />

    <asp:HiddenField ID="hid_CierraSesion" runat="server" Value="0" />

    <asp:UpdatePanel runat="server" ID="upEstimacion">
        <ContentTemplate>
             <table>
                <tr>
                    <td>
                               <asp:label runat="server" class="col-md-1 control-label" Width="160px">Estimación de Pagos</asp:label>
                               <asp:CheckBox runat="server" ID="chk_Estimacion" AutoPostBack="true" />
                               <div class="clear padding5"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                               <asp:label runat="server" class="col-md-1 control-label" Width="160px">Moneda</asp:label>
                               <asp:DropDownList runat="server" ID="ddl_Moneda" CssClass="form-control" Width="130px" Height="26px"></asp:DropDownList>
                               <div class="clear padding5"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                                <asp:label runat="server" class="col-md-1 control-label" Width="160px">Fecha de Garantia</asp:label>
                                <asp:TextBox runat="server" ID="txt_FechaIni" CssClass="form-control FechaSB" Width="130px" Height="26px" ></asp:TextBox>
                    </td>
                    <td>
                                <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                <asp:TextBox runat="server" ID="txt_FechaFin" CssClass="form-control FechaSB" Width="130px" Height="26px"  ></asp:TextBox>
                    </td>
                    <td>
                                <asp:label runat="server" class="col-md-1 control-label" Width="150px">Mostrar Garantias: </asp:label>
                                <asp:RadioButtonList runat="server" ID="opt_Garantias" CssClass="rbl" RepeatDirection ="Horizontal">
                                    <asp:ListItem Text ="Todas" Value ="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text ="No Pagadas" Value ="1" Selected="False"></asp:ListItem>
                                    <asp:ListItem Text ="Pagadas" Value ="2" Selected="False"></asp:ListItem>
                                </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <%-----------------------------------Sección 2----------------------------------------------------------------------------------------------------%>
        <div class="panel-heading">
            <input type="image" src="../Images/collapse.png" id="coVentana0"  />
            <input type="image" src="../Images/expand.png"   id="exVentana0"  />
            <strong>Filtro Pólizas</strong>
        </div>

        <div class="panel-body ventana0" >
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

                                    <asp:Panel runat="server" ID="pnlPoliza" Width="415px" Height="143px" ScrollBars="Both">
                                            <asp:GridView runat="server" ID="gvd_Poliza" AutoGenerateColumns="false" DataKeyNames="Clave" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="chk_SelPol" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="ClaveCia">
                                                        <ItemTemplate>
                                                            <asp:Label  runat="server" ID="lbl_ClavePol" Text='<%# Eval("Clave") %>' Width="100px" Font-Size="11px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" >
                                                        <ItemTemplate> 
                                                            <asp:ImageButton runat="server" ID="btn_Endosos" CommandName="Endoso" ImageUrl="~/Images/buscaPol-icon.png" Width="27" Height="27" ToolTip="Busca Endosos" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Endosos">
                                                        <ItemTemplate>
                                                            <asp:label runat="server" ID="lbl_DescripcionPol" Enabled="false" Text='<%# Eval("Descripcion")   %>' Width="200px" Font-Size="11px" Font-Bold="true" ></asp:label>
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
                                            <button type="button" id="btn_AddPol" class="btn btn-success" data-toggle="modal" data-target="#PolizaModal" >Añadir</button>
                                        </div>
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

                                    <div class="clear padding10"></div>

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

        <div class="panel-heading">
        <input type="image" src="../Images/collapse.png" id="coVentana1"  />
        <input type="image" src="../Images/expand.png"   id="exVentana1"  />
        <strong>Filtros Ramos/Productos</strong>
        </div>
        <div class="panel-body ventana1" >
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

        <div class="clear padding5"></div>

        <div  style="width:900px; border:5px solid gray; border-width: 2px 0 0 0; text-align:right; padding: 0 0 0 0px; "  >
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


        <!-- Modal -->
        <div id="CatalogoModal" style="width:400px; height:440px"  class="modal">
              <div class="modal-content">
                   <div class="modal-header" style="height:40px">
                        <button type="button" class="close"  data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><label id="lblCatalogo" style="color:darkblue;">Catálogo</label></h4>
                        <asp:HiddenField runat="server" ID="hid_Control" Value="" />
                   </div>

                   <div class="modal-body" style="height:398px">
                       <asp:UpdatePanel runat="server" ID="upCatalogo">
                           <ContentTemplate>

                            <div class="input-group">
                                <asp:label runat="server" class="col-md-1 control-label" Width="50px">Filtro:</asp:label>
                                <input type="text" id="txtFiltrar" class="form-control" style="width:290px; height:26px; font-size:12px;" />
                            </div>

                           
                              <asp:Panel runat="server" ID="pnlCatalogo" Width="100%" Height="320px" ScrollBars="Vertical">
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
</asp:Content>

