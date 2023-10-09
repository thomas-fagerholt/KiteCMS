<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="user" select="-1" />

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

<body>

<script type="text/javascript">
	var passwordChanged = false;
	function funcSubmit(){
	if(document.editUser.password.value!=document.editUser.password2.value){
		alert("<xsl:value-of select="util:localText('passwordnotequal')"/>");
		document.editUser.password2.focus();
		return false;
		}
	if(document.editUser.password.value.length == 0){
		alert("<xsl:value-of select="util:localText('passwordnotempty')"/>");
		document.editUser.password.focus();
		return false;
		}
	if (passwordChanged == false)	
		document.editUser.password.value ="";
	
	return true;
	}
</script>

	<form method="post" action="default.aspx" name="editUser" onsubmit="return funcSubmit();">
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="action" value="doedituser"/>
	<input type="hidden" name="login" value="{$user}"/>
	
	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('loginname')"/></td>
		<td><b><xsl:value-of select="//user[login=$user]/login"/></b></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('fullname')"/></td>
		<td><input name="fullname" class="alminput" value="{//user[login=$user]/fullname}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('password')"/></td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">password</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
				<xsl:attribute name="name">password</xsl:attribute>
				<xsl:attribute name="value">password</xsl:attribute>
				<xsl:attribute name="onFocus">this.value='';passwordChanged = true;</xsl:attribute>
				<xsl:attribute name="onBlur">if(this.value=='') {this.value='password';passwordChanged = false;}</xsl:attribute>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('repeatpassword')"/>&#160;</td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">password</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
				<xsl:attribute name="name">password2</xsl:attribute>
				<xsl:attribute name="value">password</xsl:attribute>
				<xsl:attribute name="onFocus">this.value='';passwordChanged = true;</xsl:attribute>
				<xsl:attribute name="onBlur">if(this.value=='') {this.value='password';passwordChanged = false;}</xsl:attribute>
			</xsl:element>
		</td>	
	</tr>
	<tr>
		<td></td>
		<td><input type="submit" class="almknap" value="{util:localText('save')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	</table>
</form>

</body>
</xsl:template>
</xsl:stylesheet>
