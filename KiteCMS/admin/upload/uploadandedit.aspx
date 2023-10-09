<%@ Page language="c#" Codebehind="uploadandedit.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.uploadandedit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
	    <title>Upload and resize file</title>
		<link href="/admin/default.css" type="text/css" rel="stylesheet"/>
		<link href="/admin/adminpages.css" type="text/css" rel="stylesheet"/>

        <link href="jquery.Jcrop.css" rel="stylesheet" type="text/css" />
        <script src="jquery.min.js" type="text/javascript"></script>
        <script src="jquery.Jcrop.js" type="text/javascript"></script>
        <script type="text/javascript">
            jQuery(document).ready(function () {
                jQuery('#<%=imgCrop.ClientID %>').Jcrop({
                    onSelect: storeCoords,
                    //Set Image Box Height & Width
                    boxWidth: 800, boxHeight: 600
                    <%=ratio %>
                });
            });

            //Function to store coordinates
            function storeCoords(c) {
                jQuery('#<%=hdnX.ClientID %>').val(Math.round(c.x));
                jQuery('#<%=hdnY.ClientID %>').val(Math.round(c.y));
                jQuery('#<%=hdnW.ClientID %>').val(Math.round(c.w));
                jQuery('#<%=hdnH.ClientID %>').val(Math.round(c.h));
                <%  if(ratio==null){ %> jQuery('#notification').html("width:" + Math.round(c.w) + "px, height:"+ Math.round(c.h) + "px"); <%} %>
            };

            function resize(obj, w, h) //{{{
            {
                $obj = $(obj);
                var nw = $obj.width(),
          nh = $obj.height();
                if ((nw > w) && w > 0) {
                    nw = w;
                    nh = (w / $obj.width()) * $obj.height();
                }
                if ((nh > h) && h > 0) {
                    nh = h;
                    nw = (h / $obj.height()) * $obj.width();
                }
                xscale = $obj.width() / nw;
                yscale = $obj.height() / nh;
                $obj.width(nw).height(nh);
            }

        </script>
	</head>
	<body>
    <div id="KiteCMSAdminBarInner"></div>

    	<form method="post" runat="server" name="upload" enctype="multipart/form-data" onsubmit="return funcSubmit();">
		<table cellspacing="0" cellpadding="0" width="1000" border="0">
			<tr>
    			<asp:literal id="leftMenu" Runat="server"><td class="leftMenu" valign="top" width="150"></td></asp:literal>
                <td valign="top" width="780" style="height: 60px">
					<h2><asp:literal id="header" Runat="server"></asp:literal></h2>
                <asp:Panel ID="plnFirst" runat="server">
					<asp:literal id="aboveIload" Runat="server"></asp:literal>
                    <div>
                    	<asp:Label id="lbCropMode" Runat="server" AssociatedControlID="ddCropMode"></asp:Label>
                        <asp:DropDownList ID="ddCropMode" runat="server" />
                    </div>
					<asp:literal id="content" Runat="server"></asp:literal>
                    <asp:FileUpload ID="fuImage" runat="server" /><br /><br />
                    <asp:Button ID="btnSave" runat="server" Text="Save Image" CssClass="almknap" onclick="btnSave_Click" />&#160;
                    <asp:Button ID="btnCancel" Text="Cancel" CssClass="almknap" runat="server"/>
                 </asp:Panel>

                 <asp:Panel ID="plnSecond" runat="server">
                  <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <asp:Panel ID="pnlCrop" runat="server" Visible="false">
                      <tr>
                        <td align="left"><b>Please select an area to crop from image below</b><br /><div id="notification">&nbsp;</div></td>
                      </tr>
                      <tr><td align="left" valign="top">
                        <asp:Image ID="imgCrop" runat="server" />
                        <asp:HiddenField ID="hdnX" runat="server" />
                        <asp:HiddenField ID="hdnY" runat="server" />
                        <asp:HiddenField ID="hdnW" runat="server" />
                        <asp:HiddenField ID="hdnH" runat="server" />
                        <asp:HiddenField ID="uploadfolder" runat="server" />
                        <asp:HiddenField ID="hiddenOverwrite" runat="server" />
                        <asp:HiddenField ID="hiddenConfigWidth" runat="server" />
                        <asp:HiddenField ID="hiddenConfigHeight" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnCrop" runat="server" Text="Crop & Save" CssClass="almknap" onclick="btnCrop_Click" />&#160;
                        <asp:Button ID="btnCancel3" Text="Cancel" CssClass="almknap" runat="server"/>
                      </td>
                      </tr>
                      <tr><td> </td></tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlCroppedImage" runat="server" Visible="false">
                      <tr><td colspan="3"><asp:Image ID="imgCropped" runat="server" /></td></tr>
                      <tr><td><br /><br />
                        <asp:Button ID="btnSaveServer" runat="server" Text="Save Image" CssClass="almknap" onclick="btnSaveServer_Click" />&#160;
                        <asp:Button ID="btnCancel2" Text="Cancel" CssClass="almknap" runat="server"/>
                      </td></tr>
                    </asp:Panel>
                    <tr><td colspan="3">
                      <asp:Literal ID="ltrMsg" EnableViewState="false"  runat="server"></asp:Literal>
                    </td></tr>
                  </table>
                </asp:Panel>
                </td>
			</tr>
		</table>
		</form>

	</body>
</html>
