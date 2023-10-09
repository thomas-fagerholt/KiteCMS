<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

<table cellpadding="0" cellspacing="2" border="0">
<tr>
	<td>
	<table border="0" width="450">
		<tr><td colspan="3"><b><xsl:value-of select="util:localText('chooseboxcategory')"/></b><br/>&#160;</td></tr>
		<xsl:for-each select="/website/boxmodule/boxcategory">
			<tr>
        <td class="adminLinkList" style="border-bottom:dotted 1px #000">
          <a href="/admin/box/editboxcategory.aspx?pageid={//selectedpage}&amp;action=formedit&amp;boxcategoryid={@id}">
					<xsl:value-of select="title"/>
          <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
        </a>
          <a href="editboxcategory.aspx?pageid={//selectedpage}&amp;action=delete&amp;boxcategoryid={@id}">
            <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('delete')}" title="{util:localText('delete')}" align="absmiddle" border="0"/>
          </a>
        </td>
			</tr>
		</xsl:for-each>
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
