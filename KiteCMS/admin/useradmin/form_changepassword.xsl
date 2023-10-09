<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

  <script type="text/javascript">
	function funcSubmit(){
	if(document.changepassword.newpassword.value==""){
		alert("<xsl:value-of select="util:localText('passwordnotempty')"/>");
		document.changepassword.newpassword.focus();
		return false;
	}
	if(document.changepassword.newpassword.value!=document.changepassword.newpassword2.value){
		alert("<xsl:value-of select="util:localText('passwordnotsame')"/>");
		document.changepassword.newpassword2.focus();
		return false;
	}
	return true;
	}
	
	</script>

	<form method="post" action="changepassword.aspx" name="changepassword" onsubmit="return funcSubmit();">
	<input type="hidden" name="action" value="save"/>
	<table border="0">
	<tr>
		<td align="right"><xsl:value-of select="util:localText('newpassword')"/>:</td>
		<td><input type="password" name="newpassword" size="20" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
		<td align="right"><xsl:value-of select="util:localText('newpassword2')"/>:</td>
		<td><input type="password" name="newpassword2" size="20" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="submit" value="{util:localText('save')}" class="almknap"/>&#160;
			<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>&#160;</td>
	</tr>
	</table>
	</form>

</xsl:template>
</xsl:stylesheet>
