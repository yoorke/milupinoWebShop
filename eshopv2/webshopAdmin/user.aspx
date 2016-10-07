<%@ Page Language="C#" MasterPageFile="~/webshopAdmin/adminPanel.Master" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="webshopAdmin.user" Title="Untitled Page" %>
<%@ Register Src="customControls/CustomStatus.ascx" TagName="CustomStatus" TagPrefix="YT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Korisnik</h1>
            </div><!--col-->
        </div><!--row-->
        <div class="row">
            <div class="col-lg-12">
                <div class="btn-group">
                    <asp:Button ID="btnSave" runat="server" Text="Sačuvaj" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnSaveClose" runat="server" Text="Sačuvaj i zatvori" OnClick="btnSaveClose_Click" CssClass="btn btn-default" />
                    <asp:Button ID="btnClose" runat="server" Text="Zatvori" OnClick="btnClose_Click" CssClass="btn btn-default" />
                </div><!--btn-group-->
            </div><!--col-->
        </div><!--row-->
        <div class="row margin-top-2">
            <div class="col-lg-12">
                <YT:CustomStatus ID="customStatus" runat="server" />
            </div>
        </div>
        <div class="row margin-top-2">
            <div class="col-lg-5 col-md-5 col-sm-10 col-xs-12">
                <div role="form">
                    <asp:HiddenField ID="lblUserID" runat="server" />
                    <div class="form-group">
                        <label for="txtLastName">Prezime: </label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div><!--form-group-->
                    <div class="form-group">
                        <label for="txtFirstName">Ime: </label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div><!--form-group-->
                    <div class="form-group">
                        <label for="txtUsername">Username: </label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                    </div><!--form-group-->
                    <%--<div class="form-group">
                        <label for="txtPassword">Šifra: </label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
                    </div><!--form-group-->--%>
                    <div class="form-group">
                        <label for="txtEmail">Email: </label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                    </div><!--form-group-->
                    <div class="form-group">
                        <label for="txtPhone">Telefon: </label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtAddress">Adresa: </label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtZip">Ptt: </label>
                        <asp:TextBox ID="txtZip" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCity">Mesto: </label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="cmbUserType">Tip: </label>
                        <asp:DropDownList ID="cmbUserType" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div><!--form-group-->
                    <div id="divDiscount" runat="server">
                        <div class="form-group">
                            <label for="txtDiscount">Popust: </label>
                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="cmbDiscountType">Tip popusta: </label>
                            <asp:DropDownList ID="cmbDiscountType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div><!--form-->
            </div><!--col-->
        </div><!--row-->
    </div><!--page-wrapper-->
    <%--<div id="topMenu">
        
    </div>
    
    <div id="mainContent">
    
        
    
    
    
    
    
    
    
    
    
    
    
    
    
        <p class="row">
            
        </p>
        
        <p class="row">
            
        </p>
        
        <p class="row">
            
        </p>
        
        <p class="row">
            
        </p>
        
        <p class="row">
            
        </p>
        
        <p class="row">
            
        </p>
    </div>--%>
</asp:Content>
