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
    public partial class coupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if(!Page.IsPostBack)
                {
                    int couponID = 0;
                    if (Page.Request.QueryString.ToString().Contains("couponID"))
                        int.TryParse(Page.Request.QueryString["couponID"].ToString(), out couponID);

                    if(couponID > 0)                        
                        loadCoupon(couponID);
                    loadIntoForm();
                }
                txtName.Focus();
            }
            else
                Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            save();
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/coupons.aspx");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/coupons.aspx");
        }

        private void save()
        {
            Coupon coupon = new Coupon();
            coupon.Name = txtName.Text;
            coupon.Code = txtCode.Text;
            coupon.DateFrom = DateTime.Parse(txtDateFrom.Text);
            coupon.DateTo = DateTime.Parse(txtDateTo.Text);
            coupon.CouponType = new CouponType(int.Parse(cmbCouponType.SelectedValue), cmbCouponType.SelectedItem.Text);
            coupon.Discount = double.Parse(txtDiscount.Text);
            coupon.Objects = ((List<CouponObject>)ViewState["objects"]);
            coupon.CouponID = ViewState["couponID"] != null ? int.Parse(ViewState["couponID"].ToString()) : -1;

            coupon.CouponID = new CouponBL().Save(coupon);
            ViewState["couponID"] = coupon.CouponID;

            setStatus("Kupon uspešno sačuvan", System.Drawing.Color.Green, true, "success");
        }

        protected void btnAddCategoryToObjects_Click(object sender, EventArgs e)
        {
            addObjectToList(new CouponObject(-1, int.Parse(cmbCategory.SelectedValue), 1, cmbCategory.SelectedItem.Text));
        }

        protected void btnAddBrandToObjects_Click(object sender, EventArgs e)
        {
            addObjectToList(new CouponObject(-1, int.Parse(cmbBrand.SelectedValue), 2, cmbBrand.SelectedItem.Text));
        }

        private void loadCoupon(int couponID)
        {
            Coupon coupon = new CouponBL().GetCoupon(couponID);
            txtName.Text = coupon.Name;
            txtCode.Text = coupon.Code;
            txtDateFrom.Text = coupon.DateFrom.ToString();
            txtDateTo.Text = coupon.DateTo.ToString();
            cmbCouponType.SelectedValue = coupon.CouponType.CouponTypeID.ToString();
            txtDiscount.Text = coupon.Discount.ToString();
            ViewState["objects"] = coupon.Objects;
            ViewState["couponID"] = coupon.CouponID;

            showObjects();
        }

        private void loadIntoForm()
        {
            cmbCouponType.DataSource = new CouponBL().GetCouponTypes(true);
            cmbCouponType.DataTextField = "name";
            cmbCouponType.DataValueField = "couponTypeID";
            cmbCouponType.DataBind();

            cmbCategory.DataSource = new CategoryBL().GetCategories();
            cmbCategory.DataTextField = "name";
            cmbCategory.DataValueField = "categoryID";
            cmbCategory.DataBind();

            cmbBrand.DataSource = new BrandBL().GetBrands(false);
            cmbBrand.DataTextField = "name";
            cmbBrand.DataValueField = "brandID";
            cmbBrand.DataBind();

            cmbProduct.DataSource = new ProductBL().GetAllProducts();
            cmbProduct.DataTextField = "FullName";
            cmbProduct.DataValueField = "productID";
            cmbProduct.DataBind();
        }

        private void addObjectToList(CouponObject couponObject)
        {
            List<CouponObject> objects = ViewState["objects"] != null ? ((List<CouponObject>)ViewState["objects"]) : new List<CouponObject>();
            objects.Add(couponObject);
            ViewState["objects"] = objects;

            showObjects();
        }

        private void showObjects()
        {
            if (ViewState["objects"] != null)
            { 
                lstObjects.DataSource = ((List<CouponObject>)ViewState["objects"]);
                lstObjects.DataTextField = "name";
                lstObjects.DataValueField = "objectTypeID";
                lstObjects.DataBind();
            }
        }

        protected void btnRemoveObject_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < lstObjects.Items.Count; i++)
                if(lstObjects.Items[i].Selected)
                {
                    ((List<CouponObject>)ViewState["objects"]).RemoveAt(i);
                    

                }
            showObjects();
        }

        private void setStatus(string text, System.Drawing.Color foreColor, bool visible, string cssclass)
        {
            status.Text = text;
            status.ForeColor = foreColor;
            status.Visible = visible;
            status.Class = "status " + cssclass;
            status.Show();
            
        }

        protected void btnAddProductToObjects_Click(object sender, EventArgs e)
        {
            addObjectToList(new CouponObject(-1, int.Parse(cmbProduct.SelectedValue), 3, cmbProduct.SelectedItem.Text));
        }
    }
}