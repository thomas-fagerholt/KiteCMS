<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="user" select="-1" />

  <xsl:template match="/">

	<form method="post" action="edit_adminshortcuts.aspx" name="adminshortcuts">
	<input type="hidden" name="action" value="save"/>
	<table border="0" width="500">
	<tr>
    <td></td>
		<td align="center"><b><xsl:value-of select="util:localText('text')"/></b></td>
		<td align="center"><b><xsl:value-of select="util:localText('link')"/></b></td>
	</tr>
	<tr>
    <td><xsl:value-of select="util:localText('link')"/> 1:</td>
		<td><input name="text0" size="20" value="{substring-before(//user[@id=$user]/shortcut[position()=1],'¤')}"  class="alminput" onfocus="this.select();"/></td>
		<td><input name="link0" size="40" value="{substring-after(//user[@id=$user]/shortcut[position()=1],'¤')}" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
    <td><xsl:value-of select="util:localText('link')"/> 2:</td>
		<td><input name="text1" size="20" value="{substring-before(//user[@id=$user]/shortcut[position()=2],'¤')}" class="alminput" onfocus="this.select();"/></td>
		<td><input name="link1" size="40" value="{substring-after(//user[@id=$user]/shortcut[position()=2],'¤')}" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
    <td><xsl:value-of select="util:localText('link')"/> 3:</td>
		<td><input name="text2" size="20" value="{substring-before(//user[@id=$user]/shortcut[position()=3],'¤')}" class="alminput" onfocus="this.select();"/></td>
		<td><input name="link2" size="40" value="{substring-after(//user[@id=$user]/shortcut[position()=3],'¤')}" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
    <td><xsl:value-of select="util:localText('link')"/> 4:</td>
		<td><input name="text3" size="20" value="{substring-before(//user[@id=$user]/shortcut[position()=4],'¤')}" class="alminput" onfocus="this.select();"/></td>
		<td><input name="link3" size="40" value="{substring-after(//user[@id=$user]/shortcut[position()=4],'¤')}" class="alminput" onfocus="this.select();"/></td>
	</tr>
	<tr>
    <td><xsl:value-of select="util:localText('link')"/> 5:</td>
		<td><input name="text4" size="20" value="{substring-before(//user[@id=$user]/shortcut[position()=5],'¤')}" class="alminput" onfocus="this.select();"/></td>
		<td><input name="link4" size="40" value="{substring-after(//user[@id=$user]/shortcut[position()=5],'¤')}" class="alminput" onfocus="this.select();"/></td>
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
