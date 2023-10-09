<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

  <h4><xsl:value-of select="util:localText('choosecategory')"/></h4>
  <p class="adminLinkList">

    <xsl:for-each select="/website/category[@pageid=//selectedpage]">
      <a href="default.aspx?pageid={//selectedpage}&amp;action=editcategory&amp;categoryid={@id}">
        <xsl:value-of select="title"/>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
      </a>
      <br/>
    </xsl:for-each>
  </p>
  
  <form action="/default.aspx">
    <input type="button" value="{util:localText('back')}" class="almknap" onclick="history.go(-1)"/>
  </form>

</xsl:template>
</xsl:stylesheet>
