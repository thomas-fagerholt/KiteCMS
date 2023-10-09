<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

  <p class="adminLinkList">
    <xsl:for-each select="//user">
      <a href="default.aspx?pageid={//selectedpage}&amp;action=formedit&amp;userid={login}">
        <xsl:value-of select="login"/>
        <xsl:if test="fullname!=''">
          (<xsl:value-of select="fullname"/>)
        </xsl:if>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
      </a>

      <a href="javascript:void(0);" onclick="if(confirm('{util:localText('dodeleteuser')} \'{login}\' ?')) {{location.href='default.aspx?action=dodelete&amp;login={login}&amp;pageid={//selectedpage}';}}">
        <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('delete')}" title="{util:localText('delete')}" align="absmiddle" border="0"/>
      </a>
      <br/>
    </xsl:for-each>
  </p>
<p><input type="button" class="almknap" value="{util:localText('back')}" onclick="location.href='/modules/default.aspx?pageid={//selectedpage}'"/></p>

</xsl:template>

</xsl:stylesheet>