<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

	
	<script language="javascript" type="text/javascript">
    function funcSubmit(){
    if(document.login.username.value==""){
    alert("<xsl:value-of select="util:localText('loginnotempty')"/>");
		document.login.username.focus();
		return false;
	}
	if(document.login.password.value==""){
		alert("<xsl:value-of select="util:localText('passwordnotempty')"/>");
		document.login.password.focus();
		return false;
	}
	return true;
	}
	
	var strLocation = '' + this.location;
	if (strLocation.indexOf("msg=wronglogin")!=-1)
		setTimeout("alert('<xsl:value-of select="util:localText('wronglogin')"/>');",100);
	if (strLocation.indexOf("msg=loginclosed")!=-1)
		setTimeout("alert('<xsl:value-of select="util:localText('loginclosed')"/>');",100);
	</script>

	<input type="hidden" name="action" value="login"/>
    <table border="0" width="270px" cellpadding="0">
	<tr>
		<td colspan="2" align="left" class="space"><img align="absmiddle" src="/admin/images/logo_white.gif"/> Version: <xsl:value-of select="util:Version()"/></td>
	</tr>
	<tr>
		<td align="left"><xsl:value-of select="util:localText('loginname')"/>:</td>
		<td><input type="text" name="username" id="username" size="20" class="alminput"/></td>
	</tr>
	<tr>
		<td align="left"><xsl:value-of select="util:localText('password')"/>:</td>
		<td><input type="password" name="password" id="password" size="20" class="alminput"/></td>
	</tr>
	<tr>
		<td></td>
		<td><input type="submit" value="{util:localText('login')}" class="almknap"/></td>
	</tr>
	<tr>
		<td></td>
		<td>&#160;</td>
	</tr>
	<tr>
		<td colspan="2" align="right">Change language:
        <xsl:if test="util:IsLanguageActive('uk')='true'">
		    <a href="/admin/access/login.aspx?language=uk"><img src="/admin/images/flag_eng.gif" border="0" alt="English" title="English"/></a>&#160;
        </xsl:if>
        <xsl:if test="util:IsLanguageActive('dk')='true'">
            <a href="/admin/access/login.aspx?language=dk"><img src="/admin/images/flag_dk.gif" border="0" alt="Danish" title="Danish"/></a>&#160;
        </xsl:if>
    </td>
	</tr>
	</table>
</xsl:template>
</xsl:stylesheet>
