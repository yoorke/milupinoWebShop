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
using eshopBE;

namespace webshopAdmin
{
    public partial class user : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("administrator"))
            {
                if (!Page.IsPostBack)
                {
                    loadIntoForm();

                    int userID = (Request.QueryString.ToString().Contains("userID")) ? int.Parse(Request.QueryString["userID"]) : -1;
                    if (userID > 0)
                        loadUser(userID);
                }
                else
                {
                    Page.Title = ViewState["pageTitle"].ToString();
                }
            }
            else
                Page.Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            saveUser();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            saveUser();
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/users.aspx");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/users.aspx");
        }

        private void loadIntoForm()
        {
            cmbUserType.DataSource = UserBL.GetUserTypes();
            cmbUserType.DataValueField = "userTypeID";
            cmbUserType.DataTextField = "name";
            cmbUserType.DataBind();

            txtDiscount.Text = "0";
            cmbDiscountType.DataSource = new CouponBL().GetCouponTypes(true);
            cmbDiscountType.DataTextField = "name";
            cmbDiscountType.DataValueField = "couponTypeID";
            cmbDiscountType.DataBind();

            if (!bool.Parse(ConfigurationManager.AppSettings["userDiscountVisible"]))
                divDiscount.Visible = false;
        }

        private void loadUser(int userID)
        {
            User user = UserBL.GetUser(userID, string.Empty);

            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtUsername.Text = user.Username;
            //txtPassword.Text = user.Password;
            cmbUserType.SelectedValue = cmbUserType.Items.FindByValue(user.UserType.UserTypeID.ToString()).Value;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            txtAddress.Text = user.Address;
            txtZip.Text = user.Zip;
            txtCity.Text = user.City;
            txtDiscount.Text = user.Discount.ToString();
            cmbDiscountType.SelectedValue = user.DiscountTypeID.ToString();
            Page.Title = user.Username;
            ViewState.Add("pageTitle", user.Username);
            lblUserID.Value = user.UserID.ToString();
        }

        private void saveUser()
        {
            eshopBE.User user = new eshopBE.User();
            user.UserID = int.Parse(lblUserID.Value);
            user.LastName = txtLastName.Text;
            user.FirstName = txtFirstName.Text;
            user.Email = txtEmail.Text;
            user.Phone = txtPhone.Text;
            user.Address = txtAddress.Text;
            user.Zip = txtZip.Text;
            user.City = txtCity.Text;
            user.Discount = double.Parse(txtDiscount.Text);
            user.DiscountTypeID = int.Parse(cmbDiscountType.SelectedValue);

            int status = new UserBL().UpdateUser(user);
            if (status > 0)
                setCustomStatus("Korisnik uspešno sačuvan", "success");
        }

        protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            
        }

        private void setCustomStatus(string text, string classes)
        {
            customStatus.Text = text;
            customStatus.Class = "alert alert-" + classes + " status";
            customStatus.Show();
        }
    }
}
