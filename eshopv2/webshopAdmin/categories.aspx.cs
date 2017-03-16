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

namespace webshopAdmin
{
    public partial class categories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("administrator"))
            {
                if(!Page.IsPostBack)
                    loadCategories();
            }
            else
                Page.Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void dgvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (int.Parse(((Label)e.Row.FindControl("lblParentID")).Text) > int.Parse(((Label)e.Row.FindControl("lblId")).Text))
                //((Label)e.Row.FindControl("lblName")).Text = " -" + ((Label)e.Row.FindControl("lblName")).Text;

                if (int.Parse(((Label)e.Row.FindControl("lblSortOrder")).Text) <= 1)
                    ((LinkButton)e.Row.FindControl("btnSortUp")).Enabled = false;
                else if (int.Parse(((Label)e.Row.FindControl("lblSortOrder")).Text) >= new CategoryBL().GetMaxSortOrder(int.Parse(((Label)e.Row.FindControl("lblParentCategoryID")).Text)))
                    ((LinkButton)e.Row.FindControl("btnSortDown")).Enabled = false;
            }
        }

        protected void dgvCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CategoryBL categoryBL = new CategoryBL();
                int status = categoryBL.DeleteCategory(int.Parse(dgvCategory.DataKeys[e.RowIndex].Values[0].ToString()));

                Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/categories.aspx");
            }
            catch (BLException ex)
            {
                setStatus(ex.Message, System.Drawing.Color.Red, true);
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/category.aspx");
        }

        /*private void products()
        {
            System.Collections.Generic.List<AttributeValue> attributes = new System.Collections.Generic.List<AttributeValue>();
            AttributeValue attributeValue = new AttributeValue();
            attributeValue.AttributeValueID = 9;
            attributeValue.Value = "1 GB";

            attributes.Add(attributeValue);

            attributeValue = new AttributeValue();
            attributeValue.AttributeValueID = 13;
            attributeValue.Value = "i3";

            attributes.Add(attributeValue);

            ProductBL productBL = new ProductBL();
            int count = productBL.GetProducts(attributes).Count;
        }*/

        private void setStatus(string text, System.Drawing.Color foreColor, bool visible)
        {
            csStatus.Text = text;
            csStatus.Visible = visible;
            csStatus.ForeColor = foreColor;
            csStatus.Show();
        }

        protected void btnSortUp_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSortDown_Click(object sender, EventArgs e)
        {

        }

        private void sort(int categoryID, int direction)
        {
            new CategoryBL().ReorderCategory(categoryID, direction);
            loadCategories();
        }

        protected void dgvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandSource is LinkButton)
            { 
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int categoryID = int.Parse(((Label)row.Cells[0].FindControl("lblId")).Text);
                switch (e.CommandName)
                {
                    case "sortUp":
                        {
                            sort(categoryID, -1);
                            break;
                        }
                    case "sortDown":
                        {
                            sort(categoryID, 1);
                            break;
                        }
                }
            }
        }

        private void loadCategories()
        {
            CategoryBL categoryBL = new CategoryBL();
            dgvCategory.DataSource = categoryBL.GetNestedCategoriesDataTable(true);
            dgvCategory.DataBind();
        }
    }
}
