<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="categoryid" select="-1" />
<xsl:param name="dateShowNew" select="-1" />
<xsl:param name="pageid" select="-1" />

<xsl:template match="/">

	<xsl:for-each select="//category[@pageid=$pageid]">
		<h3><xsl:value-of select="title"/></h3>
    <p class="adminLinkList">
    <xsl:for-each select=".//item">
			<xsl:for-each select="ancestor::item">
				<img src="/images/space.gif" width="15" height="9"/>
			</xsl:for-each>
			<a href="default.aspx?action=formactivedebate&amp;pageid={$pageid}&amp;debateid={@id}">
        <xsl:value-of select="header"/>
			  &#160;<span class="debateDate"><xsl:value-of select="substring-before(@date,' ')"/><xsl:call-template name="newdebate" /></span>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
      </a>
	  <a href="javascript:void(0);" onclick="if(confirm('{util:localText('confirmdeletedebate')}')) {{location.href='default.aspx?action=dodeletedebate&amp;pageid={$pageid}&amp;debateid={@id}';}}">
          <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('delete')}" title="{util:localText('delete')}" align="absmiddle" border="0"/>
      </a>
      <br/>
		</xsl:for-each>
    </p>
    <br/>
    <form action="/default.aspx">
      <input type="button" value="{util:localText('back')}" class="almknap" onclick="history.go(-1)"/>
    </form>
  </xsl:for-each>
</xsl:template>

<xsl:template name="newdebate">
	<xsl:if test="(substring-before(@date,'-') &gt; substring-before($dateShowNew,'-') and substring-after(substring-before(@date,' '),'-') = substring-after(substring-before($dateShowNew,' '),'-')) or (substring-before(substring-after(substring-before(@date,' '),'-'),'-')+12*substring-after(substring-after(substring-before(@date,' '),'-'),'-') &gt; substring-before(substring-after(substring-before($dateShowNew,' '),'-'),'-')+12*substring-after(substring-after(substring-before($dateShowNew,' '),'-'),'-'))">
    <span style='color:darkred'><sup><xsl:value-of select="util:localText('new')"/></sup></span>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>