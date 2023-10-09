using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using KiteCMS.Admin.core;
using System.Drawing.Drawing2D;

namespace KiteCMS.Admin
{
	public class uploadandedit : System.Web.UI.Page
	{
        public string ratio;
		protected Literal header;
        protected Literal content;
        protected Literal aboveIload;
        protected Literal leftMenu;
        protected DropDownList ddCropMode;
        protected HiddenField hdnW;
        protected HiddenField hdnX;
        protected HiddenField hdnY;
        protected HiddenField hdnH;
        protected HiddenField uploadfolder;
        protected HiddenField hiddenOverwrite;
        protected HiddenField hiddenConfigWidth;
        protected HiddenField hiddenConfigHeight;
        protected Button btnSave;
        protected Button btnCrop;
        protected Button btnSaveServer;
        protected Button btnCancel;
        protected Button btnCancel2;
        protected Button btnCancel3;
        protected Literal ltrMsg;
        protected Label lbCropMode;
        protected Panel pnlCroppedImage;
        protected Panel pnlCrop;
        protected Panel plnFirst;
        protected Panel plnSecond;
        protected FileUpload fuImage;
        protected System.Web.UI.WebControls.Image imgCrop;
        protected System.Web.UI.WebControls.Image imgCropped;
        private int maxFileSizeKB;
		private int pageId;
		private string allowedFilesList;
		private string uploadRootDirectory;
        private bool Embedded = false;
        private bool userCanOverwrite = false;
        private string m_sImageNameUserUpload = "";
        private string m_sImageNameGenerated = "";
        private const string imagePrefix = "__TEMP__";

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			pageId = admin.loadLocalMenuXml();
            userCanOverwrite = admin.userHasAccess(1502, pageId, false);

            btnSave.Text = Functions.localText("uploadandeditcropstart");
            btnCrop.Text = Functions.localText("uploadandeditcrop");
            btnSaveServer.Text = Functions.localText("uploadandeditsave");
            btnCancel.Text = btnCancel2.Text = Functions.localText("cancel");
            btnCancel.OnClientClick = btnCancel2.OnClientClick = btnCancel3.OnClientClick = "location.href='/default.aspx?pageid=" + pageId + "';return false;";

            // setting configuration of I-load
            if (!Page.IsPostBack)
            {
                funcUploadForm(userCanOverwrite);
                funcCleanupOldPictures();

                if (File.Exists(Global.adminXmlPath + "/iload.xml"))
                {
                    lbCropMode.Text = "<b>"+ Functions.localText("uploadandeditcropmode") + "</b><br/>";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Global.adminXmlPath + "/iload.xml");
                    XmlNodeList sizenodes = doc.SelectNodes("/iload/size");
                    for (int counter = 0; counter < sizenodes.Count; counter++)
                    {
                        ddCropMode.Items.Add(new ListItem(sizenodes[counter].Attributes["label"].Value, sizenodes[counter].Attributes["id"].Value));
                    }
                }
                else
                {
                    // no config file. Default free size
                    ddCropMode.Items.Add(new ListItem("Default", ""));
                }
            }

            // Test if code-infront page is simple page
            if (HttpContext.Current.Request.QueryString["simple"] != null || HttpContext.Current.Request.Form["simple"] != null)
                Embedded = true;
            
            if (Embedded)
                leftMenu.Visible = false;
            else
                leftMenu.Visible = true;

			Page page = new Page(pageId);

            maxFileSizeKB = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxFileSizeKB"]);
			allowedFilesList = System.Configuration.ConfigurationManager.AppSettings["AllowedFilesList"];
            uploadRootDirectory = System.Configuration.ConfigurationManager.AppSettings["uploadRootDirectory"];

