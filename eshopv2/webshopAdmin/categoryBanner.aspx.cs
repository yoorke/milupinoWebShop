using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eshopBE;
using eshopBL;
using System.Configuration;

namespace webshopAdmin
{
    public partial class categoryBanner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Request.QueryString.ToString().Contains("categoryBannerID"))
                    {
                        int categoryBannerID = int.Parse(Page.Request.QueryString["categoryBannerID"]);
                        loadCategoryBanner(categoryBannerID);
                    }
                    else
                        lblCategoryBannerName.Text = "Unos novog banera kategorije";
                }
            }
            else
                Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            saveCategoryBanner();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            saveCategoryBanner();
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categoryBanners.aspx");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categoryBanners.aspx");
        }

        protected void btnImageUpload_Click(object sender, EventArgs e)
        {
            if (fluImage.HasFile)
            {
                fluImage.SaveAs(Server.MapPath("~/images/" + fluImage.FileName));
                imgCategoryBanner.ImageUrl = "/images/" + fluImage.FileName;
            }
        }

        private void loadCategoryBanner(int categoryBannerID)
        {
            CategoryBanner categoryBanner = new CategoryBannerBL().GetCategoryBanner(categoryBannerID);
            txtCategoryBannerName.Text = categoryBanner.Name;
            txtUrl.Text = categoryBanner.Url;
            imgCategoryBanner.ImageUrl = "/images/" + categoryBanner.ImageUrl;
            lblCategoryBannerName.Text = categoryBanner.Name;
            lblCategoryBannerID.Value = categoryBanner.CategoryBannerID.ToString();
            Page.Title = categoryBanner.Name + " | Admin panel";
        }

        private void saveCategoryBanner()
        {
            try
            {
                CategoryBanner categoryBanner = new CategoryBanner();
                categoryBanner.Name = txtCategoryBannerName.Text;
                categoryBanner.ImageUrl = imgCategoryBanner.ImageUrl.Substring(imgCategoryBanner.ImageUrl.LastIndexOf("/") + 1);
                categoryBanner.Url = txtUrl.Text;
                if (lblCategoryBannerID.Value != string.Empty)
                    categoryBanner.CategoryBannerID = int.Parse(lblCategoryBannerID.Value);

                categoryBanner.CategoryBannerID = new CategoryBannerBL().SaveCategoryBanner(categoryBanner);
                lblCategoryBannerID.Value = categoryBanner.CategoryBannerID.ToString();
            }
            catch (Exception ex)
            {
                setStatus(ex.Message, System.Drawing.Color.Red, true);
            }
        }

        private void setStatus(string text, System.Drawing.Color foreColor, bool visible)
        {
            csStatus.Text = text;
            csStatus.ForeColor = foreColor;
            csStatus.Visible = visible;
            csStatus.Show();
        }
    }
}