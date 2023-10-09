<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

	<form method="post" action="backup.aspx" name="backup" onsubmit="location.href='/default.aspx?pageid={//selectedpage}'">
	<input type="hidden" name="action" value="dobackup"/>
	<table border="0">
	<tr>
		<td><input type="radio" name="backup" value="datafiles" checked="checked"/></td>
		<td><xsl:value-of select="util:localText('backupdatafiles')"/></td>
	</tr>
	<tr>
		<td><input type="radio" name="backup" value="userfiles"/></td>
		<td><xsl:value-of select="util:localText('backupuserfiles')"/></td>
	</tr>
	<tr>
		<td><input type="radio" name="backup" value="allfiles"/></td>
		<td><xsl:value-of select="util:localText('backupallfiles')"/></td>
	</tr>
	<tr>
		<td colspan="2">
			<input type="submit" value="{util:localText('ok')}" class="almknap"/>&#160;
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
