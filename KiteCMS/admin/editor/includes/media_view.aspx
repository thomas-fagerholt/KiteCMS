<%@ Page CodeBehind="media_view.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="KiteCMS.Admin.media_view" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
		<title>Select Image from list</title>
        <link rel="stylesheet" type="text/css" href="/admin/default.css"/>
	    <script type="text/javascript">
				var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
            function funcMediaChosen(){
                if (isIE){
	              var arr = new Array();
	              arr["href"] = '<% Response.Write(strFolder + strFile); %>';
	              arr["target"] = strTarget.value;
	              arr["titletag"] = titletag.value;
	              window.returnValue = arr;
	              } else
   		            parent.opener.funcLinkSelected('<% Response.Write (strFolder + strFile); %>',document.getElementById('strTarget').value,document.getElementById('titletag').value);
              parent.window.close();

            }

            function funcMediaCancel(){
	              var arr = new Array();
	              arr["href"] = '';
	              arr["target"] = '';
	              arr["titletag"] = '';
	            if (isIE)
		            window.returnValue = arr;
	            parent.window.close();
            }
            </script>
    </head>
    <body class="clsBody">
        <asp:Literal id="content" Runat="server"></asp:Literal>
    </body>
</html>


