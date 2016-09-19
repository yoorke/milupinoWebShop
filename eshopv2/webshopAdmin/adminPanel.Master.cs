using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;
using eshopBL;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Security.Principal;

namespace webshopAdmin
{
    public partial class admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if(Page.User.Identity.IsAuthenticated)
                    loadMenu();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(Page.ResolveUrl("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx"));
        }

        [WebMethod()]
        public static string GetProductsFromKimtec()
        {
            try
            {
                new KimtecBL().SaveProductsFromKimtec();
                Configuration configuration = WebConfigurationManager.OpenWebConfiguration("/");
                //lblProductsLoaded.Text = DateTime.Now.AddHours(9).ToString("dd/MM/yyyy hh:mm");
                configuration.AppSettings.Settings["productsLoaded"].Value = DateTime.Now.AddHours(9).ToString("dd/MM/yyyy hh:mm");
                configuration.Save();

                //divPleaseWait.Style.Add("display", "none");

                return JsonConvert.SerializeObject("Products successfully loaded");
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                throw;
            }
        }

        private void loadMenu()
        {
            rptAdminMenu.DataSource = new AdminMenuBL().GetNestedList();
            rptAdminMenu.DataBind();
        }
    }
}
