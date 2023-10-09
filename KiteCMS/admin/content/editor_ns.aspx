<%@ Page language="c#" Codebehind="editor_ns.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.editor_ns" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
<head>
<link REL='stylesheet' TYPE='text/css' HREF='/images/default.css'/>
<link REL='stylesheet' TYPE='text/css' HREF='/admin/editor/includes/toolbars.css'/>
<BASE href="<% Response.Write (HttpContext.Current.Request.ServerVariables["server_name"]); %>">
<head>
<body id="nsbody_<%= contentid %>" class="nsBody" bgcolor="#fff"></body>
</html>