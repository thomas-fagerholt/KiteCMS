<%@ Page language="c#" Codebehind="upload.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.upload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
	    <title>Upload file</title>
		<link href="/admin/default.css" type="text/css" rel="stylesheet"/>
		<link href="/admin/adminpages.css" type="text/css" rel="stylesheet"/>
	</head>
	<body>
    <div id="KiteCMSAdminBarInner"></div>
		<table cellspacing="0" cellpadding="0" width="750" border="0">
			<tr>
			<asp:literal id="leftMenu" Runat="server"><td class="leftMenu" valign="top" width="150"></td></asp:literal>
				<td valign="top" width="450">
					<h2><asp:literal id="header" Runat="server"></asp:literal></h2>
					<asp:literal id="content" Runat="server"></asp:literal></td>
			</tr>
		</table>
	</body>
</html>
