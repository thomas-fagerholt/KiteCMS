<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:util="urn:contentFunctions" exclude-result-prefixes="util">
  <xsl:output omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" method="xml" />
  <xsl:variable name="selectedpage">
    <xsl:value-of select="//selectedpage" />
  </xsl:variable>
  <xsl:template match="/">
  <xsl:for-each select="//page[@id=//selectedpage]" xml:space="preserve">
	<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="da" lang="da">
	<head>
		<xsl:element name="title">
			<xsl:if test="parameters/htmltitle !=''"><xsl:value-of select="parameters/htmltitle" /></xsl:if>
			<xsl:if test="parameters/htmltitle ='' or not(parameters/htmltitle)"><xsl:value-of select="menutitle" /></xsl:if>
		</xsl:element>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
		<meta http-equiv="imagetoolbar" content="no" />
		<meta name="generator" content="KiteCMS" />
		<meta name="description" content="{parameters/metadescription}" />
		<meta name="keywords" content="" />
		<link rel="stylesheet" href="/images/default.css" type="text/css" media="all" />
		<meta http-equiv="Pragma" content="no-cache" />
		<meta http-equiv="Cache-Control" content="no-cache" />
		<meta http-equiv="Cache-Control" content="no-store" />
	</head>
			
<body>
    <div id="container" >
	    <div id="header">
		    <h1>DEMO KiteCMS</h1>
		    <h2>This is the online demo for KiteCMS</h2>
	    </div>

		<xsl:call-template name="topmenu" />
	
		<div id="content">
			<h2><xsl:value-of select="menutitle"/></h2>
			<Editor id="html" width="500px" height="400px"/>
			<xsl:comment>modulecontent</xsl:comment>
		</div>
		<div id="subcontent">
			<xsl:call-template name="submenu" />
		</div>

	    <div id="footer">
			<div class="left small">Last updated: 
				<xsl:call-template name="longdate">
					<xsl:with-param name="date">
						<xsl:value-of select="substring-before(//page[@id=//selectedpage]/@updated,' ')"/>
					</xsl:with-param>
				</xsl:call-template>
			</div>
		    <div class="right">Built with <a href="http://www.kitecms.com" target="_blank">KiteCMS</a></div>
		    <br clear="all"/>
	    </div>

    </div>
</body>
</html>
		
</xsl:for-each>
 </xsl:template>

  <xsl:template name="topmenu">
    <!-- menu -->
    <xsl:comment>noindex</xsl:comment>

    <div id="navigation" xmlns="http://www.w3.org/1999/xhtml">
        <ul>
            <xsl:for-each select="/website/page[public = 1]">
                <xsl:variable name="moduleid">
                    <xsl:value-of select="usetemplate" />
                </xsl:variable>
                <xsl:variable name="pageurl">
                    <xsl:choose>
                    <xsl:when test="not(friendlyurl) or friendlyurl=''">
                        <xsl:value-of select="//template[@id=$moduleid]/publicurl" />?pageid=<xsl:value-of select="@id" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="friendlyurl" />
                    </xsl:otherwise>
                    </xsl:choose>
                </xsl:variable>

                <xsl:if test="descendant-or-self::page[@id=$selectedpage]">
                    <li>
                        <a href="{$pageurl}" class="selected"><xsl:value-of select="menutitle" /></a>
                    </li>
                </xsl:if>
                <xsl:if test="(public=1) and not(descendant-or-self::page[@id=$selectedpage])">
                    <li>
                        <a href="{$pageurl}"><xsl:value-of select="menutitle" /></a>
                    </li>
                </xsl:if>
            </xsl:for-each>
        </ul>
    </div>
</xsl:template>

<xsl:template name="submenu">
    <!-- submenu -->
    <xsl:comment>noindex</xsl:comment>

    <xsl:if test="count(/website/page[@id=$selectedpage or .//page/@id=$selectedpage]/page[public=1])&gt;0">
        <h2>Submenu</h2>
        <ul class="menublock">
        <xsl:for-each select="/website/page[@id=$selectedpage or .//page/@id=$selectedpage]/page[public=1]">
            <xsl:variable name="moduleid">
                <xsl:value-of select="usetemplate" />
            </xsl:variable>
            <xsl:variable name="pageurl">
                <xsl:choose>
                <xsl:when test="not(friendlyurl) or friendlyurl=''">
                    <xsl:value-of select="//template[@id=$moduleid]/publicurl" />?pageid=<xsl:value-of select="@id" /></xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="friendlyurl" />
                </xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:if test="descendant-or-self::page[@id=$selectedpage]">
                <li>
                    <a href="{$pageurl}" id="subactive"><xsl:value-of select="menutitle" /></a>
                </li>
            </xsl:if>
            <xsl:if test="(public=1) and not(descendant-or-self::page[@id=$selectedpage])">
                <li>
                    <a href="{$pageurl}"><xsl:value-of select="menutitle" /></a>
                </li>
            </xsl:if>
        </xsl:for-each>
        </ul>
    </xsl:if>
</xsl:template>

<xsl:template name="longdate">
<xsl:param name='date'/>
			<xsl:value-of select="number(substring-after(substring-after($date,'-'),'-'))"/>.
			<xsl:choose>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=1">jan</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=2">feb</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=3">mar</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=4">apr</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=5">may</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=6">jun</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=7">jul</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=8">aug</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=9">sep</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=10">oct</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=11">nov</xsl:when>
			<xsl:when test="substring-before(substring-after($date,'-'),'-')=12">dec</xsl:when>
			</xsl:choose>.&#160;<xsl:value-of select="substring-before($date,'-')"/>
</xsl:template>
</xsl:stylesheet>