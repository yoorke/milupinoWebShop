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
    public partial class attribute : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("administrator"))
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Request.QueryString.ToString().Contains("id"))
                    {
                        int attributeID = int.Parse(Page.Request.QueryString["id"]);
                        loadAttribute(attributeID);
                    }
                    if (Page.Request.QueryString.ToString().Contains("tab"))
                        lblTabName.Value = Page.Request.QueryString["tab"];
                }
            }
            else
                Page.Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/login.aspx?returnUrl=" + Page.Request.RawUrl);
        }

        protected void btnAddValue_Click(object sender, EventArgs e)
        {
            AttributeValue attributeValue = new AttributeValue();
            attributeValue.Value = txtValue.Text;
            attributeValue.AttributeID = int.Parse(lblAttributeID.Value);

            AttributeBL attributeBL = new AttributeBL();
            attributeBL.SaveAttributeValue(attributeValue, false);

            loadValues();
        }

        protected void dgvAttributeValues_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            AttributeBL attributeBL = new AttributeBL();
            attributeBL.DeleteAttributeValue(int.Parse(dgvAttributeValues.DataKeys[e.RowIndex].Values[0].ToString()));

            loadValues();
        }

        private void loadAttribute(int attributeID)
        {
            AttributeBL attributeBL = new AttributeBL();
            eshopBE.Attribute attribute = attributeBL.GetAttribute(attributeID);

            txtName.Text = attribute.Name;
            lblAttributeID.Value = attribute.AttributeID.ToString();
            Page.Title = attribute.Name + " | Admin panel";
            chkIsDescription.Checked = attribute.IsDescription;

            loadValues();

            
        }

        protected void btnSaveAttribute_Click(object sender, EventArgs e)
        {
            eshopBE.Attribute attribute = new eshopBE.Attribute();
            attribute.Name = txtName.Text;
            attribute.IsDescription = chkIsDescription.Checked;
            if (lblAttributeID.Value != string.Empty)
                attribute.AttributeID = int.Parse(lblAttributeID.Value);

            AttributeBL attributeBL = new AttributeBL();
            attributeBL.SaveAttribute(attribute);

            Response.Redirect("/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/attributes.aspx");
        }

        private void loadValues()
        {
            AttributeBL attributeBL = new AttributeBL();
            dgvAttributeValues.DataSource = attributeBL.GetAttributeValues(int.Parse(lblAttributeID.Value));
            dgvAttributeValues.DataBind();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["webshopAdminUrl"] + "/attributes.aspx");
        }

        protected void btnSaveSort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvAttributeValues.Rows.Count; i++)
            {
                new AttributeBL().SaveAttributeValueSort(int.Parse(((Label)dgvAttributeValues.Rows[i].FindControl("lblAttributeValueID")).Text), int.Parse(((TextBox)dgvAttributeValues.Rows[i].FindControl("txtSortIndex")).Text));
            }
        }
    }
}
