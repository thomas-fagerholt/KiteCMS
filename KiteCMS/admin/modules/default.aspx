<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.moduleDefault" validateRequest="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
	    <title>KiteCMS</title>
		<link href="/admin/default.css" type="text/css" rel="stylesheet"/>
		<link href="/admin/adminpages.css" type="text/css" rel="stylesheet"/>
	</head>
	<body>
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
		<tr>
    		<asp:literal id="toplogobar" Runat="server"></asp:literal>
				<td valign="top">
					<h2><asp:literal id="header" Runat="server"></asp:literal></h2>
					<asp:literal id="content" Runat="server"></asp:literal></td>
			</tr>
		</table>
		<asp:literal id="adminFooter" Runat="server"></asp:literal>
	</body>
</html>
