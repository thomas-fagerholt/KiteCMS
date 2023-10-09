<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

<script language="javascript">
function funcsubmit(){
	if(document.formedit.title.value==''){
		alert('<xsl:value-of select="util:localText('titlenotempty')"/>');
		document.formedit.title.focus();
		return false;
	}
	if(document.formedit.sender.value==''){
		alert('<xsl:value-of select="util:localText('sendernotempty')"/>');
		document.formedit.sender.focus();
		return false;
	}
document.formedit.submit();
}
</script>

<form action="default.aspx" method="post" name="formedit" onsubmit="return funcsubmit();">

<input type="hidden" name="email" value=""/>
<input type="hidden" name="action" value="doaddcategory"/>
<input type="hidden" name="pageid" value="{//selectedpage}"/>

<table border="0" width="450">
<tr>
	<td><xsl:value-of select="util:localText('categorytitle')"/>:</td>
	<td>
		<input type="text" name="title" class="alminput" size="50" maxlength="250" value=""/>
	</td>
</tr>
<tr>
	<td colspan="2"><br/><xsl:value-of select="util:localText('editemailtexts')"/></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('atsubscribe')"/>:</td>
	<td>
		<input type="text" name="onSubscribe" class="alminput" size="50" value=""/>
	</td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('atunsubscribe')"/>:</td>
	<td>
		<input type="text" name="onUnSubscribe" class="alminput" size="50" value=""/>
	</td>
</tr>
<tr>
	<td colspan="2"><br/><xsl:value-of select="util:localText('editnewslettertexts')"/></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('emailheader')"/>:</td>
	<td>
		<input type="text" name="header" class="alminput" size="50" value=""/>
	</td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('emailfooter')"/>:</td>
	<td>
		<input type="text" name="footer" class="alminput" size="50" value=""/>
	</td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('emailsender')"/>:</td>
	<td>
		<input type="text" name="sender" class="alminput" size="50" maxlength="250" value=""/>
	</td>
</tr>
<tr>
<td colspan="2"><h3><xsl:value-of select="util:localText('rssfeed')"/></h3></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('rsstitle')"/>:</td>
	<td>
		<input type="text" name="rsstitle" class="alminput" size="50" maxlength="250" value=""/>
	</td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('rssdescription')"/>:</td>
	<td>
		<input type="text" name="rssdescription" class="alminput" size="50" value=""/>
	</td>
</tr>
<tr>
	<td></td>
	<td>
	<input type="button" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();return false;"/>&#160;
	<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
	</td>
</tr>
<tr>
	<td colspan="2"><br/><xsl:value-of select="util:localText('emailnote')" disable-output-escaping="yes"/></td>
</tr>

</table>



</form>
</xsl:template>

</xsl:stylesheet>