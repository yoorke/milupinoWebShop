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
using System.Collections.Generic;
using eshopBL;
using eshopBE;

namespace eshopv2.user_controls
{
    public partial class ProductImages : System.Web.UI.UserControl
    {
        private List<ProductImage> _images;

        public List<ProductImage> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.IsPostBack)
                //ShowImages();
        }

        public void ShowImages()
        {
            if (_images != null)
            {
                //imgMain.ImageUrl = !_images[0].Contains("no-image.jpg") ? _images[0].Substring(0, _images[0].IndexOf(".jpg")) + "-main.jpg" : _images[0];
                string filename = _images[0].ImageUrl.Substring(0, _images[0].ImageUrl.LastIndexOf('.'));
                string extension = _images[0].ImageUrl.Substring(_images[0].ImageUrl.LastIndexOf('.'));
                string directory = new ProductBL().CreateImageDirectory(int.Parse(filename));
                //imgMain.ImageUrl = !_images[0].Contains("no-image.jpg") ? ("~/" + _images[0].Substring(0, _images[0].IndexOf(".jpg")) + "-large.jpg") : "~/" + _images[0];
                imgMain.ImageUrl = new ProductBL().CreateImageDirectory(int.Parse(filename)) + filename + "-" + ConfigurationManager.AppSettings["mainName"] + extension;
                //lnkImage.NavigateUrl = "~/" + _images[0];
                lnkImage.NavigateUrl = directory + filename + extension;
                pnlThumbs.Controls.Clear();
                if (_images.Count > 1)
                {
                    for (int i = 0; i < _images.Count; i++)
                    {
                        string thumbFilename = _images[i].ImageUrl.Substring(0, _images[i].ImageUrl.LastIndexOf('.'));
                        string thumbExtension = _images[i].ImageUrl.Substring(_images[i].ImageUrl.LastIndexOf('.'));
                        ImageButton image = new ImageButton();
                        image.ID = "imgThumb" + i.ToString();
                        //image.ImageUrl = !_images[i].Contains("no-image.jpg") ? _images[i].Substring(0, _images[i].IndexOf(".jpg")) + "-thumb.jpg" : _images[i];
                        //image.ImageUrl = !_images[i].Contains("no-image.jpg") ? (System.IO.File.Exists(Server.MapPath("~/" + _images[i].Substring(0, _images[i].IndexOf(".jpg")) + "-small.jpg")) ? _images[i].Substring(0, _images[i].IndexOf(".jpg")) + "-small.jpg" : _images[i].Substring(0, _images[i].IndexOf(".jpg")) + "-thumb.jpg") : _images[i];
                        image.ImageUrl = new ProductBL().CreateImageDirectory(int.Parse(thumbFilename)) + thumbFilename + "-" + ConfigurationManager.AppSettings["thumbName"] + thumbExtension;
                        image.CssClass = "thumb img-responsive";
                        //image.Click += new ImageClickEventHandler(image_Click);
                        image.OnClientClick = "changeImg(this.src);return false;";

                        pnlThumbs.Controls.Add(new LiteralControl("<div class='col-lg-3'>"));
                        pnlThumbs.Controls.Add(image);
                        pnlThumbs.Controls.Add(new LiteralControl("</div>"));
                    }
                }
                setImagesForLightBox();
            }
            else
                imgMain.ImageUrl = "/images/no-image.jpg";
        }

        /*protected void image_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton image = (ImageButton)sender;
            imgMain.ImageUrl = image.ImageUrl;
        }*/

        private void setImagesForLightBox()
        {
            List<ProductImage> tempImages = _images;
            tempImages.RemoveAt(0);
            rptImages.DataSource = tempImages;
            rptImages.DataBind();
        }

        protected void rptImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink imageLink = ((HyperLink)e.Item.FindControl("lnkImage"));
                string filename = imageLink.NavigateUrl.Substring(0, imageLink.NavigateUrl.LastIndexOf('.'));
                string extension = imageLink.NavigateUrl.Substring(imageLink.NavigateUrl.LastIndexOf('.'));
                imageLink.NavigateUrl = new ProductBL().CreateImageDirectory(int.Parse(filename)) + filename + extension;
                imageLink.Attributes["data-lightbox"] = "mainImage";
            }
        }
    }
}