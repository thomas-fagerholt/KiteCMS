<%@ Page language="c#" Codebehind="login.aspx.cs" AutoEventWireup="false" Inherits="KiteCMS.Admin.login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
	    <title>Login - KiteCMS</title>
		<link href="/admin/default.css" type="text/css" rel="stylesheet"/>
		<link href="/admin/adminpages.css" type="text/css" rel="stylesheet"/>
	<script language="javascript" type="text/javascript">
    window.onload = function() {
    document.getElementById('LoginBoxNoScript').style.display = 'none';
    document.getElementById('LoginBoxOuter').style.display = 'block';
    document.getElementById('username').focus();
    }
    </script>
	</head>
	<body class="KiteCMSLoginPage">
	<form method="post" action="login.aspx" name="login" onsubmit="return funcSubmit();">
	    <div id="LoginBoxNoScript" onload="this.style.display:none;">You need to have javascript enabled to use KiteCMS</div>
	    <div class="LoginBoxOuter" id="LoginBoxOuter" style="display:none;">
        <div class="LoginBox">
            <asp:literal id="content" Runat="server"></asp:literal>
        </div>
        <img src="/admin/images/login_shadow.gif" width="299px" height="148px" alt=""/>
        </div>
	</form>
	</body>
</html>
