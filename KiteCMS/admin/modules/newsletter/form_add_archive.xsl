<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<script type="text/javascript">
  function funcsubmit(){
    if(document.getElementById('subject').value==''){
      alert('<xsl:value-of select="util:localText('subjectnotempty')"/>');
      document.tempForm.subject.focus();
      return false;
    }
    if (document.all) {
      if(document.getElementById('tbContent_' + allEditors[0]).innerHTML==''){
        alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
        document.getElementById('tbContent_' + allEditors[0]).focus();
        return false;
    }
  } else {
    if(document.getElementById('tbContent_' + allEditors[0]).contentWindow.document.body.innerHTML==''){
      alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
		  return false;
    }
  }
  MENU_FILE_SAVE_onclick();
	return false;
}
</script>

<xsl:text disable-output-escaping="yes">
<![CDATA[
  <form action="default.aspx" target="_self" method="post" name="tempForm" onsubmit="return funcsubmit();">
]]>
</xsl:text>	

<input type="hidden" name="categoryid" value="{//selectedemailtype}"/>
<input type="hidden" name="email" value=""/>
<input type="hidden" name="action" value="doaddarchive"/>

	<table border="0">
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('subject')"/>:</td>
		<td><input type="text" name="subject" id="subject" class="alminput" size="50" maxlength="250" value="{title}"/></td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('teaser')"/>:</td>
		<td><textarea name="teaser" id="teaser" cols="100" rows="5" class="alminput"><xsl:value-of select="teaser"/></textarea></td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('content')"/>:</td>
		<td>
			<Editor id="indhold" configfile="/admin/data/editornosave.xml" NoFormtag="true" hidden="false" height="300px" />
    </td>
	</tr>
	<tr>
		<td></td>
		<td>
		<input type="submit" class="almknap" value="{util:localText('save')}"/>&#160;
		<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
		</td>
	</tr>
	</table>

</xsl:template>

</xsl:stylesheet>