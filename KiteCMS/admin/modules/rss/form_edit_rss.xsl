<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="pageid" select="-1" />
  <xsl:param name="templateoptions" select="-1" />

  <xsl:template match="/">
<head>
<link REL="stylesheet" TYPE="text/css" HREF="/admin/default.css"/>
</head>

<script type="text/javascript">
	var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
  function funcsubmit(){

  if(document.formedit.title.value == ''){
  alert('<xsl:value-of select="util:localText('titlenotempty')"/>');
  document.formedit.title.focus();
  return false;
  }
  if(document.formedit.description.value == ''){
  alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
  document.formedit.description.focus();
  return false;
  }
  if(document.formedit.source.value == ''){
  alert('<xsl:value-of select="util:localText('fieldmissing')"/> pageid');
  document.formedit.source.focus();
  return false;
  }
  if(document.formedit.templates.selectedIndex == -1){
  alert('<xsl:value-of select="util:localText('fieldmissing')"/> <xsl:value-of select="util:localText('template')"/>');
  document.formedit.templates.focus();
  return false;
  }
  document.formedit.submit();
  }
  function funcChooseLink(){
  if (isIE){
  var arr = showModalDialog("/admin/editor/includes/chooseinternallink.aspx", "", "font-family:Verdana; font-size:10; dialogWidth:30em; dialogHeight:54em;status:no;help:no" );
  if (arr != null){
  document.getElementById('source').value=arr;
  }
  } else {
  winModalWindow = window.open("/admin/editor/includes/chooseinternallink.aspx","ModalChildChild","dependent=yes,width=250,height=400,scrollbars=yes")
  winModalWindow.focus();
  }
  }

  function funcInternalLinkSelected(pageid){
  document.getElementById('source').value=pageid;
  }

</script>

<form action="default.aspx" method="post" name="formedit" onsubmit="return funcsubmit();">
<input type="hidden" name="action" value="doeditrss"/>
<input type="hidden" name="pageid" value="{$pageid}"/>

<table border="0" width="500">
<tr>
	<td width="300"><xsl:value-of select="util:localText('title')"/>:</td>
	<td width="200"><input type="text" name="title" class="alminput" size="50" maxlength="250" value="{//feed[@pageid=$pageid]/title}"/></td>
</tr>
<tr>
	<td width="300" valign="top"><xsl:value-of select="util:localText('rssdescription')"/>:</td>
	<td width="200">
    <textarea name="description" class="alminput" cols="49" style="overflow:auto" rows="5"><xsl:value-of select="//feed[@pageid=$pageid]/description"/></textarea>
  </td>
</tr>
<tr>
	<td width="300"><xsl:value-of select="util:localText('rsssource')"/>:</td>
	<td width="200">
    <input type="text" name="source" id="source" class="alminput" size="10" maxlength="250" value="{//feed[@pageid=$pageid]/source}"/>
    <a onclick="funcChooseLink();" style="cursor:pointer"><img src="/admin/editor/images/chooseLink.gif" width="22" height="23" border="0" align="top"/>
    </a>
  </td>
</tr>
<tr>
	<td width="300"><xsl:value-of select="util:localText('rssdescriptionholder')" disable-output-escaping="yes"/>:</td>
	<td width="200">
    <xsl:element name="input">
      <xsl:attribute name="type">text</xsl:attribute>
      <xsl:attribute name="name">descriptionContentholder</xsl:attribute>
      <xsl:attribute name="class">alminput</xsl:attribute>
      <xsl:attribute name="size">50</xsl:attribute>
      <xsl:attribute name="maxlength">250</xsl:attribute>
      <xsl:attribute name="value">
        <xsl:if test="//feed[@pageid=$pageid]/description_contentholder=''">metadescription</xsl:if>
        <xsl:if test="//feed[@pageid=$pageid]/description_contentholder!=''">
          <xsl:value-of select="//feed[@pageid=$pageid]/description_contentholder"/>
        </xsl:if>
      </xsl:attribute>
    </xsl:element>
  </td>
</tr>
  <tr>
    <td width="300" valign="top">
      <xsl:value-of select="util:localText('rsschoosetemplate')"/>:
    </td>
    <td width="200">
      <select name="templates" class="alminput" size="10" multiple="true">
        <xsl:value-of select="$templateoptions" disable-output-escaping="yes"/>
      </select>
    </td>
  </tr>
  <tr>
    <td width="300" valign="top">
      <xsl:value-of select="util:localText('rssinherit')"/>:
    </td>
    <td width="200">
      <xsl:element name="input">
        <xsl:attribute name="type">checkbox</xsl:attribute>
        <xsl:attribute name="name">inherit</xsl:attribute>
        <xsl:attribute name="value">true</xsl:attribute>
        <xsl:if test="//feed[@pageid=$pageid]/@inherit = 'True'">
          <xsl:attribute name="checked">true</xsl:attribute>
        </xsl:if>
      </xsl:element>
    </td>
  </tr>
  <tr>
    <td width="300" valign="top">
      <xsl:value-of select="util:localText('rssincluderoot')"/>:
    </td>
    <td width="200">
      <xsl:element name="input">
        <xsl:attribute name="type">checkbox</xsl:attribute>
        <xsl:attribute name="name">includeroot</xsl:attribute>
        <xsl:attribute name="value">true</xsl:attribute>
        <xsl:if test="//feed[@pageid=$pageid]/@includeroot = 'True'">
          <xsl:attribute name="checked">true</xsl:attribute>
        </xsl:if>
      </xsl:element>
    </td>
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