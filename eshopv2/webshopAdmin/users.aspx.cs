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

namespace webshopAdmin
{
    public partial class users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!Page.IsPostBack)
                {
                    loadUsers();
                }
            }
            else
                Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        private void loadUsers()
        {
            dgvUsers.DataSource = UserBL.GetUsers();
            dgvUsers.DataBind();
        }
        
        protected void dgvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            UserBL.DeleteUser(int.Parse(dgvUsers.DataKeys[e.RowIndex].Values[0].ToString()));
            loadUsers();
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/createUser.aspx");
        }
    }
}
