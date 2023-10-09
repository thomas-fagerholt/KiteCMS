<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<script type="text/javascript">
function funcsubmit(){
if (document.formadd.login.value==""){
	alert('<xsl:value-of select="util:localText('loginnotempty')"/>');
	document.formadd.login.focus();
	return false;
	}
if (document.formadd.password.value==""){
	alert('<xsl:value-of select="util:localText('passwordnotempty')"/>');
	document.formadd.password.focus();
	return false;
	}
return true;
}
</script>

<form method="post" action="default.aspx" name="formadd" onsubmit="return funcsubmit();">
<input type="hidden" name="pageid" value="{//selectedpage}"/>
<input type="hidden" name="action" value="doadd"/>
<table border="0" width="450">
<tr>
	<td width="100"><xsl:value-of select="util:localText('loginname')"/></td>
	<td width="350"><input name="login" class="alminput"/></td>
</tr>
<tr>
	<td width="100"><xsl:value-of select="util:localText('fullname')"/></td>
	<td width="350"><input name="fullname" class="alminput"/></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('password')"/></td>
	<td><input name="password" type="password" class="alminput"/></td>
</tr>
<tr>
	<td></td>
	<td><input type="submit" value="{util:localText('add')}" class="almknap"/>&#160;<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="location.href='/modules/default.aspx?pageid={//selectedpage}'"/></td>
</tr>
</table>



</form>
</xsl:template>

</xsl:stylesheet>