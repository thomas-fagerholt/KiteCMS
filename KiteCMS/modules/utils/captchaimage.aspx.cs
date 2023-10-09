using System;
using System.Data;
using System.Drawing.Imaging;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace KiteCMS.modules.utils
{
    public partial class captchaimage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            int width = 175;
            int height = 50;
            string renderString = "";

            if (HttpContext.Current.Request.QueryString["width"] != null)
                width = int.Parse(HttpContext.Current.Request.QueryString["width"]);
            if (HttpContext.Current.Request.QueryString["height"] != null)
                height = int.Parse(HttpContext.Current.Request.QueryString["height"]);

            if (ConfigurationManager.AppSettings["CaptchaMode"] != null)
            {
                switch (ConfigurationManager.AppSettings["CaptchaMode"].ToString().ToLower())
                {
                    case "math":
                        Random random = new Random();
                        int mathMode = random.Next(0, 2);
                        int sequenceToWrite = random.Next(1, 4);
                        int first = random.Next(1, 10);
                        int second = random.Next(1, 10);
                        int result = 0;
                        if (mathMode == 0)
                        {
                            result = first + second;
                            if (sequenceToWrite == 1)
                            {
                                this.Session["CaptchaImageText"] = first;
                                renderString = "?? + " + second + " = " + result;
                            }
                            else if (sequenceToWrite == 2)
                            {
                                this.Session["CaptchaImageText"] = second;
                                renderString = first + " + ?? = " + result;
                            }
                            else
                            {
                                this.Session["CaptchaImageText"] = result;
                                renderString = first + " + " + second + " = ??";
                            }
                        }
                        else
                        {
                            if (first < second)
                            {
                                int temp = first;
                                first = second;
                                second = temp;
                            }
                            result = first - second;
                            if (sequenceToWrite == 1)
                            {
                                this.Session["CaptchaImageText"] = first;
                                renderString = "?? - " + second + " = " + result;
                            }
                            else if (sequenceToWrite == 2)
                            {
                                this.Session["CaptchaImageText"] = second;
                                renderString = first + " - ?? = " + result;
                            }
                            else
                            {
                                this.Session["CaptchaImageText"] = result;
                                renderString = first + " - " + second + " = ??";
                            }
                        }
                        break;
                    default:
                        renderString = CaptchaImage.GenerateRandomCode();
                        this.Session["CaptchaImageText"] = renderString;
                        break;
                }
            }
            else
            {
                renderString = CaptchaImage.GenerateRandomCode();
                this.Session["CaptchaImageText"] = renderString;
            }

            // Create a CAPTCHA image using the text stored in the Session object.
            CaptchaImage ci = new CaptchaImage(renderString, width, height, "Arial");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
        }
    }
}