			// Check for permissions to this module
			admin.userHasAccess(1505,page.Pageid);
            header.Text = Functions.localText("uploadandedit");

		}

        private void funcCleanupOldPictures()
        {
            string sImagePathImages = Global.publicXmlPath;
            if (System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"] != null)
                sImagePathImages = System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"];

            foreach (string filepath in Directory.GetFiles(sImagePathImages))
            {
                if (filepath.IndexOf("\\" + imagePrefix)>-1)
                {
                    if (DateTime.Now.AddDays(-1) > File.GetCreationTime(filepath))
                    {
                        File.Delete(filepath);
                    }
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            plnFirst.Visible = false;

            string sImageName = "";
            string sImagePathImages = Global.publicXmlPath;
            if (System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"] != null)
                sImagePathImages = System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"];

            if (fuImage.HasFile)
            {
                uploadfolder.Value = HttpContext.Current.Request.Form["folder"].Replace(":", @"/").Replace("..","");
                hiddenOverwrite.Value = (HttpContext.Current.Request.Form["overwrite"] != null && HttpContext.Current.Request.Form["overwrite"].ToLower() == "true").ToString();

                //Save image to images folder
                sImageName = Guid.NewGuid().ToString().Substring(0, 8);
                string sFileExt = Path.GetExtension(fuImage.FileName);
                m_sImageNameUserUpload = imagePrefix + sImageName + sFileExt;
                m_sImageNameGenerated = Path.Combine(sImagePathImages, m_sImageNameUserUpload);
                fuImage.PostedFile.SaveAs(m_sImageNameGenerated);
                var imageStream = fuImage.PostedFile.InputStream;
                
                var memoryStream = new MemoryStream();
                imageStream.CopyTo(memoryStream);

                // Set crop size
                if (ddCropMode.SelectedValue != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Global.adminXmlPath + "/iload.xml");
                    XmlNode sizenode = doc.SelectSingleNode("/iload/size[@id='"+ ddCropMode.SelectedValue +"']");
                    if (sizenode.Attributes["fixed"] != null && sizenode.Attributes["fixed"].Value.ToLower() == "true")
                    {
                        if (sizenode.Attributes["width"] != null && sizenode.Attributes["height"] != null)
                        {
                            string width = sizenode.Attributes["width"].Value;
                            string height = sizenode.Attributes["height"].Value;
                            ratio = ",aspectRatio: " + (double.Parse(width) / double.Parse(height)).ToString().Replace(",", ".");
                            hiddenConfigWidth.Value = width;
                            hiddenConfigHeight.Value = height;
                        }
                    }
                    else
                    {
                        hdnW.Visible = hdnH.Visible = true;
                    }
                }

                pnlCrop.Visible = true;
                string myPicString = Convert.ToBase64String(memoryStream.ToArray());
                imgCrop.Attributes["src"] = "data:image/gif;base64," + myPicString;

                Session["tempImageName"] = m_sImageNameGenerated;
                Session["ImageName"] = fuImage.FileName;
            }
            else
            {
                ltrMsg.Text = "<font color='red'><b>Please select an image to crop.</b></font>";
            }
        }

        protected void btnCrop_Click(object sender, EventArgs e)
        {
            if (hdnW.Value != "")
            {
                string uploadFolder = uploadfolder.Value.Trim('/') + "/";

                m_sImageNameUserUpload = Session["tempImageName"].ToString();
                // Get Width, Height, X & Y coordinates from hidden field which gets value when you select an area to crop.
                int iWidth = Convert.ToInt32(hdnW.Value);
                int iHeight = Convert.ToInt32(hdnH.Value);
                int iXCoord = Convert.ToInt32(hdnX.Value);
                int iYCoord = Convert.ToInt32(hdnY.Value);
                //Call CropImage method defined below
                byte[] byt_CropImage = CropImage(m_sImageNameUserUpload, iWidth, iHeight, iXCoord, iYCoord);

                //resize to configuration size
                byt_CropImage = resizeImage(byt_CropImage);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byt_CropImage))
                {
                    string myPicString = Convert.ToBase64String(ms.ToArray());
                    imgCropped.Attributes["src"] = "data:image/gif;base64," + myPicString;
                    imgCropped.Attributes["onload"] = "resize(this, 800, 600);";
                }

                pnlCrop.Visible = false;
                pnlCroppedImage.Visible = true;
            }
            else
            {
                ltrMsg.Text = "<font color='red'><b>Please select area to crop.</b></font>";
            }
        }

        protected void btnSaveServer_Click(object sender, EventArgs e)
        {
            if (hdnW.Value != "")
            {
                try
                {
                    bool overwrite = false;
                    bool ExtensionAllowed = false;

                    uploadRootDirectory = "/" + uploadRootDirectory.Trim('/') + "/";
                    string uploadFolder = uploadfolder.Value.Replace(":", @"/").Trim('/');
                    if (uploadFolder != "")
                        uploadFolder += "/";
                    if (userCanOverwrite && hiddenOverwrite.Value.ToLower() == "true")
                        overwrite = true;

                    string sImagePathImages = Global.publicXmlPath;
                    if (System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"] != null)
                        sImagePathImages = System.Configuration.ConfigurationManager.AppSettings["uploadTempDirectory"];
                    sImagePathImages = sImagePathImages.TrimEnd('\\') + "\\";
                    m_sImageNameUserUpload = Session["tempImageName"].ToString();
                    string sSaveTo = (uploadRootDirectory + uploadFolder) + Session["ImageName"].ToString();

                    // check if file type is permitted
                    string strCurrentFileExtension = Path.GetExtension(sSaveTo);
                    foreach (string extension in allowedFilesList.Split(','))
                        if (strCurrentFileExtension.ToLower() == "." + extension.ToLower())
                            ExtensionAllowed = true;

                    // Get Width, Height, X & Y coordinates from hidden field which gets value when you select an area to crop.
                    int iWidth = Convert.ToInt32(hdnW.Value);
                    int iHeight = Convert.ToInt32(hdnH.Value);
                    int iXCoord = Convert.ToInt32(hdnX.Value);
                    int iYCoord = Convert.ToInt32(hdnY.Value);
                    //Call CropImage method defined below
                    byte[] byt_CropImage = CropImage(m_sImageNameUserUpload, iWidth, iHeight, iXCoord, iYCoord);

                    //resize to configuration size
                    byt_CropImage = resizeImage(byt_CropImage);

                    if (ExtensionAllowed && byt_CropImage.Length <= maxFileSizeKB * 1000 && !(sSaveTo.IndexOf("..") > -1))
                    {
                        if (Directory.Exists(Server.MapPath((uploadRootDirectory + uploadFolder))))
                        {
                            if (File.Exists(Server.MapPath(sSaveTo)) && !overwrite)
                                ltrMsg.Text = ("<span style='color:red'>" + sSaveTo + " " + Functions.localText("fileexists") + Functions.publicUrl() + "</span><br/>");
                            else
                            {
                                using (MemoryStream oMemoryStream = new MemoryStream(byt_CropImage, 0, byt_CropImage.Length))
                                {
                                    oMemoryStream.Write(byt_CropImage, 0, byt_CropImage.Length);
                                    using (System.Drawing.Image oCroppedImage = System.Drawing.Image.FromStream(oMemoryStream, true))
                                    {
                                        oCroppedImage.Save(Server.MapPath(sSaveTo), oCroppedImage.RawFormat);

                                        if (Embedded)
                                            ltrMsg.Text = "<script type=\"text/javascript\">if (navigator.appVersion.indexOf(\"MSIE\") != -1) window.parent.frames(\"select\").location=window.parent.frames(\"select\").location; else window.frames[\"parent\"].frames[\"select\"].location=window.frames[\"parent\"].frames[\"select\"].location;</script>";
                                        else
                                            ltrMsg.Text = "<script type=\"text/javascript\">document.getElementById('Picture1').style.display='none';</script>" +
                                                "<br/>" + String.Format(Functions.localText("uploadmore"), "uploadandedit.aspx?pageid=" + pageId.ToString()) + Functions.publicUrl();
                                    }
                                }
                            }
                        }
                        else
                            ltrMsg.Text = ((uploadRootDirectory + uploadFolder) + Functions.localText("folderNotExists") + "<br/>");
                    }
                    else
                    {
                        ltrMsg.Text = sSaveTo + ": <span style='color:red'>" + Functions.localText("uploadfailed") + "</span><br/>";
                    }

                    pnlCrop.Visible = false;
                    pnlCroppedImage.Visible = false;
                }
                catch (Exception)
                {
                    ltrMsg.Text = ("<span style='color:red'>" + Functions.localText("uploadfailed") + "</span>");
                }

                aboveIload.Text = "";
            }
            else
            {
                ltrMsg.Text = "<font color='red'><b>Please select area to crop.</b></font>";
            }
        }

        static byte[] CropImage(string sImagePath, int iWidth, int iHeight, int iX, int iY)
        {
            try
            {
                using (System.Drawing.Image oOriginalImage = System.Drawing.Image.FromFile(sImagePath))
                {
                    using (System.Drawing.Bitmap oBitmap = new System.Drawing.Bitmap(iWidth, iHeight))
                    {
                        oBitmap.SetResolution(oOriginalImage.HorizontalResolution, oOriginalImage.VerticalResolution);
                        using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(oBitmap))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(oOriginalImage, new System.Drawing.Rectangle(0, 0, iWidth, iHeight), iX, iY, iWidth, iHeight, System.Drawing.GraphicsUnit.Pixel);
                            MemoryStream oMemoryStream = new MemoryStream();
                            oBitmap.Save(oMemoryStream, oOriginalImage.RawFormat);
                            return oMemoryStream.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        private void funcUploadForm(bool userCanOverwrite)
        {
            Functions functions = new Functions();

            XslCompiledTransform xslt = new XslCompiledTransform();

            xslt.Load(HttpContext.Current.Server.MapPath("/admin/upload/form_uploadandedit.xsl"));

            XsltArgumentList xslArg = new XsltArgumentList();

            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("userCanOverwrite", "", userCanOverwrite);

            StringWriter writer = new StringWriter();

            xslt.Transform(Functions.FilesAndFolders(pageId), xslArg, writer);
            aboveIload.Text = writer.ToString().Substring(0, writer.ToString().IndexOf("<!--iload-->"));
            content.Text += writer.ToString().Substring(writer.ToString().IndexOf("<!--iload-->") + 12);
        }


        byte[] resizeImage(byte[] imageToResize)
        {
            int width = 0;
            int height = 0;
            if (int.TryParse(hiddenConfigWidth.Value, out width) && int.TryParse(hiddenConfigHeight.Value, out height) && width > 0 && height > 0)
            {
                // resize image
                using (MemoryStream StartMemoryStream = new MemoryStream(), NewMemoryStream = new MemoryStream())
                {
                    // write the string to the stream  
                    StartMemoryStream.Write(imageToResize, 0, imageToResize.Length);
                    Bitmap resizedImage = new Bitmap(width, height);
                    using (Graphics gfx = Graphics.FromImage(resizedImage))
                    {
                        Bitmap startBitmap = new Bitmap(StartMemoryStream);
                        gfx.DrawImage(startBitmap, new Rectangle(0, 0, width, height), new Rectangle(0, 0, startBitmap.Width, startBitmap.Height), GraphicsUnit.Pixel);
                        resizedImage.Save(NewMemoryStream, startBitmap.RawFormat);
                    }

                    return NewMemoryStream.ToArray();
                }
            }
            return imageToResize;
        }


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
