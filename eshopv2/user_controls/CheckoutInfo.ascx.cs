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
using eshopBL;
using eshopBE;

namespace eshopv2.user_controls
{
    public partial class CheckoutInfo : System.Web.UI.UserControl
    {
        private DataTable _cart;

        public DataTable CartItems
        {
            set
            {
                _cart = value;
                setValues();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void setValues()
        {
            double cartTotal=0;
            double taxBase = 0;
            double tax = 0;
            double discount = 0;
            double delivery = 0;
            double total = 0;
            double userDiscount = 0;
            CouponType discountType = new CouponType(1, "Procentualni");
            double totalForDiscount = 0;

            if(Page.User.Identity.IsAuthenticated)
            {
                eshopBE.User user = UserBL.GetUser(-1, Page.User.Identity.Name);
                userDiscount = user.Discount;
                discountType = new CouponBL().GetCouponType(user.DiscountTypeID);
            }

            for (int i = 0; i < _cart.Rows.Count; i++)
            {
                cartTotal += double.Parse(_cart.Rows[i]["productPrice"].ToString()) * double.Parse(_cart.Rows[i]["quantity"].ToString());
                discount += double.Parse(_cart.Rows[i]["productPrice"].ToString()) * double.Parse(_cart.Rows[i]["quantity"].ToString()) - double.Parse(_cart.Rows[i]["total"].ToString());
                totalForDiscount += !bool.Parse(ConfigurationManager.AppSettings["userDiscountOnlyOnProductNotOnPromotion"]) || double.Parse(_cart.Rows[i]["productPrice"].ToString()) == double.Parse(_cart.Rows[i]["userPrice"].ToString()) ? double.Parse(_cart.Rows[i]["productPrice"].ToString()) * double.Parse(_cart.Rows[i]["quantity"].ToString()) : 0;
            }
            lblNumberOfProducts.Text = _cart.Rows.Count.ToString();


            double userDiscountValue = (discountType.CouponTypeID == 1) ? (totalForDiscount) * (userDiscount / 100) : userDiscount;

            double totalWithDiscount = cartTotal - discount - userDiscountValue;

            taxBase = (cartTotal - discount - userDiscountValue) / 1.2;
            tax = cartTotal - discount - userDiscountValue - taxBase;

            

            delivery = (cartTotal - discount - userDiscountValue> double.Parse(ConfigurationManager.AppSettings["freeDeliveryTotalValue"])) ? 0 : double.Parse(ConfigurationManager.AppSettings["deliveryCost"]);

            

            total = cartTotal - discount + delivery - userDiscountValue;

            

            lblTaxBase.Text = string.Format("{0:N2}", taxBase);
            lblPdv.Text = string.Format("{0:N2}", tax);
            lblTotalWithTax.Text = string.Format("{0:N2}", cartTotal);
            double totalDiscount = discount + userDiscountValue;
            lblDiscount.Text = string.Format("{0:N2}", (0 - totalDiscount));
            lblTotalWithDiscount.Text = string.Format("{0:N2}", totalWithDiscount);
            //lblDiscount.Text = "42423424";
            lblDelivery.Text = string.Format("{0:N2}", delivery);
            lblTotal.Text = string.Format("{0:N2}", total);
        }

        protected void btnShowCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("/korpa");
        }
    }
}