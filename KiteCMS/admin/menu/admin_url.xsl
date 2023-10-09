<?xml version='1.0'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<xsl:for-each select="//page[@id=//selectedpage]">	
	<xsl:variable name="moduleid">
	<xsl:value-of select="usetemplate"/>
	</xsl:variable>

	<xsl:element name="a">
	<xsl:attribute name="href"><xsl:value-of select="//template[@id=$moduleid]/adminurl"/>?pageid=<xsl:value-of select="//selectedpage"/></xsl:attribute>
	<xsl:value-of select="util:localText('mainpage')"/>
	</xsl:element>

  <script type="text/javascript">
    var strUrl = '<xsl:value-of select="//template[@id=$moduleid]/adminurl"/>?pageid=<xsl:value-of select="//selectedpage"/>';
	</script>

</xsl:for-each>

<xsl:if test="count(//page[@id=//selectedpage])=0">

	<xsl:variable name="moduleid">
	<xsl:value-of select="/website/page/usetemplate"/>
	</xsl:variable>
	<xsl:element name="a">
	<xsl:attribute name="href"><xsl:value-of select="//template[@id=$moduleid]/adminurl"/>?pageid=<xsl:value-of select="//page/@id"/></xsl:attribute>
	<xsl:value-of select="util:localText('mainpage')"/>
	</xsl:element>

  <script type="text/javascript">
    var strUrl = '<xsl:value-of select="//template[@id=$moduleid]/adminurl"/>?pageid=<xsl:value-of select="//page/@id"/>';
	</script>
</xsl:if>

  <script type="text/javascript">
    init();

    function init() {
    if (document.all) {
    document.onkeydown = checkKey;
    }
    else {
    document.addEventListener('keypress', checkKey, true);
    document.onkeypress = checkKey;
    }
    }
    function checkKey(e) {
    if(!document.all)
    if(e.keyCode=="13")
    document.location.href = strUrl;
    if(document.all)
    if(window.event.keyCode==13)
    document.location.href = strUrl;

    }
  </script>

</xsl:template>

</xsl:stylesheet>

