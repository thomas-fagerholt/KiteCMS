<%@ Page CodeBehind="image_view.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="KiteCMS.Admin.image_view" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
		<title>Select Image from list</title>
        <link rel="stylesheet" type="text/css" href="/admin/default.css"/>
	    <script type="text/javascript">
				var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
            function funcImage(){
                if (document.getElementById("picImg")) {
	                var intwidth=document.getElementById("picImg").width;
	                var intheight=document.getElementById("picImg").height;
	                if (intwidth>200){
		                document.getElementById("picImg").width=200;
		                document.getElementById("picImg").height=(200/intwidth)*intheight;
		                }

	                if (document.getElementById("picImg").height>200){
		                document.getElementById("picImg").height=200;
		                document.getElementById("picImg").width=(200/intheight)*intwidth;
		                }
	                document.getElementById("divImg").innerHTML='<% Response.Write(KiteCMS.Functions.localText("height")); %>: '+intheight+'px, <% Response.Write(KiteCMS.Functions.localText("width")); %>: '+intwidth +'px';
	            }
            }

            function funcImageChosen(){
	            if (isIE){
		            var arr = new Array();
		            arr["src"] = '<% Response.Write(strFolder + strFile); %>';
		            arr["alt"] = document.getElementById('alt').value;
		            arr["title"] = document.getElementById('title').value;
		            arr["align"] = document.getElementById('align').value;

		            window.returnValue = arr;
	            } else
		            parent.opener.funcImageSelected('<% Response.Write (strFolder + strFile); %>',document.getElementById('alt').value,document.getElementById('title').value,document.getElementById('align').value);
	            parent.window.close();
            }

            function funcImagePopupChosen(){
	            if (isIE){
		            var arr = new Array();
		            arr["src"] = '<% Response.Write(strFolder + strFile); %>';

		            window.returnValue = arr;
	            } else
		            parent.opener.funcImagePopupSelected('<% Response.Write (strFolder + strFile); %>');
	            parent.window.close();
            }

            function funcImageCancel(){
	            if (isIE){
		            var arr = new Array();
		            arr["src"] = '';
		            window.returnValue = arr;
	            }
	            parent.window.close();
            }
    	</script>
    </head>
    <body onload="funcImage();" class="clsBody">
    <% if (popup=="false") { %>
	<table>
	<tr><td>alt-tag:</td><td><input type="text" name="alt" id="alt" class="alminput"/></td></tr>
	<tr><td>title-tag:</td><td><input type="text" name="title" id="title" class="alminput"/></td></tr>
	<tr><td>alignment:</td><td><select name="align" id="align" class="alminput"><option value="">No alignment</option><option value="left">left</option><option value="right">right</option></select></td></tr>
	</table>
       <% } %>
	<asp:Literal id="content" Runat="server"></asp:Literal>
    </body>
</html>