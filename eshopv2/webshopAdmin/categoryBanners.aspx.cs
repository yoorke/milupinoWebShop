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
    public partial class categoryBanners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!Page.IsPostBack)
                {
                    loadCategoryBanners();
                }
            }
            else
                Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnAddCategoryBanner_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categoryBanner.aspx");
        }

        protected void dgvCategoryBanners_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        private void loadCategoryBanners()
        {
            dgvCategoryBanners.DataSource = new CategoryBannerBL().GetCategoryBanners(false);
            dgvCategoryBanners.DataBind();
        }
    }
}