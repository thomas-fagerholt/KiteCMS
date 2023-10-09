<%@ Page language="c#" Codebehind="listfiles.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.listfiles" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
	    <title>List file</title>
		<link href="/admin/default.css" type="text/css" rel="stylesheet"/>
		<link href="/admin/adminpages.css" type="text/css" rel="stylesheet"/>
	</head>
	<body>
    <div id="KiteCMSAdminBarInner"></div>
		<table cellspacing="0" cellpadding="0" border="0">
			<tr>
				<td class="leftMenu" valign="top" width="150"><asp:Literal id="menu" Runat="server"></asp:Literal></td>
				<td valign="top">
					<h2><asp:literal id="header" Runat="server"></asp:literal></h2>
					<asp:literal id="content" Runat="server"></asp:literal></td>
				<td width="50"></td>
			</tr>
		</table>
	</body>
</html>
