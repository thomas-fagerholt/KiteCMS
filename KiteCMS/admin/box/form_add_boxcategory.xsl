<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="boxcategoryid" select="-1" />

<xsl:template match="/">

  <script type="text/javascript">
	function funcSubmit(){
	if(document.editBox.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.editBox.title.focus();
		return false;
		}
	if(document.editBox.htmlstring.value==""){
		alert("<xsl:value-of select="util:localText('contentnotempty')"/>");
		document.editBox.htmlstring.focus();
		return false;
		}

	return true
	}
	</script>
	
	<form method="post" action="addboxcategory.aspx" name="editBox" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="add"/>
	<input type="hidden" name="boxcategoryid" value="{$boxcategoryid}"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>

	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="" size="40"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('htmlstring')"/></td>
		<td><input type="text" class="alminput" name="htmlstring" value="" size="40"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('type')"/></td>
		<td>
			<select class="alminput" name="type">
				<option value="1"><xsl:value-of select="util:localText('insertabove')"/></option>
				<option value="0"><xsl:value-of select="util:localText('replace')"/></option>
				<option value="2"><xsl:value-of select="util:localText('insertbelove')"/></option>
			</select>
		
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

</xsl:template>
</xsl:stylesheet>
