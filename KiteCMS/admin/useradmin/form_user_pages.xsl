<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<xsl:text disable-output-escaping="yes">
<![CDATA[
	<script type="text/javascript">
	<!-- //
  function funcClickPages(menuid){
		for (var counter=0; counter != document.addUser.length; counter++){
			var arrId = document.addUser.elements[counter].id.split("@")[0].split("_");
			for (var counter2=0; counter2 < arrId.length; counter2++)
				if (document.addUser.elements[counter].name.indexOf('menuid')==0 && arrId[counter2] == menuid){
					if (eval("document.addUser.menuid"+menuid+".checked")==true){
						document.addUser.elements[counter].checked = false;
						document.addUser.elements[counter].disabled = true;
					} else {
						document.addUser.elements[counter].disabled = false;
					}
				}
		}
	}
  // -->
	</script>
]]>
</xsl:text>	

	<table cellspacing="0" cellpadding="3" border="0">
		<tr>
			<td><b><u><xsl:value-of select="util:localText('menuitems')"/></u></b></td>
		</tr>
		<tr>
			<td>

	<input type="checkbox" name="menuid0" id=":0" onclick="funcClickPages(0)" value="1"/>
	<b><xsl:value-of select="util:localText('allmenuitems')"/></b><br/>
	<xsl:for-each select="//page">	
		&#160;
		<xsl:for-each select="ancestor::page">
		&#160;
		</xsl:for-each>
		<xsl:element name="input">
			<xsl:attribute name="type">checkbox</xsl:attribute>
			<xsl:attribute name="name">menuid<xsl:value-of select="@id"/></xsl:attribute>
			<xsl:attribute name="id">_<xsl:for-each select="ancestor::page"><xsl:value-of select="@id"/>_</xsl:for-each>:<xsl:value-of select="@id"/></xsl:attribute>
			<xsl:attribute name="onclick">funcClickPages(<xsl:value-of select="@id"/>)</xsl:attribute>
			<xsl:attribute name="value">1</xsl:attribute>
		</xsl:element>

		<xsl:value-of select="menutitle"/>
		<br/>
	</xsl:for-each>
			</td>
		</tr>
	</table>

</xsl:template>

</xsl:stylesheet>

