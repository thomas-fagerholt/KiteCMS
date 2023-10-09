<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

<table cellpadding="0" cellspacing="2" border="0">
<tr>
	<td>
	<table border="0">
    <tr>
      <td class="adminLinkList">
        <h3>
          <xsl:value-of select="util:localText('chooseuser')"/>
        </h3>
        <xsl:for-each select="//user">

          <a href="/admin/useradmin/edituser.aspx?pageid={//selectedpage}&amp;action=formedit&amp;userid={@id}" title="{util:localText('edit')}">
            <xsl:value-of select="username"/>
            <xsl:if test="fullname!=''">
              (<xsl:value-of select="fullname"/>)
            </xsl:if>
            <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
          </a>
          <xsl:element name="a">
            <xsl:attribute name="href">javascript:void(0);</xsl:attribute>
            <xsl:attribute name="onclick">
              if(confirm('<xsl:value-of select="util:localText('dodeleteuser')"/>?')) {location.href='/admin/useradmin/edituser.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=deleteuser&amp;userid=<xsl:value-of select="@id"/>'}
            </xsl:attribute>
            <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('delete')}" title="{util:localText('delete')}" align="absmiddle" border="0"/>
          </xsl:element>
          <br/>
        </xsl:for-each>
      </td>
    </tr>
    <tr>
			<td colspan="3"><br/>
			<form action="/default.aspx">
				<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
			</form>
			</td>
		</tr>

	</table>
	</td>
</tr>
</table>

</xsl:template>
</xsl:stylesheet>
