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
using eshopBE;
using eshopBL;
using eshopUtilities;
using System.Collections.Generic;

namespace webshopAdmin
{
    public partial class category : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("administrator"))
            {
                if (!Page.IsPostBack)
                {
                    loadIntoForm();

                    int categoryID = (Request.QueryString.ToString().Contains("id")) ? int.Parse(Request.QueryString["id"]) : 0;
                    if (categoryID > 0)
                        loadCategory(categoryID);
                    else
                    { 
                        txtSortOrder.Text = (new CategoryBL().GetMaxSortOrder(int.Parse(cmbParent.SelectedValue)) + 1).ToString();
                        imgIcon.ImageUrl = "~/images/no-image.jpg";
                    }
                }
                else
                {
                    Page.Title = (ViewState["pageTitle"] != null) ? ViewState["pageTitle"].ToString() : "Kategorija";
                    lblTabName.Value = Request.Form[lblTabName.UniqueID];
                }

                if(lblCategoryID.Value == string.Empty)
                {
                    cmbExtraMenu.Enabled = false;
                    cmbBrand.Enabled = false;
                    btnAddBrand.Enabled = false;
                }
            }
            else
                Page.Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            saveCategory();
        }

        private void saveCategory()
        {
            try
            {
                Category category = new Category();
                category.Name = txtName.Text;
                category.Url = txtUrl.Text;
                category.ImageUrl = txtImageUrl.Text;
                category.ParentCategoryID = int.Parse(cmbParent.SelectedValue);
                if (lblCategoryID.Value != string.Empty)
                    category.CategoryID = int.Parse(lblCategoryID.Value);
                category.SortOrder = (int.Parse(txtSortOrder.Text));
                category.PricePercent = double.Parse(txtPricePercent.Text);
                category.WebPricePercent = double.Parse(txtWebPricePercent.Text);
                category.ShowOnFirstPage = chkShowOnFirstPage.Checked;
                category.NumberOfProducts = (txtNumber.Text.Length > 0) ? int.Parse(txtNumber.Text) : 0;
                category.firstPageSortOrder = (txtSortOrderFirstPage.Text.Length > 0) ? int.Parse(txtSortOrderFirstPage.Text) : 0;
                category.firstPageOrderBy = cmbCriterion.SelectedItem.Text;
                category.Description = txtDescription.Text;
                category.Active = chkActive.Checked;
                category.Slider = new Slider(int.Parse(cmbSlider.SelectedValue), cmbSlider.SelectedItem.Text, DateTime.Now, DateTime.Now, true);
                category.CategoryBannerID = cmbCategoryBanner.SelectedIndex > -1 ? int.Parse(cmbCategoryBanner.SelectedValue) : -1;
                category.UpdateProductsFromExternalApplication = chkUpdateProductsFromExternalApplication.Checked;
                category.ExportProducts = chkExportProducts.Checked;

                CategoryBL categoryBl = new CategoryBL();
                categoryBl.SaveCategory(category);


            }
            catch (BLException ex)
            {
                setStatus(ex.Message, System.Drawing.Color.Red, "danger");
            }

        }

        private void loadIntoForm()
        {
            CategoryBL categoryBl = new CategoryBL();
            cmbParent.DataSource = categoryBl.GetNestedCategoriesDataTable(true, true);
            cmbParent.DataTextField = "name";
            cmbParent.DataValueField = "categoryID";
            cmbParent.DataBind();

            AttributeBL attributeBL = new AttributeBL();
            cmbAttribute.DataSource = attributeBL.GetAllAttributes();
            cmbAttribute.DataTextField = "name";
            cmbAttribute.DataValueField = "attributeID";
            cmbAttribute.DataBind();

            cmbCriterion.Items.Add("Novi");
            cmbCriterion.Items.Add("Ceni");
            cmbCriterion.Items.Add("Slučajni");

            txtPricePercent.Text = "0,00";
            txtWebPricePercent.Text = "0,00";

            cmbSlider.DataSource = new SliderBL().GetSliders(true);
            cmbSlider.DataTextField = "name";
            cmbSlider.DataValueField = "sliderID";
            cmbSlider.DataBind();

            cmbCategoryBanner.DataSource = new CategoryBannerBL().GetCategoryBanners(true);
            cmbCategoryBanner.DataValueField = "categoryBannerID";
            cmbCategoryBanner.DataTextField = "name";
            cmbCategoryBanner.DataBind();

            cmbExtraMenu.DataSource = new CategoryExtraMenuBL().GetCategoryExtraMenus(true);
            cmbExtraMenu.DataTextField = "name";
            cmbExtraMenu.DataValueField = "categoryExtraMenuID";
            cmbExtraMenu.DataBind();

            cmbBrand.DataSource = new BrandBL().GetBrands(true);
            cmbBrand.DataTextField = "name";
            cmbBrand.DataValueField = "brandID";
            cmbBrand.DataBind();

            chkUpdateProductsFromExternalApplication.Visible = bool.Parse(ConfigurationManager.AppSettings["updateProductsFromExternalApplication"]);
            chkExportProducts.Visible = bool.Parse(ConfigurationManager.AppSettings["exportProducts"]);

            
        }

        private void loadCategory(int categoryID)
        {
            CategoryBL categoryBL = new CategoryBL();
            Category category = categoryBL.GetCategory(categoryID);

            txtName.Text = category.Name;
            txtUrl.Text = category.Url;
            txtImageUrl.Text = category.ImageUrl;
            //cmbParent.SelectedValue = cmbParent.Items.FindByValue(category.ParentCategoryID.ToString()).Value;
            cmbParent.SelectedValue = category.ParentCategoryID.ToString();
            txtSortOrder.Text = category.SortOrder.ToString();
            lblCategoryID.Value = category.CategoryID.ToString();
            txtPricePercent.Text = category.PricePercent.ToString();
            txtWebPricePercent.Text = category.WebPricePercent.ToString();
            chkShowOnFirstPage.Checked = category.ShowOnFirstPage;
            txtNumber.Text = category.NumberOfProducts.ToString();
            txtSortOrderFirstPage.Text = category.firstPageSortOrder.ToString();
            if (category.firstPageOrderBy != string.Empty && category.firstPageOrderBy != null)
                cmbCriterion.SelectedValue = cmbCriterion.Items.FindByText(category.firstPageOrderBy).Value;
            setFirstPageControls(category.ShowOnFirstPage);
            Page.Title = category.Name + " | Admin panel";
            ViewState.Add("pageTitle", Page.Title);
            txtDescription.Text = category.Description;
            chkActive.Checked = category.Active;
            if(category.Slider != null)
            {
                cmbSlider.SelectedValue = category.Slider.SliderID.ToString();
            }
            cmbCategoryBanner.SelectedValue = category.CategoryBannerID != 0 ? category.CategoryBannerID.ToString() : "-1";
            chkUpdateProductsFromExternalApplication.Checked = category.UpdateProductsFromExternalApplication;
            chkExportProducts.Checked = category.ExportProducts;

            imgIcon.ImageUrl = category.ImageUrl != string.Empty ? Server.MapPath("~/images/" + category.ImageUrl) : "~/images/no-image.jpg";


            if (lblCategoryID.Value != string.Empty)
            {
                AttributeBL attributeBL = new AttributeBL();
                dgvAttributes.DataSource = attributeBL.GetAttributesForCategory(categoryID);
                dgvAttributes.DataBind();
            }
        }

        protected void btnAddAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                AttributeBL attributeBL = new AttributeBL();
                attributeBL.SaveAttributeForCategory(int.Parse(lblCategoryID.Value), int.Parse(cmbAttribute.SelectedValue));

                loadAttributes();
            }
            catch (BLException blException)
            {
                setStatus(blException.Message, System.Drawing.Color.Red, "danger");
            }
        }

        protected void dgvAttributes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                AttributeBL attributeBL = new AttributeBL();
                int status = attributeBL.DeleteAttributeForCategory(int.Parse(lblCategoryID.Value), int.Parse(dgvAttributes.DataKeys[e.RowIndex].Values[0].ToString()));

                loadAttributes();
            }
            catch (BLException ex)
            {
                setStatus(ex.Message, System.Drawing.Color.Red, "danger");
            }
        }

        private void loadAttributes()
        {
            AttributeBL attributeBL = new AttributeBL();
            dgvAttributes.DataSource = attributeBL.GetAttributesForCategory(int.Parse(lblCategoryID.Value));
            dgvAttributes.DataBind();
        }

        private void setStatus(string text, System.Drawing.Color foreColor, string classes)
        {
            csStatus.Text = text;
            //csStatus.ForeColor = foreColor;
            csStatus.Class = "alert alert-" + classes + " status";
            csStatus.Show();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            saveCategory();
            Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categories.aspx");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categories.aspx");
        }

        protected void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkBox.NamingContainer;

            AttributeBL attributeBL = new AttributeBL();
            attributeBL.SetFilter(int.Parse(lblCategoryID.Value), int.Parse(((Label)row.FindControl("lblAttributeID")).Text), bool.Parse(((CheckBox)row.FindControl("chkFilter")).Checked.ToString()));
        }

        protected void chkIsDescription_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkBox.NamingContainer;

            AttributeBL attributeBL = new AttributeBL();
            attributeBL.SetIsDescription(int.Parse(lblCategoryID.Value), int.Parse(((Label)row.FindControl("lblAttributeID")).Text), bool.Parse(((CheckBox)row.FindControl("chkIsDescription")).Checked.ToString()));
        }

        protected void chkShowOnFirstPage_CheckedChanged(object sender, EventArgs e)
        {
            setFirstPageControls(chkShowOnFirstPage.Checked);
        }

        private void setFirstPageControls(bool state)
        {
            txtNumber.Enabled = state;
            txtSortOrderFirstPage.Enabled = state;
            cmbCriterion.Enabled = state;
        }

        protected void btnSavePositions_Click(object sender, EventArgs e)
        {
            int categoryID = int.Parse(lblCategoryID.Value);
            AttributeBL attributeBL = new AttributeBL();
            for (int i = 0; i < dgvAttributes.Rows.Count; i++)
            {
                int attributeID = int.Parse(((Label)dgvAttributes.Rows[i].FindControl("lblAttributeID")).Text);
                int position = 0;
                int.TryParse(((TextBox)dgvAttributes.Rows[i].FindControl("txtPosition")).Text, out position);

                attributeBL.SaveAttributePositionForCategory(attributeID, categoryID, position);
            }
        }

        protected void cmbExtraMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadBrandsForCategoryExtraMenuCategory();
        }

        protected void btnAddBrand_Click(object sender, EventArgs e)
        {
            if(cmbExtraMenu.SelectedIndex > 0 && cmbBrand.SelectedIndex > 0)
            {
                new CategoryBL().AddBrandToCategoryExtraMenu(int.Parse(cmbExtraMenu.SelectedValue), int.Parse(cmbBrand.SelectedValue), int.Parse(lblCategoryID.Value));
                loadBrandsForCategoryExtraMenuCategory();
            }
        }

        protected void btnRemoveBrand_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstBrands.Items.Count; i++)
                if (lstBrands.Items[i].Selected)
                    new CategoryBL().DeleteCategoryExtraMenuCategory(int.Parse(cmbExtraMenu.SelectedValue), int.Parse(lblCategoryID.Value), int.Parse(lstBrands.Items[i].Value));
            loadBrandsForCategoryExtraMenuCategory();
        }

        private void loadBrandsForCategoryExtraMenuCategory()
        {
            lstBrands.Items.Clear();
            if(cmbExtraMenu.SelectedIndex > 0)
            {
                List<Brand> brands = new CategoryBL().GetBrandsForCategoryExtraMenu(int.Parse(cmbExtraMenu.SelectedValue), int.Parse(lblCategoryID.Value));
                for (int i = 0; i < brands.Count; i++)
                    lstBrands.Items.Add(new ListItem(brands[i].Name.ToString(), brands[i].BrandID.ToString()));
            }
        }

        protected void cmbParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSortOrder.Text = (new CategoryBL().GetMaxSortOrder(int.Parse(cmbParent.SelectedValue)) + 1).ToString();
        }

        protected void btnImageUpload_Click(object sender, EventArgs e)
        {
            if (fluUpload.HasFile && txtName.Text != string.Empty)
            {
                string extension = fluUpload.FileName.Substring(fluUpload.FileName.LastIndexOf('.'));
                fluUpload.SaveAs(Server.MapPath("~/images/" + txtUrl.Text + extension));
                txtImageUrl.Text = txtUrl.Text + extension;
                imgIcon.ImageUrl = "~/images/" + txtUrl.Text + extension;
            }
            else
                setStatus("Unesite naziv kategorije", System.Drawing.Color.Red, "warning");
        }
    }
}
