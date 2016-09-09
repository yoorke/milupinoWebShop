using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using eshopBL;
using eshopUtilities;

namespace webshopAdmin
{
    public partial class promotions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("administrator"))
            {
                if (!Page.IsPostBack)
                {
                    loadPromotions();
                }
            }
            else
                Page.Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        private void loadPromotions()
        {
            PromotionBL promotionBL = new PromotionBL();
            dgvPromotions.DataSource = promotionBL.GetPromotions(false, null, null);
            dgvPromotions.DataBind();
        }

        protected void btnAddPromotion_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/promotion.aspx");
        }

        protected void dgvPromotions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                PromotionBL promotionBL = new PromotionBL();
                int status = promotionBL.DeletePromotion(int.Parse(dgvPromotions.DataKeys[e.RowIndex].Values[0].ToString()));

                Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/promotions.aspx");
            }
            catch (BLException ex)
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
