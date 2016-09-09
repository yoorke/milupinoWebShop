<%@ Page Title="" Language="C#" MasterPageFile="~/webshopAdmin/adminPanel.Master" AutoEventWireup="true" CodeBehind="coupon.aspx.cs" Inherits="webshopAdmin.coupon" %>
<%@ Register Src="customControls/CustomStatus.ascx" TagName="Status" TagPrefix="status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Kupon</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="btn-group">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Sačuvaj" CssClass="btn btn-primary" />
                    <asp:Button ID="btnSaveClose" runat="server" OnClick="btnSaveClose_Click" Text="Sačuvaj i zatvori" CssClass="btn btn-default" />
                    <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Zatvori" CssClass="btn btn-default" />
                </div>
            </div>
        </div>
        <div class="row margin-top-2">
            <div class="col-lg-12">
                <status:Status ID="status" runat="server" />
            </div>
        </div>
        <div class="row margin-top-2">
            <div class="col-lg-4">
                <div role="form">
                    <div class="form-group">
                        <label for="txtName">Naziv:</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCode">Kod:</label>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtDateFrom">Od:</label>
                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtDateTo">Do:</label>
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="cmbCouponType">Tip:</label>
                        <asp:DropDownList ID="cmbCouponType" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtDiscount">Iznos popusta:</label>
                        <div class="input-group input-append">
                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon"><span id="couponTypeUnit"></span></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row margin-top-2">
            <div class="col-lg-6">
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form" class="form-horizontal">
                            <div class="form-group">
                                <label for="cmbCategory" class="col-lg-3 control-label">Kategorija:</label>
                                <div class="col-lg-9">
                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form" class="form-horizontal">
                            <div class="form-group">
                                <label for="cmbBrand" class="col-lg-3 control-label">Proizvođač:</label>
                                <div class="col-lg-9">
                                    <asp:DropDownList ID="cmbBrand" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form" class="form-horizontal">
                            <div class="form-group">
                                <label for="cmbProduct" class="col-lg-3 control-label">Proizvod:</label>
                                <div class="col-lg-2">
                                    <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-7">
                                    <asp:DropDownList ID="cmbProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-1">
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form">
                            <div class="form-group">
                                <asp:Button ID="btnAddCategoryToObjects" runat="server" OnClick="btnAddCategoryToObjects_Click" Text=">" CssClass="btn btn-default" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form">
                            <div class="form-group">
                                <asp:Button ID="btnAddBrandToObjects" runat="server" OnClick="btnAddBrandToObjects_Click" Text=">" CssClass="btn btn-default" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div role="form">
                            <div class="form-group">
                                <asp:Button ID="btnAddProductToObjects" runat="server" OnClick="btnAddProductToObjects_Click" Text=">" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
            <div class="col-lg-5">
                <asp:ListBox ID="lstObjects" runat="server" CssClass="form-control" SelectionMode="Multiple" Height="150px"></asp:ListBox>
                <asp:Button ID="btnRemoveObject" runat="server" OnClick="btnRemoveObject_Click" Text="Obriši" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooter" runat="server">
    <script>
        $(document).ready(function () {
            $('[id*=cmbCouponType]').change(function () {
                if ($(this).val() == '1')
                    $('#couponTypeUnit')[0].innerText = '%';
                else if(($this).val() == '2')
                    $('#couponTypeUnit')[0].innerText = 'din';
            })

            if ($('[id*=cmbCouponType]').val() == '1')
                $('#couponTypeUnit')[0].innerText = '%';
            else if($('[id*=cmbCouponType]').val() == '2')
                $('#couponTypeUnit')[0].innerText = 'din';

            $('[id*=txtProductID]').blur(function () {
                if ($('[id*=txtProductID]').val() != '')
                    $('[id*=cmbProduct]').val($('[id*=txtProductID]').val());
            })
        })
    </script>
</asp:Content>
