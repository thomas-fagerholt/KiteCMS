<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

  <xsl:output omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <h3><xsl:value-of select="util:localText('choosefile')"/></h3>
    <div class="adminLinkList">
      <xsl:for-each select="/website/file">
        <a href="/admin/template/editxslfile.aspx?pageid={//selectedpage}&amp;action=edit&amp;file={.}&amp;datafolder={@datafolder}">
          <xsl:value-of select="."/>
        </a>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
        <br/>
      </xsl:for-each>
    </div>
    <p><input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/></p>
  </xsl:template>
  </xsl:stylesheet>