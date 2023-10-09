<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util msxsl">
<xsl:output omit-xml-declaration="yes"/>

<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<xsl:template match="/">

<xsl:variable name="listitem"><xsl:value-of select="/website/calendar/field[@listitem='true']/@id"/></xsl:variable>

	<xsl:variable name="nodeset" select="/website/calendar/category[@pageid=$selectedpage]/event"/> 

	<xsl:variable name="Sorted"> 
		<xsl:for-each select="$nodeset"> 
			<xsl:sort select="substring-before(item[@id='date'],'-')" order="descending" data-type="number"/>
			<xsl:sort select="substring-before(substring-after(item[@id='date'],'-'),'-')" order="descending" data-type="number"/>
			<xsl:sort select="substring-after(substring-after(item[@id='date'],'-'),'-')" order="descending" data-type="number"/>
				<xsl:copy-of select="."/>
		</xsl:for-each> 
	</xsl:variable>

  <p class="adminLinkList adminLinkListNoHeight">
    <xsl:for-each select="msxsl:node-set($Sorted)/event">
      <xsl:if test="position()=1 or substring-before(substring-after(item[@id='date'],'-'),'-')!=substring-before(substring-after(preceding-sibling::event[1]/item[@id='date'],'-'),'-')">
        <b>
          <br/>
          <xsl:call-template name="month">
            <xsl:with-param name="month">
              <xsl:value-of select="substring-before(substring-after(item[@id='date'],'-'),'-')"/>
            </xsl:with-param>
          </xsl:call-template>
          &#160;<xsl:value-of select="substring-before(item[@id='date'],'-')"/>
        </b>
        <br/>
      </xsl:if>

      <xsl:value-of select="item[@id='date']"/>&#160;<a href="default.aspx?pageid={$selectedpage}&amp;id={@id}&amp;action=formeditcalendar">
        <xsl:value-of select="item[@id=$listitem]"/>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
      </a><br/>
    </xsl:for-each>
  </p>
	<p><input type="button" value="{util:localText('back')}" class="almknap" onclick="history.go(-1);"/></p>
</xsl:template>

<xsl:template name="month">
<xsl:param name='month'/>
			<xsl:choose>
			<xsl:when test="$month=1">Januar</xsl:when>
			<xsl:when test="$month=2">Februar</xsl:when>
			<xsl:when test="$month=3">Marts</xsl:when>
			<xsl:when test="$month=4">April</xsl:when>
			<xsl:when test="$month=5">Maj</xsl:when>
			<xsl:when test="$month=6">Juni</xsl:when>
			<xsl:when test="$month=7">Juli</xsl:when>
			<xsl:when test="$month=8">August</xsl:when>
			<xsl:when test="$month=9">September</xsl:when>
			<xsl:when test="$month=10">Oktober</xsl:when>
			<xsl:when test="$month=11">November</xsl:when>
			<xsl:when test="$month='12'">December</xsl:when>
			</xsl:choose>
</xsl:template>

</xsl:stylesheet>