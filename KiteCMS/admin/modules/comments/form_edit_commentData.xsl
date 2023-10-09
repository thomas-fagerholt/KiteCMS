<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="rootpageid" select="-1" />

<xsl:template match="/">
<head>
<link REL="stylesheet" TYPE="text/css" HREF="/admin/default.css"/>
</head>

<script type="text/javascript">
function funcsubmit(){
  if(document.formedit.adminemail.value==''){
  alert('<xsl:value-of select="util:localText('adminnotempty')"/>');
  document.formedit.adminemail.focus();
  return false;
  }
  if(document.formedit.thankyoutext.value==''){
  alert('<xsl:value-of select="util:localText('thankyoutextnotempty')"/>');
  document.formedit.thankyoutext.focus();
  return false;
  }
  document.formedit.submit();
  }
</script>

<form action="default.aspx" method="post" name="formedit" onsubmit="return funcsubmit();">
<input type="hidden" name="action" value="doeditcommentdata"/>
<input type="hidden" name="pageid" value="{//selectedpage}"/>

<table border="0" width="450">
<tr>
	<td width="150"><xsl:value-of select="util:localText('adminemail')"/>:</td>
	<td width="300"><input type="text" value="{//comments[@rootpageid=$rootpageid]/adminemail}" name="adminemail" class="alminput" size="50" maxlength="250"/></td>
</tr>
<tr>
	<td colspan="2"><br/><b><xsl:value-of select="util:localText('basictextedit')"/></b></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('thankyoutext')"/>:</td>
	<td><input type="text" name="thankyoutext" class="alminput" size="50" maxlength="250" value="{//comments[@rootpageid=$rootpageid]/thankyoutext}"/></td>
</tr>
<tr>
	<td></td>
	<td>
	<input type="button" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();return false;"/>&#160;
	<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
	</td>
</tr>

</table>



</form>
</xsl:template>

</xsl:stylesheet>