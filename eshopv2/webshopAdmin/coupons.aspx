<%@ Page Title="" Language="C#" MasterPageFile="~/webshopAdmin/adminPanel.Master" AutoEventWireup="true" CodeBehind="coupons.aspx.cs" Inherits="webshopAdmin.coupons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Kuponi</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="btn-group">
                    <asp:Button ID="btnAddCoupon" runat="server" OnClick="btnAddCoupon_Click" Text="Dodaj kupon" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <asp:GridView ID="dgvCoupons" runat="server" AutoGenerateColumns="false" OnRowDeleting="dgvCoupons_RowDeleting"
                    DataKeyNames="couponID" CssClass="table table-condensed table-bordered table-hover table-striped">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCouponID" runat="server" Text='<%#Eval("couponID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Naziv">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkCoupon" runat="server" NavigateUrl='<%# "/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/coupon.aspx?couponID=" + Eval("couponID") %>'>
                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("name") %>'></asp:Label>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kod">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%#Eval("code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Važi od">
                            <ItemTemplate>
                                <asp:Label ID="lblDateFrom" runat="server" Text='<%#Eval("dateFrom") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Važi do">
                            <ItemTemplate>
                                <asp:Label ID="lblDateTo" runat="server" Text='<%#Eval("dateTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tip">
                            <ItemTemplate>
                                <asp:Label ID="lblCouponTypeName" runat="server" Text='<%#Eval("CouponType.Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="true" ControlStyle-Width="20px" DeleteImageUrl="images/delete_icon.png" ButtonType="Image" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
